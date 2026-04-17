using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.Business.AIServices.Models;

namespace RUINORERP.Business.AIServices
{
    /// <summary>
    /// LLM服务接口
    /// 定义大语言模型服务的标准操作
    /// </summary>
    public interface ILLMService
    {
        /// <summary>
        /// 获取可用的模型列表
        /// </summary>
        /// <returns>模型名称列表</returns>
        Task<List<string>> GetModelsAsync();

        /// <summary>
        /// 发送聊天请求到LLM模型
        /// </summary>
        /// <param name="messages">聊天消息列表</param>
        /// <param name="temperature">生成文本的温度参数，默认为0.8</param>
        /// <param name="maxTokens">生成文本的最大token数，默认为1024</param>
        /// <returns>模型的响应</returns>
        Task<ChatResponse> ChatAsync(List<ChatMessage> messages, float temperature = 0.8f, int maxTokens = 1024);

        /// <summary>
        /// 生成文本响应
        /// </summary>
        /// <param name="prompt">提示词</param>
        /// <param name="temperature">生成文本的温度参数，默认为0.8</param>
        /// <param name="maxTokens">生成文本的最大token数，默认为1024</param>
        /// <returns>生成的文本响应</returns>
        Task<GenerateResponse> GenerateAsync(string prompt, float temperature = 0.8f, int maxTokens = 1024);

        /// <summary>
        /// 获取当前使用的模型名称
        /// </summary>
        /// <returns>模型名称</returns>
        string GetCurrentModel();

        /// <summary>
        /// 检查服务是否可用
        /// </summary>
        /// <returns>如果服务可用返回true，否则返回false</returns>
        Task<bool> IsAvailableAsync();
    }
}