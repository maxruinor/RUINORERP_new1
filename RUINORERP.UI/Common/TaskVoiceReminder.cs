using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Speech.Synthesis;
using System.Timers;
using RUINORERP.Model.TransModel;

namespace RUINORERP.UI.Common
{

    /// <summary>
    /// 服务器消息语音提醒工具（基于System.Speech）
    /// </summary>
    public class SystemSpeechVoiceReminder : IVoiceReminder
    {
        // 1. 语音合成器实例（延长生命周期，避免异步播放时被释放）
        private readonly SpeechSynthesizer _synthesizer;

        // 2. 线程安全的消息队列（处理多条消息，避免重叠播放）
        private readonly ConcurrentQueue<string> _remindMessageQueue;

        // 3. 标记是否正在播放（控制队列消费，保证顺序播放）
        private bool _isPlaying = false;
        
        // 4. 标记语音合成器是否初始化成功
        private bool _isSynthesizerInitialized = false;

        // 5. 语音提醒配置
        /// <summary>
        /// 是否启用语音提醒
        /// </summary>
        public bool IsEnabled { get; set; } = true;
        
        /// <summary>
        /// 音量（0-100）
        /// </summary>
        public int Volume { get; set; } = 100;
        
        /// <summary>
        /// 语速（-10到10）
        /// </summary>
        public int Rate { get; set; } = 0;
        
        /// <summary>
        /// 语音性别
        /// </summary>
        public VoiceGender VoiceGender { get; set; } = VoiceGender.Female;
        
        // 6. 消息去重机制
        private readonly ConcurrentDictionary<string, DateTime> _recentMessages;
        private readonly Timer _recentMessagesCleanupTimer;
        
        /// <summary>
        /// 语音提醒类型名称
        /// </summary>
        public string ReminderTypeName => "System.Speech";
        
        /// <summary>
        /// 检查系统是否支持System.Speech.Synthesis
        /// </summary>
        /// <returns>是否支持</returns>
        public static bool IsSystemSpeechSupported()
        {
            try
            {
                using (var synthesizer = new SpeechSynthesizer())
                {
                    return synthesizer.GetInstalledVoices().Any();
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 构造函数：初始化语音合成器和队列
        /// </summary>
        public SystemSpeechVoiceReminder()
        {
            try
            {
                // 初始化语音合成器
                _synthesizer = new SpeechSynthesizer();

                // 配置语音参数
                _synthesizer.Volume = Volume;
                _synthesizer.Rate = Rate;
                try
                {
                    // 尝试选择女声，若系统无女声则使用默认语音（避免报错）
                    _synthesizer.SelectVoiceByHints(VoiceGender, VoiceAge.Adult);
                }
                catch
                {
                    Console.WriteLine("未找到指定语音包，将使用系统默认语音");
                }

                // 绑定异步播放完成事件（播放完成后消费下一条消息）
                _synthesizer.SpeakCompleted += Synthesizer_SpeakCompleted;
                
                // 标记语音合成器初始化成功
                _isSynthesizerInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"语音合成器初始化失败：{ex.Message}");
                // 语音合成器初始化失败时，保持默认值false
                _isSynthesizerInitialized = false;
                
                // 初始化一个空的语音合成器实例，避免后续调用时出现空引用异常
                _synthesizer = null;
            }

            // 初始化线程安全队列（支持多线程添加消息，如服务器异步接收消息后入队）
            _remindMessageQueue = new ConcurrentQueue<string>();
            
            // 初始化消息去重字典
            _recentMessages = new ConcurrentDictionary<string, DateTime>();
            
            // 初始化清理定时器，每5分钟清理一次过期消息
            _recentMessagesCleanupTimer = new Timer(300000); // 5分钟
            _recentMessagesCleanupTimer.Elapsed += CleanupRecentMessages;
            _recentMessagesCleanupTimer.AutoReset = true;
            _recentMessagesCleanupTimer.Start();
        }

        /// <summary>
        /// 清理过期的消息记录
        /// </summary>
        private void CleanupRecentMessages(object sender, ElapsedEventArgs e)
        {
            try
            {
                // 清理10分钟前的消息记录
                DateTime cutoffTime = DateTime.Now.AddMinutes(-10);
                var expiredKeys = _recentMessages.Where(kv => kv.Value < cutoffTime).Select(kv => kv.Key).ToList();
                
                foreach (var key in expiredKeys)
                {
                    _recentMessages.TryRemove(key, out _);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"清理过期消息失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 根据消息数据生成语音文本
        /// </summary>
        /// <param name="messageData">消息数据对象</param>
        /// <returns>生成的语音文本</returns>
        private string GenerateVoiceText(MessageData messageData)
        {
            if (messageData == null)
            {
                return string.Empty;
            }

            // 根据消息类型生成不同的语音提示文本
            string voiceText = messageData.MessageType switch
            {
                MessageType.Popup => $"弹出消息：{messageData.Title}",
                MessageType.Business => $"业务消息：{messageData.Title}",
                MessageType.System => $"系统通知：{messageData.Title}",
                _ => $"您有一条新消息：{messageData.Title}"
            };

            return voiceText;
        }

        /// <summary>
        /// 添加提醒消息（供服务器接收消息后调用，支持多线程调用）
        /// </summary>
        /// <param name="messageContent">服务器返回的提醒消息内容</param>
        public void AddRemindMessage(string messageContent)
        {
            if (string.IsNullOrWhiteSpace(messageContent))
            {
                Console.WriteLine("提醒消息内容不能为空");
                return;
            }

            // 检查语音提醒是否启用
            if (!IsEnabled)
            {
                return;
            }
            
            // 检查语音合成器是否初始化成功
            if (!_isSynthesizerInitialized)
            {
                Console.WriteLine($"语音合成器未初始化，已跳过消息播放：{messageContent}");
                return;
            }

            // 检查是否为重复消息（10分钟内）
            if (_recentMessages.TryGetValue(messageContent, out DateTime lastPlayTime))
            {
                if (DateTime.Now - lastPlayTime < TimeSpan.FromMinutes(10))
                {
                    // 10分钟内已播放过相同消息，跳过
                    return;
                }
            }

            // 更新消息播放时间
            _recentMessages[messageContent] = DateTime.Now;

            // 消息入队
            _remindMessageQueue.Enqueue(messageContent);
            Console.WriteLine($"消息已入队：{messageContent}，当前队列长度：{_remindMessageQueue.Count}");

            // 尝试消费队列（若未在播放，则立即开始播放；若正在播放，等待播放完成后自动消费）
            TryPlayNextMessage();
        }

        /// <summary>
        /// 尝试播放队列中的下一条消息
        /// </summary>
        private void TryPlayNextMessage()
        {
            // 双重判断：避免多线程同时触发播放
            if (_isPlaying || _remindMessageQueue.IsEmpty)
            {
                return;
            }
            
            // 检查语音合成器是否初始化成功
            if (!_isSynthesizerInitialized || _synthesizer == null)
            {
                // 清空队列，避免消息堆积
                while (_remindMessageQueue.TryDequeue(out _)) { }
                Console.WriteLine("语音合成器未初始化，已清空消息队列");
                return;
            }

            // 出队一条消息
            if (_remindMessageQueue.TryDequeue(out string message))
            {
                try
                {
                    _isPlaying = true;
                    Console.WriteLine($"开始异步播放：{message}");
                    // 更新语音参数（支持动态调整）
                    _synthesizer.Volume = Volume;
                    _synthesizer.Rate = Rate;
                    // 异步播放（不阻塞当前线程，适合服务器业务处理）
                    _synthesizer.SpeakAsync(message);
                }
                catch (Exception ex)
                {
                    _isPlaying = false;
                    Console.WriteLine($"播放消息失败：{ex.Message}");
                    // 播放失败后，继续尝试播放下一条
                    TryPlayNextMessage();
                }
            }
        }

        /// <summary>
        /// 异步播放完成回调事件
        /// </summary>
        private void Synthesizer_SpeakCompleted(object? sender, SpeakCompletedEventArgs e)
        {
            _isPlaying = false;
            if (e.Error != null)
            {
                Console.WriteLine($"异步播放出现异常：{e.Error.Message}");
            }
            else
            {
                Console.WriteLine("当前消息播放完成");
            }

            // 自动消费队列中的下一条消息
            TryPlayNextMessage();
        }

        /// <summary>
        /// 添加提醒消息（基于MessageData对象，接口实现）
        /// </summary>
        /// <param name="messageData">消息数据对象</param>
        public void AddRemindMessage(MessageData messageData)
        {
            if (messageData == null)
            {
                Console.WriteLine("提醒消息对象不能为空");
                return;
            }

            string voiceText = GenerateVoiceText(messageData);
            AddRemindMessage(voiceText);
        }

        /// <summary>
        /// 检查系统是否支持语音功能
        /// </summary>
        /// <returns>是否支持语音</returns>
        public bool IsSupported()
        {
            try
            {
                using (var synthesizer = new System.Speech.Synthesis.SpeechSynthesizer())
                {
                    return synthesizer.GetInstalledVoices().Any();
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 释放资源（程序退出时调用）
        /// </summary>
        public void Dispose()
        {
            // 停止并释放清理定时器
            if (_recentMessagesCleanupTimer != null)
            {
                _recentMessagesCleanupTimer.Stop();
                _recentMessagesCleanupTimer.Dispose();
            }
            
            // 安全释放语音合成器资源
            if (_isSynthesizerInitialized && _synthesizer != null)
            {
                try
                {
                    _synthesizer.SpeakAsyncCancelAll(); // 取消所有未完成的异步播放
                    _synthesizer.SpeakCompleted -= Synthesizer_SpeakCompleted;
                    _synthesizer.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"释放语音合成器资源失败：{ex.Message}");
                }
            }
            
            Console.WriteLine("语音提醒工具资源已释放");
        }
    }
 
    /// <summary>
    /// Windows内置TTS语音提醒实现（作为System.Speech的备选方案）
    /// </summary>
    public class WindowsTtsVoiceReminder : IVoiceReminder
    {
        // 语音提醒配置
        /// <summary>
        /// 是否启用语音提醒
        /// </summary>
        public bool IsEnabled { get; set; } = true;
        
        // 标记是否初始化成功
        private bool _isInitialized = false;
        
        /// <summary>
        /// 语音提醒类型名称
        /// </summary>
        public string ReminderTypeName => "Windows TTS";
        
        /// <summary>
        /// 构造函数：初始化Windows TTS语音服务
        /// </summary>
        public WindowsTtsVoiceReminder()
        {
            try
            {
                // 检查Windows语音服务是否可用
                _isInitialized = CheckWindowsTtsAvailability();
                if (_isInitialized)
                {
                    Console.WriteLine("Windows TTS语音服务初始化成功");
                }
                else
                {
                    Console.WriteLine("Windows TTS语音服务不可用");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Windows TTS语音服务初始化失败：{ex.Message}");
                _isInitialized = false;
            }
        }
        
        /// <summary>
        /// 检查Windows TTS语音服务是否可用
        /// </summary>
        /// <returns>是否可用</returns>
        private bool CheckWindowsTtsAvailability()
        {
            try
            {
                // 尝试创建SpeechSynthesizer实例来检测Windows TTS是否可用
                using (var synthesizer = new SpeechSynthesizer())
                {
                    return synthesizer.GetInstalledVoices().Any();
                }
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 添加提醒消息
        /// </summary>
        /// <param name="messageContent">消息内容</param>
        public void AddRemindMessage(string messageContent)
        {
            if (!IsEnabled || !_isInitialized)
            {
                return;
            }
            
            try
            {
                using (var synthesizer = new SpeechSynthesizer())
                {
                    synthesizer.SpeakAsync(messageContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Windows TTS语音播放失败：{ex.Message}");
            }
        }
        
        /// <summary>
        /// 添加提醒消息（基于MessageData对象）
        /// </summary>
        /// <param name="messageData">消息数据对象</param>
        public void AddRemindMessage(MessageData messageData)
        {
            if (messageData == null)
            {
                return;
            }
            
            string voiceText = messageData.MessageType switch
            {
                MessageType.Popup => $"弹出消息：{messageData.Title}",
                MessageType.Business => $"业务消息：{messageData.Title}",
                MessageType.System => $"系统通知：{messageData.Title}",
                _ => $"您有一条新消息：{messageData.Title}"
            };
            
            AddRemindMessage(voiceText);
        }
        
        /// <summary>
        /// 检查系统是否支持语音功能
        /// </summary>
        /// <returns>是否支持</returns>
        public bool IsSupported()
        {
            return _isInitialized;
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            // 无需释放资源
        }
    }
}

