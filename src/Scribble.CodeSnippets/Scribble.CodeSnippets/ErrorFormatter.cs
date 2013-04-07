using System.Collections.Generic;
using System.Linq;

namespace Scribble.CodeSnippets
{
    public class ScribbleError
    {
        public string Message { get; set; }
        public string File { get; set; }
        public int LineNumber { get; set; }
    }

    public class ErrorFormatter
    {
        public static IEnumerable<ScribbleError> Format(CodeSnippet[] incompleteSnippets)
        {
            var noValuesFound = incompleteSnippets.Where(s => string.IsNullOrWhiteSpace(s.Value))
                                                  .Select(ToNotFoundMessage);

            return noValuesFound;
        }

        public static ScribbleError ToNotFoundMessage(CodeSnippet snippet)
        {
            return new ScribbleError
            {
                File = snippet.File,
                LineNumber = snippet.StartRow,
                Message = string.Format("Code snippet reference '{0}' was not closed (specify 'end code {0}').", snippet.Key)
            };
        }
    }
}