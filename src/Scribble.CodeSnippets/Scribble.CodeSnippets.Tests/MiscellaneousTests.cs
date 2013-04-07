using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Scribble.CodeSnippets;
using Xunit;

namespace Scribble.CodeSnippet.Tests
{
    public class MiscellaneousTests
    {
        [Fact]
        public void GetCodeSnippets_ReturnsMultipleResults_AllHaveValues()
        {
            var directory = GetCurrentDirectory(@"data\get-code-snippets\");

            var parser = new CodeFileParser(directory);
            var actual = parser.Parse(f => f.EndsWith("code.cs"));

            Assert.True(actual.Count > 1);
            Assert.True(actual.All(c => !string.IsNullOrWhiteSpace(c.Value)));
        }

        [Fact]
        public void GetCodeSnippets_WithNestedSnippets_ReturnsTwoValues()
        {
            var directory = GetCurrentDirectory(@"data\get-code-snippets\");
            
            var parser = new CodeFileParser(directory);
            var actual = parser.Parse(f => f.EndsWith("nested-code.cs"));

            Assert.Equal(2, actual.Count);
            Assert.True(actual.All(c => !string.IsNullOrWhiteSpace(c.Value)));
        }

        [Fact]
        public void ApplySnippets_UsingFile_MatchesExpectedResult()
        {
            var directory = GetCurrentDirectory(@"data\apply-snippets\");
            var inputFile = Path.Combine(directory, @"input.md");
            var outputFile = Path.Combine(directory, @"output.md");

            var parser = new CodeFileParser(directory);
            var snippets = parser.Parse(f => f.EndsWith("code.cs"));

            var actual = Importer.ApplySnippets(snippets, inputFile);

            var expected = File.ReadAllText(outputFile);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_UsingSourceAndDocsFolder_WillReturnCodeSnippetCount()
        {
            var directory = GetCurrentDirectory(@"data\test-site\");

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");
            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            Assert.Equal(14, result.Snippets);
        }

        [Fact]
        public void Update_UsingSourceAndDocsFolder_ReturnsTrue()
        {
            var directory = GetCurrentDirectory(@"data\test-site\");

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");
            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            Assert.Equal(14, result.Snippets);
        }

        [Fact]
        public void Update_UsingSourceAndDocsFolder_WillFormatWithCodeSnippet()
        {
            var directory = GetCurrentDirectory(@"data\test-site\");

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");
            Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            var indexFile = Path.Combine(directory, @"docs\index.md");
            var actual = File.ReadAllText(indexFile);

            var outputFile = Path.Combine(directory, @"output.md");
            var expected = File.ReadAllText(outputFile);

            Assert.Equal(expected, actual);
        }

        static string GetCurrentDirectory(string relativePath)
        {
            var fullPath = (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            var directory = Path.GetDirectoryName(fullPath);
            if (directory == null) throw new InvalidOperationException("The directory is null what even is it!");
            return Path.Combine(directory, relativePath);
        }
    }
}
