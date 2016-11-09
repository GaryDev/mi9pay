using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace Mi9Pay.Web.Helpers
{
    public class TimeHelper
    {
        //Internet Time Server class by Alastair Dallas 01/27/04

        //Server IP addresses from 
        //http://tf.nist.gov/tf-cgi/servers.cgi#
        private static string[] Servers = {
            "129.6.15.30",
            "129.6.15.29",
            //"132.163.4.101",
            //"132.163.4.102",
            //"132.163.4.103",
            "128.138.140.44",
            "192.43.244.18",
            "131.107.1.10"
        };

        private static DateTime GetUtcTime()
        {
            //Returns UTC/GMT using an NIST server if possible, 
            // degrading to simply returning the system clock

            //If we are successful in getting NIST time, then
            // LastHost indicates which server was used and
            // LastSysTime contains the system time of the call
            // If LastSysTime is not within 15 seconds of NIST time,
            //  the system clock may need to be reset
            // If LastHost is "", time is equal to system clock

            DateTime result = default(DateTime);
            string timeHost = string.Empty;
            foreach (string ipAddress in Servers)
            {
                result = GetNISTTime(ipAddress);
                if (result > DateTime.MinValue)
                {
                    timeHost = ipAddress;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

            if (string.IsNullOrEmpty(timeHost))
                throw new Exception("时间获取失败，请检查网络后重试 (utc)");

            return result;
        }

        private static DateTime GetNISTTime(string host)
        {
            //Returns DateTime.MinValue if host unreachable or does not produce time
            string timeStr = null;

            try
            {
                TcpClient client = new TcpClientWithTimeout(host, 13, 5000).Connect();
                StreamReader reader = new StreamReader(client.GetStream());
                timeStr = reader.ReadToEnd();
                reader.Close();
            }
            catch (SocketException)
            {
                //Couldn't connect to server, transmission error
                return DateTime.MinValue;
            }
            catch (Exception)
            {
                //Some other error, such as Stream under/overflow
                return DateTime.MinValue;
            }

            if (string.IsNullOrWhiteSpace(timeStr))
            {
                return DateTime.MinValue;
            }

            //Parse timeStr
            if (timeStr.Substring(38, 9) != "UTC(NIST)")
            {
                //This signature should be there
                return DateTime.MinValue;
            }
            if ((timeStr.Substring(30, 1) != "0"))
            {
                //Server reports non-optimum status, time off by as much as 5 seconds
                return DateTime.MinValue;
                //Try a different server
            }

            int jd = int.Parse(timeStr.Substring(1, 5));
            int yr = int.Parse(timeStr.Substring(7, 2));
            int mo = int.Parse(timeStr.Substring(10, 2));
            int dy = int.Parse(timeStr.Substring(13, 2));
            int hr = int.Parse(timeStr.Substring(16, 2));
            int mm = int.Parse(timeStr.Substring(19, 2));
            int sc = int.Parse(timeStr.Substring(22, 2));

            if ((jd < 15020))
            {
                //Date is before 1900
                return DateTime.MinValue;
            }
            if ((jd > 51544))
                yr += 2000;
            else
                yr += 1900;

            return new DateTime(yr, mo, dy, hr, mm, sc);
        }

        private static DateTime GetServerTime()
        {
            DateTime server = DateTime.MinValue;
            WebRequest request = WebRequest.Create("http://www.114time.com/api/clock.php");
            request.Method = "GET";

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resContent = string.Empty;
                    // grab the response
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                resContent = reader.ReadToEnd();
                                TimeServer cfg = JsonConvert.DeserializeObject<TimeServer>(resContent);
                                if (cfg != null && !string.IsNullOrWhiteSpace(cfg.SysTime))
                                {
                                    DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                                    double d = double.Parse(cfg.SysTime);
                                    server = startTime.AddMilliseconds(d);
                                }
                            }
                        }
                    }
                }
            }

            if (server == DateTime.MinValue)
                throw new Exception("时间获取失败，请检查网络后重试 (server)");

            return server;
        }

        private static DateTime GetExpireTime()
        {
            DateTime expire = DateTime.MinValue;
            WebRequest request = WebRequest.Create("http://trial-10.apphb.com/Time.ashx");
            request.Method = "GET";

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string resContent = string.Empty;
                    // grab the response
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                resContent = reader.ReadToEnd();
                                TimeConfig cfg = JsonConvert.DeserializeObject<TimeConfig>(resContent);
                                if (cfg != null && !string.IsNullOrWhiteSpace(cfg.TimeExpire))
                                    expire = DateTime.Parse(cfg.TimeExpire);
                            }
                        }
                    }
                }
            }

            if (expire == DateTime.MinValue)
                throw new Exception("时间获取失败，请检查网络后重试 (exp)");

            return expire;
        }

        public static bool IsValid()
        {
            //DateTime now = GetUtcTime().ToLocalTime();
            DateTime now = GetServerTime();
            DateTime exp = GetExpireTime();
            if (DateTime.Compare(now, exp) <= 0)
                return true;

            return false;
        }
    }

    public class TimeServer
    {
        [JsonProperty("times")]
        public string SysTime { get; set; }
    }

    public class TimeConfig
    {
        [JsonProperty("time_expire")]
        public string TimeExpire { get; set; }
    }

    /// <summary>
    /// TcpClientWithTimeout 用来设置一个带连接超时功能的类
    /// 使用者可以设置毫秒级的等待超时时间 (1000=1second)
    /// 例如:
    /// TcpClient connection = new TcpClientWithTimeout('127.0.0.1',80,1000).Connect();
    /// </summary>
    public class TcpClientWithTimeout
    {
        protected string _hostname;
        protected int _port;
        protected int _timeout_milliseconds;
        protected TcpClient connection;
        protected bool connected;
        protected Exception exception;

        public TcpClientWithTimeout(string hostname, int port, int timeout_milliseconds)
        {
            _hostname = hostname;
            _port = port;
            _timeout_milliseconds = timeout_milliseconds;
        }
        public TcpClient Connect()
        {
            // kick off the thread that tries to connect
            connected = false;
            exception = null;
            Thread thread = new Thread(new ThreadStart(BeginConnect));
            thread.IsBackground = true; // 作为后台线程处理
                                        // 不会占用机器太长的时间
            thread.Start();

            // 等待如下的时间
            thread.Join(_timeout_milliseconds);

            if (connected == true)
            {
                // 如果成功就返回TcpClient对象
                thread.Abort();
                return connection;
            }
            if (exception != null)
            {
                // 如果失败就抛出错误
                thread.Abort();
                throw exception;
            }
            else
            {
                // 同样地抛出错误
                thread.Abort();
                string message = string.Format("TcpClient connection to {0}:{1} timed out",
                  _hostname, _port);
                throw new TimeoutException(message);
            }
        }
        protected void BeginConnect()
        {
            try
            {
                connection = new TcpClient(_hostname, _port);
                // 标记成功，返回调用者
                connected = true;
            }
            catch (Exception ex)
            {
                // 标记失败
                exception = ex;
            }
        }
    }
}