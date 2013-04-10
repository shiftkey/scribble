namespace Scribble.CodeSnippets.Models
{
    public class ScribbleMessage
    {
        public string Message { get; set; }
        public string File { get; set; }
        public int LineNumber { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, File: {1}, Line: {2}", Message, File, LineNumber);
        }
    }
}