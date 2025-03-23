using CompetitiveBackend.Core.Objects;
using Microsoft.Extensions.Configuration;
using ServicesRealisation.ServicesRealisation.Validator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesUnitTests.ValidatorTests
{
    public class CompetitionValidatorTests
    {
        class Values : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {
                    new Competition("Abob", new string('*', 10), new DateTime(2025, 1, 1, 1, 1, 1), new DateTime(2025, 1, 1, 1, 10, 1)),
                    true };
                yield return new object[] {
                    new Competition("Abo",new string('*', 10), new DateTime(2025, 1, 1, 1, 1, 1), new DateTime(2025, 1, 1, 1, 10, 1)),
                    false };
                yield return new object[] {
                    new Competition("Abondus", new string('*', 10), new DateTime(2025, 1, 1, 1, 1, 1), new DateTime(2024, 1, 1, 1, 10, 1)),
                    false };
                yield return new object[] {
                    new Competition("Colon", new string('*', 11), new DateTime(2025, 1, 1, 1, 1, 1), new DateTime(2025, 1, 1, 1, 10, 1)),
                    true };
                yield return new object[] {
                    new Competition("Colon", new string('*', 12), new DateTime(2025, 1, 1, 1, 1, 1), new DateTime(2025, 1, 1, 1, 10, 1)),
                    false };
                yield return new object[] {
                    new Competition("Colon", new string('*', 9), new DateTime(2025, 1, 1, 1, 1, 1), new DateTime(2025, 1, 1, 1, 10, 1)),
                    false };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        CompetitionValidator dt;
        public CompetitionValidatorTests()
        {
            var Dict = new Dictionary<string, string?>
            {
               {"Constraints:Competition:descriptionLength:min", "10"},
               {"Constraints:Competition:descriptionLength:max", "11"},
            };
            var conf = new ConfigurationBuilder().AddInMemoryCollection(Dict).Build();
            dt = new CompetitionValidator(conf);
        }
        [Theory]
        [ClassData(typeof(Values))]
        public void CheckOK(Competition comp, bool expected)
        {
            Assert.Equal(expected, dt.IsValid(comp, out _));
        }
    }
}
