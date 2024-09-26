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
 

namespace RUINORERP.WebServer
{

    /// <summary>
    /// autofac注册
    /// </summary>
    public class AutofacServiceRegister : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region


            /*
            #region 自动注入对应的服务接口


         // var baseType = typeof(IDependencyRepository);
          var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
          var getFiles = Directory.GetFiles(path, "*.dll").Where(o => o.ToLower().StartsWith(@path.ToLower() + "ruinor"));//路径筛选

          List<string> dlls = new List<string>();
          foreach (var file in getFiles)
          {
              FileInfo fi = new FileInfo(file);
              if (fi.Name.ToLower().Contains("ruinor"))
              {
                   dlls.Add(file);
              }
          }
            //dlls.Add(System.IO.Path.Combine(Application.StartupPath, System.AppDomain.CurrentDomain.FriendlyName));
            // dlls.Add(System.IO.Path.Combine(Application.StartupPath, "RUINORERP.Entity.dll"));

             var referencedAssemblies = dlls.ToArray().Select(System.Reflection.Assembly.LoadFrom).ToList();  //.Select(o=> Assembly.LoadFrom(o))         
             var types = referencedAssemblies.SelectMany(o => o.GetTypes());
            List<System.Reflection.Assembly> alldlls = RUINORERP.Common.Helper.AssemblyHelper.GetReferanceAssemblies(AppDomain.CurrentDomain, true);
            //var dependencyService = typeof(IDependencyService);
            //var ServiceTypes = types.Where(x => dependencyService.IsAssignableFrom(x) && x != dependencyService).ToArray();

            //var dependencyRepository = typeof(IDependencyRepository);
            //var RepositoryTypes = types.Where(x => dependencyService.IsAssignableFrom(x) && x != dependencyRepository).ToArray();



            builder.RegisterTypes(types.ToArray())
               .AsImplementedInterfaces()
               .PropertiesAutowired()
               .InstancePerDependency()
               .EnableInterfaceInterceptors()
               .InterceptedBy(types.ToArray());
            //var types = referencedAssemblies
            //    .SelectMany(a => a.DefinedTypes)
            //    .Select(type => type.AsType())
            //    .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToList();
            //var implementTypes = types.Where(x => x.IsClass).ToList();
            //var interfaceTypes = types.Where(x => x.IsInterface).ToList();

            int aa = types.Count<Type>();
            int counter = 0;
            foreach (var implementType in types)
            {
                counter++;
                if (implementType.FullName.Contains("UnitOfWork"))
                {

                }
                //if (typeof(IDependencyRepository).IsAssignableFrom(implementType))
                //{
                //    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                //    if (interfaceType != null)
                //        services.AddScoped(interfaceType, implementType);
                //}
                //else if (typeof(IDependencyRepository).IsAssignableFrom(implementType))
                //{
                //    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                //    if (interfaceType != null)
                //        services.AddSingleton(interfaceType, implementType);
                //}
                //else
                //{
                //    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                //    if (interfaceType != null)
                //        services.AddTransient(interfaceType, implementType);
                //}
            }

            //foreach (var implementType in implementTypes)
            //{
            //    if (typeof(IDependencyRepository).IsAssignableFrom(implementType))
            //    {
            //        var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
            //        if (interfaceType != null)
            //            services.AddScoped(interfaceType, implementType);
            //    }
            //    else if (typeof(IDependencyRepository).IsAssignableFrom(implementType))
            //    {
            //        var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
            //        if (interfaceType != null)
            //            services.AddSingleton(interfaceType, implementType);
            //    }
            //    else
            //    {
            //        var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
            //        if (interfaceType != null)
            //            services.AddTransient(interfaceType, implementType);
            //    }
            //}
            #endregion
            */
            #endregion


            //var dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
            //builder.RegisterAssemblyTypes(dalAssemble)
            //      .AsImplementedInterfaces().AsSelf()
            //      .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //      .PropertiesAutowired();//允许属性注入

            //var dalAssembleCore = Assembly.LoadFrom("RUINOR.Framework.Core.dll");
            //builder.RegisterAssemblyTypes(dalAssembleCore)
            //      .AsImplementedInterfaces().AsSelf()
            //      .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //      .PropertiesAutowired();//允许属性注入
            //services.AddScoped<ICurrentUser, CurrentUser>();
            //services.AddSingleton(typeof(AutoMapperConfig));
            //services.AddScoped<IMapper, Mapper>();
            //builder.RegisterType(ipmap)

            log4net.Repository.ILoggerRepository repository = LogManager.CreateRepository("kxrz");
            log4net.Config.XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            var loggerRepository = repository;
            var _loger = log4net.LogManager.GetLogger(loggerRepository.Name, "infoAppender");
            // builder.Register(c => _loger);


            //事务 缓存 AOP
            //!!!!!!下面的开关是统计绑定的，相当于 [Intercept(typeof(LogInterceptor))] // Person 使用 LogInterceptor拦截器
            var cacheType = new List<Type>();
            //builder.RegisterType<TransactionAop>();
            //cacheType.Add(typeof(TransactionAop));
            //builder.RegisterType<BlogLogAOP>();
            //cacheType.Add(typeof(BlogLogAOP));
            //DbBizLogAOP dbaop = new DbBizLogAOP("huang2023");
            //builder.RegisterInstance<DbBizLogAOP>(dbaop);
            //builder.RegisterType<DbBizLogAOP>();
            //cacheType.Add(typeof(DbBizLogAOP));

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


            /* csla
              var dependencyDal = typeof(IDependencyDal);
              var dependencyDalArray = GlobalData.FxAllTypes
                  .Where(x => dependencyDal.IsAssignableFrom(x) && x != dependencyDal).ToArray();
              builder.RegisterTypes(dependencyDalArray)
                  .AsImplementedInterfaces()
                  .PropertiesAutowired()
                  .InstancePerDependency()
                  .EnableInterfaceInterceptors()
                  .InterceptedBy(cacheType.ToArray());
            */

            //控制器
            /*var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();*/



            builder.RegisterType<DisposableContainer>()
                .As<IDisposableContainer>()
                .InstancePerLifetimeScope();
        }
    }



    //public static IList<Type> GetAllTypes(List<System.Reflection.Assembly> assemblies)
    //{
    //    List<Type> list = new List<Type>();
    //    foreach (var assembly in assemblies)
    //    {
    //        var typeinfos = assembly.DefinedTypes;
    //        foreach (var typeinfo in typeinfos)
    //        {
    //            list.Add(typeinfo.AsType());
    //        }
    //    }
    //    return list;
    //}

}


