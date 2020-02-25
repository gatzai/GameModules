using System;

namespace PureMVC.Interface
{
    public interface ICommand: INotifier
    {
        void Execute(INotification notification);
    }
}
