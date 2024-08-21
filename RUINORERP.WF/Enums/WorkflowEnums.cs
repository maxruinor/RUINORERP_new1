using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF.Enums
{
    /// <summary>
    /// 一种是开发人员后台编码定义固定流程另一种是用户通过流程设计器实现自定义业务流程。
    /// </summary>
    public enum WFType
    {
        [Description("固定流程")]
        Fixed = 0,


        [Description("自定义流程")]
        Customized = 1
    }


    //一个工作流服务能不能 弄一个 普通的。一个重要的? 一个不用保存到数据库。
    //有普通提醒的，有普通推送的，有事件型的。

    /// <summary>
    /// 
    /// </summary>
    public enum WorkflowStatus
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Active = 1
    }


    public enum WorkflowType
    {
        /// <summary>
        /// 
        /// </summary>
        定时推送 = 0,
        /// <summary>
        /// 
        /// </summary>
        动作触发 = 1
    }

    /// <summary>
    /// 工作流分支判断条件的类型
    /// </summary>
    public enum WorkflowConditionType
    {
        [Description("金额")]
        AmountThreshold = 0,

        [Description("提交人")]
        Submitter = 1,

        [Description("审核人")]
        Approver = 2,
    }

    /// <summary>
    /// 工作流业务类型
    /// </summary>
    public enum WorkflowBizType
    {
        销售流程,
        采购流程,
        生产流程,
        退货流程,
    }
}
