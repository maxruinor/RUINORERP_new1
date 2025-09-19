using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.File
{

    /// <summary>
    /// 过滤器状态枚举
    /// </summary>
    public enum FilterState
    {
        /// <summary>
        /// 准备就绪
        /// </summary>
        Ready,

        /// <summary>
        /// 处理中
        /// </summary>
        Processing,

        /// <summary>
        /// 错误状态
        /// </summary>
        Error,

        /// <summary>
        /// 完成状态
        /// </summary>
        Completed
    }


}
