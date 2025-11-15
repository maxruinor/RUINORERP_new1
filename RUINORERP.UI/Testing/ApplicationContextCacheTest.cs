using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RUINORERP.Model.Context;
using RUINORERP.IServices;
using RUINORERP.IRepository.Base;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;

namespace RUINORERP.UI.Testing
{
    /// <summary>
    /// ApplicationContext服务缓存性能测试工具
    /// </summary>
    public class ApplicationContextCacheTest
    {
        /// <summary>
        /// 运行ApplicationContext缓存性能测试
        /// </summary>
        public static void RunApplicationContextCacheTest()
        {
            Console.WriteLine("=== ApplicationContext服务缓存性能测试 ===");
            
            // 检查ApplicationContext是否可用
            if (ApplicationContext.Current == null)
            {
                Console.WriteLine("错误: ApplicationContext.Current 为空，无法进行测试");
                return;
            }
            
            // 测试参数
            int iterations = 1000;
            string serviceTypeName = "RUINORERP.IServices.IEntityCacheManager";
            
            Console.WriteLine($"测试参数: 迭代次数={iterations}, 服务类型={serviceTypeName}");
            Console.WriteLine();
            
            // 获取服务类型
            Type serviceType = Type.GetType(serviceTypeName);
            if (serviceType == null)
            {
                Console.WriteLine($"错误: 无法找到类型 {serviceTypeName}");
                return;
            }
            
            // 第一次测试 - 冷启动（缓存为空）
            Console.WriteLine("第一次测试 - 冷启动（缓存为空）:");
            TestApplicationContextPerformance(serviceType, iterations, "冷启动");
            
            // 显示缓存统计
            var stats = ApplicationContext.Current.GetServiceCacheStatistics();
            Console.WriteLine($"缓存统计: {stats}");
            Console.WriteLine();
            
            // 第二次测试 - 缓存已预热
            Console.WriteLine("第二次测试 - 缓存已预热:");
            TestApplicationContextPerformance(serviceType, iterations, "缓存已预热");
            
            // 显示最终缓存统计
            stats = ApplicationContext.Current.GetServiceCacheStatistics();
            Console.WriteLine($"最终缓存统计: {stats}");
            Console.WriteLine();
            
            // 测试多种服务类型的缓存效果
            TestMultipleServiceTypes();
            
            // 测试缓存容量限制
            TestCacheCapacity();
        }
        
        /// <summary>
        /// 测试ApplicationContext获取服务的性能
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="iterations">迭代次数</param>
        /// <param name="testName">测试名称</param>
        private static void TestApplicationContextPerformance(Type serviceType, int iterations, string testName)
        {
            // 记录开始时间
            Stopwatch stopwatch = Stopwatch.StartNew();
            
            // 使用ApplicationContext获取服务实例多次
            for (int i = 0; i < iterations; i++)
            {
                var result = ApplicationContext.Current.GetRequiredService(serviceType);
                
                if (result == null)
                {
                    Console.WriteLine($"警告: 第 {i} 次获取服务失败");
                }
            }
            
            // 记录结束时间
            stopwatch.Stop();
            
            // 显示结果
            Console.WriteLine($"{testName}完成 {iterations} 次调用，耗时: {stopwatch.Elapsed.TotalMilliseconds:F2} 毫秒");
            Console.WriteLine($"{testName}平均每次调用耗时: {(stopwatch.Elapsed.TotalMilliseconds / iterations):F4} 毫秒");
        }
        
        /// <summary>
        /// 测试多种服务类型的缓存效果
        /// </summary>
        private static void TestMultipleServiceTypes()
        {
            Console.WriteLine("=== 多种服务类型缓存测试 ===");
            
            // 清空缓存
            ApplicationContext.Current.ClearServiceCache();
            
            // 定义要测试的服务类型
            var serviceTypes = new List<Type>
            {
                typeof(IEntityCacheManager),
                typeof(IBaseRepository<>),
                typeof(ILogger)
            };
            
            // 测试每种服务类型
            foreach (var serviceType in serviceTypes)
            {
                Console.WriteLine($"测试服务类型: {serviceType.Name}");
                
                // 第一次获取（缓存未命中）
                var stopwatch = Stopwatch.StartNew();
                var service1 = ApplicationContext.Current.GetRequiredService(serviceType);
                stopwatch.Stop();
                Console.WriteLine($"  第一次获取耗时: {stopwatch.Elapsed.TotalMilliseconds:F4} 毫秒");
                
                // 第二次获取（缓存命中）
                stopwatch.Restart();
                var service2 = ApplicationContext.Current.GetRequiredService(serviceType);
                stopwatch.Stop();
                Console.WriteLine($"  第二次获取耗时: {stopwatch.Elapsed.TotalMilliseconds:F4} 毫秒");
                
                // 验证是否是同一个实例
                bool isSameInstance = ReferenceEquals(service1, service2);
                Console.WriteLine($"  是否为同一实例: {(isSameInstance ? "是" : "否")}");
                Console.WriteLine();
            }
            
            // 显示缓存统计
            var stats = ApplicationContext.Current.GetServiceCacheStatistics();
            Console.WriteLine($"缓存统计: {stats}");
            Console.WriteLine();
        }
        
        /// <summary>
        /// 测试缓存容量限制
        /// </summary>
        private static void TestCacheCapacity()
        {
            Console.WriteLine("=== 缓存容量限制测试 ===");
            
            // 清空缓存
            ApplicationContext.Current.ClearServiceCache();
            
            // 获取最大缓存大小
            var stats = ApplicationContext.Current.GetServiceCacheStatistics();
            int maxCacheSize = stats.MaxCacheSize;
            Console.WriteLine($"最大缓存大小: {maxCacheSize}");
            
            // 添加超过最大缓存大小的服务实例
            Console.WriteLine("添加超过最大缓存大小的服务实例...");
            for (int i = 0; i < maxCacheSize + 5; i++)
            {
                // 创建一个临时的服务类型
                var serviceType = typeof(ILogger);
                ApplicationContext.Current.GetRequiredService(serviceType);
                
                // 每添加10个实例显示一次缓存大小
                if ((i + 1) % 10 == 0)
                {
                    var currentStats = ApplicationContext.Current.GetServiceCacheStatistics();
                    Console.WriteLine($"  添加 {i + 1} 个实例后，缓存大小: {currentStats.CacheSize}");
                }
            }
            
            // 显示最终缓存统计
            var finalStats = ApplicationContext.Current.GetServiceCacheStatistics();
            Console.WriteLine($"最终缓存统计: {finalStats}");
            Console.WriteLine();
        }
    }
}