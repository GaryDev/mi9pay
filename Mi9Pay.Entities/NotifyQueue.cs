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

    public static class NotifyConfig
    {
        public static Dictionary<int, int> NotifyStrategy = new Dictionary<int, int>
        {
            { 1, 2 }, { 2, 10 },
            { 3, 60 }, { 4, 2 * 60 }, { 5, 6 * 60 },
            { 6, 15 * 60 }, { 7, 24 * 60 }
        };
    }

    public enum NotifyDataFormat
    {
        RAW,
        JSON
    }
}
