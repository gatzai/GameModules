using System;

namespace PureMVC.Interface
{
    public interface IFacade:INotifier
    {
        void RegisterProxy(IProxy proxy);

        void RegisterCommand(string notificationName, Func<ICommand> commandFunc);

        void RegisterMediator(IMediator mediator);



        IProxy RetrieveProxy(string proxyName);

        IMediator RetrieveMediator(string mediatorName);



        IProxy RemoveProxy(string proxyName);

        void RemoveCommand(string notificationName);

        IMediator RemoveMediator(string mediatorName);


        bool HasProxy(string proxyName);

        bool HasCommand(string notificationName);

        bool HasMediator(string mediatorName);



        void NotifyObserver(INotification notification);
    }
}
