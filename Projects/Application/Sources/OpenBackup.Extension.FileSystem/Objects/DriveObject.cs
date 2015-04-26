using System;
using System.Collections.Generic;
using System.IO;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    public class DriveObject : FileSystemObjectBase, IDriveObject
    {
        public override IFileSystemObject Parent
        {
            get
            {
                return null;
            }
        }

        public DriveType DriveType
        {
            get
            {
                return FileSystem.GetDriveType(this);
            }
        }

        public bool SupportsHardLinks
        {
            get
            {
                return FileSystem.SupportsHardLinks(this);
            }
        }

        public IDirectoryObject Root
        {
            get
            {
                return new DirectoryObject(Path + @"\", FileSystem);
            }
        }

        public DriveObject(string path, IFileSystem fileSystem) : base(FileSystemPath.Parse(path), fileSystem)
        {
        }

        public override IEnumerable<IMetadata> GetMetadata()
        {
            foreach (var metadata in base.GetMetadata())
                yield return metadata;

            yield return new Metadata("DriveType", DriveType);
        }
    }
}
