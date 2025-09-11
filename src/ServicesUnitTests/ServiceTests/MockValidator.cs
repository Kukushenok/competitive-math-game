using ServicesRealisation.ServicesRealisation.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesUnitTests.ServiceTests
{
    internal class MockValidator<T> : IValidator<T> where T: class
    {
        private bool WasCalled;
        private T? CompareObject;
        private bool _fail;
        private List<Func<T, bool>> constraints;
        public MockValidator(T? NewObject = null, bool fail = false, List<Func<T, bool>> constraints = null)
        {
            CompareObject = NewObject;
            WasCalled = false;
            _fail = fail;
            this.constraints = constraints ?? new List<Func<T, bool>>();
        }
        public bool IsValid(T value, out string? msg)
        {
            msg = "";
            WasCalled = true;
            if (CompareObject != null)
            {
                _fail = !(CompareObject!.Equals(value));
            }
            foreach (var constraint in constraints)
            {
                if(!constraint(value))
                {
                    _fail = true;
                    break;
                }
            }
            return !_fail;
        }
        public void CheckWasCalled() => Assert.True(WasCalled, "Validator should be called, not ignored!");
    }
    internal class MockValidatorBuilder<T> where T: class
    {
        private bool shouldFail = false;
        private T? compareObj = null;
        private List<Func<T, bool>> constraints = new List<Func<T, bool>>();
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
