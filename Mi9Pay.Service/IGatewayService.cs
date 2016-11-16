using Mi9Pay.Entities;
using Mi9Pay.PayProvider;
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
        OrderRequest ValidateRequestParameter(Dictionary<string, string> requestParameter);
        OrderRequest RecieveRequestForm(Dictionary<string, string> requestParameter);

        MemoryStream CreatePaymentQRCode(OrderRequest orderRequest, GatewayType gatewayType);
        OrderPaymentResponse BarcodePayment(OrderRequest orderRequest, GatewayType gatewayType, string barcode);
        OrderPaymentResponse QueryPayment(OrderRequest orderRequest, GatewayType gatewayType);

        IEnumerable<PaymentMethod> GetPaymentMethodList(int storeId, Guid merchant);
        IEnumerable<PaymentScanMode> GetPaymentScanModeList();
        IEnumerable<PaymentCombine> GetPaymentCombineList(int storeId, Guid merchant);

        string BuildReturnUrl(OrderRequest request, OrderPaymentResponse response);
        void PaymentNotify(OrderRequest request, OrderPaymentResponse response);

        int DownloadBill(string merchantCode, string[] storeIdArray, string billDate, GatewayType type);
        void RefundPayment(string merchantCode, string storeId, OrderRefundRequest refundRequest, GatewayType gatewayType);
    }
}
