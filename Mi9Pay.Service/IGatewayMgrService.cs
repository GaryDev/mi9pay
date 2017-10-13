using Mi9Pay.Entities.GatewayMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Service
{
    public interface IGatewayMgrService
    {
        Token GenerateToken(int userId);

        int Authenticate(string username, string password);
        bool ValidateToken(string token);
     

        List<Merchant> GetAllMerchants();
    }
}
