using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace RUINORERP.Model.Base
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
        /// </summary>
        event EventHandler<StatusChangedEventArgs> StatusChanged;
        
    }


    // 基础业务状态实现
    public abstract class BaseStatusEvaluator<TDataStatus> : IStatusProvider
        where TDataStatus : Enum
    {
        private TDataStatus _dataStatus;
        private bool _approvalResult;

        public TDataStatus DataStatus
        {
            get => _dataStatus;
            set
            {
                if (!Equals(_dataStatus, value))
                {
                    var old = _dataStatus;
                    _dataStatus = value;
                    OnStatusChanged(nameof(DataStatus), old, value);
                }
            }
        }


        public bool ApprovalResult
        {
            get => _approvalResult;
            set
            {
                if (_approvalResult != value)
                {
                    _approvalResult = value;
                    OnStatusChanged(nameof(ApprovalResult), !value, value);
                }
            }
        }

        public event EventHandler<StatusChangedEventArgs> StatusChanged;
        public Enum CurrentStatus { get; set; }

        protected virtual void OnStatusChanged(string propertyName, object oldValue, object newValue)
        {
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(
                propertyName,
                oldValue,
                newValue
            ));
        }

       
    }

    //// 具体业务状态提供器实现
    ////业务单据的
    //public class BusinessStatusProvider : BaseStatusEvaluator<DataStatus>
    //{
    //    // 可添加业务单据特有逻辑
    //}

    /// <summary>
    /// =========================================下面只是示例
    /// </summary>

    // 财务模块状态枚举
    public enum FinancialDataStatus
    {
        草稿,
        已提交,
        已核销,
        已取消
    }

    public enum FinancialApprovalStatus
    {
        未审核,
        已审核,
        驳回
    }



    // 财务模块状态提供器
    public class FinancialStatusProvider : BaseStatusEvaluator<FinancialDataStatus>
    {

        // 添加财务特有逻辑
        public decimal SettledAmount { get; set; }

        protected override void OnStatusChanged(string propertyName, object oldValue, object newValue)
        {
            if (propertyName == nameof(DataStatus))
            {
                // 财务模块特殊处理逻辑
            }
            base.OnStatusChanged(propertyName, oldValue, newValue);
        }
    }

    // 财务模块特有状态逻辑
    //public decimal SettledAmount { get; set; }

    //protected override void OnStatusChanged(string propertyName, object oldValue, object newValue)
    //{
    //    // 财务模块特有状态变更逻辑
    //    if (propertyName == nameof(DataStatus))
    //    {
    //        // 处理特殊状态转换逻辑
    //    }
    //    base.OnStatusChanged(propertyName, oldValue, newValue);
    //}


    //public interface IStatusProvider
    //{
    //    DataStatus? DataStatus { get; set; }
    //    ApprovalStatus? ApprovalStatus { get; set; }
    //    bool? ApprovalResult { get; set; }

    //    // 财务模块特殊状态
    //    PaymentStatus? PaymentStatus { get; set; }
    //    ARAPStatus? ARAPStatus { get; set; }

    //    // 通用方法
    //    bool HasStatusField(string fieldName);
    //    object GetStatusValue(string fieldName);
    //    void SetStatusValue(string fieldName, object value);
    //}

}
