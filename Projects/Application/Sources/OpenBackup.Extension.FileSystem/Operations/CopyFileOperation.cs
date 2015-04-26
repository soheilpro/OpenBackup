using System;

namespace OpenBackup.Extension.FileSystem
{
    public class CopyFileOperation : FileSystemOperationBase, IEquatable<CopyFileOperation>
    {
        public IFileObject Source
        {
            get;
            private set;
        }

        public IFileObject Destination
        {
            get;
            private set;
        }

        public CopyFileOperation(IFileObject source, IFileObject destination, IFileSystem fileSystem, IExecutionContext context) : base(fileSystem, context)
        {
            Source = source;
            Destination = destination;
        }

        public override void ExecuteOperation()
        {
            FileSystem.CopyFile(Source.Path, Destination.Path);
        }

        public bool Equals(CopyFileOperation other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (!Source.Equals(other.Source))
                return false;

            if (!Destination.Equals(other.Destination))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CopyFileOperation);
        }

        public override int GetHashCode()
        {
            // TODO
            return Source.GetHashCode() ^ Destination.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Copy File: '{0}' to '{1}'", Source.Path, Destination.Path);
        }
    }
}
