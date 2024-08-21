using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy; // 添加引用
using RUINORERP.Extensions.AOP;
using RUINORERP.Model;

namespace RUINORERP.Business
{
    //CodeBuilder.exe
    /// <summary>
    /// 定义被拦截的类
    /// </summary>
    //[Intercept(typeof(LogInterceptor))] // Person 使用 LogInterceptor拦截器
    [Intercept(typeof(BaseDataCacheAOP))] // Person 使用 LogInterceptor拦截器
    public class PersonBus
    {
        public int Age { get; set; }

        public PersonBus() { }

        // 非虚方法不会被拦截到
        public void Method_NoVirtua()
        {
            Console.WriteLine("Method_NoVirtua");
        }

        public virtual void Method2PersonBus()
        {
            Console.WriteLine("Method2");
        }


        public virtual string Method3PersonBus(string para1, string para2)
        {
            Console.WriteLine("Method3");
            return para1 + "&" + para2;
        }
    }
}