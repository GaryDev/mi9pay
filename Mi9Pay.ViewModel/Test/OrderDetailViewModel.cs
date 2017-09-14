using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel.Test
{
    public class OrderDetailViewModel
    {
        public string ItemName { get; set; }
        public decimal ItemQty { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal ItemAmount
        {
            get { return ItemPrice * ItemQty; }                
        }
    }
}
