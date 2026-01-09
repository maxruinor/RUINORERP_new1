using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.EnumExt
{
    /// <summary>
    /// 提醒源和提醒目标类型枚举
    /// </summary>
    public enum SourceTargetType
    {
        /// <summary>
        /// 角色
        /// </summary>
        [Description("角色")]
        角色 = 1,

        /// <summary>
        /// 人员
        /// </summary>
        [Description("人员")]
        人员 = 2,

        /// <summary>
        /// 部门
        /// </summary>
        [Description("部门")]
        部门 = 3
    }



    public enum ActionType
    {
        提交 = 1,
        审核 = 2,
        反审核 = 3
    }
}