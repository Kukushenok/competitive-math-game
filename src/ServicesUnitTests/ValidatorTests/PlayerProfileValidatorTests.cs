using CompetitiveBackend.Core.Objects;
using Microsoft.Extensions.Configuration;
using ServicesRealisation.Objects;
using ServicesRealisation.ServicesRealisation.Validator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesUnitTests.ValidatorTests
{
    public class PlayerProfileValidatorTests
    {
        PlayerAccountValidator dt;
        public PlayerProfileValidatorTests()
        {
            var Dict = new Dictionary<string, string?>
            {
               {"Constraints:Player:descriptionLength:min", "0"},
               {"Constraints:Player:descriptionLength:max", "16"},
               {"Constraints:Player:NameLength:min", "4"},
               {"Constraints:Player:NameLength:max", "16"},
               {"Constraints:Player:Password:MinLength", "8"},
               {"Constraints:Player:Password:RequiresLetters", "true"},
               {"Constraints:Player:NameRegex:pattern", ".*"},
               {"Constraints:Player:EmailRegex:pattern", ".*"},
            };
            var conf = new ConfigurationBuilder().AddInMemoryCollection(Dict).Build();
            dt = new PlayerAccountValidator(conf);
        }

        #region PlayerProfileTests
        class PlayerProfileValues : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {
                    new PlayerProfile(new string('*', 10), new string('*', 1), 0),
                    true
                };
                yield return new object[] {
                    new PlayerProfile(new string('*', 17), new string('*', 1), 0),
                    false
                };
                yield return new object[] {
                    new PlayerProfile(new string('*', 16), new string('*', 1), 0),
                    true
                };
                yield return new object[] {
                    new PlayerProfile(new string('*', 16), new string('*', 17), 0),
                    false
                };
                yield return new object[] {
                    new PlayerProfile(new string('*', 16), new string('*', 16), 0),
                    true
                };
                yield return new object[] {
                    new PlayerProfile(new string('*', 3), new string('*', 16), 0),
                    false
                };
                yield return new object[] {
                    new PlayerProfile(new string('*', 4), new string('*', 16), 0),
                    true
                };
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        [Theory]
        [ClassData(typeof(PlayerProfileValues))]
        public void CheckPlayerProfile(PlayerProfile comp, bool expected)
        {
            Assert.Equal(expected, dt.IsValid(comp, out _));
        }
        #endregion
        #region AccountTests
        class AccountValues : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Account(null!, null!, null), false
                };
                yield return new object[]
                {
                    new Account("", ""), false
                };
                yield return new object[]
                {
                    new Account("amo", "modus ponens"), false
                };
                yield return new object[]
                {
                    new Account("amog", null!, null), true
                };
                yield return new object[]
                {
                    new Account("amogus", ""), true
                };
                yield return new object[]
                {
                    new Account(new string('*', 17), ""), false
                };
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        [Theory]
        [ClassData(typeof(AccountValues))]
        public void CheckAccount(Account comp, bool expected)
        {
            Assert.Equal(expected, dt.IsValid(comp, out _));
        }
        #endregion
        #region AccountCreationTests
        class AccountCreationValues : IEnumerable<object[]>
        {
            public IEnumerable<(string,bool)> Passwords()
            {
                yield return ("1234", false);
                yield return ("", false);
                yield return ("12345678", false);
                yield return ("123a5678", true);
                yield return ("12345678a", true);
                yield return ("123456a", false);
            }
            public IEnumerator<object[]> GetEnumerator()
            {
                foreach (object[] data in new AccountValues())
                {
                    Account acc = (Account)data[0];
                    bool validness = (bool)data[1];
                    foreach((string password, bool valid) a in Passwords())
                    {
                        yield return new object[]
                        {
                            new AccountCreationData(acc, a.password), a.valid && validness
                        };
                    }
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        [Theory]
        [ClassData(typeof(AccountCreationValues))]
        public void CheckAccountCreation(AccountCreationData comp, bool expected)
        {
            Assert.Equal(expected, dt.IsValid(comp, out _));
        }
        #endregion
    }
}
