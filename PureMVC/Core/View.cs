using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using PureMVC.Interface;
using PureMVC.Patterns.Observer;

namespace PureMVC.Core
{
    public class View:IView
    {
        protected string multitonKey;

        protected ConcurrentDictionary<string, IMediator> mediatorMap;

        protected ConcurrentDictionary<string, IList<IObserver>> observerMap;

        protected static ConcurrentDictionary<string, Lazy<IView>> instanceMap = new ConcurrentDictionary<string, Lazy<IView>>();

        protected const string MULTITON_MSG = "View instance for this Multiton key already constructed!";

        public View(string key)
        {
            if (instanceMap.TryGetValue(key, out Lazy<IView> _) && multitonKey != null) throw new Exception(MULTITON_MSG);
            multitonKey = key;
            instanceMap.TryAdd(key, new Lazy<IView>(() => this));
            mediatorMap = new ConcurrentDictionary<string, IMediator>();
            observerMap = new ConcurrentDictionary<string, IList<IObserver>>();
            InitializeView();
        }

        protected virtual void InitializeView()
        {

        }

        public static IView GetInstance(string key, Func<IView> viewFunc)
        {
            return instanceMap.GetOrAdd(key, new Lazy<IView>(viewFunc)).Value;
        }

        public virtual void RegisterObserver(string notificationName, IObserver observer)
        {
            if(observerMap.TryGetValue(notificationName, out IList<IObserver> observers))
            {
                observers.Add(observer);
            }
            else
            {
                observerMap.TryAdd(notificationName, new List<IObserver> { observer});
            }
        }

        public virtual void NotifyObservers(INotification notification)
        {
            if(observerMap.TryGetValue(notification.Name, out IList<IObserver> observersRef))
            {
                var observers = new List<IObserver>(observersRef);
                foreach(IObserver observer in observers)
                {
                    observer.NotifyObserver(notification);
                }
            }
        }

        public virtual void RemoveObserver(string notificationName, object notifyContext)
        {
            if (observerMap.TryGetValue(notificationName, out IList<IObserver> observers))
            {
                for (int i = 0; i < observers.Count; ++i)
                {
                    if(observers[i].CompareNotifyContext(notifyContext))
                    {
                        observers.RemoveAt(i);
                        break;
                    }
                }

                //如果没有Observer，就直接删掉这个notification
                if(observers.Count == 0)
                {
                    observerMap.TryRemove(notificationName, out IList<IObserver> _);
                }
            }
        }

        public virtual void RegisterMediator(IMediator mediator)
        {
            if(mediatorMap.TryAdd(mediator.MediatorName, mediator))
            {
                mediator.InitializeNotifier(multitonKey);

                string[] interests = mediator.ListNotificationInterests();

                if(interests.Length > 0)
                {
                    IObserver observer = new Observer(mediator.HandleNotification, mediator);
                    for (int i = 0; i < interests.Length; ++i)
                    {
                        RegisterObserver(interests[i], observer);
                    }
                }
                mediator.OnRegister();
            }
        }

        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return mediatorMap.TryGetValue(mediatorName, out IMediator mediator) ? mediator : null;
        }

        public virtual IMediator RemoveMediator(string mediatorName)
        {
            if(mediatorMap.TryRemove(mediatorName, out IMediator mediator))
            {
                string[] interests = mediator.ListNotificationInterests();
                for(int i = 0; i < interests.Length; ++i)
                {
                    RemoveObserver(interests[i], mediator);
                }
                mediator.OnRemove();
            }
            return mediator;
        }

        public virtual bool HasMediator(string mediatorName)
        {
            return mediatorMap.ContainsKey(mediatorName);
        }

        public static void RemoveView(string key)
        {
            instanceMap.TryRemove(key, out Lazy<IView> _);

        }
    }
}
