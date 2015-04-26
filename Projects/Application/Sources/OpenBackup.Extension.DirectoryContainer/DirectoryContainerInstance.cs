using System;
using System.Collections.Generic;
using OpenBackup.Extension.FileSystem;
using OpenBackup.Extension.Sync;
using OpenBackup.Framework;

namespace OpenBackup.Extension.DirectoryContainer
{
    internal class DirectoryContainerInstance : ContainerInstanceBase
    {
        private DirectoryContainer _container;

        private FileSystemPath _containerPath;

        public DirectoryContainerInstance(DirectoryContainer container, IExecutionContext context)
        {
            var textFormatter = context.ServiceContainer.Get<ITextFormatter>();
            var path = textFormatter.Format(container.Path);

            _container = container;
            _containerPath = FileSystemPath.Parse(path);
        }

        public override IEnumerable<IObject> GetObjects(ISyncContext context)
        {
            var root = new DirectoryRoot(_container.Path, context.ExecutionContext.ServiceContainer.Get<IFileSystem>())
            {
                ChildrenOnly = true
            };
            var include = _container.Include;
            var exclude = _container.Exclude;

            foreach (var obj in new FileSystemObjectProvider(root, include, exclude).GetObjects(context.ExecutionContext))
            {
                obj.Metadata["BasePath"] = _containerPath;
                yield return obj;
            }
        }

        public override IObject FindObject(IObject obj, ISyncContext context)
        {
            if (obj is IFileObject)
                return FindFile((IFileObject)obj, context);

            if (obj is IDirectoryObject)
                return FindDirectory((IDirectoryObject)obj, context);

            throw new NotSupportedException();
        }

        private IObject FindFile(IFileObject file, ISyncContext context)
        {
            // TODO: Detect moved/renamed files to avoid unnecessary delete/copy operations

            var destination = new FileObject(GetObjectPathRelativeToThisContainer(file), context.ExecutionContext.ServiceContainer.Get<IFileSystem>());

            if (!destination.Exists)
                return null;

            return destination;
        }

        private IObject FindDirectory(IDirectoryObject directory, ISyncContext context)
        {
            var destination = new DirectoryObject(GetObjectPathRelativeToThisContainer(directory), context.ExecutionContext.ServiceContainer.Get<IFileSystem>());

            if (!destination.Exists)
                return null;

            return destination;
        }

        public override IEnumerable<IOperation> AddObject(IObject obj, ISyncContext context)
        {
            if (obj is IFileObject)
                return AddFile((IFileObject)obj, context);

            if (obj is IDirectoryObject)
                return AddDirectory((IDirectoryObject)obj, context);

            throw new NotSupportedException();
        }

        private IEnumerable<IOperation> AddFile(IFileObject file, ISyncContext context)
        {
            var destination = new FileObject(GetObjectPathRelativeToThisContainer(file), context.ExecutionContext.ServiceContainer.Get<IFileSystem>());

            if (!destination.Parent.Exists)
                yield return new CreateDirectoryOperation((IDirectoryObject)destination.Parent, context.ExecutionContext.ServiceContainer.Get<IFileSystem>(), context.ExecutionContext);

            yield return new CopyFileOperation(file, destination, context.ExecutionContext.ServiceContainer.Get<IFileSystem>(), context.ExecutionContext);
        }

        private IEnumerable<IOperation> AddDirectory(IDirectoryObject directory, ISyncContext context)
        {
            var destination = new DirectoryObject(GetObjectPathRelativeToThisContainer(directory), context.ExecutionContext.ServiceContainer.Get<IFileSystem>());

            yield return new CreateDirectoryOperation(destination, context.ExecutionContext.ServiceContainer.Get<IFileSystem>(), context.ExecutionContext);
        }

        public override IEnumerable<IChange> GetChanges(IObject obj, IObject baseObject, ISyncContext context)
        {
            if (obj is IFileObject && baseObject is IFileObject)
                return GetFileChanges((IFileObject)obj, (IFileObject)baseObject, context);

            if (obj is IDirectoryObject && baseObject is IDirectoryObject)
                return GetDirectoryChanges((IDirectoryObject)obj, (IDirectoryObject)baseObject, context);

            throw new NotSupportedException();
        }

        private IEnumerable<IChange> GetFileChanges(IFileObject file, IFileObject baseFile, ISyncContext context)
        {
            // TODO: More checks?

            if (file.Length != baseFile.Length)
            {
                yield return new ContentChange();
            }
            else if (file.LastWriteTimeUtc != baseFile.LastWriteTimeUtc)
            {
                // TODO: Make it optional
                yield return new ContentChange();
            }

            if (!string.Equals(file.ActualName, baseFile.ActualName, StringComparison.Ordinal))
                yield return new NameCasingChange();
        }

        private IEnumerable<IChange> GetDirectoryChanges(IDirectoryObject directory, IDirectoryObject baseDirectory, ISyncContext context)
        {
            if (!string.Equals(directory.ActualName, baseDirectory.ActualName, StringComparison.Ordinal))
                yield return new NameCasingChange();
        }

        public override IEnumerable<IOperation> UpdateObject(IObject obj, IObject baseObject, IChange change, ISyncContext context)
        {
            if (obj is IFileObject && baseObject is IFileObject)
                return UpdateFile((IFileObject)obj, (IFileObject)baseObject, change, context);

            if (obj is IDirectoryObject && baseObject is IDirectoryObject)
                return UpdateDirectory((IDirectoryObject)obj, (IDirectoryObject)baseObject, change, context);

            throw new NotSupportedException();
        }

        private IEnumerable<IOperation> UpdateFile(IFileObject file, IFileObject baseFile, IChange change, ISyncContext context)
        {
            if (change is ContentChange)
            {
                yield return new CopyFileOperation(baseFile, file, context.ExecutionContext.ServiceContainer.Get<IFileSystem>(), context.ExecutionContext);
            }
            else if (change is NameCasingChange)
            {
                yield return new RenameFileOperation(file, baseFile.Name, context.ExecutionContext.ServiceContainer.Get<IFileSystem>(), context.ExecutionContext);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private IEnumerable<IOperation> UpdateDirectory(IDirectoryObject directory, IDirectoryObject baseDirectory, IChange change, ISyncContext context)
        {
            if (change is NameCasingChange)
            {
                yield return new RenameDirectoryOperation(directory, baseDirectory.Name, context.ExecutionContext.ServiceContainer.Get<IFileSystem>(), context.ExecutionContext);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override IEnumerable<IOperation> RemoveObject(IObject obj, ISyncContext context)
        {
            if (obj is IFileObject)
                return RemoveFile((IFileObject)obj, context);

            if (obj is IDirectoryObject)
                return RemoveDirectory((IDirectoryObject)obj, context);

            throw new NotSupportedException();
        }

        private IEnumerable<IOperation> RemoveFile(IFileObject file, ISyncContext context)
        {
            // TODO: The file could have been deleted because of its parents being deleted

            yield return new DeleteFileOperation(file, context.ExecutionContext.ServiceContainer.Get<IFileSystem>(), context.ExecutionContext);
        }

        private IEnumerable<IOperation> RemoveDirectory(IDirectoryObject directory, ISyncContext context)
        {
            // TODO: Recursive?

            yield return new DeleteDirectoryOperation(directory, context.ExecutionContext.ServiceContainer.Get<IFileSystem>(), context.ExecutionContext);
        }

        private FileSystemPath GetObjectPathRelativeToThisContainer(IFileSystemObject obj)
        {
            var basePath = (FileSystemPath)obj.Metadata["BasePath"];

            return obj.Path.Rebase(basePath, _containerPath);
        }
    }
}
