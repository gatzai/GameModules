using System;
using PureMVC.Interface;

namespace PureMVC.Patterns.Observer
{
    public class Observer : IObserver
    {
        public Action<INotification> NotifyMethod { set; get; }

        public object NotifyContext { set; get; }

        public Observer(Action<INotification> notifyMethod, object notifyContext)
        {
            NotifyMethod = notifyMethod;
            NotifyContext = NotifyContext;
        }

        public bool CompareNotifyContext(object obj)
        {
            return NotifyContext.Equals(obj);
        }

        public void NotifyObserver(INotification notification)
        {
            NotifyMethod(notification);
        }
    }
}