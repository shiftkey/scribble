using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Scribble.CodeSnippets;
using Xunit;

namespace Scribble.CodeSnippet.Tests
{
    public class CodeFileParserTests
    {
        [Fact]
        public void GetCodeSnippets_ReturnsMultipleResults_AllHaveValues()
        {
            var directory = @"data\get-code-snippets\".ToCurrentDirectory();

            var parser = new CodeFileParser(directory);
            var actual = parser.Parse(f => f.EndsWith("code.cs"));

            Assert.True(actual.Count > 1);
            Assert.True(actual.All(c => !string.IsNullOrWhiteSpace(c.Value)));
        }

        [Fact]
        public void GetCodeSnippets_WithNestedSnippets_ReturnsTwoValues()
        {
            var directory = @"data\get-code-snippets\".ToCurrentDirectory();
            
            var parser = new CodeFileParser(directory);
            var actual = parser.Parse(f => f.EndsWith("nested-code.cs"));

            Assert.Equal(2, actual.Count);
            Assert.True(actual.All(c => !string.IsNullOrWhiteSpace(c.Value)));
        }

        [Fact]
        public void ApplySnippets_UsingFile_MatchesExpectedResult()
        {
            var directory = @"data\apply-snippets\".ToCurrentDirectory();
            var inputFile = Path.Combine(directory, @"input.md");
            var outputFile = Path.Combine(directory, @"output.md");

            var parser = new CodeFileParser(directory);
            var snippets = parser.Parse(f => f.EndsWith("code.cs"));

            var actual = DocumentFileProcessor.Apply(snippets, inputFile);

            var expected = File.ReadAllText(outputFile);
            Assert.Equal(expected, actual);
        }
    }
}
