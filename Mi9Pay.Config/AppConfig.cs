using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Mi9Pay.Config
{
    public sealed class AppConfig
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

    }
}
