using System;
using System.Xml.Linq;

namespace OpenBackup.Extension.DirectoryContainer
{
    internal class DirectoryContainerOptions
    {
        public DirectoryContainerOptions()
        {
        }

        public DirectoryContainerOptions(XElement element, ILoadingContext context) : this()
        {
            if (element == null)
                return;
        }

        public XElement ToXml()
        {
            return new XElement("Options");
        }
    }
}
