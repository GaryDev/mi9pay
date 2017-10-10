using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mi9Pay.Entities.GatewayMgr;

namespace Mi9Pay.Service
{
    public partial class GatewayMgrService : IGatewayMgrService
    {
        public int Authenticate(string username, string password)
        {
            return 1;
        }

        public Token GenerateToken(int userId)
        {
            return new Token { AuthToken = Guid.NewGuid().ToString() };
        }
    }
}
