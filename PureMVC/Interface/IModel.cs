using System;

namespace PureMVC.Interface
{
    public interface IModel
    {
        void RegisterProxy(IProxy proxy);

        IProxy RetrieveProxy(string proxyName);

        IProxy RemoveProxy(string proxyName);

        bool hasProxy(string proxyName);
    }
}
