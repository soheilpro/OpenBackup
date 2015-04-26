using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Backup
{
    [Loadable("Backup")]
    public class BackupJob : JobBase
    {
        public List<ISource> Sources
        {
            get;
            private set;
        }

        public List<IStorage> Storages
        {
            get;
            private set;
        }

        public BackupJob()
        {
            Initialize();
        }

        public BackupJob(XElement element, ILoadingContext context) : base(element, context)
        {
            Initialize();

            Sources.AddRange(LoadSources(element.Element("Sources"), context));
            Storages.AddRange(LoadStorages(element.Element("Storages"), context));
        }

        public IEnumerable<ISource> LoadSources(XElement element, ILoadingContext context)
        {
            var factory = context.ServiceContainer.Get<IFactory>();

            foreach (var sourceElement in element.Elements())
            {
                var source = factory.Create<ISource>(sourceElement.Name.LocalName, sourceElement, context);

                if (source == null)
                    throw new UnknownElementException(sourceElement);

                yield return source;
            }
        }

        public IEnumerable<IStorage> LoadStorages(XElement element, ILoadingContext context)
        {
            var factory = context.ServiceContainer.Get<IFactory>();

            foreach (var destinationElement in element.Elements())
            {
                var destination = factory.Create<IStorage>(destinationElement.Name.LocalName, destinationElement, context);

                if (destination == null)
                    throw new UnknownElementException(destinationElement);

                yield return destination;
            }
        }

        private void Initialize()
        {
            Sources = new List<ISource>();
            Storages = new List<IStorage>();
        }

        public override IJobInstance CreateInstance(IExecutionContext context)
        {
            return new BackupJobInstance(this, context);
        }

        public override XElement ToXml()
        {
            var element = new XElement("Backup",
                                       new XElement("Sources", Sources.Select(source => source.ToXml())),
                                       new XElement("Storages", Storages.Select(destination => destination.ToXml())));

            FillXml(element);

            return element;
        }
    }
}
