﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Entities
{
    public class PaymentOrderMerchant
    {
        public Guid UniqueId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string AppId { get; set; }
        public string AppKey { get; set; }
    }
}
