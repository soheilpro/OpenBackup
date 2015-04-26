using System;

namespace OpenBackup
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ServiceProviderAttribute : Attribute
    {
        public Type ServiceType
        {
            get;
            private set;
        }

        public ServiceProviderAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }
    }
}
