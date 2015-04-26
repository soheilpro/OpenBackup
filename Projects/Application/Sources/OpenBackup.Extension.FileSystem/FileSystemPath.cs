using System;
using System.Linq;

namespace OpenBackup.Extension.FileSystem
{
    public class FileSystemPath : IEquatable<FileSystemPath>
    {
        private const string Separator = @"\";

        private const string UncPrefix = @"\\";

        private static readonly char[] DriveLetters = new[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        private string[] _parts;

        private string _fullPath;

        public string this[int index]
        {
            get
            {
                return _parts[index];
            }
        }

        public int PartCount
        {
            get
            {
                return _parts.Length;
            }
        }

        public string Name
        {
            get
            {
                return _parts[_parts.Length - 1];
            }
        }

        public FileSystemPath Parent
        {
            get
            {
                return GetSubPath(0, _parts.Length - 1);
            }
        }

        public bool IsLocal
        {
            get
            {
                return _parts[0].Length == 2 && _parts[0][1] == ':';
            }
        }

        public bool IsUnc
        {
            get
            {
                return (_parts[0].StartsWith(UncPrefix, StringComparison.Ordinal));
            }
        }

        public bool IsRelative
        {
            get
            {
                return !IsLocal && !IsUnc;
            }
        }

        private FileSystemPath(string[] parts)
        {
            _parts = parts;
        }

        public FileSystemPath GetSubPath(int startIndex, int length)
        {
            if (startIndex < 0 || startIndex > _parts.Length - 1)
                throw new ArgumentOutOfRangeException("startIndex");

            if (length < 0 || length > _parts.Length)
                throw new ArgumentOutOfRangeException("length");

            if (startIndex + length > _parts.Length)
                throw new ArgumentOutOfRangeException("length");

            if (length == 0)
                return null;

            var parts = new string[length];
            Array.Copy(_parts, startIndex, parts, 0, length);

            return new FileSystemPath(parts);
        }

        public FileSystemPath GetSubPath(int startIndex)
        {
            return GetSubPath(startIndex, _parts.Length - startIndex);
        }

        public FileSystemPath Rebase(FileSystemPath oldBase, FileSystemPath newBase)
        {
            if (oldBase == null)
                throw new Exception("oldBase");

            if (newBase == null)
                throw new Exception("newBase");

            if (GetSubPath(0, oldBase.PartCount) != oldBase)
                throw new ArgumentException("oldBase");

            return newBase.Combine(GetSubPath(oldBase.PartCount));
        }

        public FileSystemPath Combine(FileSystemPath other)
        {
            return Combine(this, other);
        }

        public FileSystemPath Combine(string other)
        {
            return Combine(this, Parse(other));
        }

        public bool Equals(FileSystemPath other)
        {
            return Compare(this, other) == ComparisonResult.Equal;
        }

        public override bool Equals(object other)
        {
            return Equals(other as FileSystemPath);
        }

        public bool IsSubpathOf(FileSystemPath other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            return Compare(this, other) == ComparisonResult.XSubsetOfY;
        }

        public bool IsSubpathOfOrEqualTo(FileSystemPath other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            var result = Compare(this, other);

            return result == ComparisonResult.XSubsetOfY ||
                   result == ComparisonResult.Equal;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            if (_fullPath == null)
                _fullPath = string.Join(Separator, _parts);

            return _fullPath;
        }

        public static FileSystemPath Parse(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path");

            // Remove trailing separator (if any)
            if (path.EndsWith(Separator, StringComparison.Ordinal))
                path = path.Substring(0, path.Length - 1);

            var isUnc = path.StartsWith(UncPrefix, StringComparison.Ordinal);

            if (isUnc)
                path = path.Remove(0, UncPrefix.Length);

            var parts = path.Split(new[] {Separator}, StringSplitOptions.None);

            if (parts.Length == 0)
                throw new ArgumentException("path");

            foreach (var part in parts)
                if (string.IsNullOrEmpty(part))
                    throw new ArgumentException("path");

            // TODO: Check for invalid chars in path

            if (isUnc)
                parts[0] = UncPrefix + parts[0];

            // Drive
            if (parts[0].Length == 2 && parts[0][1] == ':')
            {
                if (!DriveLetters.Contains(parts[0][0]))
                    throw new ArgumentException("path");
            }

            return new FileSystemPath(parts);
        }

        public static FileSystemPath Combine(FileSystemPath path1, FileSystemPath path2)
        {
            if (path1 == null)
                throw new ArgumentNullException("path1");

            if (path2 == null)
                throw new ArgumentNullException("path2");

            var parts = new string[path1._parts.Length + path2.PartCount];
            Array.Copy(path1._parts, 0, parts, 0, path1._parts.Length);
            Array.Copy(path2._parts, 0, parts, path1._parts.Length, path2._parts.Length);

            return new FileSystemPath(parts);
        }

        public static ComparisonResult Compare(FileSystemPath x, FileSystemPath y, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            // Both are null, or both are same instance?
            if (ReferenceEquals(x, y))
                return ComparisonResult.Equal;

            // x is null?
            if (ReferenceEquals(x, null))
                return ComparisonResult.Different;

            // y is null?
            if (ReferenceEquals(y, null))
                return ComparisonResult.Different;

            // Iterate through parts
            var pathPartIndex = 0;

            while (true)
            {
                // Get current part or null if reached the end
                var xPart = pathPartIndex <= x._parts.Length - 1 ? x._parts[pathPartIndex] : null;
                var yPart = pathPartIndex <= y._parts.Length - 1 ? y._parts[pathPartIndex] : null;

                // If both parts are null, they must be equal because either:
                //    1: both paths are empty,
                //    2: we're done iterating and there has been no differences,
                if (xPart == null && yPart == null)
                    return ComparisonResult.Equal;

                // If parts are inequal
                if (!string.Equals(xPart, yPart, comparison))
                {
                    // If this has happened at first iteration, they must be tottaly different
                    if (pathPartIndex == 0)
                        return ComparisonResult.Different;

                    // If x part is null (and since y part cannot be null), x must be a subset of y
                    if (xPart == null)
                        return ComparisonResult.XSubsetOfY;

                    // If y part is null (and since x part cannot be null), y must be a subset of x
                    if (yPart == null)
                        return ComparisonResult.YSubsetOfX;

                    // Now that neither parts are not null, they must be totally different
                    return ComparisonResult.Different;
                }

                // Both parts are equal, advance to next parts
                pathPartIndex++;
            }
        }

        public static bool AreEqual(FileSystemPath x, FileSystemPath y, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return Compare(x, y, comparison) == ComparisonResult.Equal;
        }

        public static bool IsXSubpathOfY(FileSystemPath x, FileSystemPath y, StringComparison comparison)
        {
            return Compare(x, y, comparison) == ComparisonResult.XSubsetOfY;
        }

        public static bool IsXSubpathOfOrEqualToY(FileSystemPath x, FileSystemPath y, StringComparison comparison)
        {
            var result = Compare(x, y, comparison);

            return result == ComparisonResult.XSubsetOfY ||
                   result == ComparisonResult.Equal;
        }

        public static implicit operator string(FileSystemPath path)
        {
            return path.ToString();
        }

        public static bool operator ==(FileSystemPath x, FileSystemPath y)
        {
            return Compare(x, y) == ComparisonResult.Equal;
        }

        public static bool operator !=(FileSystemPath x, FileSystemPath y)
        {
            return Compare(x, y) != ComparisonResult.Equal;
        }

        public static bool operator <(FileSystemPath x, FileSystemPath y)
        {
            return Compare(x, y) == ComparisonResult.XSubsetOfY;
        }

        public static bool operator <=(FileSystemPath x, FileSystemPath y)
        {
            var result = Compare(x, y);

            return result == ComparisonResult.XSubsetOfY ||
                   result == ComparisonResult.Equal;
        }

        public static bool operator >(FileSystemPath x, FileSystemPath y)
        {
            return Compare(x, y) == ComparisonResult.YSubsetOfX;
        }

        public static bool operator >=(FileSystemPath x, FileSystemPath y)
        {
            var result = Compare(x, y);

            return result == ComparisonResult.YSubsetOfX ||
                   result == ComparisonResult.Equal;
        }

        public enum ComparisonResult
        {
            Equal,
            Different,
            XSubsetOfY,
            YSubsetOfX
        }
    }
}
