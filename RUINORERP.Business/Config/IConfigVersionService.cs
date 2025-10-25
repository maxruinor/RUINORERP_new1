using RUINORERP.Model.ConfigModel;
using RUINORERP.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 配置版本控制服务接口
    /// 提供配置版本的管理、存储、回滚等功能
    /// </summary>
    public interface IConfigVersionService
    {
        /// <summary>
        /// 创建配置版本快照
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <param name="configType">配置类型</param>
        /// <param name="description">版本描述</param>
        /// <returns>创建的版本信息</returns>
        ConfigVersion CreateVersion(BaseConfig config, string configType, string description);

        /// <summary>
        /// 异步创建配置版本快照
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <param name="configType">配置类型</param>
        /// <param name="description">版本描述</param>
        /// <returns>创建的版本信息</returns>
        Task<ConfigVersion> CreateVersionAsync(BaseConfig config, string configType, string description);

        /// <summary>
        /// 回滚到指定版本
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <returns>回滚是否成功</returns>
        bool RollbackToVersion(Guid versionId);

        /// <summary>
        /// 异步回滚到指定版本
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <returns>回滚是否成功</returns>
        Task<bool> RollbackToVersionAsync(Guid versionId);

        /// <summary>
        /// 获取配置类型的所有版本
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>版本列表</returns>
        List<ConfigVersion> GetVersions(string configType);

        /// <summary>
        /// 获取指定版本详情
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <returns>版本信息</returns>
        ConfigVersion GetVersion(Guid versionId);

        /// <summary>
        /// 获取当前活动版本
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>当前活动版本</returns>
        ConfigVersion GetActiveVersion(string configType);

        /// <summary>
        /// 比较两个版本的差异
        /// </summary>
        /// <param name="versionId1">版本1 ID</param>
        /// <param name="versionId2">版本2 ID</param>
        /// <returns>差异信息</returns>
        string CompareVersions(Guid versionId1, Guid versionId2);

        /// <summary>
        /// 删除指定版本
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <returns>删除是否成功</returns>
        bool DeleteVersion(Guid versionId);

        /// <summary>
        /// 比较两个版本并返回结构化的差异结果
        /// </summary>
        /// <param name="versionId1">版本1 ID</param>
        /// <param name="versionId2">版本2 ID</param>
        /// <returns>版本差异结果对象</returns>
        ConfigVersionDiffResult CompareVersionsDetailed(Guid versionId1, Guid versionId2);

        /// <summary>
        /// 从版本文件加载配置对象
        /// </summary>
        /// <param name="version">版本信息</param>
        /// <param name="configType">配置类型</param>
        /// <returns>加载的配置对象</returns>
        BaseConfig LoadConfigFromVersion(ConfigVersion version, Type configType);

        /// <summary>
        /// 获取配置版本的存储目录
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>版本存储目录路径</returns>
        string GetVersionStoragePath(string configType);
    }
}
