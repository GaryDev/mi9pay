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
        int ParseStoreId(string invoiceNumber);

        MemoryStream CreatePaymentQRCode(OrderRequest orderRequest, GatewayType gatewayType, string cid);
        OrderPaymentResponse BarcodePayment(OrderRequest orderRequest, GatewayType gatewayType, string barcode, string cid);
        OrderPaymentResponse QueryPayment(string appId, string invoiceNumber, GatewayType gatewayType, string cid);

        IEnumerable<GatewayType> GetGatewayTypes(string invoice);

        IEnumerable<PaymentMethod> GetPaymentMethodList(int storeId);
        IEnumerable<PaymentScanMode> GetPaymentScanModeList();
        IEnumerable<PaymentCombine> GetPaymentCombineList(int storeId);

        string BuildReturnUrl(OrderRequest request, OrderPaymentResponse response);
        void PaymentNotify(OrderRequest request, OrderPaymentResponse response);

        int DownloadBill(string[] storeIdArray, string billDate, GatewayType type);
        void RefundPayment(string storeId, OrderRefundRequest refundRequest, GatewayType gatewayType);
    }
}
