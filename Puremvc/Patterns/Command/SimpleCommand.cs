using JHSEngine.Interfaces;
using JHSEngine.Patterns.Observer;

namespace JHSEngine.Patterns.Command
{
    public class SimpleCommand : Notifier, ICommand, INotifier
    {
        public virtual void Execute(INotification notification)
        {
        }
    }
}
