using System;
using System.IO;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    [Loadable("ReadOnly")]
    public class ReadOnlyRule : AttributeRule
    {
        public ReadOnlyRule() : base(FileAttributes.Hidden)
        {
        }

        public ReadOnlyRule(XElement element, ILoadingContext context) : this()
        {
        }

        public override XElement ToXml()
        {
            return new XElement("ReadOnly");
        }
    }
}
