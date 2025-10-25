using System.Reflection;
using BenchmarkMeasurerHost.DataGenerator;
using Xunit.Sdk;

namespace BenchmarkMeasurer
{
    public class MeasurementData : DataAttribute
    {
        public int RepeatCount = 1;
        public static int IterIncrement = 1;
        public MeasurementData(int repeatCount)
        {
            RepeatCount = repeatCount;
        }

        private static IEnumerable<EnvironmentSettings> GetData()
        {
            yield return new EnvironmentSettings(125, 250, 5);
            yield return new EnvironmentSettings(250, 500, 5);
            yield return new EnvironmentSettings(500, 1000, 5);
            yield return new EnvironmentSettings(1000, 2000, 5);
            yield return new EnvironmentSettings(1500, 3000, 5);
            yield return new EnvironmentSettings(2000, 4000, 5);
            yield return new EnvironmentSettings(2500, 5000, 5);

            // yield return new EnvironmentSettings(5000, 7500, 5);
            // yield return new EnvironmentSettings(6000, 10000, 5);
            // yield return new EnvironmentSettings(7000, 12500, 5);
            // yield return new EnvironmentSettings(8000, 15000, 5);
            // yield return new EnvironmentSettings(10000, 20000, 5);
            // yield return new EnvironmentSettings(20000, 40000, 5);
            // yield return new EnvironmentSettings(40000, 80000, 5);
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            for (int i = 0; i < RepeatCount; i++)
            {
                foreach (EnvironmentSettings set in GetData())
                {
                    yield return [set];
                }
            }
        }
    }
}