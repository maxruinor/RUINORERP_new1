using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Enums.Auth;
using RUINORERP.PacketSpec.Enums.Cache;
using RUINORERP.PacketSpec.Enums.Message;
using RUINORERP.PacketSpec.Enums.Workflow;
using RUINORERP.PacketSpec.Enums.Exception;
using RUINORERP.PacketSpec.Enums.File;
using RUINORERP.PacketSpec.Enums.DataSync;
using RUINORERP.PacketSpec.Enums.Lock;
using RUINORERP.PacketSpec.Enums.SystemManagement;

namespace RUINORERP.PacketSpec.Enums
{
    /// <summary>
    /// 统一命令辅助类
    /// 提供跨模块枚举操作的统一接口
    /// </summary>
    public static class CommandHelper
    {
        /// <summary>
        /// 命令类别枚举
        /// </summary>
        public enum CommandCategory : byte
        {
            /// <summary>
            /// 系统命令
            /// </summary>
            [Description("系统命令")]
            System = 0x00,

            /// <summary>
            /// 认证命令
            /// </summary>
            [Description("认证命令")]
            Authentication = 0x01,

            /// <summary>
            /// 缓存命令
            /// </summary>
            [Description("缓存命令")]
            Cache = 0x02,

            /// <summary>
            /// 消息命令
            /// </summary>
            [Description("消息命令")]
            Message = 0x03,

            /// <summary>
            /// 工作流命令
            /// </summary>
            [Description("工作流命令")]
            Workflow = 0x04,

            /// <summary>
            /// 异常处理命令
            /// </summary>
            [Description("异常处理命令")]
            Exception = 0x05,

            /// <summary>
            /// 文件操作命令
            /// </summary>
            [Description("文件操作命令")]
            File = 0x06,

            /// <summary>
            /// 数据同步命令
            /// </summary>
            [Description("数据同步命令")]
            DataSync = 0x07,

            /// <summary>
            /// 锁管理命令
            /// </summary>
            [Description("锁管理命令")]
            Lock = 0x08,

            /// <summary>
            /// 系统管理命令
            /// </summary>
            [Description("系统管理命令")]
            SystemManagement = 0x09,

            /// <summary>
            /// 复合型命令
            /// </summary>
            [Description("复合型命令")]
            Composite = 0x10,

            /// <summary>
            /// 连接管理命令
            /// </summary>
            [Description("连接管理命令")]
            Connection = 0x11,

            /// <summary>
            /// 特殊功能命令
            /// </summary>
            [Description("特殊功能命令")]
            Special = 0x90
        }

        /// <summary>
        /// 获取命令类别
        /// </summary>
        /// <param name="commandValue">命令值</param>
        /// <returns>命令类别</returns>
        public static CommandCategory GetCategory(uint commandValue)
        {
            if (commandValue >= 0x90000)
                return CommandCategory.Special;
            
            var categoryValue = (byte)(commandValue >> 8);
            
            if (Enum.IsDefined(typeof(CommandCategory), categoryValue))
                return (CommandCategory)categoryValue;
            
            return CommandCategory.System;
        }

        /// <summary>
        /// 获取枚举描述（泛型方法）
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumValue">枚举值</param>
        /// <returns>描述信息</returns>
        public static string GetDescription<T>(T enumValue) where T : Enum
        {
            return enumValue.GetDescription();
        }

        /// <summary>
        /// 判断是否为客户端命令
        /// </summary>
        /// <param name="commandValue">命令值</param>
        /// <returns>是否为客户端命令</returns>
        public static bool IsClientCommand(uint commandValue)
        {
            var category = GetCategory(commandValue);
            return category switch
            {
                CommandCategory.Authentication => IsClientAuthCommand(commandValue),
                CommandCategory.Cache => IsClientCacheCommand(commandValue),
                CommandCategory.Message => IsClientMessageCommand(commandValue),
                CommandCategory.Workflow => IsClientWorkflowCommand(commandValue),
                CommandCategory.Exception => IsClientExceptionCommand(commandValue),
                CommandCategory.File => IsClientFileCommand(commandValue),
                CommandCategory.Composite => IsClientCompositeCommand(commandValue),
                _ => false
            };
        }

        /// <summary>
        /// 判断是否为服务器命令
        /// </summary>
        /// <param name="commandValue">命令值</param>
        /// <returns>是否为服务器命令</returns>
        public static bool IsServerCommand(uint commandValue)
        {
            return !IsClientCommand(commandValue);
        }

        #region 客户端命令判断方法

        private static bool IsClientAuthCommand(uint commandValue)
        {
            return commandValue switch
            {
                (uint)AuthenticationCommand.Login => true,
                (uint)AuthenticationCommand.LoginRequest => true,
                (uint)AuthenticationCommand.ForceLogin => true,
                _ => false
            };
        }

        private static bool IsClientCacheCommand(uint commandValue)
        {
            return commandValue switch
            {
                (uint)CacheCommand.CacheRequest => true,
                (uint)CacheCommand.CacheUpdate => true,
                (uint)CacheCommand.CacheDelete => true,
                (uint)CacheCommand.UpdateDynamicConfig => true,
                _ => false
            };
        }

        private static bool IsClientMessageCommand(uint commandValue)
        {
            return commandValue switch
            {
                (uint)MessageCommand.SendPopupMessage => true,
                _ => false
            };
        }

        private static bool IsClientWorkflowCommand(uint commandValue)
        {
            return commandValue switch
            {
                (uint)WorkflowCommand.WorkflowStart => true,
                (uint)WorkflowCommand.WorkflowCommand => true,
                (uint)WorkflowCommand.WorkflowApproval => true,
                (uint)WorkflowCommand.WorkflowReminderRequest => true,
                (uint)WorkflowCommand.WorkflowReminderChanged => true,
                (uint)WorkflowCommand.WorkflowReminderReply => true,
                _ => false
            };
        }

        private static bool IsClientExceptionCommand(uint commandValue)
        {
            return commandValue switch
            {
                (uint)ExceptionCommand.ReportException => true,
                (uint)ExceptionCommand.RequestAssistance => true,
                _ => false
            };
        }

        private static bool IsClientFileCommand(uint commandValue)
        {
            return commandValue switch
            {
                (uint)FileCommand.FileOperation => true,
                (uint)FileCommand.FileUpload => true,
                (uint)FileCommand.FileDownload => true,
                _ => false
            };
        }

        private static bool IsClientCompositeCommand(uint commandValue)
        {
            // 复合型命令需要根据具体业务逻辑判断
            // 这里暂时返回false，需要根据实际业务实现
            return false;
        }

        #endregion

        /// <summary>
        /// 根据命令值获取对应的枚举类型
        /// </summary>
        /// <param name="commandValue">命令值</param>
        /// <returns>枚举类型，如果找不到返回null</returns>
        public static Type GetEnumType(uint commandValue)
        {
            var category = GetCategory(commandValue);
            
            return category switch
            {
                CommandCategory.System => typeof(SystemCommand),
                CommandCategory.Authentication => typeof(AuthenticationCommand),
                CommandCategory.Cache => typeof(CacheCommand),
                CommandCategory.Message => typeof(MessageCommand),
                CommandCategory.Workflow => typeof(WorkflowCommand),
                CommandCategory.Exception => typeof(ExceptionCommand),
                CommandCategory.File => typeof(FileCommand),
                CommandCategory.DataSync => typeof(DataSyncCommand),
                CommandCategory.Lock => typeof(LockCommand),
                CommandCategory.SystemManagement => typeof(SystemManagementCommand),
                _ => null
            };
        }

        /// <summary>
        /// 根据命令值获取枚举名称
        /// </summary>
        /// <param name="commandValue">命令值</param>
        /// <returns>枚举名称，如果找不到返回空字符串</returns>
        public static string GetEnumName(uint commandValue)
        {
            var enumType = GetEnumType(commandValue);
            if (enumType == null)
                return string.Empty;

            return Enum.GetName(enumType, commandValue) ?? string.Empty;
        }

        /// <summary>
        /// 转换命令值为对应的枚举值
        /// </summary>
        /// <typeparam name="T">目标枚举类型</typeparam>
        /// <param name="commandValue">命令值</param>
        /// <returns>枚举值</returns>
        /// <exception cref="ArgumentException">当命令值不属于目标枚举类型时抛出</exception>
        public static T ConvertToEnum<T>(uint commandValue) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), commandValue))
                return (T)Enum.ToObject(typeof(T), commandValue);
            
            throw new ArgumentException($"命令值 {commandValue} 不属于枚举类型 {typeof(T).Name}");
        }
    }
}