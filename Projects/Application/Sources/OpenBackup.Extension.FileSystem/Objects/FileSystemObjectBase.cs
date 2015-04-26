using System;
using System.Collections.Generic;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    public abstract class FileSystemObjectBase : ObjectBase, IFileSystemObject, IEquatable<IFileSystemObject>
    {
        public FileSystemPath Path
        {
            get;
            private set;
        }

        protected IFileSystem FileSystem
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return Path[Path.PartCount - 1];
            }
        }

        public string ActualName
        {
            get
            {
                return FileSystem.GetActualPath(Path).Name;
            }
        }

        public bool Exists
        {
            get
            {
                return FileSystem.Exists(this);
            }
        }

        public virtual IFileSystemObject Parent
        {
            get
            {
                if (Path.PartCount == 2)
                    return new DriveObject(Path.Parent, FileSystem);
                else
                    return new DirectoryObject(Path.Parent, FileSystem);
            }
        }

        public FileSystemObjectBase(FileSystemPath path, IFileSystem fileSystem)
        {
            Path = path;
            FileSystem = fileSystem;
        }

        public FileSystemObjectBase(string path, IFileSystem fileSystem)
        {
            Path = FileSystemPath.Parse(path);
            FileSystem = fileSystem;
        }

        public bool Equals(IFileSystemObject other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (GetType() != other.GetType())
                return false;

            if (Path != other.Path)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IFileSystemObject);
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        public virtual IEnumerable<IMetadata> GetMetadata()
        {
            yield return new Metadata("Path", Path);
        }
    }
}
