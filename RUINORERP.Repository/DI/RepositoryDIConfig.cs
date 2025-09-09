using Autofac;
using Autofac.Core;
using RUINORERP.IRepository.Base;
using RUINORERP.Repository.Base;
using RUINORERP.Repository.UnitOfWorks;
using System;

namespace RUINORERP.Repository.DI
{
    /// <summary>
    /// Repository项目的依赖注入配置类
    /// </summary>
    public static class RepositoryDIConfig
    {
        /// <summary>
        /// 配置Repository项目的Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            // Register repository layer components
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("RUINORERP.Repository"))
                  .AsImplementedInterfaces()
                  .AsSelf()
                  .PropertiesAutowired()
                  .InstancePerDependency();


            // 注册仓储基础类
            builder.RegisterGeneric(typeof(BaseRepository<>))
                .As(typeof(IBaseRepository<>))
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
                
            // 注册工作单元管理器
            builder.RegisterType<UnitOfWorkManage>()
                .As<IUnitOfWorkManage>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();



        }
    }
}


 