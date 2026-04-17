using Newtonsoft.Json;

namespace RUINORERP.Business.AIServices.Models
{
    /// <summary>
    /// 生成请求类
    /// </summary>
    public class GenerateRequest
    {
        /// <summary>
        /// 要使用的模型名称
        /// </summary>
        [JsonProperty("model")]
        public string Model { get; set; }

        /// <summary>
        /// 提示词
        /// </summary>
        [JsonProperty("prompt")]
        public string Prompt { get; set; }

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
    /// 生成响应类
    /// </summary>
    public class GenerateResponse
    {
        /// <summary>
        /// 生成的文本
        /// </summary>
        [JsonProperty("response")]
        public string Response { get; set; }

        /// <summary>
        /// 生成的总时间
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
    }

    /// <summary>
    /// 模型信息类
    /// </summary>
    public class ModelInfo
    {
        /// <summary>
        /// 模型名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 模型大小
        /// </summary>
        [JsonProperty("size")]
        public long Size { get; set; }

        /// <summary>
        /// 模型修改时间
        /// </summary>
        [JsonProperty("modified_at")]
        public string ModifiedAt { get; set; }
    }

    /// <summary>
    /// 模型响应类
    /// </summary>
    public class ModelsResponse
    {
        /// <summary>
        /// 模型列表
        /// </summary>
        [JsonProperty("models")]
        public System.Collections.Generic.List<ModelInfo> Models { get; set; }
    }
}