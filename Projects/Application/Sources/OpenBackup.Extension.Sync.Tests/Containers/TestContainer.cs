using System;
using System.Collections.Generic;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Sync.Tests
{
    internal class TestContainer : ContainerBase
    {
        public List<IObject> Objects
        {
            get;
            set;
        }

        public TestContainer()
        {
            Objects = new List<IObject>();
        }

        public override IContainerInstance CreateInstance(IExecutionContext context)
        {
            return new TestContainerInstance(this, context);
        }

        public override XElement ToXml()
        {
            throw new NotImplementedException();
        }
    }
}
