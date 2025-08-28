using Autofac;
using Autofac.Core;
using System;
using System.Linq;
using System.Reflection;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// Autofac模块 - 提供统一的服务注册方式
    /// </summary>
    public class ServiceRegistrationModule : Autofac.Module
    {
        /// <summary>
        /// 加载模块时注册服务
        /// </summary>
        /// <param name="builder">Autofac容器构建器</param>
        protected override void Load(ContainerBuilder builder)
        {
            try
            {
                // 注册全局服务注册契约
                RegisterAllServiceContracts(builder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed to load ServiceRegistrationModule: {0}", ex.Message));
            }
        }

        /// <summary>
        /// 注册所有实现了IServiceRegistrationContract接口的服务注册器
        /// </summary>
        /// <param name="builder">Autofac容器构建器</param>
        private void RegisterAllServiceContracts(ContainerBuilder builder)
        {
            try
            {
                // 获取当前程序集
                var currentAssembly = Assembly.GetExecutingAssembly();

                // 注册所有实现了IServiceRegistrationContract接口的非抽象类
                var registrationTypes = currentAssembly.GetTypes()
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
                        Console.WriteLine(string.Format("Failed to register services using {0}: {1}", 
                            registrationType.FullName, ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed to register service contracts: {0}", ex.Message));
            }
        }
    }
}