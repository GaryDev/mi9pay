using Mi9Pay.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mi9Pay.Service
{
    public interface INotifyTaskService
    {
        List<NotifyQueue> GetNotificationQueue(DateTime processTime);
        void UpdateNotificationQueue(NotifyQueue queue);
    }
}
