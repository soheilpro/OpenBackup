using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    [Loadable("FixedDrives")]
    public class FixedDrivesRoot : FileSystemRootBase
    {
        public FixedDrivesRoot(IFileSystem fileSystem) : base(fileSystem)
        {
        }

        public FixedDrivesRoot(XElement element, ILoadingContext context) : this(context.ServiceContainer.Get<IFileSystem>())
        {
        }

        public override IEnumerable<IFileSystemObject> GetObjects(IExecutionContext context)
        {
            foreach (var drive in FileSystem.GetDrives().Where(drive => drive.DriveType == DriveType.Fixed))
            {
                var rootDirectory = drive.Root;

                yield return rootDirectory;

                foreach (var child in rootDirectory.GetChildren(true, context))
                    yield return child;
            }
        }

        public override XElement ToXml()
        {
            return new XElement("FixedDrives");
        }
    }
}
