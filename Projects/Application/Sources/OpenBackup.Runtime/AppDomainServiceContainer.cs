using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpenBackup.Runtime
{
    internal class AppDomainServiceContainer : IServiceContainer
    {
        private Hashtable _services = new Hashtable();

        public AppDomainServiceContainer()
        {
            var serviceProviderTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                       from type in assembly.GetTypes()
                                       let serviceProviderAttributes = Attribute.GetCustomAttributes(type, typeof(ServiceProviderAttribute))
                                       where serviceProviderAttributes.Length > 0
                                       select new
                                       {
                                           Type = type,
                                           Attributes = serviceProviderAttributes.Cast<ServiceProviderAttribute>()
                                       };

            foreach (var serviceProviderType in serviceProviderTypes)
            {
                var serviceProvider = Activator.CreateInstance(serviceProviderType.Type, this);

                foreach (var serviceProviderAttribute in serviceProviderType.Attributes)
                {
                    var list = (List<object>)_services[serviceProviderAttribute.ServiceType];

                    if (list == null)
                    {
                        list = new List<object>();
                        _services[serviceProviderAttribute.ServiceType] = list;
                    }

                    list.Add(serviceProvider);
                }
            }
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
