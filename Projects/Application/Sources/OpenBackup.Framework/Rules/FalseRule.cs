using System;
using System.Xml.Linq;

namespace OpenBackup.Framework.Rules
{
    [Loadable("False")]
    public class FalseRule : RuleBase
    {
        public FalseRule()
        {
        }

        public FalseRule(XElement element, ILoadingContext context)
        {
        }

        public override bool Matches(IObject obj, IExecutionContext context)
        {
            return false;
        }

        public override XElement ToXml()
        {
            return new XElement("False");
        }
    }
}
