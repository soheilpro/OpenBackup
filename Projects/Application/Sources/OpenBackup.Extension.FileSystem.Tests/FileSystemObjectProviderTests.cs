using System;
using System.Linq;
using NUnit.Framework;
using OpenBackup.Framework;
using OpenBackup.Framework.Rules;
using OpenBackup.Tests;

namespace OpenBackup.Extension.FileSystem.Tests
{
    [TestFixture]
    public class FileSystemObjectProviderTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetObjects_NullRoot()
        {
            var root = (IFileSystemRoot)null;
            var objectProvider = new FileSystemObjectProvider(root, null, null);

            var result = objectProvider.GetObjects(null).ToArray();
        }

        [Test]
        public void GetObjects_SingleRoot_NoInclude_NoExclude()
        {
            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<ITextFormatter>(new TextFormatter(serviceContainer));

            var context = new ExecutionContext(serviceContainer);

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file1");

            var root = new FileRoot(@"C:\file1", fileSystem);
            var objectProvider = new FileSystemObjectProvider(root, null, null);

            var result = objectProvider.GetObjects(context).ToArray();

            var expected = new IFileSystemObject[]
            {
                new FileObject(@"C:\file1", fileSystem),
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetObjects_SingleRoot_TrueInclude_NoExclude()
        {
            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<ITextFormatter>(new TextFormatter(serviceContainer));

            var context = new ExecutionContext(serviceContainer);

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file1");

            var root = new FileRoot(@"C:\file1", fileSystem);
            var include = new IRule[] {new TrueRule()};
            var objectProvider = new FileSystemObjectProvider(root, include, null);

            var result = objectProvider.GetObjects(context).ToArray();

            var expected = new IFileSystemObject[]
            {
                new FileObject(@"C:\file1", fileSystem),
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetObjects_SingleRoot_FalseInclude_NoExclude()
        {
            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<ITextFormatter>(new TextFormatter(serviceContainer));

            var context = new ExecutionContext(serviceContainer);

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file1");

            var root = new FileRoot(@"C:\file1", fileSystem);
            var include = new IRule[] {new FalseRule()};
            var objectProvider = new FileSystemObjectProvider(root, include, null);

            var result = objectProvider.GetObjects(context).ToArray();

            var expected = new IFileSystemObject[]
            {
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetObjects_SingleRoot_NoInclude_TrueExclude()
        {
            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<ITextFormatter>(new TextFormatter(serviceContainer));

            var context = new ExecutionContext(serviceContainer);

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file1");

            var root = new FileRoot(@"C:\file1", fileSystem);
            var exclude = new IRule[] {new TrueRule()};
            var objectProvider = new FileSystemObjectProvider(root, null, exclude);

            var result = objectProvider.GetObjects(context).ToArray();

            var expected = new IFileSystemObject[]
            {
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetObjects_SingleRoot_NoInclude_FalseExclude()
        {
            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<ITextFormatter>(new TextFormatter(serviceContainer));

            var context = new ExecutionContext(serviceContainer);

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file1");

            var root = new FileRoot(@"C:\file1", fileSystem);
            var exclude = new IRule[] {new FalseRule()};
            var objectProvider = new FileSystemObjectProvider(root, null, exclude);

            var result = objectProvider.GetObjects(context).ToArray();

            var expected = new IFileSystemObject[]
            {
                new FileObject(@"C:\file1", fileSystem),
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void GetObjects_MultiRoot()
        {
            var serviceContainer = new ManualServiceContainer();
            serviceContainer.Register<ITextFormatter>(new TextFormatter(serviceContainer));

            var context = new ExecutionContext(serviceContainer);

            var fileSystem = new MockFileSystem();
            fileSystem.RegisterFile(@"C:\file1");
            fileSystem.RegisterFile(@"C:\file2");

            var root1 = new FileRoot(@"C:\file1", fileSystem);
            var root2 = new FileRoot(@"C:\file2", fileSystem);
            var roots = new IFileSystemRoot[] {root1, root2};
            var objectProvider = new FileSystemObjectProvider(roots, null, null);

            var result = objectProvider.GetObjects(context).ToArray();

            var expected = new IFileSystemObject[]
            {
                new FileObject(@"C:\file1", fileSystem),
                new FileObject(@"C:\file2", fileSystem)
            };

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
