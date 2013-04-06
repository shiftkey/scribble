using System.IO;
using System.Reflection;
using Scribble.CodeSnippets;
using Xunit;

namespace Scribble.CodeSnippet.Tests
{
    public class ValidationTests
    {
        [Fact(Skip = "Todo")]
        public void Display_Message_When_Tag_Found_In_Docs_But_Not_Found_In_Code()
        {
            var directory = GetCurrentDirectory(@"data\validation\site-no-snippets\");

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            Assert.Contains("Could not find a code snippet for reference 'LinqToJsonCreateParse'", result.Messages);
            // TODO: include file path?
            // TODO: debug mode?
            // TODO: i18n?
        }

        [Fact(Skip = "Todo")]
        public void Display_Message_When_Code_Snippet_Defined_But_Not_Used_In_Docs()
        {
            var directory = GetCurrentDirectory(@"data\validation\site-no-reference\");

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            Assert.Contains("Code snippet reference 'LinqToJsonCreateParse' is not used in any pages and can be removed", result.Messages);
            // TODO: include file path? line?
            // TODO: debug mode?
        }

        [Fact]
        public void Display_Message_When_Unbalanced_Code_Snippet_Defined()
        {
            var directory = GetCurrentDirectory(@"data\validation\bad-snippet\");

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            Assert.False(result.Completed);
            Assert.Contains("Code snippet reference 'ThisIsAInvalidCodeSnippet' was not closed (specify 'end code ThisIsAInvalidCodeSnippet').", result.Messages);
            // TODO: include file path? line?
            // TODO: debug mode?
        }

        static string GetCurrentDirectory(string relativePath)
        {
            var fullPath = (new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            var directory = Path.GetDirectoryName(fullPath);
            return Path.Combine(directory, relativePath);
        }
    }
}
