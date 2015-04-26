using System;
using System.Xml.Linq;
using OpenBackup.Extension.Backup;
using OpenBackup.Framework;

namespace OpenBackup.Extension.DirectoryStorage
{
    [Loadable("Directory")]
    public class DirectoryStorage : StorageBase
    {
        public string Path
        {
            get;
            set;
        }

        public DirectoryStorageOptions Options
        {
            get;
            private set;
        }

        public DirectoryStorage(string path)
        {
            Path = path;
            Options = new DirectoryStorageOptions();
        }

        public DirectoryStorage(XElement element, ILoadingContext context)
        {
            Path = element.Element("Path").Value;
            Options = new DirectoryStorageOptions(element.Element("Options"), context);
        }

        public override IStorageInstance CreateInstance(IExecutionContext context)
        {
            return new DirectoryStorageInstance(this, context);
        }

        public override XElement ToXml()
        {
            return new XElement("Directory",
                                new XElement("Path", Path),
                                Options.ToXml().ToNullIfEmpty());
        }
    }
}
