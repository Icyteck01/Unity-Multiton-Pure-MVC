using System;

namespace JHSEngine.Interfaces
{
    public interface IController
    {
        void RegisterCommand(string notificationName, ICommand commandClassRef);
        void ExecuteCommand(INotification notification);
        void RemoveCommand(string notificationName);
        bool HasCommand(string notificationName);
    }
}
