using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base
{
    // 扩展方法类
    public static class EnumExtensions
    {
        public static bool In<T>(this T value, params T[] values) where T : Enum
        {
            return Array.IndexOf(values, value) >= 0;
        }
    }


    // 业务单据评估器
    [Serializable()]
    public class BusinessStatusEvaluator : IStatusEvaluator
    {

        // 用于存储状态的私有字段
        private Enum _currentStatus;

        // 实现接口的事件
        public event EventHandler<StatusChangedEventArgs> StatusChanged;
        /// <summary>
        /// 实现接口的状态属性（关键监控点）
        /// </summary>
        public Enum CurrentStatus
        {
            get => _currentStatus;
            set
            {
                // 仅当值变化时触发事件
                if (!Equals(_currentStatus, value))
                {
                    var oldStatus = _currentStatus;
                    _currentStatus = value;
                    // 触发状态变更事件（使用临时变量避免多线程问题）
                    StatusChanged?.Invoke(this, new StatusChangedEventArgs(_currentStatus.GetType().Name, oldStatus, value));
                }
            }
        }

        // 实现接口的审批结果属性
        public bool ApprovalResult { get; set; }



        //public ControlState Evaluate(MenuItemEnums operation,
        //                            DataStatus dataStatus,
        //                            bool approvalResult)
        //{
        //    return operation switch
        //    {
        //        MenuItemEnums.新增 => new ControlState
        //        {
        //            Visible = dataStatus == DataStatus.草稿,
        //            Enabled = dataStatus == DataStatus.草稿
        //        },

        //        MenuItemEnums.修改 => new ControlState
        //        {
        //            Visible = dataStatus.In(DataStatus.草稿, DataStatus.新建),
        //            Enabled = dataStatus == DataStatus.草稿 //&&                             approvalStatus == ApprovalStatus.未审核
        //        },

        //        MenuItemEnums.提交 => new ControlState
        //        {
        //            Visible = dataStatus == DataStatus.草稿,
        //            Enabled = dataStatus == DataStatus.草稿// &&                             approvalStatus == ApprovalStatus.未审核
        //        },

        //        // 其他操作状态判断...
        //        _ => DefaultState()
        //    };
        //}
        public ControlState Evaluate(MenuItemEnums operation,
                             DataStatus dataStatus,
                             bool approvalResult)
        {
            switch (operation)
            {
                case MenuItemEnums.新增:
                    return new ControlState
                    {
                        Visible = dataStatus == DataStatus.草稿,
                        Enabled = dataStatus == DataStatus.草稿
                    };

                case MenuItemEnums.修改:
                    return new ControlState
                    {
                        Visible = dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建, // 替代原 In() 扩展方法
                        Enabled = dataStatus == DataStatus.草稿 // && approvalStatus == ApprovalStatus.未审核（注释保留）
                    };

                case MenuItemEnums.提交:
                    return new ControlState
                    {
                        Visible = dataStatus == DataStatus.草稿,
                        Enabled = dataStatus == DataStatus.草稿 // && approvalStatus == ApprovalStatus.未审核（注释保留）
                    };

                // 其他操作状态判断...
                default:
                    return DefaultState();
            }
        }
        public string GetStatusText(DataStatus dataStatus, ApprovalStatus approvalStatus)
        {
            return $"{GetDescription(dataStatus)} | {GetDescription(approvalStatus)}";
        }

        private string GetDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            return field?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
        }

        private ControlState DefaultState() => new ControlState { Visible = true, Enabled = true };

        public string GetStatusText(DataStatus dataStatus)
        {
            throw new NotImplementedException();
        }

        public ControlState Evaluate(MenuItemEnums operation)
        {
            throw new NotImplementedException();
        }

    }


    // 财务单据评估器
    [Serializable()]
    public class FinancialStatusEvaluator : IStatusEvaluator
    {
        public Enum CurrentStatus { get; set; }
        public bool ApprovalResult { get; set; }

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        public ControlState Evaluate(MenuItemEnums operation,
                                    FinancialDataStatus dataStatus,
                                    bool approvalResult)
        {
            //return operation switch
            //{
            //    MenuItemEnums.提交 => new ControlState
            //    {
            //        Visible = dataStatus == FinancialDataStatus.草稿,
            //        Enabled = dataStatus == FinancialDataStatus.草稿 &&
            //                 approvalStatus == FinancialApprovalStatus.未审核
            //    },

            //    MenuItemEnums.支付 => new ControlState
            //    {
            //        Visible = dataStatus == FinancialDataStatus.已审核,
            //        Enabled = dataStatus == FinancialDataStatus.已审核 &&
            //                 approvalStatus == FinancialApprovalStatus.已审核
            //    },

            //    // 其他财务操作判断...

            //};

            // 实现财务单据特有状态判断逻辑
            return new ControlState();
            //return DefaultState();


        }

        private ControlState DefaultState()
        {
            throw new NotImplementedException();
        }

        public string GetStatusText(FinancialDataStatus dataStatus, FinancialApprovalStatus approvalStatus)
        {
            //return $"{GetDescription(dataStatus)} | {GetDescription(approvalStatus)}";
            return "要完善";
        }

        public string GetStatusText(FinancialDataStatus dataStatus)
        {
            throw new NotImplementedException();
        }

        public ControlState Evaluate(MenuItemEnums operation)
        {
            throw new NotImplementedException();
        }

        public ControlState Evaluate<TDataStatus>(MenuItemEnums operation)
        {
            throw new NotImplementedException();
        }
    }



    // 基础业务状态实现
    [Serializable()]
    public abstract class BaseStatusEvaluator : IStatusEvaluator
    {
        private bool _approvalResult;
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

        /// <summary>
        /// 根据TDataStatus去转换
        /// </summary>
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

        //ControlState Evaluate<TDataStatus>();

        //string GetStatusText();
    }


    /// <summary>
    /// 状态机接口 应该基于实体的状字段来处理
    /// </summary>
    public interface IStatusMachine
    {
        event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// 单据状态
        /// </summary>
        DataStatus CurrentDataStatus { get; set; }
        ApprovalStatus ApprovalStatus { get; set; }

        /// <summary>
        /// 操作性的？
        /// </summary>
        ActionStatus ActionStatus { get; set; }
        bool ApprovalResult { get; set; }

        bool CanSubmit();
        bool CanModify();
        bool CanDelete();
        bool CanApprove();

        /// <summary>
        /// 可以拒绝
        /// </summary>
        /// <returns></returns>
        bool CanReject();
        bool CanReverseApprove();
        bool CanClose();
        void Create();
        void Submit();
        void Approve();
        void Reject();
        void ReverseApprove();
        void Close();
        //保存草稿
        bool CanSaveAsDraft();

        bool CanReverseClose();

        bool CanCancel();

        void Undo();
        void Modify();
    }

    /// <summary>
    /// 完整的状态机实现，包含所有单据状态转换逻辑
    /// 状态机接口 应该基于实体的状字段来处理
    /// </summary>
    [Serializable()]
    public class BusinessStatusMachine : IStatusMachine
    {

        private readonly StatusHistory _history = new StatusHistory();


        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        //// 设计按钮（报表设计）始终可见但默认禁用
        //MenuItemEnums.设计 => Visible=完结, Enabled=false


        // 审批结果
        public bool ApprovalResult { get; set; }

        private DataStatus _currentDataStatus;
        public DataStatus CurrentDataStatus
        {
            get => _currentDataStatus;
            set
            {
                if (_currentDataStatus == value) return;
                var oldData = _currentDataStatus;
                var oldApproval = ApprovalStatus;
                var oldAction = _actionStatus;
                _currentDataStatus = value;
                OnStatusChanged(oldData, oldApproval, oldAction);
            }
        }

        private ApprovalStatus _approvalStatus;
        public ApprovalStatus ApprovalStatus
        {
            get => _approvalStatus;
            set
            {
                if (_approvalStatus == value) return;
                var oldData = CurrentDataStatus;
                var oldApproval = _approvalStatus;
                var oldAction = _actionStatus;
                _approvalStatus = value;
                OnStatusChanged(oldData, oldApproval, oldAction);
            }
        }


        private ActionStatus _actionStatus;
        public ActionStatus ActionStatus
        {
            get => _actionStatus;
            set
            {
                if (_actionStatus == value) return;
                var oldData = CurrentDataStatus;
                var oldApproval = _approvalStatus;
                var oldAction = _actionStatus;
                _actionStatus = value;
                OnStatusChanged(oldData, oldApproval, oldAction);
            }
        }

        private void OnStatusChanged(DataStatus prevData, ApprovalStatus prevApproval, ActionStatus actionStatus)
        {
            // 在状态变更时自动保存 
            //_repository.SaveStatus(_documentId, CurrentStatus);

            //事件参数不对。要实现一下。
            //StatusChanged?.Invoke(this, new StatusChangedEventArgs(prevData, prevApproval));
        }


        // 通知服务
        private readonly IWorkflowNotificationService _notificationService;


        //public BusinessStatusMachine(
        //   DataStatus initialDataStatus,
        //   bool initialApprovalResult,
        //   IWorkflowNotificationService notificationService)
        //{
        //    CurrentDataStatus = initialDataStatus;
        //    ApprovalResult = initialApprovalResult;
        //    _notificationService = notificationService;
        //}


        public BusinessStatusMachine(
            DataStatus initialDataStatus,
            bool initialApprovalResult, IWorkflowNotificationService notificationService
             )
        {
            CurrentDataStatus = initialDataStatus;
            ApprovalResult = initialApprovalResult;
            _notificationService = notificationService;
        }

        #region 状态判断方法
        public bool CanCreate() => CurrentDataStatus == DataStatus.草稿;

        public bool CanModify()
        {
            return CurrentDataStatus == DataStatus.草稿 ||
                   (CurrentDataStatus == DataStatus.新建 && ApprovalStatus == ApprovalStatus.驳回);
        }

        public bool CanDelete()
        {
            return CurrentDataStatus == DataStatus.草稿 ||
                   CurrentDataStatus == DataStatus.作废;
        }

        public bool CanSubmit()
        {
            return CurrentDataStatus == DataStatus.草稿 &&
                   ApprovalStatus == ApprovalStatus.未审核;
        }

        public bool CanSaveAsDraft()
        {
            return CurrentDataStatus == DataStatus.草稿 ||
                   (CurrentDataStatus == DataStatus.新建 && ApprovalStatus == ApprovalStatus.驳回);
        }




        public bool CanApprove()
        {
            return CurrentDataStatus == DataStatus.新建 &&
                   ApprovalStatus == ApprovalStatus.未审核;
        }

        public bool CanReverseApprove()
        {
            return CurrentDataStatus == DataStatus.确认 &&
                   ApprovalStatus == ApprovalStatus.已审核;
        }

        public bool CanClose()
        {
            return CurrentDataStatus == DataStatus.确认 ||
                   CurrentDataStatus == DataStatus.作废;
        }

        public bool CanReverseClose()
        {
            return CurrentDataStatus == DataStatus.完结;
        }

        public bool CanCancel()
        {
            return CurrentDataStatus != DataStatus.完结 &&
                   CurrentDataStatus != DataStatus.作废;
        }
        #endregion

        #region 状态操作方法
        public void Create()
        {
            if (!CanCreate())
                throw new InvalidOperationException("不能新建单据");

            CurrentDataStatus = DataStatus.草稿;
            ApprovalStatus = ApprovalStatus.未审核;
            ApprovalResult = false;
            OnStatusChange();
        }

        public void Modify()
        {
            if (!CanModify())
                throw new InvalidOperationException("当前状态不能修改");

            // 修改操作不需要改变状态
        }

        public void Delete()
        {
            if (!CanDelete())
                throw new InvalidOperationException("当前状态不能删除");

            CurrentDataStatus = DataStatus.作废;
            ApprovalStatus = ApprovalStatus.未审核;
            ApprovalResult = false;

            _notificationService.NotifyDelete();
        }


        /// <summary>
        /// 触发状态变更通知
        /// </summary>
        protected virtual void OnStatusChange()
        {

            //因为参数不对所以先注释 跑起来
            // StatusChanged?.Invoke(this, new StatusChangedEventArgs(this.CurrentDataStatus, this.ApprovalStatus));
        }
        /// <summary>
        /// 提交的变化
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public void Submit()
        {
            if (!CanSubmit())
                throw new InvalidOperationException("当前状态不能提交");
            try
            {
                _history.Push(CurrentDataStatus, ApprovalStatus, ApprovalResult);
                var prevData = CurrentDataStatus;
                var prevApproval = ApprovalStatus;

                CurrentDataStatus = DataStatus.新建;
                ApprovalStatus = ApprovalStatus.未审核;

                OnStatusChange();
                _notificationService.NotifySubmit();


                //审计功能
                //_auditLogger.Log($"用户[{_user}]执行提交操作");
                //// ...原有逻辑...
                //_auditLogger.Log($"状态变更为:{CurrentStatus}");


                // 启动工作流 !!!!!!!!!!!!!!!!!!!
                //_workflowHost.StartWorkflow("ApprovalWorkflow", new WorkflowData
                //{
                //    DocumentId = _documentId,
                //    CurrentStatus = CurrentDataStatus
                //});
            }
            catch (Exception ex)
            {
                Undo();
                throw new ApplicationException("提交失败，状态已恢复", ex);
            }


        }

        public void Undo()
        {
            if (_history.Count > 0)
            {
                var (data, approval, result) = _history.Pop();
                CurrentDataStatus = data;
                ApprovalStatus = approval;
                ApprovalResult = result;
            }
        }

        public void SaveAsDraft()
        {
            if (!CanSaveAsDraft())
                throw new InvalidOperationException("不能保存为草稿");

            CurrentDataStatus = DataStatus.草稿;
            ApprovalStatus = ApprovalStatus.未审核;
        }

        public void Approve()
        {
            if (!CanApprove())
                throw new InvalidOperationException("当前状态不能审核");

            ApprovalStatus = ApprovalStatus.已审核;
            ApprovalResult = true;
            CurrentDataStatus = DataStatus.确认;
            OnStatusChange();
            _notificationService.NotifyApprove();
        }

        public void Reject()
        {
            if (!CanApprove()) // 驳回和审核的状态条件相同
                throw new InvalidOperationException("当前状态不能驳回");

            ApprovalStatus = ApprovalStatus.驳回;
            ApprovalResult = false;
            CurrentDataStatus = DataStatus.新建;

            _notificationService.NotifyReject();
        }

        public void ReverseApprove()
        {
            if (!CanReverseApprove())
                throw new InvalidOperationException("当前状态不能反审");

            ApprovalStatus = ApprovalStatus.未审核;
            ApprovalResult = false;
            CurrentDataStatus = DataStatus.新建;

            _notificationService.NotifyReverseApprove();
        }

        public void Close()
        {
            if (!CanClose())
                throw new InvalidOperationException("当前状态不能结案");

            CurrentDataStatus = DataStatus.完结;

            _notificationService.NotifyClose();
        }

        public void ReverseClose()
        {
            if (!CanReverseClose())
                throw new InvalidOperationException("当前状态不能反结案");

            CurrentDataStatus = DataStatus.确认;

            _notificationService.NotifyReverseClose();
        }

        public void Cancel()
        {
            if (!CanCancel())
                throw new InvalidOperationException("当前状态不能作废");

            CurrentDataStatus = DataStatus.作废;
            ApprovalStatus = ApprovalStatus.未审核;
            ApprovalResult = false;

            _notificationService.NotifyCancel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        bool IStatusMachine.CanReject()
        {
            throw new NotImplementedException();
        }


        #endregion
    }

    // 状态历史记录类
    [Serializable()]
    public class StatusHistory
    {
        private readonly Stack<(DataStatus Data, ApprovalStatus Approval, bool Result)> _history = new Stack<(DataStatus, ApprovalStatus, bool)>();
        private const int MaxHistory = 5; // 最大历史记录数

        public void Push(DataStatus data, ApprovalStatus approval, bool result)
        {
            if (_history.Count >= MaxHistory)
            {
                var temp = _history.ToList();
                temp.RemoveAt(0);
                _history.Clear();
                temp.ForEach(x => _history.Push(x));
            }
            _history.Push((data, approval, result));
        }

        public (DataStatus Data, ApprovalStatus Approval, bool Result) Pop()
        {
            return _history.Count > 0 ? _history.Pop() : throw new InvalidOperationException("没有可恢复的历史记录");
        }

        public int Count => _history.Count;

    }


    /// <summary>
    /// 控件状态封装
    /// </summary>
    [Serializable()]
    public class ControlState
    {
        public bool Visible { get; set; }
        public bool Enabled { get; set; }
    }
    // 状态变更事件参数
    public class StatusChangedEventArgs : EventArgs
    {
        public string PropertyName { get; }
        public object OldValue { get; }
        public object NewValue { get; }

        public StatusChangedEventArgs(string propertyName, object oldValue, object newValue)
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public StatusChangedEventArgs(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    // 自定义特性标识状态字段
    [AttributeUsage(AttributeTargets.Property)]
    public class StatusFieldAttribute : Attribute
    {
        public string ModuleType { get; set; }
    }



    /// <summary>
    /// 通知服务接口
    /// </summary>
    public interface IWorkflowNotificationService
    {
        void NotifySubmit();
        void NotifyApprove();
        void NotifyReverseApprove();
        void NotifyClose();
        void NotifyReverseClose();
        void NotifyCancel();
        void NotifyDelete();



        //驳回通知
        void NotifyReject();
    }

    // 在通知服务中实现业务提醒
    public class WorkflowNotificationService : IWorkflowNotificationService
    {
        public void NotifyApprove()
        {
            // 触发仓库备货
            // WarehouseService.PrepareStock();
            // 通知财务
            //FinanceService.NotifyInvoice();
        }
        /*
        private readonly INotificationRuleRepository _ruleRepository;
        private readonly IMessageDispatcher _dispatcher;

        public WorkflowNotificationService(
            INotificationRuleRepository ruleRepository,
            IMessageDispatcher dispatcher)
        {
            _ruleRepository = ruleRepository;
            _dispatcher = dispatcher;
        }

        public void NotifySubmit()
        {
            var rules = _ruleRepository.GetRules(WorkflowEvent.Submit);
            foreach (var rule in rules)
            {
                var message = BuildMessage(rule);
                _dispatcher.Dispatch(message);
            }
        }

        private NotificationMessage BuildMessage(NotificationRule rule)
        {
            // 根据规则构建消息
            return new NotificationMessage
            {
                TemplateId = rule.TemplateId,
                Recipients = rule.GetRecipients(),
                Parameters = rule.GetParameters()
            };
        }
        */

        public void NotifySubmit()
        {
            // 通知上级审核
            //ApproverService.NotifySupervisor();
        }

        void IWorkflowNotificationService.NotifyCancel()
        {
            throw new NotImplementedException();
        }

        void IWorkflowNotificationService.NotifyClose()
        {
            throw new NotImplementedException();
        }

        void IWorkflowNotificationService.NotifyDelete()
        {
            throw new NotImplementedException();
        }

        void IWorkflowNotificationService.NotifyReject()
        {
            throw new NotImplementedException();
        }

        void IWorkflowNotificationService.NotifyReverseApprove()
        {
            throw new NotImplementedException();
        }

        void IWorkflowNotificationService.NotifyReverseClose()
        {
            throw new NotImplementedException();
        }
    }

}
