using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Scribble.CodeSnippets
{
    public class DocumentFileProcessor
    {
        const string LineEnding = "\r\n";

        readonly string docsFolder;

        public DocumentFileProcessor(string docsFolder)
        {
            this.docsFolder = docsFolder;
        }

        public DocumentProcessResult Apply(ICollection<CodeSnippet> snippets)
        {
            var result = new DocumentProcessResult();

            var inputFiles = new[] { "*.md", "*.mdown", "*.markdown" }.SelectMany(
              extension => Directory.GetFiles(docsFolder, extension, SearchOption.AllDirectories))
              .ToArray();

            result.Count = inputFiles.Count();

            foreach (var inputFile in inputFiles)
            {
                var fileResult = Apply(snippets, inputFile);

                if (fileResult.RequiredSnippets.Any())
                {
                    // give up if we can't continue
                    result.Include(fileResult.RequiredSnippets);
                    return result;
                }

                result.Include(fileResult.Snippets);

                File.WriteAllText(inputFile, fileResult.Text);
            }

            return result;
        }

        public static FileProcessResult Apply(ICollection<CodeSnippet> snippets, string inputFile)
        {
            var result = new FileProcessResult();

            var baselineText = File.ReadAllText(inputFile);

            var matches = Regex.Matches(baselineText, @"<!--[\s]*import[\s]*(?<key>[\w-]*)[\s]*-->");

            // ensure the required keys match what we have been provided
            var expectedKeys = matches.Cast<Match>().Select(m => m.Groups["key"].Value);
            var foundKeys = snippets.Select(m => m.Key);

            var missingKeys = expectedKeys.Except(foundKeys).ToArray();
            if (missingKeys.Any())
            {
                result.RequiredSnippets = missingKeys.Select(
                    key => new CodeSnippetReference() { File = inputFile, Key = key});
                result.Text = baselineText;
                return result;
            }

            foreach (var snippet in snippets)
            {
                // TODO: this won't change the text 
                // if a snippet is unchanged
                // so we need more context
                var output = ProcessMatch(snippet.Key, snippet.Value, baselineText);

                if (!string.Equals(output, baselineText))
                {
                    // we may have added in a snippet
                    result.Snippets.Add(snippet);
                }

                baselineText = output;
            }

            result.Text = baselineText;

            return result;
        }

        static string ProcessMatch(string key, string value, string baseLineText)
        {
            var lookup = string.Format("<!-- import {0} -->", key);

            var codeSnippet = FormatTextAsCodeSnippet(value, lookup);

            var startIndex = 0;
            var indexOf = IndexOfOrdinal(baseLineText, lookup, startIndex);

            while (indexOf > -1)
            {
                var endOfLine = IndexOfOrdinal(baseLineText, LineEnding, indexOf + lookup.Length);
                if (endOfLine > -1)
                {
                    const string blankLine = LineEnding + LineEnding;
                    var endOfNextLine = IndexOfOrdinal(baseLineText, blankLine, endOfLine);
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

                indexOf = IndexOfOrdinal(baseLineText, lookup, startIndex);
            }
            return baseLineText;
        }

        static string FormatTextAsCodeSnippet(string value, string lookup)
        {
            var valueWithoutEndings = value.TrimEnd('\r', '\n');

            var linesInFile = valueWithoutEndings
                                      .Split(new[] { LineEnding }, StringSplitOptions.None)
                                      .Select(l => l.Replace("\t", "    "))
                                      .ToArray();

            var whiteSpaceStartValues = linesInFile.Select(SpacesAtStartOfString)
                                                   .Where(count => count > 0)
                                                   .ToArray();

            var minWhiteSpace = whiteSpaceStartValues.Any()
                                    ? whiteSpaceStartValues.Min()
                                    : 0;

            var processedLines = string.Join(LineEnding,
                linesInFile.Select(l => TrimWhiteSpace(l, minWhiteSpace, 4)));

            return string.Format("{0}{2}{1}{2}", lookup, processedLines, LineEnding);
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

        static int IndexOfOrdinal(string text, string value, int startIndex)
        {
            return text.IndexOf(value, startIndex, StringComparison.Ordinal);
        }
    }
}