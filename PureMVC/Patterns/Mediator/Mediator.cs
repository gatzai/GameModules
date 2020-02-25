using PureMVC.Interface;
using PureMVC.Patterns.Observer;

namespace PureMVC.Patterns.Mediator
{
    public class Mediator:Notifier, IMediator,INotifier
    {
        public static string Name = "Mediator";

        public string MediatorName { get; protected set; }

        public object ViewComponent { get; set; }

        public Mediator(string mediatorName, object viewComponent = null)
        {
            MediatorName = mediatorName ?? Name;
            ViewComponent = viewComponent;
        }

        //返回要响应的Notification的名字字符串数组
        public virtual string[] ListNotificationInterests()
        {
            return new string[0];
        }

        public virtual void HandleNotification(INotification notification)
        {

        }

        public virtual void OnRegister()
        {

        }

        public virtual void OnRemove()
        {

        }

    }

}
