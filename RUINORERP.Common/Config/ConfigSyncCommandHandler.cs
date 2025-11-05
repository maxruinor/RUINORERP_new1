using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 配置同步命令处理器
    /// 处理从服务器接收的配置同步命令
    /// </summary>
    public class ConfigSyncCommandHandler
    {
        /// <summary>
        /// 配置管理器
        /// </summary>
        private readonly IConfigManager _configManager;
        
        /// <summary>
        /// 初始化配置同步命令处理器
        /// </summary>
        /// <param name="configManager">配置管理器</param>
        public ConfigSyncCommandHandler(IConfigManager configManager)
        {
            _configManager = configManager ?? throw new ArgumentNullException(nameof(configManager));
        }
        
        /// <summary>
        /// 处理配置同步命令
        /// 解析命令数据并调用配置管理器进行同步
        /// </summary>
        /// <param name="commandData">命令数据</param>
        /// <returns>处理是否成功</returns>
        public bool HandleSyncCommand(object commandData)
        {   
            try
            {   
                // 解析命令数据
                string configType, configDataJson, version;
                bool forceApply = false;
                
                if (TryExtractConfigData(commandData, out configType, out configDataJson, out version, out forceApply))
                {   
                    // 调用配置管理器同步配置
                    _configManager.SyncConfig(configType, configDataJson, version, forceApply);
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {   
                Debug.WriteLine($"处理配置同步命令失败: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// 尝试从命令数据中提取配置信息
        /// 支持多种数据格式：字典、JObject、动态对象等
        /// </summary>
        /// <param name="commandData">命令数据</param>
        /// <param name="configType">配置类型</param>
        /// <param name="configDataJson">配置数据JSON</param>
        /// <param name="version">配置版本</param>
        /// <param name="forceApply">是否强制应用</param>
        /// <returns>提取是否成功</returns>
        private bool TryExtractConfigData(object commandData, out string configType, out string configDataJson, out string version, out bool forceApply)
        {   
            configType = null;
            configDataJson = null;
            version = null;
            forceApply = false;
            
            if (commandData == null)
            {   
                return false;
            }
            
            // 尝试不同的数据格式解析
            try
            {   
                // 1. 尝试作为字典解析
                if (commandData is IDictionary<string, object> dictData)
                {   
                    return TryExtractFromDictionary(dictData, out configType, out configDataJson, out version, out forceApply);
                }
                
                // 2. 尝试作为JObject解析
                if (commandData is JObject jObjectData)
                {   
                    return TryExtractFromJObject(jObjectData, out configType, out configDataJson, out version, out forceApply);
                }
                
                // 3. 尝试作为动态对象解析
                try
                {   
                    dynamic dynamicData = commandData;
                    configType = dynamicData.ConfigType as string;
                    
                    // 配置数据可能是字符串或对象
                    if (dynamicData.ConfigData is string stringData)
                    {   
                        configDataJson = stringData;
                    }
                    else
                    {   
                        configDataJson = JObject.FromObject(dynamicData.ConfigData).ToString();
                    }
                    
                    version = dynamicData.Version as string;
                    forceApply = dynamicData.ForceApply as bool? ?? false;
                    
                    return !string.IsNullOrEmpty(configType) && !string.IsNullOrEmpty(configDataJson);
                }
                catch
                {   
                    // 动态解析失败，继续尝试其他方式
                }
                
                // 4. 尝试从Parameters属性中提取（向后兼容）
                try
                {   
                    dynamic dynamicData = commandData;
                    if (dynamicData.Parameters is IDictionary<string, object> parameters)
                    {   
                        return TryExtractFromDictionary(parameters, out configType, out configDataJson, out version, out forceApply);
                    }
                }
                catch
                {   
                    // 解析失败，继续尝试
                }
                
                // 5. 尝试从Data属性中提取（向后兼容）
                try
                {   
                    dynamic dynamicData = commandData;
                    if (dynamicData.Data != null)
                    {   
                        // 尝试将Data作为配置类型
                        configType = dynamicData.Data as string;
                        if (!string.IsNullOrEmpty(configType))
                        {   
                            // 尝试从Parameters中获取配置数据
                            if (dynamicData.Parameters is IDictionary<string, object> parameters)
                            {   
                                if (parameters.TryGetValue("ConfigData", out object dataObj))
                                {   
                                    configDataJson = dataObj as string ?? JObject.FromObject(dataObj).ToString();
                                    version = parameters.TryGetValue("Version", out object versionObj) ? versionObj as string : null;
                                    forceApply = parameters.TryGetValue("ForceApply", out object forceObj) && (bool)forceObj;
                                    
                                    return !string.IsNullOrEmpty(configDataJson);
                                }
                            }
                        }
                    }
                }
                catch
                {   
                    // 解析失败
                }
            }
            catch (Exception ex)
            {   
                Debug.WriteLine($"提取配置数据失败: {ex.Message}");
            }
            
            return false;
        }
        
        /// <summary>
        /// 尝试从字典中提取配置信息
        /// </summary>
        /// <param name="dict">数据字典</param>
        /// <param name="configType">配置类型</param>
        /// <param name="configDataJson">配置数据JSON</param>
        /// <param name="version">配置版本</param>
        /// <param name="forceApply">是否强制应用</param>
        /// <returns>提取是否成功</returns>
        private bool TryExtractFromDictionary(IDictionary<string, object> dict, out string configType, out string configDataJson, out string version, out bool forceApply)
        {   
            configType = null;
            configDataJson = null;
            version = null;
            forceApply = false;
            
            // 提取配置类型
            if (!dict.TryGetValue("ConfigType", out object typeObj) || typeObj == null)
            {   
                return false;
            }
            
            configType = typeObj.ToString();
            
            // 提取配置数据
            if (!dict.TryGetValue("ConfigData", out object dataObj) || dataObj == null)
            {   
                return false;
            }
            
            // 配置数据可能是字符串或对象
            if (dataObj is string stringData)
            {   
                configDataJson = stringData;
            }
            else if (dataObj is JObject jObject)
            {   
                configDataJson = jObject.ToString();
            }
            else
            {   
                // 尝试转换为JSON字符串
                try
                {   
                    configDataJson = JObject.FromObject(dataObj).ToString();
                }
                catch
                {   
                    return false;
                }
            }
            
            // 提取版本（可选）
            if (dict.TryGetValue("Version", out object versionObj) && versionObj != null)
            {   
                version = versionObj.ToString();
            }
            
            // 提取强制应用标志（可选）
            if (dict.TryGetValue("ForceApply", out object forceObj) && forceObj != null)
            {   
                bool.TryParse(forceObj.ToString(), out forceApply);
            }
            
            return true;
        }
        
        /// <summary>
        /// 尝试从JObject中提取配置信息
        /// </summary>
        /// <param name="jObject">JObject数据</param>
        /// <param name="configType">配置类型</param>
        /// <param name="configDataJson">配置数据JSON</param>
        /// <param name="version">配置版本</param>
        /// <param name="forceApply">是否强制应用</param>
        /// <returns>提取是否成功</returns>
        private bool TryExtractFromJObject(JObject jObject, out string configType, out string configDataJson, out string version, out bool forceApply)
        {   
            configType = null;
            configDataJson = null;
            version = null;
            forceApply = false;
            
            // 提取配置类型
            JToken typeToken = jObject.GetValue("ConfigType");
            if (typeToken == null)
            {   
                return false;
            }
            
            configType = typeToken.ToString();
            
            // 提取配置数据
            JToken dataToken = jObject.GetValue("ConfigData");
            if (dataToken == null)
            {   
                return false;
            }
            
            // 如果是字符串，则直接使用
            if (dataToken.Type == JTokenType.String)
            {   
                configDataJson = dataToken.ToString();
            }
            else
            {   
                // 否则，转换为JSON对象字符串
                configDataJson = dataToken.ToString();
            }
            
            // 提取版本（可选）
            JToken versionToken = jObject.GetValue("Version");
            if (versionToken != null)
            {   
                version = versionToken.ToString();
            }
            
            // 提取强制应用标志（可选）
            JToken forceToken = jObject.GetValue("ForceApply");
            if (forceToken != null && forceToken.Type == JTokenType.Boolean)
            {   
                forceApply = (bool)forceToken;
            }
            
            return true;
        }
        
        /// <summary>
        /// 创建配置同步数据
        /// 用于向客户端发送配置同步命令
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="configData">配置数据</param>
        /// <param name="forceApply">是否强制应用</param>
        /// <returns>配置同步数据字典</returns>
        public static Dictionary<string, object> CreateSyncData(string configType, object configData, bool forceApply = true)
        {   
            return new Dictionary<string, object>
            {
                { "ConfigType", configType },
                { "ConfigData", configData },
                { "Version", DateTime.Now.Ticks.ToString() }, // 使用时间戳作为版本
                { "ForceApply", forceApply }
            };
        }
    }
    
    /// <summary>
    /// 配置同步命令常量
    /// 定义配置同步相关的命令名称和键名
    /// </summary>
    public static class ConfigSyncConstants
    {
        /// <summary>
        /// 配置同步命令名称
        /// </summary>
        public const string ConfigSyncCommand = "ConfigSync";
        
        /// <summary>
        /// 配置类型键名
        /// </summary>
        public const string ConfigTypeKey = "ConfigType";
        
        /// <summary>
        /// 配置数据键名
        /// </summary>
        public const string ConfigDataKey = "ConfigData";
        
        /// <summary>
        /// 版本键名
        /// </summary>
        public const string VersionKey = "Version";
        
        /// <summary>
        /// 强制应用键名
        /// </summary>
        public const string ForceApplyKey = "ForceApply";
        
        /// <summary>
        /// 参数键名（向后兼容）
        /// </summary>
        public const string ParametersKey = "Parameters";
        
        /// <summary>
        /// 数据键名（向后兼容）
        /// </summary>
        public const string DataKey = "Data";
    }
}