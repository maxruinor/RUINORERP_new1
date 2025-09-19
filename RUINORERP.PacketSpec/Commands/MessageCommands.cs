using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 消息相关命令
    /// </summary>
    public static class MessageCommands
    {
        #region 消息命令 (0x03xx)
        /// <summary>
        /// 发送弹窗消息 - 向指定用户发送弹窗消息
        /// </summary>
        public static readonly CommandId SendPopupMessage = new CommandId(CommandCategory.Message, 0x00);
        
        /// <summary>
        /// 转发弹窗消息 - 转发弹窗消息给其他用户
        /// </summary>
        public static readonly CommandId ForwardPopupMessage = new CommandId(CommandCategory.Message, 0x01);
        
        /// <summary>
        /// 消息响应 - 对消息的响应
        /// </summary>
        public static readonly CommandId MessageResponse = new CommandId(CommandCategory.Message, 0x02);
        
        /// <summary>
        /// 转发消息结果 - 转发消息处理结果
        /// </summary>
        public static readonly CommandId ForwardMessageResult = new CommandId(CommandCategory.Message, 0x03);
        
        /// <summary>
        /// 发送用户消息 - 向指定用户发送消息
        /// </summary>
        public static readonly CommandId SendMessageToUser = new CommandId(CommandCategory.Message, 0x04);
        
        /// <summary>
        /// 发送部门消息 - 向指定部门发送消息
        /// </summary>
        public static readonly CommandId SendMessageToDepartment = new CommandId(CommandCategory.Message, 0x05);
        
        /// <summary>
        /// 广播消息 - 向所有用户广播消息
        /// </summary>
        public static readonly CommandId BroadcastMessage = new CommandId(CommandCategory.Message, 0x06);
        
        /// <summary>
        /// 发送系统通知 - 发送系统级别的通知消息
        /// </summary>
        public static readonly CommandId SendSystemNotification = new CommandId(CommandCategory.Message, 0x07);
        
        /// <summary>
        /// 系统消息 - 系统内部消息
        /// </summary>
        public static readonly CommandId SystemMessage = new CommandId(CommandCategory.Message, 0x08);
        
        /// <summary>
        /// 提示消息 - 系统提示信息
        /// </summary>
        public static readonly CommandId NotificationMessage = new CommandId(CommandCategory.Message, 0x09);
        #endregion
    }
}