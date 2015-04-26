using System;

namespace OpenBackup.Extension.FileSystem
{
    public class RenameDirectoryOperation : FileSystemOperationBase, IEquatable<RenameDirectoryOperation>
    {
        public IDirectoryObject Directory
        {
            get;
            private set;
        }

        public string NewName
        {
            get;
            private set;
        }

        public RenameDirectoryOperation(IDirectoryObject directory, string newName, IFileSystem fileSystem, IExecutionContext context) : base(fileSystem, context)
        {
            Directory = directory;
            NewName = newName;
        }

        public override void ExecuteOperation()
        {
            FileSystem.Rename(Directory, NewName);
        }

        public bool Equals(RenameDirectoryOperation other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (!Directory.Equals(other.Directory))
                return false;

            if (!NewName.Equals(other.NewName))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RenameDirectoryOperation);
        }

        public override int GetHashCode()
        {
            return Directory.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Rename Directory: '{0}' to '{1}'", Directory.Path, NewName);
        }
    }
}
