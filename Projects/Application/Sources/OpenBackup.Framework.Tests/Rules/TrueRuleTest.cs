using System;
using NUnit.Framework;
using OpenBackup.Framework.Rules;

namespace OpenBackup.Extension.Common.Tests.Rules
{
    [TestFixture]
    public class TrueRuleTest
    {
        [Test]
        public void Matches()
        {
            var rule = new TrueRule();
            var result = rule.Matches(null, null);

            Assert.AreEqual(true, result);
        }
    }
}
