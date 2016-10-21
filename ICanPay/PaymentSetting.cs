using ICanPay.Providers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace ICanPay
{
    /// <summary>
    /// ������Ҫ֧���Ķ��������ݣ�����֧������URL��ַ��HTML��
    /// </summary>
    /// <remarks>
    /// ��Ϊ����֧�����صı����֧��GB2312����������֧������ͳһʹ��GB2312���롣
    /// ����Ҫ��֤���HTML�����ҳ��ΪGB2312���룬������ܻ���Ϊ���������޷���������֧��������ʶ��֧�����ص�֧��֪ͨ��
    /// ͨ���� Web.config �е� configuration/system.web �ڵ����� <globalization requestEncoding="gb2312" responseEncoding="gb2312" />
    /// ���Խ�ҳ���Ĭ�ϱ�������ΪGB2312��Ŀǰֻ��ʹ��RMB֧������������֧�����Ķ�������ؽӿ��ĵ��޸ġ�
    /// </remarks>
    public class PaymentSetting
    {

        #region �ֶ�

        GatewayBase gateway;

        GatewayType _gatewayType;

        #endregion


        #region ���캯��

        public PaymentSetting(GatewayType gatewayType)
        {
            gateway = CreateGateway(gatewayType);
            _gatewayType = gatewayType;
        }


        public PaymentSetting(GatewayType gatewayType, Merchant merchant, Order order)
            : this(gatewayType)
        {
            gateway.Merchant = merchant;
            gateway.Order = order;
        }

        #endregion


        #region ����

        /// <summary>
        /// ����
        /// </summary>
        public GatewayBase Gateway
        {
            get
            {
                return gateway;
            }
        }


        /// <summary>
        /// �̼�����
        /// </summary>
        public Merchant Merchant
        {
            get
            {
                return gateway.Merchant;
            }

            set
            {
                gateway.Merchant = value;
            }
        }


        /// <summary>
        /// ��������
        /// </summary>
        public Order Order
        {
            get
            {
                return gateway.Order;
            }

            set
            {
                gateway.Order = value;
            }
        }


        public bool CanQueryNotify
        {
            get
            {
                if (gateway is IQueryUrl || gateway is IQueryForm)
                {
                    return true;
                }

                return false;
            }
        }


        public bool CanQueryNow
        {
            get
            {
                return gateway is IQueryNow;
            }
        }

        #endregion


        #region ����


        private GatewayBase CreateGateway(GatewayType gatewayType)
        {
            switch (gatewayType)
            {
                case GatewayType.Alipay:
                    {
                        return new AlipayGateway();
                    }

                case GatewayType.WeChatPayment:
                    {
                        return new WeChatPaymentGataway();
                    }

                case GatewayType.Tenpay:
                    {
                        return new TenpayGateway();
                    }

                case GatewayType.Yeepay:
                    {
                        return new YeepayGateway();
                    }

                default:
                    {
                        return new NullGateway();
                    }
            }
        }


        /// <summary>
        /// ����������֧��Url��Form������ά�롣
        /// </summary>
        /// <remarks>
        /// ����������Ƕ�����Url��Form������ת����Ӧ����֧��������Ƕ�ά�뽫�����ά��ͼƬ��
        /// </remarks>
        public void Payment()
        {
            IPaymentUrl paymentUrl = gateway as IPaymentUrl;
            if (paymentUrl != null)
            {
                HttpContext.Current.Response.Redirect(paymentUrl.BuildPaymentUrl());
                return;
            }

            IPaymentForm paymentForm = gateway as IPaymentForm;
            if (paymentForm != null)
            {
                HttpContext.Current.Response.Write(paymentForm.BuildPaymentForm());
                return;
            }

            IPaymentWithCode paymentQRCode = gateway as IPaymentWithCode;
            if (paymentQRCode != null)
            {
                MemoryStream ms = BuildQRCodeImage(paymentQRCode.GetPaymentQRCodeContent());
                HttpContext.Current.Response.ContentType = "image/x-png";
                HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
                return;
            }

            throw new NotSupportedException(gateway.GatewayType + " û��ʵ��֧���ӿ�");
        }

        public MemoryStream PaymentQRCode()
        {
            IPaymentWithCode codePayment = gateway as IPaymentWithCode;
            if (codePayment != null)
            {
                string qrCode = codePayment.GetPaymentQRCodeContent();

                if (string.IsNullOrEmpty(qrCode))
                    return null;

                return BuildQRCodeImage(qrCode);
            }

            throw new NotSupportedException(gateway.GatewayType + " û��ʵ��֧���ӿ�");
        }

        public PaymentResult BarcodePayment()
        {
            IPaymentWithCode codePayment = gateway as IPaymentWithCode;
            if (codePayment != null)
            {
                return codePayment.BarcodePayment();
            }
            throw new NotSupportedException(gateway.GatewayType + " û��ʵ��֧���ӿ�");
        }

        /// <summary>
        /// ��ѯ�����������Ĳ�ѯ֪ͨ����ͨ����֧��֪ͨһ������ʽ���ء��ô�������֪ͨһ���ķ������ܲ�ѯ���������ݡ�
        /// </summary>
        public void QueryNotify()
        {
            IQueryUrl queryUrl = gateway as IQueryUrl;
            if (queryUrl != null)
            {
                HttpContext.Current.Response.Redirect(queryUrl.BuildQueryUrl());
                return;
            }

            IQueryForm queryForm = gateway as IQueryForm;
            if (queryForm != null)
            {
                HttpContext.Current.Response.Write(queryForm.BuildQueryForm());
                return;
            }

            throw new NotSupportedException(gateway.GatewayType + " û��ʵ�� IQueryUrl �� IQueryForm ��ѯ�ӿ�");
        }

        
        /// <summary>
        /// ��ѯ������������ö����Ĳ�ѯ���
        /// </summary>
        /// <returns></returns>
        public bool QueryNow()
        {
            IQueryNow queryNow = gateway as IQueryNow;
            if (queryNow != null)
            {
                return queryNow.QueryNow();
            }

            throw new NotSupportedException(gateway.GatewayType + " û��ʵ�� IQueryNow ��ѯ�ӿ�");
        }

        /// <summary>
        /// ��ѯ������������ö����Ĳ�ѯ���
        /// </summary>
        /// <returns></returns>
        public PaymentResult QueryForResult()
        {
            IQueryNow queryNow = gateway as IQueryNow;
            if (queryNow != null)
            {
                return queryNow.QueryForResult();
            }

            throw new NotSupportedException(gateway.GatewayType + " û��ʵ�� IQueryNow ��ѯ�ӿ�");
        }

        /// <summary>
        /// �������ص�����
        /// </summary>
        /// <param name="gatewayParameterName">���صĲ�������</param>
        /// <param name="gatewayParameterValue">���صĲ���ֵ</param>
        public void SetGatewayParameterValue(string gatewayParameterName, string gatewayParameterValue)
        {
            Gateway.SetGatewayParameterValue(gatewayParameterName, gatewayParameterValue);
        }


        /// <summary>
        /// ���ɲ������ά��ͼƬ
        /// </summary>
        /// <param name="qrCodeContent">��ά������</param>
        private MemoryStream BuildQRCodeImage(string qrCodeContent)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeScale = 4;  // ��ά���С
            qrCodeEncoder.QRCodeVersion = 8;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

            Bitmap image = qrCodeEncoder.Encode(qrCodeContent, Encoding.Default);
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);

                string logo = string.Empty;
                if (_gatewayType == GatewayType.WeChatPayment)
                    logo = "wx";
                else if (_gatewayType == GatewayType.Alipay)
                    logo = "alipay";

                if (!string.IsNullOrEmpty(logo))
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    Image imageWithLogo = AddLogo(image, string.Format(@"{0}\bin\Logo\{1}.jpg", baseDir, logo));
                    using (MemoryStream msNew = new MemoryStream())
                    {
                        imageWithLogo.Save(msNew, ImageFormat.Png);
                        return msNew;
                    }
                }

                return ms;
            }
        }

        /// <summary>    
        /// ���ô˺�����ʹ������ͼƬ�ϲ���������ᣬ�и�    
        /// ����ͼ���м����Լ���Ŀ��ͼƬ    
        /// </summary>    
        /// <param name="imgBack">ճ����ԴͼƬ</param>    
        /// <param name="logoImg">ճ����Ŀ��ͼƬ</param>    
        public static Image AddLogo(Image imgBack, string logoImg)
        {
           if (!File.Exists(logoImg))
                return imgBack;

            Image img = Image.FromFile(logoImg);        //LogoͼƬ      
            if (img.Height != 35 || img.Width != 35)
            {
                img = KiResizeImage(img, 35, 35, 0);
            }
            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);
            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }


        /// <summary>    
        /// ResizeͼƬ    
        /// </summary>    
        /// <param name="bmp">ԭʼBitmap</param>    
        /// <param name="newW">�µĿ��</param>    
        /// <param name="newH">�µĸ߶�</param>    
        /// <param name="Mode">�����ţ���ʱδ��</param>    
        /// <returns>�����Ժ��ͼƬ</returns>    
        private static Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // ��ֵ�㷨������    
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        #endregion

    }
}
