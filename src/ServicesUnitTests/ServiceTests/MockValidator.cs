using ServicesRealisation.ServicesRealisation.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesUnitTests.ServiceTests
{
    class MockValidator<T> : IValidator<T>
    {
        private bool WasCalled;
        public T? CompareObject;
        private bool _fail;
        public void Reset(T? NewObject = default, bool fail = false) 
        {
            CompareObject = NewObject;
            WasCalled = false;
            _fail = fail;
        }
        public bool IsValid(T value, out string? msg)
        {
            msg = "";
            WasCalled = true;
            if(CompareObject != null) Assert.Equal(CompareObject!, value);
            return !_fail;
        }
        public void Check() => Assert.True(WasCalled, "Validator should be called, not ignored!");
    }
}
