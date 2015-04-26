using System;
using System.Xml.Linq;

namespace OpenBackup.Framework
{
    public abstract class FactoryBase : IFactory
    {
        public abstract T Create<T>(string name, XElement element, ILoadingContext context);
    }
}
