using System;
using System.IO;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    [Loadable("Attribute")]
    public class AttributeRule : RuleBase
    {
        public FileAttributes Attribute
        {
            get;
            set;
        }

        public AttributeRule(FileAttributes attribute = (FileAttributes)0)
        {
            Attribute = attribute;
        }

        public AttributeRule(XElement element, ILoadingContext context)
        {
            Attribute = (FileAttributes)Enum.Parse(typeof(FileAttributes), element.Value);
        }

        public override bool Matches(IObject obj, IExecutionContext context)
        {
            if (obj is IFileObject)
                return Matches((IFileObject)obj);

            if (obj is IDirectoryObject)
                return Matches((IDirectoryObject)obj);

            throw new NotSupportedException();
        }

        private bool Matches(IFileObject obj)
        {
            return (obj.Attributes & Attribute) == Attribute;
        }

        private bool Matches(IDirectoryObject obj)
        {
            return (obj.Attributes & Attribute) == Attribute;
        }

        public override XElement ToXml()
        {
            return new XElement("Attribute", Attribute);
        }
    }
}
