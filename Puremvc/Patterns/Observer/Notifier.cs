using JHSEngine.Interfaces;
using UnityEngine;

namespace JHSEngine.Patterns.Observer
{
    public class Notifier : INotifier
    {
        protected string multitonKey;

        public virtual void SendNotification(string notificationName, object body, string type)
        {
            facade.SendNotification(notificationName, body, type);
        }

        public void InitializeNotifier(string key)
        {
            multitonKey = key;
        }

        protected IFacade facade
        {
            get
            {
                return JHSEngine.Patterns.Facade.Facade.GetInstance(multitonKey);
            }
        }
    }
}
