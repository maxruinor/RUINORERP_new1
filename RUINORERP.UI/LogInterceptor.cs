using System;
using System.Linq;
using Castle.DynamicProxy; // 添加引用

namespace RUINORERP.UI
{
    /// <summary>
    /// 拦截器需要实现 IInterceptor 接口
    /// </summary>
    public class LogInterceptor : IInterceptor
    {


        public void Intercept(IInvocation invocation)
        {
            #region 方法执行前
            string beforeExe_msg = string.Format("方法执行前:拦截[{0}]类下的方法[{1}]的参数是[{2}]",
                invocation.InvocationTarget.GetType(),
                invocation.Method.Name, string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
            Console.WriteLine(beforeExe_msg);
            #endregion

            #region 方法执行
            invocation.Proceed();
            #endregion

            #region 方法执行完成后
            string afterExe_msg = string.Format("方法执行完毕，返回结果：{0}", invocation.ReturnValue);
            Console.WriteLine(afterExe_msg);
            #endregion
        }
    }
}