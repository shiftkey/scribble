namespace Scribble.CodeSnippets
{
    public static class StringExtensions
    {
        public static string RemoveStart(this string target, int count)
        {
            return target.Substring(count, target.Length - count);
        }

        public static string TrimTrailingNewLine(this string target)
        {
            return target.TrimEnd('\r', '\n'); 
        }

        
    }
}