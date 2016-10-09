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
            IEnumerable<GatewayPaymentStore> storeAccounts = _repository.Store.GetMany(s => s.StoreId == storeId);
            if (storeAccounts == null || storeAccounts.ToList().Count == 0)
                throw new Exception(string.Format("支付账号获取失败，门店ID({0})", storeId));

            GatewayPaymentMethod payMethod = GetGatewayPaymentMethodByType(gatewayType);

            GatewayPaymentAccount account = null;
            foreach (var item in storeAccounts.ToList())
            {
                account = _repository.Account.Get(c => c.UniqueId == item.GatewayPaymentAccount 
                                && c.GatewayPaymentMethod == payMethod.UniqueId);
                if (account != null)
                    return account;
            }

            if (account == null)
                throw new Exception("对应支付方式账号获取失败");

            return account;
        }

        private GatewayPaymentAccount GetGatewayPaymentAccount(string appId, GatewayType gatewayType)
        {
            GatewayPaymentApp app = GetGatewayPaymentApp(appId);

            GatewayPaymentMethod payMethod = GetGatewayPaymentMethodByType(gatewayType);

            GatewayPaymentAccount account = app.GatewayPaymentAccount.FirstOrDefault(c => c.GatewayPaymentMethod == payMethod.UniqueId);
            if (account == null)
                throw new Exception("对应支付方式账号获取失败");

            return account;
        }

        private void CreatePaymentOrder(PaymentOrder paymentOrder)
        {
            try
            {
                GatewayPaymentCustomer customer = null;
                if (paymentOrder.Customer != null)
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<PaymentOrderCustomer, GatewayPaymentCustomer>());
                    customer = Mapper.Map<PaymentOrderCustomer, GatewayPaymentCustomer>(paymentOrder.Customer);
                    if (customer != null)
                        customer.UniqueId = Guid.NewGuid();
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
                    GatewayPaymentMethod = GetGatewayPaymentMethodByType(paymentOrder.GatewayType).UniqueId
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
                            orderDetail.GatewayPaymentOrder = order.UniqueId;
                            _repository.OrderDetail.Insert(orderDetail);
                        }
                    }

                    _repository.Save();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
