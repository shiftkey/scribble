using System.Collections.Generic;

namespace Scribble.CodeSnippets
{
    public class UpdateResult
    {
        public UpdateResult()
        {
            Messages = new List<string>();
        }

        public int Snippets { get; set; }

        public List<string> Messages { get; set; }

        public bool Completed { get; set; }

        public int Files { get; set; }
    }

    public class FooResult
    {
        public int Count { get; set; }
    }
}