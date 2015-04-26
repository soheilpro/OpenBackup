using System;
using NUnit.Framework;

namespace OpenBackup.Extension.FileSystem.Tests.Objects
{
    [TestFixture]
    public class FileObjectTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullPath()
        {
            var path = (string)null;
            var file = new FileObject(path, null);
        }

        [Test]
        public void Equals_Null()
        {
            var file = new FileObject(@"C:\file", null);
            var result = file.Equals(null);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_Self()
        {
            var file = new FileObject(@"C:\file", null);
            var result = file.Equals(file);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void Equals_SameFile()
        {
            var file1 = new FileObject(@"C:\file", null);
            var file2 = new FileObject(@"C:\file", null);
            var result = file1.Equals(file2);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void Equals_AnotherDirectory()
        {
            var file1 = new FileObject(@"C:\file1", null);
            var file2 = new FileObject(@"C:\file2", null);
            var result = file1.Equals(file2);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_Drive()
        {
            var file = new FileObject(@"C:\file", null);
            var drive = new DriveObject("C:", null);
            var result = file.Equals(drive);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_Directory()
        {
            var file = new FileObject(@"C:\path", null);
            var directory = new DirectoryObject(@"C:\path", null);
            var result = file.Equals(directory);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Exists_False()
        {
            var fileSystem = new MockFileSystem();

            var file = new FileObject(@"C:\file", fileSystem);

            Assert.AreEqual(false, file.Exists);
        }

        [Test]
        public void Exists_True()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file");

            var file = new FileObject(@"C:\file", fileSystem);

            Assert.AreEqual(true, file.Exists);
        }

        [Test]
        public void Parent_Drive()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file");

            var file = new FileObject(@"C:\file", fileSystem);
            var parent = new DriveObject(@"C:", fileSystem);

            Assert.AreEqual(parent, file.Parent);
        }

        [Test]
        public void Parent_Directory()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\dir\file");

            var file = new FileObject(@"C:\dir\file", fileSystem);
            var parent = new DirectoryObject(@"C:\dir", fileSystem);

            Assert.AreEqual(parent, file.Parent);
        }

        [Test]
        public void Drive()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\dir\file");

            var file = new FileObject(@"C:\dir\file", fileSystem);
            var drive = new DriveObject(@"C:", fileSystem);

            Assert.AreEqual(drive, file.Drive);
        }
    }
}
