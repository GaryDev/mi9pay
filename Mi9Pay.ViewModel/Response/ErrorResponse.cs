using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel
{
    public class ErrorResponse : BaseResponse
    {
        public ErrorResponse(string errMsg)
        {
            return_code = "FAIL";
            return_msg = errMsg;
        }
    }
}
