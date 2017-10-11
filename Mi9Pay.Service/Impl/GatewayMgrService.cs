using Mi9Pay.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Service
{
    public partial class GatewayMgrService : IGatewayMgrService
    {
        private readonly GatewayRepository _repository;
        public GatewayMgrService(GatewayRepository repository)
        {
            _repository = repository;
        }
    }
}
