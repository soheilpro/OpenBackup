using System;
using System.Linq;
using NUnit.Framework;
using OpenBackup.Framework;
using OpenBackup.Tests;

namespace OpenBackup.Extension.FileSystem.Tests.Roots
{
    [TestFixture]
    public class FileRootTests
    {
        [Test]
        public void GetObjects_NonExistingFile()
        {
            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<ITextFormatter>(new TextFormatter(serviceContainer));

            var context = new ExecutionContext(serviceContainer);

            var fileSystem = new MockFileSystem();
            var root = new FileRoot(@"C:\root", fileSystem);
            var result = root.GetObjects(context).ToArray();

            var expected = new IFileSystemObject[]
            {
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetObjects_ExistingFile()
        {
            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<ITextFormatter>(new TextFormatter(serviceContainer));

            var context = new ExecutionContext(serviceContainer);

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\root");

            var root = new FileRoot(@"C:\root", fileSystem);
            var result = root.GetObjects(context).ToArray();

            var expected = new IFileSystemObject[]
            {
                new FileObject(@"C:\root", fileSystem)
            };

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
