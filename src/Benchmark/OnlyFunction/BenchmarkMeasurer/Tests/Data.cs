using System.Collections;

namespace BenchmarkMeasurer.Tests
{
    public class Data : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            for (int i = 0; i < 1; i++)
            {
                foreach (string name in Directory.GetFiles(Path.Combine(ContainerInitializer.COREPATH, "Results", "Dumps")))
                {
                    yield return [name];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
