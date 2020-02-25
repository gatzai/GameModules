using PureMVC.Patterns.Observer;
using PureMVC.Interface;

namespace PureMVC.Patterns.Proxy
{
    public class Proxy: Notifier, IProxy, INotifier
    {
        public static string Name = "Proxy";

        public string ProxyName { get; protected set; }

        public object Data { get; set; }

        public Proxy(string proxyName, object data = null)
        {
            ProxyName = proxyName ?? Proxy.Name;
            if (data != null) Data = data;
        }

        public virtual void OnRegister()
        {

        }

        public virtual void OnRemove()
        {

        }
    }

}
