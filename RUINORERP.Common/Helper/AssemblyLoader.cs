using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RUINORERP.Common.Helper
{
    /// <summary>
    /// 程序集加载辅助类
    /// </summary>
    public static class AssemblyLoader
    {
        /// <summary>
        /// 正在加载的程序集集合,防止递归加载
        /// </summary>
        private static readonly HashSet<string> _loadingAssemblies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 已加载程序集的路径缓存,用于避免重复加载
        /// </summary>
        private static readonly ConcurrentDictionary<string, string> _loadedAssemblyPaths = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 安全加载程序集,避免重复加载导致调试器元数据损坏
        /// </summary>
        /// <param name="assemblyName">程序集名称(可以不带.dll扩展名)</param>
        /// <returns>加载的程序集</returns>
        public static Assembly LoadAssembly(string assemblyName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            // 移除.dll扩展名(如果有)
            string cleanName = assemblyName.Replace(".dll", "").Replace(".exe", "");

            try
            {
                // 首先尝试从已加载的程序集中查找
                var loadedAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetName().Name.Equals(cleanName, StringComparison.OrdinalIgnoreCase));

                if (loadedAssembly != null)
                {
                    return loadedAssembly;
                }

                // 使用Load加载,避免LoadFrom导致的重复加载问题
                return Assembly.Load(new AssemblyName(cleanName));
            }
            catch
            {
                // 如果Load失败,尝试从文件路径加载(仅用于插件等特殊情况)
                try
                {
                    string assemblyPath = GetAssemblyPath(cleanName);
                    if (File.Exists(assemblyPath))
                    {
                        // 注意:这是fallback,仍使用LoadFrom但会触发AssemblyResolve
                        return Assembly.LoadFrom(assemblyPath);
                    }
                }
                catch
                {
                    // 忽略异常,返回null
                }

                return null;
            }
        }

        /// <summary>
        /// 从完整路径加载程序集(用于插件等动态加载场景)
        /// </summary>
        /// <param name="assemblyPath">程序集完整路径</param>
        /// <returns>加载的程序集</returns>
        public static Assembly LoadFromPath(string assemblyPath)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                throw new ArgumentNullException(nameof(assemblyPath));
            }

            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException($"程序集文件不存在: {assemblyPath}");
            }

            string assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
            string normalizedPath = Path.GetFullPath(assemblyPath);

            // 检查是否已通过此路径加载过
            string loadedPath;
            if (_loadedAssemblyPaths.TryGetValue(assemblyName.ToLower(), out loadedPath))
            {
                // 如果路径相同,返回已加载的程序集
                if (string.Equals(loadedPath, normalizedPath, StringComparison.OrdinalIgnoreCase))
                {
                    return GetLoadedAssembly(assemblyName);
                }
                // 路径不同但程序集名相同,返回已加载的避免冲突
                return GetLoadedAssembly(assemblyName);
            }

            // 防止递归加载
            if (_loadingAssemblies.Contains(assemblyName))
            {
                return GetLoadedAssembly(assemblyName);
            }

            // 首先尝试从已加载的程序集中查找
            var loadedAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase));

            if (loadedAssembly != null)
            {
                // 缓存路径
                _loadedAssemblyPaths.TryAdd(assemblyName.ToLower(), normalizedPath);
                return loadedAssembly;
            }

            // 标记为正在加载
            _loadingAssemblies.Add(assemblyName);
            try
            {
                // 优先使用Assembly.Load(通过程序集名加载)
                Assembly assembly = null;
                try
                {
                    var assemblyNameObj = AssemblyName.GetAssemblyName(assemblyPath);
                    assembly = Assembly.Load(assemblyNameObj);
                }
                catch
                {
                    // Load失败,使用LoadFrom
                    // 在LoadFrom之前,再次检查是否已被加载
                    assembly = GetLoadedAssembly(assemblyName);
                    if (assembly == null)
                    {
                        assembly = Assembly.LoadFrom(assemblyPath);
                    }
                }

                // 加载成功后缓存路径
                if (assembly != null)
                {
                    _loadedAssemblyPaths.TryAdd(assemblyName.ToLower(), normalizedPath);
                }

                return assembly;
            }
            finally
            {
                _loadingAssemblies.Remove(assemblyName);
            }
        }

        /// <summary>
        /// 获取程序集的路径
        /// </summary>
        private static string GetAssemblyPath(string assemblyName)
        {
            // 尝试多个可能的位置
            string[] searchPaths = new string[]
            {
                AppDomain.CurrentDomain.BaseDirectory,
                AppContext.BaseDirectory,
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            };

            foreach (string basePath in searchPaths)
            {
                if (!string.IsNullOrEmpty(basePath))
                {
                    string path = Path.Combine(basePath, assemblyName + ".dll");
                    if (File.Exists(path))
                    {
                        return path;
                    }

                    path = Path.Combine(basePath, assemblyName + ".exe");
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获取已加载的程序集,避免重复加载
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>已加载的程序集,如果未找到返回null</returns>
        public static Assembly GetLoadedAssembly(string assemblyName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                return null;
            }

            string cleanName = assemblyName.Replace(".dll", "").Replace(".exe", "");

            return AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name.Equals(cleanName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取指定命名空间中的类型
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="typeName">完整类型名称</param>
        /// <returns>类型对象,如果未找到返回null</returns>
        public static Type GetType(string assemblyName, string typeName)
        {
            var assembly = LoadAssembly(assemblyName);
            return assembly?.GetType(typeName);
        }
    }
}
