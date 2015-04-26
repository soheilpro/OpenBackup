using System;

namespace OpenBackup.Extension.FileSystem
{
    public class DeleteDirectoryOperation : FileSystemOperationBase, IEquatable<DeleteDirectoryOperation>
    {
        public IDirectoryObject Directory
        {
            get;
            private set;
        }

        public DeleteDirectoryOperation(IDirectoryObject directory, IFileSystem fileSystem, IExecutionContext context) : base(fileSystem, context)
        {
            Directory = directory;
        }

        public override void ExecuteOperation()
        {
            FileSystem.Delete(Directory);
        }

        public bool Equals(DeleteDirectoryOperation other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (!Directory.Equals(other.Directory))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DeleteDirectoryOperation);
        }

        public override int GetHashCode()
        {
            return Directory.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Delete Directory: '{0}'", Directory.Path);
        }
    }
}
