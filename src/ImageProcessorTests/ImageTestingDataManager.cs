using CompetitiveBackend.Core.Objects;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

namespace ImageProcessorTests
{
    public class ImageTestingDataManager : IEnumerable<object[]>
    {
        private static string ResultsDir = string.Empty;
        private static string TestsDir = string.Empty;
        private static bool Initialized = false;
        public static uint MinSize = 0;
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
            XDocument xmlDoc = XDocument.Load(xmlFilePath);
            XElement? dimensionsElement = xmlDoc.Element("Settings")?.Element("Dimensions");
            XElement? directoriesElement = xmlDoc.Element("Settings")?.Element("Paths");

            ResultsDir = directoriesElement?.Element(nameof(ResultsDir))?.Value ?? string.Empty;
            TestsDir = directoriesElement?.Element(nameof(TestsDir))?.Value ?? string.Empty;
            uint.TryParse(dimensionsElement?.Element(nameof(MinSize))?.Value, out MinSize);
            uint.TryParse(dimensionsElement?.Element(nameof(MaxSize))?.Value, out MaxSize);
            //resultsDir = "./results/";
            //testsDir = "../../../Tests";

            ResultsDir = TotalPath(ResultsDir);
            TestsDir = TotalPath(TestsDir);


            Assert.False(ResultsDir == string.Empty, "Please set up ResultsDir in testsettings.xml");
            Assert.False(TestsDir == string.Empty, "Please set up TestsDir in testsettings.xml");

            if (!Directory.Exists(ResultsDir))
            {
                Directory.CreateDirectory(ResultsDir);
            }
            if (!Directory.Exists(TestsDir))
            {
                Assert.Fail($"Tests dir is not set up correctly: {TestsDir} does not exist");
            }
            Initialized = true;
        }
        public IEnumerator<object[]> GetEnumerator()
        {
            if (!Initialized) InitFuncTestData();
            string[] files = Directory.GetFiles(TestsDir);
            for (int i = 0; i < files.Length; i++)
            {
                bool positive = true;
                if (Path.GetFileNameWithoutExtension(files[i]).StartsWith("neg", StringComparison.InvariantCultureIgnoreCase))
                {
                    positive = false;
                }
                string resultStorage = Path.Combine(ResultsDir, Path.GetFileNameWithoutExtension(files[i]) + ".jpg");
                yield return new object[] { new FuncTestStructure(files[i], positive, resultStorage) };
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}