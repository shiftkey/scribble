namespace Scribble.CodeSnippets.Models
{
    public class CodeSnippet
    {
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public string Value { get; set; }
        public string Key { get; set; }
        public string Language { get; set; }
        public string File { get; set; }
    }
}