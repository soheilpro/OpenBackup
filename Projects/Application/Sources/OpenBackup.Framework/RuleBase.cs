using System;
using System.Xml.Linq;

namespace OpenBackup.Framework
{
    public abstract class RuleBase : IRule
    {
        public abstract bool Matches(IObject obj, IExecutionContext context);

        public abstract XElement ToXml();
    }
}
