using System.Collections.Generic;
using System.Linq;
using Scribble.CodeSnippets.Models;

namespace Scribble.CodeSnippets
{
    public class ErrorFormatter
    {
        public static IEnumerable<ScribbleMessage> FormatIncomplete(IEnumerable<CodeSnippet> snippets)
        {
            return snippets.Where(s => string.IsNullOrWhiteSpace(s.Value))
                           .Select(ToNotFoundMessage);
        }

        static ScribbleMessage ToNotFoundMessage(CodeSnippet snippet)
        {
            return new ScribbleMessage
            {
                File = snippet.File,
                LineNumber = snippet.StartRow,
                Message = string.Format("Code snippet reference '{0}' was not closed (specify 'end code {0}').", snippet.Key)
            };
        }

        public static IEnumerable<ScribbleMessage> FormatUnused(IEnumerable<CodeSnippet> snippets)
        {
            return snippets.Select(ToUnusedMessage);
        }

        static ScribbleMessage ToUnusedMessage(CodeSnippet snippet)
        {
            return new ScribbleMessage
            {
                File = snippet.File,
                LineNumber = snippet.StartRow,
                Message = string.Format("Code snippet reference '{0}' is not used in any pages and can be removed", snippet.Key)
            };
        }

        public static IEnumerable<ScribbleMessage> FormatNotFound(List<CodeSnippetReference> snippets)
        {
            return snippets.Select(ToRequiredMessage);
        }

        static ScribbleMessage ToRequiredMessage(CodeSnippetReference snippet)
        {
            return new ScribbleMessage
            {
                File = snippet.File,
                LineNumber = snippet.LineNumber,
                Message = string.Format("Could not find a code snippet for reference '{0}'", snippet.Key)
            };
        }

    }
}