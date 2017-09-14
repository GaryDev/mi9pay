using Mi9Pay.ViewModel.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.ViewModel.Test
{
    public class OrderViewModel
    {
        public int StoreId { get; set; }
        public string StoreCurrency { get; set; }

        public string InvoiceNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDetailViewModel> OrderItems { get; set; }

        public PayConfig Config { get; set; }

        public string Sign { get; set; }

        public OrderViewModel()
        {
            StoreId = int.Parse(ConfigurationManager.AppSettings.Get("StoreId"));
            StoreCurrency = ConfigurationManager.AppSettings.Get("StoreCurrency");
            InvoiceNumber = DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString();

            OrderItems = new List<OrderDetailViewModel>();
            int itemCount = int.Parse(ConfigurationManager.AppSettings.Get("ItemCount"));
            decimal itemUnitPrice = decimal.Parse(ConfigurationManager.AppSettings.Get("ItemUnitPrice"));
            for (int i = 0; i < itemCount; i++)
            {
                OrderItems.Add(new OrderDetailViewModel { ItemName = string.Format("Item-{0}", i+1), ItemPrice = itemUnitPrice, ItemQty = 1M });
            }
            TotalAmount = OrderItems.Sum(x => x.ItemAmount);
            Config = new PayConfig
            {
                AppId = ConfigurationManager.AppSettings.Get("AppId"),
                AppKey = ConfigurationManager.AppSettings.Get("AppKey"),
                DoneUrl = ConfigurationManager.AppSettings.Get("DoneUrl"),
                NotifyUrl = ConfigurationManager.AppSettings.Get("NotifyUrl")
            };

            Sign = CreateSign();
        }

        private string CreateSign()
        {
            Dictionary<string, string> request = new Dictionary<string, string>();
            request.Add("app_id", Config.AppId);
            request.Add("store_id", StoreId.ToString());
            request.Add("currency", StoreCurrency);
            request.Add("invoice", InvoiceNumber);
            request.Add("amount", (TotalAmount * 100).ToString());
            for (int i = 0; i < OrderItems.Count; i++)
            {
                request.Add(string.Format("item_{0}_name", i), OrderItems[i].ItemName);
                request.Add(string.Format("item_{0}_quantity", i), OrderItems[i].ItemQty.ToString());
                request.Add(string.Format("item_{0}_amount", i), (OrderItems[i].ItemAmount * 100).ToString());
            }
            request.Add("done_url", Config.DoneUrl);
            request.Add("notify_url", Config.NotifyUrl);

            string sortedString = SignatureUtil.CreateSortedParams(request);
            string sign = SignatureUtil.CreateSignature(sortedString + Config.AppKey);
            return sign;
        }
    }

    public class PayConfig
    {
        public string AppId { get; set; }
        public string AppKey { get; set; }
        public string DoneUrl { get; set; }
        public string NotifyUrl { get; set; }
    }
}
