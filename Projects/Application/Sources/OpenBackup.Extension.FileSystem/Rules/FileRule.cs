using System;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    [Loadable("File")]
    public class FileRule : RuleBase
    {
        public FileRule()
        {
        }

        public FileRule(XElement element, ILoadingContext context)
        {
        }

        public override bool Matches(IObject obj, IExecutionContext context)
        {
            return obj is IFileObject;
        }

        public override XElement ToXml()
        {
            return new XElement("File");
        }
    }
}
