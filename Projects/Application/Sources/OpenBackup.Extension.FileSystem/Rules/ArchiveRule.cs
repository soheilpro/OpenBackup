using System;
using System.IO;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    [Loadable("Archive")]
    public class ArchiveRule : AttributeRule
    {
        public ArchiveRule() : base(FileAttributes.Hidden)
        {
        }

        public ArchiveRule(XElement element, ILoadingContext context) : this()
        {
        }

        public override XElement ToXml()
        {
            return new XElement("Archive");
        }
    }
}
