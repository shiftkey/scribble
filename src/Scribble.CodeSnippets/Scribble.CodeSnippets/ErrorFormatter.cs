using System.Collections.Generic;
using System.Linq;

namespace Scribble.CodeSnippets
{
    public class ErrorFormatter
    {
        public static IEnumerable<string> Format(CodeSnippet[] incompleteSnippets)
        {
            return incompleteSnippets.Select(i =>
                    string.Format("Code snippet reference '{0}' was not closed (specify 'end code {0}').", i.Key));
        }
    }
}