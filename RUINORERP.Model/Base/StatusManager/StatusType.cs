/**
 * 文件: StatusType.cs
 * 说明: 状态类型枚举
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态类型枚举
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// 数据性状态
        /// </summary>
        Data = 1,

        /// <summary>
        /// 操作性状态
        /// </summary>
        Action = 2,

        /// <summary>
        /// 业务性状态
        /// </summary>
        Business = 3
    }
}