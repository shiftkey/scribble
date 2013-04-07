using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Scribble.CodeSnippets;
using Xunit;
using Xunit.Extensions;

namespace Scribble.CodeSnippet.Tests
{
    public class ImportTestSuite
    {
        [Theory, PropertyData("Scenarios")]
        public void Scenario(string name, string folder, string inputFile, string expectedFile)
        {
            var expectedContents = File.ReadAllText(expectedFile);

            var parser = new CodeFileParser(folder);
            var snippets = parser.Parse(f => f.EndsWith("code.cs"));

            var actual = DocumentFileProcessor.Apply(snippets, inputFile);

            Assert.Equal(expectedContents, actual);
        }

        public static IEnumerable<object[]> Scenarios
        {
            get
            {
                var directory = GetCurrentDirectory(@"scenarios\");
                var folders = Directory.GetDirectories(directory);
                
                return (from folder in folders
                        let info = new DirectoryInfo(folder)
                        let name = info.Name
                        let inputFile = Path.Combine(folder, "input.md")
                        let outputFile = Path.Combine(folder, "output.md")
                        select new object[] { name, folder, inputFile, outputFile }).ToList();
            }
        }

        [Obsolete]
        static string GetCurrentDirectory(string relativePath)
        {
            var fullPath = (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            var directory = Path.GetDirectoryName(fullPath);
            if (directory == null) throw new InvalidOperationException("The directory is null what even is it!");
            return Path.Combine(directory, relativePath);
        }
    }
}