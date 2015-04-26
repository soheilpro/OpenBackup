using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenBackup.Extension.FileSystem
{
    public abstract class FileSystemRootBase : IFileSystemRoot
    {
        protected IFileSystem FileSystem
        {
            get;
            private set;
        }

        public FileSystemRootBase(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        public abstract IEnumerable<IFileSystemObject> GetObjects(IExecutionContext context);

        public abstract XElement ToXml();
    }
}
