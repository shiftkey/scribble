using System.Diagnostics;
using System.Linq;
using Scribble.CodeSnippets;
using Xunit;

namespace Scribble.CodeSnippet.Tests
{
    public class DocumentFileProcessorTests
    {
        [Fact]
        public void MissingKey()
        {
            var codeSnippets = new []
                               {
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey1"},
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey2"},
                               };
            var codeSnippetReferences = DocumentFileProcessor.CheckMissingKeys(codeSnippets,"<!-- import MissingKey -->");
            var codeSnippetReference = codeSnippetReferences.First();
            Assert.Equal("MissingKey", codeSnippetReference.Key);
            Assert.Equal(1, codeSnippetReference.LineNumber);
        }
        [Fact]
        public void MissingMultipleKeys()
        {
            var codeSnippets = new []
                               {
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey1"},
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey2"},
                               };
            var codeSnippetReferences = DocumentFileProcessor.CheckMissingKeys(codeSnippets, "<!-- import MissingKey1 -->\r\n\r\n<!-- import MissingKey2 -->");
            var first = codeSnippetReferences.First();
            Assert.Equal("MissingKey1", first.Key);
            Assert.Equal(1, first.LineNumber);
            var second = codeSnippetReferences.Skip(1).First();
            Assert.Equal("MissingKey2", second.Key);
            Assert.Equal(3, second.LineNumber);
        }
        [Fact]
        public void FoundKey()
        {
            var codeSnippets = new []
                               {
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey1"},
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey2"},
                               };
            var codeSnippetReferences = DocumentFileProcessor.CheckMissingKeys(codeSnippets, "<!-- import FoundKey2 -->");
            Assert.Empty( codeSnippetReferences);
        }
        [Fact]
        public void FoundMultipleKeys()
        {
            var codeSnippets = new []
                               {
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey1"},
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey2"},
                               };
            var codeSnippetReferences = DocumentFileProcessor.CheckMissingKeys(codeSnippets, "<!-- import FoundKey2 -->\r\b\n<!-- import FoundKey1 -->");
            Assert.Empty( codeSnippetReferences);
        }
        [Fact]
        public void LotsOfText()
        {
            var codeSnippets = new []
                               {
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey1"},
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey2"},
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey4"},
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey7"},
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey2"},
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey5"},
                                   new CodeSnippets.Models.CodeSnippet {Key = "FoundKey1"},
                               };
            var startNew = Stopwatch.StartNew();
            var codeSnippetReferences = DocumentFileProcessor.CheckMissingKeys(codeSnippets, @"<!-- import FoundKey2 -->\r\b\n<!-- import FoundKey1 -->
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmgfkgjnfdjkgn<!-- import FoundKey1 -->
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg<!-- import FoundKey1 -->
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmg<!-- import FoundKey1 -->
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg<!-- import FoundKey1 -->
kdjrngkjfncgdflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg<!-- import FoundKey1 -->
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg<!-- import FoundKey1 -->
kdjrngkjfncgdflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmgfkgjnfdjkgn<!-- import FoundKey1 --><!-- import FoundKey1 -->
dflkgmxdklfmgkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmgfkgjnfdjkgn
dflkgmxdklfmgkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmdfgkjndfkjgngkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
kdjrngkjfncgdflkgmxdklfmgkdflxmg<!-- import FoundKey1 -->
kdjrngkjfncgdflkgmxdklfmgkdflxmg
dflkgmxdklfmgkdflxmg
lkmdflkgmxdklfmgkdflxmg
");
            Debug.WriteLine(startNew.ElapsedTicks);
        }
    }
}
