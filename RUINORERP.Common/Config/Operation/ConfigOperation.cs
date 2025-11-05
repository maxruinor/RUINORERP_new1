/*************************************************************
 * 文件名：ConfigOperation.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置操作接口和基础配置类，定义配置系统的核心操作
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System.Threading.Tasks;

namespace RUINORERP.Common.Config.Operation
{
    /// <summary>
    /// 配置管理器接口
    /// 定义配置的基本操作
    /// </summary>
    /// <typeparam name="T">配置类型</typeparam>
    public interface IConfigurationManager<T> where T : BaseConfig
    {
        /// <summary>
        /// 异步加载配置
        /// </summary>
        /// <returns>配置实例</returns>
        Task<T> LoadConfigAsync();

        /// <summary>
        /// 异步保存配置
        /// </summary>
        /// <param name="config">配置实例</param>
        /// <returns>保存任务</returns>
        Task SaveConfigAsync(T config);

        /// <summary>
        /// 异步检查配置是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        Task<bool> ConfigExistsAsync();

        /// <summary>
        /// 异步重置配置为默认值
        /// </summary>
        /// <returns>重置任务</returns>
        Task ResetConfigAsync();
    }

    /// <summary>
    /// 基础配置类
    /// 所有配置类型的基类
    /// </summary>
    public abstract class BaseConfig
    {
        /// <summary>
        /// 配置版本
        /// 用于版本控制和迁移
        /// </summary>
        public int Version { get; set; } = 1;

        /// <summary>
        /// 配置类型名称
        /// 用于标识配置类型
        /// </summary>
        public virtual string ConfigTypeName => GetType().Name;

        /// <summary>
        /// 验证配置有效性
        /// 派生类应该重写此方法进行配置验证
        /// </summary>
        /// <returns>是否有效</returns>
        public virtual bool Validate()
        {
            return true; // 默认有效
        }

        /// <summary>
        /// 初始化默认配置
        /// 派生类应该重写此方法设置默认值
        /// </summary>
        public virtual void InitializeDefaults()
        {
            // 默认实现为空，派生类应该提供具体实现
        }

        /// <summary>
        /// 在配置保存前调用
        /// 可用于预处理配置
        /// </summary>
        public virtual void BeforeSave()
        {
            // 默认实现为空，派生类可以提供具体实现
        }

        /// <summary>
        /// 在配置加载后调用
        /// 可用于后处理配置
        /// </summary>
        public virtual void AfterLoad()
        {
            // 默认实现为空，派生类可以提供具体实现
        }

        /// <summary>
        /// 从旧版本迁移配置
        /// 当配置版本发生变化时调用
        /// </summary>
        /// <param name="oldVersion">旧版本号</param>
        public virtual void MigrateFrom(int oldVersion)
        {
            // 默认实现为空，派生类应该提供版本迁移逻辑
        }

        /// <summary>
        /// 获取配置描述
        /// 用于调试和日志记录
        /// </summary>
        /// <returns>配置描述</returns>
        public override string ToString()
        {
            return $"{ConfigTypeName} (v{Version})";
        }
    }

    /// <summary>
    /// 配置操作接口
    /// 定义配置的高级操作
    /// </summary>
    public interface IConfigOperation
    {
        /// <summary>
        /// 异步备份配置
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>备份路径</returns>
        Task<string> BackupConfigAsync(string configTypeName);

        /// <summary>
        /// 异步恢复配置
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="backupPath">备份路径</param>
        /// <returns>是否成功</returns>
        Task<bool> RestoreConfigAsync(string configTypeName, string backupPath);

        /// <summary>
        /// 异步验证配置
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <returns>验证结果</returns>
        Task<ConfigValidationResult> ValidateConfigAsync(string configTypeName);

        /// <summary>
        /// 异步清理配置
        /// 删除临时文件或过期配置
        /// </summary>
        /// <returns>清理的文件数量</returns>
        Task<int> CleanupConfigAsync();
    }

    /// <summary>
    /// 配置验证结果
    /// 包含配置验证的详细信息
    /// </summary>
    public class ConfigValidationResult
    {
        /// <summary>
        /// 配置类型名称
        /// </summary>
        public string ConfigTypeName { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 错误消息
        /// 如果验证失败，包含错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 验证详情
        /// 包含详细的验证结果
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigValidationResult()
        {
            IsValid = true;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isValid">是否有效</param>
        /// <param name="errorMessage">错误消息</param>
        public ConfigValidationResult(bool isValid, string errorMessage = null)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 获取验证结果描述
        /// </summary>
        /// <returns>描述文本</returns>
        public override string ToString()
        {
            return IsValid ? "有效" : $"无效: {ErrorMessage}";
        }
    }

    /// <summary>
    /// 配置操作异常
    /// 表示配置操作过程中发生的异常
    /// </summary>
    public class ConfigOperationException : System.Exception
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// 配置类型名称
        /// </summary>
        public string ConfigTypeName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="configTypeName">配置类型名称</param>
        public ConfigOperationException(string message, string operationType, string configTypeName = null)
            : base(message)
        {
            OperationType = operationType;
            ConfigTypeName = configTypeName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="configTypeName">配置类型名称</param>
        public ConfigOperationException(string message, System.Exception innerException, string operationType, string configTypeName = null)
            : base(message, innerException)
        {
            OperationType = operationType;
            ConfigTypeName = configTypeName;
        }
    }

    /// <summary>
    /// 配置管理器基础实现
    /// 提供配置管理的通用功能
    /// </summary>
    /// <typeparam name="T">配置类型</typeparam>
    public abstract class BaseConfigurationManager<T> : IConfigurationManager<T> where T : BaseConfig, new()
    {
        /// <summary>
        /// 异步加载配置
        /// 派生类必须实现此方法
        /// </summary>
        /// <returns>配置实例</returns>
        public abstract Task<T> LoadConfigAsync();

        /// <summary>
        /// 异步保存配置
        /// 派生类必须实现此方法
        /// </summary>
        /// <param name="config">配置实例</param>
        /// <returns>保存任务</returns>
        public abstract Task SaveConfigAsync(T config);

        /// <summary>
        /// 异步检查配置是否存在
        /// 派生类必须实现此方法
        /// </summary>
        /// <returns>是否存在</returns>
        public abstract Task<bool> ConfigExistsAsync();

        /// <summary>
        /// 异步重置配置为默认值
        /// 创建新的配置实例并初始化默认值
        /// </summary>
        /// <returns>重置任务</returns>
        public async Task ResetConfigAsync()
        {
            // 创建新的配置实例
            T config = new T();
            
            // 初始化默认值
            config.InitializeDefaults();
            
            // 保存配置
            await SaveConfigAsync(config);
        }

        /// <summary>
        /// 创建默认配置实例
        /// </summary>
        /// <returns>默认配置实例</returns>
        protected T CreateDefaultConfig()
        {
            T config = new T();
            config.InitializeDefaults();
            return config;
        }

        /// <summary>
        /// 验证配置
        /// 调用配置对象的Validate方法
        /// </summary>
        /// <param name="config">配置实例</param>
        /// <returns>验证结果</returns>
        protected bool ValidateConfig(T config)
        {
            if (config == null)
                return false;

            return config.Validate();
        }

        /// <summary>
        /// 处理配置加载后的操作
        /// 调用配置对象的AfterLoad方法
        /// </summary>
        /// <param name="config">配置实例</param>
        protected void OnAfterLoad(T config)
        {
            config?.AfterLoad();
        }

        /// <summary>
        /// 处理配置保存前的操作
        /// 调用配置对象的BeforeSave方法
        /// </summary>
        /// <param name="config">配置实例</param>
        protected void OnBeforeSave(T config)
        {
            config?.BeforeSave();
        }
    }
}