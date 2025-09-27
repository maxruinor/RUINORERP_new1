using Autofac;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Business.CommService;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.LogicaService;
using RUINORERP.Extensions;
using RUINORERP.Business.ReminderService;
using System;
using System.Collections.Generic;
using System.Reflection;
using RUINORERP.Common.CustomAttribute;
using FluentValidation;
using RUINORERP.Business.RowLevelAuthService;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using RUINORERP.Extensions.AOP;
using Microsoft.Extensions.DependencyInjection;

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
            // Register business layer components
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("RUINORERP.Business"))
                  .AsImplementedInterfaces()
                  .AsSelf()
                  .PropertiesAutowired()
                  .InstancePerDependency()
                  .EnableInterfaceInterceptors()
                  .InterceptedBy(typeof(BaseDataCacheAOP));

            // 注册CommonController
            builder.RegisterType<CommonController>()
             .As<ICommonController>()
             .PropertiesAutowired()
             .SingleInstance()
             .EnableInterfaceInterceptors()
             .InterceptedBy(typeof(BaseDataCacheAOP));

            // 注册业务特定的服务
            builder.RegisterType<AuthorizeController>()
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .SingleInstance();
                
            // 注册BillConverterFactory - 移除重复注册
            builder.RegisterType<BillConverterFactory>()
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .SingleInstance();

            // 实体业务映射服务已通过AddEntityInfoServicesWithMappings方法注册
            // 此处不再重复注册，以避免冲突

            // 注册BizCacheHelper缓存帮助类
            builder.RegisterType<BizCacheHelper>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
                
            // 注册实体加载服务
            builder.RegisterType<EntityLoader>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
                
            builder.RegisterType<DuplicateCheckService>()
                .As<IDuplicateCheckService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
                
            builder.RegisterType<UnitController>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
                
            // 注册提醒服务
            builder.RegisterType<ReminderResultManager>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            // 注册审计日志服务
            builder.RegisterType<AuditLogService>()
                .As<IAuditLogService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
            
            // 注册财务模块审计日志服务
            builder.RegisterType<FMAuditLogService>()
                .As<IFMAuditLogService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
            
            // 注册审计日志帮助类
            builder.RegisterType<RUINORERP.Business.CommService.FMAuditLogHelper>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            // 注册业务处理器
            RegisterBusinessProcessors(builder);
            
            // 注册验证器
            RegisterValidators(builder);
            
            // 注册行级权限服务
            RegisterRowLevelAuthServices(builder);

            AddBusinessServices(builder);
        }
        
        /// <summary>
        /// 注册业务处理器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterBusinessProcessors(ContainerBuilder builder)
        {
            try
            {
                // 获取当前程序集
                var businessAssembly = Assembly.GetExecutingAssembly();
                Type[] tempTypes = businessAssembly.GetTypes();
                
                // 存储需要注册的处理器
                List<KeyValuePair<string, Type>> ProcessorList = new List<KeyValuePair<string, Type>>();
                var NoWantIOCAttr = typeof(NoWantIOCAttribute);
                
                // 筛选继承自BaseProcessor且未标记NoWantIOCAttribute的类型
                for (int i = 0; i < tempTypes.Length; i++)
                {
                    // 跳过标记了NoWantIOCAttribute的类型
                    if (tempTypes[i].IsDefined(NoWantIOCAttr, false))
                        continue;
                        
                    // 筛选BaseProcessor的子类
                    if (tempTypes[i].BaseType == typeof(BaseProcessor))
                    {
                        ProcessorList.Add(new KeyValuePair<string, Type>(tempTypes[i].Name, tempTypes[i]));
                    }
                }
                
                // 注册所有筛选出的处理器
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
                // 注册行级权限过滤器
                builder.RegisterType<SqlSugarRowLevelAuthFilter>()
                    .AsSelf()
                    .SingleInstance()
                    .PropertiesAutowired();
                    
                // 注册默认行级权限规则提供者
                builder.RegisterType<DefaultRowAuthRuleProvider>()
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .SingleInstance()
                    .PropertiesAutowired();
                    
                // 注册行级权限服务
                builder.RegisterType<RowAuthService>()
                    .As<IRowAuthService>()
                    .AsSelf()
                    .InstancePerLifetimeScope()
                    .PropertiesAutowired();
                    
                // 注册默认行级权限策略初始化服务
                // 注意：这里使用Scoped生命周期以匹配ServiceCollectionExtensions中的配置
                builder.RegisterType<DefaultRowAuthPolicyInitializationService>()
                    .As<IDefaultRowAuthPolicyInitializationService>()
                    .AsSelf()
                    .InstancePerLifetimeScope()
                    .PropertiesAutowired();
                    
              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"注册行级权限服务失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 配置Business项目的服务，初始化实体映射
        /// </summary>
        /// <param name="builder">Autofac容器构建器</param>
        public static void AddBusinessServices(this ContainerBuilder builder)
        {
            // 注册实体信息服务及其依赖项
            builder.RegisterType<BusinessEntityMappingService>()
                .As<IBusinessEntityMappingService>()
                .SingleInstance()
                .PropertiesAutowired();
                
            builder.RegisterType<DefaultRowAuthPolicyInitializationService>()
                .As<IDefaultRowAuthPolicyInitializationService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
                
            builder.RegisterType<EntityInfoConfig>()
                .SingleInstance()
                .PropertiesAutowired();
        }
    }
}