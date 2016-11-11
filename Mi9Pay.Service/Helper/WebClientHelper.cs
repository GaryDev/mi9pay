using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Mi9Pay.Service.Helper
{
    public class WebClientHelper
    {
        public const string SUCCESS_CODE = "200";
        public const string ERROR_CODE = "500";

        public static string PostData(string postUrl, string postData)
        {
            try
            {
                WebRequest request = WebRequest.Create(postUrl);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.Timeout = 60000;

                byte[] btBodys = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = btBodys.Length;
                request.GetRequestStream().Write(btBodys, 0, btBodys.Length);

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return ERROR_CODE;
                    else
                    {
                        // grab the response
                        using (var responseStream = response.GetResponseStream())
                        {
                            if (responseStream == null)
                                return ERROR_CODE;
                            else
                            {
                                using (var reader = new StreamReader(responseStream))
                                {
                                    string resContent = reader.ReadToEnd();
                                    return resContent == SUCCESS_CODE ? SUCCESS_CODE : ERROR_CODE;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return ERROR_CODE;
            }
        } 

    }
}
