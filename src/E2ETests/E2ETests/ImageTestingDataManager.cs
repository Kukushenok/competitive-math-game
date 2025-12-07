using CompetitiveBackend.Core.Objects;
using E2ETests;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

namespace E2ETests
{
    public class ImageTestingDataManager : IEnumerable<object[]>
    {
        private static string ResultsDir = "";
        private static string TestsDir = "../../../../ImageProcessorTests/Tests/";
        private static bool Initialized = false;
        public static uint MinSize = 0;
        public static uint MaxSize = 256;
        public IEnumerator<object[]> GetEnumerator()
        {
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