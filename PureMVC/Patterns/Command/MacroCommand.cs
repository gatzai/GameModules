using System;
using System.Collections.Generic;
using PureMVC.Interface;
using PureMVC.Patterns.Observer;

namespace PureMVC.Patterns.Command
{
    public class MacroCommand : Notifier, ICommand, INotifier
    {
        public IList<Func<ICommand>> subcommands;

        public MacroCommand()
        {
            subcommands = new List<Func<ICommand>>();
            InitializeMacroCommand();
        }

        protected virtual void InitializeMacroCommand()
        {

        }

        protected virtual void AddSubCommand(Func<ICommand> commandFunc)
        {
            subcommands.Add(commandFunc);
        }

        public virtual void Execute(INotification notification)
        {
            while(subcommands.Count > 0)
            {
                Func<ICommand> commandFunc = subcommands[0];
                ICommand commandInstance = commandFunc();

                commandInstance.InitializeNotifier(MultitonKey);
                commandInstance.Execute(notification);
                subcommands.RemoveAt(0);
            }
        }
    }
}
