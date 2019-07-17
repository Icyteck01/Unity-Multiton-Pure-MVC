using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using JHSEngine.Interfaces;
using JHSEngine.Patterns.Observer;

namespace JHSEngine.Core
{
    public class View: IView
    {
        public View(string key)
        {
            if (instanceMap.ContainsKey(key))
                throw new Exception(MULTITON_MSG);

            multitonKey = key;
            instanceMap[key] = this;
            mediatorMap = new ConcurrentDictionary<string, IMediator>();
            observerMap = new ConcurrentDictionary<string, IList<IObserver>>();
            InitializeView();
        }

        protected virtual void InitializeView()
        {
        }

        public static IView GetInstance(string key)
        {
            if (!instanceMap.ContainsKey(key))
            {
                instanceMap[key] = new View(key);
            };
            return instanceMap[key];
        }

        public virtual void RegisterObserver(string notificationName, IObserver observer)
        {
            if (observerMap.TryGetValue(notificationName, out IList<IObserver> observers))
            {
                observers.Add(observer);
            }
            else
            {
                observerMap.TryAdd(notificationName, new List<IObserver> { observer });
            }
        }

        public virtual void NotifyObservers(INotification notification)
        {
            if (observerMap.TryGetValue(notification.Name, out IList<IObserver> observers_ref))
            {
                var observers = new List<IObserver>(observers_ref);
                foreach (IObserver observer in observers)
                {
                    observer.NotifyObserver(notification);
                }
            }
        }

        public virtual void RemoveObserver(string notificationName, object notifyContext)
        {
            if (observerMap.TryGetValue(notificationName, out IList<IObserver> observers))
            {
                for (int i = 0; i < observers.Count; i++)
                {
                    if (observers[i].CompareNotifyContext(notifyContext))
                    {
                        observers.RemoveAt(i);
                        break;
                    }
                }

                if (observers.Count == 0)
                    observerMap.TryRemove(notificationName, out IList<IObserver> _);
            }
        }

        public virtual void RegisterMediator(IMediator mediator)
        {
            if(mediatorMap.TryAdd(mediator.MediatorName, mediator))
            {
                mediator.InitializeNotifier(mediator.MediatorName);
                string[] interests = mediator.ListNotificationInterests();
                if (interests.Length > 0)
                {
                    IObserver observer = new Observer(mediator.HandleNotification, mediator);
                    for (int i = 0; i < interests.Length; i++)
                    {
                        RegisterObserver(interests[i], observer);
                    }
                }
                mediator.OnRegister();
            }
        }


        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return mediatorMap.TryGetValue(mediatorName, out IMediator mediator) ? mediator : null;
        }

        public virtual IMediator RemoveMediator(string mediatorName)
        {
            if (mediatorMap.TryRemove(mediatorName, out IMediator mediator))
            {
                string[] interests = mediator.ListNotificationInterests();
                for (int i = 0; i < interests.Length; i++)
                {
                    RemoveObserver(interests[i], mediator);
                }
                mediator.OnRemove();
            }
            return mediator;
        }


        public virtual bool HasMediator(string mediatorName)
        {
            return mediatorMap.ContainsKey(mediatorName);
        }

        protected readonly ConcurrentDictionary<string, IMediator> mediatorMap;
        protected readonly ConcurrentDictionary<string, IList<IObserver>> observerMap;
        protected string multitonKey;
        protected const string MULTITON_MSG = "View instance for this Multiton key already constructed!";
        protected static ConcurrentDictionary<string, IView> instanceMap = new ConcurrentDictionary<string, IView>();
    }
}
