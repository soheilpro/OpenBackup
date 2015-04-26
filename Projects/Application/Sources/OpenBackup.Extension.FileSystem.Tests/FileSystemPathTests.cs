using System;
using NUnit.Framework;

namespace OpenBackup.Extension.FileSystem.Tests
{
    [TestFixture]
    public class FileSystemPathTests
    {
        //[TestCase(@"")]
        //[TestCase(@"C:\")]
        //[TestCase(@"C:\a\b")]
        //[TestCase(@":")]
        //[TestCase(@"آ:")]
        //[TestCase(@"AB:")]
        //[TestCase(@"A")]
        //[TestCase(@" C:")]
        //[TestCase(@"C: ")]
        //[TestCase(@"C:\\")]
        //[TestCase(@"\\dir")]
        //[TestCase(@"C:\")]
        //[TestCase(@"C:\dir\")]
        //[TestCase(@"C:\\")]
        //[TestCase(@"\\file")]
        //[TestCase(@"\\dir\file")]

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Parse_Null()
        {
            var path = FileSystemPath.Parse(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_Empty()
        {
            var path = FileSystemPath.Parse(string.Empty);
        }

        [Test]
        public void Parse_SinglePart()
        {
            var path = FileSystemPath.Parse("a");

            Assert.AreEqual(1, path.PartCount);
            Assert.AreEqual("a", path[0]);
        }

        [Test]
        public void Parse_MultipleParts()
        {
            var path = FileSystemPath.Parse(@"a\b\c");

            Assert.AreEqual(3, path.PartCount);
            Assert.AreEqual("a", path[0]);
            Assert.AreEqual("b", path[1]);
            Assert.AreEqual("c", path[2]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_WithLeadingSeparator()
        {
            var path = FileSystemPath.Parse(@"\a");
        }

        [Test]
        public void Parse_WithTrailingSeparator()
        {
            var path = FileSystemPath.Parse(@"a\");

            Assert.AreEqual(1, path.PartCount);
            Assert.AreEqual("a", path[0]);
        }

        [Test]
        public void Roundtrip_SinglePart()
        {
            var path = FileSystemPath.Parse("a");

            Assert.AreEqual("a", path.ToString());
        }

        [Test]
        public void Roundtrip_MultiParts()
        {
            var path = FileSystemPath.Parse(@"a\b\c");

            Assert.AreEqual(@"a\b\c", path.ToString());
        }

        [Test]
        public void Roundtrip_WithTrailingSeparator()
        {
            var path = FileSystemPath.Parse(@"a\");

            Assert.AreEqual(@"a", path.ToString());
        }

        [Test]
        public void Combine_Singlepart_Singlepart()
        {
            var path1 = FileSystemPath.Parse(@"a");
            var path2 = FileSystemPath.Parse(@"b");
            var result = FileSystemPath.Combine(path1, path2);
            var expected = FileSystemPath.Parse(@"a\b");

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Combine_Multipart_Multipart()
        {
            var path1 = FileSystemPath.Parse(@"a\b");
            var path2 = FileSystemPath.Parse(@"c\d");
            var result = FileSystemPath.Combine(path1, path2);
            var expected = FileSystemPath.Parse(@"a\b\c\d");

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Compare_Different()
        {
            var path1 = FileSystemPath.Parse(@"a");
            var path2 = FileSystemPath.Parse(@"b");
            var result = FileSystemPath.Compare(path1, path2);

            Assert.AreEqual(FileSystemPath.ComparisonResult.Different, result);
        }

        [Test]
        public void Compare_Equal_Singlepart()
        {
            var path1 = FileSystemPath.Parse(@"a");
            var path2 = FileSystemPath.Parse(@"a");
            var result = FileSystemPath.Compare(path1, path2);

            Assert.AreEqual(FileSystemPath.ComparisonResult.Equal, result);
        }

        [Test]
        public void Compare_Equal_Multipart()
        {
            var path1 = FileSystemPath.Parse(@"a\b");
            var path2 = FileSystemPath.Parse(@"a\b");
            var result = FileSystemPath.Compare(path1, path2);

            Assert.AreEqual(FileSystemPath.ComparisonResult.Equal, result);
        }

        [Test]
        public void Compare_XSubsetOfY()
        {
            var path1 = FileSystemPath.Parse(@"a");
            var path2 = FileSystemPath.Parse(@"a\b");
            var result = FileSystemPath.Compare(path1, path2);

            Assert.AreEqual(FileSystemPath.ComparisonResult.XSubsetOfY, result);
        }

        [Test]
        public void Compare_YSubsetOfX()
        {
            var path1 = FileSystemPath.Parse(@"a\b");
            var path2 = FileSystemPath.Parse(@"a");
            var result = FileSystemPath.Compare(path1, path2);

            Assert.AreEqual(FileSystemPath.ComparisonResult.YSubsetOfX, result);
        }
    }
}
