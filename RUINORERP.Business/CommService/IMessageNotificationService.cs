using RUINORERP.Global.EnumExt;
using RUINORERP.PacketSpec.Models.Message;
using System;
using System.Threading.Tasks;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 消息通知服务接口
    /// 用于业务层向消息服务发送通知,实现业务层与UI层的解耦
    /// </summary>
    public interface IMessageNotificationService
    {
        /// <summary>
        /// 发送消息数据到指定接收者
        /// </summary>
        /// <param name="messageData">消息数据</param>
        /// <returns>发送结果</returns>
        Task<bool> SendMessageAsync(MessageData messageData);

        /// <summary>
        /// 发送消息到指定用户
        /// </summary>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="title">消息标题</param>
        /// <param name="content">消息内容</param>
        /// <param name="messageType">消息类型</param>
        /// <returns>发送结果</returns>
        Task<bool> SendMessageToUserAsync(long targetUserId, string title, string content, MessageType messageType = MessageType.Business);

        /// <summary>
        /// 批量发送消息到多个用户
        /// </summary>
        /// <param name="targetUserIds">目标用户ID列表</param>
        /// <param name="title">消息标题</param>
        /// <param name="content">消息内容</param>
        /// <param name="messageType">消息类型</param>
        /// <returns>发送结果</returns>
        Task<bool> SendMessageToUsersAsync(long[] targetUserIds, string title, string content, MessageType messageType = MessageType.Business);
    }
}
