using System.Collections;
using CompetitiveBackend.Core.Objects;
using Microsoft.Extensions.Configuration;
using ServicesRealisation.ServicesRealisation.Validator;

namespace ServicesUnitTests.ValidatorTests
{
    public class CompetitionValidatorTests
    {
        private sealed class Values : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Competition("Abob", new string('*', 10), new DateTime(2025, 1, 1, 1, 1, 1), new DateTime(2025, 1, 1, 1, 10, 1)),
                    true,
                };
                yield return new object[]
                {
                    new Competition("Abo", new string('*', 10), new DateTime(2025, 1, 1, 1, 1, 1), new DateTime(2025, 1, 1, 1, 10, 1)),
                    false,
                };
                yield return new object[]
                {
                    new Competition("Abondus", new string('*', 10), new DateTime(2025, 1, 1, 1, 1, 1), new DateTime(2024, 1, 1, 1, 10, 1)),
                    false,
                };
                yield return new object[]
                {
                    new Competition("Colon", new string('*', 11), new DateTime(2025, 1, 1, 1, 1, 1), new DateTime(2025, 1, 1, 1, 10, 1)),
                    true,
                };
                yield return new object[]
                {
                    new Competition("Colon", new string('*', 12), new DateTime(2025, 1, 1, 1, 1, 1), new DateTime(2025, 1, 1, 1, 10, 1)),
                    false,
                };
                yield return new object[]
                {
                    new Competition("Colon", new string('*', 9), new DateTime(2025, 1, 1, 1, 1, 1), new DateTime(2025, 1, 1, 1, 10, 1)),
                    false,
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private readonly CompetitionValidator dt;
        public CompetitionValidatorTests()
        {
            var dict = new Dictionary<string, string?>
            {
               { "Constraints:Competition:DescriptionLength:min", "10" },
               { "Constraints:Competition:DescriptionLength:max", "11" },
            };
            IConfigurationRoot conf = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            dt = new CompetitionValidator(conf);
        }

        [Theory]

        // Arrange
        [ClassData(typeof(Values))]
        public void CheckOK(Competition comp, bool expected)
        {
            // Act
            bool result = dt.IsValid(comp, out _);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
