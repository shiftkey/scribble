using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Scribble.CodeSnippets.Infrastructure;
using Scribble.CodeSnippets.Models;

namespace Scribble.CodeSnippets
{
    public class CodeFileParser
    {
        const string LineEnding = "\r\n";

        readonly string codeFolder;

        public CodeFileParser(string codeFolder)
        {
            this.codeFolder = codeFolder;
        }

        public ICollection<CodeSnippet> Parse(string[] filterOnExpression)
        {
            var filesMatchingExtensions = new List<string>();

            var allFiles = Directory.GetFiles(codeFolder, "*.*", SearchOption.AllDirectories);
            foreach (var expression in filterOnExpression)
            {
                try
                {
                    var regex = new Regex(expression);
                    filesMatchingExtensions.AddRange(allFiles.Where(f => regex.IsMatch(f)));
                }
                catch (Exception)
                {
                    var files = Directory.GetFiles(codeFolder, expression, SearchOption.AllDirectories);
                    filesMatchingExtensions.AddRange(files);
                }
            }
            return GetCodeSnippets(filesMatchingExtensions.Where(x => !x.Contains(@"\obj\"))
                .Distinct());
        }

        public static IList<CodeSnippet> GetCodeSnippets(IEnumerable<string> codeFiles)
        {
            var codeSnippets = new List<CodeSnippet>();

            foreach (var file in codeFiles)
            {
                var reading = string.Format("Reading '{0}'", file);
                string contents;
                using (TimingScope.Start(reading))
                {
                    contents = File.ReadAllText(file);
                    if (!contents.Contains("start code "))
                    {
                        continue;
                    }
                }

                var lines = contents.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                var message = string.Format("Processing '{0}'", file);
                using (TimingScope.Start(message))
                {
                    var innerList = GetCodeSnippetsUsingArray(lines, file);
                    codeSnippets.AddRange(innerList);
                }
            }

            return codeSnippets;
        }

        static IEnumerable<CodeSnippet> GetCodeSnippetsUsingArray(string[] lines, string file)
        {
            var innerList = GetCodeSnippetsFromFile(lines).ToArray();
            foreach (var snippet in innerList)
            {
                snippet.File = file;
            }
            return innerList;
        }

        static IEnumerable<CodeSnippet> GetCodeSnippetsFromFile(IList<string> lines)
        {
            var innerList = new List<CodeSnippet>();

            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];

                var indexOfStartCode = line.IndexOf("start code ");
                if (indexOfStartCode != -1)
                {
                    var startIndex = indexOfStartCode + 11;
                    var suffix = line.RemoveStart(startIndex);
                    var split = suffix.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
                    innerList.Add(new CodeSnippet
                    {
                        Key = split.First(),
                        StartRow = i + 1,
                        Language = split.Skip(1).FirstOrDefault()
                    });
                    continue;
                }

                var indexOfEndCode = line.IndexOf("end code ");
                if (indexOfEndCode != -1)
                {
                    var startIndex = indexOfEndCode + 9;
                    var suffix = line.RemoveStart(startIndex);
                    var split = suffix.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
                    var key = split.First();
                    var existing = innerList.FirstOrDefault(c => c.Key == key);
                    if (existing == null)
                    {
                        // TODO: message about failure
                    }
                    else
                    {
                        existing.EndRow = i;
                        var count = existing.EndRow - existing.StartRow;
                        existing.Value = string.Join(LineEnding, lines.Skip(existing.StartRow)
                                                                  .Take(count)
                                                                  .Where(IsNotCodeSnippetTag));
                    }
                }
            }
            return innerList;
        }

        static bool IsNotCodeSnippetTag(string line)
        {
            return !line.Contains("end code ") && !line.Contains("start code ");
        }
    }
}