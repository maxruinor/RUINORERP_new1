using RUINORERP.Global.EnumExt;
using RUINORERP.PacketSpec.Models.Message;
using System;


namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 空语音提醒实现 - 用于不支持System.Speech的系统
    /// </summary>
    public class NullVoiceReminder : IVoiceReminder
    {
        /// <summary>
        /// 是否启用语音提醒
        /// </summary>
        public bool IsEnabled { get; set; } = true;
        /// <summary>
        /// 添加提醒消息（空实现）
        /// </summary>
        /// <param name="messageContent">消息内容</param>
        public void AddRemindMessage(string messageContent)
        {
            // 记录日志但不执行任何语音播放操作
            Console.WriteLine($"[语音提醒] 系统不支持语音功能，已跳过播放：{messageContent}");
        }
        
        /// <summary>
        /// 添加提醒消息（基于MessageData对象，空实现）
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
        /// 生成语音文本
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
        /// 检查系统是否支持语音功能
        /// </summary>
        /// <returns>总是返回false</returns>
        public bool IsSupported()
        {
            return false;
        }
        
        /// <summary>
        /// 释放资源（空实现）
        /// </summary>
        public void Dispose()
        {
            // 无需释放任何资源
            Console.WriteLine("空语音提醒资源已释放");
        }
    }
}