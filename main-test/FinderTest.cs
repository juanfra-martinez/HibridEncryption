using System.IO;
using main;
using Xunit;

namespace main_test
{
    public class FinderTest
    {
        private readonly string rootPath = "test";

        public FinderTest()
        {
        }

        [Fact]
        public void ShouldGetAllFilesFromDirectory()
        {
            var files = Finder.GetAllFilesFromDirectory(rootPath);

            Assert.NotEmpty(files);
        }

        [Fact]
        public void ShouldThrowExceptionBecauseFakeDirectory()
        {
            Assert.Throws<DirectoryNotFoundException>(
                () => Finder.GetAllFilesFromDirectory("foo")
            );
        }

        [Fact]
        public void ShouldExcludeFiles()
        {
            string fileA = "test/wimgapi.dll";
            string fileB = "test/Windows/Professional.xml";
            string fileC = "test/Windows/twain_32/wiatwain.ds";
            string fileD = "test/.hiddenFile";

            bool excluded =
                Finder.ExcludeFile(fileA) && Finder.ExcludeFile(fileB) &&
                Finder.ExcludeFile(fileC) && Finder.ExcludeFile(fileD);

            Assert.True(excluded);

        }
    }
}
