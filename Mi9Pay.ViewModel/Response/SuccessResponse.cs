using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel
{
    public class SuccessResponse : BaseResponse
    {
        public Dictionary<string, string> data { get; private set; }

        public SuccessResponse()
        {
            return_code = "SUCCESS";
            return_msg = "OK";
            data = new Dictionary<string, string>();
        }

        public SuccessResponse AddData(string key, string value)
        {
            if (data.ContainsKey(key))
                data[key] = value;
            else
                data.Add(key, value);

            return this;
        }
    }
}
