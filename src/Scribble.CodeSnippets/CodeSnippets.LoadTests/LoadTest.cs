using System;
using System.IO;
using Scribble.CodeSnippets;
using Xunit;

namespace CodeSnippets.LoadTests
{
    public class LoadTest
    {
        [Fact(Skip="not really important right now")]
        public void ProcessTheJsonDotNetDocs()
        {
            var directory = @"D:\Code\github\shiftkey\Newtonsoft.Json";

            var codeFolder = directory;
            var docsFolder = Path.Combine(directory, @"docs\");
            var result = CodeImporter.Update(codeFolder, new[] { "*Tests.cs" }, docsFolder);

            Console.WriteLine("Completed: {0}", result.Completed);
            Console.WriteLine("Duration: {0}ms", result.ElapsedMilliseconds);

            foreach (var message in result.Errors)
            {
                Console.WriteLine("Error: {0}", message);
            }

            foreach (var message in result.Warnings)
            {
                Console.WriteLine("Warning: {0}", message);
            }
        }
    }
}
