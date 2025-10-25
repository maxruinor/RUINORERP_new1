using RUINORERP.Model.ConfigModel;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 配置版本控制服务实现
    /// 提供配置版本的创建、存储、回滚等功能
    /// </summary>
    public class ConfigVersionService : IConfigVersionService
    {
        private readonly ILogger<ConfigVersionService> _logger;
        private  string _versionPath;
        private  string _snapshotPath;
        private readonly object _fileLock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public ConfigVersionService(ILogger<ConfigVersionService> logger)
        {
            _logger = logger;
            
            // 初始化路径
            InitializePaths();
        }

        // 添加路径初始化方法
        private void InitializePaths()
        {
            _versionPath = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles", "Versions");
            _snapshotPath = Path.Combine(_versionPath, "Snapshots");
            
            // 确保目录存在
            if (!Directory.Exists(_snapshotPath))
            {
                Directory.CreateDirectory(_snapshotPath);
            }
        }

        /// <summary>
        /// 创建配置版本快照
        /// </summary>
        public ConfigVersion CreateVersion(BaseConfig config, string configType, string description)
        {
            try
            {
                // 获取当前版本列表
                List<ConfigVersion> versions = GetVersions(configType);
                
                // 计算新版本号
                int newVersionNumber = versions.Count > 0 ? versions.Max(v => v.VersionNumber) + 1 : 1;
                
                // 创建版本对象
                ConfigVersion newVersion = new ConfigVersion
                {
                    ConfigType = configType,
                    VersionNumber = newVersionNumber,
                    Description = description,
                    IsActive = true
                };
                
                lock (_fileLock)
                {
                    // 保存配置快照
                    string snapshotFileName = $"{configType}_v{newVersionNumber}_{DateTime.Now:yyyyMMddHHmmss}.json";
                    string snapshotFilePath = Path.Combine(_snapshotPath, snapshotFileName);
                    
                    // 序列化配置对象
                    string configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                    File.WriteAllText(snapshotFilePath, configJson);
                    
                    newVersion.SnapshotPath = snapshotFileName;
                    
                    // 更新所有版本为非活动状态
                    versions.ForEach(v => v.IsActive = false);
                    
                    // 添加新版本
                    versions.Add(newVersion);
                    
                    // 限制版本数量，保留最近50个版本
                    if (versions.Count > 50)
                    {
                        // 删除旧版本快照文件
                        foreach (var oldVersion in versions.Take(versions.Count - 50))
                        {
                            string oldSnapshotPath = Path.Combine(_snapshotPath, oldVersion.SnapshotPath);
                            if (File.Exists(oldSnapshotPath))
                            {
                                File.Delete(oldSnapshotPath);
                            }
                        }
                        versions = versions.Skip(versions.Count - 50).ToList();
                    }
                    
                    // 保存版本信息
                    SaveVersions(configType, versions);
                }
                
                // 移除审计日志调用
                _logger.LogInformation("配置版本创建成功: {ConfigType}, v{VersionNumber}", 
                    configType, newVersionNumber);
                
                return newVersion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建配置版本失败: {ConfigType}", configType);
                throw;
            }
        }

        /// <summary>
        /// 异步创建配置版本快照
        /// </summary>
        public async Task<ConfigVersion> CreateVersionAsync(BaseConfig config, string configType,  string description)
        {
            return await Task.Run(() => CreateVersion(config, configType,  description));
        }

        /// <summary>
        /// 回滚到指定版本
        /// </summary>
        public bool RollbackToVersion(Guid versionId)
        {
            try
            {
                // 查找版本信息
                ConfigVersion version = null;
                string configType = null;
                List<ConfigVersion> allVersions = new List<ConfigVersion>();
                
                // 遍历所有配置类型查找版本
                string[] versionFiles = Directory.GetFiles(_versionPath, "*_Versions.json");
                foreach (string versionFile in versionFiles)
                {
                    string content = File.ReadAllText(versionFile);
                    List<ConfigVersion> versions = JsonConvert.DeserializeObject<List<ConfigVersion>>(content);
                    version = versions.FirstOrDefault(v => v.VersionId == versionId);
                    
                    if (version != null)
                    {
                        configType = Path.GetFileNameWithoutExtension(versionFile).Replace("_Versions", "");
                        allVersions = versions;
                        break;
                    }
                }
                
                if (version == null)
                {
                    _logger.LogWarning("未找到指定版本: {VersionId}", versionId);
                    return false;
                }
                
                lock (_fileLock)
                {
                    // 读取版本快照
                    string snapshotFilePath = Path.Combine(_snapshotPath, version.SnapshotPath);
                    if (!File.Exists(snapshotFilePath))
                    {
                        _logger.LogError("版本快照文件不存在: {SnapshotPath}", version.SnapshotPath);
                        return false;
                    }
                    
                    string snapshotContent = File.ReadAllText(snapshotFilePath);
                    
                    // 计算新版本号（回滚也创建新版本）
                    int newVersionNumber = allVersions.Count > 0 ? allVersions.Max(v => v.VersionNumber) + 1 : 1;
                    
                    // 创建回滚版本
                    ConfigVersion rollbackVersion = new ConfigVersion
                    {
                        ConfigType = configType,
                        VersionNumber = newVersionNumber,
                        Description = $"回滚到版本 v{version.VersionNumber}: {version.Description}",
                        IsActive = true
                    };
                    
                    // 保存回滚版本快照
                    string newSnapshotFileName = $"{configType}_v{newVersionNumber}_{DateTime.Now:yyyyMMddHHmmss}.json";
                    string newSnapshotFilePath = Path.Combine(_snapshotPath, newSnapshotFileName);
                    File.WriteAllText(newSnapshotFilePath, snapshotContent);
                    rollbackVersion.SnapshotPath = newSnapshotFileName;
                    
                    // 更新所有版本为非活动状态
                    allVersions.ForEach(v => v.IsActive = false);
                    
                    // 添加回滚版本
                    allVersions.Add(rollbackVersion);
                    
                    // 保存版本信息
                    SaveVersions(configType, allVersions);
                    
                    // 更新当前配置文件
                    string configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles", $"{configType}.json");
                    JObject configJson = JObject.Parse(snapshotContent);
                    JObject fullConfigJson = new JObject(new JProperty(configType, configJson));
                    File.WriteAllText(configFilePath, fullConfigJson.ToString(Formatting.Indented));
                }
                
                // 移除审计日志调用
                _logger.LogInformation("配置回滚成功: {ConfigType}, 回滚到 v{VersionNumber}", 
                    configType, version.VersionNumber);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "配置回滚失败: {VersionId}", versionId);
                return false;
            }
        }

        /// <summary>
        /// 异步回滚到指定版本
        /// </summary>
        public async Task<bool> RollbackToVersionAsync(Guid versionId)
        {
            return await Task.Run(() => RollbackToVersion(versionId));
        }

        /// <summary>
        /// 获取配置类型的所有版本
        /// </summary>
        public List<ConfigVersion> GetVersions(string configType)
        {
            try
            {
                string versionFilePath = Path.Combine(_versionPath, $"{configType}_Versions.json");
                
                if (!File.Exists(versionFilePath))
                {
                    return new List<ConfigVersion>();
                }
                
                string content = File.ReadAllText(versionFilePath);
                return JsonConvert.DeserializeObject<List<ConfigVersion>>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配置版本列表失败: {ConfigType}", configType);
                return new List<ConfigVersion>();
            }
        }

        /// <summary>
        /// 获取指定版本详情
        /// </summary>
        public ConfigVersion GetVersion(Guid versionId)
        {
            try
            {
                // 遍历所有配置类型查找版本
                string[] versionFiles = Directory.GetFiles(_versionPath, "*_Versions.json");
                foreach (string versionFile in versionFiles)
                {
                    string content = File.ReadAllText(versionFile);
                    List<ConfigVersion> versions = JsonConvert.DeserializeObject<List<ConfigVersion>>(content);
                    ConfigVersion version = versions.FirstOrDefault(v => v.VersionId == versionId);
                    
                    if (version != null)
                    {
                        return version;
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配置版本详情失败: {VersionId}", versionId);
                return null;
            }
        }

        /// <summary>
        /// 获取当前活动版本
        /// </summary>
        public ConfigVersion GetActiveVersion(string configType)
        {
            List<ConfigVersion> versions = GetVersions(configType);
            return versions.FirstOrDefault(v => v.IsActive);
        }

        /// <summary>
        /// 比较两个版本的差异
        /// </summary>
        public string CompareVersions(Guid versionId1, Guid versionId2)
        {
            var diffResult = CompareVersionsDetailed(versionId1, versionId2);
            return FormatDiffResult(diffResult);
        }

        private string FormatDiffResult(ConfigVersionDiffResult diffResult)
        {
            if (!diffResult.HasDifferences)
                return "两个版本完全相同";
            
            var sb = new StringBuilder();
            sb.AppendLine("版本差异分析:");
            
            if (diffResult.AddedProperties.Count > 0)
            {
                sb.AppendLine("新增属性:");
                foreach (var prop in diffResult.AddedProperties)
                {
                    sb.AppendLine($"  + {prop.Key}: {prop.Value}");
                }
            }
            
            if (diffResult.RemovedProperties.Count > 0)
            {
                sb.AppendLine("删除属性:");
                foreach (var prop in diffResult.RemovedProperties)
                {
                    sb.AppendLine($"  - {prop.Key}: {prop.Value}");
                }
            }
            
            if (diffResult.ModifiedProperties.Count > 0)
            {
                sb.AppendLine("修改属性:");
                foreach (var prop in diffResult.ModifiedProperties)
                {
                    sb.AppendLine($"  * {prop.Key}: {prop.Value.OldValue} -> {prop.Value.NewValue}");
                }
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// 删除指定版本
        /// </summary>
        public bool DeleteVersion(Guid versionId)
        {
            try
            {
                ConfigVersion version = null;
                string configType = null;
                List<ConfigVersion> allVersions = new List<ConfigVersion>();
                
                // 遍历所有配置类型查找版本
                string[] versionFiles = Directory.GetFiles(_versionPath, "*_Versions.json");
                foreach (string versionFile in versionFiles)
                {
                    string content = File.ReadAllText(versionFile);
                    List<ConfigVersion> versions = JsonConvert.DeserializeObject<List<ConfigVersion>>(content);
                    version = versions.FirstOrDefault(v => v.VersionId == versionId);
                    
                    if (version != null)
                    {
                        configType = Path.GetFileNameWithoutExtension(versionFile).Replace("_Versions", "");
                        allVersions = versions;
                        break;
                    }
                }
                
                if (version == null)
                {
                    _logger.LogWarning("未找到指定版本: {VersionId}", versionId);
                    return false;
                }
                
                // 不能删除当前活动版本
                if (version.IsActive)
                {
                    _logger.LogWarning("不能删除当前活动版本: {VersionId}", versionId);
                    return false;
                }
                
                lock (_fileLock)
                {
                    // 删除版本快照文件
                    string snapshotFilePath = Path.Combine(_snapshotPath, version.SnapshotPath);
                    if (File.Exists(snapshotFilePath))
                    {
                        File.Delete(snapshotFilePath);
                    }
                    
                    // 从版本列表中移除
                    allVersions.Remove(version);
                    
                    // 保存版本信息
                    SaveVersions(configType, allVersions);
                }
                
                _logger.LogInformation("配置版本删除成功: {ConfigType}, v{VersionNumber}", 
                    configType, version.VersionNumber);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除配置版本失败: {VersionId}", versionId);
                return false;
            }
        }

        /// <summary>
        /// 比较两个版本并返回结构化的差异结果
        /// </summary>
        public ConfigVersionDiffResult CompareVersionsDetailed(Guid versionId1, Guid versionId2)
        {
            try
            {
                ConfigVersion version1 = GetVersion(versionId1);
                ConfigVersion version2 = GetVersion(versionId2);
                
                if (version1 == null || version2 == null)
                {
                    _logger.LogWarning("比较版本失败：找不到指定的版本信息");
                    return new ConfigVersionDiffResult();
                }
                
                if (version1.ConfigType != version2.ConfigType)
                {
                    _logger.LogWarning("比较版本失败：不能比较不同配置类型的版本");
                    return new ConfigVersionDiffResult();
                }
                
                // 读取两个版本的快照内容
                string snapshotPath1 = Path.Combine(_snapshotPath, version1.SnapshotPath);
                string snapshotPath2 = Path.Combine(_snapshotPath, version2.SnapshotPath);
                
                if (!File.Exists(snapshotPath1) || !File.Exists(snapshotPath2))
                {
                    _logger.LogWarning("比较版本失败：版本快照文件不存在");
                    return new ConfigVersionDiffResult();
                }
                
                string content1 = File.ReadAllText(snapshotPath1);
                string content2 = File.ReadAllText(snapshotPath2);
                
                // 解析为JObject进行比较
                JObject json1 = JObject.Parse(content1);
                JObject json2 = JObject.Parse(content2);
                
                // 生成结构化差异结果
                return GenerateDetailedDiff(json1, json2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "比较配置版本详细差异失败");
                return new ConfigVersionDiffResult();
            }
        }

        /// <summary>
        /// 从版本文件加载配置对象
        /// </summary>
        public BaseConfig LoadConfigFromVersion(ConfigVersion version, Type configType)
        {
            try
            {
                if (version == null || !typeof(BaseConfig).IsAssignableFrom(configType))
                {
                    _logger.LogWarning("加载配置版本失败：版本信息为空或配置类型无效");
                    return null;
                }
                
                string snapshotFilePath = Path.Combine(_snapshotPath, version.SnapshotPath);
                if (!File.Exists(snapshotFilePath))
                {
                    _logger.LogWarning("加载配置版本失败：版本快照文件不存在: {SnapshotPath}", version.SnapshotPath);
                    return null;
                }
                
                string content = File.ReadAllText(snapshotFilePath);
                return JsonConvert.DeserializeObject(content, configType) as BaseConfig;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从版本加载配置对象失败");
                return null;
            }
        }

        /// <summary>
        /// 获取配置版本的存储目录
        /// </summary>
        public string GetVersionStoragePath(string configType)
        {
            // 返回配置类型对应的版本存储目录
            // 在这个实现中，我们使用统一的版本路径，但可以根据配置类型返回子目录
            return Path.Combine(_versionPath, configType);
        }

        /// <summary>
        /// 保存版本列表
        /// </summary>
        private void SaveVersions(string configType, List<ConfigVersion> versions)
        {
            string versionFilePath = Path.Combine(_versionPath, $"{configType}_Versions.json");
            string jsonContent = JsonConvert.SerializeObject(versions, Formatting.Indented);
            File.WriteAllText(versionFilePath, jsonContent);
        }

        /// <summary>
        /// 生成详细差异结果
        /// </summary>
        private ConfigVersionDiffResult GenerateDetailedDiff(JObject json1, JObject json2)
        {
            var result = new ConfigVersionDiffResult();
            CompareJObjects(json1, json2, "", result);
            return result;
        }

        /// <summary>
        /// 递归比较两个JObject对象的差异
        /// </summary>
        private void CompareJObjects(JObject obj1, JObject obj2, string path, ConfigVersionDiffResult result)
        {
            // 获取所有属性名
            var allKeys = obj1.Properties().Select(p => p.Name)
                .Union(obj2.Properties().Select(p => p.Name))
                .Distinct();

            foreach (var key in allKeys)
            {
                var currentPath = string.IsNullOrEmpty(path) ? key : $"{path}.{key}";
                var token1 = obj1[key];
                var token2 = obj2[key];

                if (token1 == null && token2 != null)
                {
                    // 新增属性
                    result.AddedProperties[currentPath] = token2.ToObject<object>();
                }
                else if (token1 != null && token2 == null)
                {
                    // 删除属性
                    result.RemovedProperties[currentPath] = token1.ToObject<object>();
                }
                else if (token1 != null && token2 != null)
                {
                    if (token1.Type != token2.Type)
                    {
                        // 类型变化
                        result.ModifiedProperties[currentPath] = new ModifiedPropertyValue
                        {
                            OldValue = token1.ToObject<object>(),
                            NewValue = token2.ToObject<object>()
                        };
                    }
                    else if (!JToken.DeepEquals(token1, token2))
                    {
                        if (token1.Type == JTokenType.Object)
                        {
                            // 递归比较嵌套对象
                            CompareJObjects((JObject)token1, (JObject)token2, currentPath, result);
                        }
                        else if (token1.Type == JTokenType.Array)
                        {
                            CompareArrays((JArray)token1, (JArray)token2, currentPath, result);
                        }
                        else
                        {
                            // 值改变
                            result.ModifiedProperties[currentPath] = new ModifiedPropertyValue
                            {
                                OldValue = token1.ToObject<object>(),
                                NewValue = token2.ToObject<object>()
                            };
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 比较两个JArray数组的差异
        /// </summary>
        private void CompareArrays(JArray arr1, JArray arr2, string path, ConfigVersionDiffResult result)
        {
            // 数组比较逻辑
            for (int i = 0; i < Math.Max(arr1.Count, arr2.Count); i++)
            {
                var itemPath = $"{path}[{i}]";
                if (i >= arr1.Count)
                {
                    // 新增数组元素
                    result.AddedProperties[itemPath] = arr2[i].ToObject<object>();
                }
                else if (i >= arr2.Count)
                {
                    // 删除数组元素
                    result.RemovedProperties[itemPath] = arr1[i].ToObject<object>();
                }
                else if (!JToken.DeepEquals(arr1[i], arr2[i]))
                {
                    result.ModifiedProperties[itemPath] = new ModifiedPropertyValue
                    {
                        OldValue = arr1[i].ToObject<object>(),
                        NewValue = arr2[i].ToObject<object>()
                    };
                }
            }
        }

        /// <summary>
        /// 测试方法：用于公开调用GenerateDetailedDiff方法进行测试
        /// </summary>
        /// <param name="json1">第一个JSON对象</param>
        /// <param name="json2">第二个JSON对象</param>
        /// <returns>差异比较结果</returns>
        public ConfigVersionDiffResult TestGenerateDetailedDiff(JObject json1, JObject json2)
        {
            return GenerateDetailedDiff(json1, json2);
        }
    }
}
