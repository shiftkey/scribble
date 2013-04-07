using System.IO;
using Scribble.CodeSnippets;
using Xunit;

namespace Scribble.CodeSnippet.Tests
{
    public class ImporterTests
    {
        [Fact]
        public void Update_UsingSourceAndDocsFolder_WillReturnCodeSnippetCount()
        {
            var directory = @"data\test-site\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");
            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            Assert.Equal(14, result.Snippets);
        }

        [Fact]
        public void Update_UsingSourceAndDocsFolder_ReturnsTrue()
        {
            var directory = @"data\test-site\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");
            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            Assert.Equal(14, result.Snippets);
        }

        [Fact]
        public void Update_UsingSourceAndDocsFolder_WillFormatWithCodeSnippet()
        {
            var directory = @"data\test-site\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");
            Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            var indexFile = Path.Combine(directory, @"docs\index.md");
            var actual = File.ReadAllText(indexFile);

            var outputFile = Path.Combine(directory, @"output.md");
            var expected = File.ReadAllText(outputFile);

            Assert.Equal(expected, actual);
        }
    }
}