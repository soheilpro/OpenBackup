using System;
using System.Xml.Linq;

namespace OpenBackup.Framework
{
    public abstract class JobBase : IJob
    {
        public string Name
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get;
            private set;
        }

        public JobBase()
        {
            IsEnabled = true;
        }

        public JobBase(XElement element, ILoadingContext context) : this()
        {
            var nameAttribute = element.Attribute("Name");
            if (nameAttribute != null)
                Name = nameAttribute.Value;

            var isEnabledAttribute = element.Attribute("IsEnabled");
            if (isEnabledAttribute != null)
                IsEnabled = bool.Parse(isEnabledAttribute.Value);
        }

        public abstract IJobInstance CreateInstance(IExecutionContext context);

        public abstract XElement ToXml();

        protected void FillXml(XElement element)
        {
            element.Add(Name != null ? new XAttribute("Name", Name) : null);
            element.Add(!IsEnabled ? new XAttribute("IsEnabled", "False") : null);
        }
    }
}
