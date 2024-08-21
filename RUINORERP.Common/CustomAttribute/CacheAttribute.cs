using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Common.CustomAttribute
{
    /// <summary>
    /// 方法实现缓存的标识
    /// </summary>
    [Serializable]
    public class CacheAttribute : Attribute
    {
        /// <summary>
        /// 缓存的失效时间设置，默认采用30分钟
        /// </summary>
        public int ExpirationPeriod = 30;

        /// <summary>
        /// PostSharp的调用处理，实现数据的缓存处理
        /// </summary>
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            //默认30分钟失效，如果设置过期时间，那么采用设置值
            TimeSpan timeSpan = new TimeSpan(0, 0, ExpirationPeriod, 0);

            var cache = MethodResultCache.GetCache(args.Method, timeSpan);
            var arguments = args.Arguments.ToList();//args.Arguments.Union(new[] {WindowsIdentity.GetCurrent().Name}).ToList();
            var result = cache.GetCachedResult(arguments);
            if (result != null)
            {
                args.ReturnValue = result;
                return;
            }
            else
            {
                base.OnInvoke(args);
                //调用后更新缓存
                cache.CacheCallResult(args.ReturnValue, arguments);
            }
        }
    }
}
