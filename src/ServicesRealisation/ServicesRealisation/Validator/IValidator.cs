using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesRealisation.ServicesRealisation.Validator
{
    public interface IValidator<T> 
    {
        public bool IsValid(T value, out string? msg);
    }
}
