using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mi9Pay.Entities;
using AutoMapper;
using Mi9Pay.DataModel;
using System.Transactions;
using NLog;

namespace Mi9Pay.Service
{
    public class NotifyTaskService : INotifyTaskService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private GatewayRepository _repository;

        public NotifyTaskService()
        {
            _repository = new GatewayRepository();
        }

        public List<NotifyQueue> GetNotificationQueue(DateTime processTime)
        {
            try
            {
                //_repository = new GatewayRepository();
                List<GatewayPaymentNotifyQueue> notifyQueueList = _repository.NotifyQueue.GetMany(q =>
                        q.Processed == "N" &&
                        q.NextInterval > 0 &&
                        DateTime.Compare(processTime, q.SendDate.AddMinutes(q.NextInterval)) >= 0).ToList();

                Mapper.Initialize(cfg => cfg.CreateMap<GatewayPaymentNotifyQueue, NotifyQueue>());
                List<NotifyQueue> queueList = Mapper.Map<List<GatewayPaymentNotifyQueue>, List<NotifyQueue>>(notifyQueueList);
                return queueList;
            }
            catch (Exception ex)
            {
                logger.Info("错误原因:");
                logger.Info(ex.Message);
                if (ex.InnerException != null)
                    logger.Info(ex.InnerException.Message);

                throw ex;
            }            
        }

        public void UpdateNotificationQueue(NotifyQueue queue)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    //_repository = new GatewayRepository();
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
                logger.Info("错误原因: " + queue.OrderNumber);
                logger.Info(ex.Message);
                if (ex.InnerException != null)
                    logger.Info(ex.InnerException.Message);

                throw ex;
            }
        }
    }
}
