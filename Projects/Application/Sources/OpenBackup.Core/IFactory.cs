using System;
using System.Xml.Linq;

namespace OpenBackup
{
    public interface IFactory
    {
        T Create<T>(string name, XElement element, ILoadingContext context);
    }
}
