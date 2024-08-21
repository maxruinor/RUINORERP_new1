using System;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;

namespace RUINOR.Framework.Core.Common.DI
{
    public abstract class BaseFilterAttribute : Attribute, IFilter
    {
        public abstract void OnActionExecuted(IInvocation invocation);
        public abstract void OnActionExecuting(IInvocation invocation);
    }

}