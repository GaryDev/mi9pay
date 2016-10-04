using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ICanPay.Configs
{
    public class AlipayConfig
    {
        public static string alipay_public_key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDIgHnOn7LLILlKETd6BFRJ0GqgS2Y3mn1wMQmyh9zEyWlz5p1zrahRahbXAfCfSqshSNfqOmAQzSHRVjCqjsAw1jyqrXaPdKBmr90DIpIxmIyKXv4GGAkPyJ/6FTFY99uhpiq0qadD/uSzQsefWo0aTvP/65zi3eof7TcZ32oWpwIDAQAB";
        //这里要配置没有经过的原始私钥
        public static string merchant_private_key = @"MIICWwIBAAKBgQC50ir7qdKkpF7D0hwfZunQfZTiCmno5vYTIM6oGY+52ME0tUBx4hW+NMUnLqXqKhqSeMZrC17+Xqq/2sBmhtDryAUcU2+gTKUcksAc7JXQdzkGqqTgRQYQQM5A5J14Pld49XTuTQIXRTzB06aOISSkqpuym+7oI8AGQF5hmbHVbQIDAQABAoGAMJkazL7ZbF3guu4UlNHhjLmLWqLGmDbvXIlUAvrMcBqUTSiqNh6e+SPr/BdjJR7l3DDiE9Thfz1bAto/P5E6yj0QCkTkbnj2xYzPSaMfwct9QpTY074cbkTZZ6ez8ja8midjqAMU/nBdevE+UtszKA8XjHW0AFAFMBYyk5Z/WgECQQDsHNn1youCCJWMlGzo/MZy/Hlll1vlps52Fo+bROdV9OKvQ4iLGTAB7JR07AA8nSK03pATgUGFbcopB5Vkj+thAkEAyXjoYOPwzh4iASPD0BQJbizti2EmHQ3w0hIi5tXRsGxfIZpgGcbyS48Y5at/WjGMUljF2jXCwdiYhfGgKSPRjQJAeADt8pPANhXg1HN3qy8WOckCdlToex89nh03XeY2YaS2NffwBSqHEONKTObJ9AS1aBIaTh+KyqMTdakKD/Np4QJAVdJgQq22bUbeu1eN2PxADCOtSLsobiX7GLFLFsOsYBe56mmFWFWr7s3VEDietj/3Azj3hv1xqftm9V+5Fu1AHQJAFC3FoL/9EML516GahIHCEzimFqNtTe3ct6N3zkoHgmM+Ha3C575tPMG1rDHfHeckQEbb4l6TxqP8iVB9p0i1kA==";
        public static string merchant_public_key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC50ir7qdKkpF7D0hwfZunQfZTiCmno5vYTIM6oGY+52ME0tUBx4hW+NMUnLqXqKhqSeMZrC17+Xqq/2sBmhtDryAUcU2+gTKUcksAc7JXQdzkGqqTgRQYQQM5A5J14Pld49XTuTQIXRTzB06aOISSkqpuym+7oI8AGQF5hmbHVbQIDAQAB";
        public static string appId = "2016090900473027";

        public static string serverUrl = "https://openapi.alipaydev.com/gateway.do";
        public static string mapiUrl = "https://mapi.alipay.com/gateway.do";
        public static string monitorUrl = "http://mcloudmonitor.com/gateway.do";

        public static string pid = "2088102174860232";
        public static string storeId = "STORE_MIPAY";
        public static string operId = "OPER_MIPAY";

        public static string charset = "utf-8";//"utf-8";
        public static string sign_type = "RSA";
        public static string version = "1.0";


        public AlipayConfig()
        {
            //
        }

        public static string getMerchantPublicKeyStr()
        {
            StreamReader sr = new StreamReader(merchant_public_key);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }

        public static string getMerchantPriveteKeyStr()
        {
            StreamReader sr = new StreamReader(merchant_private_key);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }
    }
}
