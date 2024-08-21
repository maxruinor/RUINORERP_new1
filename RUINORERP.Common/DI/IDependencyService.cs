namespace RUINORERP.Common.DI

{

    /// <summary>
    /// Scope实现该接口将自动注册到Ioc容器，生命周期为每次请求创建一个实例
    /// </summary>
    /// <summary>
    /// 注入标记
    /// 允许使用拦截器服务
    /// </summary>
    public interface IDependencyService: IDependency
    {
    }
}