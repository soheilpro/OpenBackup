using System;

namespace OpenBackup.Extension.FileSystem
{
    public class RenameFileOperation : FileSystemOperationBase, IEquatable<RenameFileOperation>
    {
        public IFileObject File
        {
            get;
            private set;
        }

        public string NewName
        {
            get;
            private set;
        }

        public RenameFileOperation(IFileObject file, string newName, IFileSystem fileSystem, IExecutionContext context) : base(fileSystem, context)
        {
            File = file;
            NewName = newName;
        }

        public override void ExecuteOperation()
        {
            FileSystem.Rename(File, NewName);
        }

        public bool Equals(RenameFileOperation other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (!File.Equals(other.File))
                return false;

            if (!NewName.Equals(other.NewName))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RenameFileOperation);
        }

        public override int GetHashCode()
        {
            return File.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Rename File: '{0}' to '{1}'", File.Path, NewName);
        }
    }
}
