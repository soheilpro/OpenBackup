using System;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    public abstract class FileSystemOperationBase : OperationBase
    {
        protected IFileSystem FileSystem
        {
            get;
            private set;
        }

        public FileSystemOperationBase(IFileSystem fileSystem, IExecutionContext context) : base(context)
        {
            FileSystem = fileSystem;
        }
    }
}
