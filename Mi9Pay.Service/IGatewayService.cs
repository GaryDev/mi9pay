using ICanPay;
using Mi9Pay.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Service
{
    public interface IGatewayService
    {
        void ValidateRequestParameter(Dictionary<string, string> requestParameter);
        OrderRequest RecieveRequestForm(Dictionary<string, string> requestParameter);

        MemoryStream CreatePaymentQRCode(OrderRequest orderRequest, GatewayType gatewayType);

        IEnumerable<GatewayType> GetGatewayTypes();
        IEnumerable<PaymentMethod> GetPaymentMethods(int storeId);

        OrderPaymentResponse QueryPayment(string appId, string invoiceNumber, GatewayType gatewayType);

        string BuildReturnUrl(OrderRequest request, OrderPaymentResponse response);
    }
}
