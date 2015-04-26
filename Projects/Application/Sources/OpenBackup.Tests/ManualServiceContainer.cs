using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpenBackup.Tests
{
    public class ManualServiceContainer : IServiceContainer
    {
        private Hashtable _services = new Hashtable();

        public void Register<T>(T service)
        {
            var list = (List<object>)_services[typeof(T)];

            if (list == null)
            {
                list = new List<object>();
                _services[typeof(T)] = list;
            }

            list.Add(service);
        }

        public T Get<T>()
        {
            var services = (List<object>)_services[typeof(T)];

            if (services == null)
                return default(T);

            return (T)services.FirstOrDefault();
        }

        public T[] GetAll<T>()
        {
            var services = (List<object>)_services[typeof(T)];

            if (services == null)
                return new T[0];

            return services.Cast<T>().ToArray();
        }
    }
}
