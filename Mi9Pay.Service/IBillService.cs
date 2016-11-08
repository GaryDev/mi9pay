using Mi9Pay.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Service
{
    public interface IBillService
    {
        int ImportData(GatewayRepository repository, string data);
    }
}
