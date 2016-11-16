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

		public static string GetEntityConnectionString(string fileName)
        {
            string providerName = "System.Data.SqlClient"; ///MS-SQL server was the database server

            EntityConnectionStringBuilder conStr = new EntityConnectionStringBuilder();
            conStr.Provider = providerName;
            conStr.ProviderConnectionString = AppConfig.EFConnectionString; //database connection string

            //main assembly level configuration
            conStr.Metadata = string.Format(@"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", fileName);
            return conStr.ToString();
        }
    }
}
