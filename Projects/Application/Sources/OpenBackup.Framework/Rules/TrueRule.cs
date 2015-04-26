using System;
using System.Xml.Linq;

namespace OpenBackup.Framework.Rules
{
    [Loadable("True")]
    public class TrueRule : RuleBase
    {
        public TrueRule()
        {
        }

        public TrueRule(XElement element, ILoadingContext context)
        {
        }

        public override bool Matches(IObject obj, IExecutionContext context)
        {
            return true;
        }

        public override XElement ToXml()
        {
            return new XElement("True");
        }
    }
}
