﻿
namespace Mi9Pay.PayProvider
{
    /// <summary>
    /// 订单是使用二维码支付时创建订单的支付二维码
    /// </summary>
    internal interface IPaymentWithCode
    {

        /// <summary>
        /// 获得订单的支付二维码内容
        /// </summary>
        /// <returns>订单的支付二维码内容</returns>
        string GetPaymentQRCodeContent();

        PaymentResult BarcodePayment();

        bool RefundPayment();
    }
}