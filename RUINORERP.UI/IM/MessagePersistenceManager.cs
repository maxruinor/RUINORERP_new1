using Newtonsoft.Json;

using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Message;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RUINORERP.UI.IM
{
    /// <summary>
    /// 消息持久化管理器
    /// 负责消息数据的本地持久化存储
    /// </summary>
    public class MessagePersistenceManager
    {
        /// <summary>
        /// 消息数据文件名
        /// </summary>
        private const string MESSAGE_FILE_NAME = "messages.json";

        /// <summary>
        /// 数据版本号
        /// </summary>
        private const string DATA_VERSION = "1.0";

        /// <summary>
        /// 数据保存路径
        /// </summary>
        private string _dataPath;

        /// <summary>
        /// 消息列表
        /// </summary>
        private List<MessageData> _messages;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MessagePersistenceManager()
        {
            // 获取应用程序执行目录
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // 创建data文件夹
            string dataDirectory = Path.Combine(appDirectory, "data");
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            // 设置数据文件路径
            _dataPath = Path.Combine(dataDirectory, MESSAGE_FILE_NAME);

            // 加载消息数据
            _messages = LoadMessages();
        }

        /// <summary>
        /// 加载消息数据
        /// </summary>
        /// <returns>消息列表</returns>
        public List<MessageData> LoadMessages()
        {
            try
            {
                // 检查文件是否存在
                if (!File.Exists(_dataPath))
                {
                    return new List<MessageData>();
                }

                // 读取文件内容
                string jsonContent = File.ReadAllText(_dataPath);

                // 反序列化JSON数据
                var messageData = JsonConvert.DeserializeObject<MessagePersistenceData>(jsonContent);

                // 如果数据版本不匹配，返回空列表
                if (messageData?.Version != DATA_VERSION)
                {
                    return new List<MessageData>();
                }

                return messageData.Messages ?? new List<MessageData>();
            }
            catch (Exception ex)
            {
                // 如果读取或反序列化失败，返回空列表
                DebugHelper.WriteLine($"加载消息数据失败: {ex.Message}");
                return new List<MessageData>();
            }
        }

        /// <summary>
        /// 保存消息数据
        /// </summary>
        /// <param name="messages">消息列表</param>
        public void SaveMessages(List<MessageData> messages)
        {
            try
            {
                // 创建持久化数据对象
                var messageData = new MessagePersistenceData
                {
                    Version = DATA_VERSION,
                    LastUpdated = DateTime.Now,
                    Messages = messages
                };

                // 序列化JSON数据，忽略null值属性以节省存储空间
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                string jsonContent = JsonConvert.SerializeObject(messageData, Formatting.Indented, jsonSettings);

                // 写入文件
                File.WriteAllText(_dataPath, jsonContent);

                // 更新内存中的消息列表
                _messages = messages;
            }
            catch (Exception ex)
            {
                // 如果保存失败，记录错误
                DebugHelper.WriteLine($"保存消息数据失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="message">消息对象</param>
        public void AddMessage(MessageData message)
        {
            if (message.BizData is TodoUpdate todoUpdate)
            {
                // 清除单据数据以节省存储空间
                todoUpdate.entity = null;
            }
            // 添加消息到列表
            _messages.Add(message);

            // 保存到持久化存储
            SaveMessages(_messages);
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="id">消息ID</param>
        public void DeleteMessage(long id)
        {
            // 从列表中移除消息
            _messages.RemoveAll(m => m.MessageId == id);

            // 保存到持久化存储
            SaveMessages(_messages);
        }

        /// <summary>
        /// 删除多条消息
        /// </summary>
        /// <param name="ids">消息ID列表</param>
        public void DeleteMessages(IEnumerable<long> ids)
        {
            // 从列表中移除多条消息
            _messages.RemoveAll(m => ids.Contains(m.MessageId));

            // 保存到持久化存储
            SaveMessages(_messages);
        }

        /// <summary>
        /// 更新消息
        /// </summary>
        /// <param name="message">消息对象</param>
        public void UpdateMessage(MessageData message)
        {
            // 查找消息
            int index = _messages.FindIndex(m => m.MessageId == message.MessageId);

            // 如果找到消息，更新它
            if (index != -1)
            {
                _messages[index] = message;

                // 保存到持久化存储
                SaveMessages(_messages);
            }
        }

        /// <summary>
        /// 获取所有消息
        /// </summary>
        /// <returns>消息列表</returns>
        public List<MessageData> GetAllMessages()
        {
            return _messages;
        }

        /// <summary>
        /// 根据ID获取消息
        /// </summary>
        /// <param name="id">消息ID</param>
        /// <returns>消息对象</returns>
        public MessageData GetMessageById(long id)
        {
            return _messages.FirstOrDefault(m => m.MessageId == id);
        }

 

        /// <summary>
        /// 清除所有消息
        /// </summary>
        public void ClearAllMessages()
        {
            try
            {
                // 清空内存中的消息列表
                _messages.Clear();
                
                // 清空持久化文件
                var persistenceData = new MessagePersistenceData
                {
                    Version = DATA_VERSION,
                    LastUpdated = DateTime.Now,
                    Messages = new List<MessageData>()
                };
                
                // 序列化JSON数据，忽略null值属性以节省存储空间
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                string jsonContent = JsonConvert.SerializeObject(persistenceData, Formatting.Indented, jsonSettings);
                File.WriteAllText(_dataPath, jsonContent);

                DebugHelper.WriteLine("已成功清除所有消息");
            }
            catch (Exception ex)
            {
                DebugHelper.WriteLine($"清除所有消息时发生错误: {ex.Message}");
                throw new Exception($"清除所有消息失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 消息持久化数据结构
        /// </summary>
        private class MessagePersistenceData
        {
            /// <summary>
            /// 数据版本号
            /// </summary>
            [JsonProperty("version")]
            public string Version { get; set; }

            /// <summary>
            /// 最后更新时间
            /// </summary>
            [JsonProperty("lastUpdated")]
            public DateTime LastUpdated { get; set; }

            /// <summary>
            /// 消息列表
            /// </summary>
            [JsonProperty("messages")]
            public List<MessageData> Messages { get; set; }
        }
    }
}