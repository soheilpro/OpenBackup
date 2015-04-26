using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace OpenBackup.Extension.FileSystem
{
    public partial class FileSystem
    {
        private static partial class Win32
        {
            private const string LongPathPrefix = @"\\?\";

            public static bool FileExists(string path)
            {
                bool isDirectory;

                return Exists(path, out isDirectory) && !isDirectory;
            }

            public static bool DirectoryExists(string path)
            {
                bool isDirectory;

                return Exists(path, out isDirectory) && isDirectory;
            }

            private static bool Exists(string path, out bool isDirectory)
            {
                string normalizedPath;

                if (TryNormalizeLongPath(path, out normalizedPath))
                {
                    FileAttributes attributes;
                    var errorCode = TryGetFileAttributes(normalizedPath, out attributes);

                    if (errorCode == 0)
                    {
                        isDirectory = IsDirectory(attributes);
                        return true;
                    }
                }

                isDirectory = false;
                return false;
            }

            public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern = null)
            {
                return EnumerateFileSystemEntries(path, searchPattern, includeDirectories: true, includeFiles: false);
            }

            public static IEnumerable<string> EnumerateFiles(string path, string searchPattern = null)
            {
                return EnumerateFileSystemEntries(path, searchPattern, includeDirectories: false, includeFiles: true);
            }

            public static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern = null)
            {
                return EnumerateFileSystemEntries(path, searchPattern, includeDirectories: true, includeFiles: true);
            }

            private static IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, bool includeDirectories, bool includeFiles)
            {
                var normalizedSearchPattern = string.IsNullOrEmpty(searchPattern) || searchPattern == "." ? "*" : searchPattern;
                var normalizedPath = NormalizeLongPath(path);

                // First check whether the specified path refers to a directory and exists
                FileAttributes attributes;
                var errorCode = TryGetDirectoryAttributes(normalizedPath, out attributes);

                if (errorCode != 0)
                    throw GetExceptionFromWin32Error(errorCode);

                return EnumerateFileSystemIterator(normalizedPath, normalizedSearchPattern, includeDirectories, includeFiles);
            }

            private static IEnumerable<string> EnumerateFileSystemIterator(string normalizedPath, string normalizedSearchPattern, bool includeDirectories, bool includeFiles)
            {
                // NOTE: Any exceptions thrown from this method are thrown on a call to IEnumerator<string>.MoveNext()

                var path = RemoveLongPathPrefix(normalizedPath);
                NativeMethods.WIN32_FIND_DATA findData;

                using (var handle = BeginFind(Path.Combine(normalizedPath, normalizedSearchPattern), out findData))
                {
                    if (handle == null)
                        yield break;

                    do
                    {
                        var currentFileName = findData.cFileName;

                        if (IsDirectory(findData.dwFileAttributes))
                        {
                            if (includeDirectories && !IsCurrentOrParentDirectory(currentFileName))
                                yield return Path.Combine(path, currentFileName);
                        }
                        else
                        {
                            if (includeFiles)
                                yield return Path.Combine(path, currentFileName);
                        }
                    }
                    while (NativeMethods.FindNextFile(handle, out findData));

                    var errorCode = Marshal.GetLastWin32Error();

                    if (errorCode != NativeMethods.ERROR_NO_MORE_FILES)
                        throw GetExceptionFromWin32Error(errorCode);
                }
            }

            private static SafeFindHandle BeginFind(string normalizedPathWithSearchPattern, out NativeMethods.WIN32_FIND_DATA findData)
            {
                var handle = NativeMethods.FindFirstFile(normalizedPathWithSearchPattern, out findData);

                if (handle.IsInvalid)
                {
                    var errorCode = Marshal.GetLastWin32Error();

                    if (errorCode != NativeMethods.ERROR_FILE_NOT_FOUND)
                        throw GetExceptionFromWin32Error(errorCode);

                    return null;
                }

                return handle;
            }

            public static FileAttributes GetFileAttributes(string path)
            {
                return GetFileData(path).dwFileAttributes;
            }

            public static FileAttributes GetDirectoryAttributes(string path)
            {
                return GetFileData(path).dwFileAttributes;
            }

            private static int TryGetFileAttributes(string normalizedPath, out FileAttributes attributes)
            {
                attributes = NativeMethods.GetFileAttributes(normalizedPath);

                if ((int)attributes == NativeMethods.INVALID_FILE_ATTRIBUTES)
                    return Marshal.GetLastWin32Error();

                return 0;
            }

            private static int TryGetDirectoryAttributes(string normalizedPath, out FileAttributes attributes)
            {
                var errorCode = TryGetFileAttributes(normalizedPath, out attributes);

                if (!IsDirectory(attributes))
                    errorCode = NativeMethods.ERROR_DIRECTORY;

                return errorCode;
            }

            public static long GetFileLength(string path)
            {
                var fileData = GetFileData(path);

                return ((fileData.nFileSizeHigh << 0x20) | (fileData.nFileSizeLow & 0xffffffffL));
            }

            public static DateTime GetCreationTimeUtc(string path)
            {
                var fileData = GetFileData(path);

                return GetDateTime(fileData.ftCreationTime);
            }

            public static DateTime GetLastWriteTimeUtc(string path)
            {
                var fileData = GetFileData(path);

                return GetDateTime(fileData.ftLastWriteTime);
            }

            public static DateTime GetLastAccessTimeUtc(string path)
            {
                var fileData = GetFileData(path);

                return GetDateTime(fileData.ftLastAccessTime);
            }

            private static int TryGetFileData(string normalizedPath, out NativeMethods.WIN32_FILE_ATTRIBUTE_DATA fileData)
            {
                if (!NativeMethods.GetFileAttributesEx(normalizedPath, NativeMethods.GET_FILEEX_INFO_LEVELS.GetFileExInfoStandard, out fileData))
                    return Marshal.GetLastWin32Error();

                return 0;
            }

            private static NativeMethods.WIN32_FILE_ATTRIBUTE_DATA GetFileData(string path)
            {
                var normalizedPath = NormalizeLongPath(path);
                NativeMethods.WIN32_FILE_ATTRIBUTE_DATA fileData;

                var errorCode = TryGetFileData(normalizedPath, out fileData);

                if (errorCode != 0)
                    throw GetExceptionFromWin32Error(errorCode);

                return fileData;
            }

            private static DateTime GetDateTime(FILETIME time)
            {
                return DateTime.FromFileTimeUtc((((long)time.dwHighDateTime) << 0x20) | (uint)time.dwLowDateTime);
            }

            public static void CreateDirectory(string path)
            {
                var normalizedPath = NormalizeLongPath(path);

                if (!NativeMethods.CreateDirectory(normalizedPath, IntPtr.Zero))
                {
                    // To mimic Directory.CreateDirectory, we don't throw if the directory (not a file) already exists
                    var errorCode = Marshal.GetLastWin32Error();

                    if (errorCode != NativeMethods.ERROR_ALREADY_EXISTS || !DirectoryExists(path))
                        throw GetExceptionFromWin32Error(errorCode);
                }
            }

            public static void Copy(string sourcePath, string destinationPath, bool overwrite)
            {
                var normalizedSourcePath = NormalizeLongPath(sourcePath, "sourcePath");
                var normalizedDestinationPath = NormalizeLongPath(destinationPath, "destinationPath");

                if (!NativeMethods.CopyFile(normalizedSourcePath, normalizedDestinationPath, !overwrite))
                    throw GetExceptionFromLastWin32Error();
            }

            public static void CreateHardLink(string sourcePath, string destinationPath)
            {
                var normalizedSourcePath = NormalizeLongPath(sourcePath, "sourcePath");
                var normalizedDestinationPath = NormalizeLongPath(destinationPath, "destinationPath");

                if (!NativeMethods.CreateHardLink(normalizedDestinationPath, normalizedSourcePath, IntPtr.Zero))
                    throw GetExceptionFromLastWin32Error();
            }

            public static void Move(string sourcePath, string destinationPath)
            {
                var normalizedSourcePath = NormalizeLongPath(sourcePath, "sourcePath");
                var normalizedDestinationPath = NormalizeLongPath(destinationPath, "destinationPath");

                if (!NativeMethods.MoveFile(normalizedSourcePath, normalizedDestinationPath))
                    throw GetExceptionFromLastWin32Error();
            }

            public static void DeleteFile(string path)
            {
                var normalizedPath = NormalizeLongPath(path);

                if (!NativeMethods.DeleteFile(normalizedPath))
                    throw GetExceptionFromLastWin32Error();
            }

            public static void DeleteDirectory(string path)
            {
                var normalizedPath = NormalizeLongPath(path);

                if (!NativeMethods.RemoveDirectory(normalizedPath))
                    throw GetExceptionFromLastWin32Error();
            }

            /// <summary>
            /// Normalizes path (can be longer than MAX_PATH) and adds \\?\ long path prefix
            /// </summary>
            private static string NormalizeLongPath(string path, string parameterName = "path")
            {
                if (path == null)
                    throw new ArgumentNullException(parameterName);

                if (path.Length == 0)
                    throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "'{0}' cannot be an empty string.", parameterName), parameterName);

                var buffer = new StringBuilder(path.Length + 1); // Add 1 for NULL
                var length = NativeMethods.GetFullPathName(path, (uint)buffer.Capacity, buffer, IntPtr.Zero);

                if (length > buffer.Capacity)
                {
                    buffer.Capacity = (int)length;
                    length = NativeMethods.GetFullPathName(path, length, buffer, IntPtr.Zero);
                }

                if (length == 0)
                    throw GetExceptionFromLastWin32Error(parameterName);

                if (length > NativeMethods.MAX_LONG_PATH)
                    throw GetExceptionFromWin32Error(NativeMethods.ERROR_FILENAME_EXCED_RANGE, parameterName);

                return AddLongPathPrefix(buffer.ToString());
            }

            private static bool TryNormalizeLongPath(string path, out string result)
            {
                try
                {
                    result = NormalizeLongPath(path);
                    return true;
                }
                catch (ArgumentException)
                {
                }
                catch (PathTooLongException)
                {
                }

                result = null;
                return false;
            }

            public static string GetLongPathName(string path)
            {
                if (path == null)
                    throw new ArgumentNullException("path");

                if (path.Length == 0)
                    throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "'{0}' cannot be an empty string.", path), path);

                var normalizedPath = NormalizeLongPath(path);

                var buffer = new StringBuilder(normalizedPath.Length + 1); // Add 1 for NULL
                var length = NativeMethods.GetLongPathName(normalizedPath, buffer, buffer.Capacity);

                if (length > buffer.Capacity)
                {
                    buffer.Capacity = length;
                    length = NativeMethods.GetLongPathName(normalizedPath, buffer, buffer.Capacity);
                }

                if (length == 0)
                    throw GetExceptionFromLastWin32Error("path");

                if (length > NativeMethods.MAX_LONG_PATH)
                    throw GetExceptionFromWin32Error(NativeMethods.ERROR_FILENAME_EXCED_RANGE, "path");

                return RemoveLongPathPrefix(buffer.ToString());
            }

            private static string AddLongPathPrefix(string path)
            {
                return LongPathPrefix + path;
            }

            private static string RemoveLongPathPrefix(string normalizedPath)
            {
                return normalizedPath.Substring(LongPathPrefix.Length);
            }

            private static bool IsDirectory(FileAttributes attributes)
            {
                return (attributes & FileAttributes.Directory) == FileAttributes.Directory;
            }

            private static bool IsCurrentOrParentDirectory(string directoryName)
            {
                return directoryName.Equals(".", StringComparison.OrdinalIgnoreCase) ||
                       directoryName.Equals("..", StringComparison.OrdinalIgnoreCase);
            }

            private static Exception GetExceptionFromLastWin32Error(string parameterName = "path")
            {
                return GetExceptionFromWin32Error(Marshal.GetLastWin32Error(), parameterName);
            }

            private static Exception GetExceptionFromWin32Error(int errorCode, string parameterName = "path")
            {
                var message = GetMessageFromErrorCode(errorCode);

                switch (errorCode)
                {
                    case NativeMethods.ERROR_FILE_NOT_FOUND:
                        return new FileNotFoundException(message);

                    case NativeMethods.ERROR_PATH_NOT_FOUND:
                        return new DirectoryNotFoundException(message);

                    case NativeMethods.ERROR_ACCESS_DENIED:
                        return new UnauthorizedAccessException(message);

                    case NativeMethods.ERROR_FILENAME_EXCED_RANGE:
                        return new PathTooLongException(message);

                    case NativeMethods.ERROR_INVALID_DRIVE:
                        return new DriveNotFoundException(message);

                    case NativeMethods.ERROR_OPERATION_ABORTED:
                        return new OperationCanceledException(message);

                    case NativeMethods.ERROR_INVALID_NAME:
                        return new ArgumentException(message, parameterName);

                    default:
                        return new IOException(message, MakeHRFromErrorCode(errorCode));
                }
            }

            private static int MakeHRFromErrorCode(int errorCode)
            {
                return unchecked((int)0x80070000 | errorCode);
            }

            private static string GetMessageFromErrorCode(int errorCode)
            {
                var buffer = new StringBuilder(512);
                var bufferLength = NativeMethods.FormatMessage(NativeMethods.FORMAT_MESSAGE_IGNORE_INSERTS | NativeMethods.FORMAT_MESSAGE_FROM_SYSTEM | NativeMethods.FORMAT_MESSAGE_ARGUMENT_ARRAY, IntPtr.Zero, errorCode, 0, buffer, buffer.Capacity, IntPtr.Zero);

                Contract.Assert(bufferLength != 0);

                return buffer.ToString();
            }

            private sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
            {
                internal SafeFindHandle() : base(true)
                {
                }

                protected override bool ReleaseHandle()
                {
                    return NativeMethods.FindClose(base.handle);
                }
            }
        }
    }
}
