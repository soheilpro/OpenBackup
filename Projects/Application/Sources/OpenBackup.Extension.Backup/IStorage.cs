using System;
using System.Xml.Linq;

namespace OpenBackup.Extension.Backup
{
    public interface IStorage
    {
        IStorageInstance CreateInstance(IExecutionContext context);

        XElement ToXml();
    }
}
