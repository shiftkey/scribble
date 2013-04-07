using System.Collections.Generic;

namespace Scribble.CodeSnippets
{
    public class UpdateResult
    {
        public UpdateResult()
        {
            Errors = new List<ScribbleMessage>();
            Warnings = new List<ScribbleMessage>();
        }

        public bool Completed { get; set; }

        public int Snippets { get; set; }

        public int Files { get; set; }

        public List<ScribbleMessage> Warnings { get; set; }

        public List<ScribbleMessage> Errors { get; set; }
    }

    public class FileProcessResult
    {
        public string Text { get; set; }
    }
}