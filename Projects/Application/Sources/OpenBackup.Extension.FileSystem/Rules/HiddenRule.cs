using System;
using System.IO;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    [Loadable("Hidden")]
    public class HiddenRule : AttributeRule
    {
        public HiddenRule() : base(FileAttributes.Hidden)
        {
        }

        public HiddenRule(XElement element, ILoadingContext context) : this()
        {
        }

        public override XElement ToXml()
        {
            return new XElement("Hidden");
        }
    }
}
