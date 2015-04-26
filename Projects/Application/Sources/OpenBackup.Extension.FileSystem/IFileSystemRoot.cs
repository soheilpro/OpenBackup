using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenBackup.Extension.FileSystem
{
    public interface IFileSystemRoot
    {
        IEnumerable<IFileSystemObject> GetObjects(IExecutionContext context);

        XElement ToXml();
    }
}
