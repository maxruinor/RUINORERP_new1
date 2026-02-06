using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RUINORERP.AI
{
    /// <summary>
    /// Ollama API服务类，用于封装对本地部署的Ollama大模型的调用
    /// </summary>
    public class OllamaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        /// <summary>
        /// 初始化OllamaService实例
        /// </summary>
        /// <param name="apiBaseUrl">Ollama API的基础地址，默认为http://localhost:11434/api</param>
        public OllamaService(string apiBaseUrl = "http://localhost:11434/api")
        {
            _apiBaseUrl = apiBaseUrl;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_apiBaseUrl)
            };
        }

        /// <summary>
        /// 获取可用的模型列表
        /// </summary>
        /// <returns>模型列表</returns>
        /// <exception cref="HttpRequestException">当API调用失败时抛出</exception>
        public async Task<List<string>> GetModelsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/tags");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ModelsResponse>(content);

                return result?.Models?.ConvertAll(m => m.Name) ?? new List<string>();
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"获取模型列表失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 发送聊天请求到Ollama模型
        /// </summary>
        /// <param name="model">要使用的模型名称</param>
        /// <param name="messages">聊天消息列表</param>
        /// <param name="temperature">生成文本的温度参数，默认为0.8</param>
        /// <param name="maxTokens">生成文本的最大token数，默认为1024</param>
        /// <returns>模型的响应</returns>
        /// <exception cref="HttpRequestException">当API调用失败时抛出</exception>
        public async Task<ChatResponse> ChatAsync(string model, List<ChatMessage> messages, float temperature = 0.8f, int maxTokens = 1024)
        {
            try
            {
                var requestBody = new ChatRequest
                {
                    Model = model,
                    Messages = messages,
                    Temperature = temperature,
                    MaxTokens = maxTokens
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("/chat", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ChatResponse>(responseContent);
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"聊天请求失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 生成文本响应
        /// </summary>
        /// <param name="model">要使用的模型名称</param>
        /// <param name="prompt">提示词</param>
        /// <param name="temperature">生成文本的温度参数，默认为0.8</param>
        /// <param name="maxTokens">生成文本的最大token数，默认为1024</param>
        /// <returns>生成的文本</returns>
        /// <exception cref="HttpRequestException">当API调用失败时抛出</exception>
        public async Task<GenerateResponse> GenerateAsync(string model, string prompt, float temperature = 0.8f, int maxTokens = 1024)
        {
            try
            {
                var requestBody = new GenerateRequest
                {
                    Model = model,
                    Prompt = prompt,
                    Temperature = temperature,
                    MaxTokens = maxTokens
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("/generate", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GenerateResponse>(responseContent);
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"生成请求失败: {ex.Message}", ex);
            }
        }
    }

    #region 模型类

    /// <summary>
    /// 模型响应类
    /// </summary>
    public class ModelsResponse
    {
        /// <summary>
        /// 模型列表
        /// </summary>
        [JsonProperty("models")]
        public List<ModelInfo> Models { get; set; }
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
    }

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
    }

    #endregion
}
