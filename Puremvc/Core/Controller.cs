using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using JHSEngine.Interfaces;
using JHSEngine.Patterns.Observer;

namespace JHSEngine.Core
{
    public class Controller : IController
    {
        public Controller(string key)
        {
            if(instanceMap.ContainsKey(key))
            {
                throw new Exception(MULTITON_MSG);
            }

            multitonKey = key;
            instanceMap[multitonKey] = this;
            commandMap = new ConcurrentDictionary<string, ICommand>();
            InitializeController();
        }

        protected virtual void InitializeController()
        {
            if (view != null)
                return;

            view = View.GetInstance(multitonKey);
        }

        public static IController GetInstance(string key)
        {
            if (!instanceMap.ContainsKey(key))
            {
                instanceMap[key] = new Controller(key);
            };
            return instanceMap[key];
        }

        public virtual void ExecuteCommand(INotification notification)
        {
            if (commandMap.TryGetValue(notification.Name, out ICommand commandFunc))
            {
                commandFunc.InitializeNotifier(multitonKey);
                commandFunc.Execute(notification);
            }
        }

        public virtual void RegisterCommand(string notificationName, ICommand commandFunc)
        {
            if (commandMap.TryGetValue(notificationName, out ICommand _) == false)
            {
                view.RegisterObserver(notificationName, new Observer(ExecuteCommand, this));
            }
            commandMap[notificationName] = commandFunc;
        }

        public virtual void RemoveCommand(string notificationName)
        {
            if (commandMap.TryRemove(notificationName, out ICommand _))
            {
                view.RemoveObserver(notificationName, this);
            }
        }

        public virtual bool HasCommand(string notificationName)
        {
            return commandMap.ContainsKey(notificationName);
        }

        protected string multitonKey;
        protected IView view;
        protected readonly ConcurrentDictionary<string, ICommand> commandMap;
        protected const string MULTITON_MSG = "Facade instance for this Multiton key already constructed!";
        protected static Dictionary<string, IController> instanceMap = new Dictionary<string, IController>();
    }
}
