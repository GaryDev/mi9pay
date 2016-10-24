using ICanPay.Configs;
using System;

namespace ICanPay
{
    /// <summary>
    /// 商户数据
    /// </summary>
    public class Merchant
    {

        #region 私有字段

        string appId;
        string userName;
        string key;
        string publicKey;
        Uri notifyUrl;

        #endregion


        #region 构造函数

        public Merchant()
        {
        }


        public Merchant(string userName, string key, Uri notifyUrl, GatewayType gatewayType)
        {
            this.userName = userName;
            this.key = key;
            this.notifyUrl = notifyUrl;
            GatewayType = gatewayType;
        }

        #endregion


        #region 属性

        public string AppId { get; set; }

        /// <summary>
        /// 商户帐号
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("UserName", "商户帐号不能为空");
                }

                userName = value;
            }
        }


        /// <summary>
        /// 商户密钥
        /// </summary>
        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Key", "商户密钥不能为空");
                }

                key = value;
            }
        }

        public string PublicKey
        {
            get
            {
                return publicKey;
            }

            set
            {
                publicKey = value;
            }
        }


        /// <summary>
        /// 网关回发通知URL
        /// </summary>
        public Uri NotifyUrl
        {
            get
            {
                return notifyUrl;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("NotifyUrl", "网关通知Url不能为空");
                }

                notifyUrl = value;
            }
        }


        /// <summary>
        /// 网关类型
        /// </summary>
        public GatewayType GatewayType { get; set; }

        #endregion

    }
}