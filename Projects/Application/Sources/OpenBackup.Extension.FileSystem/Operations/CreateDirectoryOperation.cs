using System;

namespace OpenBackup.Extension.FileSystem
{
    public class CreateDirectoryOperation : FileSystemOperationBase, IEquatable<CreateDirectoryOperation>
    {
        public IDirectoryObject Directory
        {
            get;
            private set;
        }

        public CreateDirectoryOperation(IDirectoryObject directory, IFileSystem fileSystem, IExecutionContext context) : base(fileSystem, context)
        {
            Directory = directory;
        }

        public override void ExecuteOperation()
        {
            FileSystem.CreateDirectory(Directory.Path);
        }

        public bool Equals(CreateDirectoryOperation other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (!Directory.Equals(other.Directory))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CreateDirectoryOperation);
        }

        public override int GetHashCode()
        {
            return Directory.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Create Directory: '{0}'", Directory.Path);
        }
    }
}
