using System;

namespace OpenBackup.Extension.FileSystem
{
    public class DeleteFileOperation : FileSystemOperationBase, IEquatable<DeleteFileOperation>
    {
        public IFileObject File
        {
            get;
            private set;
        }

        public DeleteFileOperation(IFileObject file, IFileSystem fileSystem, IExecutionContext context) : base(fileSystem, context)
        {
            File = file;
        }

        public override void ExecuteOperation()
        {
            FileSystem.Delete(File);
        }

        public bool Equals(DeleteFileOperation other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (!File.Equals(other.File))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DeleteFileOperation);
        }

        public override int GetHashCode()
        {
            return File.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Delete File: '{0}'", File.Path);
        }
    }
}
