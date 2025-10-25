using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using RUINORERP.IServices;

namespace RUINORERP.Services.DI
{
    /// <summary>
    /// Services项目的依赖注入配置类
    /// </summary>
    public static class ServicesDIConfig
    {
        /// <summary>
        /// 配置Services项目的Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            // Register service layer components
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("RUINORERP.Services"))
                  .AsImplementedInterfaces()
                  .AsSelf()
                  .PropertiesAutowired()
                  .InstancePerDependency();

            // 注册所有服务实现
            RegisterServices(builder);

        }

        /// <summary>
        /// 注册所有服务实现
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterServices(ContainerBuilder builder)
        {
            try
            {
                var servicesAssembly = Assembly.GetExecutingAssembly();
                Type[] tempTypes = servicesAssembly.GetTypes();

                List<Type> serviceTypes = new List<Type>();

                for (int i = 0; i < tempTypes.Length; i++)
                {
                    // 筛选出所有服务实现类（继承自BaseServices且实现对应接口）
                    if (tempTypes[i].Name.EndsWith("Services") && 
                        tempTypes[i].BaseType != null && 
                        tempTypes[i].BaseType.Name.StartsWith("BaseServices"))
                    {
                        serviceTypes.Add(tempTypes[i]);
                    }
                }

                // 批量注册服务
                foreach (var serviceType in serviceTypes)
                {
                    // 获取服务接口（约定：服务实现类名为XXXServices，对应接口名为IXXXServices）
                    string interfaceName = "I" + serviceType.Name;
                    Type serviceInterface = serviceType.GetInterface(interfaceName);
                    
                    if (serviceInterface != null)
                    {
                        builder.RegisterType(serviceType)
                            .As(serviceInterface)
                            .AsSelf()
                            .InstancePerLifetimeScope()
                            .PropertiesAutowired();
                    }
                }
                
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"注册服务失败: {ex.Message}");
            }
        }
    }
}