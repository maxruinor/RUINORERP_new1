using Autofac;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Business.CommService;
using RUINORERP.Business.BizMapperService;
using System;
using System.Collections.Generic;
using System.Reflection;
using RUINORERP.Common.CustomAttribute;
using FluentValidation;
using RUINORERP.Business.RowLevelAuthService;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using RUINORERP.Extensions.AOP;

namespace RUINORERP.Business.DI
{
    /// <summary>
    /// Business项目的依赖注入配置类
    /// </summary>
    public static class BusinessDIConfig
    {
        /// <summary>
        /// 配置Business项目的Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            //services.AddSingleton<ILoggerService, Log4NetService>();
            //services.AddSingleton<IAuthorizationService, AuthorizationService>();
            //services.AddSingleton<IAuthenticationService, AuthenticationService>();
 
 


            // Register business layer components
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("RUINORERP.Business"))
                  .AsImplementedInterfaces()
                  .AsSelf()
                  .PropertiesAutowired()
                  .InstancePerDependency()
                  .EnableInterfaceInterceptors()
                  .InterceptedBy(typeof(BaseDataCacheAOP));

            // Register specific business components
            builder.RegisterType<BillConverterFactory>()
                  .As<BillConverterFactory>()
                  .SingleInstance();

            // 注册业务特定的服务
            builder.RegisterType<AuthorizeController>()
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .SingleInstance();
                
            builder.RegisterType<BillConverterFactory>()
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .SingleInstance();
                
            builder.RegisterType<SqlSugarRowLevelAuthFilter>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
                
            // 注册实体业务映射服务
            builder.RegisterType<EntityInfoService>()
                .As<IEntityInfoService>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
                
            builder.RegisterType<EntityInfoConfig>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
                
            // 注册业务处理器
            RegisterBusinessProcessors(builder);
            
            // 注册验证器
            RegisterValidators(builder);
            
            // 注册行级权限服务
            RegisterRowLevelAuthServices(builder);
        }
        
        /// <summary>
        /// 注册业务处理器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterBusinessProcessors(ContainerBuilder builder)
        {
            try
            {
                var businessAssembly = Assembly.GetExecutingAssembly();
                Type[] tempTypes = businessAssembly.GetTypes();
                
                List<KeyValuePair<string, Type>> ProcessorList = new List<KeyValuePair<string, Type>>();
                var NoWantIOCAttr = typeof(NoWantIOCAttribute);
                
                for (int i = 0; i < tempTypes.Length; i++)
                {
                    // 是否为自定义特性，否则跳过，进入下一个循环
                    if (tempTypes[i].IsDefined(NoWantIOCAttr, false))
                        continue;
                        
                    if (tempTypes[i].BaseType == typeof(BaseProcessor))
                    {
                        ProcessorList.Add(new KeyValuePair<string, Type>(tempTypes[i].Name, tempTypes[i]));
                    }
                }
                
                // 用名称注册处理器
                foreach (var item in ProcessorList)
                {
                    builder.RegisterType(item.Value).Named(item.Key, typeof(BaseProcessor))
                        .AsImplementedInterfaces()
                        .AsSelf()
                        .SingleInstance()
                        .PropertiesAutowired();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"注册业务处理器失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 注册验证器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterValidators(ContainerBuilder builder)
        {
            try
            {
                var businessAssembly = Assembly.GetExecutingAssembly();
                Type[] tempTypes = businessAssembly.GetTypes();
                
                List<KeyValuePair<string, Type>> ValidatorGenericlist = new List<KeyValuePair<string, Type>>();
                List<KeyValuePair<string, Type>> NewBaseValidatorGenericlist = new List<KeyValuePair<string, Type>>();
                
                for (int i = 0; i < tempTypes.Length; i++)
                {
                    if (tempTypes[i].BaseType == null) continue;
                    
                    if (tempTypes[i].BaseType.Name.Contains("AbstractValidator")
                        && !tempTypes[i].Name.Contains("BaseValidatorGeneric")
                        && !tempTypes[i].BaseType.Name.Contains("BaseValidatorGeneric") 
                        && tempTypes[i].BaseType.IsGenericType)
                    {
                        ValidatorGenericlist.Add(new KeyValuePair<string, Type>(
                            tempTypes[i].Name.Replace("`1", ""), tempTypes[i]));
                    }
                    
                    // 基类本身
                    if (tempTypes[i].Name.Contains("BaseValidatorGeneric") && tempTypes[i].BaseType.IsGenericType)
                    {
                        builder.RegisterGeneric(typeof(BaseValidatorGeneric<>));
                        builder.RegisterGeneric(typeof(AbstractValidator<>));
                    }
                    
                    // 子类
                    if (tempTypes[i].BaseType.Name.Contains("BaseValidatorGeneric") && tempTypes[i].BaseType.IsGenericType)
                    {
                        NewBaseValidatorGenericlist.Add(new KeyValuePair<string, Type>(
                            tempTypes[i].Name.Replace("`1", ""), tempTypes[i]));
                    }
                }
                
                // 用名称注册泛型验证器
                foreach (var item in ValidatorGenericlist)
                {
                    builder.RegisterType(item.Value)
                        .AsImplementedInterfaces()
                        .AsSelf()
                        .SingleInstance()
                        .PropertiesAutowired();
                }
                
                // 用名称注册新的基类验证器
                foreach (var item in NewBaseValidatorGenericlist)
                {
                    builder.RegisterType(item.Value)
                        .AsImplementedInterfaces()
                        .AsSelf()
                        .SingleInstance()
                        .PropertiesAutowired();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"注册验证器失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 注册行级权限服务
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterRowLevelAuthServices(ContainerBuilder builder)
        {
            try
            {
                // 注册默认行级权限规则提供者
                builder.RegisterType<DefaultRowAuthRuleProvider>()
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .SingleInstance()
                    .PropertiesAutowired();
                    
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"注册行级权限服务失败: {ex.Message}");
            }
        }
    }
}