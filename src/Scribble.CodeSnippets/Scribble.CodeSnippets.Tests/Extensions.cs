using System;
using System.IO;
using System.Reflection;

namespace Scribble.CodeSnippet.Tests
{
    public static class Extensions
    {
        public static string ToCurrentDirectory(this string relativePath)
        {
            return Assembly.GetExecutingAssembly().ToCurrentDirectory(relativePath);
        }

        public static string ToCurrentDirectory(this Assembly assembly, string relativePath)
        {
            var fullPath = (new Uri(assembly.CodeBase)).AbsolutePath;
            var directory = Path.GetDirectoryName(fullPath);
            if (directory == null) throw new InvalidOperationException("The directory is null what even is it!");
            return Path.Combine(directory, relativePath);
        }
    }
}