using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public class BaseValidatorGeneric<T> :AbstractValidator<T> where T : class 
    {
        public BaseValidatorGeneric()
        {

        }
    }
}
