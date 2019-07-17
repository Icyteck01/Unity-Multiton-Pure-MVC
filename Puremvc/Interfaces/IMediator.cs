using System;

namespace JHSEngine.Interfaces
{
    public interface IMediator: INotifier
    {
        string MediatorName { get; }
        object ViewComponent { get; }
        string[] ListNotificationInterests();
        void HandleNotification(INotification notification);
        void OnRegister();
        void OnRemove();
    }
}
