﻿using Autofac;
using Autofac.Extras.DynamicProxy;
using RUINORERP.Extensions.AOP;
using RUINORERP.Common;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices.BASE;
using RUINORERP.Model;
using RUINORERP.Repository.Base;
using RUINORERP.Services.BASE;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Common.Helper;

namespace RUINORERP.Extensions
{
    public class AutofacModuleRegister : Autofac.Module
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AutofacModuleRegister));

        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();


            #region 带有接口层的服务注入

            var servicesDllFile = Path.Combine(basePath, "RUINORERP.Services.dll");
            var repositoryDllFile = Path.Combine(basePath, "RUINORERP.Repository.dll");

            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                var msg = "Repository.dll和service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                log.Error(msg);
                throw new Exception(msg);
            }

            //!!!!!!下面的开关是统计绑定的，相当于 [Intercept(typeof(LogInterceptor))] // Person 使用 LogInterceptor拦截器
            // AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
            var cacheType = new List<Type>();
            if (AppSettings.app(new string[] {"AppSettings", "RedisCachingAOP", "Enabled"}).ObjToBool())
            {
                builder.RegisterType<BlogRedisCacheAOP>();
                cacheType.Add(typeof(BlogRedisCacheAOP));
            }

            if (AppSettings.app(new string[] {"AppSettings", "MemoryCachingAOP", "Enabled"}).ObjToBool())
            {
                builder.RegisterType<BlogCacheAOP>();
                cacheType.Add(typeof(BlogCacheAOP));
            }

            if (AppSettings.app(new string[] {"AppSettings", "TranAOP", "Enabled"}).ObjToBool())
            {
                builder.RegisterType<BlogTranAOP>();
                cacheType.Add(typeof(BlogTranAOP));
            }

            if (AppSettings.app(new string[] {"AppSettings", "LogAOP", "Enabled"}).ObjToBool())
            {
              //  builder.RegisterType<BlogLogAOP>();
               // cacheType.Add(typeof(BlogLogAOP));
            }

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();//注册仓储
            builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>)).InstancePerDependency();//注册服务

            // 获取 Service.dll 程序集服务，并注册
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired()
                .EnableInterfaceInterceptors()       //引用Autofac.Extras.DynamicProxy;
                .InterceptedBy(cacheType.ToArray()); //允许将拦截器服务的列表分配给注册。

            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerDependency();

            builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            #endregion

            #region 没有接口层的服务层注入

            //因为没有接口层，所以不能实现解耦，只能用 Load 方法。
            //注意如果使用没有接口的服务，并想对其使用 AOP 拦截，就必须设置为虚方法
            //var assemblysServicesNoInterfaces = Assembly.Load("RUINORERP.Services");
            //builder.RegisterAssemblyTypes(assemblysServicesNoInterfaces);

            #endregion

            #region 没有接口的单独类，启用class代理拦截

            //只能注入该类中的虚方法，且必须是public
            //这里仅仅是一个单独类无接口测试，不用过多追问
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Love)))
                .EnableClassInterceptors()
                .InterceptedBy(cacheType.ToArray());

            #endregion

            #region 单独注册一个含有接口的类，启用interface代理拦截

            //不用虚方法
            //builder.RegisterType<AopService>().As<IAopService>()
            //   .AsImplementedInterfaces()
            //   .EnableInterfaceInterceptors()
            //   .InterceptedBy(typeof(BlogCacheAOP));

            #endregion
        }
    }
}