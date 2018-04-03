using System;
using System.Collections.Generic;
using System.Linq;
using Mi9Pay.Entities;
using AutoMapper;
using Mi9Pay.DataModel;
using System.Transactions;

namespace Mi9Pay.Service
{
    public class NotifyTaskService : INotifyTaskService
    {
        public NotifyTaskService(string connectString)
        {
            Repository = new GatewayRepository(connectString);
        }

        public GatewayRepository Repository { get; set; }

        public List<NotifyQueue> GetNotificationQueue(DateTime processTime)
        {
            //_repository = new GatewayRepository();
            var notifyQueueList = Repository.NotifyQueue.GetMany(q =>
                    q.Processed == "N" &&
                    q.NextInterval > 0 &&
                    DateTime.Compare(processTime, q.SendDate.AddMinutes(q.NextInterval)) >= 0).ToList();

            Mapper.Initialize(cfg => cfg.CreateMap<GatewayPaymentNotifyQueue, NotifyQueue>());
            var queueList = Mapper.Map<List<GatewayPaymentNotifyQueue>, List<NotifyQueue>>(notifyQueueList);
            return queueList;
        }

        public void UpdateNotificationQueue(NotifyQueue queue)
        {
            using (var scope = new TransactionScope())
            {
                //_repository = new GatewayRepository();
                var notifyQueue = Repository.NotifyQueue.GetSingle(q => q.UniqueId == queue.UniqueId);
                if (notifyQueue != null)
                {
                    notifyQueue.LastSendDate = queue.LastSendDate;
                    notifyQueue.NextInterval = queue.NextInterval;
                    notifyQueue.ProcessedCount = queue.ProcessedCount;
                    notifyQueue.Processed = queue.Processed;
                    Repository.NotifyQueue.Update(notifyQueue);
                    Repository.Save();
                    scope.Complete();
                }
            }
        }
    }
}
