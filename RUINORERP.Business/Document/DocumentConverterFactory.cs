using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.LogHelper;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using Microsoft.Extensions.DependencyInjection;

namespace RUINORERP.Business.Document
{
    /// <summary>
    /// 单据转换器工厂
    /// 负责管理和创建各种单据转换器实例
    /// </summary>
    public class DocumentConverterFactory
    {
        /// <summary>
        /// 转换器实例缓存
        /// Key: 转换器类型键
        /// Value: 转换器实例
        /// </summary>
        private readonly Dictionary<string, object> _convertersCache = new Dictionary<string, object>();
        
        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<DocumentConverterFactory> _logger;
        
        /// <summary>
        /// 服务提供者，用于创建转换器实例
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider">服务提供者，用于创建转换器实例</param>
        /// <param name="logger">日志记录器</param>
        public DocumentConverterFactory(IServiceProvider serviceProvider, ILogger<DocumentConverterFactory> logger = null)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger;
            // 初始化缓存
            _convertersCache = new Dictionary<string, object>();
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
                                
                                // 创建注册方法的委托
                                var registerMethod = typeof(DocumentConverterFactory)
                                    .GetMethod("RegisterConverterInternal", BindingFlags.NonPublic | BindingFlags.Instance)
                                    .MakeGenericMethod(sourceType, targetType);
                                
                                // 使用服务提供者创建转换器实例，而不是使用Activator.CreateInstance
                                var converter = _serviceProvider.GetRequiredService(converterType);
                                
                                // 注册转换器
                                registerMethod.Invoke(this, new[] { converter });
                                
                                _logger?.LogInformation($"成功注册转换器: {converterType.FullName}");
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
                
                _logger?.LogInformation($"转换器自动发现完成，共注册 {_convertersCache.Count} 个转换器");
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
        private void RegisterConverterInternal<TSource, TTarget>(IDocumentConverter<TSource, TTarget> converter)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }
            
            string key = GetConverterKey<TSource, TTarget>();
            
            // 添加到缓存
            _convertersCache[key] = converter;
        }

        /// <summary>
        /// 注册转换器
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <typeparam name="TTarget">目标单据类型</typeparam>
        /// <param name="converter">转换器实例</param>
        public void Register<TSource, TTarget>(IDocumentConverter<TSource, TTarget> converter)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            RegisterConverterInternal(converter);
        }

        /// <summary>
        /// 获取转换器
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <typeparam name="TTarget">目标单据类型</typeparam>
        /// <returns>转换器实例</returns>
        public IDocumentConverter<TSource, TTarget> GetConverter<TSource, TTarget>()
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            string key = GetConverterKey<TSource, TTarget>();
            
            if (_convertersCache.TryGetValue(key, out var converter))
            {
                return converter as IDocumentConverter<TSource, TTarget>;
            }
            
            return null;
        }

        /// <summary>
        /// 获取实体的显示名称（从Description特性获取）
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>实体显示名称，如果未找到Description特性则返回类型名称</returns>
        private string GetEntityDisplayName(Type entityType)
        {
            try
            {
                if (entityType == null)
                {
                    _logger?.LogWarning("实体类型为空");
                    return "未知实体";
                }

                // 获取Description特性
                var descriptionAttr = entityType.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
                if (descriptionAttr != null && !string.IsNullOrEmpty(descriptionAttr.Description))
                {
                    return descriptionAttr.Description;
                }

                // 如果没有Description特性或Description为空，返回类型名称
                _logger?.LogDebug("实体 {EntityType} 未找到Description特性或Description为空，使用类型名称", entityType.Name);
                return entityType.Name;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "获取实体 {EntityType} 显示名称失败，使用类型名称", entityType.Name);
                return entityType.Name;
            }
        }

        /// <summary>
        /// 获取可用的转换选项
        /// </summary>
        /// <param name="sourceEntity">源实体</param>
        /// <returns>转换选项列表</returns>
        public List<ConversionOption> GetAvailableConversions(BaseEntity sourceEntity)
        {
            if (sourceEntity == null)
            {
                throw new ArgumentNullException(nameof(sourceEntity));
            }

            var options = new List<ConversionOption>();
            var sourceType = sourceEntity.GetType();

            // 查找所有以该类型为源类型的转换器
            foreach (var kvp in _convertersCache)
            {
                // 解析键值
                var keyParts = kvp.Key.Split(':');
                if (keyParts.Length == 2 && keyParts[0] == sourceType.FullName)
                {
                    var converter = kvp.Value;
                    var converterType = converter.GetType();
                    
                    // 获取目标类型
                    var targetType = GetTargetTypeFromConverter(converterType);
                    
                    if (targetType != null)
                    {
                        // 获取源和目标实体的显示名称
                        var sourceDisplayName = GetEntityDisplayName(sourceType);
                        var targetDisplayName = GetEntityDisplayName(targetType);
                        
                        options.Add(new ConversionOption
                        {
                            SourceDocumentType = sourceType.Name,
                            TargetDocumentType = targetType.Name,
                            SourceDocumentDisplayName = sourceDisplayName,
                            TargetDocumentDisplayName = targetDisplayName,
                            ConverterType = converterType,
                            DisplayName = $"转换为{targetDisplayName}"
                        });
                    }
                }
            }

            return options;
        }

        /// <summary>
        /// 获取可用的转换选项（泛型版本）
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <returns>转换选项列表</returns>
        public List<ConversionOption> GetAvailableConversions<TSource>() where TSource : BaseEntity
        {
            var options = new List<ConversionOption>();
            var sourceType = typeof(TSource);

            // 查找所有以该类型为源类型的转换器
            foreach (var kvp in _convertersCache)
            {
                // 解析键值
                var keyParts = kvp.Key.Split(':');
                if (keyParts.Length == 2 && keyParts[0] == sourceType.FullName)
                {
                    var converter = kvp.Value;
                    var converterType = converter.GetType();
                    
                    // 获取目标类型
                    var targetType = GetTargetTypeFromConverter(converterType);
                    
                    if (targetType != null)
                    {
                        // 获取源和目标实体的显示名称
                        var sourceDisplayName = GetEntityDisplayName(sourceType);
                        var targetDisplayName = GetEntityDisplayName(targetType);
                        
                        options.Add(new ConversionOption
                        {
                            SourceDocumentType = sourceType.Name,
                            TargetDocumentType = targetType.Name,
                            SourceDocumentDisplayName = sourceDisplayName,
                            TargetDocumentDisplayName = targetDisplayName,
                            ConverterType = converterType,
                            DisplayName = $"转换为{targetDisplayName}"
                        });
                    }
                }
            }

            return options;
        }

        /// <summary>
        /// 从转换器类型获取目标类型
        /// </summary>
        private Type GetTargetTypeFromConverter(Type converterType)
        {
            var interfaceType = converterType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && 
                                   i.GetGenericTypeDefinition() == typeof(IDocumentConverter<,>));
            
            return interfaceType?.GetGenericArguments()[1];
        }
        
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <typeparam name="TTarget">目标单据类型</typeparam>
        /// <param name="source">源单据对象</param>
        /// <returns>转换后的目标单据对象</returns>
        public async Task<TTarget> ConvertAsync<TSource, TTarget>(TSource source)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // 获取转换器
            var converter = GetConverter<TSource, TTarget>();
            if (converter == null)
            {
                throw new InvalidOperationException($"未找到从 {typeof(TSource).Name} 到 {typeof(TTarget).Name} 的转换器");
            }

            try
            {
                // 执行转换
                var result = await converter.ConvertAsync(source);
                _logger?.LogInformation($"转换成功: {typeof(TSource).Name} -> {typeof(TTarget).Name}");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"转换过程中发生错误: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 验证转换是否可行
        /// </summary>
        /// <typeparam name="TSource">源单据类型</typeparam>
        /// <typeparam name="TTarget">目标单据类型</typeparam>
        /// <param name="source">源单据对象</param>
        /// <returns>验证结果</returns>
        public async Task<ValidationResult> ValidateConversionAsync<TSource, TTarget>(TSource source)
            where TSource : BaseEntity
            where TTarget : BaseEntity, new()
        {
            if (source == null)
            {
                return ValidationResult.Fail("源单据对象不能为空");
            }

            var converter = GetConverter<TSource, TTarget>();
            if (converter == null)
            {
                return ValidationResult.Fail($"未找到从 {typeof(TSource).Name} 到 {typeof(TTarget).Name} 的转换器");
            }

            try
            {
                return await converter.ValidateConversionAsync(source);
            }
            catch (Exception ex)
            {
                return ValidationResult.Fail($"验证过程中发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 刷新转换器缓存
        /// </summary>
        public void RefreshConverters()
        {
            _convertersCache.Clear();
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
        /// 源单据显示名称（从Description特性获取）
        /// </summary>
        public string SourceDocumentDisplayName { get; set; }
        
        /// <summary>
        /// 目标单据显示名称（从Description特性获取）
        /// </summary>
        public string TargetDocumentDisplayName { get; set; }
        
        /// <summary>
        /// 转换器类型
        /// </summary>
        public Type ConverterType { get; set; }
    }
    
}