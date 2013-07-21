using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Scribble.CodeSnippets.Models;

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
            var baselineText = File.ReadAllText(inputFile);

            var result = ApplyToText(snippets, baselineText);

            foreach (var missingKey in result.RequiredSnippets)
            {
                missingKey.File = inputFile;
            }
            return result;
        }

        public static FileProcessResult ApplyToText(ICollection<CodeSnippet> snippets, string baselineText)
        {
            var result = new FileProcessResult();

            var missingKeys = CheckMissingKeys(snippets, baselineText);
            if (missingKeys.Any())
            {
                result.RequiredSnippets = missingKeys;
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

        public static CodeSnippetReference[] CheckMissingKeys(IEnumerable<CodeSnippet> snippets, string baselineText)
        {
            var foundKeys = snippets.Select(m => m.Key);
            var expectedKeys = FindExpectedKeys(baselineText);
            return expectedKeys.Where(k => !foundKeys.Contains(k.Key)).ToArray();
        }

        static IEnumerable<CodeSnippetReference> FindExpectedKeys(string baselineText)
        {
            var stringReader = new StringReader(baselineText);

            string line;
            var lineNumber = 0;
            while ((line = stringReader.ReadLine()) != null)
            {
                lineNumber++;
                var indexOfImportStart = line.IndexOf("<!-- import ");

                if (indexOfImportStart > -1)
                {
                    var indexOfImportEnd = line.IndexOf(" -->");
                    if (indexOfImportEnd > -1)
                    {
                        var startIndex = indexOfImportStart + 12;
                        var key = line.Substring(startIndex, indexOfImportEnd - startIndex);
                        yield return new CodeSnippetReference
                        {
                            LineNumber = lineNumber,
                            Key = key
                        };
                    }
                }
            }
        }

        static string ProcessMatch(string key, string value, string baseLineText)
        {
            var lookup = string.Format("<!-- import {0} -->", key);

            var codeSnippet = FormatTextAsCodeSnippet(value);

            var builder = new StringBuilder();
            using (var reader = new StringReader(baseLineText))
            {
                string line;
                var eatingCode = false;
                while ((line = reader.ReadLine()) != null)
                {
                    if (eatingCode)
                    {
                        if (line.StartsWith("    ") || line.StartsWith("\t"))
                        {
                            continue;   
                        }
                        eatingCode = false;
                    }
                    builder.AppendLine(line);
                    if (line.Contains(lookup))
                    {
                        builder.AppendLine(codeSnippet);
                        eatingCode = true;
                    }
                }
            }

            return builder.ToString().TrimTrailingNewLine();
        }

        static string FormatTextAsCodeSnippet(string value)
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

            return string.Join(LineEnding,
                linesInFile.Select(l => TrimWhiteSpace(l, minWhiteSpace, 4)));
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


    }
}