using System;
using System.Collections.Generic;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    [Loadable("AllDrives")]
    public class AllDrivesRoot : FileSystemRootBase
    {
        public AllDrivesRoot(IFileSystem fileSystem) : base(fileSystem)
        {
        }

        public AllDrivesRoot(XElement element, ILoadingContext context) : this(context.ServiceContainer.Get<IFileSystem>())
        {
        }

        public override IEnumerable<IFileSystemObject> GetObjects(IExecutionContext context)
        {
            foreach (var drive in FileSystem.GetDrives())
            {
                var rootDirectory = drive.Root;

                yield return rootDirectory;

                foreach (var child in rootDirectory.GetChildren(true, context))
                    yield return child;
            }
        }

        public override XElement ToXml()
        {
            return new XElement("AllDrives");
        }
    }
}
