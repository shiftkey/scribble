using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Scribble.CodeSnippets;
using Xunit;
using Xunit.Extensions;

namespace Scribble.CodeSnippet.Tests
{
    public class ImportTestSuite
    {
        [Theory, PropertyData("Scenarios")]
        public void Scenario(string name, Stream code, Stream input, Stream expected)
        {
            var expectedContents = new StreamReader(expected).ReadToEnd();

            var actual = Importer.Process(code, input);

            Assert.Equal(expectedContents, actual);
        }

        public static IEnumerable<object[]> Scenarios
        {
            get
            {
                var scenarios = new List<object[]>();
                var assembly = Assembly.GetExecutingAssembly();

                // because why even return a sorted list here?
                var resources = assembly.GetManifestResourceNames()
                                        .Where(c => c.Contains(".scenarios."))
                                        .OrderBy(c => c);

                for (var i = 0; i < resources.Count(); i += 3)
                {
                    var firstResource = resources.ElementAt(i);

                    // magic number representing assembly + folder length to ignore
                    var name = firstResource.Substring(38, firstResource.IndexOf(".", 38) - 38);

                    var codeStream = assembly.GetManifestResourceStream(firstResource);
                    var inputStream = assembly.GetManifestResourceStream(resources.ElementAt(i + 1));
                    var expectedStream = assembly.GetManifestResourceStream(resources.ElementAt(i + 2));

                    scenarios.Add(new object[] { name, codeStream, inputStream, expectedStream });
                }

                return scenarios;
            }
        }
    }
}