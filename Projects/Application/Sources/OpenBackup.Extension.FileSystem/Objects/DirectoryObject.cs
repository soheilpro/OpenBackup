using System;
using System.Collections.Generic;
using System.IO;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    public class DirectoryObject : FileSystemObjectBase, IDirectoryObject
    {
        public FileAttributes Attributes
        {
            get
            {
                return FileSystem.GetAttributes(this);
            }
        }

        public IDriveObject Drive
        {
            get
            {
                return new DriveObject(Path[0], FileSystem);
            }
        }

        public DirectoryObject(FileSystemPath path, IFileSystem fileSystem) : base(path, fileSystem)
        {
        }

        public DirectoryObject(string path, IFileSystem fileSystem) : this(FileSystemPath.Parse(path), fileSystem)
        {
        }

        public IEnumerable<IDirectoryObject> GetDirectories(IExecutionContext context)
        {
            if (!Exists)
                return new IDirectoryObject[0];

            try
            {
                return FileSystem.GetSubDirectories(this);
            }
            catch (Exception exception)
            {
                context.RegisterError(new ExecutionException(exception));

                return new IDirectoryObject[0];
            }
        }

        public IEnumerable<IFileObject> GetFiles(IExecutionContext context)
        {
            if (!Exists)
                return new IFileObject[0];

            try
            {
                return FileSystem.GetFiles(this);
            }
            catch (Exception exception)
            {
                context.RegisterError(new ExecutionException(exception));

                return new IFileObject[0];
            }
        }

        public IEnumerable<IFileSystemObject> GetChildren(bool recursive, IExecutionContext context)
        {
            ExceptionHandler exceptionHandler = (Exception exception) => { context.RegisterError(new ExecutionException(exception)); };

            foreach (var file in GetFiles(context).Catch(exceptionHandler))
                yield return file;

            foreach (var subDirectory in GetDirectories(context).Catch(exceptionHandler))
            {
                yield return subDirectory;

                if (recursive)
                    foreach (var subDirectoryChild in subDirectory.GetChildren(recursive, context))
                        yield return subDirectoryChild;
            }
        }

        public IDirectoryObject CombineDirectory(string path)
        {
            return new DirectoryObject(Path.Combine(path), FileSystem);
        }

        public IFileObject CombineFile(string path)
        {
            return new FileObject(Path.Combine(path), FileSystem);
        }

        public override IEnumerable<IMetadata> GetMetadata()
        {
            foreach (var metadata in base.GetMetadata())
                yield return metadata;

            yield return new Metadata("Attributes", Attributes);

            // TODO
            // yield return new Metadata("CreationTimeUtc", CreationTimeUtc);
            // yield return new Metadata("LastWriteTimeUtc", LastWriteTimeUtc);
            // yield return new Metadata("LastAccessTimeUtc", LastAccessTimeUtc);
        }
    }
}
