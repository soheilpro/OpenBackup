using System;
using System.Collections.Generic;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    [Loadable("Directory")]
    public class DirectoryRoot : FileSystemRootBase
    {
        public string Path
        {
            get;
            private set;
        }

        public bool ChildrenOnly
        {
            get;
            set;
        }

        public DirectoryRoot(string path, IFileSystem fileSystem) : base(fileSystem)
        {
            Path = path;
        }

        public DirectoryRoot(XElement element, ILoadingContext context) : this(element.Value, context.ServiceContainer.Get<IFileSystem>())
        {
        }

        public override IEnumerable<IFileSystemObject> GetObjects(IExecutionContext context)
        {
            var textFormatter = context.ServiceContainer.Get<ITextFormatter>();
            var path = textFormatter.Format(Path);

            var directory = new DirectoryObject(path, FileSystem);

            if (!directory.Exists)
                yield break;

            if (!ChildrenOnly)
                yield return directory;

            foreach (var child in directory.GetChildren(true, context))
                yield return child;
        }

        public override XElement ToXml()
        {
            return new XElement("Directory", new XText(Path));
        }
    }
}
