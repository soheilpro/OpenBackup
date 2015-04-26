using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Runtime
{
    [ServiceProvider(typeof(IFactory))]
    internal class LoadableTypesFactory : FactoryBase
    {
        private List<LoadableType> _loadableTypes;

        public LoadableTypesFactory(IServiceContainer serviceContainer)
        {
            var loadableTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                from type in assembly.GetTypes()
                                let loadableAttribute = Attribute.GetCustomAttribute(type, typeof(LoadableAttribute)) as LoadableAttribute
                                where loadableAttribute != null
                                select new LoadableType(loadableAttribute.Name, type);

            _loadableTypes = new List<LoadableType>(loadableTypes);
        }

        public override T Create<T>(string name, XElement element, ILoadingContext context)
        {
            foreach (var loadableType in _loadableTypes)
            {
                if (!string.Equals(name, loadableType.Name))
                    continue;

                if (!typeof(T).IsAssignableFrom(loadableType.Type))
                    continue;

                return (T)Activator.CreateInstance(loadableType.Type, element, context);
            }

            return default(T);
        }

        private class LoadableType
        {
            public string Name
            {
                get;
                private set;
            }

            public Type Type
            {
                get;
                private set;
            }

            public LoadableType(string name, Type type)
            {
                Name = name;
                Type = type;
            }
        }
    }
}
