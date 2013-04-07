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
                var messages = ErrorFormatter.Format(incompleteSnippets);
                result.Messages.AddRange(messages);
                return result;
            }

            result.Snippets = snippets.Count;

            var processor = new DocumentFileProcessor(docsFolder);
            var processResult = processor.Apply(snippets);

            result.Files = processResult.Count;
            result.Completed = true;

            return result;
        }
    }
}