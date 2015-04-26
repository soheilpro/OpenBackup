using System;
using NUnit.Framework;
using OpenBackup.Framework.Rules;

namespace OpenBackup.Extension.Common.Tests.Rules
{
    [TestFixture]
    public class FalseRuleTest
    {
        [Test]
        public void Matches()
        {
            var rule = new FalseRule();
            var result = rule.Matches(null, null);

            Assert.AreEqual(false, result);
        }
    }
}
