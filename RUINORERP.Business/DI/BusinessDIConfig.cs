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

using RUINORERP.Business.Cache;
using RUINORERP.Business.Config;
using RUINORERP.IServices;
using RUINORERP.Model.Context;

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
                  .Where(t => !t.IsAssignableFrom(typeof(RUINORERP.Business.Document.DocumentConverterBase<,>)) && 
                             !t.Name.EndsWith("Converter"))
                  .AsImplementedInterfaces()
                  .AsSelf()
                  .PropertiesAutowired()
                  .InstancePerDependency()
                  .EnableInterfaceInterceptors()
                  .InterceptedBy(typeof(BaseDataCacheAOP));

            // 单独注册转换器类，不启用接口拦截
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("RUINORERP.Business"))
                  .Where(t => t.IsAssignableFrom(typeof(RUINORERP.Business.Document.DocumentConverterBase<,>)) || 
                             t.Name.EndsWith("Converter"))
                  .AsImplementedInterfaces()
                  .AsSelf()
                  .PropertiesAutowired()
                  .InstancePerDependency();

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
 
        // 注册新的优化缓存初始化服务
            builder.RegisterType<EntityCacheInitializationService>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
            // 实体业务映射服务已通过AddEntityInfoServicesWithMappings方法注册
            // 此处不再重复注册，以避免冲突

            // 注册缓存同步元数据管理器，用于管理缓存同步状态信息
            // 直接使用Autofac的单例机制，确保整个应用生命周期中只有一个实例
            builder.RegisterType<CacheSyncMetadataManager>()
                .As<ICacheSyncMetadata>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();


            // 注册新的优化缓存管理器
            //这里是单例，批量扫描程序集是 .InstancePerDependency()，这意味着每次请求都会创建一个新的实例
            builder.RegisterType<EntityCacheManager>()
                .As<IEntityCacheManager>()
                .SingleInstance()
                .PropertiesAutowired();
                
    
                
            // 注册事件驱动缓存管理器（单例模式）
            builder.RegisterType<EventDrivenCacheManager>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
                
            // 注册表结构管理器为注入式单例
            builder.RegisterType<RUINORERP.Business.Cache.TableSchemaManager>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
            
            // 注册缓存数据提供者，用于在缓存未命中时从数据库加载数据
            builder.RegisterType<SqlSugarCacheDataProvider>()
                .As<ICacheDataProvider>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

                
            // 注册缓存订阅管理器
            builder.RegisterType<CacheSubscriptionManager>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
                
            // 注册库存缓存管理器
            builder.RegisterType<InventoryCacheManager>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
  

            // 注册实体加载服务
            builder.RegisterType<EntityLoader>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
                
            // 注册单据转换器工厂
            builder.RegisterType<RUINORERP.Business.Document.DocumentConverterFactory>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
            
           
                
            // 注册联动操作管理器
            builder.RegisterType<RUINORERP.Business.Document.ActionManager>()
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

            // 注册审计日志帮助类（主类）
            builder.RegisterType<RUINORERP.Business.CommService.AuditLogHelper>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            // 注册业务处理器
            RegisterBusinessProcessors(builder);

            // 注册验证器
            RegisterValidators(builder);

            // 注册行级权限服务
            RegisterRowLevelAuthServices(builder);

            // 3. 注册动态查询帮助类
            builder.RegisterType<DynamicQueryHelper>().SingleInstance();

            // 注册泛型配置服务
            builder.RegisterGeneric(typeof(GenericConfigService<>))
                .As(typeof(IGenericConfigService<>))
                .SingleInstance()
                .InstancePerDependency()
                .PropertiesAutowired();

            // 注册泛型配置版本服务
            builder.RegisterGeneric(typeof(GenericConfigVersionService<>))
                .As(typeof(GenericConfigVersionService<>))
                .InstancePerDependency()
                .PropertiesAutowired();
            
            // 为ConfigManagerService添加单独的注册，禁用接口拦截
            // 这是因为ConfigManagerService实现的接口可能不是公开可见的，或者有其他问题导致不能使用接口拦截
            builder.RegisterType<ConfigManagerService>()
                .As<IConfigManagerService>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired()
                .InstancePerDependency();
                
            // 为ConfigEncryptionService添加单独的注册，禁用接口拦截
            // 这是因为IConfigEncryptionService接口可能不是公开可见的，需要避免接口拦截
            builder.RegisterType<ConfigEncryptionService>()
                .As<IConfigEncryptionService>()
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerDependency();
                
            // 为ConfigValidationService添加单独的注册，禁用接口拦截
            builder.RegisterType<ConfigValidationService>()
                .As<IConfigValidationService>()
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerDependency();
                
            // 为ConfigVersionService添加单独的注册，禁用接口拦截
            builder.RegisterType<ConfigVersionService>()
                .As<IConfigVersionService>()
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerDependency();
             
            AddBizMapperService(builder);
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

                // 注册BaseValidatorGeneric<T>并确保它能接收ApplicationContext
                builder.RegisterGeneric(typeof(BaseValidatorGeneric<>))
                    .WithParameter(new ResolvedParameter(
                        (pi, ctx) => pi.ParameterType == typeof(ApplicationContext),
                        (pi, ctx) => ctx.Resolve<ApplicationContext>()
                    ))
                    .InstancePerDependency()
                    .PropertiesAutowired();

                // 注册AbstractValidator<T>
                builder.RegisterGeneric(typeof(AbstractValidator<>))
                    .InstancePerDependency();

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

                    // 子类 - BaseValidatorGeneric的派生类
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
                        .InstancePerDependency()
                        .PropertiesAutowired();
                }

                // 用名称注册新的基类验证器，并确保注入ApplicationContext
                foreach (var item in NewBaseValidatorGenericlist)
                {
                    builder.RegisterType(item.Value)
                        .AsImplementedInterfaces()
                        .AsSelf()
                        .InstancePerDependency()
                        .WithParameter(new ResolvedParameter(
                            (pi, ctx) => pi.ParameterType == typeof(ApplicationContext),
                            (pi, ctx) => ctx.Resolve<ApplicationContext>()
                        ))
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
        public static void AddBizMapperService(this ContainerBuilder builder)
        {
            // 注册实体信息服务及其依赖项
            builder.RegisterType<EntityMappingService>()
                .As<IEntityMappingService>()
                .SingleInstance()
                .PropertiesAutowired();

            builder.RegisterType<DefaultRowAuthPolicyInitializationService>()
                .As<IDefaultRowAuthPolicyInitializationService>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            builder.RegisterType<BizEntityInfoConfig>()
                .SingleInstance()
                .PropertiesAutowired();

        }
    }
}