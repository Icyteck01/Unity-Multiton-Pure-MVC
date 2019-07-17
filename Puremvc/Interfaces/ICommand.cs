namespace JHSEngine.Interfaces
{
    public interface ICommand: INotifier
    {
        void Execute(INotification Notification);
    }
}
