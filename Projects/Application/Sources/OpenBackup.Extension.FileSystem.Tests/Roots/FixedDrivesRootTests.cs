using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace OpenBackup.Extension.FileSystem.Tests.Roots
{
    [TestFixture]
    public class FixedDrivesTests
    {
        [Test]
        public void GetObjects_NoDrives()
        {
            var fileSystem = new MockFileSystem();

            var root = new AllDrivesRoot(fileSystem);
            var result = root.GetObjects(null).ToArray();

            var expected = new IFileSystemObject[]
            {
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetObjects_MixedDrives()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDrive(@"C:", DriveType.Fixed);
            fileSystem.RegisterDrive(@"D:", DriveType.Network);
            fileSystem.RegisterDrive(@"E:", DriveType.Removable);

            var root = new FixedDrivesRoot(fileSystem);
            var result = root.GetObjects(null).ToArray();

            var expected = new IFileSystemObject[]
            {
                new DirectoryObject(@"C:\", fileSystem)
            };

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
