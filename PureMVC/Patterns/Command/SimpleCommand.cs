using PureMVC.Interface;
using PureMVC.Patterns.Observer;

namespace PureMVC.Patterns.Command
{
    public class SimpleCommand:Notifier, ICommand, INotifier
    {
        public SimpleCommand()
        {
        }

        public virtual void Execute(INotification notification)
        {
        }
    }

}
