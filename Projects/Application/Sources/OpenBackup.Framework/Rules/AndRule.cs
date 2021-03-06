﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace OpenBackup.Framework.Rules
{
    [Loadable("And")]
    public class AndRule : RuleBase
    {
        public List<IRule> Rules
        {
            get;
            private set;
        }

        public AndRule()
        {
            Rules = new List<IRule>();
        }

        public AndRule(XElement element, ILoadingContext context) : this()
        {
            Rules.AddRange(LoadRules(element, context));
        }

        private IEnumerable<IRule> LoadRules(XElement element, ILoadingContext context)
        {
            var factory = context.ServiceContainer.Get<IFactory>();

            foreach (var ruleElement in element.Elements())
            {
                var rule = factory.Create<IRule>(ruleElement.Name.LocalName, ruleElement, context);

                if (rule == null)
                    throw new UnknownElementException(ruleElement);

                yield return rule;
            }
        }

        public override bool Matches(IObject obj, IExecutionContext context)
        {
            return Rules.All(rule => rule.Matches(obj, context));
        }

        public override XElement ToXml()
        {
            return new XElement("And",
                                Rules.Select(rule => rule.ToXml()));
        }
    }
}
