using BenchmarkMeasurerHost.DataGenerator;
using System.Reflection;
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
        private IEnumerable<EnvironmentSettings> GetData()
        {
            yield return new EnvironmentSettings(100, 250, 5);
            yield return new EnvironmentSettings(250, 500, 5);
            yield return new EnvironmentSettings(500, 1000, 5);
            yield return new EnvironmentSettings(1000, 2500, 5);
            yield return new EnvironmentSettings(2500, 5000, 5);
        }
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            for(int i = 0; i < RepeatCount; i++)
            {
                foreach(EnvironmentSettings set in GetData())
                {
                    yield return [set];
                }
            }
        }
    }
}