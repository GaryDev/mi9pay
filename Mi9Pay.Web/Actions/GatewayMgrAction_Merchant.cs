using Mi9Pay.Entities.GatewayMgr;
using Mi9Pay.ViewModel.Response.GatewayMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Mi9Pay.Web.Actions
{
    public partial class GatewayMgrAction
    {
        public HttpResponseMessage GetAllMerchants()
        {
            List<Merchant> merchantList = _gatewayMgrService.GetAllMerchants();
            if (merchantList == null || merchantList.Count == 0)
            {
                return _request.CreateResponse(HttpStatusCode.NotFound);
            }

            List<MerchantResponse> merchants = new List<MerchantResponse>();
            foreach (var merchant in merchantList)
            {
                merchants.Add(new MerchantResponse { code = merchant.Code, name = merchant.Name });
            }

            return _request.CreateResponse(HttpStatusCode.OK, merchants);
        }
    }
}