using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OpenBackup.Extension.Backup;
using OpenBackup.Extension.FileSystem;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystemSource
{
    [Loadable("FileSystem")]
    public class FileSystemSource : SourceBase
    {
        public List<IFileSystemRoot> Roots
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

        public FileSystemSource(IEnumerable<IFileSystemRoot> roots)
        {
            Initialize();

            Roots.AddRange(roots);
        }

        public FileSystemSource(XElement element, ILoadingContext context) : base(element, context)
        {
            Initialize();

            if (element.Element("Roots") == null)
                throw new MissingElementException(element, "Roots");

            Roots.AddRange(LoadFileSystemRoots(element.Element("Roots"), context));
            Include.AddRange(LoadRules(element.Element("Include"), context));
            Exclude.AddRange(LoadRules(element.Element("Exclude"), context));
        }

        private IEnumerable<IFileSystemRoot> LoadFileSystemRoots(XElement element, ILoadingContext context)
        {
            var factory = context.ServiceContainer.Get<IFactory>();

            foreach (var rootElement in element.Elements())
            {
                var root = factory.Create<IFileSystemRoot>(rootElement.Name.LocalName, rootElement, context);

                if (root == null)
                    throw new UnknownElementException(rootElement);

                yield return root;
            }
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
            Roots = new List<IFileSystemRoot>();
            Include = new List<IRule>();
            Exclude = new List<IRule>();
        }

        public override ISourceInstance CreateInstance(IExecutionContext context)
        {
            return new FileSystemSourceInstance(this, context);
        }

        public override XElement ToXml()
        {
            var element = new XElement("FileSystem",
                                       new XElement("Roots", Roots.Select(root => root.ToXml())).ToNullIfEmpty(),
                                       new XElement("Include", Include.Select(include => include.ToXml())).ToNullIfEmpty(),
                                       new XElement("Exclude", Exclude.Select(exclude => exclude.ToXml())).ToNullIfEmpty());

            FillXml(element);

            return element;
        }
    }
}
