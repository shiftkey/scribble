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
            return incompleteSnippets.Select(AsNotFoundMessage);
        }

        public static ScribbleError AsNotFoundMessage(CodeSnippet snippet)
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