using System.Linq;

namespace Scribble.CodeSnippets
{
    public class Importer
    {
        public static UpdateResult Update(string codeFolder, string[] extensionsToSearch, string docsFolder)
        {
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

            if (snippetsNotUsed.Any())
            {
                var messages = ErrorFormatter.FormatUnused(snippetsNotUsed);
                result.Warnings.AddRange(messages);
            }

            if (processResult.HasMessages)
            {
                var warnings = ErrorFormatter.Format(processResult.Warnings);
                result.Warnings.AddRange(warnings);

                var errors = ErrorFormatter.Format(processResult.Errors);
                result.Errors.AddRange(errors);    
            }

            result.Files = processResult.Count;
            result.Completed = true;

            return result;
        }
    }
}