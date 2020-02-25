using System.Collections.Concurrent;
using PureMVC.Interface;
using PureMVC.Core;
using PureMVC.Patterns.Observer;
using System;

namespace PureMVC.Patterns.Facade
{
    public class Facade:IFacade
    {

        protected IController controller;

        protected IModel model;

        protected IView view;

        protected string multitonKey;

        protected static ConcurrentDictionary<string, Lazy<IFacade>> instanceMap = new ConcurrentDictionary<string, Lazy<IFacade>>();

        protected const string MULTITON_MSG = "View instance for this Multiton key already constructed!";


        public Facade(string key)
        {
            if (instanceMap.TryGetValue(key, out Lazy<IFacade> facade) && multitonKey != null) throw new Exception(MULTITON_MSG);

            InitializeNotifier(key);
            instanceMap.TryAdd(key, new Lazy<IFacade>(() => this));
            InitializeFacade();
        }
        
        public virtual void InitializeNotifier(string key)
        {
            multitonKey = key;
        }

        protected virtual void InitializeFacade()
        {
            InitializeModel();
            InitializeController();
            InitializeView();
        }

        public static IFacade GetInstance(string key, Func<IFacade> facadeFunc)
        {
            return instanceMap.GetOrAdd(key, new Lazy<IFacade>(facadeFunc)).Value;
        }

        protected virtual void InitializeController()
        {
            controller = Controller.GetInstance(multitonKey, ()=> new Controller(multitonKey));
        }

        protected virtual void InitializeModel()
        {
            model = Model.GetInstance(multitonKey, () => new Model(multitonKey));
        }

        protected virtual void InitializeView()
        {
            view = View.GetInstance(multitonKey, () => new View(multitonKey));
        }



        public virtual void RegisterCommand(string notificationName, Func<ICommand> commandFunc)
        {
            controller.RegisterCommand(notificationName, commandFunc);
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            model.RegisterProxy(proxy);
        }

        public virtual void RegisterMediator(IMediator mediator)
        {
            view.RegisterMediator(mediator);
        }



        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return model.RetrieveProxy(proxyName);
        }

        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return view.RetrieveMediator(mediatorName);
        }



        public virtual void RemoveCommand(string notificationName)
        {
            controller.RemoveCommand(notificationName);
        }

        public virtual IProxy RemoveProxy(string proxyName)
        {
            return model.RemoveProxy(proxyName);
        }

        public virtual IMediator RemoveMediator(string mediatorName)
        {
            return view.RemoveMediator(mediatorName);
        }



        public virtual bool HasCommand(string notificationName)
        {
            return controller.HasCommand(notificationName);
        }

        public virtual bool HasProxy(string notificationName)
        {
            return model.hasProxy(notificationName);
        }

        public virtual bool HasMediator(string notificationName)
        {
            return view.HasMediator(notificationName);
        }

        public virtual void SendNotification(string notificationName, object body = null, string type = null)
        {
            NotifyObserver(new Notification(notificationName,body, type));
        }

        public virtual void NotifyObserver(INotification notification)
        {
            view.NotifyObservers(notification);
        }

        public static bool HasCore(string key)
        {
            return instanceMap.TryGetValue(key,out Lazy<IFacade> _);
        }

        public static void RemoveCore(string key)
        {
            if (instanceMap.TryGetValue(key, out Lazy<IFacade> _) == false)
                return;
            else
            {
                Model.RemoveModel(key);
                View.RemoveView(key);
                Controller.RemoveController(key);
                instanceMap.TryRemove(key, out Lazy<IFacade> _);
            }
        }


    }

}

