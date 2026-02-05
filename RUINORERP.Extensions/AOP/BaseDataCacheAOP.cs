using RUINORERP.Common;
using Castle.DynamicProxy;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace RUINORERP.Extensions.AOP
{
    /// <summary>
    /// 面向切面的缓存使用
    /// 内存级缓存应用于基础资料
    /// 有两种使用方法，一种是在类名前特性标记，另一种是IOC注册时指定
    /// </summary>
    public class BaseDataCacheAOP : CacheAOPbase
    {
        // 通过注入的方式，把缓存操作接口通过构造函数注入
        private readonly ICaching _cache;
        
        // 监控和诊断统计信息
        private static long _cacheHits = 0;
        private static long _cacheMisses = 0;
        private static long _cacheErrors = 0;
        private static long _totalExecutionTime = 0;
        private static int _executionCount = 0;
        
        // 构造函数
        public BaseDataCacheAOP(ICaching cache)
        {
            _cache = cache;
        }
        
        // 监控统计信息属性
        public static long CacheHits => _cacheHits;
        public static long CacheMisses => _cacheMisses;
        public static long CacheErrors => _cacheErrors;
        public static long TotalExecutionTime => _totalExecutionTime;
        public static int ExecutionCount => _executionCount;
        public static double AverageExecutionTime => ExecutionCount > 0 ? (double)TotalExecutionTime / ExecutionCount : 0;

        //Intercept方法是拦截的关键所在，也是IInterceptor接口中的唯一定义
        public override void Intercept(IInvocation invocation)
        {
            // 记录开始时间
            var startTime = Stopwatch.GetTimestamp();
            
            try
            {
                #region 方法执行前
                string beforeExe_msg = string.Format("AOP方法执行前:拦截[{0}]类下的方法[{1}]的参数是[{2}]",
                    invocation.InvocationTarget.GetType(),
                    invocation.Method.Name, string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
#if DEBUG
                {
                    System.Diagnostics.Debug.WriteLine(beforeExe_msg);
                }
#endif
                #endregion

                // 排除不需要缓存的类型的方法拦截
                var targetType = invocation.InvocationTarget.GetType();
                var typeName = targetType.Name;
                var fullTypeName = targetType.FullName;
                
                // 排除数据库操作相关类型
                if (typeName.Contains("UnitOfWork") || typeName.Contains("DbClient") || typeName.Contains("Repository"))
                {
                    invocation.Proceed();
                    return;
                }
                
                // 排除缓存相关类型
                if (typeName.Contains("Cache") || typeName.Contains("Caching"))
                {
                    invocation.Proceed();
                    return;
                }
                
                // 排除服务相关类型
                if (typeName.Contains("Service") && !typeName.Contains("Business"))
                {
                    invocation.Proceed();
                    return;
                }
                
                // 排除工具类和辅助类
                if (typeName.Contains("Helper") || typeName.Contains("Util") || typeName.Contains("Tool"))
                {
                    invocation.Proceed();
                    return;
                }
                
                // 排除系统级组件
                if (fullTypeName.Contains("System.") || fullTypeName.Contains("Microsoft."))
                {
                    invocation.Proceed();
                    return;
                }

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
                        // 缓存命中
                        System.Threading.Interlocked.Increment(ref _cacheHits);
                        
                        //将当前获取到的缓存值，赋值给当前执行方法
                        invocation.ReturnValue = cacheValue;
                        return;
                    }
                    else
                    {
                        // 缓存未命中
                        System.Threading.Interlocked.Increment(ref _cacheMisses);
                    }
                    
                    //去执行当前的方法
                    invocation.Proceed();
                    
                    //存入缓存
                    if (!string.IsNullOrWhiteSpace(cacheKey) && invocation.ReturnValue != null)
                    {
                        try
                        {
                            _cache.Set(cacheKey, invocation.ReturnValue, qCachingAttribute.AbsoluteExpiration);
                        }
                        catch (Exception ex)
                        {
                            // 缓存设置错误
                            System.Threading.Interlocked.Increment(ref _cacheErrors);
#if DEBUG
                            System.Diagnostics.Debug.WriteLine($"缓存设置错误: {ex.Message}");
#endif
                        }
                    }
                }
                else
                {
                    invocation.Proceed();//直接执行被拦截方法
                }
            }
            catch (Exception ex)
            {
                // 记录错误
                System.Threading.Interlocked.Increment(ref _cacheErrors);
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"AOP拦截错误: {ex.Message}");
#endif
                
                // 继续执行，确保业务逻辑不受影响
                invocation.Proceed();
            }
            finally
            {
                // 记录执行时间和计数
                var endTime = Stopwatch.GetTimestamp();
                var executionTime = (endTime - startTime) * 1000 / Stopwatch.Frequency;
                System.Threading.Interlocked.Add(ref _totalExecutionTime, executionTime);
                System.Threading.Interlocked.Increment(ref _executionCount);
                
                // 定期输出统计信息（每100次执行）
                if (ExecutionCount % 100 == 0)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"缓存统计: 命中={CacheHits}, 未命中={CacheMisses}, 错误={CacheErrors}, 执行次数={ExecutionCount}, 平均执行时间={AverageExecutionTime:F2}ms");
#endif
                }
            }
        }
    }

}
