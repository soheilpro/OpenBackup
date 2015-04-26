using System;
using System.Linq;
using NUnit.Framework;
using OpenBackup.Extension.FileSystem.Sync;
using OpenBackup.Extension.Sync;
using OpenBackup.Tests;

namespace OpenBackup.Extension.FileSystem.Tests.Sync.Containers {

    [TestFixture]
    public class DirectoryContainerInstanceTests {

        [Test]
        public void GetObjects() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\dir");
            fileSystem.RegisterDirectory(@"C:\dir\dir1");
            fileSystem.RegisterDirectory(@"C:\dir\dir2");
            fileSystem.RegisterFile(@"C:\dir\file1");
            fileSystem.RegisterFile(@"C:\dir\file2");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\dir");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.GetObjects(syncContext).ToArray();

            var expected = new IFileSystemObject[] {
                new FileObject(@"C:\dir\file1", fileSystem),
                new FileObject(@"C:\dir\file2", fileSystem),
                new DirectoryObject(@"C:\dir\dir1", fileSystem),
                new DirectoryObject(@"C:\dir\dir2", fileSystem)
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void FindObject_File_Existing() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left");
            fileSystem.RegisterFile(@"C:\left\file");
            fileSystem.RegisterDirectory(@"C:\right");
            fileSystem.RegisterFile(@"C:\right\file");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var file = new FileObject(@"C:\left\file", fileSystem);
            file.Metadata["BasePath"] = Path.Parse(@"C:\left");

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.FindObject(file, syncContext);

            var expected = new FileObject(@"C:\right\file", fileSystem);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FindObject_File_NonExisting() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left");
            fileSystem.RegisterFile(@"C:\left\file");
            fileSystem.RegisterDirectory(@"C:\right");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var file = new FileObject(@"C:\left\file", fileSystem);
            file.Metadata["BasePath"] = Path.Parse(@"C:\left");

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.FindObject(file, syncContext);

            Assert.AreEqual(null, result);
        }

        [Test]
        public void FindObject_Directory_Existing() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left");
            fileSystem.RegisterDirectory(@"C:\left\dir");
            fileSystem.RegisterDirectory(@"C:\right");
            fileSystem.RegisterDirectory(@"C:\right\dir");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var directory = new DirectoryObject(@"C:\left\dir", fileSystem);
            directory.Metadata["BasePath"] = Path.Parse(@"C:\left");

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.FindObject(directory, syncContext);

            var expected = new DirectoryObject(@"C:\right\dir", fileSystem);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FindObject_Directory_NonExisting() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left");
            fileSystem.RegisterDirectory(@"C:\left\dir");
            fileSystem.RegisterDirectory(@"C:\right");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var directory = new DirectoryObject(@"C:\left\dir", fileSystem);
            directory.Metadata["BasePath"] = Path.Parse(@"C:\left");

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.FindObject(directory, syncContext);

            Assert.AreEqual(null, result);
        }

        [Test]
        public void AddObject_File_CopyFile() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left");
            fileSystem.RegisterFile(@"C:\left\file");
            fileSystem.RegisterDirectory(@"D:\right");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"D:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var leftFile = new FileObject(@"C:\left\file", fileSystem);
            leftFile.Metadata["BasePath"] = Path.Parse(@"C:\left");

            var rightFile = new FileObject(@"D:\right\file", fileSystem);

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.AddObject(leftFile, syncContext).ToArray();

            var expected = new IOperation[] {
                new CopyFileOperation(leftFile, rightFile, fileSystem, executionContext)
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void AddObject_File_CreateHardLink() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left");
            fileSystem.RegisterFile(@"C:\left\file");
            fileSystem.RegisterDirectory(@"C:\right");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var leftFile = new FileObject(@"C:\left\file", fileSystem);
            leftFile.Metadata["BasePath"] = Path.Parse(@"C:\left");

            var rightFile = new FileObject(@"C:\right\file", fileSystem);

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.AddObject(leftFile, syncContext).ToArray();

            var expected = new IOperation[] {
                new CreateHardLinkOperation(leftFile, rightFile, fileSystem, executionContext)
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetChanges_File_NoChanges() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\left\file", length: 100, lastWriteTime: DateTime.MinValue);
            fileSystem.RegisterFile(@"C:\right\file", length: 100, lastWriteTime: DateTime.MinValue);

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var leftFile = new FileObject(@"C:\left\file", fileSystem);
            var rightFile = new FileObject(@"C:\right\file", fileSystem);

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.GetChanges(rightFile, leftFile, syncContext).ToArray();

            var expected = new IChange[] {
            };

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetChanges_File_LengthChanged() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\left\file", length: 100, lastWriteTime: DateTime.MinValue);
            fileSystem.RegisterFile(@"C:\right\file", length: 200, lastWriteTime: DateTime.MinValue);

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var leftFile = new FileObject(@"C:\left\file", fileSystem);
            var rightFile = new FileObject(@"C:\right\file", fileSystem);

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.GetChanges(rightFile, leftFile, syncContext).ToArray();

            var expected = new IChange[] {
                new ContentChange()
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetChanges_File_LastWriteTimeChanged() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\left\file", length: 100, lastWriteTime: DateTime.MinValue);
            fileSystem.RegisterFile(@"C:\right\file", length: 100, lastWriteTime: DateTime.MinValue.AddDays(1));

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var leftFile = new FileObject(@"C:\left\file", fileSystem);
            var rightFile = new FileObject(@"C:\right\file", fileSystem);

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.GetChanges(rightFile, leftFile, syncContext).ToArray();

            var expected = new IChange[] {
                new ContentChange()
            };

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetChanges_File_NameCasingChanged() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\left\file");
            fileSystem.RegisterFile(@"C:\right\FILE");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var leftFile = new FileObject(@"C:\left\file", fileSystem);
            var rightFile = new FileObject(@"C:\right\file", fileSystem);

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.GetChanges(rightFile, leftFile, syncContext).ToArray();

            var expected = new IChange[] {
                new NameCasingChange()
            };

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetChanges_Directory_NoChanges() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left\dir");
            fileSystem.RegisterDirectory(@"C:\right\dir");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var leftDirectory = new DirectoryObject(@"C:\left\dir", fileSystem);
            var rightDirectory = new DirectoryObject(@"C:\right\dir", fileSystem);

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.GetChanges(rightDirectory, leftDirectory, syncContext).ToArray();

            var expected = new IChange[] {
            };

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetChanges_Directory_NameCasingChanged() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left\dir");
            fileSystem.RegisterDirectory(@"C:\right\DIR");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var leftDirectory = new DirectoryObject(@"C:\left\dir", fileSystem);
            var rightDirectory = new DirectoryObject(@"C:\right\dir", fileSystem);

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.GetChanges(rightDirectory, leftDirectory, syncContext).ToArray();

            var expected = new IChange[] {
                new NameCasingChange()
            };

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void UpdateObject_File_ContentChange() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left");
            fileSystem.RegisterFile(@"C:\left\file");
            fileSystem.RegisterDirectory(@"C:\right");
            fileSystem.RegisterFile(@"C:\right\file");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var leftFile = new FileObject(@"C:\left\file", fileSystem);
            leftFile.Metadata["BasePath"] = Path.Parse(@"C:\left");

            var rightFile = new FileObject(@"C:\right\file", fileSystem);

            var change = new ContentChange();

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.UpdateObject(rightFile, leftFile, change, syncContext).ToArray();

            var expected = new IOperation[] {
                new CopyFileOperation(leftFile, rightFile, fileSystem, executionContext)
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void UpdateObject_File_NameCasingChange() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left");
            fileSystem.RegisterFile(@"C:\left\file");
            fileSystem.RegisterDirectory(@"C:\right");
            fileSystem.RegisterFile(@"C:\right\FILE");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var leftFile = new FileObject(@"C:\left\file", fileSystem);
            leftFile.Metadata["BasePath"] = Path.Parse(@"C:\left");

            var rightFile = new FileObject(@"C:\right\file", fileSystem);

            var change = new NameCasingChange();

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.UpdateObject(rightFile, leftFile, change, syncContext).ToArray();

            var expected = new IOperation[] {
                new RenameFileOperation(rightFile, leftFile.ActualName, fileSystem, executionContext)
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void UpdateObject_Directory_NameCasingChange() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left");
            fileSystem.RegisterDirectory(@"C:\left\dir");
            fileSystem.RegisterDirectory(@"C:\right");
            fileSystem.RegisterDirectory(@"C:\right\DIR");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var leftDirectory = new DirectoryObject(@"C:\left\dir", fileSystem);
            leftDirectory.Metadata["BasePath"] = Path.Parse(@"C:\left");

            var rightDirectory = new DirectoryObject(@"C:\right\dir", fileSystem);

            var change = new NameCasingChange();

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.UpdateObject(rightDirectory, leftDirectory, change, syncContext).ToArray();

            var expected = new IOperation[] {
                new RenameDirectoryOperation(rightDirectory, leftDirectory.ActualName, fileSystem, executionContext)
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void RemoveObject_File() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left");
            fileSystem.RegisterDirectory(@"C:\right");
            fileSystem.RegisterFile(@"C:\right\file");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var rightFile = new FileObject(@"C:\right\file", fileSystem);

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.RemoveObject(rightFile, syncContext).ToArray();

            var expected = new IOperation[] {
                new DeleteFileOperation(rightFile, fileSystem, executionContext)
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void RemoveObject_Directory() {

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\left");
            fileSystem.RegisterDirectory(@"C:\right");
            fileSystem.RegisterDirectory(@"C:\right\dir");

            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<IFileSystem>(fileSystem);

            var executionContext = new ExecutionContext(serviceContainer);

            var directoryContainer = new DirectoryContainer(@"C:\right");
            var directoryContainerInstance = directoryContainer.CreateInstance(executionContext);

            var rightDirectory = new DirectoryObject(@"C:\right\dir", fileSystem);

            var syncContext = new SyncContext(null, executionContext);
            var result = directoryContainerInstance.RemoveObject(rightDirectory, syncContext).ToArray();

            var expected = new IOperation[] {
                new DeleteDirectoryOperation(rightDirectory, fileSystem, executionContext)
            };

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
