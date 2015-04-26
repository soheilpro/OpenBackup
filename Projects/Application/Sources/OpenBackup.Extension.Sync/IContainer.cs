using System;
using System.Xml.Linq;

namespace OpenBackup.Extension.Sync
{
    public interface IContainer
    {
        IContainerInstance CreateInstance(IExecutionContext context);

        XElement ToXml();
    }
}
