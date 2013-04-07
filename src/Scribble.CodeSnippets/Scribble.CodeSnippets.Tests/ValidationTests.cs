using System.IO;
using System.Linq;
using Scribble.CodeSnippets;
using Xunit;

namespace Scribble.CodeSnippet.Tests
{
    public class ValidationTests
    {
        [Fact(Skip = "Todo")]
        public void Display_Message_When_Tag_Found_In_Docs_But_Not_Found_In_Code()
        {
            var directory = @"data\validation\site-no-snippets\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            Assert.False(result.Completed);

            var error = result.Errors.First();
            var message = error.Message;

            Assert.Equal(message, "Could not find a code snippet for reference 'LinqToJsonCreateParse'");
            // TODO: debug mode?
            // TODO: i18n?
        }


        [Fact]
        public void When_Code_Snippet_Defined_But_Not_Used_Returns_True()
        {
            var directory = @"data\validation\site-no-reference\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            Assert.True(result.Completed);
        }

        [Fact(Skip = "Todo")]
        public void When_Code_Snippet_Defined_But_Not_Used_In_Docs_Displays_Warning_Message()
        {
            var directory = @"data\validation\site-no-reference\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            var error = result.Errors.First();
            var message = error.Message;
            Assert.Equal(message, "Code snippet reference 'LinqToJsonCreateParse' is not used in any pages and can be removed");
            // TODO: include file path? line?
            // TODO: debug mode?
        }

        [Fact]
        public void When_Incomplete_Snippet_Found_Displays_Error_Message()
        {
            var directory = @"data\validation\bad-snippet\".ToCurrentDirectory();

            var codeFolder = Path.Combine(directory, @"source\");
            var docsFolder = Path.Combine(directory, @"docs\");

            var result = Importer.Update(codeFolder, new[] { "*.cs" }, docsFolder);

            var error = result.Errors.First();

            // message explains error
            Assert.Equal(error.Message, "Code snippet reference 'ThisIsAInvalidCodeSnippet' was not closed (specify 'end code ThisIsAInvalidCodeSnippet').");
            
            // file is as we expected
            Assert.True(error.File.StartsWith(codeFolder));
            Assert.True(error.File.EndsWith("code.cs"));

            // and we have the right line number to look at
            Assert.Equal(error.LineNumber, 30);
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
