using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Mi9Pay.ViewModel.Utility
{
    public class SignatureUtil
    {
        public static string CreateSortedParams(Dictionary<string, string> data)
        {
            Dictionary<string, string> sortedData = data.OrderBy(o => o.Key).ToDictionary(k => k.Key, v => v.Value);
            StringBuilder sb = new StringBuilder();
            foreach (string key in sortedData.Keys)
            {
                if (!string.IsNullOrWhiteSpace(sortedData[key]))
                {
                    //string value = System.Web.HttpUtility.UrlEncode(sortedData[key], Encoding.UTF8);
                    string value = sortedData[key];
                    sb.AppendFormat("{0}={1}&", key, value);
                }
            }

            string param = sb.ToString();
            return string.IsNullOrWhiteSpace(param) ? string.Empty : param.Substring(0, param.Length - 1);
        }

        public static string CreateSortedParams(NameValueCollection namedCollection)
        {
            Dictionary<string, string> data = CreateSignDictWithQueryString(namedCollection);
            return CreateSortedParams(data);
        }

        private static Dictionary<string, string> CreateSignDictWithQueryString(NameValueCollection namedCollection)
        {
            var signDict = new Dictionary<string, string>();
            if (namedCollection != null)
            {
                foreach (var named in namedCollection.AllKeys)
                {
                    if (string.IsNullOrEmpty(namedCollection[named])) continue;
                    signDict[named] = namedCollection[named];
                }
            }
            return signDict;
        }

        public static string CreateSignature(string strTobeEncrypt, SignatureMethod method = SignatureMethod.MD5)
        {
            switch (method)
            {
                case SignatureMethod.MD5:
                    return MD5Signature(strTobeEncrypt);
                default:
                    return MD5Signature(strTobeEncrypt);
            }
        }

        private static string MD5Signature(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] secret = md5.ComputeHash(Encoding.Default.GetBytes(text));
            string sign = BitConverter.ToString(secret).Replace("-", "");
            return sign.ToLower();
        }
    }

    public enum SignatureMethod
    {
        MD5
    }
}
