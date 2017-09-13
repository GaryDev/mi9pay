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
        public NotifyTaskBundle(int interval) 
            : base(interval)
        {

        }

        protected override TaskExecution GetTask()
        {
            return new NotifyTaskExecution();
        }
    }
}
