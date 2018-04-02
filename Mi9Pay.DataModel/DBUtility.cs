using Mi9Pay.Config;
using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.DataModel
{
    public class DBUtility
    {
        public static readonly string DataModelFileName = "GatewayPayDataModel";

		public static string GetEntityConnectionString(string fileName, string providerConnString = null)
        {
            const string providerName = "System.Data.SqlClient";
		    var conStr = new EntityConnectionStringBuilder
		    {
		        Provider = providerName,
		        ProviderConnectionString = string.IsNullOrEmpty(providerConnString) ? AppConfig.EFConnectionString : providerConnString,
		        Metadata = string.Format(@"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", fileName)
		    };
		    //database connection string

		    //main assembly level configuration
		    return conStr.ToString();
        }
    }
}
