using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenBackup.Extension.FileSystem.Tests
{
    public class MockFileSystem : IFileSystem
    {
        private Hashtable _driveType = new Hashtable();
        private Hashtable _supportsHardLinks = new Hashtable();
        private Hashtable _attributes = new Hashtable();
        private Hashtable _lengths = new Hashtable();
        private Hashtable _lastWriteTime = new Hashtable();

        /// <summary>
        /// TODO: Objects must be sorted
        /// </summary>
        public List<IFileSystemObject> Objects
        {
            get;
            private set;
        }

        public MockFileSystem()
        {
            Objects = new List<IFileSystemObject>();
        }

        public void RegisterDrive(string path, DriveType type = DriveType.Fixed, bool supportsHardLinks = true)
        {
            var drive = new DriveObject(path, this);

            Objects.Add(drive);
            _driveType[drive] = type;
            _supportsHardLinks[drive] = supportsHardLinks;
        }

        public void RegisterDirectory(string path, FileAttributes attributes = FileAttributes.Normal)
        {
            var directory = new DirectoryObject(path, this);

            EnsureParent(directory.Parent);

            Objects.Add(directory);
            _attributes[directory] = attributes | FileAttributes.Directory;
        }

        public void RegisterFile(string path, FileAttributes attributes = FileAttributes.Normal, long length = 0, DateTime lastWriteTime = default(DateTime))
        {
            var file = new FileObject(path, this);

            EnsureParent(file.Parent);

            Objects.Add(file);
            _attributes[file] = attributes;
            _lengths[file] = length;
            _lastWriteTime[file] = lastWriteTime;
        }

        private void EnsureParent(IFileSystemObject parent)
        {
            if (parent == null)
                return;

            if (Exists(parent))
                return;

            EnsureParent(parent.Parent);

            if (parent is DriveObject)
                RegisterDrive(parent.Path);
            else if (parent is DirectoryObject)
                RegisterDirectory(parent.Path);
        }

        private IFileSystemObject Find(FileSystemPath path)
        {
            return Objects.FirstOrDefault(o => FileSystemPath.Compare(o.Path, path, StringComparison.OrdinalIgnoreCase) == FileSystemPath.ComparisonResult.Equal);
        }

        private IFileSystemObject Find(IFileSystemObject obj)
        {
            return Objects.FirstOrDefault(o => o.GetType() == obj.GetType() && FileSystemPath.Compare(o.Path, obj.Path, StringComparison.OrdinalIgnoreCase) == FileSystemPath.ComparisonResult.Equal);
        }

        public bool Exists(IFileSystemObject obj)
        {
            return Find(obj) != null;
        }

        public IEnumerable<IDriveObject> GetDrives()
        {
            return Objects.OfType<IDriveObject>();
        }

        public IEnumerable<IDirectoryObject> GetSubDirectories(IDirectoryObject directory)
        {
            return Objects.OfType<IDirectoryObject>().Where(obj => obj.Parent != null && obj.Parent.Equals(directory));
        }

        public IEnumerable<IFileObject> GetFiles(IDirectoryObject directory)
        {
            return Objects.OfType<IFileObject>().Where(obj => obj.Parent != null && obj.Parent.Equals(directory));
        }

        public FileAttributes GetAttributes(IFileSystemObject obj)
        {
            return (FileAttributes)_attributes[obj];
        }

        public long GetFileLength(IFileObject file)
        {
            return (long)_lengths[Find(file)];
        }

        public DateTime GetCreationTimeUtc(IFileSystemObject obj)
        {
            throw new NotImplementedException();
        }

        public DateTime GetLastWriteTimeUtc(IFileSystemObject obj)
        {
            return (DateTime)_lastWriteTime[Find(obj)];
        }

        public DateTime GetLastAccessTimeUtc(IFileSystemObject obj)
        {
            throw new NotImplementedException();
        }

        public DriveType GetDriveType(IDriveObject drive)
        {
            return (DriveType)_driveType[Find(drive)];
        }

        public void CopyFile(FileSystemPath sourcePath, FileSystemPath destinationPath)
        {
            throw new NotImplementedException();
        }

        public void CreateHardLink(FileSystemPath sourcePath, FileSystemPath destinationPath)
        {
            throw new NotImplementedException();
        }

        public void Delete(IFileSystemObject obj)
        {
            throw new NotImplementedException();
        }

        public void CreateDirectory(FileSystemPath path)
        {
            throw new NotImplementedException();
        }

        public void Rename(IFileSystemObject obj, string newName)
        {
            throw new NotImplementedException();
        }

        public FileSystemPath GetActualPath(FileSystemPath path)
        {
            return Find(path).Path;
        }

        public bool SupportsHardLinks(IDriveObject drive)
        {
            return (bool)_supportsHardLinks[drive];
        }
    }
}
