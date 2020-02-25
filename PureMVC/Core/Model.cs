using System;
using System.Collections.Concurrent;
using PureMVC.Interface;


namespace PureMVC.Core
{
    public class Model:IModel
    {

        protected string multitonKey;

        protected readonly ConcurrentDictionary<string, IProxy> proxyMap;

        protected const string MULTITON_MSG = "Model instance for this Multiton key already constructed!";

        public static readonly ConcurrentDictionary<string, Lazy<IModel>> instanceMap = new ConcurrentDictionary<string, Lazy<IModel>>();

        public Model(string key)
        {
            if (instanceMap.ContainsKey(key) && multitonKey != null) throw new Exception(MULTITON_MSG);
            multitonKey = key;
            instanceMap.TryAdd(key, new Lazy<IModel>(() => this));
            proxyMap = new ConcurrentDictionary<string, IProxy>();
            InitializeModel();
        }

        void InitializeModel()
        {

        }

        public static IModel GetInstance(string key, Func<IModel> modelFunc)
        {
            return instanceMap.GetOrAdd(key, new Lazy<IModel>(modelFunc)).Value;
        }

        public virtual bool hasProxy(string proxyName)
        {
            return proxyMap.ContainsKey(proxyName);
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            proxy.InitializeNotifier(multitonKey);
            proxyMap[proxy.ProxyName] = proxy;
            proxy.OnRegister();
        }

        public virtual IProxy RemoveProxy(string proxyName)
        {
            if(proxyMap.TryRemove(proxyName, out IProxy proxy))
            {
                proxy.OnRemove();
            }
            return proxy;
        }

        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return proxyMap.TryGetValue(proxyName, out IProxy proxy) ? proxy : null;
        }

        public static void RemoveModel(string key)
        {
            instanceMap.TryRemove(key, out Lazy<IModel> _);
        }
    }
}
