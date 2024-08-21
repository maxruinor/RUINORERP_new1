using System;
using System.Collections.Generic;
using System.Text;

using Autofac.Extras.DynamicProxy; // 添加引用
using RUINORERP.Extensions.AOP;

namespace RUINORERP.UI
{
    /// <summary>
    /// 定义被拦截的类
    /// </summary>
    //[Intercept(typeof(LogInterceptor))] // Person 使用 LogInterceptor拦截器
    //[Intercept(typeof(BaseDataCacheAOP))] // Person 使用 LogInterceptor拦截器
    public class Person
    {
        public int Age { get; set; }

        public Person() { }

        // 非虚方法不会被拦截到
        public void Method_NoVirtua()
        {
            Console.WriteLine("Method_NoVirtua");
        }

        public virtual void Method2()
        {
            Console.WriteLine("Method2");
        }

        public virtual string Method3(string para1, string para2)
        {
            Console.WriteLine("Method3");
            return para1 + "&" + para2;
        }
    }
}