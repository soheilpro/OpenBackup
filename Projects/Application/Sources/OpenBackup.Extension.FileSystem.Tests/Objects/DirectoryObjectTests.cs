using System;
using NUnit.Framework;

namespace OpenBackup.Extension.FileSystem.Tests.Objects
{
    [TestFixture]
    public class DirectoryObjectTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullPath()
        {
            var path = (string)null;
            var directory = new DirectoryObject(path, null);
        }

        [Test]
        public void Equals_Null()
        {
            var directory = new DirectoryObject(@"C:\dir", null);
            var result = directory.Equals(null);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_Self()
        {
            var directory = new DirectoryObject(@"C:\dir", null);
            var result = directory.Equals(directory);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void Equals_SameDirectory()
        {
            var directory1 = new DirectoryObject(@"C:\dir", null);
            var directory2 = new DirectoryObject(@"C:\dir", null);
            var result = directory1.Equals(directory2);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void Equals_AnotherDirectory()
        {
            var directory1 = new DirectoryObject(@"C:\dir1", null);
            var directory2 = new DirectoryObject(@"C:\dir2", null);
            var result = directory1.Equals(directory2);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_Drive()
        {
            var directory = new DirectoryObject(@"C:\dir", null);
            var drive = new DriveObject("C:", null);
            var result = directory.Equals(drive);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_File()
        {
            var directory = new DirectoryObject(@"C:\path", null);
            var file = new FileObject(@"C:\path", null);
            var result = directory.Equals(file);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Exists_False()
        {
            var fileSystem = new MockFileSystem();

            var directory = new DirectoryObject(@"C:\dir1", fileSystem);

            Assert.AreEqual(false, directory.Exists);
        }

        [Test]
        public void Exists_True()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\dir1");

            var directory = new DirectoryObject(@"C:\dir1", fileSystem);

            Assert.AreEqual(true, directory.Exists);
        }

        [Test]
        public void Parent_Drive()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\dir");

            var directory = new DirectoryObject(@"C:\file", fileSystem);
            var parent = new DriveObject(@"C:", fileSystem);

            Assert.AreEqual(parent, directory.Parent);
        }

        [Test]
        public void Parent_Directory()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\dir\dir");

            var directory = new DirectoryObject(@"C:\dir\dir", fileSystem);
            var parent = new DirectoryObject(@"C:\dir", fileSystem);

            Assert.AreEqual(parent, directory.Parent);
        }

        [Test]
        public void Drive()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\dir");

            var directory = new FileObject(@"C:\dir", fileSystem);
            var drive = new DriveObject(@"C:", fileSystem);

            Assert.AreEqual(drive, directory.Drive);
        }
    }
}
