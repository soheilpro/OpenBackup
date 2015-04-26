using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Runtime
{
    public class Solution : ISolution
    {
        public List<IJob> Jobs
        {
            get;
            private set;
        }

        public Solution()
        {
            Jobs = new List<IJob>();
        }

        public Solution(XElement element, ILoadingContext context) : this()
        {
            Jobs.AddRange(LoadJobs(element.Element("Jobs"), context));
        }

        private IEnumerable<IJob> LoadJobs(XElement element, ILoadingContext context)
        {
            var factory = context.ServiceContainer.Get<IFactory>();

            foreach (var jobElement in element.Elements())
            {
                var job = factory.Create<IJob>(jobElement.Name.LocalName, jobElement, context);

                if (job == null)
                    throw new UnknownElementException(jobElement);

                yield return job;
            }
        }

        public XElement ToXml()
        {
            return new XElement("Solution",
                                new XElement("Jobs", Jobs.Select(job => job.ToXml())));
        }
    }
}
