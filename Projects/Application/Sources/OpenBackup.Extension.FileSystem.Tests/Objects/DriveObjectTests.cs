using System;
using NUnit.Framework;

namespace OpenBackup.Extension.FileSystem.Tests.Objects
{
    [TestFixture]
    public class DriveObjectTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullPath()
        {
            var path = (string)null;
            var drive = new DriveObject(path, null);
        }

        [Test]
        public void Equals_Null()
        {
            var drive = new DriveObject("C:", null);
            var result = drive.Equals(null);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_Self()
        {
            var drive = new DriveObject("C:", null);
            var result = drive.Equals(drive);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void Equals_SameDrive()
        {
            var drive1 = new DriveObject("C:", null);
            var drive2 = new DriveObject("C:", null);
            var result = drive1.Equals(drive2);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void Equals_AnotherDrive()
        {
            var drive1 = new DriveObject("C:", null);
            var drive2 = new DriveObject("D:", null);
            var result = drive1.Equals(drive2);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_Directory()
        {
            var drive = new DriveObject("C:", null);
            var directory = new DirectoryObject(@"C:\dir", null);
            var result = drive.Equals(directory);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Equals_File()
        {
            var drive = new DriveObject("C:", null);
            var file = new FileObject(@"C:\file", null);
            var result = drive.Equals(file);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Exists_NoDrive()
        {
            var fileSystem = new MockFileSystem();

            var drive = new DriveObject(@"C:", fileSystem);

            Assert.AreEqual(false, drive.Exists);
        }

        [Test]
        public void Exists_ExistingDrive()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDrive(@"C:");

            var drive = new DriveObject(@"C:", fileSystem);

            Assert.AreEqual(true, drive.Exists);
        }

        [Test]
        public void Parent_Null()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDrive(@"C:");

            var drive = new DriveObject(@"C:", fileSystem);

            Assert.AreEqual(null, drive.Parent);
        }

        [Test]
        public void Root()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDrive(@"C:");

            var drive = new DriveObject(@"C:", fileSystem);
            var root = new DirectoryObject(@"C:\", fileSystem);

            Assert.AreEqual(root, drive.Root);
        }
    }
}
