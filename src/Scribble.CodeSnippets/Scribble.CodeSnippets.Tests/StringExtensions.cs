namespace Scribble.CodeSnippet.Tests
{
    public static class StringExtensions
    {
        //TODo: hack to get around git newlines. needs fixing
        public static string FixNewLines(this string target)
        {
            return target.Replace("\r\n", "\n");
        }
    }
}