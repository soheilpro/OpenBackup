using System;
using System.Linq;
using NUnit.Framework;
using OpenBackup.Extension.FileSystem.Backup;
using OpenBackup.Extension.Backup;
using OpenBackup.Framework;
using OpenBackup.Tests;

namespace OpenBackup.Extension.FileSystem.Tests.Backup {
    
    [TestFixture]
    public class DirectoryStorageInstanceTests {

        [Test]
        public void BackupDirectoryPath() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file");

            var dateTime = new MockDateTime(new DateTime(2010, 10, 1, 0, 0, 0));

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);
            serviceContainer.Register<IDateTimeService>(dateTime);

            var executionContext = new ExecutionContext(serviceContainer);

            var file = new FileObject(@"C:\file", fileSystem);

            var storage = new DirectoryStorage(@"C:\backup");
            var storageInstance = (DirectoryStorageInstance)storage.CreateInstance(executionContext);
            
            var expected = Path.Parse(@"C:\backup").Combine(dateTime.GetCurrentDateTime().ToString("yyyy-MM-dd HH-mm-ss"));

            Assert.AreEqual(expected, storageInstance.BackupDirectoryPath);
        }

        [Test]
        public void Store_File() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file");
            
            var dateTime = new MockDateTime(new DateTime(2010, 10, 1, 0, 0, 0));

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);
            serviceContainer.Register<IDateTimeService>(dateTime);

            var executionContext = new ExecutionContext(serviceContainer);

            var file = new FileObject(@"C:\file", fileSystem);

            var storage = new DirectoryStorage(@"C:\backup");
            var storageInstance = storage.CreateInstance(executionContext);

            var backupContext = new BackupContext(null, executionContext);
            var result = storageInstance.Store(file, backupContext).ToArray();

            var expected = new IOperation[] {
                new CopyFileOperation(file, new FileObject(@"C:\backup\2010-10-01 00-00-00\C\file", fileSystem), fileSystem, executionContext)
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void Store_Directory() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\dir");
            
            var dateTime = new MockDateTime(new DateTime(2010, 10, 1, 0, 0, 0));

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);
            serviceContainer.Register<IDateTimeService>(dateTime);

            var executionContext = new ExecutionContext(serviceContainer);

            var directory = new DirectoryObject(@"C:\dir", fileSystem);

            var storage = new DirectoryStorage(@"C:\backup");
            var storageInstance = storage.CreateInstance(executionContext);

            var backupContext = new BackupContext(null, executionContext);
            var result = storageInstance.Store(directory, backupContext).ToArray();

            var expected = new IOperation[] {
                new CreateDirectoryOperation(new DirectoryObject(@"C:\backup\2010-10-01 00-00-00\C\dir", fileSystem), fileSystem, executionContext)
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void Store_Drive() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDrive(@"C:");

            var dateTime = new MockDateTime(new DateTime(2010, 10, 1, 0, 0, 0));

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);
            serviceContainer.Register<IDateTimeService>(dateTime);

            var executionContext = new ExecutionContext(serviceContainer);

            var drive = new DriveObject(@"C:", fileSystem);

            var storage = new DirectoryStorage(@"C:\backup");
            var storageInstance = storage.CreateInstance(executionContext);

            var backupContext = new BackupContext(null, executionContext);
            var result = storageInstance.Store(drive, backupContext).ToArray();
        }
    }
}
