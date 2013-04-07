using System.Collections.Generic;
using System.Linq;

namespace Scribble.CodeSnippets
{
    public class DocumentProcessResult
    {
        public DocumentProcessResult()
        {
            Warnings = new object[0];
            Errors = new object[0];
        }

        public int Count { get; set; }

        public IEnumerable<object> Warnings { get; set; }

        public IEnumerable<object> Errors { get; set; }

        public bool HasMessages {
            get { return Warnings.Any() || Errors.Any(); }
        }
    }
}