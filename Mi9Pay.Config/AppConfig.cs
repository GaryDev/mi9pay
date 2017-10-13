using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Mi9Pay.Config
{
    public sealed partial class AppConfig
    {
        public static bool IsLogEnabled
        {
            get
            {
                bool logEnabled = false;
                if (ConfigurationManager.AppSettings.AllKeys.Contains("LogEnabled"))
                {
                    string logEnabledValue = ConfigurationManager.AppSettings.Get("LogEnabled");
                    if (bool.TryParse(logEnabledValue, out logEnabled))
                        return logEnabled;
                }
                return logEnabled;
            }
        }

        public static string EFConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("EFConnectionString");
            }
        }


        public static string PasswordPhase
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("PasswordPhase");
            }
        }

        public static TimeSpan TokenTimeout
        {
            get
            {
                TimeSpan timeout = new TimeSpan(0, 5, 0);
                if (ConfigurationManager.AppSettings.AllKeys.Contains("TokenTimeout"))
                {
                    string timeoutValue = ConfigurationManager.AppSettings.Get("TokenTimeout");
                    bool hasFormatError = false;

                    string[] timeoutArray = timeoutValue.Split(':');

                    if (timeoutArray.Length != 3)
                        hasFormatError = true;
                    else
                    {
                        int hours = 0;
                        int minutes = 0;
                        int seconds = 0;
                        if (!int.TryParse(timeoutArray[0], out hours) ||
                            !int.TryParse(timeoutArray[1], out minutes) ||
                            !int.TryParse(timeoutArray[2], out seconds))
                            hasFormatError = true;
                        else
                            timeout = new TimeSpan(hours, minutes, seconds);
                    }

                    if (hasFormatError)
                        throw new ConfigurationErrorsException("Error format of Token Timeout (Hours:Minutes:Seconds) in App Config");
                }
                return timeout;
            }
        }
    }

    public sealed class AppConstants
    {
        public static readonly string KEY_TOKEN = "Token";
        public static readonly string KEY_CLIENT_TIME = "ClientTime";
    }
}
