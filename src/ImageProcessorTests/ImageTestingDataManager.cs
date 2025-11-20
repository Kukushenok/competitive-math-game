using System.Collections;
using System.Xml.Linq;
using ImageProcessorRealisation;

namespace ImageProcessorTests
{
    public class ImageTestingDataManager : IEnumerable<object[]>
    {
        private static string resultsDir = string.Empty;
        private static string testsDir = string.Empty;
        private static bool initialized;
        private static uint minSize;
        private static uint maxSize = 256;

        public ImageTestingDataManager()
        {
        }

        public static Options SetupConstraints(Options opt)
        {
            return opt.AddConstraints(minSize, maxSize);
        }

        private static string TotalPath(string path)
        {
            return Path.Combine(Environment.CurrentDirectory, "../../../", path);
        }

        private static void ParseDimensionsElements(XElement? dimensionsElement)
        {
            if (!uint.TryParse(dimensionsElement?.Element(nameof(minSize))?.Value, out minSize))
            {
                minSize = 0;
            }

            if (!uint.TryParse(dimensionsElement?.Element(nameof(maxSize))?.Value, out maxSize))
            {
                maxSize = 256;
            }
        }

        private static void ReadElement(XElement? directoriesElement, XElement? dimensionsElement)
        {
            resultsDir = directoriesElement?.Element(nameof(resultsDir))?.Value ?? string.Empty;
            testsDir = directoriesElement?.Element(nameof(testsDir))?.Value ?? string.Empty;
            ParseDimensionsElements(dimensionsElement);

            resultsDir = TotalPath(resultsDir);
            testsDir = TotalPath(testsDir);

            Assert.False(resultsDir == string.Empty, "Please set up ResultsDir in testsettings.xml");
            Assert.False(testsDir == string.Empty, "Please set up TestsDir in testsettings.xml");
        }

        public static void InitFuncTestData()
        {
            string xmlFilePath = TotalPath("testsettings.xml");

            // Load the XML file
            var xmlDoc = XDocument.Load(xmlFilePath);
            XElement? dimensionsElement = xmlDoc.Element("Settings")?.Element("Dimensions");
            XElement? directoriesElement = xmlDoc.Element("Settings")?.Element("Paths");

            ReadElement(directoriesElement, dimensionsElement);

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