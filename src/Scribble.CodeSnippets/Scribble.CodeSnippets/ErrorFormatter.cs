using System.Collections.Generic;
using System.Linq;

namespace Scribble.CodeSnippets
{
    public class ErrorFormatter
    {
        public static IEnumerable<ScribbleMessage> FormatIncomplete(CodeSnippet[] incompleteSnippets)
        {
            return incompleteSnippets.Where(s => string.IsNullOrWhiteSpace(s.Value))
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

        public static IEnumerable<ScribbleMessage> Format(DocumentProcessResult incompleteSnippets)
        {
            return Enumerable.Empty<ScribbleMessage>();
        }

        public static IEnumerable<ScribbleMessage> Format(IEnumerable<object> messages)
        {
            return Enumerable.Empty<ScribbleMessage>();
        }

        public static IEnumerable<ScribbleMessage> FormatUnused(IEnumerable<CodeSnippet> codeSnippets)
        {
            return codeSnippets.Select(ToUnusedMessage);
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

        public static IEnumerable<ScribbleMessage> FormatNotFound(List<CodeSnippetReference> codeSnippets)
        {
            return codeSnippets.Select(ToRequiredMessage);
        }


        static ScribbleMessage ToRequiredMessage(CodeSnippetReference snippet)
        {
            return new ScribbleMessage
            {
                File = snippet.File,
                Message = string.Format("Could not find a code snippet for reference '{0}'", snippet.Key)
            };
        }

    }
}