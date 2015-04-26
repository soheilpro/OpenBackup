using System;
using System.Xml.Linq;

namespace OpenBackup.Extension.Sync
{
    public abstract class ContainerBase : IContainer
    {
        protected ContainerBase()
        {
        }

        protected ContainerBase(XElement element, ILoadingContext context)
        {
        }

        public abstract IContainerInstance CreateInstance(IExecutionContext context);

        public abstract XElement ToXml();

        public virtual void FillXml(XElement element)
        {
        }
    }
}
