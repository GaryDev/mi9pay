using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler;

namespace Mi9Pay.PayNotifyTask
{
    public class NotifyTaskBundle : TaskBundle
    {
        protected override int GetInterval()
        {
            return 1;
        }

        protected override ITaskExecution GetTask()
        {
            ITaskExecution task = new NotifyTaskExecution();
            return task;
        }
    }
}
