using System;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    [Loadable("Directory")]
    public class DirectoryRule : RuleBase
    {
        public DirectoryRule()
        {
        }

        public DirectoryRule(XElement element, ILoadingContext context)
        {
        }

        public override bool Matches(IObject obj, IExecutionContext context)
        {
            return obj is IDirectoryObject;
        }

        public override XElement ToXml()
        {
            return new XElement("Directory");
        }
    }
}
