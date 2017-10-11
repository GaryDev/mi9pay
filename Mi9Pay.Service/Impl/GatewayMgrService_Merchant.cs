using AutoMapper;
using Mi9Pay.DataModel;
using Mi9Pay.Entities.GatewayMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Service
{
    public partial class GatewayMgrService
    {
        public List<Merchant> GetAllMerchants()
        {
            List<GatewayPaymentMerchant> merchants = _repository.Merchant.GetAll().ToList();

            Mapper.Initialize(cfg => cfg.CreateMap<GatewayPaymentMerchant, Merchant>());
            List<Merchant> merchantList = Mapper.Map<List<GatewayPaymentMerchant>, List<Merchant>>(merchants);
            return merchantList;
        }
    }
}
