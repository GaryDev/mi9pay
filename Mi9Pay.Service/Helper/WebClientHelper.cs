using Mi9Pay.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Mi9Pay.Service.Helper
{
    public class WebClientHelper
    {
        public const string SUCCESS_CODE = "200";
        public const string ERROR_CODE = "500";

        public static void SendNotification(NotifyAsyncParameter notifyParameter)
        {
            if (notifyParameter == null) return;

            ParameterizedThreadStart pts = new ParameterizedThreadStart(PostNotification);
            Thread notifyThread = new Thread(pts);
            notifyThread.Start(notifyParameter);
        }

        private static void PostNotification(object o)
        {
            NotifyAsyncParameter parameter = o as NotifyAsyncParameter;
            if (parameter != null)
            {
                bool notifySuccess = PostData(parameter.NotifyPostInfo.PostUrl, parameter.NotifyPostInfo.PostData, parameter.NotifyPostInfo.IsRawData);
                NotifyQueue queue = parameter.NotifyQueue;
                queue.NextInterval = notifySuccess ? 0 : NotifyConfig.NotifyStrategy[1];
                queue.Processed = notifySuccess ? "Y" : "N";
                parameter.PostAction(queue);
            }
        }

        private static bool PostData(string postUrl, string postData, bool rawData)
        {
            try
            {
                WebRequest request = WebRequest.Create(postUrl);
                request.ContentType = rawData ? "application/x-www-form-urlencoded" : "application/json";
                request.Method = "POST";
                request.Timeout = 60000;

                byte[] btBodys = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = btBodys.Length;
                request.GetRequestStream().Write(btBodys, 0, btBodys.Length);

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return false;
                    else
                    {
                        // grab the response
                        using (var responseStream = response.GetResponseStream())
                        {
                            if (responseStream == null)
                                return false;
                            else
                            {
                                using (var reader = new StreamReader(responseStream))
                                {
                                    string resContent = reader.ReadToEnd();
                                    return resContent == SUCCESS_CODE;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    

    
}
