using System;
using System.Xml.Linq;

namespace OpenBackup.Framework
{
    public interface IRule
    {
        bool Matches(IObject obj, IExecutionContext context);

        XElement ToXml();
    }
}
