using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 服务注册管理器 - 负责管理和执行所有实现了IServiceRegistrationContract接口的服务注册器
    /// </summary>
    public static class ServiceRegistrationManager
    {
        /// <summary>
        /// 执行所有实现了IServiceRegistrationContract接口的服务注册器
        /// </summary>
        /// <param name="builder">Autofac容器构建器</param>
        /// <param name="assemblies">要扫描的程序集，默认为当前程序集</param>
        public static void RegisterAllServices(ContainerBuilder builder, params Assembly[] assemblies)
        {
            // 如果没有指定程序集，则使用当前程序集
            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetExecutingAssembly() };
            }
            
            // 获取所有实现了IServiceRegistrationContract接口的非抽象类
            var registrationTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IServiceRegistrationContract).IsAssignableFrom(type)
                    && !type.IsAbstract
                    && !type.IsInterface)
                .ToList();
            
            // 实例化并执行每个服务注册器
            foreach (var registrationType in registrationTypes)
            {
                try
                {
                    var registration = (IServiceRegistrationContract)Activator.CreateInstance(registrationType);
                    registration.Register(builder);
                }
                catch (Exception ex)
                {
                    // 实际应用中应该使用日志记录异常
                    Console.WriteLine($"Failed to register services using {registrationType.FullName}: {ex.Message}");
                }
            }
        }
        
        /// <summary>
        /// 注册特定类型的服务注册器
        /// </summary>
        /// <typeparam name="T">实现了IServiceRegistrationContract接口的类型</typeparam>
        /// <param name="builder">Autofac容器构建器</param>
        public static void RegisterService<T>(ContainerBuilder builder) where T : IServiceRegistrationContract, new()
        {
            try
            {
                var registration = new T();
                registration.Register(builder);
            }
            catch (Exception ex)
            {
                // 实际应用中应该使用日志记录异常
                Console.WriteLine($"Failed to register services using {typeof(T).FullName}: {ex.Message}");
            }
        }
    }
}