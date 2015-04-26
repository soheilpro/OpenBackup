using System;
using System.Xml.Linq;
using OpenBackup.Framework;

namespace OpenBackup.Extension.FileSystem
{
    [Loadable("Path")]
    public class PathRule : RuleBase
    {
        private FileSystemPath _path;

        public string Path
        {
            get;
            set;
        }

        public PathRule(string path, IServiceContainer serviceContainer)
        {
            var textExpander = serviceContainer.Get<ITextFormatter>();
            var fileSystem = serviceContainer.Get<IFileSystem>();

            _path = fileSystem.GetActualPath(FileSystemPath.Parse(textExpander.Format(path)));
            Path = path;
        }

        public PathRule(XElement element, ILoadingContext context) : this(element.Value, context.ServiceContainer)
        {
        }

        public override bool Matches(IObject obj, IExecutionContext context)
        {
            if (obj is IFileSystemObject)
                return Matches((IFileSystemObject)obj, context);

            return false;
        }

        private bool Matches(IFileSystemObject obj, IExecutionContext context)
        {
            var result = FileSystemPath.Compare(obj.Path, _path, StringComparison.OrdinalIgnoreCase);

            return result == FileSystemPath.ComparisonResult.YSubsetOfX ||
                   result == FileSystemPath.ComparisonResult.Equal;
        }

        public override XElement ToXml()
        {
            return new XElement("Path", Path);
        }
    }
}
