using System;
using System.Xml.Linq;
using OpenBackup.Extension.Backup;
using OpenBackup.Framework;

namespace OpenBackup.Extension.MetadataFileStorage
{
    [Loadable("MetadataFile")]
    public class MetadataFileStorage : StorageBase
    {
        public string Path
        {
            get;
            set;
        }

        public MetadataFileStorage(string path)
        {
            Path = path;
        }

        public MetadataFileStorage(XElement element, ILoadingContext context)
        {
            Path = element.Element("Path").Value;
        }

        public override IStorageInstance CreateInstance(IExecutionContext context)
        {
            return new MetadataFileStorageInstance(this, context);
        }

        public override XElement ToXml()
        {
            return new XElement("MetadataFile",
                                new XElement("Path", Path));
        }
    }
}
