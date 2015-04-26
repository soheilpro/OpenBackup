using System;
using System.Xml.Linq;

namespace OpenBackup.Extension.Backup
{
    public abstract class StorageBase : IStorage
    {
        protected StorageBase()
        {
        }

        protected StorageBase(XElement element, ILoadingContext context)
        {
        }

        public abstract IStorageInstance CreateInstance(IExecutionContext context);

        public abstract XElement ToXml();

        public virtual void FillXml(XElement element)
        {
        }
    }
}
