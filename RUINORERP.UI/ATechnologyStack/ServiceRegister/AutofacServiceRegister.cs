using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.DynamicProxy;
using System.IO;
using RUINORERP.Common.DI;
using RUINORERP.Common.Helper;
using log4net;
using RUINORERP.Common.Global;
using RUINORERP.Extensions.AOP;
using RUINORERP.UI.ATechnologyStack.AOP;

namespace RUINORERP.UI.ATechnologyStack.ServiceRegister
{

    /// <summary>
    /// autofac注册
    /// </summary>
    public class AutofacServiceRegister : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region
                   
            #endregion
                       

            //事务 缓存 AOP
            //!!!!!!下面的开关是统计绑定的，相当于 [Intercept(typeof(LogInterceptor))] // Person 使用 LogInterceptor拦截器
            var cacheType = new List<Type>();
            //builder.RegisterType<TransactionAop>();
            //cacheType.Add(typeof(TransactionAop));
            //builder.RegisterType<BlogLogAOP>();
            //cacheType.Add(typeof(BlogLogAOP));
            //DbBizLogAOP dbaop = new DbBizLogAOP("huang2023");
            //builder.RegisterInstance<DbBizLogAOP>(dbaop);
            builder.RegisterType<DbBizLogAOP>();
            cacheType.Add(typeof(DbBizLogAOP));

            // 获取所有待注入服务类
            var dependencyService = typeof(IDependencyService);
            var dependencyServiceArray = GlobalData.FxAllTypes
                .Where(x => dependencyService.IsAssignableFrom(x) && x != dependencyService).ToArray();
            builder.RegisterTypes(dependencyServiceArray)
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(cacheType.ToArray());//这里注入了 //!!!!!!下面的开关是统计绑定的，相当于 [Intercept(typeof(LogInterceptor))] // Person 使用 LogInterceptor拦截器


            // 获取所有待注入仓储类
            var dependencyRepository = typeof(IDependencyRepository);
            var dependencyRepositoryArray = GlobalData.FxAllTypes
                .Where(x => dependencyRepository.IsAssignableFrom(x) && x != dependencyRepository).ToArray();
            builder.RegisterTypes(dependencyRepositoryArray)
                .AsImplementedInterfaces()
                .InstancePerDependency();



            builder.RegisterType<DisposableContainer>()
                .As<IDisposableContainer>()
                .InstancePerLifetimeScope();
        }
    }



    
}


