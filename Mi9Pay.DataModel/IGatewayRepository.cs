using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.DataModel
{
    public interface IGatewayRepository : IDisposable
    {
        void Save();
    }
}
