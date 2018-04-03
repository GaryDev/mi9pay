using Mi9Pay.Entities;
using Mi9Pay.Service;
using Mi9Pay.Service.Helper;
using System;
using System.Collections.Generic;
using System.Threading;
using TaskScheduler;

namespace Mi9Pay.PayNotifyTask
{
    public class NotifyTaskBundle : TaskBundle
    {
        protected override ScheduleTask GetTask()
        {
            return new NotifyScheduleTask();
        }
    }

    internal class NotifyScheduleTask : ScheduleTaskAdapter
    {
        private INotifyTaskService _taskService;
        public INotifyTaskService TaskService
        {
            get
            {
                if (_taskService == null)
                    _taskService = new NotifyTaskService(DbString);
                return _taskService;
            }
        }

        protected override ScheduleTaskResult TaskAction()
        {
            var result = new ScheduleTaskResult();
            var processTime = DateTime.Now;
            try
            {
                logger.Debug(string.Format("任务 {0} 处理开始 - {1}.", Id, DateTimeToString(processTime)));
                var queueList = TaskService.GetNotificationQueue(processTime);

                result.ExtendedParams.Add("processTime", processTime);
                result.ExtendedParams.Add("queueList", queueList);
            }
            catch (Exception ex)
            {
                result.ResultCode = TaskResultCode.FAIL;
                result.ResultMessage = ex.Message;
                if (ex.InnerException != null)
                    result.ResultMessage += Environment.NewLine + ex.InnerException.Message;
            }
            return result;
        }

        protected override void TaskCallbackAction(ScheduleTaskResult result)
        {
            if (result.ResultCode == TaskResultCode.FAIL)
            {
                logger.Error(string.Format("任务 {0} - 缺少返回结果. 错误原因:", Id));
                logger.Error(result.ResultMessage);
                return;
            }

            var queueList = (List<NotifyQueue>)result.ExtendedParams["queueList"];
            if (queueList == null || queueList.Count == 0)
            {
                logger.Debug(string.Format("任务 {0} - 没有要处理的订单.", Id));
                return;
            }
            var processTime = (DateTime)result.ExtendedParams["processTime"];

            try
            {
                logger.Debug(string.Format("任务 {0} 处理记录数：{1} - {2}.", Id, queueList.Count, DateTimeToString(processTime)));
                queueList.ForEach(q =>
                {
                    logger.Debug(string.Format("任务 {0} 处理订单号{1}, 处理次数{2} - {3}.",
                        Id, q.OrderNumber, q.ProcessedCount, DateTimeToString(processTime)));
                    q.LastSendDate = processTime;
                    q.ProcessedCount = q.ProcessedCount + 1;
                    var parameter = new NotifyAsyncParameter
                    {
                        NotifyPostInfo = new NotifyPostInfo
                        {
                            PostUrl = q.NotifyUrl,
                            PostData = q.PostData,
                            IsRawData = (q.PostDataFormat == NotifyDataFormat.RAW)
                        },
                        NotifyQueue = q,
                        PostAction = TaskService.UpdateNotificationQueue
                    };

                    logger.Debug(string.Format("任务 {0} 处理订单号{1}, 通知发送地址{2}, 发送数据格式{3} - {4}.",
                        Id, q.OrderNumber, q.NotifyUrl, Enum.GetName(typeof(NotifyDataFormat), q.PostDataFormat), DateTimeToString(processTime)));
                    WebClientHelper.SendNotification(parameter);
                    Thread.Sleep(5000);
                });
                logger.Debug(string.Format("任务 {0} 处理结束 - {1}.", Id, DateTimeToString(processTime)));
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("任务 {0} 处理失败 - {1}.", Id, DateTimeToString(processTime)));
                logger.Error("错误原因:");
                logger.Error(ex.Message);
                if (ex.InnerException != null)
                    logger.Error(ex.InnerException.Message);
            }
        }

        private static string DateTimeToString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
