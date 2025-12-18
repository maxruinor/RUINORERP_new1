using RUINORERP.Common;
using Castle.DynamicProxy;
using System.Linq;

namespace RUINORERP.Extensions.AOP
{
    /// <summary>
    /// 面向切面的缓存使用
    /// 内存级缓存应用于基础资料
    /// 有两种使用方法，一种是在类名前特性标记，另一种是IOC注册时指定
    /// </summary>
    public class BaseDataCacheAOP : CacheAOPbase
    {
        //通过注入的方式，把缓存操作接口通过构造函数注入
        private readonly ICaching _cache;
        public BaseDataCacheAOP(ICaching cache)
        {
            _cache = cache;
        }

        //Intercept方法是拦截的关键所在，也是IInterceptor接口中的唯一定义
        public override void Intercept(IInvocation invocation)
        {
            #region 方法执行前
            string beforeExe_msg = string.Format("AOP方法执行前:拦截[{0}]类下的方法[{1}]的参数是[{2}]",
                invocation.InvocationTarget.GetType(),
                invocation.Method.Name, string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
            //System.System.Diagnostics.Debug.WriteLine(beforeExe_msg);
            #endregion

            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            //如果需要验证
            var CachingAttribute = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute));
            if (CachingAttribute is CachingAttribute qCachingAttribute)
            {
                //获取自定义缓存键
                var cacheKey = CustomCacheKey(invocation);
                //根据key获取相应的缓存值
                var cacheValue = _cache.Get(cacheKey);
                if (cacheValue != null)
                {
                    //将当前获取到的缓存值，赋值给当前执行方法
                    invocation.ReturnValue = cacheValue;
                    return;
                }
                //去执行当前的方法
                invocation.Proceed();
                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    _cache.Set(cacheKey, invocation.ReturnValue, qCachingAttribute.AbsoluteExpiration);
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }
        }
    }

}
