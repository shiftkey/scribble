using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Scribble.CodeSnippets
{
    public class Importer
    {
        const string StartRegex = @".*?start\s*code\s*(?<key>[A-Za-z-]*)(?<language>[A-Za-z]*).*?";
        const string EndRegex = @".*?end\s*code\s*(?<key>[A-Za-z-]*)";
        const RegexOptions Options = RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Singleline;

        const string LineEnding = "\r\n";

        public static string Process(Stream code, Stream input)
        {
            var codeContents = new StreamReader(code)
                                        .ReadToEnd()
                                        .Split(new[] { LineEnding }, StringSplitOptions.None);
            var baselineText = new StreamReader(input).ReadToEnd();

            var snippets = GetCodeSnippetsFromFile(codeContents);
            
            foreach (var snippet in snippets)
            {
                baselineText = ProcessMatch(snippet.Key, snippet.Value, baselineText);
            }

            return baselineText;
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

        static string ProcessMatch(string key, string value, string baseLineText)
        {
            var lookup = string.Format("<!-- import {0} -->", key);

            var codeSnippet = FormatTextAsCodeSnippet(value, lookup);

            var startIndex = 0;
            var indexOf = baseLineText.IndexOf(lookup, startIndex);

            while (indexOf > -1)
            {
                var endOfLine = baseLineText.IndexOf(LineEnding, indexOf + lookup.Length);
                if (endOfLine > -1)
                {
                    const string blankLine = LineEnding + LineEnding;
                    var endOfNextLine = baseLineText.IndexOf(blankLine, endOfLine);
                    if (endOfNextLine > -1)
                    {
                        var start = endOfLine + 2;
                        var end = (endOfNextLine + 4) - start;

                        baseLineText = baseLineText.Remove(start, end);
                    }
                    else
                    {
                        if (endOfLine != baseLineText.Length)
                        {
                            endOfNextLine = baseLineText.Length;
                            baseLineText = baseLineText.Remove(endOfLine, endOfNextLine - endOfLine);
                        }
                    }
                }

                startIndex = indexOf + lookup.Length;

                baseLineText = baseLineText.Remove(indexOf, lookup.Length)
                                           .Insert(indexOf, codeSnippet);

                indexOf = baseLineText.IndexOf(lookup, startIndex);
            }
            return baseLineText;
        }

        static string FormatTextAsCodeSnippet(string value, string lookup)
        {
            var valueWithoutEndings = value.TrimEnd('\r', '\n');

            var linesInFile = valueWithoutEndings
                                      .Split(new[] { LineEnding }, StringSplitOptions.None)
                                      .Select(l => l.Replace("\t", "    "));

            var whiteSpaceStartValues = linesInFile.Select(SpacesAtStartOfString)
                                                   .Where(count => count > 0);

            var minWhiteSpace = whiteSpaceStartValues.Any()
                                    ? whiteSpaceStartValues.Min()
                                    : 0;

            var processedLines = string.Join(LineEnding,
                linesInFile.Select(l => TrimWhiteSpace(l, minWhiteSpace, 4)));

            return string.Format("{0}{2}{1}{2}", lookup, processedLines, LineEnding);
        }

        static string TrimWhiteSpace(string input, int removeCount, int insertCount)
        {
            var temp = string.Copy(input);
            if (removeCount > 0 && temp.Length >= removeCount)
            {
                temp = temp.Substring(removeCount);
            }

            var sb = new StringBuilder();
            for (var i = 0; i < insertCount; i++)
            {
                sb.Append(' ');
            }

            sb.Append(temp);

            return sb.ToString();
        }

        // as soon as we find a non-space character, bail out
        static int SpacesAtStartOfString(string s)
        {
            if (s.Length == 0)
                return -1;

            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] != ' ')
                    return i;
            }
            return s.Length;
        }

        public static string ApplySnippets(IList<CodeSnippet> snippets, string inputFile)
        {
            var baselineText = File.ReadAllText(inputFile);

            foreach (var snippet in snippets)
            {
                baselineText = ProcessMatch(snippet.Key, snippet.Value, baselineText);
            }

            return baselineText;
        }

        public static UpdateResult Update(string codeFolder, string[] extensionsToSearch, string docsFolder)
        {
            var result = new UpdateResult();
            var codeFiles = extensionsToSearch.SelectMany(
                extension => Directory.GetFiles(codeFolder, extension, SearchOption.AllDirectories));

            var snippets = GetCodeSnippets(codeFiles);

            var incompleteSnippets = snippets.Where(s => string.IsNullOrWhiteSpace(s.Value)).ToArray();
            if (incompleteSnippets.Any())
            {
                result.Messages.AddRange(
                    incompleteSnippets.Select(i =>
                        string.Format("Code snippet reference '{0}' was not closed (specify 'end code {0}'). Ignoring...", i.Key)));

                return result;
            }

            result.Snippets = snippets.Count;

            var inputFiles = new[] { "*.md","*.mdown","*.markdown" }.SelectMany(
              extension => Directory.GetFiles(docsFolder, extension, SearchOption.AllDirectories));

            foreach (var inputFile in inputFiles)
            {
                var newText = ApplySnippets(snippets, inputFile);
                File.WriteAllText(inputFile, newText);
            }

            return result;
        }
    }

    public class UpdateResult
    {
        public UpdateResult()
        {
            Messages = new List<string>();
        }

        public int Snippets { get; set; }

        public List<string> Messages { get; set; }
    }
}