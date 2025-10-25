using System.Collections;
using System.Xml.Linq;

namespace ImageProcessorTests
{
    public class ImageTestingDataManager : IEnumerable<object[]>
    {
        private static string resultsDir = string.Empty;
        private static string testsDir = string.Empty;
        private static bool initialized;
        public static uint MinSize;
        public static uint MaxSize = 256;

        public ImageTestingDataManager()
        {
        }

        private static string TotalPath(string path)
        {
            return Path.Combine(Environment.CurrentDirectory, "../../../", path);
        }

        public static void InitFuncTestData()
        {
            string xmlFilePath = TotalPath("testsettings.xml");

            // Load the XML file
            var xmlDoc = XDocument.Load(xmlFilePath);
            XElement? dimensionsElement = xmlDoc.Element("Settings")?.Element("Dimensions");
            XElement? directoriesElement = xmlDoc.Element("Settings")?.Element("Paths");

            resultsDir = directoriesElement?.Element(nameof(resultsDir))?.Value ?? string.Empty;
            testsDir = directoriesElement?.Element(nameof(testsDir))?.Value ?? string.Empty;
            uint.TryParse(dimensionsElement?.Element(nameof(MinSize))?.Value, out MinSize);
            uint.TryParse(dimensionsElement?.Element(nameof(MaxSize))?.Value, out MaxSize);

            // resultsDir = "./results/";
            // testsDir = "../../../Tests";
            resultsDir = TotalPath(resultsDir);
            testsDir = TotalPath(testsDir);

            Assert.False(resultsDir == string.Empty, "Please set up ResultsDir in testsettings.xml");
            Assert.False(testsDir == string.Empty, "Please set up TestsDir in testsettings.xml");

            if (!Directory.Exists(resultsDir))
            {
                Directory.CreateDirectory(resultsDir);
            }

            if (!Directory.Exists(testsDir))
            {
                Assert.Fail($"Tests dir is not set up correctly: {testsDir} does not exist");
            }

            initialized = true;
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            if (!initialized)
            {
                InitFuncTestData();
            }

            string[] files = Directory.GetFiles(testsDir);
            for (int i = 0; i < files.Length; i++)
            {
                bool positive = true;
                if (Path.GetFileNameWithoutExtension(files[i]).StartsWith("neg", StringComparison.InvariantCultureIgnoreCase))
                {
                    positive = false;
                }

                string resultStorage = Path.Combine(resultsDir, Path.GetFileNameWithoutExtension(files[i]) + ".jpg");
                yield return new object[] { new FuncTestStructure(files[i], positive, resultStorage) };
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}