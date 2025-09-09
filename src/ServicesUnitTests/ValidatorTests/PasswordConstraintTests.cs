using ServicesRealisation.ServicesRealisation.Validator.Constraints;
using System.Data;

namespace ServicesUnitTests.ValidatorTests
{
    public class PasswordConstraintTests
    {
        [Theory]
        [InlineData("1234", true)]
        [InlineData("12345", true)]
        [InlineData("", false)]
        [InlineData("123", false)]
        [InlineData("12344919293921391293", true)]
        public void LengthPasswordTest(string password, bool isValid)
        {
            var cons = new PasswordConstraint(4, false);
            Assert.Equal(isValid, cons.IsValid(password, out _));
        }
        [Theory]
        [InlineData("1234__", false)]
        [InlineData("12345A", true)]
        [InlineData("", false)]
        [InlineData("123", false)]
        [InlineData("12344919293921391293", false)]
        [InlineData("______", false)]
        [InlineData("___A___", true)]
        public void LettersPasswordTest(string password, bool isValid)
        {
            var cons = new PasswordConstraint(4, true);
            Assert.Equal(isValid, cons.IsValid(password, out _));
        }
    }
}