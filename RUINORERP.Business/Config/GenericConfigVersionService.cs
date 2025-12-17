using RUINORERP.Model.ConfigModel;
using RUINORERP.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 泛型配置版本控制服务实现
    /// 提供类型安全的配置版本管理、存储、回滚等功能
    /// 内部使用 ConfigVersionService 作为底层服务
    /// </summary>
    /// <typeparam name="T">配置类型，必须继承自BaseConfig</typeparam>
    public class GenericConfigVersionService<T> : IGenericConfigVersionService<T> where T : BaseConfig
    {
        private readonly IConfigVersionService _configVersionService;
        private readonly ILogger<GenericConfigVersionService<T>> _logger;
        private readonly string _configType;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configVersionService">基础配置版本服务</param>
        /// <param name="logger">日志记录器</param>
        public GenericConfigVersionService(IConfigVersionService configVersionService, ILogger<GenericConfigVersionService<T>> logger)
        {
            _configVersionService = configVersionService;
            _logger = logger;
            _configType = typeof(T).Name;
        }

        /// <summary>
        /// 创建配置版本快照
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <param name="description">版本描述</param>
        /// <returns>创建的版本信息</returns>
        public ConfigVersion CreateVersion(T config, string description)
        {
            try
            {
                return _configVersionService.CreateVersion(config, _configType, description);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建配置版本失败: {ConfigType}", _configType);
                throw;
            }
        }

        /// <summary>
        /// 异步创建配置版本快照
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <param name="description">版本描述</param>
        /// <returns>创建的版本信息</returns>
        public async Task<ConfigVersion> CreateVersionAsync(T config, string description)
        {
            try
            {
                return await _configVersionService.CreateVersionAsync(config, _configType, description);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "异步创建配置版本失败: {ConfigType}", _configType);
                throw;
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
                return _configVersionService.RollbackToVersion(versionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "回滚配置版本失败: {VersionId}, {ConfigType}", versionId, _configType);
                throw;
            }
        }

        /// <summary>
        /// 异步回滚到指定版本
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <returns>回滚是否成功</returns>
        public async Task<bool> RollbackToVersionAsync(Guid versionId)
        {
            try
            {
                return await _configVersionService.RollbackToVersionAsync(versionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "异步回滚配置版本失败: {VersionId}, {ConfigType}", versionId, _configType);
                throw;
            }
        }

        /// <summary>
        /// 获取当前配置类型的所有版本
        /// </summary>
        /// <returns>版本列表</returns>
        public List<ConfigVersion> GetVersions()
        {
            try
            {
                return _configVersionService.GetVersions(_configType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配置版本列表失败: {ConfigType}", _configType);
                return new List<ConfigVersion>();
            }
        }

        /// <summary>
        /// 获取指定版本详情
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <returns>版本信息</returns>
        public ConfigVersion GetVersion(Guid versionId)
        {
            try
            {
                return _configVersionService.GetVersion(versionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取配置版本详情失败: {VersionId}, {ConfigType}", versionId, _configType);
                return null;
            }
        }

        /// <summary>
        /// 获取当前活动版本
        /// </summary>
        /// <returns>当前活动版本</returns>
        public ConfigVersion GetActiveVersion()
        {
            try
            {
                return _configVersionService.GetActiveVersion(_configType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取当前活动版本失败: {ConfigType}", _configType);
                return null;
            }
        }

        /// <summary>
        /// 比较两个版本的差异
        /// </summary>
        /// <param name="versionId1">版本1 ID</param>
        /// <param name="versionId2">版本2 ID</param>
        /// <returns>差异信息</returns>
        public string CompareVersions(Guid versionId1, Guid versionId2)
        {
            try
            {
                return _configVersionService.CompareVersions(versionId1, versionId2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "比较配置版本差异失败: {VersionId1}, {VersionId2}, {ConfigType}", versionId1, versionId2, _configType);
                return "比较版本差异时发生错误";
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
                return _configVersionService.DeleteVersion(versionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除配置版本失败: {VersionId}, {ConfigType}", versionId, _configType);
                return false;
            }
        }

        /// <summary>
        /// 比较两个版本并返回结构化的差异结果
        /// </summary>
        /// <param name="versionId1">版本1 ID</param>
        /// <param name="versionId2">版本2 ID</param>
        /// <returns>版本差异结果对象</returns>
        public ConfigVersionDiffResult CompareVersionsDetailed(Guid versionId1, Guid versionId2)
        {
            try
            {
                return _configVersionService.CompareVersionsDetailed(versionId1, versionId2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "比较配置版本详细差异失败: {VersionId1}, {VersionId2}, {ConfigType}", versionId1, versionId2, _configType);
                return new ConfigVersionDiffResult();
            }
        }

        /// <summary>
        /// 从版本文件加载配置对象
        /// </summary>
        /// <param name="version">版本信息</param>
        /// <returns>加载的配置对象</returns>
        public T LoadConfigFromVersion(ConfigVersion version)
        {
            try
            {
                if (version == null)
                {
                    _logger.LogWarning("加载配置版本失败：版本信息为空");
                    return null;
                }

                BaseConfig baseConfig = _configVersionService.LoadConfigFromVersion(version, typeof(T));
                return baseConfig as T;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从版本加载配置对象失败: {VersionId}, {ConfigType}", version?.VersionId ?? Guid.Empty, _configType);
                return null;
            }
        }

        /// <summary>
        /// 获取配置版本的存储目录
        /// </summary>
        /// <returns>版本存储目录路径</returns>
        public string GetVersionStoragePath()
        {
            try
            {
                return _configVersionService.GetVersionStoragePath(_configType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取版本存储路径失败: {ConfigType}", _configType);
                return string.Empty;
            }
        }
    }
}