using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mi9Pay.Entities.GatewayMgr
{
    public class User
    {
        public User()
        {
            Tokens = new List<Token>();
        }

        public Guid UniqueId { get; set; }
        public int UserId { get; set; }

        public List<Token> Tokens { get; set; }
    }
}