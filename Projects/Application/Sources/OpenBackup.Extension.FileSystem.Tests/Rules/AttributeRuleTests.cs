using System;
using System.IO;
using NUnit.Framework;

namespace OpenBackup.Extension.FileSystem.Tests.Rules
{
    [TestFixture]
    public class AttributeRuleTests
    {
        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void Matches_Drive()
        {
            var rule = new AttributeRule();
            var drive = new DriveObject(@"C:", null);

            var result = rule.Matches(drive, null);
        }

        [Test]
        public void Matches_Directory_Directory()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\dir");

            var rule = new AttributeRule(FileAttributes.Directory);
            var directory = new DirectoryObject(@"C:\dir", fileSystem);

            var result = rule.Matches(directory, null);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void Matches_File_System()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file", FileAttributes.System);

            var rule = new AttributeRule(FileAttributes.System);
            var file = new FileObject(@"C:\file", fileSystem);

            var result = rule.Matches(file, null);

            Assert.AreEqual(true, result);
        }
    }
}
