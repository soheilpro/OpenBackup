using System;
using System.IO;
using NUnit.Framework;

namespace OpenBackup.Extension.FileSystem.Tests.Rules
{
    [TestFixture]
    public class ArchiveRuleTests
    {
        [Test]
        public void Matches_File_True()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file", FileAttributes.Archive);

            var rule = new AttributeRule(FileAttributes.Archive);
            var file = new FileObject(@"C:\file", fileSystem);

            var result = rule.Matches(file, null);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void Matches_File_False()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file");

            var rule = new AttributeRule(FileAttributes.Archive);
            var file = new FileObject(@"C:\file", fileSystem);

            var result = rule.Matches(file, null);

            Assert.AreEqual(false, result);
        }
    }
}
