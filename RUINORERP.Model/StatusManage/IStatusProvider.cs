using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.StatusManage
{
    // 状态管理核心接口
    public interface IStatusProvider
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
        /// 【已过时】请使用新的状态管理体系 - 参见RUINORERP.Model.Base.StateManager命名空间
        /// 替代方案：使用IStatusTransitionContext.OnStatusChanged事件
        /// </summary>
        [Obsolete("请使用新的状态管理体系 - 参见RUINORERP.Model.Base.StateManager命名空间。替代方案：使用IStatusTransitionContext.OnStatusChanged事件")]
        event EventHandler<StatusChangedEventArgs> StatusChanged;

    }

    /// <summary>
    /// 状态变更事件参数
    /// </summary>
    public class StatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 旧值
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// 新值
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// 实体对象
        /// </summary>
        public object Entity { get; set; }

        /// <summary>
        /// 来源状态
        /// </summary>
        public Enum FromStatus { get; set; }

        /// <summary>
        /// 目标状态
        /// </summary>
        public Enum ToStatus { get; set; }

        /// <summary>
        /// 变更原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 状态类型
        /// </summary>
        public Type StateType { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public StatusChangedEventArgs()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        public StatusChangedEventArgs(string propertyName, object oldValue, object newValue)
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
