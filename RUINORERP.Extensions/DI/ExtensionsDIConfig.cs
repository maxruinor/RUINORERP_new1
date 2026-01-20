using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using RUINORERP.Extensions.AOP;
using RUINORERP.Extensions.ServiceExtensions;
using RUINORERP.Common.Helper;

namespace RUINORERP.Extensions.DI
{
    /// <summary>
    /// Extensions项目的依赖注入配置类
    /// </summary>
    public static class ExtensionsDIConfig
    {
        /// <summary>
        /// 配置Extensions项目的Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            // 注册AOP拦截器
            builder.RegisterType<BaseDataCacheAOP>();
            builder.RegisterType<BlogCacheAOP>();
            builder.RegisterType<BlogLogAOP>();
            builder.RegisterType<BlogRedisCacheAOP>();
            builder.RegisterType<BlogTranAOP>();
            builder.RegisterType<BlogUserAuditAOP>();


            // 注册服务扩展相关类
            builder.RegisterType<AutofacRegister>();

            // 批量注册Extensions程序集中的类型
            var assembly = AssemblyLoader.LoadAssembly("RUINORERP.Extensions");
            builder.RegisterAssemblyTypes(assembly)
                  .AsImplementedInterfaces()
                  .AsSelf()
                  .PropertiesAutowired()
                  .InstancePerDependency();
        }
    }
}