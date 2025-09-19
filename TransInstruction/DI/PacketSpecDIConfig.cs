using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransInstruction.DI
{
    public static class PacketSpecDIConfig
    {
        /// <summary>
        /// 配置Repository项目的Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            // Register repository layer components
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("TransInstruction"))
                  .AsImplementedInterfaces()
                  .AsSelf()
                  .PropertiesAutowired()
            .InstancePerDependency();

            //builder.RegisterType<FileStorageService>()
            //.As<IFileStorageService>()
            //.SingleInstance();

 

        }
    }
}
