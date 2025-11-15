using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.LogHelper;
using RUINORERP.Model;
using RUINORERP.Model.Base;

namespace RUINORERP.Business.Document
{
    /// <summary>
    /// 转换器优先级特性
    /// 用于为转换器指定优先级，优先级越高的转换器会被优先选择
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ConverterPriorityAttribute : Attribute
    {
        /// <summary>
        /// 优先级值，默认为100
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="priority">优先级值</param>
        public ConverterPriorityAttribute(int priority = 100)
        {
            Priority = priority;
        }
    }

    /// <summary>
    /// 单据转换器工厂
    /// 负责管理和创建各种单据转换器实例
    /// </summary>
    public class DocumentConverterFactory
    {
        /// <summary>
        /// 转换器实例缓存
        /// Key: 转换器类型键
        /// Value: 转换器实例列表（按优先级排序）
        /// </summary>
        private readonly Dictionary<string, List<object>> _convertersCache = new Dictionary<string, List<object>>();
        
        /// <summary>
        /// 所有注册的转换器信息
        /// </summary>
        private readonly List<ConverterInfo> _allConverters = new List<ConverterInfo>();
        
        /// <summary>
        /// 是否已初始化
        /// </summary>
        private bool _initialized = false;
        
        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<DocumentConverterFactory> _logger;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public DocumentConverterFactory(ILogger<DocumentConverterFactory> logger = null)
        {
            _logger = logger;
            // 初始化缓存
            _convertersCache = new Dictionary<string, List<object>>();
            // 自动发现并注册所有转换器
            AutoDiscoverAndRegister();
        }

        /// <summary>
        /// 自动发现并注册所有转换器
        /// </summary>
        private void AutoDiscoverAndRegister()
        {
            try
            {
                _logger?.LogInformation("开始自动发现转换器...");
                
                // 获取当前应用程序域中的所有程序集
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                
                foreach (var assembly in assemblies)
                {
                    try
                    {
                        // 获取程序集中所有实现了IDocumentConverter接口的类型
                        var converterTypes = assembly.GetTypes()
                            .Where(t => !t.IsAbstract && !t.IsInterface && 
                                       t.GetInterfaces().Any(i => 
                                           i.IsGenericType && 
                                           i.GetGenericTypeDefinition() == typeof(IDocumentConverter<,>)));

                        foreach (var converterType in converterTypes)
                        {
                            try
                            {
                                // 获取接口的泛型参数
                                var interfaceType = converterType.GetInterfaces()
                                    .First(i => i.IsGenericType && 
                                               i.GetGenericTypeDefinition() == typeof(IDocumentConverter<,>));

                                var sourceType = interfaceType.GetGenericArguments()[0];
                                var targetType = interfaceType.GetGenericArguments()[1];
                                
                                // 获取优先级
                                var priorityAttribute = converterType.GetCustomAttribute<ConverterPriorityAttribute>();
                                int priority = priorityAttribute?.Priority ?? 100;
                                
                                // 创建注册方法的委托
                                var registerMethod = typeof(DocumentConverterFactory)
                                    .GetMethod("RegisterConverterInternal", BindingFlags.NonPublic | BindingFlags.Instance)
                                    .MakeGenericMethod(sourceType, targetType);
                                
                                // 创建转换器实例
                                var converter = Activator.CreateInstance(converterType);
                                
                                // 注册转换器
                                registerMethod.Invoke(this, new[] { converter, priority });
                                
                                _logger?.LogInformation($"成功注册转换器: {converterType.FullName}, 优先级: {priority}");
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogError(ex, $"注册转换器 {converterType.FullName} 失败");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, $"扫描程序集 {assembly.FullName} 时出错");
                    }
                }
                
                _initialized = true;
                _logger?.LogInformation($"转换器自动发现完成，共注册 {_allConverters.Count} 个转换器");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "自动发现转换器过程中发生错误");
                throw;
            }
        }

        /// <summary>
        /// 内部注册转换器方法
        /// </summary>
        private void RegisterConverterInternal<TSource, TTarget>(IDocumentConverter<TSource, TTarget> converter, int priority)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }
            
            string key = GetConverterKey<TSource, TTarget>();
            
            // 添加到缓存
            if (!_convertersCache.ContainsKey(key))
            {
                _convertersCache[key] = new List<object>();
            }
            
            // 查找插入位置，保持按优先级排序（优先级高的在前）
            int insertIndex = 0;
            while (insertIndex < _convertersCache[key].Count)
            {
                var existingConverter = _convertersCache[key][insertIndex];
                var existingPriority = GetConverterPriority(existingConverter.GetType());
                
                if (priority > existingPriority)
                {
                    break;
                }
                
                insertIndex++;
            }
            
            _convertersCache[key].Insert(insertIndex, converter);
            
            // 添加到所有转换器列表
            _allConverters.Add(new ConverterInfo
            {
                SourceType = typeof(TSource),
                TargetType = typeof(TTarget),
                Converter = converter,
                Priority = priority
            });
        }

        /// <summary>
        /// 获取转换器优先级
        /// </summary>
        private int GetConverterPriority(Type converterType)
        {
            var attribute = converterType.GetCustomAttribute<ConverterPriorityAttribute>();
            return attribute?.Priority ?? 100;
        }

        /// <summary>
        /// 注册转换器（手动注册）
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <typeparam name="TTarget">目标单据类型</typeparam>
        /// <param name="converter">转换器实例</param>
        /// <param name="priority">优先级</param>
        public void Register<TSource, TTarget>(IDocumentConverter<TSource, TTarget> converter, int priority = 100)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            RegisterConverterInternal(converter, priority);
        }

        /// <summary>
        /// 泛型注册转换器方法
        /// 根据指定的转换器类型创建实例并注册
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <typeparam name="TTarget">目标单据类型</typeparam>
        /// <typeparam name="TConverter">转换器类型</typeparam>
        /// <param name="priority">优先级</param>
        public void Register<TSource, TTarget, TConverter>(int priority = 100)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
            where TConverter : IDocumentConverter<TSource, TTarget>, new()
        {
            // 创建转换器实例
            var converter = new TConverter();
            // 调用已有的Register方法注册
            Register(converter, priority);
        }

        /// <summary>
        /// 获取转换器
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <typeparam name="TTarget">目标单据类型</typeparam>
        /// <returns>转换器实例（优先级最高的）</returns>
        public IDocumentConverter<TSource, TTarget> GetConverter<TSource, TTarget>()
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            string key = GetConverterKey<TSource, TTarget>();
            
            if (_convertersCache.TryGetValue(key, out var convertersList) && convertersList.Count > 0)
            {
                // 返回优先级最高的转换器（列表第一个）
                return (IDocumentConverter<TSource, TTarget>)convertersList[0];
            }
            
            throw new KeyNotFoundException($"找不到从{typeof(TSource).Name}到{typeof(TTarget).Name}的转换器");
        }

        /// <summary>
        /// 获取指定源单据可用的所有转换目标
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <returns>可用的转换选项列表（按优先级排序）</returns>
        public List<ConversionOption> GetAvailableConversions<TSource>()
            where TSource : BaseEntity
        {
            var result = new List<ConversionOption>();
            
            // 从所有转换器列表中筛选出源类型匹配的转换器
            var matchingConverters = _allConverters
                .Where(info => info.SourceType == typeof(TSource))
                .OrderByDescending(info => info.Priority) // 按优先级排序
                .ToList();
            
            foreach (var info in matchingConverters)
            {
                // 使用更安全的类型转换
                if (info.Converter is IDocumentConverter<TSource, BaseEntity> converter)
                {
                    result.Add(new ConversionOption
                    {
                        DisplayName = converter.DisplayName,
                        SourceDocumentType = converter.SourceDocumentType,
                        TargetDocumentType = converter.TargetDocumentType,
                        ConverterType = info.Converter.GetType(),
                        Priority = info.Priority
                    });
                }
            }
            
            return result;
        }

        /// <summary>
        /// 执行转换
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <typeparam name="TTarget">目标单据类型</typeparam>
        /// <param name="source">源单据对象</param>
        /// <param name="conversionContext">转换上下文（可选）</param>
        /// <returns>转换后的目标单据对象</returns>
        public async Task<TTarget> ConvertAsync<TSource, TTarget>(TSource source, object conversionContext = null)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            _logger?.LogDebug($"执行转换: {typeof(TSource).Name} -> {typeof(TTarget).Name}");
            
            // 检查是否有多个转换器可用
            string key = GetConverterKey<TSource, TTarget>();
            
            if (_convertersCache.TryGetValue(key, out var convertersList))
            {
                foreach (var converterObj in convertersList)
                {
                    try
                    {
                        var converter = (IDocumentConverter<TSource, TTarget>)converterObj;
                        
                        // 先验证是否可以转换
                        var validationResult = await converter.ValidateConversionAsync(source);
                        if (validationResult.CanConvert)
                        {
                            // 执行转换
                            var result = await converter.ConvertAsync(source);
                            _logger?.LogInformation($"转换成功: {typeof(TSource).Name} -> {typeof(TTarget).Name}");
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, $"转换器 {converterObj.GetType().Name} 执行失败，尝试下一个转换器");
                    }
                }
            }
            
            // 如果没有可用的转换器或所有转换器都失败了，使用默认转换器
            var defaultConverter = GetConverter<TSource, TTarget>();
            _logger?.LogDebug("使用默认转换器执行转换");
            return await defaultConverter.ConvertAsync(source);
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <typeparam name="TTarget">目标单据类型</typeparam>
        /// <param name="source">源单据对象</param>
        /// <returns>验证结果</returns>
        public async Task<ValidationResult> ValidateConversionAsync<TSource, TTarget>(TSource source)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            var converter = GetConverter<TSource, TTarget>();
            return await converter.ValidateConversionAsync(source);
        }

        /// <summary>
        /// 刷新转换器缓存
        /// 重新扫描并注册所有转换器
        /// </summary>
        public void RefreshConverters()
        {
            _logger?.LogInformation("刷新转换器缓存");
            
            // 清空缓存
            _convertersCache.Clear();
            _allConverters.Clear();
            _initialized = false;
            
            // 重新发现并注册
            AutoDiscoverAndRegister();
        }

        /// <summary>
        /// 生成转换器键
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <typeparam name="TTarget">目标单据类型</typeparam>
        /// <returns>转换器键</returns>
        private string GetConverterKey<TSource, TTarget>()
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            return $"{typeof(TSource).FullName}:{typeof(TTarget).FullName}";
        }
        
        /// <summary>
        /// 获取源单据类型
        /// </summary>
        /// <param name="sourceTypeFullName">源单据类型的全名</param>
        /// <returns>源单据类型</returns>
        public Type GetSourceType(string sourceTypeFullName)
        {
            return Type.GetType(sourceTypeFullName);
        }
        
        /// <summary>
        /// 获取目标单据类型
        /// </summary>
        /// <param name="targetTypeFullName">目标单据类型的全名</param>
        /// <returns>目标单据类型</returns>
        public Type GetTargetType(string targetTypeFullName)
        {
            return Type.GetType(targetTypeFullName);
        }
        
        /// <summary>
        /// 转换器信息内部类
        /// </summary>
        private class ConverterInfo
        {
            public Type SourceType { get; set; }
            public Type TargetType { get; set; }
            public object Converter { get; set; }
            public int Priority { get; set; }
        }
    }
    
    /// <summary>
    /// 转换选项类
    /// 表示可用的单据转换选项
    /// </summary>
    public class ConversionOption
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// 源单据类型名称
        /// </summary>
        public string SourceDocumentType { get; set; }
        
        /// <summary>
        /// 目标单据类型名称
        /// </summary>
        public string TargetDocumentType { get; set; }
        
        /// <summary>
        /// 转换器类型
        /// </summary>
        public Type ConverterType { get; set; }
        
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }
    }
    
}