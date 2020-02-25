using System;

namespace PureMVC.Interface
{
    public interface IProxy:INotifier
    {
        string ProxyName { get; }

        object Data { get; set; }

        void OnRegister();

        void OnRemove();
    }
}
