using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mi9Pay.Entities.GatewayMgr;
using Mi9Pay.DataModel;
using Mi9Pay.Config;
using System.Transactions;

namespace Mi9Pay.Service
{
    public partial class GatewayMgrService
    {
        public int Authenticate(string username, string password)
        {
            GatewayPaymentUser user = _repository.User.GetSingle(x => x.UserName == username && x.PassWord == password);
            if (user == null)
            {
                return 0;
            }
            return user.UserCode;
        }

        public bool ValidateToken(string token)
        {
            GatewayPaymentToken dbToken = _repository.Token.GetSingle(x => x.AuthToken == token);
            if (dbToken != null)
            {
                DateTime now = DateTime.Now;
                if (DateTime.Compare(dbToken.ExpiresOn, now) != -1)
                {
                    try
                    {
                        dbToken.ExpiresOn = now.AddTicks(AppConfig.TokenTimeout.Ticks);
                        using (var scope = new TransactionScope())
                        {
                            _repository.Token.Update(dbToken);
                            _repository.Save();
                            scope.Complete();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    return true;
                }
            }

            return false;
        }

        public Token GenerateToken(int userId)
        {
            DateTime issuedOn = DateTime.Now;
            DateTime expiredOn = issuedOn.AddTicks(AppConfig.TokenTimeout.Ticks);
            string newToken = Guid.NewGuid().ToString();

            try
            {
                GatewayPaymentToken token = new GatewayPaymentToken
                {
                    UniqueId = Guid.NewGuid(),
                    IssuedOn = issuedOn,
                    ExpiresOn = expiredOn,
                    AuthToken = newToken,
                    GatewayPaymentUser = _repository.User.GetSingle(x => x.UserCode == userId).UniqueId
                };
                using (var scope = new TransactionScope())
                {
                    _repository.Token.Insert(token);
                    _repository.Save();
                    scope.Complete();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return new Token { AuthToken = newToken };
        }
    }
}
