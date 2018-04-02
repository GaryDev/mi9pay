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
}
