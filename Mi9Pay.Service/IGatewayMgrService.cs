﻿using Mi9Pay.Entities.GatewayMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Service
{
    public interface IGatewayMgrService
    {
        int Authenticate(string username, string password);
        Token GenerateToken(int userId);
    }
}