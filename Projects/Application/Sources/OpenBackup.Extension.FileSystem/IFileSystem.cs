using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBackup.Extension.FileSystem
{
    public interface IFileSystem
    {
        bool Exists(IFileSystemObject obj);

        FileAttributes GetAttributes(IFileSystemObject obj);

        IEnumerable<IDriveObject> GetDrives();

        IEnumerable<IDirectoryObject> GetSubDirectories(IDirectoryObject directory);

        IEnumerable<IFileObject> GetFiles(IDirectoryObject directory);

        long GetFileLength(IFileObject file);

        DateTime GetCreationTimeUtc(IFileSystemObject obj);

        DateTime GetLastWriteTimeUtc(IFileSystemObject obj);

        DateTime GetLastAccessTimeUtc(IFileSystemObject obj);

        DriveType GetDriveType(IDriveObject drive);

        void CopyFile(FileSystemPath sourcePath, FileSystemPath destinationPath);

        void CreateDirectory(FileSystemPath path);

        void CreateHardLink(FileSystemPath sourcePath, FileSystemPath destinationPath);

        void Delete(IFileSystemObject obj);

        void Rename(IFileSystemObject obj, string newName);

        FileSystemPath GetActualPath(FileSystemPath path);

        bool SupportsHardLinks(IDriveObject drive);
    }
}
