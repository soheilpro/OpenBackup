using System;
using System.Xml.Linq;

namespace OpenBackup.Framework
{
    public class UnknownElementException : LoadingException
    {
        public XElement Element
        {
            get;
            private set;
        }

        public UnknownElementException(XElement element)
        {
            Element = element;
        }

        public override string Message
        {
            get
            {
                // TODO
                return Element.Path();
            }
        }
    }
}
