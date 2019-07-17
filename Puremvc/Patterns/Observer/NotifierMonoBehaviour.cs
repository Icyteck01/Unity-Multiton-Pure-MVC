using JHSEngine.Interfaces;
using UnityEngine;

namespace JHSEngine.Patterns.Observer
{
    public class NotifierMonoBehaviour : MonoBehaviour, INotifier
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

        protected IFacade facade => JHSEngine.Patterns.Facade.Facade.GetInstance(multitonKey);
    }
}
