using System;
using System.Linq;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.Sync
{
    public class Pair : IPair
    {
        public IContainer Left
        {
            get;
            set;
        }

        public IContainer Right
        {
            get;
            set;
        }

        public Pair(IContainer left, IContainer right)
        {
            Initialize();

            Left = left;
            Right = right;
        }

        public Pair(XElement element, ILoadingContext context)
        {
            Initialize();

            Left = LoadContainer(element.Element("Left"), context);
            Right = LoadContainer(element.Element("Right"), context);
        }

        private IContainer LoadContainer(XElement element, ILoadingContext context)
        {
            var containerElement = element.Elements().SingleOrDefault();
            var factory = context.ServiceContainer.Get<IFactory>();

            var container = factory.Create<IContainer>(containerElement.Name.LocalName, containerElement, context);

            if (container == null)
                throw new UnknownElementException(containerElement);

            return container;
        }

        private void Initialize()
        {
        }

        public IPairInstance CreateInstance(IExecutionContext context)
        {
            return new PairInstance(this, context);
        }

        public XElement ToXml()
        {
            return new XElement("Pair",
                                new XElement("Left", Left.ToXml()),
                                new XElement("Right", Right.ToXml()));
        }
    }
}
