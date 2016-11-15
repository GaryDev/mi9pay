using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Entities
{
    public class NotifyQueue
    {
        public Guid UniqueId { get; set; }
        public string OrderNumber { get; set; }
        public string NotifyUrl { get; set; }
        public string PostData { get; set; }
        public NotifyDataFormat PostDataFormat { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime LastSendDate { get; set; }
        public int NextInterval { get; set; }
        public int ProcessedCount { get; set; }
        public string Processed { get; set; }
    }

    public class NotifyPostInfo
    {
        public string PostUrl { get; set; }
        public string PostData { get; set; }
        public bool IsRawData { get; set; }
    }

    public class NotifyAsyncParameter
    {
        public NotifyPostInfo NotifyPostInfo { get; set; }
        public NotifyQueue NotifyQueue { get; set; }
        public Action<NotifyQueue> PostAction { get; set; }
    }

    public static class NotifyConfig
    {
        // Minutes
        public static int[] NotifyStrategy = new int[]
        {
            0, 2, 10, 60, 2 * 60, 6 * 60, 15 * 60, 24 * 60
        };
    }

    public enum NotifyDataFormat
    {
        RAW,
        JSON
    }
}
