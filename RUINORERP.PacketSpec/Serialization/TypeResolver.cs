using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 高性能类型解析器 - 支持类型名称到类型的快速解析和缓存
    /// </summary>
    public static class TypeResolver
    {
        #region 缓存存储

        // 线程安全的类型缓存
        private static readonly ConcurrentDictionary<string, Type> _typeCache = new ConcurrentDictionary<string, Type>();

        // 程序集缓存，避免重复加载
        private static readonly ConcurrentDictionary<string, Assembly> _assemblyCache = new ConcurrentDictionary<string, Assembly>();

        // 类型别名映射（可选）
        private static readonly Dictionary<string, string> _typeAliases = new Dictionary<string, string>
        {
            { "string", typeof(string).AssemblyQualifiedName },
            { "int", typeof(int).AssemblyQualifiedName },
            { "long", typeof(long).AssemblyQualifiedName },
            { "double", typeof(double).AssemblyQualifiedName },
            { "decimal", typeof(decimal).AssemblyQualifiedName },
            { "DateTime", typeof(DateTime).AssemblyQualifiedName },
            { "bool", typeof(bool).AssemblyQualifiedName },
            { "object", typeof(object).AssemblyQualifiedName }
        };

        #endregion

        #region 公共方法

        /// <summary>
        /// 根据类型名称获取 Type 对象（高性能缓存版本）
        /// </summary>
        /// <param name="typeName">类型名称（支持简单名称、完整名称、程序集限定名称）</param>
        /// <returns>对应的 Type 对象，如果找不到返回 null</returns>
        public static Type GetType(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return null;

            // 检查缓存
            if (_typeCache.TryGetValue(typeName, out var cachedType))
                return cachedType;

            // 处理类型别名
            if (_typeAliases.TryGetValue(typeName, out var aliasedName))
            {
                return GetType(aliasedName); // 递归调用，但会走缓存
            }

            Type type = null;

            // 尝试不同的解析策略
            type = type ?? ResolveByExactMatch(typeName);
            type = type ?? ResolveByAssemblyQualifiedName(typeName);
            type = type ?? ResolveByFullName(typeName);
            type = type ?? ResolveBySimpleName(typeName);
            type = type ?? ResolveByLoadingAssembly(typeName);

            // 如果找到类型，添加到缓存
            if (type != null)
            {
                _typeCache[typeName] = type;

                // 同时缓存其他可能的键名
                CacheAlternativeKeys(type, typeName);
            }

            return type;
        }

        /// <summary>
        /// 创建类型的实例
        /// </summary>
        public static object CreateInstance(string typeName)
        {
            var type = GetType(typeName);
            return type != null ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// 创建泛型类型的实例
        /// </summary>
        public static object CreateInstance(string typeName, params Type[] genericTypeArguments)
        {
            var type = GetType(typeName);
            if (type == null || !type.IsGenericTypeDefinition)
                return null;

            try
            {
                var genericType = type.MakeGenericType(genericTypeArguments);
                return Activator.CreateInstance(genericType);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 预注册常用类型到缓存
        /// </summary>
        public static void PreRegisterCommonTypes()
        {
            var commonTypes = new[]
            {
                // 基础类型
                typeof(string), typeof(int), typeof(long), typeof(double), typeof(decimal),
                typeof(DateTime), typeof(bool), typeof(Guid), typeof(TimeSpan),
                
                // 集合类型
                typeof(List<>), typeof(Dictionary<,>), typeof(HashSet<>), typeof(Queue<>), typeof(Stack<>),
                typeof(ConcurrentDictionary<,>), typeof(ConcurrentBag<>), typeof(ConcurrentQueue<>),
                
                // 系统类型
                typeof(object), typeof(ValueType), typeof(Enum), typeof(Array),
                
                // Nullable 类型
                typeof(Nullable<>)
            };

            foreach (var type in commonTypes)
            {
                RegisterType(type);
            }
        }

        /// <summary>
        /// 手动注册类型到缓存
        /// </summary>
        public static void RegisterType(Type type)
        {
            if (type == null) return;

            var keys = new[]
            {
                type.Name,                          // 简单名称
                type.FullName,                      // 完整名称  
                type.AssemblyQualifiedName,         // 程序集限定名称
                $"{type.FullName}, {type.Assembly.GetName().Name}" // 简化的程序集限定名称
            };

            foreach (var key in keys.Where(k => !string.IsNullOrEmpty(k)))
            {
                _typeCache[key] = type;
            }
        }

        /// <summary>
        /// 手动注册类型到缓存（泛型版本）
        /// </summary>
        public static void RegisterType<T>()
        {
            RegisterType(typeof(T));
        }

        /// <summary>
        /// 清除缓存（用于测试或重新加载）
        /// </summary>
        public static void ClearCache()
        {
            _typeCache.Clear();
            _assemblyCache.Clear();
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        public static (int TypeCount, int AssemblyCount) GetCacheStats()
        {
            return (_typeCache.Count, _assemblyCache.Count);
        }

        #endregion

        #region 私有解析方法

        /// <summary>
        /// 精确匹配解析
        /// </summary>
        private static Type ResolveByExactMatch(string typeName)
        {
            // 首先尝试 Type.GetType（最快的方式）
            var type = Type.GetType(typeName, false);
            if (type != null) return type;

            return null;
        }

        /// <summary>
        /// 通过程序集限定名称解析
        /// </summary>
        private static Type ResolveByAssemblyQualifiedName(string typeName)
        {
            try
            {
                // 如果已经是程序集限定名称，使用 Assembly.Load 和 Type.GetType
                if (typeName.Contains(","))
                {
                    var type = Type.GetType(typeName,
                        assemblyName =>
                        {
                            var assembly = LoadAssembly(assemblyName.FullName);
                            return assembly;
                        },
                        null,
                        false);

                    return type;
                }
            }
            catch
            {
                // 忽略解析错误
            }

            return null;
        }

        /// <summary>
        /// 通过完整名称解析
        /// </summary>
        private static Type ResolveByFullName(string typeName)
        {
            // 在所有已加载程序集中搜索
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var type = assembly.GetType(typeName);
                    if (type != null) return type;
                }
                catch
                {
                    // 忽略单个程序集的错误
                }
            }

            return null;
        }

        /// <summary>
        /// 通过简单名称解析
        /// </summary>
        private static Type ResolveBySimpleName(string typeName)
        {
            // 如果以上方法都失败，尝试按简单名称搜索
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var type = assembly.GetTypes()
                        .FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));

                    if (type != null) return type;
                }
                catch (ReflectionTypeLoadException)
                {
                    // 忽略无法加载类型的程序集
                }
            }

            return null;
        }

        /// <summary>
        /// 通过加载程序集解析
        /// </summary>
        private static Type ResolveByLoadingAssembly(string typeName)
        {
            try
            {
                // 尝试提取程序集名称并加载
                var assemblyName = ExtractAssemblyName(typeName);
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    var assembly = LoadAssembly(assemblyName);
                    if (assembly != null)
                    {
                        var typeNameOnly = ExtractTypeName(typeName);
                        return assembly.GetType(typeNameOnly);
                    }
                }
            }
            catch
            {
                // 忽略加载失败
            }

            return null;
        }

        /// <summary>
        /// 加载程序集（带缓存）
        /// </summary>
        private static Assembly LoadAssembly(string assemblyName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
                return null;

            return _assemblyCache.GetOrAdd(assemblyName, name =>
            {
                try
                {
                    // 首先尝试从已加载程序集中查找
                    var loadedAssembly = AppDomain.CurrentDomain.GetAssemblies()
                        .FirstOrDefault(a => a.FullName == name || a.GetName().Name == name);

                    if (loadedAssembly != null) return loadedAssembly;

                    // 尝试加载程序集
                    return Assembly.Load(name);
                }
                catch
                {
                    return null;
                }
            });
        }

        /// <summary>
        /// 从完整类型名称中提取程序集名称
        /// </summary>
        private static string ExtractAssemblyName(string fullTypeName)
        {
            var parts = fullTypeName.Split(',');
            return parts.Length > 1 ? parts[1].Trim() : null;
        }

        /// <summary>
        /// 从完整类型名称中提取类型名称
        /// </summary>
        private static string ExtractTypeName(string fullTypeName)
        {
            var parts = fullTypeName.Split(',');
            return parts[0].Trim();
        }

        /// <summary>
        /// 缓存类型的其他可能键名
        /// </summary>
        private static void CacheAlternativeKeys(Type type, string originalKey)
        {
            if (type == null) return;

            var alternativeKeys = new[]
            {
                type.Name,
                type.FullName,
                type.AssemblyQualifiedName
            };

            foreach (var key in alternativeKeys)
            {
                if (!string.IsNullOrEmpty(key) && key != originalKey)
                {
                    _typeCache.TryAdd(key, type);
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 类型解析器的扩展方法
    /// </summary>
    public static class TypeResolverExtensions
    {
        /// <summary>
        /// 批量注册类型
        /// </summary>
        public static void RegisterTypes(this IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                TypeResolver.RegisterType(type);
            }
        }

        /// <summary>
        /// 从程序集注册所有公开类型
        /// </summary>
        public static void RegisterAllTypesFromAssembly(this Assembly assembly)
        {
            try
            {
                var types = assembly.GetExportedTypes();
                types.RegisterTypes();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从程序集注册类型失败: {assembly.FullName}, 错误: {ex.Message}");
            }
        }
    }
}

