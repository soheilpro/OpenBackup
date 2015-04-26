using System;
using System.Collections.Generic;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    [Loadable("File")]
    public class FileRoot : FileSystemRootBase
    {
        public string Path
        {
            get;
            private set;
        }

        public FileRoot(string path, IFileSystem fileSystem) : base(fileSystem)
        {
            Path = path;
        }

        public FileRoot(XElement element, ILoadingContext context) : this(element.Value, context.ServiceContainer.Get<IFileSystem>())
        {
        }

        public override IEnumerable<IFileSystemObject> GetObjects(IExecutionContext context)
        {
            var textFormatter = context.ServiceContainer.Get<ITextFormatter>();
            var path = textFormatter.Format(Path);

            var file = new FileObject(path, FileSystem);

            if (!FileSystem.Exists(file))
                yield break;

            yield return file;
        }

        public override XElement ToXml()
        {
            return new XElement("File", new XText(Path));
        }
    }
}
