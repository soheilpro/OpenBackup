using System;
using System.IO;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    [Loadable("System")]
    public class SystemRule : AttributeRule
    {
        public SystemRule() : base(FileAttributes.Hidden)
        {
        }

        public SystemRule(XElement element, ILoadingContext context) : this()
        {
        }

        public override XElement ToXml()
        {
            return new XElement("System");
        }
    }
}
