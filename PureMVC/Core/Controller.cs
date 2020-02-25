using System;
using System.Collections.Concurrent;
using PureMVC.Interface;
using PureMVC.Patterns.Observer;

namespace PureMVC.Core
{
    public class Controller : IController
    {
        protected IView view;

        protected string multitonKey;

        protected ConcurrentDictionary<string, Func<ICommand>> commandMap;

        protected static ConcurrentDictionary<string, Lazy<IController>> instanceMap = new ConcurrentDictionary<string, Lazy<IController>>();

        protected const string MULTITON_MSG = "Controller instance for this Multiton key already constructed!";


        public Controller(string key)
        {
            if (instanceMap.TryGetValue(key, out Lazy<IController> _) && multitonKey != null) throw new Exception(MULTITON_MSG);
            multitonKey = key;
            instanceMap.TryAdd(multitonKey, new Lazy<IController>(() => this));
            commandMap = new ConcurrentDictionary<string, Func<ICommand>>();
            InitializeController();
        }

        void InitializeController()
        {
            view = View.GetInstance(multitonKey, () => new View(multitonKey));

        }

        public static IController GetInstance(string key, Func<IController> controllerFunc)
        {
            return instanceMap.GetOrAdd(key, (k) => new Lazy<IController>(controllerFunc)).Value;
        }

        public virtual void ExecuteCommand(INotification notification)
        {
            if(commandMap.TryGetValue(notification.Name, out Func<ICommand> commandFunc))
            {
                ICommand commandInstance = commandFunc();
                commandInstance.InitializeNotifier(multitonKey);
                commandInstance.Execute(notification);
            }
        }


        public virtual bool HasCommand(string notificationName)
        {
            return commandMap.ContainsKey(notificationName);
        }

        public virtual void RegisterCommand(string notificationName, Func<ICommand> commandFunc)
        {
            if(commandMap.TryGetValue(notificationName, out Func<ICommand> _) == false)
            {
                view.RegisterObserver(notificationName, new Observer(ExecuteCommand, this));
            }

            commandMap[notificationName] = commandFunc;
        }

        public virtual void RemoveCommand(string notificationName)
        {
            if(commandMap.TryRemove(notificationName, out Func<ICommand> _))
            {
                view.RemoveObserver(notificationName, this);
            }
        }

        public static void RemoveController(string key)
        {
            instanceMap.TryRemove(key, out Lazy<IController> _);
        }
    }
}

