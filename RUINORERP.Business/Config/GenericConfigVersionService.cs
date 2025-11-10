using RUINORERP.Model.ConfigModel;
using RUINORERP.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 泛型配置版本服务
    /// 为特定类型的配置提供版本管理功能
    /// </summary>
    /// <typeparam name="T">配置类型，必须继承自BaseConfig</typeparam>
    public class GenericConfigVersionService<T> where T : BaseConfig
    {
        private readonly ILogger<GenericConfigVersionService<T>> _logger;
        private  string _versionPath;
        private  string _snapshotPath;
        private readonly object _fileLock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public GenericConfigVersionService(ILogger<GenericConfigVersionService<T>> logger)
        {
            _logger = logger;
            InitializePaths();
        }

        /// <summary>
        /// 创建配置版本
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <param name="description">版本描述</param>
        /// <returns>创建的版本信息</returns>
        public ConfigVersion CreateVersion(T config, string description)
        {
            try
            {
                var versions = GetVersions();
                int newVersionNumber = versions.Count > 0 ? versions.Max(v => v.VersionNumber) + 1 : 1;
                
                var newVersion = new ConfigVersion
                {
                    ConfigType = typeof(T).Name,
                    VersionNumber = newVersionNumber,
                    Description = description,
                    CreatedTime = DateTime.Now,
                    IsActive = true
                };

                lock (_fileLock)
                {
                    // 保存快照
                    string snapshotFileName = $"{typeof(T).Name}_v{newVersionNumber}_{DateTime.Now:yyyyMMddHHmmss}.json";
                    string snapshotFilePath = Path.Combine(_snapshotPath, snapshotFileName);
                    
                    string configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                    File.WriteAllText(snapshotFilePath, configJson);
                    
                    newVersion.SnapshotPath = snapshotFileName;
                    
                    // 更新版本状态
                    versions.ForEach(v => v.IsActive = false);
                    versions.Add(newVersion);
                    
                    // 限制版本数量
                    if (versions.Count > 50)
                    {
                        var oldVersions = versions.Take(versions.Count - 50).ToList();
                        foreach (var oldVersion in oldVersions)
                        {
                            string oldSnapshotPath = Path.Combine(_snapshotPath, oldVersion.SnapshotPath);
                            if (File.Exists(oldSnapshotPath))
                            {
                                File.Delete(oldSnapshotPath);
                            }
                        }
                        versions = versions.Skip(versions.Count - 50).ToList();
                    }
                    
                    SaveVersions(versions);
                }
                
                _logger.LogDebug("配置版本创建成功: {ConfigType}, v{VersionNumber}", 
                    typeof(T).Name, newVersionNumber);
                
                return newVersion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建配置版本失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// 获取所有配置版本
        /// </summary>
        /// <returns>版本列表</returns>
        public List<ConfigVersion> GetVersions()
        {
            try
            {
                string versionFilePath = GetVersionFilePath();
                
                if (!File.Exists(versionFilePath))
                {
                    return new List<ConfigVersion>();
                }
                
                string content = File.ReadAllText(versionFilePath);
                return JsonConvert.DeserializeObject<List<ConfigVersion>>(content) ?? new List<ConfigVersion>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配置版本列表失败: {ConfigType}", typeof(T).Name);
                return new List<ConfigVersion>();
            }
        }

        /// <summary>
        /// 比较两个版本的差异
        /// </summary>
        /// <param name="versionId1">版本1 ID</param>
        /// <param name="versionId2">版本2 ID</param>
        /// <returns>差异结果</returns>
        public ConfigVersionDiffResult CompareVersions(Guid versionId1, Guid versionId2)
        {
            try
            {
                var version1 = GetVersion(versionId1);
                var version2 = GetVersion(versionId2);
                
                if (version1 == null || version2 == null)
                {
                    _logger.LogWarning("比较版本失败：找不到指定的版本信息");
                    return new ConfigVersionDiffResult();
                }
                
                string snapshotPath1 = Path.Combine(_snapshotPath, version1.SnapshotPath);
                string snapshotPath2 = Path.Combine(_snapshotPath, version2.SnapshotPath);
                
                if (!File.Exists(snapshotPath1) || !File.Exists(snapshotPath2))
                {
                    _logger.LogWarning("比较版本失败：版本快照文件不存在");
                    return new ConfigVersionDiffResult();
                }
                
                string content1 = File.ReadAllText(snapshotPath1);
                string content2 = File.ReadAllText(snapshotPath2);
                
                JObject json1 = JObject.Parse(content1);
                JObject json2 = JObject.Parse(content2);
                
                return GenerateDetailedDiff(json1, json2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "比较配置版本失败");
                return new ConfigVersionDiffResult();
            }
        }

        /// <summary>
        /// 获取指定版本
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <returns>版本信息</returns>
        public ConfigVersion GetVersion(Guid versionId)
        {
            try
            {
                var versions = GetVersions();
                return versions.FirstOrDefault(v => v.VersionId == versionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取指定版本失败: {VersionId}", versionId);
                return null;
            }
        }

        /// <summary>
        /// 回滚到指定版本
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <returns>回滚是否成功</returns>
        public bool RollbackToVersion(Guid versionId)
        {
            try
            {
                var version = GetVersion(versionId);
                if (version == null)
                {
                    _logger.LogWarning("未找到指定版本: {VersionId}", versionId);
                    return false;
                }

                lock (_fileLock)
                {
                    // 加载版本快照
                    string snapshotFilePath = Path.Combine(_snapshotPath, version.SnapshotPath);
                    if (!File.Exists(snapshotFilePath))
                    {
                        _logger.LogError("版本快照文件不存在: {SnapshotPath}", version.SnapshotPath);
                        return false;
                    }

                    // 重置所有版本为非活动状态
                    var versions = GetVersions();
                    versions.ForEach(v => v.IsActive = false);
                    
                    // 设置当前版本为活动
                    var targetVersion = versions.FirstOrDefault(v => v.VersionId == versionId);
                    if (targetVersion != null)
                    {
                        targetVersion.IsActive = true;
                    }
                    
                    SaveVersions(versions);
                }

                _logger.LogDebug("配置版本回滚成功: {ConfigType}, v{VersionNumber}", 
                    typeof(T).Name, version.VersionNumber);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "回滚配置版本失败: {VersionId}", versionId);
                return false;
            }
        }

        /// <summary>
        /// 删除指定版本
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <returns>删除是否成功</returns>
        public bool DeleteVersion(Guid versionId)
        {
            try
            {
                var versions = GetVersions();
                var version = versions.FirstOrDefault(v => v.VersionId == versionId);
                
                if (version == null)
                {
                    _logger.LogWarning("未找到指定版本: {VersionId}", versionId);
                    return false;
                }

                // 不能删除当前活动版本
                if (version.IsActive)
                {
                    _logger.LogWarning("不能删除当前活动的版本: {VersionId}", versionId);
                    return false;
                }

                lock (_fileLock)
                {
                    // 删除快照文件
                    string snapshotFilePath = Path.Combine(_snapshotPath, version.SnapshotPath);
                    if (File.Exists(snapshotFilePath))
                    {
                        File.Delete(snapshotFilePath);
                    }

                    // 从版本列表中移除
                    versions.Remove(version);
                    SaveVersions(versions);
                }

                _logger.LogDebug("配置版本删除成功: {ConfigType}, v{VersionNumber}", 
                    typeof(T).Name, version.VersionNumber);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除配置版本失败: {VersionId}", versionId);
                return false;
            }
        }

        /// <summary>
        /// 从版本加载配置
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <returns>加载的配置对象</returns>
        public T LoadFromVersion(Guid versionId)
        {
            try
            {
                var version = GetVersion(versionId);
                if (version == null)
                {
                    _logger.LogWarning("未找到指定版本: {VersionId}", versionId);
                    return null;
                }

                string snapshotFilePath = Path.Combine(_snapshotPath, version.SnapshotPath);
                if (!File.Exists(snapshotFilePath))
                {
                    _logger.LogError("版本快照文件不存在: {SnapshotPath}", version.SnapshotPath);
                    return null;
                }

                string content = File.ReadAllText(snapshotFilePath);
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从版本加载配置失败: {VersionId}", versionId);
                return null;
            }
        }

        /// <summary>
        /// 获取版本文件路径
        /// </summary>
        /// <returns>版本文件完整路径</returns>
        private string GetVersionFilePath()
        {
            return Path.Combine(_versionPath, $"{typeof(T).Name}_Versions.json");
        }

        /// <summary>
        /// 保存版本列表
        /// </summary>
        /// <param name="versions">版本列表</param>
        private void SaveVersions(List<ConfigVersion> versions)
        {
            try
            {
                string versionFilePath = GetVersionFilePath();
                string jsonContent = JsonConvert.SerializeObject(versions, Formatting.Indented);
                File.WriteAllText(versionFilePath, jsonContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存版本列表失败: {ConfigType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// 初始化存储路径
        /// </summary>
        private void InitializePaths()
        {
            _versionPath = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles", "Versions");
            _snapshotPath = Path.Combine(_versionPath, "Snapshots");
            
            if (!Directory.Exists(_snapshotPath))
            {
                Directory.CreateDirectory(_snapshotPath);
            }
        }

        /// <summary>
        /// 生成详细的差异信息
        /// </summary>
        /// <param name="json1">第一个JSON对象</param>
        /// <param name="json2">第二个JSON对象</param>
        /// <returns>差异结果</returns>
        private ConfigVersionDiffResult GenerateDetailedDiff(JObject json1, JObject json2)
        {
            var result = new ConfigVersionDiffResult();
            
            // 获取所有属性名
            var allProperties = new HashSet<string>(json1.Properties().Select(p => p.Name));
            allProperties.UnionWith(json2.Properties().Select(p => p.Name));
            
            foreach (string propertyName in allProperties)
            {
                bool hasProperty1 = json1.TryGetValue(propertyName, out JToken value1);
                bool hasProperty2 = json2.TryGetValue(propertyName, out JToken value2);
                
                if (!hasProperty1 && hasProperty2)
                {
                    // 新增的属性
                    result.AddedProperties[propertyName] = value2.ToObject<object>();
                }
                else if (hasProperty1 && !hasProperty2)
                {
                    // 删除的属性
                    result.RemovedProperties[propertyName] = value1.ToObject<object>();
                }
                else if (hasProperty1 && hasProperty2 && !JToken.DeepEquals(value1, value2))
                {
                    // 修改的属性
                    result.ModifiedProperties[propertyName] = new ModifiedPropertyValue
                    {
                        OldValue = value1.ToObject<object>(),
                        NewValue = value2.ToObject<object>()
                    };
                }
            }
            
            return result;
        }
    }
}