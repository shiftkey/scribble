using System.Diagnostics;
using System.Linq;
using Scribble.CodeSnippets.Models;

namespace Scribble.CodeSnippets
{
    public class Importer
    {
        public static UpdateResult Update(string codeFolder, string[] extensionsToSearch, string docsFolder)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = new UpdateResult();

            var codeParser = new CodeFileParser(codeFolder);
            var snippets = codeParser.Parse(extensionsToSearch);

            var incompleteSnippets = snippets.Where(s => string.IsNullOrWhiteSpace(s.Value)).ToArray();
            if (incompleteSnippets.Any())
            {
                var messages = ErrorFormatter.FormatIncomplete(incompleteSnippets);
                result.Errors.AddRange(messages);
                return result;
            }

            result.Snippets = snippets.Count;

            var processor = new DocumentFileProcessor(docsFolder);
            var processResult = processor.Apply(snippets);

            var snippetsNotUsed = snippets.Except(processResult.SnippetsUsed).ToArray();

            var snippetsMissed = processResult.SnippetReferences;

            if (snippetsMissed.Any())
            {
                var messages = ErrorFormatter.FormatNotFound(snippetsMissed);
                result.Errors.AddRange(messages);
            }

            if (snippetsNotUsed.Any())
            {
                var messages = ErrorFormatter.FormatUnused(snippetsNotUsed);
                result.Warnings.AddRange(messages);
            }

            result.Files = processResult.Count;
            result.Completed = !result.Errors.Any();

            stopwatch.Stop();
            result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            return result;
        }
    }
}