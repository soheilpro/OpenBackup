using System;
using System.Xml.Linq;

namespace OpenBackup.Extension.Backup
{
    public abstract class SourceBase : ISource
    {
        protected SourceBase()
        {
        }

        protected SourceBase(XElement element, ILoadingContext context)
        {
        }

        public abstract ISourceInstance CreateInstance(IExecutionContext context);

        public abstract XElement ToXml();

        public virtual void FillXml(XElement element)
        {
        }
    }
}
