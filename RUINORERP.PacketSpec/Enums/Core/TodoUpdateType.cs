using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.Core
{
    /// <summary>
    /// 任务状态更新类型
    /// </summary>
    public enum TodoUpdateType
    {
        /// <summary>
        /// 任务创建
        /// </summary>
        [Description("任务创建")]
        Created,

        /// <summary>
        /// 任务状态变更
        /// </summary>
        [Description("任务状态变更")]
        StatusChanged,

        /// <summary>
        /// 任务审核
        /// </summary>
        [Description("任务审核")]
        Approved,

        /// <summary>
        /// 任务删除
        /// </summary>
        [Description("任务删除")]
        Deleted,

    }
}
