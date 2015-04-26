using System;
using NUnit.Framework;

namespace OpenBackup.Extension.FileSystem.Tests.Rules
{
    [TestFixture]
    public class FileRuleTests
    {
        [Test]
        public void Matches_Drive()
        {
            var rule = new FileRule();
            var drive = new DriveObject(@"C:", null);

            var result = rule.Matches(drive, null);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Matches_Directory()
        {
            var rule = new FileRule();
            var directory = new DirectoryObject(@"C:\dir", null);

            var result = rule.Matches(directory, null);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Matches_File()
        {
            var rule = new FileRule();
            var file = new FileObject(@"C:\file", null);

            var result = rule.Matches(file, null);

            Assert.AreEqual(true, result);
        }
    }
}
