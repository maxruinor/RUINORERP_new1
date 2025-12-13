/**
 * 文件: StatusType.cs
 * 说明: 状态类型枚举 - V4版本支持DataStatus与业务状态互斥关系
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 更新日期: 2025-01-12 - V4版本合并Data和Business为同级互斥状态
 */

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态类型枚举 - V4版本支持DataStatus与业务状态同级互斥关系
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// 主要状态类型 - DataStatus与业务状态互斥，一个实体只能使用一种主要状态类型
        /// </summary>
        Primary = 1,

        /// <summary>
        /// 操作性状态 - 可与其他状态共存
        /// </summary>
        Action = 2,
        
        /// <summary>
        /// 审核状态 - 可与其他状态共存
        /// </summary>
        Approval = 3,
        
        /// <summary>
        /// 审核结果 - 可与其他状态共存
        /// </summary>
        ApprovalResult = 4
    }
}