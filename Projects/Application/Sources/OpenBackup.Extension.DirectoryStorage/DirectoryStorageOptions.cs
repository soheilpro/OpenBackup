using System;
using System.Xml.Linq;

namespace OpenBackup.Extension.DirectoryStorage
{
    public class DirectoryStorageOptions
    {
        public string DirectoryName
        {
            get;
            set;
        }

        private string DirectoryNameDefault
        {
            get
            {
                return "%YYYY%-%MM%-%DD%_%HH%-%mm%-%SS%";
            }
        }

        public bool EnableHardLinks
        {
            get;
            set;
        }

        private bool EnableHardLinksDefault
        {
            get
            {
                return true;
            }
        }

        public DirectoryStorageOptions()
        {
            DirectoryName = DirectoryNameDefault;
            EnableHardLinks = EnableHardLinksDefault;
        }

        public DirectoryStorageOptions(XElement element, ILoadingContext context) : this()
        {
            if (element == null)
                return;

            var directoryNameElemet = element.Element("DirectoryName");

            if (directoryNameElemet != null)
                DirectoryName = directoryNameElemet.Value;

            var enableHardLinksElemet = element.Element("EnableHardLinks");

            if (enableHardLinksElemet != null)
                EnableHardLinks = bool.Parse(enableHardLinksElemet.Value);
        }

        public XElement ToXml()
        {
            return new XElement("Options",
                                DirectoryName != DirectoryNameDefault ? new XElement("DirectoryName", DirectoryName) : null,
                                EnableHardLinks != EnableHardLinksDefault ? new XElement("EnableHardLinks", EnableHardLinks.ToString()) : null);
        }
    }
}
