using System.IO;
using System.Linq;
using System.Reflection;
using Scribble.CodeSnippets;
using Xunit;

namespace Scribble.CodeSnippet.Tests
{
    public class MiscellaneousTests
    {
        [Fact(Skip = "great more build server hilarity")]
        public void GetCodeSnippets_ReturnsMultipleResults_AllHaveValues()
        {
            var directory = GetCurrentDirectory(@"data\get-code-snippets\");
            var codeFile = Path.Combine(directory, @"code.cs");

            var actual = Importer.GetCodeSnippets(new[] { codeFile });

            Assert.True(actual.Count > 1);
            Assert.True(actual.All(c => !string.IsNullOrWhiteSpace(c.Value)));
        }

        [Fact(Skip = "great more build server hilarity")]
        public void ApplySnippets_UsingFile_MatchesExpectedResult()
        {
            var directory = GetCurrentDirectory(@"data\apply-snippets\");
            var codeFile = Path.Combine(directory, @"code.cs");
            var inputFile = Path.Combine(directory, @"input.md");
            var outputFile = Path.Combine(directory, @"output.md");

            var snippets = Importer.GetCodeSnippets(new[] { codeFile });
            var actual = Importer.ApplySnippets(snippets, inputFile);

            var expected = File.ReadAllText(outputFile);
            Assert.Equal(expected, actual);
        }

        [Fact(Skip = "great more build server hilarity")]
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
            var fullPath = (new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            var directory = Path.GetDirectoryName(fullPath);
            return Path.Combine(directory, relativePath);
        }
    }
}
