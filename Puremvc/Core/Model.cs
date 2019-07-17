using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using JHSEngine.Interfaces;

namespace JHSEngine.Core
{
    public class Model: IModel
    {
        public Model(string key)
        {
            if(instanceMap.ContainsKey(key))
            {
                throw new Exception(MULTITON_MSG);
            }
            instanceMap[key] = this;
            multitonKey = key;
            proxyMap = new ConcurrentDictionary<string, IProxy>();
            InitializeModel();
        }

        protected virtual void InitializeModel()
        {
        }

        public static IModel GetInstance(string key)
        {
            if (!instanceMap.ContainsKey(key))
            {
                instanceMap[key] = new Model(key);
            };
            return instanceMap[key];
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            proxyMap[proxy.ProxyName] = proxy;
            proxy.OnRegister();
        }

        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return proxyMap.TryGetValue(proxyName, out IProxy proxy) ? proxy : null;
        }

        public virtual IProxy RemoveProxy(string proxyName)
        {
            if (proxyMap.TryRemove(proxyName, out IProxy proxy))
            {
                proxy.OnRemove();
            }
            return proxy;
        }

        public virtual bool HasProxy(string proxyName)
        {
            return proxyMap.ContainsKey(proxyName);
        }


        protected const string MULTITON_MSG = "Model instance for this Multiton key already constructed!";
        protected static Dictionary<string, IModel> instanceMap = new Dictionary<string, IModel>();
        protected string multitonKey;
        protected readonly ConcurrentDictionary<string, IProxy> proxyMap;
    }
}
