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

        private IEnumerable<GatewayPaymentMethod> GetGatewayPaymentMethods()
        {
            IEnumerable<GatewayPaymentMethod> paymentMethods = _repository.Method.GetAll();
            if (paymentMethods == null || paymentMethods.ToList().Count == 0)
                throw new Exception("未配置支付方式");

            return paymentMethods;
        }

        private GatewayPaymentStore GetGatewayPaymentStore(int storeId)
        {
            GatewayPaymentStore store = _repository.Store.Get(x => x.StoreId == storeId);
            if (store == null)
                throw new Exception(string.Format("无效的门店ID({0})", storeId));

            return store;
        }

        private IEnumerable<GatewayPaymentStorePaymentMethod> GetStorePaymentMethods(int storeId)
        {
            GatewayPaymentStore store = GetGatewayPaymentStore(storeId);
            IEnumerable<GatewayPaymentStorePaymentMethod> storePaymentMethods = _repository.StorePaymentMethod.GetMany(x => x.GatewayPaymentStore == store.UniqueId);
            if (storePaymentMethods == null || storePaymentMethods.ToList().Count == 0)
                throw new Exception(string.Format("未配置支付方式，门店ID({0})", storeId));

            return storePaymentMethods;
        }

        private GatewayPaymentMethod GetGatewayPaymentMethodByType(GatewayType gatewayType)
        {
            string payCode = Enum.GetName(typeof(GatewayType), gatewayType);
            GatewayPaymentMethod payMethod = GetGatewayPaymentMethods().ToList().FirstOrDefault(m => string.Compare(m.Code, payCode, true) == 0);
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

        private GatewayPaymentAccount GetGatewayPaymentAccount(int storeId, GatewayType gatewayType)
        {
            GatewayPaymentStore store = GetGatewayPaymentStore(storeId);
            GatewayPaymentMethod payMethod = GetGatewayPaymentMethodByType(gatewayType);

            GatewayPaymentMerchant merchant = _repository.Merchant.Get(m => m.UniqueId == store.GatewayPaymentMerchant);
            GatewayPaymentAccount account = _repository.Account.Get(c => c.GatewayPaymentMerchant == merchant.UniqueId && c.GatewayPaymentMethod == payMethod.UniqueId);;          

            if (account == null)
                throw new Exception("对应支付方式账号获取失败");

            return account;
        }

        private bool PaymentOrderExisted(string invoiceNumber, int storeId, GatewayType gatewayType)
        {
            GatewayPaymentMethod payMethod = GetGatewayPaymentMethodByType(gatewayType);
            GatewayPaymentOrder order = _repository.Order.Get(o => o.OrderNumber == invoiceNumber 
                && o.StoreID == storeId 
                && o.GatewayPaymentMethod == payMethod.UniqueId);

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
                    GatewayPaymentMethod = GetGatewayPaymentMethodByType(paymentOrder.GatewayType).UniqueId,
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
                    Guid paymentMethod = GetGatewayPaymentMethodByType(paymentOrder.GatewayType).UniqueId;
                    List<GatewayPaymentOrder> orderList = _repository.Order.GetMany(x => x.OrderNumber == paymentOrder.InvoiceNumber).ToList();
                    if (orderList != null && orderList.Count > 0)
                    {
                        foreach (GatewayPaymentOrder order in orderList)
                        {
                            if (order.GatewayPaymentMethod == paymentMethod)
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
    }
}
