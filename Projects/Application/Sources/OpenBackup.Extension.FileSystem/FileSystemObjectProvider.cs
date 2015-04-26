using System;
using System.Collections.Generic;
using System.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    public class FileSystemObjectProvider
    {
        private IFileSystemRoot[] _roots;

        private IRule[] _include;

        private IRule[] _exclude;

        public FileSystemObjectProvider(IEnumerable<IFileSystemRoot> roots, IEnumerable<IRule> include, IEnumerable<IRule> exclude)
        {
            if (roots == null)
                throw new ArgumentNullException("roots");

            _roots = roots.ToArray();
            _include = include != null ? include.ToArray() : new IRule[0];
            _exclude = exclude != null ? exclude.ToArray() : new IRule[0];

            foreach (var root in _roots)
                if (root == null)
                    throw new ArgumentNullException("root");

            foreach (var inc in _include)
                if (inc == null)
                    throw new ArgumentNullException("include");

            foreach (var exc in _exclude)
                if (exc == null)
                    throw new ArgumentNullException("exclude");
        }

        public FileSystemObjectProvider(IFileSystemRoot root, IEnumerable<IRule> include, IEnumerable<IRule> exclude) : this(new[] {root}, include, exclude)
        {
        }

        public IEnumerable<IFileSystemObject> GetObjects(IExecutionContext context)
        {
            foreach (var root in _roots)
            {
                foreach (var obj in root.GetObjects(context))
                {
                    if (!ShouldIncludeObject(obj, context))
                        continue;

                    if (ShouldExcludeObject(obj, context))
                        continue;

                    yield return obj;
                }
            }
        }

        private bool ShouldIncludeObject(IFileSystemObject obj, IExecutionContext context)
        {
            if (_include.Length == 0)
                return true;

            foreach (var include in _include)
                if (!include.Matches(obj, context))
                    return false;

            return true;
        }

        private bool ShouldExcludeObject(IFileSystemObject obj, IExecutionContext context)
        {
            if (_exclude.Length == 0)
                return false;

            foreach (var exclude in _exclude)
                if (exclude.Matches(obj, context))
                    return true;

            return false;
        }
    }
}
