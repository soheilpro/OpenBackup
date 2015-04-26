using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OpenBackup.Extension.Sync;
using OpenBackup.Framework;

namespace OpenBackup.Extension.DirectoryContainer
{
    [Loadable("Directory")]
    public class DirectoryContainer : ContainerBase
    {
        public string Path
        {
            get;
            set;
        }

        public List<IRule> Include
        {
            get;
            private set;
        }

        public List<IRule> Exclude
        {
            get;
            private set;
        }

        internal DirectoryContainerOptions Options
        {
            get;
            private set;
        }

        public DirectoryContainer(string path)
        {
            Initialize();

            Path = path;
            Options = new DirectoryContainerOptions();
        }

        public DirectoryContainer(XElement element, ILoadingContext context) : base(element, context)
        {
            Initialize();

            Path = element.Element("Path").Value;
            Include.AddRange(LoadRules(element.Element("Include"), context));
            Exclude.AddRange(LoadRules(element.Element("Exclude"), context));
            Options = new DirectoryContainerOptions(element.Element("Options"), context);
        }

        private IEnumerable<IRule> LoadRules(XElement element, ILoadingContext context)
        {
            if (element == null)
                yield break;

            var factory = context.ServiceContainer.Get<IFactory>();

            foreach (var ruleElement in element.Elements())
            {
                var rule = factory.Create<IRule>(ruleElement.Name.LocalName, ruleElement, context);

                if (rule == null)
                    throw new UnknownElementException(ruleElement);

                yield return rule;
            }
        }

        private void Initialize()
        {
            Include = new List<IRule>();
            Exclude = new List<IRule>();
        }

        public override IContainerInstance CreateInstance(IExecutionContext context)
        {
            return new DirectoryContainerInstance(this, context);
        }

        public override XElement ToXml()
        {
            var element = new XElement("Directory",
                                       new XElement("Path", Path),
                                       new XElement("Include", Include.Select(include => include.ToXml())).ToNullIfEmpty(),
                                       new XElement("Exclude", Exclude.Select(exclude => exclude.ToXml())).ToNullIfEmpty(),
                                       Options.ToXml().ToNullIfEmpty());

            FillXml(element);

            return element;
        }
    }
}
