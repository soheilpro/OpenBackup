using System;
using System.Linq;
using NUnit.Framework;
using OpenBackup.Framework;
using OpenBackup.Tests;

namespace OpenBackup.Extension.FileSystem.Tests.Roots
{
    [TestFixture]
    public class DirectoryRootTests
    {
        [Test]
        public void GetObjects_NonExistingDirectory()
        {
            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<ITextFormatter>(new TextFormatter(serviceContainer));

            var context = new ExecutionContext(serviceContainer);

            var fileSystem = new MockFileSystem();

            var root = new DirectoryRoot(@"C:\dir", fileSystem);
            var result = root.GetObjects(context).ToArray();

            var expected = new IFileSystemObject[]
            {
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetObjects_0Level()
        {
            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<ITextFormatter>(new TextFormatter(serviceContainer));

            var context = new ExecutionContext(serviceContainer);

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\root");

            var root = new DirectoryRoot(@"C:\root", fileSystem);
            var result = root.GetObjects(context).ToArray();

            var expected = new IFileSystemObject[]
            {
                new DirectoryObject(@"C:\root", fileSystem)
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetObjects_1Level()
        {
            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<ITextFormatter>(new TextFormatter(serviceContainer));

            var context = new ExecutionContext(serviceContainer);

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\root\dir1");

            var root = new DirectoryRoot(@"C:\root", fileSystem);
            var result = root.GetObjects(context).ToArray();

            var expected = new IFileSystemObject[]
            {
                new DirectoryObject(@"C:\root", fileSystem),
                new DirectoryObject(@"C:\root\dir1", fileSystem)
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetObjects_2Level()
        {
            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<ITextFormatter>(new TextFormatter(serviceContainer));

            var context = new ExecutionContext(serviceContainer);

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterDirectory(@"C:\root\dir1\dir2");

            var root = new DirectoryRoot(@"C:\root", fileSystem);
            var result = root.GetObjects(context).ToArray();

            var expected = new IFileSystemObject[]
            {
                new DirectoryObject(@"C:\root", fileSystem),
                new DirectoryObject(@"C:\root\dir1", fileSystem),
                new DirectoryObject(@"C:\root\dir1\dir2", fileSystem)
            };

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
