using System.Collections.Generic;

namespace Scribble.CodeSnippets.Models
{
    public class FileProcessResult
    {
        public FileProcessResult()
        {
            Snippets = new List<CodeSnippet>();
            RequiredSnippets = new List<CodeSnippetReference>();
        }

        public string Text { get; set; }

        public List<CodeSnippet> Snippets { get; set; }

        public IEnumerable<CodeSnippetReference> RequiredSnippets { get; set; }
    }
}