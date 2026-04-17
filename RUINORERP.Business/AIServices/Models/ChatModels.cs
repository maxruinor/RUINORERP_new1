using Newtonsoft.Json;
using System.Collections.Generic;

namespace RUINORERP.Business.AIServices.Models
{
    /// <summary>
    /// 聊天消息类
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// 消息角色，可选值：system、user、assistant
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// 创建系统消息
        /// </summary>
        public static ChatMessage CreateSystemMessage(string content)
        {
            return new ChatMessage { Role = "system", Content = content };
        }

        /// <summary>
        /// 创建用户消息
        /// </summary>
        public static ChatMessage CreateUserMessage(string content)
        {
            return new ChatMessage { Role = "user", Content = content };
        }

        /// <summary>
        /// 创建助手消息
        /// </summary>
        public static ChatMessage CreateAssistantMessage(string content)
        {
            return new ChatMessage { Role = "assistant", Content = content };
        }
    }

    /// <summary>
    /// 聊天请求类
    /// </summary>
    public class ChatRequest
    {
        /// <summary>
        /// 要使用的模型名称
        /// </summary>
        [JsonProperty("model")]
        public string Model { get; set; }

        /// <summary>
        /// 聊天消息列表
        /// </summary>
        [JsonProperty("messages")]
        public List<ChatMessage> Messages { get; set; }

        /// <summary>
        /// 生成文本的温度参数
        /// </summary>
        [JsonProperty("temperature")]
        public float Temperature { get; set; }

        /// <summary>
        /// 生成文本的最大token数
        /// </summary>
        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }

        /// <summary>
        /// 是否流式输出
        /// </summary>
        [JsonProperty("stream")]
        public bool Stream { get; set; } = false;
    }

    /// <summary>
    /// 聊天响应类
    /// </summary>
    public class ChatResponse
    {
        /// <summary>
        /// 响应消息
        /// </summary>
        [JsonProperty("message")]
        public ChatMessage Message { get; set; }

        /// <summary>
        /// 生成的总token数
        /// </summary>
        [JsonProperty("total_duration")]
        public long TotalDuration { get; set; }

        /// <summary>
        /// 提示处理的token数
        /// </summary>
        [JsonProperty("prompt_eval_count")]
        public int PromptEvalCount { get; set; }

        /// <summary>
        /// 生成的token数
        /// </summary>
        [JsonProperty("eval_count")]
        public int EvalCount { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 获取响应文本内容
        /// </summary>
        public string GetContent()
        {
            return Message?.Content ?? string.Empty;
        }
    }
}