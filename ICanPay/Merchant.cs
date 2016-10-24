using ICanPay.Configs;
using System;

namespace ICanPay
{
    /// <summary>
    /// �̻�����
    /// </summary>
    public class Merchant
    {

        #region ˽���ֶ�

        string appId;
        string userName;
        string key;
        string publicKey;
        Uri notifyUrl;

        #endregion


        #region ���캯��

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


        #region ����

        public string AppId { get; set; }

        /// <summary>
        /// �̻��ʺ�
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
                    throw new ArgumentNullException("UserName", "�̻��ʺŲ���Ϊ��");
                }

                userName = value;
            }
        }


        /// <summary>
        /// �̻���Կ
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
                    throw new ArgumentNullException("Key", "�̻���Կ����Ϊ��");
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
        /// ���ػط�֪ͨURL
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
                    throw new ArgumentNullException("NotifyUrl", "����֪ͨUrl����Ϊ��");
                }

                notifyUrl = value;
            }
        }


        /// <summary>
        /// ��������
        /// </summary>
        public GatewayType GatewayType { get; set; }

        #endregion

    }
}