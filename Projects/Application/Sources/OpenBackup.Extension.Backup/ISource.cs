using System;
using System.Xml.Linq;

namespace OpenBackup.Extension.Backup
{
    public interface ISource
    {
        ISourceInstance CreateInstance(IExecutionContext context);

        XElement ToXml();
    }
}
