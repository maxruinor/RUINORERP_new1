using Autofac;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Business.CommService;
using RUINORERP.Business.RowLevelAuthService;
using RUINORERP.Repository;
using RUINORERP.Services;
using RUINORERP.IServices;
using System;
using System.Collections.Generic;
using System.Reflection;
using RUINORERP.Common.CustomAttribute;
using FluentValidation;
using RUINORERP.UI.WorkFlowTester;
using RUINORERP.UI.SysConfig;
using RUINORERP.UI.IM;
using RUINORERP.UI.ClientCmdService;
using RUINORERP.UI.FM;
using RUINORERP.UI.Monitoring.Auditing;
using RUINORERP.UI.BusinessService.SmartMenuService;
using RUINORERP.UI.WorkFlowDesigner;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;

namespace RUINORERP.UI
{
    /// <summary>
    /// UI项目的依赖注入配置类
    /// </summary>
    public static class UIStartup
    {
        /// <summary>
        /// 配置UI项目的Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            // 注册UI专用服务
            RegisterUIServices(builder);
            
            // 注册窗体相关服务
            RegisterFormServices(builder);
            
            // 注册工作流相关服务
            RegisterWorkflowServices(builder);
            
            // 注册配置相关服务
            RegisterConfigServices(builder);
            
            // 注册各层依赖（通过各项目自己的配置类）
            Repository.RepositoryStartup.ConfigureContainer(builder);     // 注册仓储层依赖
            IServices.IServicesStartup.ConfigureContainer(builder);       // 注册服务接口层依赖
            Services.ServicesStartup.ConfigureContainer(builder);         // 注册服务实现层依赖
            Business.BusinessStartup.ConfigureContainer(builder);         // 注册业务逻辑层依赖
        }

        /// <summary>
        /// 注册UI专用服务
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterUIServices(ContainerBuilder builder)
        {
            // 注册主窗体
            builder.RegisterType<MainForm>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
                
            // 注册配置管理器
            builder.RegisterType<ConfigManager>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
                
            // 注册菜单跟踪器
            builder.RegisterType<MenuTracker>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
                
            // 注册GridView相关服务
            builder.RegisterType<GridViewRelated>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
        }

        /// <summary>
        /// 注册窗体相关服务
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterFormServices(ContainerBuilder builder)
        {
            // 注册基础窗体
            builder.RegisterType<frmBase>()
                .AsSelf()
                .PropertiesAutowired();
                
            builder.RegisterType<BaseUControl>()
                .AsSelf()
                .PropertiesAutowired();
                
            builder.RegisterType<BaseQuery>()
                .AsSelf()
                .PropertiesAutowired();
                
            // 注册其他UI控件和服务
            builder.RegisterType<UCBaseClass>()
                .AsSelf()
                .PropertiesAutowired();
        }

        /// <summary>
        /// 注册工作流相关服务
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterWorkflowServices(ContainerBuilder builder)
        {
            // 注册工作流设计器相关服务
            builder.RegisterType<WorkflowDesignerService>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
                
            // 注册工作流测试相关服务
            builder.RegisterType<WorkflowTestService>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
        }

        /// <summary>
        /// 注册配置相关服务
        /// </summary>
        /// <param name="builder">容器构建器</param>
        private static void RegisterConfigServices(ContainerBuilder builder)
        {
            // 注册系统配置相关服务
            builder.RegisterType<SysConfigService>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired();
                
            // 注册审计日志服务
            builder.RegisterType<AuditLogHelper>()
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }
    }
}