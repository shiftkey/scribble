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
        const string StartRegex = @".*?start\s*code\s*(?<key>[A-Za-z-]*)(?<language>[A-Za-z]*).*?";
        const string EndRegex = @".*?end\s*code\s*(?<key>[A-Za-z-]*)";
        const RegexOptions Options = RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Singleline;

        const string LineEnding = "\r\n";

        readonly string codeFolder;

        public CodeFileParser(string codeFolder)
        {
            this.codeFolder = codeFolder;
        }

        public ICollection<CodeSnippet> Parse(string[] filterOnExpression)
        {
            try
            {
                var allFiles = Directory.GetFiles(codeFolder, "*.*", SearchOption.AllDirectories);

                var filesMatchingRegex = new List<string>();

                foreach (var regex in filterOnExpression)
                {
                    foreach (var f in allFiles)
                    {
                        if (Regex.IsMatch(f, regex))
                        {
                            filesMatchingRegex.Add(f);
                        }
                    }
                }

                return GetCodeSnippets(filesMatchingRegex.Distinct());
            }
            catch (ArgumentException ex)
            {
                // right so someone is passing in a regex
                var filesMatchingExtensions = filterOnExpression.SelectMany(
                    extension => Directory.GetFiles(codeFolder, extension, SearchOption.AllDirectories)).ToList();
                return GetCodeSnippets(filesMatchingExtensions);
            }
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
                    if (Regex.Matches(contents, @".*?\s*start\s*code\s*", RegexOptions.Compiled).Count == 0) continue;
                }

                var lines = contents.Split(new[] { "\r\n", "\n " }, StringSplitOptions.None);
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

                var isStartTag = Regex.Match(line, StartRegex, Options);
                if (isStartTag.Success)
                {
                    innerList.Add(new CodeSnippet
                    {
                        Key = isStartTag.Groups["key"].Value,
                        StartRow = i + 1,
                        Language = isStartTag.Groups["language"].Value
                    });
                    continue;
                }

                var isEndTag = Regex.Match(line, EndRegex);
                if (isEndTag.Success)
                {
                    var key = isEndTag.Groups["key"].Value;

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

        static bool IsNotCodeSnippetTag(string s)
        {
            if (Regex.IsMatch(s, StartRegex, Options)) return false;
            if (Regex.IsMatch(s, EndRegex, Options)) return false;
            return true;
        }
    }
}