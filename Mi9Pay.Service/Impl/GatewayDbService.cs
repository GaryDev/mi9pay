using AutoMapper;
using ICanPay;
using Mi9Pay.DataModel;
using Mi9Pay.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Mi9Pay.Service
{
    public partial class GatewayService
    {
        private GatewayPaymentApp GetGatewayPaymentApp(string appId)
        {
            GatewayPaymentApp app = _repository.App.Get(x => x.Appid == appId);
            if (app == null)
                throw new Exception("无效的appid");

            return app;
        }

        private GatewayPaymentMerchant GetGatewayPaymentMerchant(string merchantCode)
        {
            GatewayPaymentMerchant merchant = _repository.Merchant.Get(x => x.Code == merchantCode);
            if (merchant == null)
                throw new Exception("商家信息获取失败");

            return merchant;
        }

        private IEnumerable<GatewayPaymentMethodTypeJoinResult> GetPaymentMethodCombinations(int storeId, Guid merchant)
        {
            IEnumerable<GatewayPaymentMethodTypeJoinResult> paymentMethodCombinations = _repository.GetPaymentMethodCombinationByStore(storeId, merchant);
            if (paymentMethodCombinations == null || paymentMethodCombinations.ToList().Count == 0)
                throw new Exception(string.Format("未配置支付方式，门店ID({0})", storeId));

            return paymentMethodCombinations;
        }

        private GatewayPaymentMethod GetGatewayPaymentMethodByType(GatewayType gatewayType)
        {
            string payMethodCode = Enum.GetName(typeof(GatewayType), gatewayType);
            GatewayPaymentMethod payMethod = _repository.Method.Get(m => string.Compare(m.Code, payMethodCode, true) == 0);
            if (payMethod == null)
                throw new Exception("支付方式获取失败");

            return payMethod;
        }

        private GatewayPaymentOrderType GetGatewayPaymentOrderType(string orderType)
        {
            GatewayPaymentOrderType type = _repository.OrderType.Get(x => string.Compare(x.Code, orderType, true) == 0);
            if (type == null)
                throw new Exception("订单类型获取失败");

            return type;
        }

        private GatewayPaymentOrderStatus GetGatewayPaymentOrderStatus(PaymentOrderStatus orderStatus)
        {
            string statusCode = Enum.GetName(typeof(PaymentOrderStatus), orderStatus);
            GatewayPaymentOrderStatus status = _repository.OrderStatus.Get(x => string.Compare(x.Code, statusCode, true) == 0);
            if (status == null)
                throw new Exception("订单状态获取失败");

            return status;
        }

        private GatewayPaymentStorePaymentMethod GetGatewayPaymentStorePaymentMethod(int storeId, Guid paymentCombine)
        {
            GatewayPaymentStorePaymentMethod storePaymentMethod = _repository.StorePaymentMethod.Get(x => 
                        x.GatewayPaymentMethodTypeJoin == paymentCombine && x.GatewayPaymentStore1.StoreId == storeId);
            if (storePaymentMethod == null)
                throw new Exception("门店支付方式获取失败");

            return storePaymentMethod;
        }

        private GatewayPaymentAccount GetGatewayPaymentAccount(OrderRequest orderRequest, GatewayType gatewayType, bool ignoreNull = false)
        {
            string payMethodCode = Enum.GetName(typeof(GatewayType), gatewayType);
            GatewayPaymentAccount account = _repository.GetGatewayPaymentAccount(orderRequest.StoreId, orderRequest.Merchant.UniqueId, payMethodCode);
            if (account == null && !ignoreNull)
                throw new Exception("对应支付方式账号获取失败");

            return account;
        }

        private bool PaymentOrderExisted(OrderRequest orderRequest)
        {
            GatewayPaymentOrder order = _repository.GetGatewayPaymentOrder(orderRequest.InvoiceNumber, 
                orderRequest.StoreId, orderRequest.Merchant.UniqueId, Guid.Parse(orderRequest.PaymentCombine));
            if (order != null && order.UniqueId != Guid.Empty)
                return true;

            return false;
        }

        private void CreatePaymentOrder(PaymentOrder paymentOrder)
        {
            try
            {
                DateTime createTime = DateTime.Now;
                GatewayPaymentCustomer customer = null;
                if (paymentOrder.Customer != null)
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<PaymentOrderCustomer, GatewayPaymentCustomer>());
                    customer = Mapper.Map<PaymentOrderCustomer, GatewayPaymentCustomer>(paymentOrder.Customer);
                    if (customer != null)
                    {
                        customer.UniqueId = Guid.NewGuid();
                        customer.TSID = createTime;
                    }
                }
                GatewayPaymentOrder order = new GatewayPaymentOrder
                {
                    UniqueId = Guid.NewGuid(),
                    StoreID = paymentOrder.StoreId,
                    Subject = paymentOrder.Subject,
                    OrderNumber = paymentOrder.InvoiceNumber,
                    TotalAmount = paymentOrder.TotalAmount,
                    Discount = paymentOrder.Discount,
                    Tax = paymentOrder.Tax,
                    ShippingFee = paymentOrder.ShippingFee,
                    OrderType = GetGatewayPaymentOrderType(paymentOrder.OrderType).UniqueId,
                    GatewayPaymentOrderStatus = GetGatewayPaymentOrderStatus(paymentOrder.Status).UniqueId,
                    GatewayPaymentStorePaymentMethod = paymentOrder.StorePaymentMethod,
                    GatewayPaymentMerchant = paymentOrder.Merchant.UniqueId,
                    //GatewayPaymentMethod = GetGatewayPaymentMethodByType(paymentOrder.GatewayType).UniqueId,
                    TSID = createTime
                };
                using (var scope = new TransactionScope())
                {
                    if (customer != null)
                    {
                        _repository.Customer.Insert(customer);
                        order.GatewayPaymentCustomer = customer.UniqueId;
                    }

                    _repository.Order.Insert(order);

                    foreach (var item in paymentOrder.PayItems)
                    {
                        Mapper.Initialize(cfg => cfg.CreateMap<PaymentOrderDetail, GatewayPaymentOrderDetail>());
                        GatewayPaymentOrderDetail orderDetail = Mapper.Map<PaymentOrderDetail, GatewayPaymentOrderDetail>(item);
                        if (orderDetail != null)
                        {
                            orderDetail.UniqueId = Guid.NewGuid();
                            orderDetail.GatewayPaymentOrder = order.UniqueId;
                            orderDetail.TSID = createTime;
                            _repository.OrderDetail.Insert(orderDetail);
                        }
                    }
                    _repository.Save();
                    scope.Complete();
                }
            }
            catch (Exception)
            {
                //throw ex;
            }
        }

        private void UpdatePaymentOrder(PaymentOrder paymentOrder)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    List<GatewayPaymentOrder> orderList = _repository.Order.GetMany(x => 
                        x.OrderNumber == paymentOrder.InvoiceNumber && 
                        x.StoreID == paymentOrder.StoreId && 
                        x.GatewayPaymentMerchant == paymentOrder.Merchant.UniqueId).ToList();
                    if (orderList != null && orderList.Count > 0)
                    {
                        foreach (GatewayPaymentOrder order in orderList)
                        {
                            if (order.GatewayPaymentStorePaymentMethod == paymentOrder.StorePaymentMethod)
                            {
                                order.TradeNumber = paymentOrder.TradeNumber;
                                order.GatewayPaymentOrderStatus = GetGatewayPaymentOrderStatus(paymentOrder.Status).UniqueId;
                            }
                            else
                            {
                                order.GatewayPaymentOrderStatus = GetGatewayPaymentOrderStatus(PaymentOrderStatus.CLOSED).UniqueId;
                            }
                            _repository.Order.Update(order);
                        }
                        _repository.Save();
                        scope.Complete();
                    }
                }
            }
            catch (Exception)
            {
                //throw ex;
            }
        }

        private void ValidatePaymentOrderStatus(string invoiceNumber, string tradeNo)
        {
            GatewayPaymentOrder order = _repository.Order.GetSingle(x => x.OrderNumber == invoiceNumber && x.TradeNumber == tradeNo);
            if (order == null)
                throw new Exception("未查询到相关支付订单");

            if (order.GatewayPaymentOrderStatus1 == null)
                throw new Exception("未查询到相关支付订单状态");

            if (order.GatewayPaymentOrderStatus1.Code != "PAID")
                throw new Exception(string.Format("该支付订单不能进行退款，状态: {0}", order.GatewayPaymentOrderStatus1.Description));
        }

        private void UpdatePaymentOrderStatus(string invoiceNumber, string tradeNo, PaymentOrderStatus status)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    GatewayPaymentOrder order = _repository.Order.GetSingle(x => x.OrderNumber == invoiceNumber && x.TradeNumber == tradeNo);
                    if (order != null)
                    {
                        order.GatewayPaymentOrderStatus = GetGatewayPaymentOrderStatus(status).UniqueId;
                        _repository.Save();
                        scope.Complete();
                    }
                }
            }
            catch (Exception)
            {
                //throw ex;
            }
        }

        private void CreateNotifyQueue(NotifyQueue queue)
        {
            try
            {
                Mapper.Initialize(cfg => cfg.CreateMap<NotifyQueue, GatewayPaymentNotifyQueue>());
                GatewayPaymentNotifyQueue notifyQueue = Mapper.Map<NotifyQueue, GatewayPaymentNotifyQueue>(queue);
                if (notifyQueue != null)
                {
                    using (var scope = new TransactionScope())
                    {
                        _repository.NotifyQueue.Insert(notifyQueue);
                        _repository.Save();
                        scope.Complete();
                    }
                }
            }
            catch (Exception)
            {
                //throw ex;
            }
        }
    }
}
