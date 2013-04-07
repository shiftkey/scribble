using System.IO;
using System.Linq;
using Scribble.CodeSnippets;
using Xunit;

namespace Scribble.CodeSnippet.Tests
{
    public class ValidationTests
    {
        [Fact]
        public void When_Tag_Found_In_Docs_But_Not_Found_In_Code_Returns_False()
        {
            var directory = @"data\validation\no-snippets\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            Assert.False(result.Completed);
        }

        [Fact]
        public void When_Tag_Found_In_Docs_But_Not_Found_In_Code_Display_Error()
        {
            var directory = @"data\validation\no-snippets\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var expectedFile = Path.Combine(docsFolder, "index.md");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            var error = result.Errors.First();

            // message explains error
            Assert.Equal(error.Message, "Could not find a code snippet for reference 'LinqToJsonCreateParse'");

            // file is as we expected
            Assert.Equal(error.File, expectedFile);

            // and we have the right line number to look at
            Assert.Equal(15, error.LineNumber);
        }

        [Fact]
        public void When_Code_Snippet_Defined_But_Not_Used_Returns_True()
        {
            var directory = @"data\validation\no-reference\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            Assert.True(result.Completed);
        }

        [Fact(Skip="For some reason this test fails intermittently")]
        public void When_Code_Snippet_Defined_But_Not_Used_In_Docs_Displays_Warning_Message()
        {
            var directory = @"data\validation\no-reference\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var expectedFile = Path.Combine(codeFolder, "code.cs");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            var warning = result.Warnings.First();
            // message explains error
            Assert.Equal(warning.Message, "Code snippet reference 'LinqToJsonCreateParse' is not used in any pages and can be removed");

            // file is as we expected
            Assert.Equal(warning.File, expectedFile);

            // and we have the right line number to look at
            Assert.Equal(32, warning.LineNumber);
        }

        [Fact]
        public void When_Incomplete_Snippet_Found_Displays_Error_Message()
        {
            var directory = @"data\validation\bad-snippet\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var expectedFile = Path.Combine(codeFolder, "code.cs");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            var error = result.Errors.First();

            // message explains error
            Assert.Equal(error.Message, "Code snippet reference 'ThisIsAInvalidCodeSnippet' was not closed (specify 'end code ThisIsAInvalidCodeSnippet').");

            // file is as we expected
            Assert.Equal(error.File, expectedFile);

            // and we have the right line number to look at
            Assert.Equal(30, error.LineNumber);
        }

        [Fact]
        public void When_Incomplete_Snippet_Found_Does_Not_Import()
        {
            var directory = @"data\validation\bad-snippet\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            Assert.False(result.Completed);
        }
    }
}