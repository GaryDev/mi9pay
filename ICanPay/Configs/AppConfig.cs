using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ICanPay.Configs
{
    public class AppConfig
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

    }
}
