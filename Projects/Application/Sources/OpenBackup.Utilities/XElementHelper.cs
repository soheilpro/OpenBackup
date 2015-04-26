using System;
using System.Linq;
using System.Xml.Linq;

namespace OpenBackup
{
    public static class XElementHelper
    {
        public static XElement ToNullIfEmpty(this XElement element)
        {
            if (element.IsEmpty)
                return null;

            return element;
        }

        public static string Path(this XElement element)
        {
            return string.Join(" > ", element.Ancestors().Reverse().Select(e => e.Name)) + " > " + element.Name;
        }
    }
}
