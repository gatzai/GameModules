using System;

namespace PureMVC.Interface
{
    public interface INotifier
    {
        void SendNotification(string notificationName, object body = null, string type = null);

        void InitializeNotifier(string key);
    }
}
