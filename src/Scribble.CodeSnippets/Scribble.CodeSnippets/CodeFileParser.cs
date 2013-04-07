using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Scribble.CodeSnippets.Models;

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

            return GetCodeSnippets(codeFiles);
        }

        public static IList<CodeSnippet> GetCodeSnippets(IEnumerable<string> codeFiles)
        {
            var codeSnippets = new List<CodeSnippet>();

            foreach (var file in codeFiles)
            {
                var lines = File.ReadAllLines(file);

                var innerList = GetCodeSnippetsUsingArray(lines, file);
                //var innerList2 = GetCodeSnippetsUsingRegex(file, lines);
                codeSnippets.AddRange(innerList);
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

        //static IEnumerable<CodeSnippet> GetCodeSnippetsUsingRegex(string file, string[] lines)
        //{
        //    var overall = new Stopwatch();
        //    overall.Start();

        //    var innerList2 = new List<CodeSnippet>();
        //    var text = File.ReadAllText(file);

        //    var stopwatch = new Stopwatch();
        //    stopwatch.Restart(); 
        //    var inner = new Stopwatch();
        //    foreach (Match tag in Regex.Matches(text, StartRegex, Options))
        //    {
        //        inner.Restart();    
        //        var key = tag.Groups["key"];
        //        var value = key.Value;
        //        var startRow = text.Substring(0, key.Index)
        //                           .Count(c => c == '\n') + 1;
        //        var language = tag.Groups["language"].Value;

        //        innerList2.Add(new CodeSnippet
        //        {
        //            File = file,
        //            Key = value,
        //            StartRow = startRow,
        //            Language = language
        //        });
        //        inner.Stop();
        //        Console.WriteLine("StartRegex inner loop {0} took {1}ms", value, inner.ElapsedMilliseconds);
        //    }

        //    stopwatch.Stop();
        //    Console.WriteLine("StartRegex loop took {0}ms", stopwatch.ElapsedMilliseconds);

        //    stopwatch.Restart();
        //    foreach (Match tag in Regex.Matches(text, EndRegex, Options))
        //    {

        //        var key = tag.Groups["key"];
        //        var value = key.Value;
        //        var rowNumber = text.Substring(0, key.Index)
        //                            .Count(c => c == '\n') + 1;

        //        var existing = innerList2.FirstOrDefault(c => c.Key == value);
        //        if (existing == null)
        //        {
        //            // TODO: message about failure
        //        }
        //        else
        //        {
        //            existing.EndRow = rowNumber;
        //            var count = existing.EndRow - existing.StartRow;
        //            existing.Value = string.Join(LineEnding, lines.Skip(existing.StartRow)
        //                                                          .Take(count)
        //                                                          .Where(IsNotCodeSnippetTag));
        //        }
        //    }

        //    stopwatch.Stop();
        //    Console.WriteLine("EndRegex loop took {0}ms", stopwatch.ElapsedMilliseconds);

        //    overall.Stop();
        //    Console.WriteLine("Overall took {0}ms", overall.ElapsedMilliseconds);

        //    return innerList2;
        //}

        [Obsolete]
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