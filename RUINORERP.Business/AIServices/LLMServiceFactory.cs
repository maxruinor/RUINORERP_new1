using System;
using RUINORERP.Business.Config;
using RUINORERP.Model.ConfigModel;

namespace RUINORERP.Business.AIServices
{
    /// <summary>
    /// LLM服务工厂类
    /// 根据配置创建对应的LLM服务实例
    /// </summary>
    public static class LLMServiceFactory
    {
        /// <summary>
        /// 创建LLM服务实例
        /// 根据SystemGlobalConfig中的AIProviderType配置创建对应的服务
        /// </summary>
        /// <returns>ILLMService实例</returns>
        /// <exception cref="NotSupportedException">当配置的AI提供商类型不支持时抛出</exception>
        public static ILLMService Create()
        {
            var config = ConfigManager.GetSystemGlobalConfig();
            return Create(config.AIProviderType, config);
        }

        /// <summary>
        /// 创建指定类型的LLM服务实例
        /// </summary>
        /// <param name="providerType">AI提供商类型</param>
        /// <returns>ILLMService实例</returns>
        /// <exception cref="NotSupportedException">当指定的AI提供商类型不支持时抛出</exception>
        public static ILLMService Create(string providerType)
        {
            var config = ConfigManager.GetSystemGlobalConfig();
            return Create(providerType, config);
        }

        /// <summary>
        /// 创建指定类型的LLM服务实例（使用指定配置）
        /// </summary>
        /// <param name="providerType">AI提供商类型</param>
        /// <param name="config">系统全局配置</param>
        /// <returns>ILLMService实例</returns>
        /// <exception cref="NotSupportedException">当指定的AI提供商类型不支持时抛出</exception>
        public static ILLMService Create(string providerType, SystemGlobalConfig config)
        {
            if (string.IsNullOrEmpty(providerType))
            {
                throw new ArgumentException("AI提供商类型不能为空", nameof(providerType));
            }

            switch (providerType.ToLower())
            {
                case "ollama":
                    return new OllamaService(config);
                case "openai":
                    // 预留：OpenAI服务实现
                    throw new NotImplementedException("OpenAI服务尚未实现");
                case "azureopenai":
                    // 预留：Azure OpenAI服务实现
                    throw new NotImplementedException("Azure OpenAI服务尚未实现");
                default:
                    throw new NotSupportedException($"不支持的AI提供商类型: {providerType}");
            }
        }

        /// <summary>
        /// 检查指定的AI提供商类型是否受支持
        /// </summary>
        /// <param name="providerType">AI提供商类型</param>
        /// <returns>如果支持返回true，否则返回false</returns>
        public static bool IsProviderSupported(string providerType)
        {
            if (string.IsNullOrEmpty(providerType))
            {
                return false;
            }

            switch (providerType.ToLower())
            {
                case "ollama":
                    return true;
                case "openai":
                case "azureopenai":
                    // 已实现但可能未完全测试
                    return false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 获取支持的AI提供商类型列表
        /// </summary>
        /// <returns>支持的提供商类型数组</returns>
        public static string[] GetSupportedProviders()
        {
            return new[] { "Ollama", "OpenAI", "AzureOpenAI" };
        }
    }
}