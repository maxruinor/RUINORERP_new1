using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using RUINORERP.IServices.BASE;

namespace RUINORERP.IServices.DI
{
    /// <summary>
    /// IServices项目的依赖注入配置类
    /// </summary>
    public static class IServicesDIConfig
    {
        /// <summary>
        /// 配置IServices项目的Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            // 注册服务接口
            RegisterServiceInterfaces(builder);
        }

        /// <summary>
        /// 注册所有服务接口
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterServiceInterfaces(ContainerBuilder builder)
        {
            try
            {
                var iServicesAssembly = Assembly.GetExecutingAssembly();
                Type[] tempTypes = iServicesAssembly.GetTypes();

                List<Type> interfaceTypes = new List<Type>();

                for (int i = 0; i < tempTypes.Length; i++)
                {
                    // 筛选出所有服务接口（继承自IBaseServices）
                    if (tempTypes[i].IsInterface && 
                        tempTypes[i].Name.StartsWith("I") && 
                        tempTypes[i] != typeof(IBaseServices<>) &&
                        tempTypes[i].Name.EndsWith("Services"))
                    {
                        var baseInterface = tempTypes[i].GetInterface("IBaseServices`1");
                        if (baseInterface != null)
                        {
                            interfaceTypes.Add(tempTypes[i]);
                        }
                    }
                }

                // 批量注册服务接口（用于后续在Services项目中关联实现）
                foreach (var interfaceType in interfaceTypes)
                {
                    builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                        .Where(t => t.GetInterface(interfaceType.Name) != null)
                        .As(interfaceType)
                        .InstancePerLifetimeScope();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"注册服务接口失败: {ex.Message}");
            }
        }
    }
}