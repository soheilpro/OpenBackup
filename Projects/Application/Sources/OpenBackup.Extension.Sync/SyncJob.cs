using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Sync
{
    [Loadable("Sync")]
    public class SyncJob : JobBase
    {
        public List<IPair> Pairs
        {
            get;
            private set;
        }

        public SyncJob()
        {
            Initialize();
        }

        public SyncJob(XElement element, ILoadingContext context) : base(element, context)
        {
            Initialize();

            Pairs.AddRange(LoadPairs(element.Element("Pairs"), context));
        }

        public IEnumerable<IPair> LoadPairs(XElement element, ILoadingContext context)
        {
            foreach (var sourceElement in element.Elements())
                yield return new Pair(sourceElement, context);
        }

        private void Initialize()
        {
            Pairs = new List<IPair>();
        }

        public override IJobInstance CreateInstance(IExecutionContext context)
        {
            return new SyncJobInstance(this, context);
        }

        public override XElement ToXml()
        {
            var element = new XElement("Sync",
                                       new XElement("Pairs", Pairs.Select(pair => pair.ToXml())));

            FillXml(element);

            return element;
        }
    }
}
