using System.Collections.Generic;
using System.Linq;

namespace Scribble.CodeSnippets
{
    public class ErrorFormatter
    {
        public static IEnumerable<ScribbleMessage> Format(CodeSnippet[] incompleteSnippets)
        {
            var noValuesFound = incompleteSnippets.Where(s => string.IsNullOrWhiteSpace(s.Value))
                                                  .Select(ToNotFoundMessage);

            return noValuesFound;
        }

        public static ScribbleMessage ToNotFoundMessage(CodeSnippet snippet)
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
    }
}