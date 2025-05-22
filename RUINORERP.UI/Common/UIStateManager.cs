using Fireasy.Common.Configuration;
using Netron.GraphLib;
using Org.BouncyCastle.Crypto.IO;
using RUINORERP.Business.CommService;
using RUINORERP.Business.FMService;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.UI.IM;
using RUINORERP.UI.Properties;
using SHControls.Mycontrol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Windows.Forms;
using WorkflowCore.Interface;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// UI 状态管理器 - 控制所有操作按钮的状态
    /// StatusEvaluator
    /// </summary>
    [Obsolete("用StatusEvaluator代理了")]
    public class UIStateManager
    {
        private readonly IStatusMachine _statusMachine;

        public UIStateManager(IStatusMachine statusMachine)
        {
            _statusMachine = statusMachine;
        }

        /// <summary>
        /// 根据菜单项枚举获取按钮状态
        /// </summary>
        public ControlState GetButtonState(MenuItemEnums operation)
        {
            return operation switch
            {
                // 基础操作
                MenuItemEnums.新增 => GetCreateState(),
                MenuItemEnums.取消 => GetCancelState(),
                MenuItemEnums.删除 => GetDeleteState(),
                MenuItemEnums.修改 => GetModifyState(),
                MenuItemEnums.提交 => GetSubmitState(),
                MenuItemEnums.保存 => GetSaveState(),

                // 审核相关
                MenuItemEnums.审核 => GetApproveState(),
                MenuItemEnums.反审 => GetReverseApproveState(),

                // 结案相关
                MenuItemEnums.结案 => GetCloseState(),
                MenuItemEnums.反结案 => GetReverseCloseState(),

                // 文件操作
                MenuItemEnums.导入 => GetImportState(),
                MenuItemEnums.导出 => GetExportState(),
                MenuItemEnums.打印 => GetPrintState(),
                MenuItemEnums.预览 => GetPreviewState(),
                MenuItemEnums.设计 => GetDesignState(),

                // 其他功能
                MenuItemEnums.刷新 => new ControlState { Visible = true, Enabled = true },
                MenuItemEnums.关闭 => new ControlState { Visible = true, Enabled = true },
                MenuItemEnums.查询 => new ControlState { Visible = true, Enabled = true },
                MenuItemEnums.选中 => new ControlState { Visible = true, Enabled = true },

                // 特殊功能
                MenuItemEnums.复制性新增 => GetCopyCreateState(),
                MenuItemEnums.数据特殊修正 => GetSpecialModifyState(),
                MenuItemEnums.已锁定 => GetLockedState(),

                // 默认处理
                _ => new ControlState { Visible = false, Enabled = false }
            };
        }

        #region 私有状态获取方法
        private ControlState GetCreateState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus == DataStatus.草稿,
            Enabled = true// _statusMachine.CanCreate()
        };

        private ControlState GetCopyCreateState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus.In(DataStatus.确认, DataStatus.完结),
            Enabled = _statusMachine.CurrentDataStatus == DataStatus.确认
        };

        private ControlState GetModifyState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus.In(DataStatus.草稿, DataStatus.新建),
            Enabled = _statusMachine.CanModify()
        };

        private ControlState GetSpecialModifyState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus == DataStatus.确认,
            Enabled = _statusMachine.ApprovalStatus == ApprovalStatus.已审核
        };

        private ControlState GetDeleteState() => new()
        {
            Visible = true,
            Enabled = _statusMachine.CanDelete()
        };

        private ControlState GetSubmitState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus == DataStatus.草稿,
            Enabled = _statusMachine.CanSubmit()
        };

        private ControlState GetSaveState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus.In(DataStatus.草稿, DataStatus.新建),
            Enabled = _statusMachine.CanSaveAsDraft()
        };

        private ControlState GetApproveState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus == DataStatus.新建,
            Enabled = _statusMachine.CanApprove()
        };

        private ControlState GetReverseApproveState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus == DataStatus.确认,
            Enabled = _statusMachine.CanReverseApprove()
        };

        private ControlState GetCloseState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus.In(DataStatus.确认, DataStatus.已取消),
            Enabled = _statusMachine.CanClose()
        };

        private ControlState GetReverseCloseState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus == DataStatus.完结,
            Enabled = _statusMachine.CanReverseClose()
        };

        private ControlState GetCancelState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus != DataStatus.已取消,
            Enabled = _statusMachine.CanCancel()
        };

        private ControlState GetImportState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus == DataStatus.草稿,
            Enabled = _statusMachine.CurrentDataStatus == DataStatus.草稿
        };

        private ControlState GetExportState() => new()
        {
            Visible = true,
            Enabled = _statusMachine.CurrentDataStatus != DataStatus.已取消
        };

        private ControlState GetPrintState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus.In(DataStatus.确认, DataStatus.完结),
            Enabled = _statusMachine.CurrentDataStatus.In(DataStatus.确认, DataStatus.完结)
        };

        private ControlState GetPreviewState() => GetPrintState();

        private ControlState GetDesignState()
        {
            var state = new ControlState
            {
                Visible = _statusMachine.CurrentDataStatus == DataStatus.完结,
                Enabled = false  // 默认禁用，需要特殊权限
            };


            //这里集成权限判断 tb_p4
            //if (state.Visible && _permissionService.HasPermission("REPORT_DESIGN"))
            //{
            //    state.Enabled = true;
            //}

            return state;
        }


        private ControlState GetLockedState() => new()
        {
            Visible = _statusMachine.CurrentDataStatus == DataStatus.已取消,
            Enabled = false
        };
        #endregion
        /// <summary>
        /// 获取当前状态显示文本
        /// </summary>
        public string GetStatusText()
        {
            var dataStatus = GetEnumDescription(_statusMachine.CurrentDataStatus);
            var approvalStatus = GetEnumDescription(_statusMachine.ApprovalStatus);

            return $"{dataStatus} | {approvalStatus}";
        }
        // 使用资源文件管理显示文本
        public string GetButtonText(MenuItemEnums operation)
        {
            return Resources.ResourceManager.GetString($"BTN_{operation}");
        }
        private string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(
                field, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
        }
    }




    // 状态评估器委托
    public delegate ControlState DeletegateStatusEvaluator(
        DataStatus dataStatus,
        ApprovalStatus approvalStatus,
        bool approvalResult,
        object context);




    // 状态评估器,轻量级状态判断器
    //实体中也有这些字段。在这里计算各种状态

    public class StatusEvaluator : IStatusEvaluator
    {
        public Enum CurrentStatus { get; set; }
        public bool ApprovalResult { get; set; }

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        public static ControlState GetControlState(
      DataStatus dataStatus,
      ApprovalStatus approvalStatus,
      bool approvalResult,
      MenuItemEnums operation, ActionStatus actionStatus)
        {
            return operation switch
            {
                MenuItemEnums.新增 => NewCreateState(dataStatus, actionStatus),
                MenuItemEnums.修改 => ModifyState(dataStatus, approvalStatus),
                MenuItemEnums.提交 => SubmitState(dataStatus, approvalStatus),
                MenuItemEnums.审核 => ApproveState(dataStatus, approvalStatus),
                MenuItemEnums.反审 => ReverseApproveState(dataStatus, approvalStatus),
                MenuItemEnums.结案 => CloseState(dataStatus, approvalStatus, approvalResult),
                // 其他操作状态判断...
                _ => DefaultState()
            };

            //MenuItemEnums.新增 => new ControlState { Visible = true, Enabled = true },
            //    MenuItemEnums.修改 => new ControlState
            //    {
            //        Visible = dataStatus == DataStatus.草稿,
            //        Enabled = dataStatus == DataStatus.草稿 && approvalStatus == ApprovalStatus.未审核
            //    },


        }

        private static ControlState SubmitState(DataStatus status, ApprovalStatus approval)
        {
            return new ControlState
            {
                Visible = status == DataStatus.草稿,
                Enabled = status == DataStatus.草稿 && approval == ApprovalStatus.未审核
            };
        }

        private static ControlState ApproveState(DataStatus status, ApprovalStatus approval)
        {
            return new ControlState
            {
                Visible = status == DataStatus.新建,
                Enabled = status == DataStatus.新建 &&
                         approval.In(ApprovalStatus.未审核, ApprovalStatus.驳回)
            };
        }

        private static ControlState ReverseApproveState(DataStatus status, ApprovalStatus approval)
        {
            return new ControlState
            {
                Visible = status == DataStatus.确认,
                Enabled = status == DataStatus.确认 &&
                         approval == ApprovalStatus.已审核
            };
        }

        private static ControlState CloseState(DataStatus status, ApprovalStatus approval, bool result)
        {
            return new ControlState
            {
                Visible = status.In(DataStatus.确认, DataStatus.已取消),
                Enabled = status == DataStatus.确认 &&
                         approval == ApprovalStatus.已审核 &&
                         result
            };
        }




        private static ControlState NewCreateState(DataStatus status, ActionStatus actionStatus) => new ControlState
        {
            Visible = status == DataStatus.草稿,
            Enabled = status == DataStatus.草稿
        };

        private static ControlState ModifyState(DataStatus status, ApprovalStatus approval) => new ControlState
        {
            Visible = status.In(DataStatus.草稿, DataStatus.新建),
            Enabled = status == DataStatus.草稿 && approval == ApprovalStatus.未审核
        };

        private static ControlState DefaultState() => new ControlState
        {
            Visible = true,
            Enabled = true
        };



        public static string GetStatusDisplay(DataStatus dataStatus, ApprovalStatus approvalStatus)
        {
            return $"{GetEnumDescription(dataStatus)} | {GetEnumDescription(approvalStatus)}";
        }

        private static string GetEnumDescription(Enum value) => value.ToString();

        public ControlState Evaluate(MenuItemEnums operation)
        {
            throw new NotImplementedException();
        }

        public ControlState Evaluate<TDataStatus>(MenuItemEnums operation)
        {
            throw new NotImplementedException();
        }
    }

    /*
    public static class FinancialStateEvaluator
    {
        /// <summary>
        /// 财务单据状态评估核心方法
        /// </summary>
        public ControlState Evaluate(
            Enum status,
            MenuItemEnums operation,
            bool hasRelatedRecords)
        {
            // 公共基础状态判断
            var baseState = EvaluateBaseStatus(status, operation);
            if (baseState != null) return baseState;

            // 按单据类型处理
            return status switch
            {
                PrePaymentStatus preStatus =>
                    EvaluatePrePayment(preStatus, operation, hasRelatedRecords),

                ARAPStatus arapStatus =>
                    EvaluateARAP(arapStatus, operation, hasRelatedRecords),

                PaymentStatus paymentStatus =>
                    EvaluatePayment(paymentStatus, operation, hasRelatedRecords),

                _ => throw new ArgumentException("未知财务单据类型")
            };
        }

        #region 基础状态判断
        private ControlState? EvaluateBaseStatus(Enum status, MenuItemEnums operation)
        {
            // 公共锁定状态处理
            if (operation == MenuItemEnums.已锁定)
            {
                return new ControlState
                {
                    Visible = status.IsFinalStatus(),
                    Enabled = false
                };
            }

            // 公共终态处理
            if (status.IsFinalStatus())
            {
                return new ControlState
                {
                    Visible = operation.In(
                        MenuItemEnums.查看,
                        MenuItemEnums.打印,
                        MenuItemEnums.导出),
                    Enabled = false
                };
            }

            return null;
        }
        #endregion

        #region 预付款单状态判断
        private ControlState EvaluatePrePayment(
            PrePaymentStatus status,
            MenuItemEnums operation,
            bool hasRelatedRecords)
        {
            return operation switch
            {
                MenuItemEnums.保存 when status == PrePaymentStatus.草稿 =>
                    new ControlState { Visible = true, Enabled = true },

                MenuItemEnums.提交 when status == PrePaymentStatus.草稿 =>
                    new ControlState { Visible = true, Enabled = false }, // 需先保存

                MenuItemEnums.提交 when status == PrePaymentStatus.待审核 =>
                    new ControlState { Visible = true, Enabled = true },

                MenuItemEnums.核销 when status == PrePaymentStatus.已生效 =>
                    new ControlState
                    {
                        Visible = true,
                        Enabled = !hasRelatedRecords
                    },

                MenuItemEnums.反核销 when status.HasFlag(PrePaymentStatus.部分核销) =>
                    new ControlState { Visible = true, Enabled = true },

                _ => new ControlState
                {
                    Visible = operation.In(
                        MenuItemEnums.新增,
                        MenuItemEnums.修改,
                        MenuItemEnums.删除),
                    Enabled = status.IsEditable()
                }
            };
        }
        #endregion

        #region 应收应付单状态判断
        private ControlState EvaluateARAP(
            ARAPStatus status,
            MenuItemEnums operation,
            bool hasRelatedRecords)
        {
            return operation switch
            {
                MenuItemEnums.创建付款单 when status.CanCreatePayment() =>
                    new ControlState { Visible = true, Enabled = true },

                MenuItemEnums.标记坏账 when status.CanMarkBadDebt() =>
                    new ControlState { Visible = true, Enabled = true },

                MenuItemEnums.部分支付 when status.AllowPartialPayment() =>
                    new ControlState { Visible = true, Enabled = true },

                _ => new ControlState
                {
                    Visible = operation.In(
                        MenuItemEnums.新增,
                        MenuItemEnums.提交,
                        MenuItemEnums.审核),
                    Enabled = status.IsEditable()
                }
            };
        }
        #endregion

        #region 收付款单状态判断
        private ControlState EvaluatePayment(
            PaymentStatus status,
            MenuItemEnums operation,
            bool hasRelatedRecords)
        {
            return operation switch
            {
                MenuItemEnums.关联核销单 when status.CanSettlePayment() =>
                    new ControlState { Visible = true, Enabled = true },

                MenuItemEnums.修改金额 when status.AllowAmountChange() =>
                    new ControlState { Visible = true, Enabled = true },

                MenuItemEnums.确认支付 when status == PaymentStatus.已生效 =>
                    new ControlState { Visible = true, Enabled = true },

                _ => new ControlState
                {
                    Visible = operation.In(
                        MenuItemEnums.提交,
                        MenuItemEnums.冲销),
                    Enabled = status.IsEditable()
                }
            };
        }
        #endregion
    }
}
    */

    /*
 // 使用灵活的状态评估委托
public UIStateBinder(..., Func<DataStatus, ApprovalStatus, bool, MenuItemEnums, ControlState> stateEvaluator = null)
{
_stateEvaluator = stateEvaluator ?? StatusEvaluator.GetControlState;
}
 */
    public static class ControlExtensions
    {
        public static bool In<T>(this T value, params T[] values) where T : Enum
        {
            return values.Contains(value);
        }
    }



    // 增强的UI状态绑定器（支持多状态类型）
    // 增强的UI状态绑定器（泛型版本）
    //应该在这个类中，通过传入的按钮。来控制UI
    //UI状态绑定器 - 负责管理所有控件状态

    public class UIStateBinder<TDataStatus> : IDisposable where TDataStatus : Enum
    {
        private bool _disposed;
        //private readonly IStatusProvider _statusProvider;
        private readonly BaseEntity _statusProvider;
        private readonly ToolStrip _container;
        private readonly Dictionary<Control, MenuItemEnums> _controls = new Dictionary<Control, MenuItemEnums>();
        private readonly IStatusEvaluator _stateEvaluator;
        private readonly Dictionary<MenuItemEnums, (ToolStripItem Control, DeletegateStatusEvaluator Evaluator)> _controlEvaluatorMappings = new();
        private readonly Dictionary<MenuItemEnums, ToolStripItem> _controlMappings = new Dictionary<MenuItemEnums, ToolStripItem>();
        private readonly Dictionary<MenuItemEnums, ControlState> _stateCache = new();
        private IStatusMachine _statusMachine;
        private readonly IWorkflowHost _workflowHost;
        private readonly Func<ControlState> _stateGetter;
        private readonly Action<Control> _stateApplier;
        public ToolStrip Container { get; set; }

        public UIStateBinder(
            //IStatusProvider statusProvider,
            BaseEntity statusProvider,
            ToolStrip container,
            IStatusEvaluator stateEvaluator = null, IWorkflowHost workflowHost = null)
        {
            _workflowHost = workflowHost;
            IWorkflowNotificationService notificationService = MainForm.Instance.AppContext.GetRequiredService<IWorkflowNotificationService>();
            _statusProvider = statusProvider ?? throw new ArgumentNullException(nameof(statusProvider));
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _stateEvaluator = stateEvaluator ?? CreateDefaultEvaluator(statusProvider);
            Container = _container;
            _statusMachine = new BusinessStatusMachine((DataStatus)_stateEvaluator.CurrentStatus,
                _stateEvaluator.ApprovalResult//这里是如果有值则显示他的值，如果没有则false。要修复
              , notificationService
              );
            if (_stateEvaluator != null)
            {
                // 订阅状态变更事件
                stateEvaluator.StatusChanged += (sender, e) =>
                {
                    Console.WriteLine($"状态变更：旧状态={e.OldValue?.ToString() ?? "null"} -> 新状态={e.NewValue}");
                    // 这里可以添加你的业务逻辑判断
                    HandleStatusChange(e.OldValue as Enum, e.NewValue as Enum);
                };
                // 测试状态变更（触发事件）
                //evaluator.CurrentStatus = DataStatus.草稿;  // 第一次设置（无旧状态）
                //evaluator.CurrentStatus = DataStatus.新建; // 状态变更（触发事件）
                //evaluator.CurrentStatus = DataStatus.新建; // 相同状态（不触发事件）
            }

            //获取MenuItemEnums枚举所有的值的集合GetValue
            var values = Enum.GetValues(typeof(MenuItemEnums));
            MenuTextItems = new Dictionary<string, MenuItemEnums>();
            foreach (var value in values)
            {
                if (value is MenuItemEnums menuItem)
                {
                    MenuTextItems.Add(value.ToString(), menuItem);
                }
            }
            // 在构造函数中自动初始化
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                InitializeControls();
            }
            // 订阅状态变化事件
            _statusProvider.StatusChanged += OnStatusChanged;


        }

        #region old

        public UIStateBinder()
        {
            // 构造函数的实现
        }



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="machine"></param>
        ///// <param name="container"></param>
        ///// <param name="workflowHost"></param>
        ///// <param name="stateEvaluator">使用灵活的状态评估委托</param>
        ///// <exception cref="ArgumentNullException"></exception>
       // public UIStateBinder(
       //IStatusMachine machine,
       //ToolStrip container,
       //IWorkflowHost workflowHost,
       ////最后ControlState是结果。前面的都是参数
       //Func<DataStatus, ApprovalStatus, bool, MenuItemEnums, ActionStatus, ControlState> stateEvaluator = null)
       // {

       // }


        public void InitializeControls()
        {
            // 处理ToolStrip及其派生控件
            if (Container is ToolStrip toolStrip)
            {
                // 自动发现带Tag标记的控件
                FindTaggedControls(toolStrip.Items);
            }
            // 处理其他容器控件
            else
            {
            
            }
        }


        public Dictionary<string, MenuItemEnums> MenuTextItems { get; set; }


        private void FindTaggedControls(ToolStripItemCollection Items)
        {
            MenuItemEnums operation = MenuItemEnums.新增;
            foreach (var item in Items)
            {
                //if (ctrl.Tag is MenuItemEnums operation)
                if (item is ToolStripButton toolStripButton)
                {
                    if (MenuTextItems.TryGetValue(toolStripButton.Text, out operation))
                    {
                        RegisterControl(toolStripButton, operation);
                    }
                }
                if (item is ToolStripDropDownButton subItem)
                {
                    if (subItem.HasDropDownItems)
                    {
                        FindTaggedControls(subItem.DropDownItems);
                    }
                }
                if (item is ToolStripSplitButton tsSubItem)
                {

                    if (MenuTextItems.TryGetValue(tsSubItem.Text, out operation))
                    {
                        RegisterControl(tsSubItem, operation);
                    }
                    if (tsSubItem.HasDropDownItems)
                    {
                        FindTaggedControls(tsSubItem.DropDownItems);
                    }
                }






            }
        }

        public virtual void RegisterControl(ToolStripItem control, MenuItemEnums operation)
        {
            _controlMappings[operation] = control;
            control.Click += (s, e) => HandleOperation(operation);
            // 处理按钮点击事件
            //if (control is Button btn)
            //{
            //    btn.Click += (s, e) => HandleOperation(operation);
            //}
        }



        /// <summary>
        /// 注册控件及其状态评估逻辑
        /// </summary>
        public void RegisterControl(MenuItemEnums operation, ToolStripItem control, DeletegateStatusEvaluator evaluator)
        {
            _controlEvaluatorMappings[operation] = (control, evaluator);
            UpdateControlState(operation);
        }

        /// <summary>
        /// 批量注册控件
        /// </summary>
        public void RegisterControls(Dictionary<MenuItemEnums, (ToolStripItem, DeletegateStatusEvaluator)> mappings)
        {
            foreach (var kvp in mappings)
            {
                RegisterControl(kvp.Key, kvp.Value.Item1, kvp.Value.Item2);
            }
        }


        private void UpdateControlState(MenuItemEnums operation)
        {
            if (!_controlEvaluatorMappings.TryGetValue(operation, out var mapping)) return;

            var newState = mapping.Evaluator(
                _statusMachine.CurrentDataStatus,
                _statusMachine.ApprovalStatus,
                _statusMachine.ApprovalResult,
                _statusMachine.ActionStatus // 可根据需要传入上下文
            );

            if (!_stateCache.TryGetValue(operation, out var oldState) || !oldState.Equals(newState))
            {
                ApplyStateToControl(mapping.Control, newState);
                _stateCache[operation] = newState;
            }
        }



        public void UpdateAllControls()
        {
            // 根据当前状态更新控件状态
            //var oldcurrentStatus = (TDataStatus)_statusProvider.CurrentStatus;
            //var oldapprovalResult = _statusProvider.ApprovalResult;

            foreach (var item in _container.Items)
            {
                if (item is ToolStripButton button)
                {
                    if (Enum.TryParse<MenuItemEnums>(button.Text, out var operation))
                    {
                        //var state = _evaluator.Evaluate(operation, currentStatus, approvalResult);
                        //button.Enabled = state.Enabled;
                        //button.Visible = state.Visible;
                    }
                }
            }


            var currentStatus = _statusMachine.CurrentDataStatus;
            var approvalStatus = _statusMachine.ApprovalStatus;
            var approvalResult = _statusMachine.ApprovalResult;
            var actionStatus = _statusMachine.ActionStatus;
            //foreach (var kvp in _controlMappings)
            //{
            //    var state = _stateEvaluator(
            //        currentStatus,
            //        approvalStatus,
            //        approvalResult,
            //        kvp.Key, actionStatus
            //    );
            //    _stateEvaluator.
            //    ApplyStateToControl(kvp.Value, state);
            //}
        }

        private void ApplyStateToControl(ToolStripItem control, ControlState state)
        {
            //线程安全更新
            if (control.GetCurrentParent()?.InvokeRequired ?? false)
            {
                control.GetCurrentParent()?.BeginInvoke((Action)(() =>
                {
                    control.Enabled = state.Enabled;
                    control.Visible = state.Visible;
                }));
            }
            else
            {
                control.Enabled = state.Enabled;
                control.Visible = state.Visible;
            }
        }


        private void UpdateUIToolBar()
        {
            DataStatus dataStatus = _statusMachine.CurrentDataStatus;
            // ActionStatus actionStatus = (ActionStatus)(Enum.Parse(typeof(ActionStatus), entity.GetPropertyValue(typeof(ActionStatus).Name).ToString()));
            switch (dataStatus)
            {
                //点新增
                case DataStatus.草稿:
                    _controlMappings[MenuItemEnums.新增].Enabled = false;
                    _controlMappings[MenuItemEnums.取消].Enabled = true;
                    _controlMappings[MenuItemEnums.刷新].Enabled = false;
                    _controlMappings[MenuItemEnums.修改].Enabled = false;
                    _controlMappings[MenuItemEnums.提交].Enabled = false;
                    _controlMappings[MenuItemEnums.审核].Enabled = false;
                    _controlMappings[MenuItemEnums.反审].Enabled = false;
                    _controlMappings[MenuItemEnums.保存].Enabled = true;
                    _controlMappings[MenuItemEnums.打印].Enabled = false;


                    _controlMappings[MenuItemEnums.删除].Enabled = _statusMachine.CanDelete();



                    _controlMappings[MenuItemEnums.结案].Enabled = false;


                    //if (actionStatus == ActionStatus.新增)
                    //{
                    //    toolStripButtonSave.Enabled = true;
                    //    toolStripbtnModify.Enabled = false;
                    //}
                    //else
                    //{
                    //    toolStripButtonSave.Enabled = false;
                    //}


                    break;
                case DataStatus.新建:

                    //_controlMappings[MenuItemEnums.新增].Enabled = false;// 允许新增其他单据
                    //_controlMappings[MenuItemEnums.取消].Enabled = true;
                    //_controlMappings[MenuItemEnums.修改].Enabled = true;
                    //_controlMappings[MenuItemEnums.提交].Enabled = false;
                    //_controlMappings[MenuItemEnums.审核].Enabled = true;
                    //_controlMappings[MenuItemEnums.反审].Enabled = false;
                    //_controlMappings[MenuItemEnums.保存].Enabled = true;
                    //_controlMappings[MenuItemEnums.打印].Enabled = true;
                    //_controlMappings[MenuItemEnums.删除].Enabled = true;
                    //_controlMappings[MenuItemEnums.结案].Enabled = false;

                    _controlMappings[MenuItemEnums.新增].Enabled = true;    // 允许新增其他单据
                    _controlMappings[MenuItemEnums.取消].Enabled = true;
                    _controlMappings[MenuItemEnums.修改].Enabled = true;
                    _controlMappings[MenuItemEnums.提交].Enabled = false;
                    _controlMappings[MenuItemEnums.审核].Enabled = true;
                    _controlMappings[MenuItemEnums.保存].Enabled = true;
                    _controlMappings[MenuItemEnums.打印].Enabled = true;
                    _controlMappings[MenuItemEnums.删除].Enabled = true;
                    _controlMappings[MenuItemEnums.结案].Enabled = false;


                    break;
                case DataStatus.确认:

                    //_controlMappings[MenuItemEnums.新增].Enabled = false;
                    //_controlMappings[MenuItemEnums.取消].Enabled = false;
                    //_controlMappings[MenuItemEnums.修改].Enabled = false;
                    //_controlMappings[MenuItemEnums.提交].Enabled = false;
                    //_controlMappings[MenuItemEnums.审核].Enabled = false;
                    //_controlMappings[MenuItemEnums.反审].Enabled = true;
                    //_controlMappings[MenuItemEnums.保存].Enabled = false;
                    //_controlMappings[MenuItemEnums.打印].Enabled = true;
                    //_controlMappings[MenuItemEnums.删除].Enabled = false;
                    //_controlMappings[MenuItemEnums.结案].Enabled = true;

                    _controlMappings[MenuItemEnums.新增].Enabled = true;
                    _controlMappings[MenuItemEnums.取消].Enabled = false;
                    _controlMappings[MenuItemEnums.修改].Enabled = false;
                    _controlMappings[MenuItemEnums.提交].Enabled = false;
                    _controlMappings[MenuItemEnums.审核].Enabled = false;
                    _controlMappings[MenuItemEnums.反审].Enabled = true;
                    _controlMappings[MenuItemEnums.保存].Enabled = false;
                    _controlMappings[MenuItemEnums.打印].Enabled = true;
                    _controlMappings[MenuItemEnums.删除].Enabled = false;
                    _controlMappings[MenuItemEnums.结案].Enabled = true;
                    break;
                case DataStatus.完结:
                    //_controlMappings[MenuItemEnums.新增].Enabled = false;
                    //_controlMappings[MenuItemEnums.取消].Enabled = false;
                    //_controlMappings[MenuItemEnums.修改].Enabled = false;
                    //_controlMappings[MenuItemEnums.提交].Enabled = false;
                    //_controlMappings[MenuItemEnums.审核].Enabled = false;
                    //_controlMappings[MenuItemEnums.反审].Enabled = false;
                    //_controlMappings[MenuItemEnums.保存].Enabled = false;
                    //_controlMappings[MenuItemEnums.打印].Enabled = true;
                    //_controlMappings[MenuItemEnums.删除].Enabled = false;
                    //_controlMappings[MenuItemEnums.结案].Enabled = false;

                    _controlMappings[MenuItemEnums.新增].Enabled = true;
                    _controlMappings[MenuItemEnums.取消].Enabled = false;
                    _controlMappings[MenuItemEnums.修改].Enabled = false;
                    _controlMappings[MenuItemEnums.提交].Enabled = false;
                    _controlMappings[MenuItemEnums.审核].Enabled = false;
                    _controlMappings[MenuItemEnums.反审].Enabled = false;
                    _controlMappings[MenuItemEnums.保存].Enabled = false;
                    _controlMappings[MenuItemEnums.打印].Enabled = true;
                    _controlMappings[MenuItemEnums.删除].Enabled = false;
                    _controlMappings[MenuItemEnums.结案].Enabled = false;

                    break;
                case DataStatus.已取消:
                    _controlMappings[MenuItemEnums.新增].Enabled = true;
                    _controlMappings[MenuItemEnums.取消].Enabled = false;
                    _controlMappings[MenuItemEnums.修改].Enabled = false;
                    _controlMappings[MenuItemEnums.提交].Enabled = false;
                    _controlMappings[MenuItemEnums.审核].Enabled = false;
                    _controlMappings[MenuItemEnums.反审].Enabled = false;
                    _controlMappings[MenuItemEnums.保存].Enabled = false;
                    _controlMappings[MenuItemEnums.打印].Enabled = true;
                    _controlMappings[MenuItemEnums.删除].Enabled = false;
                    _controlMappings[MenuItemEnums.结案].Enabled = false;
                    break;

                default:
                    break;
            }
        }


        private void ShowErrorPrompt(Exception ex)
        {
            MessageBox.Show(Container.FindForm(),
                $"操作失败: {ex.Message}",
                "系统提示",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }






        public void Dispose()
        {
            _statusMachine.StatusChanged -= OnStatusChanged;
            foreach (var item in _controlMappings)
            {
                item.Value.Click -= (s, e) => HandleOperation(item.Key);
            }
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void HandleOperation(MenuItemEnums operation)
        {

            try
            {
                switch (operation)
                {
                    case MenuItemEnums.修改:
                        _statusMachine.CanModify();
                        _statusMachine.Modify();
                        break;
                    case MenuItemEnums.新增:
                        _statusMachine.CanCancel();
                        _statusMachine.Create();
                        break;
                    case MenuItemEnums.提交:
                        _statusMachine.Submit();
                        break;
                    case MenuItemEnums.审核:
                        _statusMachine.Approve();
                        break;
                    case MenuItemEnums.反审:
                        _statusMachine.ReverseApprove();
                        break;
                    case MenuItemEnums.取消:
                        break;
                    case MenuItemEnums.关闭:
                        break;
                    case MenuItemEnums.功能:
                        break;
                    case MenuItemEnums.帮助:
                        break;
                    case MenuItemEnums.刷新:
                        break;
                    case MenuItemEnums.导入:
                        break;
                    case MenuItemEnums.导出:
                        break;
                    case MenuItemEnums.打印:
                        break;
                    case MenuItemEnums.结案:
                        break;
                    // 其他操作处理...
                    default:
                        break;
                        //throw new NotSupportedException($"不支持的操作类型: {operation}");
                }
            }
            catch (Exception ex)
            {
                ShowErrorPrompt(ex);
                _statusMachine.Undo(); // 自动恢复状态
                UpdateAllControls();
            }
        }


        private void CheckWorkflowTriggers()
        {
            // 根据当前状态触发工作流事件
            var eventName = GetWorkflowEventName();
            if (!string.IsNullOrEmpty(eventName))
            {
                // _workflowHost.PublishEvent(eventName, _machine.WorkflowId, null);
            }
        }

        private string GetWorkflowEventName() => (_statusMachine.CurrentDataStatus, _statusMachine.ApprovalStatus) switch
        {
            (DataStatus.新建, ApprovalStatus.未审核) => "SubmitEvent",
            (DataStatus.确认, ApprovalStatus.已审核) => "ApproveEvent",
            _ => null
        };


        private void DefaultStateApplier(ToolStripItem control)
        {
            //if (control is ToolStripItem item)
            //{
            //    item.Enabled = state.Enabled;
            //    item.Visible = state.Visible;
            //}
            // 其他控件类型处理...
        }




        #endregion

        //=========================

        /// <summary>
        /// 业务逻辑处理示例
        /// </summary>
        private static void HandleStatusChange(Enum oldStatus, Enum newStatus)
        {
            if (newStatus is DataStatus status)
            {
                switch (status)
                {
                    case DataStatus.草稿:
                        Console.WriteLine("执行审批通过后的业务逻辑...");
                        break;
                    case DataStatus.新建:
                        Console.WriteLine("执行审批拒绝后的业务逻辑...");
                        break;
                        // 其他状态分支...
                }
            }
        }

        private IStatusEvaluator CreateDefaultEvaluator(BaseEntity provider)
        {

            IStatusEvaluator statusEvaluator = null;
            //应该是根据不同的类型 返回不同的结果
            if (typeof(TDataStatus) == typeof(DataStatus))
            {
                statusEvaluator = new StatusEvaluator();
                StatusEvaluator.GetControlState(DataStatus.草稿, ApprovalStatus.未审核, false, MenuItemEnums.新增, ActionStatus.无操作);
                //statusEvaluator.Evaluate<DataStatus>(MenuItemEnums.新增);
            }
            else
            {
                statusEvaluator = new FinancialStatusEvaluator();
            }

            return statusEvaluator;
            /*
                        return provider switch
                        {
                            FinancialStatusProvider => new StatusEvaluator(),
                            FinancialStatusEvaluator => new FinancialStatusEvaluator(),
                            BaseStatusEvaluator<DataStatus> => new BusinessStatusEvaluator(),
                            _ => throw new NotSupportedException("未知状态类型")
                        };*/
        }


        private void OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            UpdateAllControls();
            CheckWorkflowTriggers();
            UpdateUIToolBar();

        }








        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _statusProvider.StatusChanged -= OnStatusChanged;
                // 释放其他托管资源
            }

            _disposed = true;
        }

        ~UIStateBinder()
        {
            Dispose(false);
        }

        // 其他实现...
    }


    // 操作处理器（实现示例）
    public static class OperationHandlers
    {
        private static readonly Dictionary<MenuItemEnums, Action<IStatusProvider>> _handlers;

        static OperationHandlers()
        {
            _handlers = new Dictionary<MenuItemEnums, Action<IStatusProvider>>();
            InitializeHandlers();
        }

        private static void InitializeHandlers()
        {
            // 使用反射发现所有处理方法
            var methods = typeof(OperationHandlers).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<OperationHandlerAttribute>() != null);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<OperationHandlerAttribute>();
                _handlers[attr.Operation] = provider => method.Invoke(null, new object[] { provider });
            }
        }

        [AttributeUsage(AttributeTargets.Method)]
        private class OperationHandlerAttribute : Attribute
        {
            public MenuItemEnums Operation { get; }

            public OperationHandlerAttribute(MenuItemEnums operation)
            {
                Operation = operation;
            }
        }

        [OperationHandler(MenuItemEnums.提交)]
        private static void HandleSubmit(IStatusProvider provider)
        {
            //if (provider is BusinessStatusProvider businessProvider)
            //{
            //    // 业务提交逻辑
            //}
            //else if (provider is FinancialStatusProvider financialProvider)
            //{
            //    // 财务提交逻辑
            //}
        }

        public static Action<IStatusProvider> GetHandler(MenuItemEnums operation)
        {
            return _handlers.TryGetValue(operation, out var handler) ? handler : null;
        }
    }






}
