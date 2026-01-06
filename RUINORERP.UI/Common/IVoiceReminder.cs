using RUINORERP.PacketSpec.Models.Message;
using System;


namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 语音提醒服务接口
    /// </summary>
    public interface IVoiceReminder : IDisposable
    {
        /// <summary>
        /// 是否启用语音提醒
        /// </summary>
        bool IsEnabled { get; set; }
        
        /// <summary>
        /// 添加提醒消息
        /// </summary>
        /// <param name="messageContent">消息内容</param>
        void AddRemindMessage(string messageContent);
        
        /// <summary>
        /// 添加提醒消息（基于MessageData对象）
        /// </summary>
        /// <param name="messageData">消息数据对象</param>
        void AddRemindMessage(MessageData messageData);
        
        /// <summary>
        /// 检查系统是否支持语音功能
        /// </summary>
        /// <returns>是否支持语音</returns>
        bool IsSupported();
    }
}