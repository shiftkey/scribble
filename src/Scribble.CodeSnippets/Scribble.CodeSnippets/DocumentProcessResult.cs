using System.Collections.Generic;
using System.Linq;

namespace Scribble.CodeSnippets
{
    public class DocumentProcessResult
    {
        public DocumentProcessResult()
        {
            SnippetsUsed = new List<CodeSnippet>();
            
            Warnings = new object[0];
            Errors = new object[0];
        }

        public int Count { get; set; }

        public IEnumerable<object> Warnings { get; set; }

        public IEnumerable<object> Errors { get; set; }

        public bool HasMessages {
            get { return Warnings.Any() || Errors.Any(); }
        }

        public List<CodeSnippet> SnippetsUsed { get; set; }

        public void Include(List<CodeSnippet> snippets)
        {
            foreach (var snippet in snippets)
            {
                if (SnippetsUsed.Any(s => s.Key == snippet.Key))
                    continue;
               
                SnippetsUsed.Add(snippet);
            }
        }
    }
}