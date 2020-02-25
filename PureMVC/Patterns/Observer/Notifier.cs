using System;
using PureMVC.Interface;

namespace PureMVC.Patterns.Observer
{
    public class Notifier : INotifier
    {
        protected IFacade Facade
        {
            get
            {
                if (MultitonKey == null) throw new Exception(MULTITON_MSG);
                return Patterns.Facade.Facade.GetInstance(MultitonKey, ()=>new Facade.Facade(MultitonKey));
            }
        }

        protected string MULTITON_MSG = "multitonKey for this Notifier not yet initialized!";

        public string MultitonKey { get; protected set; }

        public virtual void SendNotification(string notificationName, object body = null, string type = null)
        {
            Facade.SendNotification(notificationName, body, type);
        }

        public void InitializeNotifier(string key)
        {
            MultitonKey = key;
        }
    }
}
