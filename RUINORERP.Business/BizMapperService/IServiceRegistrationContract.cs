using System;
using Autofac;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 服务注册契约接口 - 定义统一的服务注册规范
    /// </summary>
    public interface IServiceRegistrationContract
    {
        /// <summary>
        /// 注册服务到Autofac容器
        /// </summary>
        /// <param name="builder">Autofac容器构建器</param>
        void Register(ContainerBuilder builder);
    }
}