using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RUINORERP.Business.AIServices.Models;
using RUINORERP.Business.Config;
using RUINORERP.Model.ConfigModel;

namespace RUINORERP.Business.AIServices
{
    /// <summary>
    /// Ollama API服务类，用于封装对本地部署的Ollama大模型的调用
    /// </summary>
    public class OllamaService : ILLMService
    {
        private readonly HttpClient _httpClient;
        private readonly SystemGlobalConfig _config;

        /// <summary>
        /// 初始化OllamaService实例
        /// 从系统配置中读取Ollama API地址和默认模型
        /// </summary>
        public OllamaService()
        {
            _config = ConfigManager.GetConfig<SystemGlobalConfig>();
            var apiBaseUrl = _config.OllamaApiAddress;
            
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl),
                Timeout = TimeSpan.FromSeconds(_config.AIRequestTimeout)
            };
        }

        /// <summary>
        /// 初始化OllamaService实例（指定配置）
        /// </summary>
        /// <param name="config">系统全局配置</param>
        public OllamaService(SystemGlobalConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            var apiBaseUrl = _config.OllamaApiAddress;
            
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl),
                Timeout = TimeSpan.FromSeconds(_config.AIRequestTimeout)
            };
        }

        /// <summary>
        /// 获取可用的模型列表
        /// </summary>
        /// <returns>模型名称列表</returns>
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
        /// <param name="messages">聊天消息列表</param>
        /// <param name="temperature">生成文本的温度参数，默认为0.8</param>
        /// <param name="maxTokens">生成文本的最大token数，默认为1024</param>
        /// <returns>模型的响应</returns>
        /// <exception cref="HttpRequestException">当API调用失败时抛出</exception>
        public async Task<ChatResponse> ChatAsync(List<ChatMessage> messages, float temperature = 0.8f, int maxTokens = 1024)
        {
            try
            {
                var requestBody = new ChatRequest
                {
                    Model = _config.OllamaDefaultModel,
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
                return new ChatResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"聊天请求失败: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 生成文本响应
        /// </summary>
        /// <param name="prompt">提示词</param>
        /// <param name="temperature">生成文本的温度参数，默认为0.8</param>
        /// <param name="maxTokens">生成文本的最大token数，默认为1024</param>
        /// <returns>生成的文本响应</returns>
        /// <exception cref="HttpRequestException">当API调用失败时抛出</exception>
        public async Task<GenerateResponse> GenerateAsync(string prompt, float temperature = 0.8f, int maxTokens = 1024)
        {
            try
            {
                var requestBody = new GenerateRequest
                {
                    Model = _config.OllamaDefaultModel,
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
                return new GenerateResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"生成请求失败: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 获取当前使用的模型名称
        /// </summary>
        /// <returns>模型名称</returns>
        public string GetCurrentModel()
        {
            return _config.OllamaDefaultModel;
        }

        /// <summary>
        /// 检查服务是否可用
        /// </summary>
        /// <returns>如果服务可用返回true，否则返回false</returns>
        public async Task<bool> IsAvailableAsync()
        {
            try
            {
                var models = await GetModelsAsync();
                return models.Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}