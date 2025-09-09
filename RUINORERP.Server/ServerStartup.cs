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
using RUINORERP.Extensions.DI;

namespace RUINORERP.Server
{
    /// <summary>
    /// Server项目的依赖注入配置类
    /// </summary>
    public static class ServerStartup
    {
        /// <summary>
        /// 配置Server项目的Autofac容器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            // 注册各层依赖（通过各项目自己的配置类）
            Repository.RepositoryStartup.ConfigureContainer(builder);     // 注册仓储层依赖
            IServices.IServicesStartup.ConfigureContainer(builder);       // 注册服务接口层依赖
            Services.ServicesStartup.ConfigureContainer(builder);         // 注册服务实现层依赖
            Business.BusinessStartup.ConfigureContainer(builder);         // 注册业务逻辑层依赖
            ExtensionsDIConfig.ConfigureContainer(builder);               // 注册扩展层依赖
        }
    }
}