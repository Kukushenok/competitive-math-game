using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesRealisation.ServicesRealisation.Validator
{
    public class DummyValidator<T> : IValidator<T>
    {

        public bool IsValid(T value, out string? msg)
        {
            msg = null;
            return true;
        }
    }
}
