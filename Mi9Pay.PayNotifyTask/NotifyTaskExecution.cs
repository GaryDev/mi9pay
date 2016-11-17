using Mi9Pay.Entities;
using Mi9Pay.Service;
using Mi9Pay.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler;

namespace Mi9Pay.PayNotifyTask
{
    public class NotifyTaskExecution : TaskExecutionAdapter
    {
        public override TaskExecResult TaskTrigger()
        {
            DateTime processTime = DateTime.Now;
            logger.Info(string.Format("任务 {0} 处理开始 - {1}.", TaskId, processTime));
            try
            {
                INotifyTaskService taskService = new NotifyTaskService();
                List<NotifyQueue> queueList = taskService.GetNotificationQueue(processTime);
                logger.Info(string.Format("任务 {0} 处理记录数：{1} - {2}.", TaskId, queueList.Count, processTime));
                if (queueList.Count > 0)
                {
                    queueList.ForEach(q =>
                    {
                        logger.Info(string.Format("任务 {0} 处理订单号{1}, 处理次数{2} - {3}.", 
                            TaskId, q.OrderNumber, q.ProcessedCount, processTime));

                        q.LastSendDate = processTime;
                        q.ProcessedCount = q.ProcessedCount + 1;
                        NotifyAsyncParameter parameter = new NotifyAsyncParameter
                        {
                            NotifyPostInfo = new NotifyPostInfo
                            {
                                PostUrl = q.NotifyUrl,
                                PostData = q.PostData,
                                IsRawData = (q.PostDataFormat == NotifyDataFormat.RAW)
                            },
                            NotifyQueue = q,
                            PostAction = taskService.UpdateNotificationQueue
                        };
                        logger.Info(string.Format("任务 {0} 处理订单号{1}, 通知发送地址{2}, 发送数据格式{3} - {4}.",
                            TaskId, q.OrderNumber, q.NotifyUrl, Enum.GetName(typeof(NotifyDataFormat), q.PostDataFormat), processTime));

                        WebClientHelper.SendNotification(parameter);
                        logger.Info(string.Format("任务 {0} 处理成功 - {1}.", TaskId, processTime));
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Info(string.Format("任务 {0} 处理失败 - {1}.", TaskId, processTime));
                logger.Info("错误原因:");
                logger.Info(ex.Message);
                if (ex.InnerException != null)
                    logger.Info(ex.InnerException.Message);
            }
            return null;
        }
    }
}
