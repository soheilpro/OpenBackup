using System;
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
            private static class NativeMethods
            {
                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool CopyFile(string src, string dst, [MarshalAs(UnmanagedType.Bool)] bool failIfExists);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                public static extern SafeFindHandle FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool FindNextFile(SafeFindHandle hFindFile, out WIN32_FIND_DATA lpFindFileData);

                [DllImport("kernel32.dll", SetLastError = true)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool FindClose(IntPtr hFindFile);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                public static extern uint GetFullPathName(string lpFileName, uint nBufferLength, StringBuilder lpBuffer, IntPtr mustBeNull);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                public static extern int GetLongPathName(string lpszShortPath, StringBuilder lpszLongPath, int cchBuffer);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool DeleteFile(string lpFileName);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool RemoveDirectory(string lpPathName);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool CreateDirectory(string lpPathName, IntPtr lpSecurityAttributes);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool MoveFile(string lpPathNameFrom, string lpPathNameTo);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                public static extern SafeFileHandle CreateFile(string lpFileName, EFileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                public static extern FileAttributes GetFileAttributes(string lpFileName);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool GetFileAttributesEx(string lpFileName, GET_FILEEX_INFO_LEVELS fInfoLevelId, out WIN32_FILE_ATTRIBUTE_DATA lpFileInformation);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                public static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);

                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                public static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

                public const int ERROR_FILE_NOT_FOUND = 0x2;
                public const int ERROR_PATH_NOT_FOUND = 0x3;
                public const int ERROR_ACCESS_DENIED = 0x5;
                public const int ERROR_INVALID_DRIVE = 0xf;
                public const int ERROR_NO_MORE_FILES = 0x12;
                public const int ERROR_INVALID_NAME = 0x7B;
                public const int ERROR_ALREADY_EXISTS = 0xB7;
                public const int ERROR_FILENAME_EXCED_RANGE = 0xCE;
                public const int ERROR_DIRECTORY = 0x10B;
                public const int ERROR_OPERATION_ABORTED = 0x3e3;
                public const int INVALID_FILE_ATTRIBUTES = -1;

                public const int MAX_PATH = 260;
                public const int MAX_LONG_PATH = 32000;
                public const int MAX_ALTERNATE = 14;

                public const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
                public const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
                public const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000;

                [Flags]
                public enum EFileAccess : uint
                {
                    GenericRead = 0x80000000,
                    GenericWrite = 0x40000000,
                    GenericExecute = 0x20000000,
                    GenericAll = 0x10000000,
                }

                [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
                public struct WIN32_FIND_DATA
                {
                    public FileAttributes dwFileAttributes;
                    public FILETIME ftCreationTime;
                    public FILETIME ftLastAccessTime;
                    public FILETIME ftLastWriteTime;
                    public int nFileSizeHigh;
                    public int nFileSizeLow;
                    public int dwReserved0;
                    public int dwReserved1;

                    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
                    public string cFileName;

                    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ALTERNATE)]
                    public string cAlternate;
                }

                public enum GET_FILEEX_INFO_LEVELS
                {
                    GetFileExInfoStandard,
                    GetFileExMaxInfoLevel
                }

                [StructLayout(LayoutKind.Sequential)]
                public struct WIN32_FILE_ATTRIBUTE_DATA
                {
                    public FileAttributes dwFileAttributes;
                    public FILETIME ftCreationTime;
                    public FILETIME ftLastAccessTime;
                    public FILETIME ftLastWriteTime;
                    public uint nFileSizeHigh;
                    public uint nFileSizeLow;
                }
            }
        }
    }
}
