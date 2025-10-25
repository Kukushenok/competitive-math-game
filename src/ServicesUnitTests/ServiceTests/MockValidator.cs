using ServicesRealisation.ServicesRealisation.Validator;

namespace ServicesUnitTests.ServiceTests
{
    internal sealed class MockValidator<T> : IValidator<T>
        where T : class
    {
        private bool wasCalled;
        private readonly T? compareObject;
        private bool fail;
        private readonly List<Func<T, bool>> constraints;
        public MockValidator(T? newObject = null, bool fail = false, List<Func<T, bool>>? constraints = null)
        {
            compareObject = newObject;
            wasCalled = false;
            this.fail = fail;
            this.constraints = constraints ?? [];
        }

        public bool IsValid(T value, out string? msg)
        {
            msg = string.Empty;
            wasCalled = true;
            if (compareObject != null)
            {
                fail = !compareObject!.Equals(value);
            }

            foreach (Func<T, bool> constraint in constraints)
            {
                if (!constraint(value))
                {
                    fail = true;
                    break;
                }
            }

            return !fail;
        }

        public void CheckWasCalled()
        {
            Assert.True(wasCalled, "Validator should be called, not ignored!");
        }
    }

    internal sealed class MockValidatorBuilder<T>
        where T : class
    {
        private bool shouldFail;
        private T? compareObj;
        private readonly List<Func<T, bool>> constraints = [];
        public MockValidatorBuilder<T> CheckEtalon(T etalon)
        {
            compareObj = etalon;
            return this;
        }

        public MockValidatorBuilder<T> FailByDefault()
        {
            shouldFail = true;
            return this;
        }

        public MockValidatorBuilder<T> WithConstraint(Func<T, bool> func)
        {
            constraints.Add(func);
            return this;
        }

        public MockValidator<T> Build()
        {
            return new MockValidator<T>(compareObj, shouldFail, constraints);
        }
    }
}
