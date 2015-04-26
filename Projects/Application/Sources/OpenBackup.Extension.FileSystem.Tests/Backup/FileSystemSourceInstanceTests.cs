using System;
using System.Linq;
using NUnit.Framework;
using OpenBackup.Extension.FileSystem.Backup;
using OpenBackup.Extension.Backup;
using OpenBackup.Framework;
using OpenBackup.Tests;

namespace OpenBackup.Extension.FileSystem.Tests.Backup {
    
    [TestFixture]
    public class FileSystemSourceInstanceTests {

        [Test]
        public void GetObjects() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\dir");
            fileSystem.RegisterDirectory(@"C:\dir\dir1");
            fileSystem.RegisterFile(@"C:\dir\file1");
            fileSystem.RegisterFile(@"C:\file2");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var roots = new IFileSystemRoot[] {
                new DirectoryRoot(@"C:\dir", fileSystem),
                new FileRoot(@"C:\file2", fileSystem)
            };

            var source = new FileSystemSource(roots);
            var sourceInstance = source.CreateInstance(executionContext);

            var backupContext = new BackupContext(null, executionContext);
            var result = sourceInstance.GetObjects(backupContext).ToArray();

            var expected = new IObject[] {
                new DirectoryObject(@"C:\dir", fileSystem),
                new FileObject(@"C:\dir\file1", fileSystem),
                new DirectoryObject(@"C:\dir\dir1", fileSystem),
                new FileObject(@"C:\file2", fileSystem)
            };

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
