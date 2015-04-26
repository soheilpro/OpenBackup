using System;
using System.Collections.Generic;
using System.IO;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    public class FileObject : FileSystemObjectBase, IFileObject
    {
        public FileAttributes Attributes
        {
            get
            {
                return FileSystem.GetAttributes(this);
            }
        }

        public long Length
        {
            get
            {
                return FileSystem.GetFileLength(this);
            }
        }

        public IDriveObject Drive
        {
            get
            {
                return new DriveObject(Path[0], FileSystem);
            }
        }

        public DateTime CreationTimeUtc
        {
            get
            {
                return FileSystem.GetCreationTimeUtc(this);
            }
        }

        public DateTime LastWriteTimeUtc
        {
            get
            {
                return FileSystem.GetLastWriteTimeUtc(this);
            }
        }

        public DateTime LastAccessTimeUtc
        {
            get
            {
                return FileSystem.GetLastAccessTimeUtc(this);
            }
        }

        public FileObject(FileSystemPath path, IFileSystem fileSystem) : base(path, fileSystem)
        {
        }

        public FileObject(string path, IFileSystem fileSystem) : this(FileSystemPath.Parse(path), fileSystem)
        {
        }

        public override IEnumerable<IMetadata> GetMetadata()
        {
            foreach (var metadata in base.GetMetadata())
                yield return metadata;

            yield return new Metadata("Length", Length);
            yield return new Metadata("Attributes", Attributes);
            yield return new Metadata("CreationTimeUtc", CreationTimeUtc);
            yield return new Metadata("LastWriteTimeUtc", LastWriteTimeUtc);
            yield return new Metadata("LastAccessTimeUtc", LastAccessTimeUtc);
        }
    }
}
