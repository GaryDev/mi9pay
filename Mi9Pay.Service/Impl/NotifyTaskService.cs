using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mi9Pay.Entities;
using AutoMapper;
using Mi9Pay.DataModel;
using System.Transactions;

namespace Mi9Pay.Service
{
    public class NotifyTaskService : INotifyTaskService
    {
        private GatewayRepository _repository;
        public NotifyTaskService()
        {
            _repository = new GatewayRepository();
        }

        public List<NotifyQueue> GetNotificationQueue(DateTime processTime)
        {
            List<GatewayPaymentNotifyQueue> notifyQueueList = _repository.NotifyQueue.GetMany(q => 
                q.Processed == "N" &&
                q.NextInterval > 0 &&
                DateTime.Compare(processTime, q.LastSendDate.AddMinutes(q.NextInterval)) >= 0).ToList();

            Mapper.Initialize(cfg => cfg.CreateMap<GatewayPaymentNotifyQueue, NotifyQueue>());
            List<NotifyQueue> queueList = Mapper.Map<List<GatewayPaymentNotifyQueue>, List<NotifyQueue>>(notifyQueueList);
            return queueList;
        }

        public void UpdateNotificationQueue(NotifyQueue queue)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    GatewayPaymentNotifyQueue notifyQueue = _repository.NotifyQueue.GetSingle(q => q.UniqueId == queue.UniqueId);
                    if (notifyQueue != null)
                    {
                        notifyQueue.LastSendDate = queue.LastSendDate;
                        notifyQueue.NextInterval = queue.NextInterval;
                        notifyQueue.ProcessedCount = queue.ProcessedCount;
                        notifyQueue.Processed = queue.Processed;
                        _repository.NotifyQueue.Update(notifyQueue);
                        _repository.Save();
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
