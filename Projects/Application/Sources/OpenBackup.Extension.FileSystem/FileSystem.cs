using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenBackup.Extension.FileSystem
{
    [ServiceProvider(typeof(IFileSystem))]
    public partial class FileSystem : IFileSystem
    {
        public FileSystem(IServiceContainer serviceContainer)
        {
        }

        public bool Exists(IFileSystemObject obj)
        {
            if (obj is IDriveObject)
                return DriveExists((IDriveObject)obj);

            if (obj is IFileObject)
                return FileExists((IFileObject)obj);

            if (obj is IDirectoryObject)
                return DirectoryExists((IDirectoryObject)obj);

            throw new NotSupportedException();
        }

        private bool DriveExists(IDriveObject drive)
        {
            return DriveInfo.GetDrives().Any(d => string.Equals(d.Name, drive.Root.Path, StringComparison.OrdinalIgnoreCase));
        }

        private bool FileExists(IFileObject file)
        {
            return Win32.FileExists(file.Path);
        }

        private bool DirectoryExists(IDirectoryObject directory)
        {
            return Win32.DirectoryExists(directory.Path);
        }

        public FileAttributes GetAttributes(IFileSystemObject obj)
        {
            if (obj is IFileObject)
                return GetFileAttributes((IFileObject)obj);

            if (obj is IDirectoryObject)
                return GetDirectoryAttributes((IDirectoryObject)obj);

            throw new NotSupportedException();
        }

        private FileAttributes GetFileAttributes(IFileObject file)
        {
            return Win32.GetFileAttributes(file.Path);
        }

        private FileAttributes GetDirectoryAttributes(IDirectoryObject directory)
        {
            return Win32.GetDirectoryAttributes(directory.Path);
        }

        public IEnumerable<IDriveObject> GetDrives()
        {
            return from drive in DriveInfo.GetDrives()
                   select new DriveObject(drive.Name.Substring(0, 2), this);
        }

        public IEnumerable<IFileObject> GetFiles(IDirectoryObject directory)
        {
            return from file in Win32.EnumerateFiles(directory.Path)
                   select new FileObject(file, this);
        }

        public IEnumerable<IDirectoryObject> GetSubDirectories(IDirectoryObject directory)
        {
            return from subDirectory in Win32.EnumerateDirectories(directory.Path)
                   select new DirectoryObject(subDirectory, this);
        }

        public long GetFileLength(IFileObject file)
        {
            return Win32.GetFileLength(file.Path);
        }

        public DateTime GetCreationTimeUtc(IFileSystemObject obj)
        {
            if (obj is IFileObject)
                return GetFileCreationTimeUtc((IFileObject)obj);

            if (obj is IDirectoryObject)
                return GetDirectoryCreationTimeUtc((IDirectoryObject)obj);

            throw new NotSupportedException();
        }

        private DateTime GetFileCreationTimeUtc(IFileObject file)
        {
            return Win32.GetCreationTimeUtc(file.Path);
        }

        private DateTime GetDirectoryCreationTimeUtc(IDirectoryObject directory)
        {
            return Win32.GetCreationTimeUtc(directory.Path);
        }

        public DateTime GetLastWriteTimeUtc(IFileSystemObject obj)
        {
            if (obj is IFileObject)
                return GetLastFileWriteTimeUtc((IFileObject)obj);

            if (obj is IDirectoryObject)
                return GetLastDirectoryWriteTimeUtc((IDirectoryObject)obj);

            throw new NotSupportedException();
        }

        private DateTime GetLastFileWriteTimeUtc(IFileObject file)
        {
            return Win32.GetLastWriteTimeUtc(file.Path);
        }

        private DateTime GetLastDirectoryWriteTimeUtc(IDirectoryObject directory)
        {
            return Win32.GetLastWriteTimeUtc(directory.Path);
        }

        public DateTime GetLastAccessTimeUtc(IFileSystemObject obj)
        {
            if (obj is IFileObject)
                return GetLastFileAccessTimeUtc((IFileObject)obj);

            if (obj is IDirectoryObject)
                return GetLastDirectoryAccessTimeUtc((IDirectoryObject)obj);

            throw new NotSupportedException();
        }

        private DateTime GetLastFileAccessTimeUtc(IFileObject file)
        {
            return Win32.GetLastAccessTimeUtc(file.Path);
        }

        private DateTime GetLastDirectoryAccessTimeUtc(IDirectoryObject directory)
        {
            return Win32.GetLastAccessTimeUtc(directory.Path);
        }

        public DriveType GetDriveType(IDriveObject drive)
        {
            return new DriveInfo(drive.Name).DriveType;
        }

        public void CopyFile(FileSystemPath sourcePath, FileSystemPath destinationPath)
        {
            Win32.Copy(sourcePath, destinationPath, true);
        }

        public void CreateHardLink(FileSystemPath sourcePath, FileSystemPath destinationPath)
        {
            Win32.CreateHardLink(sourcePath, destinationPath);
        }

        public void Delete(IFileSystemObject obj)
        {
            if (obj is IFileObject)
            {
                DeleteFile((IFileObject)obj);
            }
            else if (obj is IDirectoryObject)
            {
                DeleteDirectory((IDirectoryObject)obj);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void DeleteFile(IFileObject file)
        {
            Win32.DeleteFile(file.Path);
        }

        public void DeleteDirectory(IDirectoryObject directory)
        {
            Win32.DeleteDirectory(directory.Path);
        }

        public void CreateDirectory(FileSystemPath path)
        {
            var parentDirectory = path.Parent;

            if (!Win32.DirectoryExists(parentDirectory))
                CreateDirectory(parentDirectory);

            Win32.CreateDirectory(path);
        }

        public void Rename(IFileSystemObject obj, string newName)
        {
            if (obj is IFileObject)
            {
                RenameFile((IFileObject)obj, newName);
            }
            else if (obj is IDirectoryObject)
            {
                RenameDirectory((IDirectoryObject)obj, newName);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void RenameFile(IFileObject file, string newName)
        {
            var newPath = file.Path.Parent.Combine(newName);

            Win32.Move(file.Path, newPath);
        }

        public void RenameDirectory(IDirectoryObject directory, string newName)
        {
            var newPath = directory.Path.Parent.Combine(newName);

            Win32.Move(directory.Path, newPath);
        }

        public FileSystemPath GetActualPath(FileSystemPath path)
        {
            return FileSystemPath.Parse(Win32.GetLongPathName(path));
        }

        public bool SupportsHardLinks(IDriveObject drive)
        {
            var driveFormat = new DriveInfo(drive.Name).DriveFormat;

            return string.Equals(driveFormat, "NTFS", StringComparison.OrdinalIgnoreCase);
        }
    }
}
