using System;
using System.Collections.Generic;
using System.Linq;
using OpenBackup.Extension.Backup;
using OpenBackup.Extension.FileSystem;
using OpenBackup.Framework;

namespace OpenBackup.Extension.DirectoryStorage
{
    public class DirectoryStorageInstance : StorageInstanceBase
    {
        private DirectoryStorage _storage;
        private IDirectoryObject _storageDirectory;
        private IDirectoryObject _backupDirectory;
        private IDirectoryObject[] _oldBackupDirectories;
        private bool _hardLinksEnabled;

        public FileSystemPath BackupDirectoryPath
        {
            get
            {
                return _backupDirectory.Path;
            }
        }

        public DirectoryStorageInstance(DirectoryStorage storage, IExecutionContext context)
        {
            var textFormatter = context.ServiceContainer.Get<ITextFormatter>();
            var fileSystem = context.ServiceContainer.Get<IFileSystem>();
            var path = textFormatter.Format(storage.Path);
            var directoryName = textFormatter.Format(storage.Options.DirectoryName);

            _storage = storage;
            _storageDirectory = new DirectoryObject(path, fileSystem);
            _backupDirectory = _storageDirectory.CombineDirectory(directoryName);

            if (_storageDirectory.Exists)
                _oldBackupDirectories = _storageDirectory.GetDirectories(context).ToArray();
            else
                _oldBackupDirectories = new DirectoryObject[0];

            _hardLinksEnabled = _storage.Options.EnableHardLinks && _storageDirectory.Drive.SupportsHardLinks;
        }

        public override IEnumerable<IOperation> Store(IObject obj, IBackupContext context)
        {
            if (obj is IFileObject)
                return StoreFile((IFileObject)obj, context.ExecutionContext);

            if (obj is IDirectoryObject)
                return StoreDirectory((IDirectoryObject)obj, context.ExecutionContext);

            if (obj is IFileSystemStorableObject)
                return StoreFileSystemStorable((IFileSystemStorableObject)obj, context.ExecutionContext);

            throw new NotSupportedException();
        }

        private IEnumerable<IOperation> StoreFile(IFileObject file, IExecutionContext context)
        {
            var fileStorePath = GetStorePath(file);
            var destination = _backupDirectory.CombineFile(fileStorePath);

            // TODO: Look in other places too? In case of possible moves/renames

            var oldBackupFiles = from oldBackupDirectory in _oldBackupDirectories
                                 let oldBackupFile = oldBackupDirectory.CombineFile(fileStorePath)
                                 where AreIdentical(file, oldBackupFile)
                                 select oldBackupFile;

            var oldBackupCopy = oldBackupFiles.FirstOrDefault();

            if (!destination.Parent.Exists)
                yield return new CreateDirectoryOperation((IDirectoryObject)destination.Parent, context.ServiceContainer.Get<IFileSystem>(), context);

            if (oldBackupCopy != null && _hardLinksEnabled)
                yield return new CreateHardLinkOperation(oldBackupCopy, destination, context.ServiceContainer.Get<IFileSystem>(), context);
            else
                yield return new CopyFileOperation(file, destination, context.ServiceContainer.Get<IFileSystem>(), context);

            // TODO: Should set dates as well?
            // TODO: Should set attributes?
        }

        private IEnumerable<IOperation> StoreDirectory(IDirectoryObject directory, IExecutionContext context)
        {
            var directoryStorePath = GetStorePath(directory);
            var destination = _backupDirectory.CombineDirectory(directoryStorePath);

            yield return new CreateDirectoryOperation(destination, context.ServiceContainer.Get<IFileSystem>(), context);

            // TODO: Should set dates as well?
            // TODO: Should set attributes?
        }

        private IEnumerable<IOperation> StoreFileSystemStorable(IFileSystemStorableObject filesystemStorableObject, IExecutionContext context)
        {
            return filesystemStorableObject.Store(_backupDirectory, context.ServiceContainer.Get<IFileSystem>(), context);
        }

        private static string GetStorePath(IFileSystemObject obj)
        {
            return ((string)obj.Path).Replace(":", string.Empty);
        }

        private static bool AreIdentical(IFileObject file1, IFileObject file2)
        {
            if (file1 == null || file2 == null)
                return false;

            if (!file1.Exists || !file2.Exists)
                return false;

            if (file1.Length != file2.Length)
                return false;

            if (file1.LastWriteTimeUtc != file2.LastWriteTimeUtc)
                return false;

            // TODO: More checks?

            return true;
        }
    }
}
