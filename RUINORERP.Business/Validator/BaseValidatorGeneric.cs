using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public abstract class BaseValidatorGeneric<T> : AbstractValidator<T> where T : class
    {
        protected BaseValidatorGeneric()
        {

        }
        //public abstract void Initialize();

        public virtual void Initialize()
        { 
        }


        ///// <summary>
        ///// 为了实现个性化验证，可以在子类中重写此方法
        ///// </summary>
        //public virtual void Initialize()
        //{

        //}

    }
}
