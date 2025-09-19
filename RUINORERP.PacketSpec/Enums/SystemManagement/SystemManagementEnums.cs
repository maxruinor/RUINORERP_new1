using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.SystemManagement
{
    /// <summary>
    /// 系统管理命令枚举
    /// </summary>
    public enum SystemManagementCommand : uint
    {
        /// <summary>
        /// 推送版本更新
        /// </summary>
        [Description("推送版本更新")]
        PushVersionUpdate = 0x0900,

        /// <summary>
        /// 切换服务器
        /// </summary>
        [Description("切换服务器")]
        SwitchServer = 0x0901,

        /// <summary>
        /// 关机
        /// </summary>
        [Description("关机")]
        Shutdown = 0x0902,

        /// <summary>
        /// 删除列配置文件
        /// </summary>
        [Description("删除列配置文件")]
        DeleteColumnConfig = 0x0903
    }
}