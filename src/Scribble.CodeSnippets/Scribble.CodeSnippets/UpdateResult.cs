using System.Collections.Generic;

namespace Scribble.CodeSnippets
{
    public class UpdateResult
    {
        public UpdateResult()
        {
            Messages = new List<ScribbleError>();
        }

        public int Snippets { get; set; }

        public List<ScribbleError> Messages { get; set; }

        public bool Completed { get; set; }

        public int Files { get; set; }
    }

    public class FooResult
    {
        public int Count { get; set; }
    }
}