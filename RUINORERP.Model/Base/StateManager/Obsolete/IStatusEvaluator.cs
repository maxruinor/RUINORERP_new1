using RUINORERP.Model.StatusManage;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base.StateManager
{
    // 状态评估策略接口
    /// <summary>
    /// 应该要有方法显示状态
    /// </summary>
    public interface IStatusEvaluator
    {
        /// <summary>
        /// 获取当前业务状态值（根据不同类型返回对应枚举）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        Enum CurrentStatus { get; set; }
        /// <summary>
        /// 获取当前审批结果
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        bool ApprovalResult { get; set; }

        /// <summary>
        /// 状态变更事件
        /// </summary>
        event EventHandler<StatusChangedEventArgs> StatusChanged;

    }
}
