using System;
using System.Xml.Linq;

namespace OpenBackup.Framework
{
    public class MissingElementException : LoadingException
    {
        public XElement ParentElement
        {
            get;
            private set;
        }

        public string Element
        {
            get;
            private set;
        }

        public MissingElementException(XElement parentElement, string element)
        {
            ParentElement = parentElement;
            Element = element;
        }

        public override string Message
        {
            get
            {
                return string.Format("'{0}' element requires a child '{1}' element.", ParentElement.Path(), Element);
            }
        }
    }
}
