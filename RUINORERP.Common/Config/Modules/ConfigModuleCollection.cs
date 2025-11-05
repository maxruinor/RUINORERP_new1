/*************************************************************
 * 文件名：ConfigModuleCollection.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置模块集合
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RUINORERP.Common.Config.Modules
{
    /// <summary>
    /// 配置模块接口
    /// </summary>
    public interface IConfigModule
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 模块版本
        /// </summary>
        Version Version { get; }
        
        /// <summary>
        /// 模块描述
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// 模块优先级（值越小优先级越高）
        /// </summary>
        int Priority { get; }
        
        /// <summary>
        /// 模块依赖
        /// </summary>
        IList<string> Dependencies { get; }
        
        /// <summary>
        /// 是否已初始化
        /// </summary>
        bool IsInitialized { get; }
        
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="context">模块上下文</param>
        void Initialize(IConfigModuleContext context);
        
        /// <summary>
        /// 启动模块
        /// </summary>
        void Start();
        
        /// <summary>
        /// 停止模块
        /// </summary>
        void Stop();
        
        /// <summary>
        /// 清理模块资源
        /// </summary>
        void Dispose();
    }
    
    /// <summary>
    /// 配置模块上下文
    /// </summary>
    public interface IConfigModuleContext
    {
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务实例</returns>
        T GetService<T>();
        
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns>服务实例</returns>
        object GetService(Type serviceType);
        
        /// <summary>
        /// 获取所有已注册的模块
        /// </summary>
        /// <returns>模块列表</returns>
        IReadOnlyList<IConfigModule> GetRegisteredModules();
        
        /// <summary>
        /// 获取模块配置
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns>模块配置</returns>
        IDictionary<string, object> GetModuleConfig(string moduleName);
        
        /// <summary>
        /// 设置模块配置
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="config">模块配置</param>
        void SetModuleConfig(string moduleName, IDictionary<string, object> config);
    }
    
    /// <summary>
    /// 配置模块集合
    /// </summary>
    public class ConfigModuleCollection : IDisposable
    {
        private readonly List<IConfigModule> _modules = new List<IConfigModule>();
        private readonly List<IConfigModule> _initializedModules = new List<IConfigModule>();
        private readonly List<IConfigModule> _startedModules = new List<IConfigModule>();
        private readonly IConfigModuleContext _context;
        private readonly Dictionary<string, IDictionary<string, object>> _moduleConfigs = new Dictionary<string, IDictionary<string, object>>();
        private bool _isDisposed;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">模块上下文</param>
        public ConfigModuleCollection(IConfigModuleContext context = null)
        {
            _context = context ?? new DefaultConfigModuleContext(this);
        }
        
        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="module">配置模块</param>
        /// <returns>模块集合</returns>
        public ConfigModuleCollection RegisterModule(IConfigModule module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            
            // 检查是否已注册
            if (_modules.Any(m => m.Name.Equals(module.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"模块 '{module.Name}' 已注册");
            
            _modules.Add(module);
            return this;
        }
        
        /// <summary>
        /// 注册模块（通过类型）
        /// </summary>
        /// <typeparam name="TModule">模块类型</typeparam>
        /// <returns>模块集合</returns>
        public ConfigModuleCollection RegisterModule<TModule>() where TModule : IConfigModule, new()
        {
            return RegisterModule(new TModule());
        }
        
        /// <summary>
        /// 注册多个模块
        /// </summary>
        /// <param name="modules">模块列表</param>
        /// <returns>模块集合</returns>
        public ConfigModuleCollection RegisterModules(IEnumerable<IConfigModule> modules)
        {
            if (modules == null)
                throw new ArgumentNullException(nameof(modules));
            
            foreach (var module in modules)
            {
                RegisterModule(module);
            }
            
            return this;
        }
        
        /// <summary>
        /// 从程序集加载并注册模块
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns>模块集合</returns>
        public ConfigModuleCollection RegisterModulesFromAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            
            try
            {
                var moduleTypes = assembly.GetTypes()
                    .Where(t => typeof(IConfigModule).IsAssignableFrom(t) && 
                                t.IsClass && !t.IsAbstract && !t.IsInterface);
                
                foreach (var moduleType in moduleTypes)
                {
                    try
                    {
                        var module = (IConfigModule)Activator.CreateInstance(moduleType);
                        RegisterModule(module);
                    }
                    catch (Exception ex)
                    {
                        // 记录错误但继续处理其他模块
                        Console.WriteLine($"加载模块 '{moduleType.Name}' 失败: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"从程序集 '{assembly.FullName}' 加载模块失败", ex);
            }
            
            return this;
        }
        
        /// <summary>
        /// 从当前程序集加载并注册模块
        /// </summary>
        /// <returns>模块集合</returns>
        public ConfigModuleCollection RegisterModulesFromCurrentAssembly()
        {
            return RegisterModulesFromAssembly(Assembly.GetExecutingAssembly());
        }
        
        /// <summary>
        /// 初始化所有模块
        /// </summary>
        public void InitializeAllModules()
        {
            // 按依赖关系排序模块
            var sortedModules = SortModulesByDependencies();
            
            foreach (var module in sortedModules)
            {
                if (!module.IsInitialized)
                {
                    try
                    {
                        module.Initialize(_context);
                        _initializedModules.Add(module);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"初始化模块 '{module.Name}' 失败", ex);
                    }
                }
            }
        }
        
        /// <summary>
        /// 启动所有模块
        /// </summary>
        public void StartAllModules()
        {
            // 确保所有模块已初始化
            InitializeAllModules();
            
            // 按优先级排序模块（优先级高的先启动）
            var sortedModules = _initializedModules.OrderBy(m => m.Priority).ToList();
            
            foreach (var module in sortedModules)
            {
                if (!_startedModules.Contains(module))
                {
                    try
                    {
                        module.Start();
                        _startedModules.Add(module);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"启动模块 '{module.Name}' 失败", ex);
                    }
                }
            }
        }
        /// <summary>
        /// 停止所有模块
        /// </summary>
        public void StopAllModules()
        {
            // 按优先级反向排序模块（优先级低的先停止）
            var sortedModules = _startedModules.OrderByDescending(m => m.Priority).ToList();
            
            foreach (var module in sortedModules)
            {
                try
                {
                    module.Stop();
                }
                catch (Exception ex)
                {
                    // 记录错误但继续处理其他模块
                    Console.WriteLine($"停止模块 '{module.Name}' 失败: {ex.Message}");
                }
                finally
                {
                    _startedModules.Remove(module);
                }
            }
        }
        
        /// <summary>
        /// 按依赖关系排序模块
        /// </summary>
        /// <returns>排序后的模块列表</returns>
        private List<IConfigModule> SortModulesByDependencies()
        {
            var sorted = new List<IConfigModule>();
            var visited = new HashSet<string>();
            var tempVisited = new HashSet<string>();
            
            // 构建模块字典
            var moduleMap = _modules.ToDictionary(m => m.Name, StringComparer.OrdinalIgnoreCase);
            
            // 深度优先搜索拓扑排序
            foreach (var module in _modules)
            {
                if (!visited.Contains(module.Name))
                {
                    Visit(module, moduleMap, visited, tempVisited, sorted);
                }
            }
            
            return sorted;
        }
        
        /// <summary>
        /// 访问模块并构建依赖关系
        /// </summary>
        /// <param name="module">当前模块</param>
        /// <param name="moduleMap">模块映射</param>
        /// <param name="visited">已访问集合</param>
        /// <param name="tempVisited">临时访问集合（用于检测循环依赖）</param>
        /// <param name="sorted">排序结果</param>
        private void Visit(IConfigModule module, Dictionary<string, IConfigModule> moduleMap, 
            HashSet<string> visited, HashSet<string> tempVisited, List<IConfigModule> sorted)
        {
            if (tempVisited.Contains(module.Name))
                throw new InvalidOperationException($"检测到循环依赖: {module.Name}");
            
            if (!visited.Contains(module.Name))
            {
                tempVisited.Add(module.Name);
                
                // 访问所有依赖
                foreach (var dependencyName in module.Dependencies)
                {
                    if (moduleMap.TryGetValue(dependencyName, out var dependencyModule))
                    {
                        Visit(dependencyModule, moduleMap, visited, tempVisited, sorted);
                    }
                    else
                    {
                        throw new InvalidOperationException($"模块 '{module.Name}' 依赖的模块 '{dependencyName}' 未注册");
                    }
                }
                
                tempVisited.Remove(module.Name);
                visited.Add(module.Name);
                sorted.Add(module);
            }
        }
        
        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="TModule">模块类型</typeparam>
        /// <returns>模块实例</returns>
        public TModule GetModule<TModule>() where TModule : IConfigModule
        {
            return _modules.OfType<TModule>().FirstOrDefault();
        }
        
        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns>模块实例</returns>
        public IConfigModule GetModule(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
                throw new ArgumentNullException(nameof(moduleName));
            
            return _modules.FirstOrDefault(m => m.Name.Equals(moduleName, StringComparison.OrdinalIgnoreCase));
        }
        
        /// <summary>
        /// 获取所有模块
        /// </summary>
        /// <returns>模块列表</returns>
        public IReadOnlyList<IConfigModule> GetAllModules()
        {
            return _modules.OrderBy(m => m.Priority).ToList().AsReadOnly();
        }
        
        /// <summary>
        /// 获取已初始化的模块
        /// </summary>
        /// <returns>已初始化的模块列表</returns>
        public IReadOnlyList<IConfigModule> GetInitializedModules()
        {
            return _initializedModules.AsReadOnly();
        }
        
        /// <summary>
        /// 获取已启动的模块
        /// </summary>
        /// <returns>已启动的模块列表</returns>
        public IReadOnlyList<IConfigModule> GetStartedModules()
        {
            return _startedModules.AsReadOnly();
        }
        
        /// <summary>
        /// 设置模块配置
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="config">模块配置</param>
        public void SetModuleConfig(string moduleName, IDictionary<string, object> config)
        {
            if (string.IsNullOrEmpty(moduleName))
                throw new ArgumentNullException(nameof(moduleName));
            
            _moduleConfigs[moduleName] = config ?? new Dictionary<string, object>();
        }
        
        /// <summary>
        /// 获取模块配置
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns>模块配置</returns>
        public IDictionary<string, object> GetModuleConfig(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
                throw new ArgumentNullException(nameof(moduleName));
            
            _moduleConfigs.TryGetValue(moduleName, out var config);
            return config ?? new Dictionary<string, object>();
        }
        
        /// <summary>
        /// 清理资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// 清理资源
        /// </summary>
        /// <param name="disposing">是否手动清理</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // 停止所有模块
                    StopAllModules();
                    
                    // 释放所有模块资源
                    foreach (var module in _initializedModules)
                    {
                        try
                        {
                            module.Dispose();
                        }
                        catch (Exception)
                        {
                            // 忽略异常
                        }
                    }
                    
                    _initializedModules.Clear();
                    _modules.Clear();
                    _moduleConfigs.Clear();
                }
                
                _isDisposed = true;
            }
        }
        
        /// <summary>
        /// 默认配置模块上下文实现
        /// </summary>
        private class DefaultConfigModuleContext : IConfigModuleContext
        {
            private readonly ConfigModuleCollection _collection;
            
            public DefaultConfigModuleContext(ConfigModuleCollection collection)
            {
                _collection = collection;
            }
            
            public T GetService<T>()
            {
                return (T)GetService(typeof(T));
            }
            
            public object GetService(Type serviceType)
            {
                // 简化实现：在实际应用中，应该从依赖注入容器获取服务
                // 这里返回null，表示默认情况下无法解析服务
                return null;
            }
            
            public IReadOnlyList<IConfigModule> GetRegisteredModules()
            {
                return _collection.GetAllModules();
            }
            
            public IDictionary<string, object> GetModuleConfig(string moduleName)
            {
                return _collection.GetModuleConfig(moduleName);
            }
            
            public void SetModuleConfig(string moduleName, IDictionary<string, object> config)
            {
                _collection.SetModuleConfig(moduleName, config);
            }
        }
    }
    
    /// <summary>
    /// 配置模块基类
    /// </summary>
    public abstract class ConfigModuleBase : IConfigModule
    {
        private bool _isInitialized;
        
        /// <summary>
        /// 模块名称
        /// </summary>
        public virtual string Name => GetType().Name.Replace("Module", "");
        
        /// <summary>
        /// 模块版本
        /// </summary>
        public virtual Version Version => new Version(1, 0, 0);
        
        /// <summary>
        /// 模块描述
        /// </summary>
        public virtual string Description => string.Empty;
        
        /// <summary>
        /// 模块优先级
        /// </summary>
        public virtual int Priority => 0;
        
        /// <summary>
        /// 模块依赖
        /// </summary>
        public virtual IList<string> Dependencies { get; } = new List<string>();
        
        /// <summary>
        /// 是否已初始化
        /// </summary>
        public bool IsInitialized => _isInitialized;
        
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="context">模块上下文</param>
        public virtual void Initialize(IConfigModuleContext context)
        {
            _isInitialized = true;
        }
        
        /// <summary>
        /// 启动模块
        /// </summary>
        public virtual void Start()
        {
            // 默认实现为空
        }
        
        /// <summary>
        /// 停止模块
        /// </summary>
        public virtual void Stop()
        {
            // 默认实现为空
        }
        
        /// <summary>
        /// 清理模块资源
        /// </summary>
        public virtual void Dispose()
        {
            // 默认实现为空
        }
        
        /// <summary>
        /// 获取模块配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="context">模块上下文</param>
        /// <returns>模块配置</returns>
        protected T GetModuleConfig<T>(IConfigModuleContext context) where T : new()
        {
            var configDict = context.GetModuleConfig(Name);
            var config = new T();
            
            // 将字典配置映射到对象属性
            foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (configDict.TryGetValue(property.Name, out var value))
                {
                    try
                    {
                        if (value != null && property.PropertyType.IsAssignableFrom(value.GetType()))
                        {
                            property.SetValue(config, value);
                        }
                    }
                    catch (Exception)
                    {
                        // 忽略属性设置错误
                    }
                }
            }
            
            return config;
        }
    }
    
    /// <summary>
    /// 配置模块扩展方法
    /// </summary>
    public static class ConfigModuleExtensions
    {
        /// <summary>
        /// 检查模块是否已注册
        /// </summary>
        /// <param name="collection">模块集合</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>是否已注册</returns>
        public static bool IsModuleRegistered(this ConfigModuleCollection collection, string moduleName)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            
            return collection.GetModule(moduleName) != null;
        }
        
        /// <summary>
        /// 检查模块是否已初始化
        /// </summary>
        /// <param name="collection">模块集合</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>是否已初始化</returns>
        public static bool IsModuleInitialized(this ConfigModuleCollection collection, string moduleName)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            
            var module = collection.GetModule(moduleName);
            return module != null && module.IsInitialized;
        }
        
        /// <summary>
        /// 获取模块信息
        /// </summary>
        /// <param name="module">配置模块</param>
        /// <returns>模块信息字典</returns>
        public static IDictionary<string, object> GetModuleInfo(this IConfigModule module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            
            return new Dictionary<string, object>
            {
                { "Name", module.Name },
                { "Version", module.Version.ToString() },
                { "Description", module.Description },
                { "Priority", module.Priority },
                { "Dependencies", module.Dependencies },
                { "IsInitialized", module.IsInitialized }
            };
        }
        
        /// <summary>
        /// 检查模块依赖是否都已注册
        /// </summary>
        /// <param name="module">配置模块</param>
        /// <param name="collection">模块集合</param>
        /// <returns>是否所有依赖都已注册</returns>
        public static bool AreDependenciesRegistered(this IConfigModule module, ConfigModuleCollection collection)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            
            foreach (var dependency in module.Dependencies)
            {
                if (!collection.IsModuleRegistered(dependency))
                    return false;
            }
            
            return true;
        }
    }
}