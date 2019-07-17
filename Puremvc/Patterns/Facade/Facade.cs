using System;
using JHSEngine.Interfaces;
using JHSEngine.Core;
using JHSEngine.Patterns.Observer;
using System.Collections.Generic;

namespace JHSEngine.Patterns.Facade
{
    public class Facade : IFacade
    {
        public Facade(string key)
        {
            if(instanceMap.ContainsKey(key))
                throw new Exception(MULTITON_MSG);
            multitonKey = key;
            instanceMap[key] = this;
            InitializeFacade();
        }

        protected virtual void InitializeFacade()
        {
            InitializeModel();
            InitializeController();
            InitializeView();
        }

        public static IFacade GetInstance(string key)
        {
            if (!instanceMap.ContainsKey(key)){
                instanceMap[key] = new Facade(key);
            };
            return instanceMap[key];
        }

        protected virtual void InitializeController()
        {
            if (controller != null)
            {
                return;
            }
            controller = Controller.GetInstance(multitonKey);
        }

        protected virtual void InitializeModel()
        {
            if(model != null)
            {
                return;
            }
            model = Model.GetInstance(multitonKey);
        }

        protected virtual void InitializeView()
        {
            if (view != null)
                return;

            view = View.GetInstance(multitonKey);
        }

        public virtual void RegisterCommand(string notificationName, ICommand commandFunc)
        {
            controller.RegisterCommand(notificationName, commandFunc);           
        }

        public virtual void RemoveCommand(string notificationName)
        {
            controller.RemoveCommand(notificationName);
        }

        public virtual bool HasCommand(string notificationName)
        {
            return controller.HasCommand(notificationName);
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            model.RegisterProxy(proxy);
        }

        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return model.RetrieveProxy(proxyName);
        }

        public virtual IProxy RemoveProxy(string proxyName)
        {
            return model.RemoveProxy(proxyName);
        }

        public virtual bool HasProxy(string proxyName)
        {
            return model.HasProxy(proxyName);
        }

        public virtual void RegisterMediator(IMediator mediator)
        {
            view.RegisterMediator(mediator);
        }

        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            return view.RetrieveMediator(mediatorName);
        }
        
        public virtual IMediator RemoveMediator(string mediatorName)
        {
            return view.RemoveMediator(mediatorName);
        }
        
        public virtual bool HasMediator(string mediatorName)
        {
            return view.HasMediator(mediatorName);
        }
        
        public virtual void SendNotification(string notificationName, object body=null, string type = null)
        {
            NotifyObservers(new Notification(notificationName, body, type));
        }
        
        public virtual void NotifyObservers(INotification notification)
        {
            view.NotifyObservers(notification);
        }

        public virtual void InitializeNotifier(string key)
        {
            multitonKey = key;
        }

        protected string multitonKey;
        protected IController controller;
        protected IModel model;
        protected IView view;
        protected const string MULTITON_MSG = "Facade instance for this Multiton key already constructed!";
        protected static Dictionary<string, IFacade> instanceMap = new Dictionary<string, IFacade>();
    }
}
