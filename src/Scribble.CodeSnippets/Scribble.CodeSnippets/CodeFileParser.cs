using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Scribble.CodeSnippets
{
    public class CodeFileParser
    {
        const string StartRegex = @".*?start\s*code\s*(?<key>[A-Za-z-]*)(?<language>[A-Za-z]*).*?";
        const string EndRegex = @".*?end\s*code\s*(?<key>[A-Za-z-]*)";
        const RegexOptions Options = RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Singleline;

        const string LineEnding = "\r\n";
        
        readonly string codeFolder;

        public CodeFileParser(string codeFolder)
        {
            this.codeFolder = codeFolder;
        }

        public ICollection<CodeSnippet> Parse(string[] extensionsToSearch)
        {
            var codeFiles = extensionsToSearch.SelectMany(
                extension => Directory.GetFiles(codeFolder, extension, SearchOption.AllDirectories));

            var snippets = GetCodeSnippets(codeFiles);

            return snippets;
        }

        public ICollection<CodeSnippet> Parse(Func<string, bool> isValidFile)
        {
            var codeFiles = Directory.GetFiles(codeFolder, "*.*", SearchOption.AllDirectories)
                                     .Where(isValidFile);

            var snippets = GetCodeSnippets(codeFiles);

            return snippets;
        }

        public static IList<CodeSnippet> GetCodeSnippets(IEnumerable<string> codeFiles)
        {
            var codeSnippets = new List<CodeSnippet>();

            foreach (var file in codeFiles)
            {
                var lines = File.ReadAllLines(file);

                var innerList = GetCodeSnippetsFromFile(lines);

                codeSnippets.AddRange(innerList);
            }

            return codeSnippets;
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