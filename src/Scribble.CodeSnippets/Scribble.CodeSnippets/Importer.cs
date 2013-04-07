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
                result.Messages.AddRange(
                    incompleteSnippets.Select(i =>
                        string.Format("Code snippet reference '{0}' was not closed (specify 'end code {0}').", i.Key)));

                return result;
            }

            result.Snippets = snippets.Count;

            var processor = new DocumentFileProcessor(docsFolder);
            processor.Apply(snippets);

            result.Completed = true;

            return result;
        }
    }
}