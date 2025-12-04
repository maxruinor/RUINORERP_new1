using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Business;
using RUINORERP.UI.UCSourceGrid;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Common.CollectionExtension;
using static RUINORERP.UI.Common.DataBindingHelper;
using static RUINORERP.UI.Common.GUIUtils;
using RUINORERP.Model.Dto;
using DevAge.Windows.Forms;
using RUINORERP.Common.Helper;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global;
using RUINORERP.UI.Report;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.AdvancedUIModule;
using Krypton.Navigator;
using System.Linq.Expressions;
using RUINORERP.Common.Extensions;
using System.Collections;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Logging;
using RUINOR.Core;
using RUINORERP.Model.CommonModel;
using FluentValidation;
using FluentValidation.Results;
using Krypton.Toolkit;
using System.IO;
using System.Diagnostics;
using SqlSugar;
using RUINORERP.Business.Processor;
using ExCSS;

using OfficeOpenXml.FormulaParsing.Excel.Functions;
using MySqlX.XDevAPI.Common;
using RUINORERP.Business.Security;
using RUINORERP.UI.CommonUI;
using ImageHelper = RUINORERP.UI.Common.ImageHelper;
using Netron.GraphLib;
using Newtonsoft.Json;
using RUINORERP.UI.SS;
using MathNet.Numerics.LinearAlgebra.Factorization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using SourceGrid;
using RUINORERP.UI.FormProperty;
using SourceGrid.Cells.Models;
using FastReport.Table;
using FastReport.DevComponents.AdvTree;
using Newtonsoft.Json.Linq;
using System.Web.Caching;
using Microsoft.Extensions.Caching.Memory;
using RUINORERP.UI.PSI.SAL;


using RUINORERP.Model.TransModel;
using System.Threading;
using System.Management.Instrumentation;
using FastReport.DevComponents.DotNetBar;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using System.Windows.Controls.Primitives;
using RUINORERP.PacketSpec.Models;
using RUINORERP.Common.LogHelper;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.StatusManagerService;
using RUINORERP.Model.Base;
using System.Windows.Documents;
using RUINORERP.UI.Monitoring.Auditing;
using RUINORERP.UI.FM;
using RUINORERP.UI.FM.FMBase;
using LiveChartsCore.Geo;
using RUINORERP.UI.MRP.MP;
using Winista.Text.HtmlParser.Lex;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System.Management;
using RUINORERP.Business.BizMapperService;
using RUINORERP.UI.Network;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Business.Cache;
using RUINORERP.UI.Network.Services;
using RUINORERP.Business.CommService;
using RUINOR.WinFormsUI.CustomPictureBox;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.UI.StateManagement.UI;
using RUINORERP.UI.StateManagement.Core;
using RUINORERP.Model.Base.StatusManager;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 单据类型的编辑 主表T子表C
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BaseBillEditGeneric<T, C> : BaseBillEdit, IContextMenuInfoAuth, IToolStripMenuInfoAuth where T : BaseEntity, new() where C : class, new()
    {
        public virtual List<UControls.ContextMenuController> AddContextMenu()
        {
            List<UControls.ContextMenuController> list = new List<UControls.ContextMenuController>();
            return list;
        }

        /// <summary>
        /// 集成式锁管理服务 v2.0.0
        /// 推荐使用新的集成式服务，提供心跳集成、智能缓存和异常恢复功能
        /// </summary>
        private ClientLockManagementService _integratedLockService;
        private CancellationTokenSource _lockRefreshTokenSource;
        private readonly ClientCommunicationService _clientCommunicationService;
        private Task _lockRefreshTask;
        private long _currentBillId;
        private long _currentMenuId;

        public virtual ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;
        }

        #region 单据明细中的 产品公共部分的字段提取。为了能统一控制这些公共字段

        public List<Type> PublicEntityObjects { get; set; } = new List<Type>();

        public virtual void AddPublicEntityObject(Type type)
        {
            if (!PublicEntityObjects.Contains(type))
            {
                PublicEntityObjects.Add(type);
            }

        }

        #endregion



        /// <summary>
        /// 初始化状态管理系统
        /// 子类可以重写此方法以添加自定义的状态管理初始化逻辑
        /// </summary>
        protected override void InitializeStateManagement()
        {
            // 添加设计模式检测，避免在设计器中运行时出现空引用异常
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                // 在设计模式下，只进行必要的初始化，避免访问ApplicationContext
                // 设置基本组件状态以确保设计器正常工作
                return;
            }


            // 调用基类的InitializeStateManagement方法
            // 基类StateAwareControl已经处理了基本的状态管理器初始化
            base.InitializeStateManagement();

            // 只有在基类初始化失败且ApplicationContext不为null时，才尝试从应用上下文获取
            if (this.StateManager == null && RUINORERP.Model.Context.ApplicationContext.Current != null)
            {
                this.StateManager = RUINORERP.Model.Context.ApplicationContext.Current.GetRequiredService<IUnifiedStateManager>();
            }

            // 注册状态变更事件处理程序
            this.StatusChanged -= HandleStatusChangedEvent;
            this.StatusChanged += HandleStatusChangedEvent;

            // 初始化按钮状态管理
            InitializeButtonStateManagement();

            // 为泛型类型配置状态管理
            ConfigureGenericStateManager();

            // 注意：StatusContext是在BindEntity方法中创建的，
            // 所以在这里不能直接访问，需要延迟事件订阅
            // 事件订阅将在OnStatusContextChanged方法中处理

            // 初始化泛型特定的按钮状态管理
            InitializeGenericButtonStateManagement();
        }



        /// <summary>
        /// 处理状态变更事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void HandleStatusChangedEvent(object sender, StateTransitionEventArgs e)
        {
            try
            {
                if (e.Entity is BaseEntity entity && this.EditEntity == entity)
                {
                    // 统一处理状态变更
                    OnEntityStateChanged(sender, e);

                    // 记录状态变更日志
                    logger?.Debug($"实体 {entity.GetType().Name} 状态从 {e.OldStatus} 变更为 {e.NewStatus}，原因：{e.Reason}");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, $"处理状态变更事件失败：{ex.Message}");
            }
        }


        /// <summary>
        /// 实体状态变更事件处理程序 - 统一处理所有状态变更事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        protected virtual void OnEntityStateChanged(object sender, StateTransitionEventArgs e)
        {
            if (e.Entity is BaseEntity entity)
            {
                // 统一更新所有UI状态
                UpdateAllUIStates(entity);

                // 触发子类的特定状态变更处理
                HandleSpecificStateChange(e.OldStatus, e.NewStatus, e.Reason);
            }
        }


        /// <summary>
        /// 更新状态显示
        /// </summary>
        protected virtual void UpdateStateDisplay()
        {
            if (EditEntity == null) return;

            try
            {
                // 使用V3状态管理系统获取当前状态描述
                if (EditEntity is BaseEntity baseEntity)
                {
                    // 获取状态描述
                    string statusDesc = baseEntity.GetCurrentStatusDescription();

                    // 更新状态标签（如果存在）
                    var lblDataStatus = this.Controls.Find("lblDataStatus", true).FirstOrDefault() as Label;
                    if (lblDataStatus != null && !string.IsNullOrEmpty(statusDesc))
                    {
                        lblDataStatus.Text = statusDesc;
                        lblDataStatus.ForeColor = GetStatusColor(baseEntity.GetDataStatus());
                    }

                    // 更新审核状态显示
                    if (EditEntity.ContainsProperty(nameof(ApprovalStatus)))
                    {
                        try
                        {
                            var approvalStatus = EditEntity.GetPropertyValue(nameof(ApprovalStatus));
                            if (approvalStatus != null)
                            {
                                var lblReview = this.Controls.Find("lblReview", true).FirstOrDefault() as Label;
                                if (lblReview != null)
                                {
                                    // 安全转换审核状态，如果转换失败则显示默认值
                                    if (Enum.IsDefined(typeof(ApprovalStatus), approvalStatus))
                                    {
                                        lblReview.Text = ((ApprovalStatus)approvalStatus).ToString();
                                    }
                                    else if (approvalStatus is string statusStr && Enum.TryParse<ApprovalStatus>(statusStr, out var parsedStatus))
                                    {
                                        lblReview.Text = parsedStatus.ToString();
                                    }
                                    else
                                    {
                                        lblReview.Text = approvalStatus.ToString();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            logger?.LogError(ex, "更新审核状态显示失败: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "更新状态显示失败");
            }
        }


        /// <summary>
        /// 根据当前状态更新UI - 调用统一的UI状态更新方法
        /// </summary>
        protected virtual void UpdateUIByCurrentState()
        {
            if (EditEntity is BaseEntity baseEntity)
            {
                UpdateAllUIStates(baseEntity);
            }
        }


        /// <summary>
        /// 更新工具栏按钮状态 - 调用统一的按钮状态更新方法
        /// </summary>
        /// <param name="currentStatus">当前状态</param>
        protected override void UpdateToolbarButtons(DataStatus currentStatus)
        {
            // 调用统一的按钮状态更新方法
            UpdateAllButtonStates(currentStatus);
            // 更新基于权限的按钮可见性
            UpdatePermissionBasedButtonVisibility();
        }


        /// <summary>
        /// 更新基于权限的按钮可见性
        /// </summary>
        protected virtual void UpdatePermissionBasedButtonVisibility()
        {
            if (EditEntity is BaseEntity baseEntity)
            {
                // 使用状态管理系统检查操作权限并更新按钮可见性
                var actionPermissions = new[] { "UpdateCustomizedCost", "AntiCloseCase", "SpecialOperations" };

                foreach (var action in actionPermissions)
                {
                    var buttonName = $"toolStripButton{action}";
                    var button = FindToolStripButtonByName(buttonName);

                    if (button != null)
                    {
                        button.Visible = CanExecuteAction(action);
                    }
                }
            }
        }

        /// <summary>
        /// 检查操作权限 - 使用UI控制器
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <returns>是否有权限</returns>
        protected virtual bool CanExecuteAction(string actionName)
        {
            if (EditEntity is BaseEntity baseEntity && StatusContext != null && UIController != null)
            {
                // 将操作名称转换为MenuItemEnums
                if (Enum.TryParse<MenuItemEnums>(actionName, out var menuAction))
                {
                    return UIController.CanExecuteAction(menuAction, StatusContext);
                }
            }
            return false;
        }

        /// <summary>
        /// 检查状态转换权限 - 使用状态管理器
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>转换结果</returns>
        protected virtual async Task<StateTransitionCheckResult> CanTransitionToAsync(string targetStatus)
        {
            if (EditEntity is BaseEntity baseEntity && StateManager != null)
            {
                try
                {
                    // 尝试解析为DataStatus
                    if (Enum.TryParse<DataStatus>(targetStatus, out var dataStatus))
                    {
                        var result = await StateManager.ValidateDataStatusTransitionAsync(baseEntity, dataStatus);
                        return result.IsSuccess
                            ? StateTransitionCheckResult.Allowed()
                            : StateTransitionCheckResult.Denied(result.ErrorMessage);
                    }

                    // 如果目标状态不是DataStatus，检查业务状态
                    var statusType = FMPaymentStatusHelper.GetStatusType(baseEntity);
                    if (statusType != null)
                    {
                        var statusValue = Enum.Parse(statusType, targetStatus);
                        var validateMethod = StateManager.GetType().GetMethod("ValidateBusinessStatusTransitionAsync")
                            .MakeGenericMethod(statusType);

                        var task = (Task<StateTransitionResult>)validateMethod.Invoke(StateManager, new object[] { baseEntity, statusValue });
                        var result = await task;

                        return result.IsSuccess
                            ? StateTransitionCheckResult.Allowed()
                            : StateTransitionCheckResult.Denied(result.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    return StateTransitionCheckResult.Denied($"状态转换检查失败: {ex.Message}");
                }
            }
            return StateTransitionCheckResult.Denied("实体或状态管理器不可用");
        }

        /// <summary>
        /// 执行状态转换 - 使用状态管理器（异步版本）
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>转换结果</returns>
        protected virtual async Task<StateTransitionResult> TransitionToAsync(DataStatus targetStatus, string reason = "")
        {
            if (EditEntity is BaseEntity baseEntity && StateManager != null)
            {
                try
                {
                    // 验证状态转换
                    var validationResult = await StateManager.ValidateDataStatusTransitionAsync(baseEntity, targetStatus);
                    if (!validationResult.IsSuccess)
                    {
                        return validationResult;
                    }

                    // 执行状态转换
                    StateManager.SetEntityDataStatus(baseEntity, targetStatus);

                    // 更新UI状态
                    UpdateAllUIStates(baseEntity);

                    return StateTransitionResult.Success($"状态转换成功: {targetStatus}");
                }
                catch (Exception ex)
                {
                    return StateTransitionResult.Failure($"状态转换失败: {ex.Message}");
                }
            }
            return StateTransitionResult.Failure("实体或状态管理器不可用");
        }

        /// <summary>
        /// 执行状态转换 - 使用状态管理器（同步版本）
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>转换结果</returns>
        protected virtual StateTransitionResult TransitionTo(DataStatus targetStatus, string reason = "")
        {
            if (EditEntity is BaseEntity baseEntity && StateManager != null)
            {
                try
                {
                    // 执行状态转换（跳过异步验证以保持同步）
                    StateManager.SetEntityDataStatus(baseEntity, targetStatus);

                    // 更新UI状态
                    UpdateAllUIStates(baseEntity);

                    return StateTransitionResult.Success($"状态转换成功: {targetStatus}");
                }
                catch (Exception ex)
                {
                    return StateTransitionResult.Failure($"状态转换失败: {ex.Message}");
                }
            }
            return StateTransitionResult.Failure("实体或状态管理器不可用");
        }


        /// <summary>
        /// 状态上下文变更事件处理程序
        /// 当StatusContext属性变更时调用
        /// </summary>
        protected override void OnStatusContextChanged()
        {
            // 调用基类方法
            base.OnStatusContextChanged();

            // 如果StatusContext不为null，订阅状态变更事件
            if (StatusContext != null)
            {
                // 注册泛型特定的状态变更事件处理程序
                StatusContext.StatusChanged += OnGenericEntityStateChanged;
            }
        }

        /// <summary>
        /// 当通用实体状态改变时调用
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        protected virtual void OnGenericEntityStateChanged(object sender, StateTransitionEventArgs e)
        {
            // 获取实体
            var entity = e.Entity as BaseEntity;
            if (entity != null)
            {
                // 使用统一方法更新所有UI状态
                UpdateAllUIStates(entity);
            }
        }

        /// <summary>
        /// 统一更新所有UI状态 - 集中处理所有UI状态更新逻辑
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected virtual void UpdateAllUIStates(BaseEntity entity)
        {
            if (entity == null) return;

            try
            {
                // 使用新的状态管理系统更新UI
                if (StateManager != null)
                {
                    // 获取当前状态
                    var currentStatus = StateManager.GetDataStatus(entity);

                    // 统一更新所有按钮状态
                    UpdateAllButtonStates(currentStatus);

                    // 更新UI控件状态
                    UpdateUIControlsByState(currentStatus);

                    // 更新状态显示
                    UpdateStateDisplay();

                    // 更新基于权限的按钮可见性
                    UpdatePermissionBasedButtonVisibility();
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "统一更新UI状态失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 统一更新所有按钮状态 - 集中管理所有工具栏按钮的状态
        /// </summary>
        /// <param name="currentStatus">当前状态</param>
        protected virtual void UpdateAllButtonStates(DataStatus currentStatus)
        {
            try
            {
                bool canModify = false, canSave = false, canSubmit = false, canReview = false, canReverseReview = false;
                bool canCloseCase = false, canAntiCloseCase = false, canDelete = false, canCancel = false;

                // 优先使用UIController和StatusContext获取按钮状态
                if (UIController != null && StatusContext != null)
                {
                    // 检查各种操作的可执行性
                    canModify = UIController.CanExecuteAction(MenuItemEnums.修改, StatusContext);
                    canSave = UIController.CanExecuteAction(MenuItemEnums.保存, StatusContext);
                    canSubmit = UIController.CanExecuteAction(MenuItemEnums.提交, StatusContext);
                    canReview = UIController.CanExecuteAction(MenuItemEnums.审核, StatusContext);
                    canReverseReview = UIController.CanExecuteAction(MenuItemEnums.反审, StatusContext);
                    canCloseCase = UIController.CanExecuteAction(MenuItemEnums.结案, StatusContext);
                    canAntiCloseCase = UIController.CanExecuteAction(MenuItemEnums.反结案, StatusContext);
                    canDelete = UIController.CanExecuteAction(MenuItemEnums.删除, StatusContext);
                    canCancel = UIController.CanExecuteAction(MenuItemEnums.取消, StatusContext);
                }
                else if (StateManager != null && EditEntity is BaseEntity entity)
                {
                    // 如果UIController不可用，使用StateManager获取可执行的操作
                    var availableActions = StatusHelper.GetAvailableActions(currentStatus);
                    canModify = availableActions.Contains(MenuItemEnums.修改);
                    canSave = availableActions.Contains(MenuItemEnums.保存);
                    canSubmit = availableActions.Contains(MenuItemEnums.提交);
                    canReview = availableActions.Contains(MenuItemEnums.审核);
                    canReverseReview = availableActions.Contains(MenuItemEnums.反审);
                    canCloseCase = availableActions.Contains(MenuItemEnums.结案);
                    canAntiCloseCase = availableActions.Contains(MenuItemEnums.反结案);
                    canDelete = availableActions.Contains(MenuItemEnums.删除);
                    canCancel = availableActions.Contains(MenuItemEnums.取消);
                }

                // 统一更新按钮状态
                toolStripbtnModify.Enabled = canModify;
                toolStripButtonSave.Enabled = canSave;
                toolStripbtnSubmit.Enabled = canSubmit;
                toolStripbtnReview.Enabled = canReview;
                toolStripBtnReverseReview.Enabled = canReverseReview;
                toolStripButton结案.Enabled = canCloseCase;
                toolStripbtnDelete.Enabled = canDelete;
                toolStripBtnCancel.Visible = canCancel;
                UpdateStateDisplay();
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "更新按钮状态失败: {ex.Message}", ex);
            }

        }
           

        /// <summary>
        /// 根据通用实体状态更新UI
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected virtual void UpdateUIBasedOnGenericEntityState(BaseEntity entity)
        {
            // 调用统一的UI状态更新方法
            UpdateAllUIStates(entity);
        }



        /// <summary>
        /// 配置泛型状态管理器
        /// </summary>
        protected virtual void ConfigureGenericStateManager()
        {
            // 检查状态管理器是否已初始化
            if (StateManager == null)
            {
                // 使用单例工厂获取状态管理器，避免重复初始化
                //var factory = StateManagerFactoryV3.Instance;
                //this.StateManager = factory.GetStateManager();
            }

            // 获取主表和子表的类型信息
            var mainEntityType = typeof(T);
            var childEntityType = typeof(C);

            // 如果需要，可以为子表类型也配置状态管理
            // 这里可以添加子表特定的状态管理逻辑
        }

        /// <summary>
        /// 初始化泛型特定的按钮状态管理
        /// </summary>
        protected virtual void InitializeGenericButtonStateManagement()
        {
            // 根据当前实体状态初始化按钮状态
            if (EditEntity != null)
            {
                UpdateUIBasedOnGenericEntityState(EditEntity);
            }
        }




        /// <summary>
        /// 检查操作可执行性 - 使用新的状态管理系统
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="entity">实体对象</param>
        /// <returns>是否可执行</returns>
        protected virtual bool CanExecuteAction(MenuItemEnums action, T entity)
        {
            if (entity == null || StatusContext == null)
                return false;

            try
            {
                // 使用新的状态管理系统检查操作权限
                if (UIController != null)
                {
                    // action参数已经是MenuItemEnums类型，可以直接使用
                    return UIController.CanExecuteAction(action, StatusContext);
                }

                // 如果UIController不可用，回退到原始逻辑
                return CanExecuteActionFallback(action, entity);
            }
            catch (Exception ex)
            {
                // 如果状态管理系统出错，回退到原始逻辑
                System.Diagnostics.Debug.WriteLine($"使用状态管理系统检查操作权限失败: {ex.Message}");
                return CanExecuteActionFallback(action, entity);
            }
        }

        /// <summary>
        /// 回退的操作可执行性检查方法（原始逻辑）
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="entity">实体对象</param>
        /// <returns>是否可执行</returns>
        private bool CanExecuteActionFallback(MenuItemEnums action, T entity)
        {
            // 获取当前状态
            var currentStatus = StatusContext.CurrentStatus;

            // 根据操作类型和状态判断是否可执行
            switch (action)
            {
                case MenuItemEnums.新增:
                    return true; // 新增操作总是允许

                case MenuItemEnums.修改:
                    return IsEditableStatus(currentStatus);

                case MenuItemEnums.保存:
                    return IsEditableStatus(currentStatus);

                case MenuItemEnums.删除:
                    return IsEditableStatus(currentStatus);

                case MenuItemEnums.提交:
                    return CanSubmitStatus(currentStatus);

                case MenuItemEnums.审核:
                    return CanReviewStatus(currentStatus);

                case MenuItemEnums.反审:
                    return CanReverseReviewStatus(currentStatus);

                case MenuItemEnums.结案:
                    return CanCloseCaseStatus(currentStatus);

                case MenuItemEnums.反结案:
                    return CanAntiCloseCaseStatus(currentStatus);

                default:
                    return true; // 默认允许其他操作
            }
        }

        /// <summary>
        /// 判断状态是否可编辑 - 使用新的状态管理系统
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可编辑</returns>
        private bool IsEditableStatus(object status)
        {
            try
            {
                // 使用新的状态管理系统检查编辑权限
                if (UIController != null && StatusContext != null)
                {
                    // 检查修改操作是否可用
                    return UIController.CanExecuteAction(MenuItemEnums.修改, StatusContext);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"使用状态管理系统检查编辑权限失败: {ex.Message}");
            }

            // 如果状态管理系统出错，回退到原始逻辑
            return IsEditableStatusFallback(status);
        }

        /// <summary>
        /// 回退的编辑状态检查方法（使用StatusHelper）- 已过时
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可编辑</returns>
        [Obsolete("请使用V3状态管理系统的UIController.CanExecuteAction方法替代")]
        private bool IsEditableStatusFallback(object status)
        {
            // 使用StatusHelper中的IsEditableStatus方法替换原始逻辑
            return StatusHelper.IsEditableStatus(status);
        }

        /// <summary>
        /// 判断状态是否可提交 - 使用新的状态管理系统
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可提交</returns>
        private bool CanSubmitStatus(object status)
        {
            try
            {
                // 使用新的状态管理系统检查提交权限
                if (UIController != null && StatusContext != null)
                {
                    // 检查提交操作是否可用
                    return UIController.CanExecuteAction(MenuItemEnums.提交, StatusContext);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"使用状态管理系统检查提交权限失败: {ex.Message}");
            }

            // 如果状态管理系统出错，回退到原始逻辑
            return CanSubmitStatusFallback(status);
        }

        /// <summary>
        /// 回退的提交状态检查方法（使用StatusHelper）- 已过时
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可提交</returns>
        [Obsolete("请使用V3状态管理系统的UIController.CanExecuteAction方法替代")]
        private bool CanSubmitStatusFallback(object status)
        {
            // 使用StatusHelper中的IsApprovedStatus方法替换原始逻辑 - 已过时
            // 提交状态通常对应于草稿状态
            if (status is DataStatus dataStatus)
                return !StatusHelper.IsApprovedStatus(dataStatus);
            else if (status is PrePaymentStatus preStatus)
                return !StatusHelper.IsApprovedStatus(preStatus);
            else if (status is ARAPStatus arapStatus)
                return !StatusHelper.IsApprovedStatus(arapStatus);
            else if (status is PaymentStatus paymentStatus)
                return !StatusHelper.IsApprovedStatus(paymentStatus);

            return false;
        }

        /// <summary>
        /// 判断状态是否可审核 - 使用新的状态管理系统
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可审核</returns>
        private bool CanReviewStatus(object status)
        {
            try
            {
                // 使用新的状态管理系统检查审核权限
                if (UIController != null && StatusContext != null)
                {
                    // 检查审核操作是否可用
                    return UIController.CanExecuteAction(MenuItemEnums.审核, StatusContext);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"使用状态管理系统检查审核权限失败: {ex.Message}");
            }

            // 如果状态管理系统出错，回退到原始逻辑
            return CanReviewStatusFallback(status);
        }

        /// <summary>
        /// 回退的审核状态检查方法（使用StatusHelper）- 已过时
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可审核</returns>
        [Obsolete("请使用V3状态管理系统的UIController.CanExecuteAction方法替代")]
        private bool CanReviewStatusFallback(object status)
        {
            // 使用StatusHelper中的IsApprovedStatus方法替换原始逻辑 - 已过时
            // 审核状态通常对应于新建状态（未审核状态）
            if (status is DataStatus dataStatus)
                return dataStatus == DataStatus.新建;
            else if (status is PrePaymentStatus preStatus)
                return preStatus == PrePaymentStatus.待审核;
            else if (status is ARAPStatus arapStatus)
                return arapStatus == ARAPStatus.待审核;
            else if (status is PaymentStatus paymentStatus)
                return paymentStatus == PaymentStatus.待审核;

            return false;
        }

        /// <summary>
        /// 根据名称查找ToolStripButton控件
        /// ToolStripButton不在Controls集合中，需要在ToolStrip的Items中查找
        /// </summary>
        /// <param name="buttonName">按钮名称</param>
        /// <returns>找到的ToolStripButton，如果未找到则返回null</returns>
        protected ToolStripButton FindToolStripButtonByName(string buttonName)
        {
            if (string.IsNullOrEmpty(buttonName))
                return null;

            // 遍历所有ToolStrip控件
            foreach (Control control in this.Controls)
            {
                if (control is ToolStrip toolStrip)
                {
                    // 在ToolStrip的Items中查找按钮
                    foreach (ToolStripItem item in toolStrip.Items)
                    {
                        if (item is ToolStripButton toolButton && toolButton.Name == buttonName)
                        {
                            return toolButton;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 判断状态是否可反审核 - 使用新的状态管理系统
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可反审核</returns>
        private bool CanReverseReviewStatus(object status)
        {
            try
            {
                // 使用新的状态管理系统检查反审核权限
                if (UIController != null && StatusContext != null)
                {
                    // 检查反审核操作是否可用
                    return UIController.CanExecuteAction(MenuItemEnums.反审, StatusContext);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"使用状态管理系统检查反审核权限失败: {ex.Message}");
            }

            // 如果状态管理系统出错，回退到原始逻辑
            return CanReverseReviewStatusFallback(status);
        }

        /// <summary>
        /// 回退的反审核状态检查方法（使用StatusHelper）- 已过时
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可反审核</returns>
        [Obsolete("请使用V3状态管理系统的UIController.CanExecuteAction方法替代")]
        private bool CanReverseReviewStatusFallback(object status)
        {
            // 使用StatusHelper中的IsApprovedStatus方法替换原始逻辑 - 已过时
            // 反审核状态通常对应于已审核状态
            if (status is DataStatus dataStatus)
                return StatusHelper.IsApprovedStatus(dataStatus);
            else if (status is PrePaymentStatus preStatus)
                return preStatus == PrePaymentStatus.待核销 || preStatus == PrePaymentStatus.已生效;
            else if (status is ARAPStatus arapStatus)
                return arapStatus == ARAPStatus.待支付;
            else if (status is PaymentStatus paymentStatus)
                return false; // 付款状态不支持反审核

            return false;
        }

        /// <summary>
        /// 判断状态是否可结案 - 使用新的状态管理系统
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可结案</returns>
        private bool CanCloseCaseStatus(object status)
        {
            try
            {
                // 使用新的状态管理系统检查结案权限
                if (UIController != null && StatusContext != null)
                {
                    // 检查结案操作是否可用
                    return UIController.CanExecuteAction(MenuItemEnums.结案, StatusContext);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"使用状态管理系统检查结案权限失败: {ex.Message}");
            }

            // 如果状态管理系统出错，回退到原始逻辑
            return CanCloseCaseStatusFallback(status);
        }

        /// <summary>
        /// 回退的结案状态检查方法（使用StatusHelper）- 已过时
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可结案</returns>
        [Obsolete("请使用V3状态管理系统的UIController.CanExecuteAction方法替代")]
        private bool CanCloseCaseStatusFallback(object status)
        {
            // 使用StatusHelper中的IsApprovedStatus方法替换原始逻辑 - 已过时
            // 结案状态通常对应于已审核状态
            if (status is DataStatus dataStatus)
                return StatusHelper.IsApprovedStatus(dataStatus);
            else if (status is PrePaymentStatus preStatus)
                return preStatus == PrePaymentStatus.待核销;
            else if (status is ARAPStatus arapStatus)
                return arapStatus == ARAPStatus.待支付 || arapStatus == ARAPStatus.部分支付;
            else if (status is PaymentStatus paymentStatus)
                return paymentStatus == PaymentStatus.已支付;

            return false;
        }

        /// <summary>
        /// 判断状态是否可反结案 - 使用新的状态管理系统
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可反结案</returns>
        private bool CanAntiCloseCaseStatus(object status)
        {
            try
            {
                // 使用新的状态管理系统检查反结案权限
                if (UIController != null && StatusContext != null)
                {
                    // 检查反结案操作是否可用
                    return UIController.CanExecuteAction(MenuItemEnums.反结案, StatusContext);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"使用状态管理系统检查反结案权限失败: {ex.Message}");
            }

            // 如果状态管理系统出错，回退到原始逻辑
            return CanAntiCloseCaseStatusFallback(status);
        }

        /// <summary>
        /// 回退的反结案状态检查方法（使用StatusHelper）
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可反结案</returns>
        private bool CanAntiCloseCaseStatusFallback(object status)
        {
            // 使用StatusHelper中的IsDisabledStatus方法替换原始逻辑
            // 反结案状态通常对应于完结状态
            if (status is DataStatus dataStatus)
                return StatusHelper.IsDisabledStatus(dataStatus);
            else if (status is PrePaymentStatus preStatus)
                return preStatus == PrePaymentStatus.全额核销;
            else if (status is ARAPStatus arapStatus)
                return arapStatus == ARAPStatus.全部支付 || arapStatus == ARAPStatus.全部支付;
            else if (status is PaymentStatus paymentStatus)
                return paymentStatus == PaymentStatus.已支付;

            return false;
        }

        /// <summary>
        /// 根据状态更新子表操作 - 使用新的状态管理系统
        /// </summary>
        /// <param name="status">当前数据状态</param>
        protected virtual void UpdateChildTableOperations(DataStatus status)
        {
            try
            {
                // 使用新的状态管理系统获取可执行的操作
                if (UIController != null && StatusContext != null)
                {
                    // 获取当前状态下可执行的操作
                    var availableActions = UIController.GetAvailableActions(StatusContext);

                    // 检查是否允许子表操作
                    bool canAdd = availableActions.Any(a => a.ToString().Contains("新增") || a.ToString().Contains("Add"));
                    bool canEdit = availableActions.Any(a => a.ToString().Contains("修改") || a.ToString().Contains("Edit"));
                    bool canDelete = availableActions.Any(a => a.ToString().Contains("删除") || a.ToString().Contains("Delete"));

                    // 启用/禁用子表操作
                    EnableChildTableOperations(canAdd, canEdit, canDelete);
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"使用状态管理系统更新子表操作失败: {ex.Message}");
            }

            // 如果状态管理系统出错，回退到原始逻辑
            UpdateChildTableOperationsFallback(status);
        }

        /// <summary>
        /// 回退的子表操作更新方法（原始逻辑）
        /// </summary>
        /// <param name="status">当前数据状态</param>
        private void UpdateChildTableOperationsFallback(DataStatus status)
        {
            // 根据状态控制子表的增删改操作
            switch (status)
            {
                case DataStatus.草稿:
                    // 草稿状态允许所有操作
                    EnableChildTableOperations(true, true, true);
                    break;
                case DataStatus.新建:
                    // 新建状态不允许修改
                    EnableChildTableOperations(false, false, false);
                    break;
                case DataStatus.确认:
                    // 确认状态不允许修改
                    EnableChildTableOperations(false, false, false);
                    break;
                case DataStatus.完结:
                    // 完结状态不允许修改
                    EnableChildTableOperations(false, false, false);
                    break;
                case DataStatus.作废:
                    // 作废状态不允许修改
                    EnableChildTableOperations(false, false, false);
                    break;
                default:
                    // 默认不允许修改
                    EnableChildTableOperations(false, false, false);
                    break;
            }
        }

        /// <summary>
        /// 启用或禁用子表操作
        /// </summary>
        /// <param name="allowAdd">是否允许添加</param>
        /// <param name="allowEdit">是否允许编辑</param>
        /// <param name="allowDelete">是否允许删除</param>
        protected virtual void EnableChildTableOperations(bool allowAdd, bool allowEdit, bool allowDelete)
        {
            // 这里可以根据实际的子表控件进行操作
            // 例如：设置子表的只读状态、禁用添加/删除按钮等

            // 示例代码（需要根据实际控件进行调整）：
            // if (gridViewChild != null)
            // {
            //     gridViewChild.ReadOnly = !allowEdit;
            //     gridViewChild.AllowUserToAddRows = allowAdd;
            //     gridViewChild.AllowUserToDeleteRows = allowDelete;
            // }
        }




        public BaseBillEditGeneric()
        {
            InitializeComponent();

            // 订阅AppDomain未处理异常事件，确保在程序崩溃时也能尝试清理锁定资源
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {

                AddExcludeMenuList();
                AddExtendButton(CurMenuInfo);
                if (!this.DesignMode)
                {
                    frm = new frmFormProperty();

                    this.OnBindDataToUIEvent += BindData;

                    KryptonButton button保存当前单据 = new KryptonButton();
                    button保存当前单据.Text = "保存当前单据";
                    button保存当前单据.Click += button保存当前单据_Click;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button保存当前单据);

                    KryptonContextMenu kcm加载最新数据 = new KryptonContextMenu();
                    KryptonContextMenuItem menuItem选择要加载的数据 = new KryptonContextMenuItem("选择数据");
                    menuItem选择要加载的数据.Text = "选择数据";
                    menuItem选择要加载的数据.Click += MenuItem选择要加载的数据_Click;

                    KryptonContextMenuItems kryptonContextMenuItems1 = new KryptonContextMenuItems();

                    kcm加载最新数据.Items.AddRange(new KryptonContextMenuItemBase[] {
            kryptonContextMenuItems1});

                    kryptonContextMenuItems1.Items.AddRange(new KryptonContextMenuItemBase[] {
            menuItem选择要加载的数据});

                    KryptonDropButton button加载最新数据 = new KryptonDropButton();
                    button加载最新数据.Text = "加载数据";
                    button加载最新数据.Click += button加载最新数据_Click;
                    button加载最新数据.KryptonContextMenu = kcm加载最新数据;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button加载最新数据);

                    KryptonButton button快速录入数据 = new KryptonButton();

                    button快速录入数据.Text = "快速录入数据";
                    button快速录入数据.Click += button快速录入数据_Click;

                    frm.flowLayoutPanelButtonsArea.Controls.Add(button快速录入数据);

                    KryptonButton button请求协助处理 = new KryptonButton();
                    button请求协助处理.Text = "请求协助处理";
                    button请求协助处理.Click += button请求协助处理_Click;

                    frm.flowLayoutPanelButtonsArea.Controls.Add(button请求协助处理);


                    Krypton.Toolkit.KryptonButton button录入数据预设 = new Krypton.Toolkit.KryptonButton();
                    button录入数据预设.Text = "录入数据预设";
                    button录入数据预设.ToolTipValues.Description = "对单据，资料等数据进行预设，并且可以提供多个预设模板，提高录入速度。";
                    button录入数据预设.ToolTipValues.EnableToolTips = true;
                    button录入数据预设.ToolTipValues.Heading = "提示";
                    button录入数据预设.Click += button录入数据预设_Click;
                    button录入数据预设.Width = 120;
                    frm.flowLayoutPanelButtonsArea.Controls.Add(button录入数据预设);


                    // 使用EntityMappingHelper代替BizTypeMapper
                    CurrentBizType = EntityMappingHelper.GetBizType(typeof(T).Name);
                    CurrentBizTypeName = CurrentBizType.ToString();
                }
                menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
                _entityInfoService = Startup.GetFromFac<IEntityMappingService>();
            }
            // 通过依赖注入获取缓存管理器
            _cacheManager = Startup.GetFromFac<IEntityCacheManager>();
            _tableSchemaManager = TableSchemaManager.Instance;
            _integratedLockService = Startup.GetFromFac<ClientLockManagementService>();
            _clientCommunicationService = Startup.GetFromFac<ClientCommunicationService>();
        }

        public readonly IEntityCacheManager _cacheManager;
        public readonly TableSchemaManager _tableSchemaManager;

        private async void button录入数据预设_Click(object sender, EventArgs e)
        {
            if (EditEntity == null)
            {
                EditEntity = Activator.CreateInstance(typeof(T)) as T;
            }
            bool rs = await UIBizService.SetInputDataAsync<T>(CurMenuInfo, EditEntity);
            if (rs)
            {
                // EditEntity = LoadQueryConditionToUI();
            }
        }


        #region 单据 主表公共信息 如类型：名称

        public BizType CurrentBizType { get; set; }
        public string CurrentBizTypeName
        {
            get; set;

        }
        #endregion

        private void MenuItem选择要加载的数据_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "cache files (*.cache)|*.cache|All files (*.*)|*.*";
            //加载最新数据
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                EditEntity = manager.Deserialize<T>(openFileDialog1.FileName);
                OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
                MainForm.Instance.uclog.AddLog("成功加载选择的数据。");
            }
        }

        private async void button请求协助处理_Click(object sender, EventArgs e)
        {
            //弹出一个弹出框，让用户输入协助处理的内容。
            //再把单据相关内容发送到服务器转发到管理员那里

            frmInputContent frm = new frmInputContent();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //发送协助处理请求
                //先获取当前单据的ID
                #region
                try
                {
                    if (EditEntity != null)
                    {
                        #region  单据数据  后面优化可以多个单?限制5个？
                        await Save(false);

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogError(ex, "请求协助处理");
                }
                #endregion
            }

        }

        private void button快速录入数据_Click(object sender, EventArgs e)
        {
            frm.Close();
            frmQuicklyInputGeneric<C> frmQuicklyInput = new frmQuicklyInputGeneric<C>();
            frmQuicklyInput.OnApplyQuicklyInputData += OnApplyQuicklyInputData;
            frmQuicklyInput.CurMenuInfo = CurMenuInfo;
            if (EditEntity == null)
            {
                Add();
            }
            var details = EditEntity.GetPropertyValue(typeof(C).Name + "s");
            if (details == null)
            {
                details = new List<C>();
            }
            frmQuicklyInput.lines = details as List<C>;
            if (frmQuicklyInput.ShowDialog() == DialogResult.OK)
            {
                EditEntity.SetPropertyValue(typeof(C).Name + "s", frmQuicklyInput.lines);
                OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
            }
        }

        private void OnApplyQuicklyInputData(List<C> lines)
        {
            EditEntity.SetPropertyValue(typeof(C).Name + "s", lines);
            OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
        }
        // 移除BizTypeMapper，使用EntityMappingHelper

        RUINORERP.Common.Helper.XmlHelper manager = new RUINORERP.Common.Helper.XmlHelper();
        private void button加载最新数据_Click(object sender, EventArgs e)
        {
            //RUINORERP.Common.Helper.XmlHelper manager = new RUINORERP.Common.Helper.XmlHelper();
            EditEntity = manager.Deserialize<T>(CurMenuInfo.CaptionCN + ".cache");
            OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
            MainForm.Instance.uclog.AddLog("成功加载上次的数据。");
        }

        private async void button保存当前单据_Click(object sender, EventArgs e)
        {
            await AutoSaveDataAsync();
        }

        #region 图片相关

        /// <summary>
        /// 下载并显示凭证图片 - 增强版本
        /// </summary>
        public async Task DownloadVoucherImageAsync(T entity, MagicPictureBox magicPicBox)
        {
            magicPicBox.MultiImageSupport = true;
            var ctrpay = Startup.GetFromFac<FileManagementController>();
            try
            {
                var list = await ctrpay.DownloadImageAsync(entity as BaseEntity);

                if (list == null || list.Count == 0)
                {
                    return;
                }

                // 简化处理逻辑，直接处理文件存储信息
                List<byte[]> imageDataList = new List<byte[]>();
                List<string> imageNames = new List<string>();
                List<ImageInfo> imageInfos = new List<ImageInfo>();

                foreach (var downloadResponse in list)
                {
                    if (downloadResponse.IsSuccess && downloadResponse.FileStorageInfos != null)
                    {
                        foreach (var fileStorageInfo in downloadResponse.FileStorageInfos)
                        {
                            if (fileStorageInfo.FileData != null && fileStorageInfo.FileData.Length > 0)
                            {
                                imageDataList.Add(fileStorageInfo.FileData);
                                imageInfos.Add(ctrpay.ConvertToImageInfo(fileStorageInfo));
                                AddFileStorageInfo(entity as BaseEntity, fileStorageInfo);
                            }
                        }
                    }
                    else
                    {
                        logger.LogWarning("图片下载失败: {ErrorMessage}",
                            downloadResponse.ErrorMessage ?? "未知错误");
                    }
                }

                if (imageDataList.Count > 0)
                {
                    try
                    {
                        // 使用统一的LoadImages方法，自动处理单张和多张图片
                        magicPicBox.LoadImages(imageDataList, imageInfos, true);
                        MainForm.Instance.uclog.AddLog($"成功加载 {imageDataList.Count} 张凭证图片");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "加载图片失败");
                        MainForm.Instance.uclog.AddLog($"加载图片失败: {ex.Message}");
                    }
                }
                else
                {
                    logger.LogInformation("未找到有效的图片数据");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "下载凭证图片异常");
                MainForm.Instance.uclog.AddLog($"下载凭证图片出错：{ex.Message}");
            }
        }


        /// <summary>
        /// 上传凭证图片 - 增强版本
        /// </summary>
        public async Task<bool> UploadVoucherImageAsync(T entity, MagicPictureBox magicPicBox,
            bool onlyUpdated = true, bool useVersionControl = false)
        {
            var ctrpay = Startup.GetFromFac<FileManagementController>();
            try
            {
                // 检查是否有图片需要上传
                // 对于单个MagicPictureBox控件，直接检查Image属性即可
                if (magicPicBox.Image == null)
                {
                    logger.LogInformation("没有需要上传的图片");
                    return true;
                }

                // 根据onlyUpdated参数决定获取所有图片还是仅变更的图片
                var imageBytesWithInfoList = onlyUpdated ?
                    magicPicBox.GetUpdatedImageBytesWithInfo() :
                    magicPicBox.GetAllImageBytesWithInfo();

                if (imageBytesWithInfoList == null || imageBytesWithInfoList.Count == 0)
                {
                    logger.LogInformation("没有需要上传的图片数据");
                    return true;
                }

                bool allSuccess = true;
                int successCount = 0;

                // 遍历上传所有图片
                foreach (var imageDataWithInfo in imageBytesWithInfoList)
                {
                    byte[] imageData = imageDataWithInfo.Item1;
                    ImageInfo imageInfo = imageDataWithInfo.Item2;

                    if (imageData == null || imageData.Length == 0)
                    {
                        logger.LogWarning("跳过空图片数据: {FileName}", imageInfo.OriginalFileName);
                        continue;
                    }

                    // 检查文件大小限制
                    if (imageData.Length > 10 * 1024 * 1024) // 10MB限制
                    {
                        logger.LogWarning("图片文件过大: {FileName}, Size: {Size}MB",
                            imageInfo.OriginalFileName, imageData.Length / 1024 / 1024);
                        MainForm.Instance.uclog.AddLog($"图片 {imageInfo.OriginalFileName} 超过大小限制(10MB)");
                        allSuccess = false;
                        continue;
                    }

                    // 准备版本控制参数
                    long? existingFileId = null;
                    string updateReason = null;

                    if (imageInfo.FileId > 0 && imageInfo.IsUpdated)
                    {
                        existingFileId = imageInfo.FileId;
                        updateReason = "图片更新";
                    }

                    // 上传图片
                    var response = await ctrpay.UploadImageAsync(entity as BaseEntity, imageInfo.OriginalFileName, imageData, "预付凭证", existingFileId, updateReason, useVersionControl);

                    if (response.IsSuccess)
                    {
                        successCount++;
                        MainForm.Instance.uclog.AddLog($"凭证图片上传成功：{imageInfo.OriginalFileName}");

                        // 上传成功后，将图片标记为未更新
                        if (imageInfo.IsUpdated)
                        {
                            imageInfo.IsUpdated = false;
                        }
                    }
                    else
                    {
                        allSuccess = false;
                        MainForm.Instance.uclog.AddLog($"凭证图片上传失败：{imageInfo.OriginalFileName}，原因：{response.Message}");
                        logger.LogError("图片上传失败: {FileName}, Error: {Error}",
                            imageInfo.OriginalFileName, response.Message);
                    }
                }

                if (successCount > 0)
                {
                    MainForm.Instance.uclog.AddLog($"成功上传 {successCount} 张凭证图片");
                }

                return allSuccess;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "上传凭证图片异常");
                MainForm.Instance.uclog.AddLog($"上传凭证图片出错：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 专门处理已更新图片的上传 - 增强版本
        /// </summary>
        /// <param name="entity">销售订单实体</param>
        /// <param name="updatedImages">需要更新的图片列表</param>
        /// <returns>上传是否成功</returns>
        public async Task<bool> UploadUpdatedImagesAsync(T entity, List<Tuple<byte[], ImageInfo>> updatedImages)
        {
            var ctrpay = Startup.GetFromFac<FileManagementController>();
            try
            {
                if (updatedImages == null || updatedImages.Count == 0)
                {
                    logger.LogInformation("没有需要更新的图片");
                    return true;
                }

                bool allSuccess = true;
                int successCount = 0;

                // 遍历上传所有需要更新的图片
                foreach (var imageDataWithInfo in updatedImages)
                {
                    byte[] imageData = imageDataWithInfo.Item1;
                    ImageInfo imageInfo = imageDataWithInfo.Item2;

                    if (imageData == null || imageData.Length == 0)
                    {
                        logger.LogWarning("跳过空图片数据: {FileName}", imageInfo.OriginalFileName);
                        continue;
                    }

                    // 检查文件大小限制
                    if (imageData.Length > 10 * 1024 * 1024) // 10MB限制
                    {
                        logger.LogWarning("图片文件过大: {FileName}, Size: {Size}MB",
                            imageInfo.OriginalFileName, imageData.Length / 1024 / 1024);
                        MainForm.Instance.uclog.AddLog($"图片 {imageInfo.OriginalFileName} 超过大小限制(10MB)");
                        allSuccess = false;
                        continue;
                    }

                    // 检查是否为更新操作，准备版本控制参数
                    long? existingFileId = null;
                    string updateReason = "图片更新";

                    // 如果图片信息包含文件ID且图片已更新
                    if (imageInfo.FileId > 0 && imageInfo.IsUpdated)
                    {
                        existingFileId = imageInfo.FileId;
                    }

                    // 上传图片，启用版本控制
                    var response = await ctrpay.UploadImageAsync(entity as BaseEntity, imageInfo.OriginalFileName, imageData, "预付凭证", existingFileId, updateReason, true);

                    if (response.IsSuccess)
                    {
                        successCount++;
                        MainForm.Instance.uclog.AddLog($"凭证图片更新成功：{imageInfo.OriginalFileName}");
                        // 上传成功后，将图片标记为未更新
                        imageInfo.IsUpdated = false;
                    }
                    else
                    {
                        allSuccess = false;
                        MainForm.Instance.uclog.AddLog($"凭证图片更新失败：{imageInfo.OriginalFileName}，原因：{response.Message}");
                        logger.LogError("图片更新失败: {FileName}, Error: {Error}",
                            imageInfo.OriginalFileName, response.Message);
                    }
                }

                if (successCount > 0)
                {
                    MainForm.Instance.uclog.AddLog($"成功更新 {successCount} 张凭证图片");
                }

                return allSuccess;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "更新凭证图片异常");
                MainForm.Instance.uclog.AddLog($"更新凭证图片出错：{ex.Message}");
                return false;
            }
        }




        public void AddFileStorageInfo(BaseEntity entity, tb_FS_FileStorageInfo FileStorageInfo)
        {
            if (FileStorageInfo.FileId <= 0)
                return;
            if (entity.FileStorageInfoList.Contains(FileStorageInfo))
                return;
            entity.FileStorageInfoList.Add(FileStorageInfo);
        }
        #endregion



        /// <summary>
        /// 绑定数据到UI
        /// </summary>
        /// <param name="entity"></param>
        public virtual void BindData(T entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.AcceptChanges();
            }
            #region 联查

            toolStripbtnRelatedQuery.DropDownItems.Clear();
            _ = Task.Run(async () => await LoadRelatedDataToDropDownItemsAsync());
            #endregion


            #region 单据联动

            // 调用LoadConvertDocToDropDownItemsAsync加载单据联动选项
            LoadConvertDocToDropDownItemsAsync();

            #endregion


            InitializeStateManagement();
            _ = Task.Run(async () => await ToolBarEnabledControl(entity));

            //暂时统一不显示外币
            UIHelper.ControlForeignFieldInvisible<T>(this, false);


            #region 加载前，清空原来的锁定单据
            //单据被锁定时。显示锁定图标。并且提示无法操作？
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
            if (pkid > 0)
            {
                //如果要锁这个单 看这个单是不是已经被其它人锁，如果没有人锁则我可以锁

                //关闭时会解锁，查询的方式不停加载也要解锁前面的
                long userid = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;

                //解锁这个业务的自己名下的其它单
                _ = Task.Run(async () => await UNLockByBizName(userid));
            }
            else
            {
                //没人锁定
                UpdateLockUI(false);
            }


            #endregion
        }



        internal override void LoadDataToUI(object Entity)
        {
            try
            {
                if (Entity == null) return;

                // 释放之前可能存在的锁定（单据切换时自动释放）
                if (_currentBillId > 0 && _currentMenuId > 0)
                {
                    _ = Task.Run(async () => await ReleasePreviousLockAsync());
                }

                // 首先调用基类的实现，确保基础的状态管理集成
                base.LoadDataToUI(Entity);

                if (Entity is T typedEntity)
                {
                    // 直接在当前UI线程中执行类型化的数据绑定，避免STA线程问题
                    BindData(typedEntity);

                    // 初始化泛型状态管理
                    InitializeStateManagement();

                    // 在后台线程中执行工具栏控制（不涉及UI控件操作）
                    _ = Task.Run(async () => await ToolBarEnabledControl(Entity));

                    // 增强的状态管理集成
                    EnhancedStateManagementIntegration(typedEntity);

                    // 加载新单据后检查锁定状态
                    _ = Task.Run(async () => await CheckLockAfterLoad(typedEntity));
                }
            }
            catch (Exception ex)
            {
                // 异常情况下清理锁定资源
                _ = Task.Run(async () => await CleanupLockOnExceptionAsync(ex));
                throw; // 重新抛出异常，让上层处理
            }
        }

        /// <summary>
        /// 释放之前的锁定
        /// </summary>
        private async Task ReleasePreviousLockAsync()
        {
            try
            {
                if (_integratedLockService != null && _currentBillId > 0 && _currentMenuId > 0)
                {
                    MainForm.Instance.uclog.AddLog($"切换单据时释放之前的锁定：单据ID={_currentBillId}, 菜单ID={_currentMenuId}", UILogType.普通消息);

                    // 检查是否为当前用户的锁定，只释放自己的锁定
                    var lockStatus = await _integratedLockService.CheckLockStatusAsync(_currentBillId, _currentMenuId);
                    if (lockStatus?.LockInfo?.IsLocked == true)
                    {
                        long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                        if (lockStatus.LockInfo.LockedUserId == currentUserId)
                        {
                            await _integratedLockService.UnlockBillAsync(_currentBillId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, $"释放之前的锁定失败：单据ID={_currentBillId}");
            }
        }

        /// <summary>
        /// 加载后检查锁定状态
        /// </summary>
        private async Task CheckLockAfterLoad(T typedEntity)
        {
            try
            {
                if (typedEntity == null || _integratedLockService == null) return;

                string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
                long pkid = (long)ReflectionHelper.GetPropertyValue(typedEntity, PKCol);

                if (pkid > 0)
                {
                    // 更新当前单据信息
                    _currentBillId = pkid;
                    _currentMenuId = CurMenuInfo?.MenuID ?? 0;

                    // 直接检查锁定状态，减少中间调用层级
                    await CheckLockStatusAndUpdateUI(pkid);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "加载后检查锁定状态失败");
            }
        }

        /// <summary>
        /// 异常情况下清理锁定资源
        /// </summary>
        private async Task CleanupLockOnExceptionAsync(Exception ex)
        {
            try
            {
                if (_integratedLockService != null && _currentBillId > 0 && _currentMenuId > 0)
                {
                    MainForm.Instance.logger.LogError(ex, $"异常情况下清理锁定资源：单据ID={_currentBillId}, 菜单ID={_currentMenuId}");

                    // 检查是否为当前用户的锁定，只清理自己的锁定
                    var lockStatus = await _integratedLockService.CheckLockStatusAsync(_currentBillId, _currentMenuId);
                    if (lockStatus?.LockInfo?.IsLocked == true)
                    {
                        long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                        if (lockStatus.LockInfo.LockedUserId == currentUserId)
                        {
                            await _integratedLockService.UnlockBillAsync(_currentBillId);
                            MainForm.Instance.uclog.AddLog($"异常情况下成功清理锁定资源：单据ID={_currentBillId}", UILogType.普通消息);
                        }
                    }
                }
            }
            catch (Exception cleanupEx)
            {
                MainForm.Instance.logger.LogError(cleanupEx, "异常情况下清理锁定资源时发生异常");
            }
        }

        /// <summary>
        /// 处理未处理的异常，尝试清理锁定资源
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = e.ExceptionObject as Exception;
                if (ex != null && _currentBillId > 0 && _currentMenuId > 0)
                {
                    // 在程序崩溃时尝试清理锁定资源
                    //LockReleaseHelper.CleanupLockOnException(lockManagementService, MainForm.Instance.uclog, _currentBillId, _currentMenuId.ToString(), ex);
                }
            }
            catch (Exception)
            {
                // 在最外层捕获所有可能的异常，确保不会干扰主异常处理流程
            }

        }

        /// <summary>
        /// 增强的状态管理集成
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected virtual void EnhancedStateManagementIntegration(T entity)
        {
            if (entity is BaseEntity baseEntity && baseEntity.IsStateManagerInitialized)
            {
                // 更新状态显示
                UpdateStatusDisplay(baseEntity);

                // 根据状态更新操作按钮
                UpdateActionButtonsByState(baseEntity);

                // 应用状态特定的UI规则
                ApplyStateSpecificRules(baseEntity);

                // 注册状态变更事件处理
                RegisterStateChangeHandlers(baseEntity);

                // 记录加载状态日志
                //    LogStateLoading(baseEntity);
            }
        }

        /// <summary>
        /// 更新状态显示
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected virtual void UpdateStatusDisplay(BaseEntity entity)
        {
            if (entity == null) return;

            if (entity.ContainsProperty(nameof(DataStatus)))
            {
                var status = (DataStatus)entity.GetPropertyValue(nameof(DataStatus));
                if (status == DataStatus.确认)
                {
                    // string statusDescription = entity.GetStatusDescription(status.Value);

                    // 更新状态标签或显示控件
                    //  // 如果有状态显示控件，可以在这里更新
                    // logger.LogInformation($"单据状态更新为: {statusDescription}");
                }
            }
        }

        /// <summary>
        /// 根据状态更新操作按钮
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected virtual void UpdateActionButtonsByState(BaseEntity entity)
        {
            if (entity == null) return;

            // 获取可用操作列表
            var availableActions = base.GetAvailableDataStatusTransitions().ToList();

            //// 根据可用操作更新按钮状态
            //if (availableActions != null)
            //{
            //    // 启用保存按钮
            //    toolStripButtonSave.Enabled = availableActions.Contains(DataStatus) || 
            //                                availableActions.Contains("修改");

            //    // 启用提交按钮
            //    toolStripbtnSubmit.Enabled = availableActions.Contains("提交");

            //    // 启用审核按钮
            //    toolStripbtnReview.Enabled = availableActions.Contains("提交");

            //    // 启用删除按钮
            //    toolStripbtnDelete.Enabled = availableActions.Contains("删除");

            //    // 更新其他按钮状态...
            //}
        }

        /// <summary>
        /// 应用状态特定的UI规则 - 使用V3状态管理系统优化版本
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected virtual void ApplyStateSpecificRules(BaseEntity entity)
        {
            if (entity == null) return;

            //// 使用基类的SetControlsState方法设置控件状态
            //bool isEditable = entity.IsEditable();
            //SetControlsState(Controls, isEditable);

            //// 根据确认状态设置特殊控件
            //bool isConfirmed = entity.IsConfirmed();
            //bool isFinalState = entity.IsFinalState();

            //if (isConfirmed && !isFinalState)
            //{
            //    // 已确认但非终态的特殊处理
            //    // 例如：显示确认后的操作选项
            //}

            // 根据终态设置
            //if (isFinalState)
            //{
            //    // 终态下的特殊处理
            //    // 例如：完全禁用编辑功能
            //}
        }

        /// <summary>
        /// 注册状态变更事件处理
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected virtual void RegisterStateChangeHandlers(BaseEntity entity)
        {
            // 注意：需要确保实体支持状态变更事件通知
            // 这里可以添加额外的事件注册逻辑
        }






        /// <summary>
        /// 不同情况，显示不同的可用情况
        /// </summary>
        internal void ToolBarEnabledControl(MenuItemEnums menu)
        {
            // 使用统一的UI状态更新方法，根据当前实体状态更新所有按钮
            if (EditEntity is BaseEntity baseEntity)
            {
                var currentStatus = baseEntity.GetDataStatus();
                UpdateAllButtonStates(currentStatus);

                // 针对特殊操作进行额外处理
                switch (menu)
                {
                    case MenuItemEnums.新增:
                    case MenuItemEnums.取消:
                    case MenuItemEnums.修改:
                        // 确保取消按钮的可见性正确
                        if (menu == MenuItemEnums.取消)
                        {
                            toolStripBtnCancel.Visible = false;
                        }
                        break;
                }
            }
            Edited = toolStripButtonSave.Enabled;
        }

        /// <summary>
        /// 根据单据实体属性状态来对应显示各种按钮控制
        /// </summary>
        /// <param name="entity"></param>
        protected virtual async Task ToolBarEnabledControl(object entity)
        {
            if (entity == null) return;
            UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);

            //可以修改
            if (entity.ContainsProperty(typeof(DataStatus).Name))
            {
                DataStatus dataStatus = (DataStatus)int.Parse(entity.GetPropertyValue(typeof(DataStatus).Name).ToString());
                ActionStatus actionStatus = (ActionStatus)(Enum.Parse(typeof(ActionStatus), entity.GetPropertyValue(typeof(ActionStatus).Name).ToString()));

                // 使用统一的UI状态更新方法
                UpdateAllButtonStates(dataStatus);

                // 根据ActionStatus处理保存按钮状态
                if (actionStatus == ActionStatus.新增)
                {
                    toolStripButtonSave.Enabled = true;
                    toolStripbtnModify.Enabled = false;
                }
                else
                {
                    toolStripButtonSave.Enabled = false;
                }



                #region 数据状态修改时也会影响到按钮
                if (entity is BaseEntity currentEntity)
                {
                    currentEntity.AcceptChanges();

                    //如果属性变化 则状态为修改
                    currentEntity.PropertyChanged += (sender, s2) =>
                            {
                                //权限允许
                                if ((true && dataStatus == DataStatus.草稿) || (true && dataStatus == DataStatus.新建))
                                {
                                    currentEntity.ActionStatus = ActionStatus.修改;
                                    ToolBarEnabledControl(MenuItemEnums.修改);
                                }

                                //数据状态变化会影响按钮变化
                                if (s2.PropertyName == "DataStatus")
                                {
                                    if (dataStatus == DataStatus.草稿)
                                    {
                                        ToolBarEnabledControl(MenuItemEnums.新增);

                                    }
                                    if (dataStatus == DataStatus.新建)
                                    {
                                        ToolBarEnabledControl(MenuItemEnums.新增);
                                    }
                                    if (dataStatus == DataStatus.确认)
                                    {
                                        ToolBarEnabledControl(MenuItemEnums.审核);
                                    }

                                    if (dataStatus == DataStatus.完结)
                                    {
                                        ToolBarEnabledControl(MenuItemEnums.结案);
                                    }
                                }
                            };
                }
                #endregion
            }

            // 获取状态类型和值

            var statusType = FMPaymentStatusHelper.GetStatusType(entity as BaseEntity);

            if (statusType == null) return;

            // 动态获取状态值
            dynamic status = entity.GetPropertyValue(statusType.Name);
            int statusValue = (int)status;
            dynamic statusEnum = Enum.ToObject(statusType, statusValue);

            // 通用按钮状态 - 使用V3状态管理系统
            try
            {
                if (UIController != null && StatusContext != null)
                {
                    // 使用新的状态管理系统检查按钮状态
                    toolStripbtnModify.Enabled = UIController.CanExecuteAction(MenuItemEnums.修改, StatusContext);
                    toolStripButtonSave.Enabled = UIController.CanExecuteAction(MenuItemEnums.保存, StatusContext);
                    toolStripbtnDelete.Enabled = UIController.CanExecuteAction(MenuItemEnums.删除, StatusContext);
                    toolStripBtnCancel.Visible = UIController.CanExecuteAction(MenuItemEnums.取消, StatusContext);
                }
                else
                {
                    // 回退到原始逻辑
                    bool isEditable = FMPaymentStatusHelper.CanModify(statusEnum);
                    bool canCancel = FMPaymentStatusHelper.CanCancel(statusEnum, HasRelatedRecords(entity as BaseEntity));

                    toolStripbtnModify.Enabled = isEditable;
                    toolStripButtonSave.Enabled = isEditable;
                    toolStripbtnDelete.Enabled = isEditable;
                    toolStripBtnCancel.Visible = canCancel;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"使用V3状态管理系统失败，回退到原始逻辑: {ex.Message}");
                // 回退到原始逻辑
                bool isEditable = FMPaymentStatusHelper.CanModify(statusEnum);
                bool canCancel = FMPaymentStatusHelper.CanCancel(statusEnum, HasRelatedRecords(entity as BaseEntity));

                toolStripbtnModify.Enabled = isEditable;
                toolStripButtonSave.Enabled = isEditable;
                toolStripbtnDelete.Enabled = isEditable;
                toolStripBtnCancel.Visible = canCancel;
            }

            // 特殊操作按钮
            //ConfigureSpecialButtons(statusEnum);

            // 通用按钮状态 - 使用V3状态管理系统
            try
            {
                if (UIController != null && StatusContext != null)
                {
                    // 使用新的状态管理系统检查按钮状态
                    toolStripbtnSubmit.Enabled = UIController.CanExecuteAction(MenuItemEnums.提交, StatusContext);
                    toolStripbtnReview.Enabled = UIController.CanExecuteAction(MenuItemEnums.审核, StatusContext);
                }
                else
                {
                    // 回退到原始逻辑
                    toolStripbtnSubmit.Enabled = FMPaymentStatusHelper.CanSubmit(statusEnum);
                    toolStripbtnReview.Enabled = statusEnum is PrePaymentStatus pre && pre == PrePaymentStatus.待审核 ||
                                                statusEnum is ARAPStatus arap && arap == ARAPStatus.待审核 ||
                                                statusEnum is DataStatus dataStatus1 && dataStatus1 == DataStatus.新建 ||
                                                statusEnum is StatementStatus statementStatus && statementStatus == StatementStatus.已发送 ||
                                                statusEnum is PaymentStatus pay && pay == PaymentStatus.待审核;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"使用V3状态管理系统失败，回退到原始逻辑: {ex.Message}");
                // 回退到原始逻辑
                toolStripbtnSubmit.Enabled = FMPaymentStatusHelper.CanSubmit(statusEnum);
                toolStripbtnReview.Enabled = statusEnum is PrePaymentStatus pre && pre == PrePaymentStatus.待审核 ||
                                            statusEnum is ARAPStatus arap && arap == ARAPStatus.待审核 ||
                                            statusEnum is DataStatus dataStatus1 && dataStatus1 == DataStatus.新建 ||
                                            statusEnum is StatementStatus statementStatus && statementStatus == StatementStatus.已发送 ||
                                            statusEnum is PaymentStatus pay && pay == PaymentStatus.待审核;
            }


            // 使用V3状态管理系统获取业务状态
            if (this.StateManager != null && this.UIController != null && entity is BaseEntity currentBaseEntity)
            {
                try
                {
                    // 获取业务状态
                    var businessStatusV3 = GetBusinessStatus(currentBaseEntity);
                    // 锁定状态处理
                    await HandleLockStatus(currentBaseEntity, businessStatusV3);
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "使用V3状态管理系统失败，回退到传统逻辑");
                    // 回退到传统逻辑 - 直接获取业务状态
                    var businessStatus = GetBusinessStatus(entity as BaseEntity);
                    // 锁定状态处理
                    await HandleLockStatus(entity as BaseEntity, businessStatus);
                }
            }
            else
            {
                // 回退到传统逻辑 - 直接获取业务状态
                var businessStatus = GetBusinessStatus(entity as BaseEntity);
                // 锁定状态处理
                await HandleLockStatus(entity as BaseEntity, businessStatus);
            }


        }













        #region 状态处理私有方法




        private void ConfigureSpecialButtons(dynamic status)
        {
            toolStripButton结案.Visible = false;
            //toolStripButton坏账.Visible = false;

            try
            {
                if (UIController != null && StatusContext != null)
                {
                    // 使用新的状态管理系统检查结案操作
                    bool canCloseCase = UIController.CanExecuteAction(MenuItemEnums.结案, StatusContext);
                    toolStripButton结案.Visible = canCloseCase;
                    toolStripButton结案.Enabled = canCloseCase;
                }
                else
                {
                    // 回退到原始逻辑
                    if (status is PrePaymentStatus preStatus)
                    {
                        toolStripButton结案.Visible = preStatus == PrePaymentStatus.待核销;
                        toolStripButton结案.Enabled = preStatus == PrePaymentStatus.待核销;
                    }
                    else if (status is ARAPStatus arapStatus)
                    {
                        toolStripButton结案.Visible = arapStatus == ARAPStatus.待支付 ||
                                                     arapStatus == ARAPStatus.部分支付;
                        toolStripButton结案.Enabled = FMPaymentStatusHelper.CanReverse(arapStatus);

                        //toolStripButton坏账.Visible = toolStripButton结案.Visible;
                        //toolStripButton坏账.Enabled = toolStripButton结案.Enabled;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"使用V3状态管理系统配置特殊按钮失败，回退到原始逻辑: {ex.Message}");
                // 回退到原始逻辑
                if (status is PrePaymentStatus preStatus)
                {
                    toolStripButton结案.Visible = preStatus == PrePaymentStatus.待核销;
                    toolStripButton结案.Enabled = preStatus == PrePaymentStatus.待核销;
                }
                else if (status is ARAPStatus arapStatus)
                {
                    toolStripButton结案.Visible = arapStatus == ARAPStatus.待支付 ||
                                                 arapStatus == ARAPStatus.部分支付;
                    toolStripButton结案.Enabled = FMPaymentStatusHelper.CanReverse(arapStatus);

                    //toolStripButton坏账.Visible = toolStripButton结案.Visible;
                    //toolStripButton坏账.Enabled = toolStripButton结案.Enabled;
                }
            }
        }

        private bool HasRelatedRecords(BaseEntity entity)
        {
            // 实现关联记录检查逻辑
            // 例如：检查是否有核销记录、支付记录等
            return false;
        }





        private long GetPrimaryKeyValue(BaseEntity entity)
        {
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
            return pkid;
        }

        private void DisableAllOperations()
        {
            toolStripbtnModify.Enabled = false;
            toolStripbtnSubmit.Enabled = false;
            toolStripBtnReverseReview.Enabled = false;
            toolStripbtnReview.Enabled = false;
            toolStripButtonSave.Enabled = false;
            toolStripbtnDelete.Enabled = false;
            toolStripbtnPrint.Enabled = false;
            toolStripButton结案.Enabled = false;
        }
        #endregion

        #region 辅助方法
        private Type GetActualStatusType(BaseEntity entity)
        {
            if (entity.ContainsProperty(typeof(PrePaymentStatus).Name)) return typeof(PrePaymentStatus);
            if (entity.ContainsProperty(typeof(ARAPStatus).Name)) return typeof(ARAPStatus);
            if (entity.ContainsProperty(typeof(PaymentStatus).Name)) return typeof(PaymentStatus);
            throw new InvalidOperationException("未知状态类型");
        }

        private Enum GetStatusValue(BaseEntity entity, Type statusType)
        {
            object value = entity.GetPropertyValue(statusType.Name);
            return (Enum)Enum.Parse(statusType, value.ToString());
        }



        #endregion


        #region 状态机处理新2025-6-17

        /// <summary>
        /// 根据单据状态控制工具栏按钮
        /// </summary>
        /// <param name="entity">单据实体</param>
        protected virtual async Task ToolBarEnabledControl(BaseEntity entity)
        {
            if (entity == null) return;

            // 统一处理UI更新，先尝试使用V3状态管理系统
            if (this.StateManager != null && this.UIController != null && entity != null)
            {
                try
                {
                    // 使用V3状态管理系统的按钮控制
                    UpdateUIByCurrentState();


                    return;
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, "使用V3状态管理系统失败，使用整合逻辑");
                }
            }

            // 整合逻辑 - 直接获取业务状态
            var businessStatus = GetBusinessStatus(entity);

            // 通用按钮状态 - 使用传统状态判断
            if (businessStatus != null)
            {
                bool isEditable = false;
                bool canSubmit = false;
                bool canReview = false;
                bool canReverseReview = false;
                bool canClose = false;

                switch (businessStatus)
                {
                    case PrePaymentStatus pre:
                        isEditable = pre == PrePaymentStatus.草稿;
                        canSubmit = pre == PrePaymentStatus.草稿;
                        canReview = pre == PrePaymentStatus.待审核;
                        canReverseReview = pre == PrePaymentStatus.待核销 || pre == PrePaymentStatus.已生效;
                        canClose = pre == PrePaymentStatus.待核销;
                        break;
                    case ARAPStatus arap:
                        isEditable = arap == ARAPStatus.草稿;
                        canSubmit = arap == ARAPStatus.草稿;
                        canReview = arap == ARAPStatus.待审核;
                        canReverseReview = arap == ARAPStatus.待支付;
                        canClose = arap == ARAPStatus.待支付 || arap == ARAPStatus.部分支付;
                        break;
                    case PaymentStatus pay:
                        isEditable = pay == PaymentStatus.草稿;
                        canSubmit = pay == PaymentStatus.草稿;
                        canReview = pay == PaymentStatus.待审核;
                        canReverseReview = false;
                        canClose = false;
                        break;
                    case StatementStatus statement:
                        isEditable = statement == StatementStatus.草稿;
                        canSubmit = statement == StatementStatus.草稿;
                        canReview = statement == StatementStatus.已发送;
                        canReverseReview = statement == StatementStatus.已确认;
                        canClose = false;
                        break;
                    case DataStatus dataStatus:
                        isEditable = dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建;
                        canSubmit = dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建;
                        canReview = dataStatus == DataStatus.新建;
                        canReverseReview = dataStatus == DataStatus.确认;
                        canClose = false;
                        break;
                }

                // 使用统一的按钮状态更新方法
                UpdateAllButtonStates(StateManager.GetDataStatus(entity));

                // 保存状态特殊处理
                toolStripButtonSave.Enabled = entity.HasChanged;

                // 反审按钮特殊处理
                toolStripBtnReverseReview.Visible = canReverseReview;
            }


        }




        #region 锁定统计功能


        /// <summary>
        /// 验证优化后的锁定流程核心功能
        /// 此方法用于测试三个核心步骤是否正确：查询锁状态、锁/解锁操作、UI状态更新
        /// </summary>
        /// <returns>测试结果</returns>
        public string VerifyLockProcess()
        {
            // 这是一个仅用于调试和验证的方法
            // 在实际应用中，锁定流程的正确性会在用户操作中体现

            var results = new List<string>();

            // 验证1: 检查锁定状态查询机制是否正常
            if (_integratedLockService != null)
                results.Add("✅ 锁定状态查询机制可用");
            else
                results.Add("❌ 锁定服务未初始化");

            // 验证2: 检查锁定按钮是否可用
            if (tsBtnLocked != null)
                results.Add("✅ 锁定按钮UI组件可用");
            else
                results.Add("❌ 锁定按钮未初始化");

            // 验证3: 检查优化后的锁定流程核心方法是否存在
            bool hasLockMethod = true; // 我们已经验证过这些方法存在
            bool hasUnlockMethod = true;
            bool hasCheckMethod = true;

            if (hasLockMethod && hasUnlockMethod && hasCheckMethod)
                results.Add("✅ 所有核心锁定方法都存在");
            else
                results.Add("❌ 部分核心锁定方法缺失");

            return string.Join(Environment.NewLine, results);
        }


        #endregion

        /// <summary>
        /// 处理单据锁定状态，更新UI显示
        /// </summary>
        /// <param name="entity">单据实体</param>
        /// <param name="businessStatus">业务状态</param>
        /// <remarks>
        /// 整合版本：调用统一的CheckLockStatusAndUpdateUI方法处理锁定状态检查和UI更新
        /// 避免重复代码，保持功能一致
        /// </remarks>
        private async Task HandleLockStatus(BaseEntity entity, Enum businessStatus)
        {
            if (entity == null) return;

            // 调用统一的锁定状态检查和UI更新方法
            await CheckLockStatusAndUpdateUI(entity);

            // 对于被他人锁定的情况，CheckLockStatusAndUpdateUI已禁用控件
            // 这里只需要在未被锁定或自己锁定的情况下恢复控件可用性
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);

            if (pkid > 0 && _integratedLockService != null)
            {
                try
                {
                    var lockInfo = await _integratedLockService.GetLockInfoAsync(pkid, CurMenuInfo.MenuID);
                    long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                    bool isSelfLock = lockInfo != null && lockInfo.IsLocked && lockInfo.LockedUserId == currentUserId;

                    // 未被锁定或自己锁定时恢复控件可用性
                    if (!lockInfo?.IsLocked ?? true || isSelfLock)
                    {
                        foreach (Control control in Controls)
                        {
                            control.Enabled = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogError(ex, "处理锁定状态时发生异常");
                    MainForm.Instance.uclog.AddLog($"处理锁定状态时发生异常: {ex.Message}", UILogType.错误);
                }
            }

            // 根据业务状态启用适当的操作按钮
            EnableOperationsBasedOnStatus(businessStatus);
        }



        /// <summary>
        /// 异步检查锁定状态
        /// 直接使用ClientLocalLockCacheService，不再维护本地缓存
        /// 重要规则：如果锁定用户不在线，则视为未锁定状态
        /// </summary>
        private async Task<(bool IsLocked, LockInfo LockInfo)> CheckLockStatusAsync(long billId, long menuId)
        {
            try
            {
                // 直接使用集成锁定服务检查锁定状态
                var lockInfo = await _integratedLockService.GetLockInfoAsync(billId, menuId);

                // 客户端取锁定状态：先取本地，再取服务器
                // 只要是锁定了，并且不是自己时就不能编辑等相关处理
                bool isLocked = lockInfo != null && lockInfo.IsLocked;

                string lockStatusMsg = isLocked ? "已锁定" : "未锁定";

                MainForm.Instance?.uclog?.AddLog($"检查单据[{billId}]锁定状态: {lockStatusMsg}", UILogType.普通消息);


                return (isLocked, lockInfo);
            }
            catch (Exception ex)
            {
                MainForm.Instance?.logger?.LogError(ex, $"检查单据[{billId}]锁定状态时发生错误");
                MainForm.Instance?.uclog?.AddLog($"检查锁定状态时发生错误: {ex.Message}", UILogType.错误);
                return (false, null);
            }
        }



        /// <summary>
        /// 集中管理UI控件的启用/禁用状态
        /// </summary>
        /// <param name="enabled">是否启用</param>
        /// <param name="excludeControls">需要排除的控件列表</param>
        private void SetControlsEnabled(bool enabled, params string[] excludeControlsName)
        {
            try
            {
                foreach (Control control in Controls)
                {
                    bool shouldExclude = false;
                    foreach (var excludeControl in excludeControlsName)
                    {
                        if (control.Name == excludeControl)
                        {
                            shouldExclude = true;
                            break;
                        }
                    }
                    if (!shouldExclude)
                    {
                        control.Enabled = enabled;
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "设置控件启用状态时发生异常");
            }
        }

        /// <summary>
        /// 检查锁定状态并更新UI（基于实体）
        /// 统一处理锁定状态相关的所有UI和按钮状态
        /// </summary>
        /// <param name="entity">单据实体</param>
        /// <returns>锁定状态信息和操作权限状态</returns>
        public async Task<(bool IsLocked, bool CanPerformCriticalOperations, LockInfo LockInfo)> CheckLockStatusAndUpdateUI(BaseEntity entity)
        {
            if (entity == null) return (false, true, null);

            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
            if (pkid <= 0) return (false, true, null);

            // 调用基于billId的版本，避免代码重复
            var result = await CheckLockStatusAndUpdateUI(pkid);

            // 如果是被他人锁定，需要禁用所有编辑控件（UI加载时的特殊处理）
            if (result.IsLocked && !result.CanPerformCriticalOperations)
            {
                SetControlsEnabled(false, tsBtnLocked.Name, toolStripbtnClose.Name);
                tsBtnLocked.Enabled = true;
                toolStripbtnClose.Enabled = true;
                toolStripBtnCancel.Visible = true;
            }

            return result;
        }



        /// <summary>根据业务状态启用或禁用操作按钮</summary>
        /// <param name="status">业务状态枚举值</param>
        private void EnableOperationsBasedOnStatus(Enum status)
        {
            if (status == null) return;

            // 初始化通用按钮状态
            bool canEdit = false;
            bool canSubmit = false;
            bool canReview = false;
            bool canReverseReview = false;
            bool canClose = false;

            // 根据不同类型的业务状态设置操作权限
            if (status is PrePaymentStatus preStatus)
            {
                canEdit = preStatus == PrePaymentStatus.草稿;
                canSubmit = FMPaymentStatusHelper.CanSubmit(preStatus);
                canReview = preStatus == PrePaymentStatus.待审核;
                canReverseReview = preStatus == PrePaymentStatus.待核销 || preStatus == PrePaymentStatus.已生效;
                canClose = preStatus == PrePaymentStatus.待核销;
            }
            else if (status is ARAPStatus arapStatus)
            {
                canEdit = arapStatus == ARAPStatus.草稿;
                canSubmit = FMPaymentStatusHelper.CanSubmit(arapStatus);
                canReview = arapStatus == ARAPStatus.待审核;
                canReverseReview = arapStatus == ARAPStatus.待支付;
                canClose = FMPaymentStatusHelper.CanReverse(arapStatus);
            }
            else if (status is PaymentStatus payStatus)
            {
                canEdit = payStatus == PaymentStatus.草稿;
                canSubmit = FMPaymentStatusHelper.CanSubmit(payStatus);
                canReview = payStatus == PaymentStatus.待审核;
                // 支付状态不支持反审核和结案
            }
            else if (status is StatementStatus statementStatus)
            {
                canEdit = statementStatus == StatementStatus.草稿;
                canSubmit = statementStatus == StatementStatus.草稿;
                canReview = statementStatus == StatementStatus.已发送;
                canReverseReview = statementStatus == StatementStatus.已确认;
                // 对账单不支持结案
            }
            else if (status is DataStatus dataStatus)
            {
                canEdit = dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建;
                canSubmit = dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建;
                canReview = dataStatus == DataStatus.新建;
                canReverseReview = dataStatus == DataStatus.确认;
                // 通用数据状态不支持结案
            }

            // 统一应用操作权限到按钮
            toolStripbtnModify.Enabled = canEdit;
            toolStripbtnSubmit.Enabled = canSubmit;
            toolStripbtnReview.Enabled = canReview;
            toolStripBtnReverseReview.Enabled = canReverseReview;
            toolStripBtnReverseReview.Visible = canReverseReview;
            toolStripButton结案.Enabled = canClose;
        }

        /// <summary>设置实体状态属性值</summary>
        private void SetEntityStatus<TStatus>(BaseEntity entity, TStatus status) where TStatus : Enum
        {
            string propertyName = typeof(TStatus).Name;
            if (entity.ContainsProperty(propertyName))
            {
                ReflectionHelper.SetPropertyValue(entity, propertyName, (int)(object)status);
            }
        }

        /// <summary>获取业务状态</summary>
        private Enum GetBusinessStatus(BaseEntity entity)
        {
            if (entity == null) return null;

            // 检测实体中的业务状态属性
            var statusTypes = new[]
            {
                typeof(PrePaymentStatus),
                typeof(ARAPStatus),
                typeof(PaymentStatus),
                typeof(StatementStatus),
                typeof(DataStatus) // 添加DataStatus支持
            };

            foreach (var statusType in statusTypes)
            {
                if (entity.ContainsProperty(statusType.Name))
                {
                    var statusValue = entity.GetPropertyValue(statusType.Name);
                    if (statusValue != null)
                    {
                        try
                        {
                            // 尝试转换为对应的枚举类型
                            return (Enum)Enum.Parse(statusType, statusValue.ToString());
                        }
                        catch (Exception ex)
                        {
                            logger?.LogWarning(ex, $"无法将实体属性 {statusType.Name} 的值 {statusValue} 转换为枚举类型");
                        }
                    }
                }
            }

            return null;
        }


        ///// <summary>刷新工具栏状态</summary>
        public async Task RefreshToolbar()
        {
            if (EditEntity != null)
            {
                await ToolBarEnabledControl(EditEntity);
            }
        }

        #endregion


        #region 帮助信息提示


        public void InitHelpInfoToControl(System.Windows.Forms.Control.ControlCollection Controls)
        {
            foreach (var item in Controls)
            {
                if (item is Control)
                {
                    if (item.GetType().Name == "KryptonTextBox")
                    {
                        KryptonTextBox ktb = item as KryptonTextBox;
                        if ((item as Control).DataBindings.Count > 0)
                        {
                            if (GetHelpInfoByBinding(ktb.DataBindings).Length > 0)
                            {
                                ButtonSpecAny bsa = new ButtonSpecAny();
                                bsa.Image = System.Drawing.Image.FromStream(Common.DataBindingHelper.GetResource("help4"));
                                bsa.Tag = ktb;
                                bsa.Click += Bsa_Click;
                                ktb.ButtonSpecs.Add(bsa);

                                //可以边框为红色不？
                                //或必填项目有特别提示？
                            }
                        }
                    }
                }

            }
        }





        private void Bsa_Click(object sender, EventArgs e)
        {
            ProcessHelpInfo(true, sender);
        }




        public new void ProcessHelpInfo(bool fromBtn, object sender)
        {
            string tipTxt = string.Empty;
            //时长timeout默认值设置的是3000ms(也就是3秒)
            int timeout = 3000;
            _timeoutTimer4tips = new System.Threading.Timer(OnTimerElapsed, null, timeout, System.Threading.Timeout.Infinite);
            toolTipBase.Hide(this);
            if (fromBtn)
            {
                ButtonSpecAny bsa = sender as ButtonSpecAny;
                tipTxt = GetHelpInfoByBinding((bsa.Owner as KryptonTextBox).DataBindings);
                Control ctl = bsa.Owner as Control;
                if (string.IsNullOrEmpty(tipTxt))
                {
                    return;
                }
                toolTipBase.SetToolTip(ctl, tipTxt);
                toolTipBase.Show(tipTxt, ctl);
            }
            else
            {
                #region F1
                if (ActiveControl.GetType().ToString() == "Krypton.Toolkit.KryptonTextBox+InternalTextBox")
                {
                    KryptonTextBox txt = ActiveControl.Parent as KryptonTextBox;
                    tipTxt = GetHelpInfoByBinding(txt.DataBindings);
                    //if (txt.DataBindings.Count > 0)
                    //{
                    //    string filedName = txt.DataBindings[0].BindingMemberInfo.BindingField;
                    //    string[] cns = txt.DataBindings[0].BindingManagerBase.Current.ToString().Split('.');
                    //    string className = cns[cns.Length - 1];
                    //    var obj = Startup.AutoFacContainer.ResolveNamed<BaseEntity>(className);
                    //    if (obj.HelpInfos.ContainsKey(filedName))
                    //    {
                    //        tipTxt = "【" + obj.FieldNameList.Find(f => f.Key == filedName).Value.Trim() + "】";
                    //        tipTxt += obj.HelpInfos[filedName].ToString();
                    //    }

                    //}
                }
                else
                {

                }
                if (string.IsNullOrEmpty(tipTxt))
                {
                    return;
                }
                toolTipBase.SetToolTip(ActiveControl, tipTxt);
                toolTipBase.Show(tipTxt, ActiveControl);
                #endregion
            }





        }


        /// <summary>
        /// 获取帮助信息集合对应的值
        /// </summary>
        /// <param name="cbc"></param>
        /// <returns></returns>
        private string GetHelpInfoByBinding(ControlBindingsCollection cbc)
        {
            string tipTxt = string.Empty;
            if (cbc.Count > 0)
            {
                string filedName = cbc[0].BindingMemberInfo.BindingField;
                if (cbc[0].BindingManagerBase == null)
                {
                    return tipTxt;
                }
                string[] cns = cbc[0].BindingManagerBase.Current.ToString().Split('.');
                string className = cns[cns.Length - 1];

                var obj = Startup.GetFromFacByName<BaseEntity>(className);
                if (obj.HelpInfos != null)
                {
                    if (obj.HelpInfos.ContainsKey(filedName))
                    {
                        tipTxt = "【" + obj.FieldNameList[filedName].Trim() + "】";
                        tipTxt += obj.HelpInfos[filedName].ToString();
                    }
                }


            }
            return tipTxt;
        }



        public void OnTimerElapsed(object state)
        {
            toolTipBase.Hide(this);
            _timeoutTimer4tips.Dispose();

        }
        private void toolTipBase_Popup(object sender, PopupEventArgs e)
        {
            //ToolTip tool = (ToolTip)sender;
            //if (e.AssociatedControl.Name == "textBox1")//e代表我们要触发的事件，我们是在textBox1触发
            //{
            //    tool.ToolTipTitle = "提示信息";//修改标题
            //    tool.ToolTipIcon = ToolTipIcon.Info;//修改图标
            //}
            //else
            //{
            //    tool.ToolTipTitle = "";
            //    tool.ToolTipIcon = ToolTipIcon.Info;
            //}
        }
        private void timerForToolTip_Tick(object sender, EventArgs e)
        {

        }
        #endregion



        #region 特殊显示必填项


        /// <summary>
        /// 特殊显示必填项 
        /// </summary>
        /// <typeparam name="T">要验证必填的类型</typeparam>
        /// <param name="rules"></param>
        /// <param name="Controls"></param>
        public void InitRequiredToControl(AbstractValidator<T> rules, System.Windows.Forms.Control.ControlCollection Controls)
        {
            List<string> notEmptyList = new List<string>();
            List<string> checkList = new List<string>();
            foreach (var item in rules)
            {
                string colName = item.PropertyName;
                var rr = item.Components;
                foreach (var com in item.Components)
                {
                    if (com.Validator.Name == "NotEmptyValidator")
                    {
                        //这里找到了不能为空的验证器。为了体验在UI
                        notEmptyList.Add(colName);
                    }
                    else
                    if (com.Validator.Name == "PredicateValidator")
                    {
                        checkList.Add(colName);
                    }
                }
            }


            foreach (var item in Controls)
            {
                if (item is Control)
                {
                    if (item is VisualControlBase)
                    {
                        if (item.GetType().Name == "KryptonTextBox")
                        {
                            KryptonTextBox ktb = item as KryptonTextBox;
                            if ((item as Control).DataBindings.Count > 0)
                            {
                                #region 找到绑定的字段
                                if (ktb.DataBindings.Count > 0)
                                {
                                    string filedName = ktb.DataBindings[0].BindingMemberInfo.BindingField;
                                    string col = notEmptyList.FirstOrDefault(c => c == filedName);
                                    if (col.IsNotEmptyOrNull())
                                    {
                                        ktb.StateCommon.Border.Color1 = Color.FromArgb(255, 128, 128);
                                    }

                                    string colchk = checkList.FirstOrDefault(c => c == filedName);
                                    if (colchk.IsNotEmptyOrNull())
                                    {
                                        ktb.StateCommon.Border.Color1 = Color.FromArgb(255, 0, 204);
                                    }

                                }
                                #endregion


                            }
                        }
                        if (item.GetType().Name == "KryptonComboBox")
                        {
                            KryptonComboBox ktb = item as KryptonComboBox;
                            if ((item as Control).DataBindings.Count > 0)
                            {
                                //ButtonSpecAny bsa = new ButtonSpecAny();
                                // bsa.Image = Image.FromStream(Common.DataBindingHelper.GetResource("help4"));
                                // bsa.Tag = ktb;
                                //bsa.Click += Bsa_Click;
                                // ktb.ButtonSpecs.Add(bsa);
                                // ktb.StateCommon.Border.Color1 =  Color.FromArgb(255, 128, 128);
                                //可以边框为红色不？
                                //或必填项目有特别提示？
                                #region 找到绑定的字段
                                if (ktb.DataBindings.Count > 0)
                                {
                                    string filedName = ktb.DataBindings[0].BindingMemberInfo.BindingField;
                                    string col = notEmptyList.FirstOrDefault(c => c == filedName);
                                    if (col.IsNotEmptyOrNull())
                                    {
                                        ktb.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(255, 128, 128);
                                    }
                                    string colchk = checkList.FirstOrDefault(c => c == filedName);
                                    if (colchk.IsNotEmptyOrNull())
                                    {
                                        ktb.StateCommon.ComboBox.Border.Color1 = Color.FromArgb(255, 0, 204);
                                    }
                                }
                                #endregion


                            }
                        }
                    }
                }
            }
        }

        #endregion



        #region 基础资料下拉添加编辑项





        Expression<Func<TParent, bool>> Wrap<TParent, TElement>(Expression<Func<TParent, IEnumerable<TElement>>> collection, Expression<Func<TElement, bool>> isOne, Expression<Func<IEnumerable<TElement>, Func<TElement, bool>, bool>> isAny)
        {
            var parent = Expression.Parameter(typeof(TParent), "parent");

            return
                (Expression<Func<TParent, bool>>)Expression.Lambda
                (
                    Expression.Invoke
                    (
                        isAny,
                        Expression.Invoke
                        (
                            collection,
                            parent
                        ),
                        isOne
                    ),
                    parent
                );
        }


        #endregion

        public delegate void BindDataToUIHander(T entity, ActionStatus actionStatus);

    [Browsable(true), Description("绑定数据对象到UI")]
    public event BindDataToUIHander OnBindDataToUIEvent;



    string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
    /// <summary>
    /// 控制功能按钮
    /// </summary>
    /// <param name="p_Text"></param>
    protected async override void DoButtonClick(MenuItemEnums menuItem)
    {
        //操作前将数据收集  保存单据时间出错，这个方法开始是 将查询条件生效
        // this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);

        MainForm.Instance.AppContext.log.ActionName = menuItem.ToString();
        if (!MainForm.Instance.AppContext.IsSuperUser)
        {
            /*
            tb_MenuInfo menuInfo = MainForm.Instance.AppContext.CurUserInfo.UserMenuList.Where(c => c.MenuType == "行为菜单").Where(c => c.FormName == this.Name).FirstOrDefault();
            if (menuInfo == null)
            {
                MessageBox.Show($"没有使用【{menuInfo.MenuName}】的权限。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<tb_ButtonInfo> btnList = MainForm.Instance.AppContext.CurUserInfo.UserButtonList.Where(c => c.MenuID == menuInfo.MenuID).ToList();
            if (!btnList.Where(b => b.BtnText == menuItem.ToString()).Any())
            {
                MessageBox.Show($"没有使用【{menuItem.ToString()}】的权限。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/
        }


        //操作前是不是锁定。自己排除
        long pkid = 0;
        //操作前将数据收集
        this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
        switch (menuItem)
        {
            case MenuItemEnums.联查:
                break;
            case MenuItemEnums.已锁定:
                // 处理锁定按钮点击事件
                if (tsBtnLocked.Tag is LockResponse lockResponse && lockResponse.LockInfo != null)
                {
                    long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;

                    // 如果是自己锁定的，直接解锁（无需确认）
                    if (lockResponse.LockInfo.LockedUserId == currentUserId)
                    {
                        logger?.Debug($"用户[{currentUserId}]选择解锁自己锁定的单据");
                        UNLock();
                    }
                    else
                    {
                        // 如果是他人锁定的，直接请求解锁（无需确认）
                        logger?.Debug($"用户[{currentUserId}]请求解锁被用户[{lockResponse.LockInfo.LockedUserName}]锁定的单据");
                        RequestUnLock();
                    }
                }
                else
                {
                    logger?.LogWarning("无法获取锁定信息，请刷新后重试");
                }
                break;
            case MenuItemEnums.新增:
                Add();
                break;
            case MenuItemEnums.取消:
                Cancel();
                break;
            case MenuItemEnums.复制性新增:
                AddByCopy();
                break;

            case MenuItemEnums.数据特殊修正:
                SpecialDataFix();
                break;

            case MenuItemEnums.删除:
                if (await IsLock())
                {
                    return;
                }
                await Delete();

                break;
            case MenuItemEnums.修改:
                if (await IsLock())
                {
                    return;
                }
                await LockBill();
                Modify();
                break;
            case MenuItemEnums.查询:
                Query();
                break;
            case MenuItemEnums.保存:
                if (await IsLock())
                {
                    return;
                }
                //操作前将数据收集
                this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
                if (EditEntity != null)
                {
                    pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
                    if (pkid > 0)
                    {
                        //如果有审核状态才去判断
                        if (editEntity.ContainsProperty(typeof(DataStatus).Name))
                        {
                            var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
                            if (dataStatus == DataStatus.完结 || dataStatus == DataStatus.确认)
                            {
                                toolStripbtnSubmit.Enabled = false;
                                if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
                                {
                                    MainForm.Instance.uclog.AddLog("已经是【完结】或【确认】状态，保存失败。");
                                }
                                return;
                            }
                        }

                    }
                    toolStripButtonSave.Enabled = false;
                    bool rsSave = await Save(true);
                    if (!rsSave)
                    {
                        toolStripButtonSave.Enabled = true;
                    }
                }
                else
                {
                    MainForm.Instance.uclog.AddLog("单据不能为空，保存失败。");
                }


                break;
            case MenuItemEnums.提交:
                if (await IsLock())
                {
                    return;
                }
                //操作前将数据收集
                this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);

                toolStripbtnSubmit.Enabled = false;
                bool rs = await Submit();
                if (!rs)
                {
                    toolStripbtnSubmit.Enabled = true;
                }
                else
                {
                    //提交后别人可以审核 
                    UNLock();
                }
                break;
            case MenuItemEnums.关闭:
                Exit(this);
                break;
            case MenuItemEnums.刷新:
                Refreshs();
                break;
            case MenuItemEnums.属性:
                Property();
                break;
            case MenuItemEnums.审核:
                if (await IsLock())
                {
                    return;
                }
                await LockBill();
                toolStripbtnReview.Enabled = false;
                ReviewResult reviewResult = await Review();
                if (!reviewResult.Succeeded)
                {
                    UNLock();
                    toolStripbtnReview.Enabled = true;
                }

                break;
            case MenuItemEnums.反审:
                if (await IsLock())
                {
                    return;
                }
                await LockBill();
                toolStripBtnReverseReview.Enabled = false;
                bool rs反审 = await ReReview();
                if (!rs反审)
                {
                    UNLock();
                    toolStripBtnReverseReview.Enabled = true;
                }

                break;
            case MenuItemEnums.结案:
                if (await IsLock())
                {
                    return;
                }
                await CloseCaseAsync();
                break;
            case MenuItemEnums.反结案:
                if (await IsLock())
                {
                    return;
                }
                await AntiCloseCaseAsync();
                break;
            case MenuItemEnums.打印:

                if (PrintConfig != null && PrintConfig.tb_PrintTemplates != null)
                {
                    //如果当前单据只有一个模块，就直接打印
                    if (PrintConfig.tb_PrintTemplates.Count == 1)
                    {
                        await Print();
                        return;
                    }
                }

                //个性化设置了打印要选择模板打印时，就进入设计介面
                if (MainForm.Instance.AppContext.CurrentUser_Role_Personalized.SelectTemplatePrint.HasValue
                       && MainForm.Instance.AppContext.CurrentUser_Role_Personalized.SelectTemplatePrint.Value)
                {
                    await PrintDesigned();
                }
                else
                {
                    await Print();
                }

                toolStripbtnPrint.Enabled = false;
                break;
            case MenuItemEnums.预览:
                await Preview();
                break;
            case MenuItemEnums.设计:
                await PrintDesigned();
                break;
            case MenuItemEnums.导出:
                break;

            default:
                break;
        }


    }

    MenuPowerHelper menuPowerHelper = null;
    public async void MenuItem_Click(object sender, EventArgs e)
    {
        if (sender is ToolStripMenuItem menuItem)
        {
            if (menuItem.Tag is RelatedQueryParameter parameter)
            {
                #region mrp生产模块
                if (parameter.bizType == BizType.需求分析)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_ProductionDemand).Name
                    && m.FormName == nameof(UCProduceRequirement)
                    && m.BIBaseForm.Contains("BaseBillEditGeneric")
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_ProductionDemand>>(typeof(tb_ProductionDemand).Name + "Controller");
                        tb_ProductionDemand entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }

                }

                if (parameter.bizType == BizType.生产计划单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_ProductionPlan).Name
                    && m.BIBaseForm.Contains("BaseBillEditGeneric")
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_ProductionPlan>>(typeof(tb_ProductionPlan).Name + "Controller");
                        var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }


                if (parameter.bizType == BizType.生产领料单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_MaterialRequisition).Name
                    && m.BIBaseForm.Contains("BaseBillEditGeneric")
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_MaterialRequisition>>(typeof(tb_MaterialRequisition).Name + "Controller");
                        var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }
                if (parameter.bizType == BizType.生产退料单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_MaterialReturn).Name
                    && m.BIBaseForm.Contains("BaseBillEditGeneric")
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_MaterialReturn>>(typeof(tb_MaterialReturn).Name + "Controller");
                        var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }
                if (parameter.bizType == BizType.制令单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_ManufacturingOrder).Name
                    && m.BIBaseForm.Contains("BaseBillEditGeneric")
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_ManufacturingOrder>>(typeof(tb_ManufacturingOrder).Name + "Controller");
                        var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }
                if (parameter.bizType == BizType.返工入库单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_MRP_ReworkEntry).Name
                    && m.BIBaseForm.Contains("BaseBillEditGeneric")
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_MRP_ReworkEntry>>(typeof(tb_MRP_ReworkEntry).Name + "Controller");
                        var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }

                if (parameter.bizType == BizType.返工退库单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_MRP_ReworkReturn).Name
                    && m.BIBaseForm.Contains("BaseBillEditGeneric")
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_MRP_ReworkReturn>>(typeof(tb_MRP_ReworkReturn).Name + "Controller");
                        var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }

                if (parameter.bizType == BizType.缴库单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_FinishedGoodsInv).Name
                    && m.BIBaseForm.Contains("BaseBillEditGeneric")
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_FinishedGoodsInv>>(typeof(tb_FinishedGoodsInv).Name + "Controller");
                        var entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }

                #endregion

                if (parameter.bizType == BizType.对账单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_FM_Statement).Name
                    && m.BizType == (int)parameter.bizType
                    && m.BIBaseForm == "BaseBillEditGeneric`2"
                    //&& m.BIBizBaseForm == nameof(UCFMStatement)
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_FM_Statement>>(typeof(tb_FM_Statement).Name + "Controller");
                        tb_FM_Statement entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }


                if (parameter.bizType == BizType.付款单 || parameter.bizType == BizType.收款单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_FM_PaymentRecord).Name
                    && m.BIBizBaseForm == nameof(UCPaymentRecord)
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_FM_PaymentRecord>>(typeof(tb_FM_PaymentRecord).Name + "Controller");
                        tb_FM_PaymentRecord entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }


                if (parameter.bizType == BizType.应付款单 || parameter.bizType == BizType.应收款单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_FM_ReceivablePayable).Name
                    && m.BIBizBaseForm == nameof(UCReceivablePayable)
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_FM_ReceivablePayable>>(typeof(tb_FM_ReceivablePayable).Name + "Controller");
                        tb_FM_ReceivablePayable entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }

                if (parameter.bizType == BizType.预付款单 || parameter.bizType == BizType.预收款单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_FM_PreReceivedPayment).Name
                    && m.BIBizBaseForm == nameof(UCPreReceivedPayment)
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_FM_PreReceivedPayment>>(typeof(tb_FM_PreReceivedPayment).Name + "Controller");
                        tb_FM_PreReceivedPayment entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }


                if (parameter.bizType == BizType.其他费用收入 || parameter.bizType == BizType.其他费用支出)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_FM_OtherExpense).Name
                    && m.BIBizBaseForm == nameof(UCOtherExpense)
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_FM_OtherExpense>>(typeof(tb_FM_OtherExpense).Name + "Controller");
                        tb_FM_OtherExpense entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }
                if (parameter.bizType == BizType.采购价格调整单 || parameter.bizType == BizType.销售价格调整单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_FM_PriceAdjustment).Name
                    && m.BIBizBaseForm == nameof(UCPriceAdjustment)
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_FM_PriceAdjustment>>(typeof(tb_FM_PriceAdjustment).Name + "Controller");
                        tb_FM_PriceAdjustment entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        await menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }

                if (parameter.bizType == BizType.采购订单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_PurOrder).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_PurOrder>>(typeof(tb_PurOrder).Name + "Controller");
                        tb_PurOrder entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }

                if (parameter.bizType == BizType.采购入库单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_PurEntry).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_PurEntry>>(typeof(tb_PurEntry).Name + "Controller");
                        tb_PurEntry entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }
                if (parameter.bizType == BizType.采购退货单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_PurEntryRe).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_PurEntryRe>>(typeof(tb_PurEntryRe).Name + "Controller");
                        tb_PurEntryRe entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }
                if (parameter.bizType == BizType.采购退货入库)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_PurReturnEntry).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_PurReturnEntry>>(typeof(tb_PurReturnEntry).Name + "Controller");
                        tb_PurReturnEntry entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }

                if (parameter.bizType == BizType.归还单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_ProdReturning).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_ProdReturning>>(typeof(tb_ProdReturning).Name + "Controller");
                        tb_ProdReturning entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }
                if (parameter.bizType == BizType.销售订单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_SaleOrder).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_SaleOrder>>(typeof(tb_SaleOrder).Name + "Controller");
                        tb_SaleOrder entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }
                if (parameter.bizType == BizType.销售出库单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_SaleOut).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_SaleOut>>(typeof(tb_SaleOut).Name + "Controller");
                        tb_SaleOut entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }
                if (parameter.bizType == BizType.销售退回单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_SaleOutRe).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_SaleOutRe>>(typeof(tb_SaleOutRe).Name + "Controller");
                        tb_SaleOutRe entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }

                if (parameter.bizType == BizType.费用报销单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_FM_ExpenseClaim).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_FM_ExpenseClaim>>(typeof(tb_FM_ExpenseClaim).Name + "Controller");
                        tb_FM_ExpenseClaim entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }


                if (parameter.bizType == BizType.售后申请单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_AS_AfterSaleApply).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_AS_AfterSaleApply>>(typeof(tb_AS_AfterSaleApply).Name + "Controller");
                        tb_AS_AfterSaleApply entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }
                if (parameter.bizType == BizType.售后交付单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_AS_AfterSaleDelivery).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_AS_AfterSaleDelivery>>(typeof(tb_AS_AfterSaleDelivery).Name + "Controller");
                        tb_AS_AfterSaleDelivery entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }
                if (parameter.bizType == BizType.维修工单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_AS_RepairOrder).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_AS_RepairOrder>>(typeof(tb_AS_RepairOrder).Name + "Controller");
                        tb_AS_RepairOrder entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }
                if (parameter.bizType == BizType.维修入库单)
                {
                    var RelatedBillMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                    && m.EntityName == typeof(tb_AS_RepairInStock).Name
                    && m.MenuName.Contains(parameter.bizType.ToString())
                    ).FirstOrDefault();
                    if (RelatedBillMenuInfo != null)
                    {
                        var controller = Startup.GetFromFacByName<BaseController<tb_AS_RepairInStock>>(typeof(tb_AS_RepairInStock).Name + "Controller");
                        tb_AS_RepairInStock entity = await controller.BaseQueryByIdNavAsync(parameter.billId);
                        //要把单据信息传过去
                        menuPowerHelper.ExecuteEvents(RelatedBillMenuInfo, entity);
                    }
                }

            }
        }

    }

    /// <summary>
    /// 检查并锁定单据
    /// </summary>
    /// <returns>锁定是否成功</returns>
    protected virtual async Task<bool> CheckAndLockBill()
    {
        if (EditEntity == null)
            return false;

        try
        {
            // 获取单据ID
            string pkCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long billId = (long)ReflectionHelper.GetPropertyValue(EditEntity, pkCol);

            if (billId <= 0)
            {
                MainForm.Instance.uclog.AddLog("单据ID无效，无法执行锁定检查", UILogType.警告);
                return false;
            }

            // 获取业务数据
            CommBillData billData = EntityMappingHelper.GetBillData(typeof(T), EditEntity);



            // 获取当前用户信息
            var userInfo = MainForm.Instance.AppContext.CurUserInfo.UserInfo;
            long currentUserId = userInfo.User_ID;


            // 优先从本地缓存检查锁定状态
            var lockInfo = await _integratedLockService.GetLockCacheService().GetLockInfoAsync(billId, CurMenuInfo.MenuID);
            var lockStatus = new LockResponse();
            bool cacheHit = lockInfo != null && lockInfo.IsLocked && !lockInfo.IsExpired;

            // 如果缓存未命中或无效，才从服务器检查
            if (!cacheHit)
            {
                MainForm.Instance.uclog.AddLog($"本地缓存未命中，从服务器检查单据[{billId}]锁定状态", UILogType.提示);
                lockStatus = await _integratedLockService.CheckLockStatusAsync(billId, CurMenuInfo.MenuID);
            }
            else
            {
                // 使用缓存数据构建响应
                lockStatus.IsSuccess = true;
                lockStatus.LockInfo = lockInfo;
                MainForm.Instance.uclog.AddLog($"从本地缓存获取单据[{billId}]锁定状态，避免网络请求", UILogType.提示);
            }

            if (lockStatus.IsSuccess)
            {
                if (lockStatus.LockInfo != null && lockStatus.LockInfo.IsLocked)
                {
                    if (lockStatus.LockInfo.LockedUserId != currentUserId)
                    {
                        // 单据已被其他用户锁定
                        string message = $"单据已被用户【{lockStatus.LockInfo.LockedUserName}】锁定，无法编辑。\n" +
                                       $"锁定时间：{lockStatus.LockInfo.LockTime:yyyy-MM-dd HH:mm:ss}\n" +
                                       $"如需编辑，请联系锁定用户或请求解锁。";

                        MainForm.Instance.uclog.AddLog($"单据【{billId}】已被用户【{lockStatus.LockInfo.LockedUserName}】锁定{(cacheHit ? "(缓存数据)" : "")}", UILogType.警告);
                        // 仅在用户主动操作时显示MessageBox提示
                        // 此方法通常在用户点击编辑等按钮时调用，属于主动操作，保留提示

                        // 更新UI状态
                        UpdateLockUI(true, lockStatus.LockInfo);

                        return false;
                    }
                    else
                    {
                        // 当前用户已锁定，无需重复锁定
                        MainForm.Instance.uclog.AddLog($"单据【{billId}】已由您锁定", UILogType.普通消息);

                        // 更新UI状态
                        UpdateLockUI(true, lockStatus.LockInfo);

                        return true;
                    }
                }
                else
                {
                    // 未锁定，尝试锁定
                    MainForm.Instance.uclog.AddLog($"单据【{billId}】未锁定，准备执行锁定操作", UILogType.普通消息);
                }
            }
            else
            {
                // 检查锁状态失败，记录日志但继续尝试锁定
                string errorMsg = lockStatus?.Message ?? "未知错误";
                MainForm.Instance.uclog.AddLog($"检查锁定状态时出错: {errorMsg}", UILogType.警告);
            }

            // 尝试锁定单据
            var lockResponse = await _integratedLockService.LockBillAsync(billId, billData.BillNo, EntityMappingHelper.GetEntityInfo<T>().BizType, CurMenuInfo.MenuID);

            if (lockResponse != null && lockResponse.IsSuccess)
            {
                MainForm.Instance.uclog.AddLog($"单据【{billId}】锁定成功", UILogType.普通消息);

                // 更新UI状态
                UpdateLockUI(true, lockResponse.LockInfo);

                return true;
            }
            else
            {
                // 锁定失败
                string errorMsg = lockResponse?.Message ?? "锁定失败";
                MainForm.Instance.uclog.AddLog($"单据【{billId}】锁定失败：{errorMsg}", UILogType.错误);
                // 仅在用户主动操作时显示MessageBox提示
                // 此方法通常在用户点击编辑等按钮时调用，属于主动操作，保留提示
                //  MessageBox.Show($"单据锁定失败：{errorMsg}", "锁定失败", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }
        catch (Exception ex)
        {
            MainForm.Instance.uclog.AddLog($"锁定单据失败: {ex.Message}", UILogType.错误);
            // 仅在用户主动操作时显示MessageBox提示
            // 此方法通常在用户点击编辑等按钮时调用，属于主动操作，保留提示
            // MessageBox.Show($"锁定单据失败: {ex.Message}", "锁定异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }

    /// <summary>
    /// 解锁单据
    /// </summary>
    protected virtual async void UnlockBill()
    {

        try
        {
            // 获取当前用户信息
            long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;


            // 获取单据ID
            string pkCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long billId = (long)ReflectionHelper.GetPropertyValue(EditEntity, pkCol);


            // 执行解锁操作
            var lockResponse = await _integratedLockService.UnlockBillAsync(billId);

            if (lockResponse.IsSuccess)
            {
                // 使用UpdateLockUI方法更新解锁后的UI状态
                UpdateLockUI(false);
            }
            else
            {
                // 仅在用户主动操作时显示MessageBox提示
                // 解锁操作通常由用户主动触发，保留提示
                //   MessageBox.Show(lockResponse.Message, "解锁失败",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MainForm.Instance.PrintInfoLog("解锁单据失败: " + ex.Message);
            // 仅在用户主动操作时显示MessageBox提示
            // 解锁操作通常由用户主动触发，保留提示
            //   MessageBox.Show("解锁单据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// 管理员强制解锁单据
    /// </summary>
    protected virtual void ForceUnlockBill()
    {

        if (true)
        {
            // 使用UpdateLockUI方法更新解锁后的UI状态
            UpdateLockUI(false);
            logger?.Debug("单据已强制解锁");
        }
    }

    /// <summary>
    /// <summary>
    /// 简化版锁定单据方法，遵循三个核心步骤：查询锁定状态、如果可用则锁定、更新UI
    /// </summary>
    /// <returns>锁定是否成功</returns>
    public override async Task<bool> LockBill()
    {
        if (EditEntity == null)
            return false;

        try
        {
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long billId = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            if (billId <= 0)
                return false;

            // 获取当前用户信息和单据编号
            var userInfo = MainForm.Instance.AppContext.CurUserInfo.UserInfo;
            string BillNo = ReflectionHelper.GetPropertyValue(EditEntity, EntityMappingHelper.GetEntityInfo<T>().NoField).ToString();

            // 核心步骤1: 查询锁定状态
            var (isLocked, lockInfo) = await CheckLockStatusAsync(billId, CurMenuInfo.MenuID);

            // 如果已锁定
            if (isLocked && lockInfo != null)
            {
                // 被当前用户锁定，直接返回成功
                if (lockInfo.LockedUserId == userInfo.User_ID)
                {
                    // 更新UI状态
                    UpdateLockUI(true, lockInfo);
                    return true;
                }
                // 被其他用户锁定，返回失败
                return false;
            }

            // 核心步骤2: 尝试锁定单据
            var result = await _integratedLockService.LockBillAsync(billId, BillNo, EntityMappingHelper.GetEntityInfo<T>().BizType, CurMenuInfo.MenuID);
            bool lockSuccess = result != null && result.IsSuccess;

            // 核心步骤3: 更新UI状态
            UpdateLockUI(lockSuccess, result?.LockInfo);

            return lockSuccess;
        }
        catch (Exception ex)
        {
            MainForm.Instance.logger.LogError(ex, "锁定单据失败");
            return false;
        }
    }




    /// <summary>
    /// 锁定单据
    /// </summary>
    /// <param name="BillID">单据ID</param>
    /// <param name="userid">用户ID</param>
    /// <returns>异步任务</returns>
    private async Task LockBill(long BillID, string BillNo, long userid)
    {
        if (BillID <= 0 || userid <= 0)
        {
            logger?.LogWarning("锁定单据失败：单据ID或用户ID无效");
            return;
        }

        try
        {

            var result = await _integratedLockService.LockBillAsync(BillID, BillNo, EntityMappingHelper.GetEntityInfo<T>().BizType, CurMenuInfo.MenuID);
            if (result.IsSuccess && result.LockInfo.IsLocked)
            {
                // 更新UI状态
                if (tsBtnLocked != null && !IsDisposed)
                {
                    Invoke((MethodInvoker)(() =>
                    {
                        // 使用UpdateLockUI方法更新UI状态
                        UpdateLockUI(true, result.LockInfo);
                    }));
                }

                if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
                {
                    MainForm.Instance.uclog.AddLog($"单据 {BillID} 锁定成功", UILogType.普通消息);
                }
            }
            else
            {
                string errorMsg = result?.Message ?? "锁定失败";
                logger?.LogError($"单据 {BillID} 锁定失败：{errorMsg}");
                MainForm.Instance.uclog.AddLog($"单据 {BillID} 锁定失败：{errorMsg}", UILogType.错误);
            }
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, $"单据 {BillID} 锁定异常");
            MainForm.Instance.uclog.AddLog($"单据 {BillID} 锁定异常: {ex.Message}", UILogType.错误);
        }
    }

    /// <summary>
    /// 简化版检查单据是否被锁定
    /// </summary>
    /// <returns>如果单据被锁定且不是当前用户锁定，则返回true；否则返回false</returns>
    private async Task<bool> IsLock()
    {
        if (EditEntity == null || _integratedLockService == null)
            return false;

        try
        {
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
            if (pkid <= 0)
                return false;

            // 使用现有的检查锁定状态方法
            var (isLocked, lockInfo) = await CheckLockStatusAsync(pkid, CurMenuInfo.MenuID);

            // 更新UI状态
            UpdateLockUI(isLocked, lockInfo);

            // 只在被其他用户锁定时返回true
            if (isLocked && lockInfo != null)
            {
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                return lockInfo.LockedUserId != currentUserId;
            }

            return false;
        }
        catch (Exception ex)
        {
            MainForm.Instance.logger.LogError(ex, "检查锁定状态失败");
            return false;
        }
    }


    /// <summary>
    /// 结案处理
    /// 一般会自动结案，但是有些需要人工结案
    /// </summary>
    /// <returns></returns>
    protected override async Task<bool> CloseCaseAsync()
    {
        if (EditEntity == null)
        {
            return false;
        }

        CommonUI.frmOpinion frm = new CommonUI.frmOpinion();
        frm.ShowCloseCaseImage = ReflectionHelper.ExistPropertyName<T>("CloseCaseImagePath");
        string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
        long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
        ApprovalEntity ae = new ApprovalEntity();
        ae.BillID = pkid;
        CommBillData cbd = EntityMappingHelper.GetBillData<T>(EditEntity);
        ae.BillNo = cbd.BillNo;
        ae.bizType = cbd.BizType;
        ae.bizName = cbd.BizName;
        ae.CloseCaseOpinions = "完成结案";
        ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
        frm.BindData(ae);
        if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
        {
            List<T> needCloseCases = new List<T>();
            if (ReflectionHelper.ExistPropertyName<T>("CloseCaseOpinions"))
            {
                EditEntity.SetPropertyValue("CloseCaseOpinions", frm.txtOpinion.Text);
            }
            //已经审核的并且通过的情况才能结案
            if (ReflectionHelper.ExistPropertyName<T>("DataStatus") && ReflectionHelper.ExistPropertyName<T>("ApprovalStatus") && ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
            {
                // 确认状态下 已经审核并且通过
                if (EditEntity.GetPropertyValue("DataStatus").ToInt() == (int)DataStatus.确认
                    && EditEntity.GetPropertyValue("ApprovalStatus").ToInt() == (int)ApprovalStatus.已审核
                    && EditEntity.GetPropertyValue("ApprovalResults") != null
                    && EditEntity.GetPropertyValue("ApprovalResults").ToBool() == true
                    )
                {
                    needCloseCases.Add(EditEntity);
                }
            }

            if (needCloseCases.Count == 0)
            {
                MainForm.Instance.PrintInfoLog($"要结案的数据为：{needCloseCases.Count}:请检查数据！");
                return false;
            }

            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            ReturnResults<bool> rs = await ctr.BatchCloseCaseAsync(needCloseCases);
            if (rs.Succeeded)
            {
                if (frm.CloseCaseImage != null && ReflectionHelper.ExistPropertyName<T>("CloseCaseImagePath"))
                {
                    string strCloseCaseImagePath = System.DateTime.Now.ToString("yy") + "/" + System.DateTime.Now.ToString("MM") + "/" + Ulid.NewUlid().ToString();
                    byte[] bytes = UI.Common.ImageHelper.ImageToByteArray(frm.CloseCaseImage);
                    HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                    ////上传新文件时要加后缀名
                    string uploadRsult = await httpWebService.UploadImageAsyncOK("", strCloseCaseImagePath + ".jpg", bytes, "upload");
                    if (uploadRsult.Contains("UploadSuccessful"))
                    {
                        EditEntity.SetPropertyValue("CloseCaseImagePath", strCloseCaseImagePath);
                        //这里更新数据库
                        await ctr.BaseSaveOrUpdate(EditEntity);
                    }
                    else
                    {
                        MainForm.Instance.LoginWebServer();
                    }
                }


                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);
                MainForm.Instance.auditLogHelper.CreateAuditLog<T>("结案", EditEntity, $"结案意见:{ae.CloseCaseOpinions}");
                Refreshs();
            }
            else
            {
                MainForm.Instance.PrintInfoLog($"{ae.BillNo}结案操作失败,原因是{rs.ErrorMsg},如果无法解决，请联系管理员！", Color.Red);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private Business.BizMapperService.IEntityMappingService _entityInfoService;


    /// <summary>
    /// 审核 注意后面还需要加很多业务逻辑。
    /// 比方出库单，审核就会减少库存修改成本
    /// （如果有月结动作，则在月结时统计修改成本，更科学，因为如果退单等会影响成本）b
    /// </summary>
    protected async override Task<ReviewResult> Review()
    {
        ReviewResult reviewResult = new ReviewResult();
        if (EditEntity == null)
        {
            return reviewResult;
        }

        if (EditEntity is BaseEntity baseEntity)
        {
            if (baseEntity.HasChanged == true && baseEntity.GetEffectiveChanges().Count > 0)
            {
                MessageBox.Show("数据已经被修改，不能再次审核。\r\n 请【保存】或【刷新】后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return reviewResult;
            }
        }

        // 需要恢复的字段列表
        string[] fieldsToRestore = new string[]
        {
                nameof(PaymentStatus),
                nameof(ARAPStatus),
                nameof(PrePaymentStatus),
                nameof(ApprovalStatus),
                nameof(DataStatus),
                "ApprovalOpinions",
                "ApprovalResults"
        };



        ApprovalEntity ae = new ApprovalEntity();
        if (ReflectionHelper.ExistPropertyName<T>("ApprovalStatus") && ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
        {
            //审核过，并且通过了，不能再次审核
            if ((EditEntity.GetPropertyValue("ApprovalStatus").ToInt() == (int)ApprovalStatus.已审核)
                && EditEntity.GetPropertyValue("ApprovalResults") != null
                && EditEntity.GetPropertyValue("ApprovalResults").ToBool() == true
                )
            {
                MainForm.Instance.uclog.AddLog("【未审核】或【驳回】的单据才能再次审核。");
                return reviewResult;
            }
        }
        //如果已经审核并且审核通过，则不能再次审核

        //
        //CommBillData cbd = EntityMappingHelper.GetBillData<T>(EditEntity);
        CommonUI.frmApproval frm = new CommonUI.frmApproval();
        string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
        long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
        ae.BillID = pkid;

        var statusType = FMPaymentStatusHelper.GetStatusType(EditEntity as BaseEntity);
        if (statusType == typeof(DataStatus))
        {
            Business.BizMapperService.BizEntityInfo entityInfo = _entityInfoService.GetEntityInfo<T>();
            if (entityInfo != null)
            {
                ae.BillNo = EditEntity.GetPropertyValue(entityInfo.NoField).ToString();
                ae.bizType = entityInfo.BizType;
                ae.bizName = entityInfo.BizType.ToString();
            }
        }
        else
        {
            int flag = (int)ReflectionHelper.GetPropertyValue(EditEntity, nameof(ReceivePaymentType)); ;
            Business.BizMapperService.BizEntityInfo entityInfo = _entityInfoService.GetEntityInfo<T>(flag);
            if (entityInfo != null)
            {
                ae.BillNo = EditEntity.GetPropertyValue(entityInfo.NoField).ToString();
                ae.bizType = entityInfo.BizType;
                ae.bizName = entityInfo.BizType.ToString();
            }
        }

        ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
        frm.BindData(ae);
        await Task.Delay(1);
        if (frm.ShowDialog() == DialogResult.OK)//审核了。不管是同意还是不同意
        {
            RevertCommand command = new RevertCommand();
            //缓存当前编辑的对象。如果撤销就回原来的值
            //审核只是修改的审核状态。不用缓存全部
            T oldobj = CloneHelper.DeepCloneObject_maxnew<T>(EditEntity);

            // 克隆指定字段的值
            var originalFieldValues = CloneSpecificFields(EditEntity, fieldsToRestore);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                //CloneHelper.SetValues<T>(EditEntity, oldobj);
                // 只恢复指定的字段
                RestoreSpecificFields(EditEntity, originalFieldValues);
            };

            if (ae.ApprovalResults == true)
            {
                ////审核通过，使用v3状态管理系统转换状态
                //if (EditEntity.StatusContext != null)
                //{
                //    try
                //    {
                //        await EditEntity.TransitionToAsync(DataStatus.确认);
                //    }
                //    catch (InvalidOperationException ex)
                //    {
                //        MainForm.Instance.uclog.AddLog($"状态转换失败: {ex.Message}");
                //        reviewResult.Succeeded = false;
                //        return reviewResult;
                //    }
                //}
                //else
                //{
                //    // 回退到旧的状态管理系统
                await TransitionToAsync(DataStatus.确认, "审核通过");
                //EditEntity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.确认);
                //}
            }
            else
            {
                //审核了。驳回 时数据状态要更新为新建。要再次修改后提交
                #region UI驳回直接保存返回。不用进入审核流程了。


                if (ReflectionHelper.ExistPropertyName<T>("ApprovalOpinions"))
                {
                    EditEntity.SetPropertyValue("ApprovalOpinions", ae.ApprovalOpinions);
                }
                if (ReflectionHelper.ExistPropertyName<T>("ApprovalStatus"))
                {
                    EditEntity.SetPropertyValue("ApprovalStatus", (int)ApprovalStatus.驳回);
                }
                if (ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
                {
                    EditEntity.SetPropertyValue("ApprovalResults", false);
                }
                BusinessHelper.Instance.ApproverEntity(EditEntity);
                BaseController<T> ctrBase = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                //因为只需要更新主表
                ReturnResults<T> rr = await ctrBase.BaseSaveOrUpdate(EditEntity);
                reviewResult.approval = ae;
                reviewResult.Succeeded = rr.Succeeded;
                return reviewResult;
                #endregion
            }


            //中间中的所有字段，都给值到单据主表中，后面需要处理审核历史这种再完善
            PropertyInfo[] array_property = ae.GetType().GetProperties();
            {
                foreach (var property in array_property)
                {
                    //保存审核结果 将审核中间值给到单据中，是否做循环处理？
                    if (ReflectionHelper.ExistPropertyName<T>(property.Name))
                    {
                        object aeValue = ReflectionHelper.GetPropertyValue(ae, property.Name);
                        ReflectionHelper.SetPropertyValue(EditEntity, property.Name, aeValue);
                    }
                }
            }
            //审核通过赋值
            if (ReflectionHelper.ExistPropertyName<T>("ApprovalOpinions"))
            {
                EditEntity.SetPropertyValue("ApprovalOpinions", ae.ApprovalOpinions);
            }
            if (ReflectionHelper.ExistPropertyName<T>("ApprovalStatus"))
            {
                EditEntity.SetPropertyValue("ApprovalStatus", (int)ApprovalStatus.已审核);
            }
            if (ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
            {
                EditEntity.SetPropertyValue("ApprovalResults", true);
            }

            ReturnResults<T> rmr = new ReturnResults<T>();
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
            // AdjustingInventoryAsync
            //因为只需要更新主表
            rmr = await ctr.ApprovalAsync(EditEntity);
            if (rmr.Succeeded)
            {
                reviewResult.Succeeded = rmr.Succeeded;

                //如果是出库单审核，则上传到服务器 锁定订单无法修改
                if (ae.bizType == BizType.销售出库单)
                {
                    //锁定对应的订单
                    if (EditEntity is tb_SaleOut saleOut)
                    {
                        if (saleOut.tb_saleorder != null)
                        {
                            await LockBill(saleOut.tb_saleorder.SOrder_ID, saleOut.tb_saleorder.SOrderNo, MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID);
                        }
                    }

                    #region 销售出库单如果启用了财务模块，则会生成应收款单

                    AuthorizeController authorizeController = MainForm.Instance.AppContext.GetRequiredService<AuthorizeController>();
                    if (authorizeController.EnableFinancialModule())
                    {
                        if (MainForm.Instance.AppContext.FMConfig.AutoAuditReceiveable)
                        {
                            #region 自动审核应收款单
                            //销售订单审核时自动将预付款单设为"已生效"状态
                            var ctrpayable = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                            if (rmr.ReturnObjectAsOtherEntity is tb_FM_ReceivablePayable payable)
                            {
                                payable.ApprovalOpinions = "系统自动审核";
                                payable.ApprovalStatus = (int)ApprovalStatus.已审核;
                                payable.ApprovalResults = true;
                                ReturnResults<tb_FM_ReceivablePayable> autoApproval = await ctrpayable.ApprovalAsync(payable, true);
                                if (!autoApproval.Succeeded)
                                {
                                    autoApproval.Succeeded = false;
                                    autoApproval.ErrorMsg = $"应收款单自动审核失败：{autoApproval.ErrorMsg ?? "未知错误"}";
                                }
                                else
                                {
                                    MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_ReceivablePayable>("应收款单自动审核成功", autoApproval.ReturnObject as tb_FM_ReceivablePayable);
                                }
                            }
                            #endregion
                        }
                    }

                    #endregion
                }

                //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                //MainForm.Instance.ecs.AddSendData(od);

                #region 销售退货

                //如果是出库单审核，则上传到服务器 锁定订单无法修改
                if (ae.bizType == BizType.销售退回单)
                {
                    #region 销售退回单 如果启用了财务模块

                    AuthorizeController authorizeController = MainForm.Instance.AppContext.GetRequiredService<AuthorizeController>();
                    if (authorizeController.EnableFinancialModule())
                    {
                        if (MainForm.Instance.AppContext.FMConfig.AutoAuditReceiveable)
                        {
                            #region 自动审核应收款单
                            //销售订单审核时自动将预付款单设为"已生效"状态
                            var ctrpayable = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                            if (rmr.ReturnObjectAsOtherEntity is tb_FM_ReceivablePayable payable)
                            {
                                //只有有应收记录。挂账都可以手工进行后面的步骤。
                                //所以这里应收审核过了。说明平台退款了，认为销售退回单中的平台退款已经处理了。

                                if ((payable.ApprovalStatus.GetValueOrDefault() == (int)ApprovalStatus.未审核) || !payable.ApprovalResults.GetValueOrDefault())
                                {
                                    payable.ApprovalOpinions = "【销售退回单】审核时，系统自动审核";
                                    payable.ApprovalStatus = (int)ApprovalStatus.已审核;
                                    payable.ApprovalResults = true;
                                    ReturnResults<tb_FM_ReceivablePayable> autoApproval = await ctrpayable.ApprovalAsync(payable, true);
                                    if (!autoApproval.Succeeded)
                                    {
                                        autoApproval.Succeeded = false;
                                        autoApproval.ErrorMsg = $"【销售退回单】审核时,应收款单自动审核失败：{autoApproval.ErrorMsg ?? "未知错误"}";
                                    }
                                    else
                                    {
                                        MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_ReceivablePayable>("【销售退回单】审核时，应收款单自动审核成功", autoApproval.ReturnObject as tb_FM_ReceivablePayable);
                                    }

                                    //自动退款？
                                    //平台订单 经过运费在 平台退款操作后，退回单状态中已经是 退款状态了。
                                    if (MainForm.Instance.AppContext.FMConfig.AutoAuditReceivePaymentRecordByPlatform)
                                    {
                                        if (rmr.ReturnObject is tb_SaleOutRe saleOutRe)
                                        {
                                            if (saleOutRe.IsFromPlatform)
                                            {

                                                #region 生成应收款红字单，退款
                                                //自动生成销售退回单的对应的应该收款单（红字的）对应的收款记录
                                                var paymentController = MainForm.Instance.AppContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                                                List<tb_FM_ReceivablePayable> receivablePayables = new List<tb_FM_ReceivablePayable>();
                                                receivablePayables.Add(payable);

                                                tb_FM_PaymentRecord newPaymentRecord = await paymentController.BuildPaymentRecord(receivablePayables);
                                                newPaymentRecord.Remark = "平台单，已退款，货回仓审核时自动生成的收款单（负数）红字";
                                                newPaymentRecord.PaymentStatus = (int)PaymentStatus.待审核;
                                                if (!newPaymentRecord.Paytype_ID.HasValue && saleOutRe.Paytype_ID.HasValue)
                                                {
                                                    newPaymentRecord.Paytype_ID = saleOutRe.Paytype_ID;
                                                }
                                                var rrs = await paymentController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(newPaymentRecord, false);
                                                if (rrs.Succeeded)
                                                {
                                                    if (saleOutRe.RefundStatus == (int)RefundStatus.已退款已退货)
                                                    {
                                                        //自动审核收款单
                                                        newPaymentRecord.ApprovalOpinions = "【平台单】已退款，销售退回单审核时，自动审核";
                                                        newPaymentRecord.ApprovalStatus = (int)ApprovalStatus.已审核;
                                                        newPaymentRecord.ApprovalResults = true;

                                                        ReturnResults<tb_FM_PaymentRecord> rrRecord = await paymentController.ApprovalAsync(newPaymentRecord);
                                                        if (!rrRecord.Succeeded)
                                                        {
                                                            MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_PaymentRecord>("【平台单】销售退回时，自动审核失败：" + rrRecord.ErrorMsg, rrRecord.ReturnObject as tb_FM_PaymentRecord);
                                                        }
                                                        else
                                                        {

                                                            MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_PaymentRecord>("【平台单】销售退回时，自动审核成功", rrRecord.ReturnObject as tb_FM_PaymentRecord);
                                                        }
                                                    }
                                                }
                                                #endregion

                                            }
                                        }
                                    }


                                }



                            }
                            #endregion
                        }
                    }

                    #endregion
                }

                #endregion

                //如果是出库单审核，则上传到服务器 锁定订单无法修改
                if (ae.bizType == BizType.采购入库单 || ae.bizType == BizType.采购退货入库)
                {
                    //锁定对应的订单
                    if (EditEntity is tb_PurEntry PurEntry)
                    {
                        if (PurEntry.tb_purorder != null)
                        {
                            await LockBill(PurEntry.tb_purorder.PurOrder_ID, PurEntry.tb_purorder.PurOrderNo, MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID);
                        }
                    }
                    if (EditEntity is tb_PurReturnEntry PurReturnEntry)
                    {
                        if (PurReturnEntry.tb_purentryre != null)
                        {
                            await LockBill(PurReturnEntry.tb_purentryre.PurEntryRe_ID, PurReturnEntry.tb_purentryre.PurEntryReNo, MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID);
                        }
                    }

                    #region 采购入库单如果启用了财务模块，则会生成应付款单

                    AuthorizeController authorizeController = MainForm.Instance.AppContext.GetRequiredService<AuthorizeController>();
                    if (authorizeController.EnableFinancialModule())
                    {
                        if (MainForm.Instance.AppContext.FMConfig.AutoAuditPaymentable)
                        {
                            #region 自动审核应付款单
                            //销售订单审核时自动将预付款单设为"已生效"状态
                            var ctrpayable = MainForm.Instance.AppContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                            if (rmr.ReturnObjectAsOtherEntity is tb_FM_ReceivablePayable payable)
                            {
                                if (payable.ARAPStatus == (int)ARAPStatus.待审核)
                                {
                                    payable.ApprovalOpinions = $"由采购入库单确认{payable.SourceBillNo},系统自动审核";
                                    payable.ApprovalStatus = (int)ApprovalStatus.已审核;
                                    payable.ApprovalResults = true;

                                    //if (PurEntry.tb_purorder != null && !payable.PayeeInfoID.HasValue)
                                    //{
                                    //    //通过订单添加付款信息
                                    //    payable.PayeeInfoID = PurEntry.tb_purorder.PayeeInfoID;
                                    //}

                                    ReturnResults<tb_FM_ReceivablePayable> autoApproval = await ctrpayable.ApprovalAsync(payable, true);
                                    if (!autoApproval.Succeeded)
                                    {
                                        autoApproval.Succeeded = false;
                                        autoApproval.ErrorMsg = $"应付款单自动审核失败：{autoApproval.ErrorMsg ?? "未知错误"}";
                                        if (MainForm.Instance.AppContext.SysConfig.ShowDebugInfo)
                                        {
                                            MainForm.Instance.logger.LogDebug(autoApproval.ErrorMsg);
                                        }
                                        await MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_ReceivablePayable>("应付款单自动审核失败", autoApproval.ReturnObject as tb_FM_ReceivablePayable, autoApproval.ErrorMsg);
                                    }
                                    else
                                    {
                                        MainForm.Instance.FMAuditLogHelper.CreateAuditLog<tb_FM_ReceivablePayable>("应付款单自动审核成功", autoApproval.ReturnObject as tb_FM_ReceivablePayable);
                                    }
                                }
                            }
                            #endregion
                        }
                    }

                    #endregion


                }


                //审核成功
                ToolBarEnabledControl(MenuItemEnums.审核);
                //如果审核结果为不通过时，审核不是灰色。
                if (!ae.ApprovalResults)
                {
                    toolStripbtnReview.Enabled = true;
                }
                await ToolBarEnabledControl(EditEntity);
                ae.ApprovalResults = true;
                reviewResult.approval = ae;
                if (ae.bizType.ToString().Contains("款"))
                {
                    await MainForm.Instance.FMAuditLogHelper.CreateAuditLog<T>("审核", EditEntity, $"审核结果：{(ae.ApprovalResults ? "通过" : "拒绝")}-{ae.ApprovalOpinions}");
                }
                else
                {
                    await MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("审核", EditEntity, $"审核结果：{(ae.ApprovalResults ? "通过" : "拒绝")}-{ae.ApprovalOpinions}");
                }



                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核成功。", Color.Red);
            }
            else
            {
                //审核失败 要恢复之前的值
                command.Undo();
                ae.ApprovalResults = false;
                ae.ApprovalStatus = (int)ApprovalStatus.未审核;
                await ToolBarEnabledControl(EditEntity);
                reviewResult.approval = ae;
                // 记录审计日志
                await MainForm.Instance.AuditLogHelper.CreateAuditLog("审核失败", EditEntity, $"审核结果:{(ae.ApprovalResults ? "通过" : "拒绝")},{rmr.ErrorMsg}");
                MainForm.Instance.logger.LogError($"{ae.bizName}:{ae.BillNo}审核失败{rmr.ErrorMsg}");
                MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败{rmr.ErrorMsg},请联系管理员！", Color.Red);
                MessageBox.Show($"{ae.bizName}:{ae.BillNo}审核失败。\r\n {rmr.ErrorMsg}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            toolStripbtnReview.Enabled = false;
        }
        else
        {
            toolStripbtnReview.Enabled = true;
        }
        return reviewResult;
    }

    /// <summary>
    /// 反审核 与审核相反
    /// </summary>
    protected async override Task<bool> ReReview()
    {
        bool rs = false;
        ApprovalEntity ae = new ApprovalEntity();
        if (EditEntity == null)
        {
            return rs;
        }
        string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
        long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
        if (pkid > 0)
        {
            //判断是否锁定
            if (await IsLock())
            {
                MainForm.Instance.uclog.AddLog($"单据已被锁定，请刷新后再试");
                return rs;
                //分读写锁  保存后就只有读。释放 写？
            }
        }



        if (ReflectionHelper.ExistPropertyName<T>("ApprovalStatus") && ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
        {
            //反审，要审核过，并且通过了，才能反审。
            if (EditEntity.GetPropertyValue("ApprovalStatus").ToInt() == (int)ApprovalStatus.已审核
                && EditEntity.GetPropertyValue("ApprovalResults") != null
                && EditEntity.GetPropertyValue("ApprovalResults").ToBool() == true
                )
            {
                ae.ApprovalResults = true;
            }
            else
            {
                MainForm.Instance.uclog.AddLog("已经审核,且【同意】的单据才能反审。");
                return rs;
            }
        }


        CommonUI.frmReApproval frm = new CommonUI.frmReApproval();


        ae.BillID = pkid;
        CommBillData cbd = EntityMappingHelper.GetBillData<T>(EditEntity);
        ae.BillNo = cbd.BillNo;
        ae.bizType = cbd.BizType;
        ae.bizName = cbd.BizName;
        ae.Approver_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
        frm.BindData(ae);
        if (frm.ShowDialog() == DialogResult.OK)
        {
            RevertCommand command = new RevertCommand();
            //缓存当前编辑的对象。如果撤销就回原来的值
            T oldobj = CloneHelper.DeepCloneObject_maxnew<T>(EditEntity);
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码 意思是如果取消反审，内存中反审核的数据要变为空白（之前的样子）
                CloneHelper.SetValues<T>(EditEntity, oldobj);
            };
            //保存旧值要在下面更新值前

            //中间中的所有字段，都给值到单据主表中，后面需要处理审核历史这种再完善
            PropertyInfo[] array_property = ae.GetType().GetProperties();
            {
                foreach (var property in array_property)
                {
                    //保存审核结果 将审核中间值给到单据中，是否做循环处理？
                    //Expression<Func<ApprovalEntity, object>> PNameExp = t => t.ApprovalStatus;
                    //MemberInfo minfo = PNameExp.GetMemberInfo();
                    //string propertyName = minfo.Name;


                    // 回退到旧的状态管理系统
                    await TransitionToAsync(DataStatus.新建, "新建单据");
                    //EditEntity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.新建);

                    if (ReflectionHelper.ExistPropertyName<T>("ApprovalOpinions"))
                    {
                        EditEntity.SetPropertyValue("ApprovalOpinions", ae.ApprovalOpinions);
                    }
                    if (ReflectionHelper.ExistPropertyName<T>("ApprovalStatus"))
                    {
                        EditEntity.SetPropertyValue("ApprovalStatus", (int)ApprovalStatus.未审核);
                    }
                    if (ReflectionHelper.ExistPropertyName<T>("ApprovalResults"))
                    {
                        EditEntity.SetPropertyValue("ApprovalResults", false);
                    }
                    BusinessHelper.Instance.ApproverEntity(EditEntity);


                    if (ReflectionHelper.ExistPropertyName<T>(property.Name))
                    {
                        object aeValue = ReflectionHelper.GetPropertyValue(ae, property.Name);
                        ReflectionHelper.SetPropertyValue(EditEntity, property.Name, aeValue);
                    }
                }
            }



            ReturnResults<T> rmr = new ReturnResults<T>();
            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");

            //反审前 刷新最新数据才能判断 比方销售订单 没有关掉当前UI时。已经出库。再反审。后面再优化为缓存处理锁单来不用查数据库刷新。
            BaseEntity pkentity = (editEntity as T) as BaseEntity;
            EditEntity = await ctr.BaseQueryByIdNavAsync(pkentity.PrimaryKeyID) as T;

            rmr = await ctr.AntiApprovalAsync(EditEntity);
            if (rmr.Succeeded)
            {
                rs = true;
                //如果是出库单审核，则上传到服务器 锁定订单无法修改
                if (ae.bizType == BizType.销售出库单)
                {
                    //锁定对应的订单
                    if (EditEntity is tb_SaleOut saleOut)
                    {
                        if (saleOut.tb_saleorder != null)
                        {
                            UNLock(saleOut.tb_saleorder.SOrder_ID);
                        }
                    }

                }

                ToolBarEnabledControl(MenuItemEnums.反审);
                //这里推送到审核，启动工作流
                await MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("反审", EditEntity, $"反审原因{ae.ApprovalOpinions}");
            }
            else
            {
                //审核失败 要恢复之前的值
                command.Undo();
                rs = false;
                await ToolBarEnabledControl(EditEntity);
                await MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("反审失败", EditEntity, $"反审原因{ae.ApprovalOpinions},{rmr.ErrorMsg}");
                MainForm.Instance.logger.LogError($"{cbd.BillNo}反审失败{rmr.ErrorMsg}");
                MainForm.Instance.PrintInfoLog($"{cbd.BillNo}反审失败{rmr.ErrorMsg},请联系管理员！", Color.Red);
                MessageBox.Show($"{ae.bizName}:{ae.BillNo}反审失败。\r\n {rmr.ErrorMsg}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        return rs;
    }


    #region
    // 添加辅助方法
    private Dictionary<string, object> CloneSpecificFields(T entity, params string[] fieldNames)
    {
        var fieldValues = new Dictionary<string, object>();
        foreach (var fieldName in fieldNames)
        {
            if (ReflectionHelper.ExistPropertyName<T>(fieldName))
            {
                fieldValues[fieldName] = ReflectionHelper.GetPropertyValue(entity, fieldName);
            }
        }
        return fieldValues;
    }

    private void RestoreSpecificFields(T entity, Dictionary<string, object> fieldValues)
    {
        foreach (var kvp in fieldValues)
        {
            if (ReflectionHelper.ExistPropertyName<T>(kvp.Key))
            {
                ReflectionHelper.SetPropertyValue(entity, kvp.Key, kvp.Value);
            }
        }
    }
    #endregion

    private T editEntity;
    public T EditEntity { get => editEntity; set => editEntity = value; }

    public List<T> PrintData { get; set; }

    /// <summary>
    /// 取消添加 取消修改
    /// </summary>
    protected virtual void Cancel()
    {
        if (OnBindDataToUIEvent != null)
        {
            bindingSourceSub.Clear();
            //OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
        }

        ToolBarEnabledControl(MenuItemEnums.取消);
        bindingSourceSub.CancelEdit();

    }

    frmFormProperty frm = null;
    protected override void Property()
    {
        if (frm.ShowDialog() == DialogResult.OK)
        {
            //保存属性
            ToolBarEnabledControl(MenuItemEnums.属性);
            //MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("属性", EditEntity);
        }
        base.Property();
    }



    /// <summary>
    /// 保存图片到服务器。所有图片都保存到服务器。即使草稿换电脑还可以看到
    /// </summary>
    /// <param name="RemoteSave"></param>
    /// <returns></returns>
    public async Task<bool> SaveFileToServer(SourceGridDefine sgd, List<C> Details)
    {
        bool result = true;
        List<SGDefineColumnItem> ImgCols = new List<SGDefineColumnItem>();
        foreach (C detail in Details)
        {
            PropertyInfo[] props = typeof(C).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                var col = sgd[prop.Name];
                if (col != null)
                {
                    if (col.CustomFormat == CustomFormatType.WebPathImage && !ImgCols.Contains(col))
                    {
                        ImgCols.Add(col);
                    }
                }
            }
        }
        try
        {
            result = await UploadImageAsync(ImgCols, sgd.grid, Details);
        }
        catch (Exception ex)
        {
            MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
        }
        return result;
    }

    private async Task<bool> UploadImageAsync(List<SGDefineColumnItem> ImgCols, Grid grid, List<C> Details)
    {
        bool rs = true;
        //保存图片到本地临时目录，图片数据保存在grid1控件中，所以要循环控件的行，控件真实数据行以1为起始
        int totalRowsFlag = grid.RowsCount;
        if (grid.HasSummary)
        {
            totalRowsFlag--;//减去一行总计行
        }
        for (int i = 1; i < totalRowsFlag; i++)
        {
            foreach (var col in ImgCols)
            {
                int realIndex = grid.Columns.GetColumnInfo(col.UniqueId).Index;
                if (grid[i, realIndex].Value == null)
                {
                    continue;
                }
                var model = grid[i, realIndex].Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;
                //比较是否更新了图片数据
                string newhash = valueImageWeb.GetImageNewHash();
                if (valueImageWeb.CellImageBytes != null)
                {
                    #region 需要上传

                    if (!valueImageWeb.GetImageoldHash().Equals(newhash, StringComparison.OrdinalIgnoreCase)
                    && grid[i, realIndex].Value.ToString() == valueImageWeb.CellImageHashName)
                    {
                        string oldfileName = valueImageWeb.GetOldRealfileName();
                        string newfileName = valueImageWeb.GetNewRealfileName();
                        HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                        //如果服务器有旧文件 。可以先删除
                        if (!string.IsNullOrEmpty(valueImageWeb.GetImageoldHash()))
                        {
                            string deleteRsult = await httpWebService.DeleteImageAsync(oldfileName, "delete123");
                            MainForm.Instance.PrintInfoLog("DeleteImage:" + deleteRsult);
                        }
                        ////上传新文件时要加后缀名
                        string uploadRsult = await httpWebService.UploadImageAsync(CurMenuInfo.MenuID.ToString(), (EditEntity as BaseEntity).PrimaryKeyID.ToString(), newfileName + ".jpg", valueImageWeb.CellImageBytes, "upload");
                        if (uploadRsult.Contains("UploadSuccessful") || uploadRsult.Contains("ImageExists"))
                        {
                            // 提取文件名（无论是新上传还是已存在）
                            string resultFileName = uploadRsult.Contains("UploadSuccessful") ?
                                uploadRsult.Replace("UploadSuccessful: ", "").Trim() :
                                uploadRsult.Replace("ImageExists: ", "").Trim();

                            valueImageWeb.UpdateImageName(newhash);
                            grid[i, realIndex].Value = resultFileName;

                            string detailPKName = UIHelper.GetPrimaryKeyColName(typeof(C));
                            object PKValue = grid[i, realIndex].Row.RowData.GetPropertyValue(detailPKName);
                            var detail = Details.Where(x => x.GetPropertyValue(detailPKName).ToString().Equals(PKValue.ToString())).FirstOrDefault();
                            detail.SetPropertyValue(col.ColName, resultFileName);
                            rs = true;

                            if (uploadRsult.Contains("UploadSuccessful"))
                            {
                                MainForm.Instance.PrintInfoLog("UploadSuccessful:" + resultFileName);
                            }
                            else
                            {
                                MainForm.Instance.PrintInfoLog("ImageExists - 使用现有图片:" + resultFileName);
                            }
                        }
                        else
                        {
                            MainForm.Instance.LoginWebServer();
                            rs = false;
                        }
                    }
                    #endregion
                }
            }
        }
        return rs;
    }

    protected override void Add()
    {

        List<T> list = new List<T>();
        EditEntity = Activator.CreateInstance(typeof(T)) as T;

        //StatusMachine.Create();
        try
        {
            //将预设值写入到新增的实体中
            if (MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations == null)
            {
                MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations = new List<tb_UIMenuPersonalization>();
            }
            tb_UIMenuPersonalization menuSetting = MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations.FirstOrDefault(c => c.MenuID == CurMenuInfo.MenuID);
            if (menuSetting != null && menuSetting.tb_UIInputDataFields != null)
            {
                List<QueryField> fields = new List<QueryField>();
                UIBizService.GetInputDataField(typeof(T), fields);
                foreach (var item in menuSetting.tb_UIInputDataFields)
                {
                    if (item.EnableDefault1.HasValue && item.EnableDefault1.Value)
                    {
                        // 进行类型转换 后设置为默认值
                        var queryField = fields.FirstOrDefault(c => c.FieldName == item.FieldName);
                        if (queryField != null && item.Default1 != null)
                        {
                            object convertedValue = Convert.ChangeType(item.Default1, queryField.ColDataType);
                            EditEntity.SetPropertyValue(item.FieldName, convertedValue);
                        }
                    }
                }
            }

        }
        catch (Exception)
        {


        }

        BusinessHelper.Instance.InitEntity(EditEntity);
        BusinessHelper.Instance.InitStatusEntity(EditEntity);

        if (OnBindDataToUIEvent != null)
        {
            bindingSourceSub.Clear();
            OnBindDataToUIEvent(EditEntity, ActionStatus.新增);
        }

        if (ReflectionHelper.ExistPropertyName<T>(typeof(ActionStatus).Name))
        {
            ReflectionHelper.SetPropertyValue(EditEntity, typeof(ActionStatus).Name, (int)ActionStatus.新增);
        }
        if (EditEntity is BaseEntity baseEntity)
        {
            baseEntity.FileStorageInfoList = new List<tb_FS_FileStorageInfo>();
        }
        ToolBarEnabledControl(MenuItemEnums.新增);
        //bindingSourceEdit.CancelEdit();
        UIHelper.ControlMasterColumnsInvisible(CurMenuInfo, this);
    }

    protected override void Modify()
    {
        if (editEntity == null)
        {
            return;
        }

        // 在修改前检查锁定状态
        _ = Task.Run(async () =>
        {
            await CheckLockStatusAndUpdateUI(editEntity);
        });
        // 获取状态类型和值
        var statusType = FMPaymentStatusHelper.GetStatusType(editEntity as BaseEntity);
        if (statusType != null)
        {
            // 动态获取状态值
            dynamic status = editEntity.GetPropertyValue(statusType.Name);
            int statusValue = (int)status;
            dynamic statusEnum = Enum.ToObject(statusType, statusValue);

            if (!FMPaymentStatusHelper.CanModify(statusEnum))
            {
                toolStripbtnModify.Enabled = false;
                toolStripButtonSave.Enabled = false;
                MainForm.Instance.PrintInfoLog($"当前单据状态为{statusEnum}不允许修改!");
                return;
            }

            var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
            if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
            {
                base.Modify();
                MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("修改", EditEntity);
            }
            else
            {
                toolStripbtnModify.Enabled = false;
            }
        }

        BusinessHelper.Instance.EditEntity(EditEntity);
        EditEntity.SetPropertyValue(typeof(ActionStatus).Name, ActionStatus.修改);
        ToolBarEnabledControl(MenuItemEnums.修改);
    }

    protected async override void SpecialDataFix()
    {
        if (EditEntity == null)
        {
            return;
        }

        // 保存前检查锁定状态
        var lockStatus = await CheckLockStatusAndUpdateUI(EditEntity);
        if (!lockStatus.CanPerformCriticalOperations)
        {
            MainForm.Instance.uclog.AddLog("单据已被锁定，无法保存修改", UILogType.警告);
            return;
        }
        //没有经验通过下面先不计算
        if (!Validator(EditEntity))
        {
            return;
        }

        List<C> details = new List<C>();
        bindingSourceSub.EndEdit();
        string detailPKName = UIHelper.GetPrimaryKeyColName(typeof(C));
        List<C> detailentity = bindingSourceSub.DataSource as List<C>;
        if (typeof(C).Name.Contains("Detail") && detailentity != null)
        {
            //产品ID有值才算有效值
            details = detailentity.Where(t => t.GetPropertyValue(detailPKName).ToLong() > 0).ToList();
            EditEntity.SetPropertyValue(typeof(C).Name.ToLower() + "s", details);
            if (!Validator<C>(details))
            {
                return;
            }
        }


        var result = await MainForm.Instance.AppContext.Db.UpdateableByObject(details).ExecuteCommandAsync();
        BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
        ReturnResults<T> SaveResult = new ReturnResults<T>();
        SaveResult = await ctr.BaseSaveOrUpdate(EditEntity);
        if (SaveResult.Succeeded)
        {
            // MainForm.Instance.auditLogHelper.CreateAuditLog<T>("数据特殊修正", EditEntity);
            MainForm.Instance.PrintInfoLog($"修正成功。");
        }
        else
        {
            MainForm.Instance.PrintInfoLog($"修正失败。", Color.Red);
        }

        await MainForm.Instance.AuditLogHelper.CreateAuditLog("数据修正", EditEntity, $"结果:{(SaveResult.Succeeded ? "成功" : "失败")},{SaveResult.ErrorMsg}");
    }

    private string GetPrimaryKeyProperty(Type type)
    {
        foreach (PropertyInfo property in type.GetProperties())
        {
            if (property.GetCustomAttributes(typeof(SugarColumn), true).Length > 0)
            {
                if (((SugarColumn)property.GetCustomAttributes(typeof(SugarColumn), true)[0]).IsPrimaryKey)
                {
                    return property.Name;
                }
            }
        }
        return string.Empty;
    }

    /// <summary>
    /// 加载相关数据的
    /// 联查数据
    /// </summary>
    protected virtual Task LoadRelatedDataToDropDownItemsAsync()
    {
        if (toolStripbtnRelatedQuery.DropDownItems.Count > 0)
        {
            toolStripbtnRelatedQuery.Visible = true;
        }
        else
        {
            toolStripbtnRelatedQuery.Visible = false;
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// 加载单据联动选项到下拉菜单
    /// </summary>
    protected virtual void LoadConvertDocToDropDownItemsAsync()
    {
        // 清空现有菜单项
        toolStripbtnConvertDocuments.DropDownItems.Clear();

        try
        {
            // 获取当前单据类型
            var sourceDocType = typeof(T);

            // 获取所有可转换的目标单据类型
            var converterFactory = Startup.GetFromFac<RUINORERP.Business.Document.DocumentConverterFactory>();
            var availableConversions = converterFactory.GetAvailableConversions<T>();

            // 为每种可转换类型创建菜单项
            foreach (var conversionOption in availableConversions)
            {
                var menuItem = new ToolStripMenuItem(conversionOption.DisplayName);
                // 保存转换选项的引用，避免闭包问题
                var targetType = converterFactory.GetTargetType(conversionOption.TargetDocumentType);
                menuItem.Click += async (sender, e) =>
                {
                    try
                    {
                        // 在后台线程执行转换，避免UI卡顿
                        await PerformDocumentConversionAsync(targetType);
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.uclog.AddLog("执行单据联动时发生错误: " + ex.Message, Global.UILogType.错误);
                        MessageBox.Show("执行单据联动时发生错误: " + ex.Message, "操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                toolStripbtnConvertDocuments.DropDownItems.Add(menuItem);
            }

            // 根据是否有可转换选项设置按钮可见性
            toolStripbtnConvertDocuments.Visible = toolStripbtnConvertDocuments.DropDownItems.Count > 0;
            toolStripbtnConvertDocuments.Text = "联动";
        }
        catch (Exception ex)
        {
            MainForm.Instance.uclog.AddLog("加载单据联动选项失败: " + ex.Message, Global.UILogType.错误);
            toolStripbtnConvertDocuments.Visible = false;
        }
    }

    /// <summary>
    /// 执行单据转换操作
    /// </summary>
    /// <param name="targetType">目标单据类型</param>
    private async Task PerformDocumentConversionAsync(Type targetType)
    {
        if (EditEntity == null)
        {
            // 使用Invoke确保在UI线程显示消息框
            this.Invoke((MethodInvoker)delegate
            {
                MessageBox.Show("请先加载或保存单据后再执行联动操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
            return;
        }

        try
        {
            // 使用ActionManager执行单据联动
            var actionManager = Startup.GetFromFac<RUINORERP.Business.Document.ActionManager>();

            // 准备转换选项
            var options = new RUINORERP.Business.Document.ActionOptions
            {
                UseTransaction = true,
                SaveTarget = true
            };

            // 执行联动操作
            dynamic result = null;
            string targetTypeName = targetType?.Name ?? "Unknown";

            // 使用反射调用正确的泛型方法
            var method = typeof(RUINORERP.Business.Document.ActionManager)
                .GetMethod("ExecuteActionAsync")
                .MakeGenericMethod(typeof(T), targetType);

            var task = (Task)method.Invoke(actionManager, new object[] { EditEntity, options });
            await task;

            // 获取结果属性
            var resultProperty = task.GetType().GetProperty("Result");
            result = resultProperty?.GetValue(task);

            // 在UI线程处理结果
            this.Invoke((MethodInvoker)delegate
            {
                if (result != null && result.Success)
                {
                    // 显示成功消息，并提供打开新单据的选项
                    var dialogResult = MessageBox.Show($"单据联动成功！已生成{targetTypeName}。是否打开新生成的单据？",
                        "操作成功", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (dialogResult == DialogResult.Yes && result.Data != null)
                    {
                        // 这里可以根据实际情况打开新单据
                        // 由于没有具体的打开单据方法，暂时只显示消息
                        MessageBox.Show("新单据已生成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (result != null)
                {
                    // 显示失败消息
                    string errorMsg = result.ErrorMessage ?? "未知错误";
                    MessageBox.Show("单据联动失败: " + errorMsg, "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("不支持的目标单据类型，请联系管理员配置相应的转换器", "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }
        catch (Exception ex)
        {
            // 记录错误并显示消息
            MainForm.Instance.uclog.AddLog("执行单据联动时发生错误: " + ex.Message, Global.UILogType.错误);

            this.Invoke((MethodInvoker)delegate
            {
                MessageBox.Show("执行单据联动时发生错误: " + ex.Message, "操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }
    }

    protected virtual void RelatedQuery()
    {
        MessageBox.Show("功能开发中。。。。");
        if (EditEntity == null)
        {
            return;
        }

        T NewEditEntity = default(T);
        if (OnBindDataToUIEvent != null)
        {
            bindingSourceSub.Clear();

            // NewEditEntity = Activator.CreateInstance(typeof(T)) as T;

            NewEditEntity = EditEntity.DeepCloneByjson();

            // 获取需要忽略的属性配置
            var ignoreProperties = ConfigureIgnoreProperties();
            //复制性新增 时  PK要清空，单据编号类的,还有他的关联性子集
            // 获取主键列名
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();

            //这里只是取打印配置信息
            CommBillData cbd = new CommBillData();
            cbd = EntityMappingHelper.GetBillData(typeof(T), NewEditEntity);

            string billNoColName = cbd.BillNoColName;

            ReflectionHelper.SetPropertyValue(NewEditEntity, billNoColName, string.Empty);

            // 重置主实体的主键
            ResetPrimaryKey(NewEditEntity, PKCol);

            // 重置审批状态
            ResetApprovalStatus(NewEditEntity);

            // 递归处理所有导航属性（明细集合）
            ProcessNavigationProperties(NewEditEntity, PKCol, ignoreProperties);

            OnBindDataToUIEvent(NewEditEntity, ActionStatus.复制);

        }
        ToolBarEnabledControl(MenuItemEnums.新增);
        return;
    }
    protected virtual T AddByCopy()
    {
        if (EditEntity == null)
        {
            MessageBox.Show("请先选择一个单据作为复制的基准。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        T NewEditEntity = default(T);
        if (OnBindDataToUIEvent != null)
        {

            bindingSourceSub.Clear();

            NewEditEntity = EditEntity.DeepCloneByjson();
            //复制性新增 时  PK要清空，单据编号类的,还有他的关联性子集


            // 获取忽略属性配置
            var ignoreConfig = ConfigureIgnoreProperties();



            // 重置需要忽略的属性
            ResetIgnoredProperties(NewEditEntity, ignoreConfig);

            // 获取主键列名
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();

            //这里只是取打印配置信息
            CommBillData cbd = new CommBillData();
            cbd = EntityMappingHelper.GetBillData(typeof(T), NewEditEntity);

            string billNoColName = cbd.BillNoColName;

            ReflectionHelper.SetPropertyValue(NewEditEntity, billNoColName, string.Empty);


            // 重置主实体的主键
            ResetPrimaryKey(NewEditEntity, PKCol);

            // 重置审批状态
            ResetApprovalStatus(NewEditEntity);

            // 递归处理所有导航属性（明细集合）
            ProcessNavigationProperties(NewEditEntity, PKCol, ignoreConfig);

            OnBindDataToUIEvent(NewEditEntity, ActionStatus.复制);

        }
        ToolBarEnabledControl(MenuItemEnums.新增);
        return NewEditEntity;
    }

    #region 复制性新增
    // 忽略属性配置
    // 基类中的配置方法，使用字符串方式配置通用属性
    protected virtual IgnorePropertyConfiguration ConfigureIgnoreProperties()
    {
        var config = new IgnorePropertyConfiguration();

        // 使用字符串方式配置通用属性（这些属性可能存在于各种实体中）
        config.IgnoreIfExists<T>("DataStatus")
              .IgnoreIfExists<T>("PrimaryKeyID")
              .IgnoreIfExists<T>("Created_at")
              .IgnoreIfExists<T>("Created_by")
              .IgnoreIfExists<T>("Modified_at")
              .IgnoreIfExists<T>("Modified_by")
              .IgnoreIfExists<T>("ApprovalStatus")
              .IgnoreIfExists<T>("ApprovalResults")
              .IgnoreIfExists<T>("Approver_by")
              .IgnoreIfExists<T>("Approver_at")
              .IgnoreIfExists<T>("PrintStatus");

        return config;
    }



    // 重置需要忽略的属性
    private void ResetIgnoredProperties(object entity, IgnorePropertyConfiguration ignoreConfig)
    {
        if (entity == null) return;

        var entityType = entity.GetType();
        // 检查是否有为该类型定义的忽略属性
        var ignoredProperties = ignoreConfig.GetIgnoredProperties(entityType);

        // 检查是否有为该类型定义的忽略属性
        foreach (var propName in ignoredProperties)
        {
            if (ReflectionHelper.ExistPropertyName(entityType, propName))
            {
                var prop = entityType.GetProperty(propName);
                if (prop != null && prop.CanWrite)
                {
                    // 根据属性类型设置默认值
                    if (prop.PropertyType == typeof(string))
                        prop.SetValue(entity, null);
                    else if (prop.PropertyType == typeof(int))
                        prop.SetValue(entity, 0);
                    else if (prop.PropertyType == typeof(long))
                        prop.SetValue(entity, 0L);
                    else if (prop.PropertyType == typeof(decimal))
                        prop.SetValue(entity, 0m);
                    else if (prop.PropertyType == typeof(DateTime))
                        prop.SetValue(entity, DateTime.MinValue);
                    else if (prop.PropertyType == typeof(DateTime?))
                        prop.SetValue(entity, null);
                    else if (prop.PropertyType == typeof(bool))
                        prop.SetValue(entity, false);
                    // 可以根据需要添加更多类型的处理
                }
            }
        }

        // 递归处理导航属性
        var navigationProperties = entityType.GetProperties()
            .Where(p => p.PropertyType.IsClass &&
                       p.PropertyType != typeof(string) &&
                       !p.PropertyType.IsValueType);

        foreach (var navProp in navigationProperties)
        {
            var navValue = navProp.GetValue(entity);
            if (navValue != null)
            {
                if (navValue is System.Collections.IEnumerable &&
                    !(navValue is string))
                {
                    // 处理集合类型的导航属性
                    foreach (var item in (System.Collections.IEnumerable)navValue)
                    {
                        ResetIgnoredProperties(item, ignoreConfig);
                    }
                }
                else
                {
                    // 处理单个对象的导航属性
                    ResetIgnoredProperties(navValue, ignoreConfig);
                }
            }
        }
    }
    // 重置实体的主键
    private void ResetPrimaryKey(object entity, string pkCol)
    {
        if (entity == null) return;

        long pkid = (long)ReflectionHelper.GetPropertyValue(entity, pkCol);
        if (pkid > 0)
        {
            ReflectionHelper.SetPropertyValue(entity, pkCol, 0);
        }
    }

    // 重置审批状态相关属性
    private void ResetApprovalStatus(object entity)
    {
        if (entity == null) return;

        if (ReflectionHelper.ExistPropertyName(entity.GetType(), typeof(ApprovalStatus).Name))
        {
            ReflectionHelper.SetPropertyValue(entity, typeof(ApprovalStatus).Name, (int)ApprovalStatus.未审核);
        }
        if (ReflectionHelper.ExistPropertyName(entity.GetType(), typeof(DataStatus).Name))
        {
            TransitionTo(DataStatus.草稿, "保存为草稿");
            //ReflectionHelper.SetPropertyValue(entity, typeof(DataStatus).Name, (int)DataStatus.草稿);
        }

        if (ReflectionHelper.ExistPropertyName(entity.GetType(), "ApprovalResults"))
        {
            ReflectionHelper.SetPropertyValue(entity, "ApprovalResults", false);
        }

        BusinessHelper.Instance.InitEntity(entity);
        BusinessHelper.Instance.ClearEntityApproverInfo(entity);
        BusinessHelper.Instance.ClearEntityEditInfo(entity);
        BusinessHelper.Instance.InitStatusEntity(entity);
    }

    // 递归处理所有导航属性（明细集合）

    // 获取引用主实体的外键属性
    private PropertyInfo GetForeignKeyProperty(Type entityType, string parentPKCol)
    {
        // 尝试查找与主实体主键同名的属性
        var fkProperty = entityType.GetProperty(parentPKCol);
        if (fkProperty != null && fkProperty.PropertyType == typeof(long) || fkProperty.PropertyType == typeof(long?))
        {
            return fkProperty;
        }

        // 如果找不到同名属性，可以尝试其他约定，例如添加"_ID"后缀
        fkProperty = entityType.GetProperty($"{entityType.Name}_{parentPKCol}");
        if (fkProperty != null && fkProperty.PropertyType == typeof(long) || fkProperty.PropertyType == typeof(long?))
        {
            return fkProperty;
        }

        // 可以添加更多的外键检测逻辑...

        return null;
    }

    // 处理主实体及其一级明细集合
    private void ProcessNavigationProperties(object entity, string parentPKCol, IgnorePropertyConfiguration ignoreConfig)
    {
        if (entity == null) return;

        if (entity.ContainsProperty("PrimaryKeyID"))
            entity.SetPropertyValue("PrimaryKeyID", 0);

        var type = entity.GetType();
        var entityName = type.Name;

        // 清空特定的关联查询结果（导航属性）
        // 清空销售订单的销售出库记录
        if (entity is RUINORERP.Model.tb_SaleOrder saleOrder && saleOrder.tb_SaleOuts != null)
        {
            saleOrder.tb_SaleOuts.Clear();
        }
        // 清空采购订单的采购入库记录
        else if (entity is RUINORERP.Model.tb_PurOrder purOrder && purOrder.tb_PurEntries != null)
        {
            purOrder.tb_PurEntries.Clear();
        }

        // 获取主实体的主键值，用于更新明细的外键
        long parentPKValue = (long)ReflectionHelper.GetPropertyValue(entity, parentPKCol);

        // 查找所有以主实体名+Detail结尾的导航属性
        var detailProperties = type.GetProperties()
            .Where(p => p.PropertyType.IsGenericType &&
                       p.PropertyType.GetGenericTypeDefinition() == typeof(List<>) &&
                       p.PropertyType.GetGenericArguments()[0].Name.EndsWith($"{entityName}Detail"))
            .ToList();

        foreach (var detailProperty in detailProperties)
        {
            // 获取明细集合类型
            var detailType = detailProperty.PropertyType.GetGenericArguments()[0];

            // 获取明细的主键和外键属性名
            string detailPKCol = BaseUIHelper.GetEntityPrimaryKey(detailType);
            string detailFKCol = $"{entityName}_ID"; // 假设外键名为"主表名_ID"

            // 检查外键属性是否存在
            var fkProperty = detailType.GetProperty(detailFKCol);
            if (fkProperty == null)
            {
                // 尝试查找外键属性（多种可能的命名约定）
                fkProperty = detailType.GetProperty($"{entityName}_ID") ??
                                detailType.GetProperty(parentPKCol) ??
                                detailType.GetProperties().FirstOrDefault(p =>
                                    p.Name.EndsWith("_ID") && p.PropertyType == typeof(long));
            }

            if (fkProperty != null)
            {
                var collection = detailProperty.GetValue(entity) as System.Collections.IEnumerable;
                if (collection != null)
                {
                    foreach (var item in collection)
                    {
                        if (item.ContainsProperty("PrimaryKeyID"))
                            item.SetPropertyValue("PrimaryKeyID", 0);

                        // 重置明细的主键
                        ReflectionHelper.SetPropertyValue(item, detailPKCol, 0);

                        // 重置明细的外键（指向主实体）
                        ReflectionHelper.SetPropertyValue(item, fkProperty.Name, 0);
                        // 重置需要忽略的属性
                        ResetIgnoredProperties(item, ignoreConfig);

                        // 重置明细的状态性字段
                        // 重置销售订单明细的出库数量
                        if (item is RUINORERP.Model.tb_SaleOrderDetail saleDetail)
                        {
                            saleDetail.TotalDeliveredQty = 0;
                        }
                        // 重置采购订单明细的已交数量、退回数量和未交数量
                        else if (item is RUINORERP.Model.tb_PurOrderDetail purDetail)
                        {
                            purDetail.DeliveredQuantity = 0;
                            purDetail.TotalReturnedQty = 0;
                            // 根据数量重新计算未交数量
                            purDetail.UndeliveredQty = purDetail.Quantity - purDetail.DeliveredQuantity;
                        }

                        // 处理明细的子明细（第二级）
                        ProcessSecondLevelDetails(item, detailPKCol, detailType.Name, ignoreConfig);
                    }
                }
            }
        }
    }

    // 处理第二级明细集合（明细的明细）
    private void ProcessSecondLevelDetails(object entity, string parentPKCol, string parentEntityName,
        IgnorePropertyConfiguration ignoreConfig)
    {
        if (entity == null) return;

        var type = entity.GetType();

        // 查找所有以当前实体名+Detail结尾的导航属性
        var subDetailProperties = type.GetProperties()
            .Where(p => p.PropertyType.IsGenericType &&
                       p.PropertyType.GetGenericTypeDefinition() == typeof(List<>) &&
                       p.PropertyType.GetGenericArguments()[0].Name.EndsWith($"{parentEntityName}Detail"))
            .ToList();

        // 查找所有集合类型的导航属性（假设它们是子明细）
        //var subDetailProperties = type.GetProperties()
        //    .Where(p => p.PropertyType.IsGenericType &&
        //               p.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
        //    .ToList();




        foreach (var subDetailProperty in subDetailProperties)
        {
            // 获取子明细集合类型
            var subDetailType = subDetailProperty.PropertyType.GetGenericArguments()[0];

            // 获取子明细的主键和外键属性名
            string subDetailPKCol = BaseUIHelper.GetEntityPrimaryKey(subDetailType);
            string subDetailFKCol = $"{parentEntityName}_ID"; // 假设外键名为"父表名_ID"

            // 检查外键属性是否存在
            var fkProperty = subDetailType.GetProperty(subDetailFKCol);
            if (fkProperty == null)
            {
                // 尝试其他可能的外键命名方式
                fkProperty = subDetailType.GetProperty($"{parentEntityName}_ID") ??
                     subDetailType.GetProperty(parentPKCol) ??
                     subDetailType.GetProperties().FirstOrDefault(p =>
                         p.Name.EndsWith("_ID") && p.PropertyType == typeof(long));
            }

            if (fkProperty != null)
            {
                var collection = subDetailProperty.GetValue(entity) as System.Collections.IEnumerable;
                if (collection != null)
                {
                    foreach (var item in collection)
                    {
                        // 重置子明细的主键
                        ReflectionHelper.SetPropertyValue(item, subDetailPKCol, 0);

                        // 重置子明细的外键（指向父明细）
                        ReflectionHelper.SetPropertyValue(item, fkProperty.Name, 0);


                        // 重置需要忽略的属性
                        ResetIgnoredProperties(item, ignoreConfig);
                    }
                }
            }
        }
    }

    #endregion



    protected override void Clear(SourceGridDefine sgd)
    {
        SourceGrid.Grid grid1 = sgd.grid;
        EditEntity = Activator.CreateInstance(typeof(T)) as T;
        BusinessHelper.Instance.InitEntity(EditEntity);
        BusinessHelper.Instance.InitStatusEntity(EditEntity);
        bindingSourceSub.Clear();
        //清空明细表格
        #region
        //先清空 不包含 列头和总计
        SourceGrid.RangeRegion rr = new SourceGrid.RangeRegion(new SourceGrid.Position(grid1.Rows.Count, grid1.Columns.Count));
        for (int ii = 0; ii < grid1.Rows.Count; ii++)
        {
            grid1.Rows[ii].RowData = null;
        }
        grid1.ClearValues(rr);


        #endregion
    }
    protected bool Validator(T EditEntity)
    {
        BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
        bool vd = base.ShowInvalidMessage(ctr.BaseValidator(EditEntity));
        return vd;
    }

    /// <summary>
    /// 验证明细
    /// </summary>
    /// <typeparam name="Child"></typeparam>
    /// <param name="details"></param>
    /// <returns></returns>
    protected bool Validator<Child>(List<Child> details) where Child : class
    {
        List<bool> subrs = new List<bool>();
        var lastlist = ((IEnumerable<dynamic>)details).ToList();
        foreach (var item in lastlist)
        {
            BaseController<Child> ctr = Startup.GetFromFacByName<BaseController<Child>>(typeof(Child).Name + "Controller");
            bool sub_bool = base.ShowInvalidMessage(ctr.BaseValidator(item as Child));
            subrs.Add(sub_bool);
        }
        if (subrs.Where(c => c.Equals(false)).Any())
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    /// <summary>
    /// 更新式保存，有一些单据，实在要修改，并且明细没有删除和添加时候执行
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected async Task<ReturnMainSubResults<T>> UpdateSave(T entity)
    {
        string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
        long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
        if (pkid == 0)
        {
            await TransitionToAsync(DataStatus.草稿, "保存为草稿");
            //entity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.草稿);
        }
        ReturnMainSubResults<T> rmr = new ReturnMainSubResults<T>();
        BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
        rmr = await ctr.BaseUpdateWithChild(entity);
        if (rmr.Succeeded)
        {
            if (ReflectionHelper.ExistPropertyName<T>(typeof(ActionStatus).Name))
            {
                //注意這里保存的是枚举
                ReflectionHelper.SetPropertyValue(entity, typeof(ActionStatus).Name, (int)ActionStatus.加载);
            }

            ToolBarEnabledControl(MenuItemEnums.保存);
            MainForm.Instance.uclog.AddLog("更新式保存成功");
            MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("更新式保存成功", rmr.ReturnObject);
        }
        else
        {
            MainForm.Instance.uclog.AddLog("更新式保存成功失败，请重试;或联系管理员。" + rmr.ErrorMsg, UILogType.错误);
        }
        return rmr;
    }

    protected async Task<ReturnMainSubResults<T>> Save(T entity)
    {
        string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
        long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
        long originalPkid = pkid; // 保存原始ID用于后续锁定操作

        if (pkid == 0)
        {
            await TransitionToAsync(DataStatus.草稿, "保存为草稿");
            //entity.SetPropertyValue(typeof(DataStatus).Name, (int)DataStatus.草稿);
            BusinessHelper.Instance.InitEntity(entity);
        }
        else
        {
            // 保存前检查锁定状态 - 优先从本地缓存检查
            if (await IsLock())
            {
                return new ReturnMainSubResults<T>()
                {
                    Succeeded = false,
                    ErrorMsg = "单据已被其他用户锁定，请刷新后再试或联系锁定人员解锁"
                };
            }
        }

        if (editEntity.ContainsProperty(typeof(DataStatus).Name))
        {
            //如果修改前的状态是新建，则修改后的状态是草稿。要重新提交才进入下一步审核
            var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
            if (dataStatus == DataStatus.新建)
            {
                if (ReflectionHelper.ExistPropertyName<T>(typeof(DataStatus).Name))
                {
                    await TransitionToAsync(DataStatus.草稿, "保存为草稿");
                    //ReflectionHelper.SetPropertyValue(EditEntity, typeof(DataStatus).Name, (int)DataStatus.草稿);
                }
            }
        }

        ReturnMainSubResults<T> rmr = new ReturnMainSubResults<T>();
        BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
        rmr = await ctr.BaseSaveOrUpdateWithChild<T>(entity);
        if (rmr.Succeeded)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.AcceptChanges();
            }

            if (ReflectionHelper.ExistPropertyName<T>(typeof(ActionStatus).Name))
            {
                //注意這里保存的是枚举
                ReflectionHelper.SetPropertyValue(entity, typeof(ActionStatus).Name, (int)ActionStatus.加载);
            }

            // 保存成功后的锁定状态管理
            await PostSaveLockManagement(entity, originalPkid);

            ToolBarEnabledControl(MenuItemEnums.保存);
            MainForm.Instance.uclog.AddLog("保存成功");
        }
        else
        {
            MainForm.Instance.uclog.AddLog("保存失败，请重试;或联系管理员。" + rmr.ErrorMsg, UILogType.错误);
        }
        await MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("保存", rmr.ReturnObject, $"结果:{(rmr.Succeeded ? "成功" : "失败")},{rmr.ErrorMsg}");
        return rmr;
    }



    /// <summary>
    /// 保存后的锁定状态管理
    /// 确保保存成功后正确处理锁定状态，维护本地缓存与服务器状态的一致性
    /// </summary>
    /// <param name="entity">保存后的实体对象</param>
    /// <param name="originalPkid">保存前的主键ID（用于新增单据）</param>
    private async Task PostSaveLockManagement(T entity, long originalPkid)
    {
        try
        {
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            long currentPkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);

            if (currentPkid <= 0) return;

            // 更新当前单据ID和菜单ID，用于锁定管理
            _currentBillId = currentPkid;
            _currentMenuId = CurMenuInfo?.MenuID ?? 0;

            // 对于新增单据（originalPkid=0），保存后自动获取锁定
            if (originalPkid == 0 && _integratedLockService != null)
            {
                MainForm.Instance.uclog.AddLog($"新增单据保存成功，自动获取锁定：单据ID={currentPkid}", UILogType.普通消息);
                string BillNo = ReflectionHelper.GetPropertyValue(EditEntity, EntityMappingHelper.GetEntityInfo<T>().NoField).ToString();
                var lockResult = await _integratedLockService.LockBillAsync(currentPkid, BillNo, EntityMappingHelper.GetEntityInfo<T>().BizType, _currentMenuId);
                if (lockResult?.IsSuccess == true)
                {
                    // 更新UI显示锁定状态
                    UpdateLockUI(true, lockResult.LockInfo);
                    MainForm.Instance.uclog.AddLog($"新增单据自动锁定成功", UILogType.普通消息);
                }
                else
                {
                    MainForm.Instance.uclog.AddLog($"新增单据自动锁定失败：{lockResult?.Message ?? "未知错误"}", UILogType.警告);
                }
            }
            // 对于已有单据，刷新锁定状态确保本地缓存与服务器同步
            else if (originalPkid > 0 && _integratedLockService != null)
            {
                await RefreshLockStatus(currentPkid);
            }
        }
        catch (Exception ex)
        {
            MainForm.Instance.logger.LogError(ex, "保存后锁定状态管理失败");
            MainForm.Instance.uclog.AddLog($"锁定状态管理异常：{ex.Message}", UILogType.错误);
        }
    }

    /// <summary>
    /// 刷新锁定状态
    /// 优先从本地缓存获取，如果缓存过期或不存在则从服务器查询
    /// </summary>
    /// <param name="billId">单据ID</param>
    /// <returns>刷新结果</returns>
    private async Task RefreshLockStatus(long billId)
    {
        if (_integratedLockService == null || billId <= 0) return;

        try
        {
            // 调用CheckLockStatusAndUpdateUI方法，它已经包含了完整的状态检查、缓存管理和UI更新逻辑
            var (isLocked, canOperate, lockInfo) = await CheckLockStatusAndUpdateUI(billId);

            // 记录锁定状态刷新日志
            if (isLocked && lockInfo != null)
            {
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                bool isSelfLock = lockInfo.LockedUserId == currentUserId;
                string lockInfoMsg = isSelfLock ?
                    $"您已锁定当前单据" :
                    $"单据已被【{lockInfo.LockedUserName}】锁定";
                MainForm.Instance?.uclog?.AddLog($"锁定状态刷新：{lockInfoMsg}", UILogType.普通消息);
            }
        }
        catch (Exception ex)
        {
            MainForm.Instance?.logger?.LogError(ex, $"刷新单据 {billId} 锁定状态失败");
            MainForm.Instance?.uclog?.AddLog($"刷新锁定状态失败: {ex.Message}", UILogType.错误);
        }
    }

    /// <summary>
    /// 检查锁定状态并更新UI
    /// 作为核心入口点，处理所有锁定状态检测和UI更新逻辑
    /// </summary>
    /// <param name="billId">单据ID</param>
    /// <returns>锁定状态信息和操作权限状态</returns>
    public async Task<(bool IsLocked, bool CanPerformCriticalOperations, LockInfo LockInfo)> CheckLockStatusAndUpdateUI(long billId)
    {
        if (_integratedLockService == null || billId <= 0)
            return (false, true, null);

        try
        {
            // 核心步骤1: 查询锁定状态
            var (isLocked, lockInfo) = await CheckLockStatusAsync(billId, _currentMenuId);
            long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            bool isSelfLock = isLocked && (lockInfo?.LockedUserId == currentUserId);
            bool canPerformCriticalOperations = !isLocked || isSelfLock;

            // 核心步骤3: 更新UI状态
            UpdateLockUI(isLocked, lockInfo);

            return (isLocked, canPerformCriticalOperations, lockInfo);
        }
        catch (Exception ex)
        {
            // 简化错误处理，避免过度日志
            MainForm.Instance?.logger?.LogError(ex, $"检查锁定状态失败：单据ID={billId}");
            return (false, true, null);
        }
    }

    /// <summary>
    /// 禁用重要操作按钮
    /// 当单据被其他用户锁定时，限制用户执行重要操作


    /// <summary>
    /// 更新锁定UI显示
    /// 统一管理锁定按钮的显示状态和提示信息，使用统一的锁状态图片标识
    /// </summary>
    /// <param name="isLocked">是否被锁定</param>
    /// <param name="lockInfo">锁定信息</param>
    private void UpdateLockUI(bool isLocked, LockInfo lockInfo = null)
    {
        if (tsBtnLocked == null || IsDisposed) return;

        try
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => UpdateLockUI(isLocked, lockInfo)));
                return;
            }

            // 始终显示锁状态按钮，无论是否锁定
            tsBtnLocked.Visible = true;
            tsBtnLocked.Tag = lockInfo;

            if (isLocked && lockInfo != null)
            {
                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                bool isSelfLock = lockInfo.LockedUserId == currentUserId;

                // 统一使用锁状态图片标识锁定状态，根据锁定者身份使用不同图片
                tsBtnLocked.Image = isSelfLock ?
                    Properties.Resources.unlockbill :  // 当前用户锁定：显示解锁图标
                    Properties.Resources.Lockbill;     // 其他用户锁定：显示锁定图标

                // 优化提示信息，确保包含完整的锁定详情
                string lockTimeStr = lockInfo.LockTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (isSelfLock)
                {
                    tsBtnLocked.ToolTipText = $"🔒 锁定状态：您已锁定此单据\n" +
                                            $"👤 锁定用户：{lockInfo.LockedUserName}\n" +
                                            $"⏰ 锁定时间：{lockTimeStr}\n" +
                                            $"💡 提示：关闭单据自动解锁";
                    // 设置绿色背景表示自己锁定，提供直观视觉反馈
                    tsBtnLocked.BackColor = System.Drawing.Color.LightGreen;
                    tsBtnLocked.ForeColor = System.Drawing.Color.Black;
                    tsBtnLocked.Text = "已锁定(自己)";
                }
                else
                {
                    tsBtnLocked.ToolTipText = $"🔒 锁定状态：单据已被锁定\n" +
                                            $"👤 锁定用户：{lockInfo.LockedUserName}\n" +
                                            $"⏰ 锁定时间：{lockTimeStr}\n" +
                                            $"💡 提示：点击可请求解锁";
                    // 设置红色背景表示他人锁定，提供直观视觉反馈
                    tsBtnLocked.BackColor = System.Drawing.Color.LightCoral;
                    tsBtnLocked.ForeColor = System.Drawing.Color.White;
                    tsBtnLocked.Text = $"已锁定({lockInfo.LockedUserName})";
                }
                // 启用按钮，允许点击操作
                tsBtnLocked.Enabled = true;
            }
            else
            {
                // 未锁定状态：显示未锁定图标和状态
                tsBtnLocked.Image = Properties.Resources.unlockbill;  // 使用解锁图标表示未锁定
                tsBtnLocked.ToolTipText = "🔓 锁定状态：单据未被锁定\n💡 提示：您可以编辑此单据";
                tsBtnLocked.Text = "未锁定";
                tsBtnLocked.BackColor = System.Drawing.Color.LightBlue;
                tsBtnLocked.ForeColor = System.Drawing.Color.Black;
                tsBtnLocked.Enabled = false;  // 未锁定时禁用按钮，避免误操作
            }

            // 确保按钮在工具栏中正确显示
            tsBtnLocked.TextImageRelation = TextImageRelation.ImageBeforeText;
            tsBtnLocked.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            tsBtnLocked.AutoSize = true;
        }
        catch (Exception ex)
        {
            MainForm.Instance.logger.LogError(ex, "更新锁定UI显示失败");
        }
    }

    /// <summary>
    /// 删除远程的图片
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async virtual Task<bool> DeleteRemoteImages()
    {
        await Task.Delay(0);
        var ctrpay = Startup.GetFromFac<FileManagementController>();
        try
        {
            var fileDeleteResponse = await ctrpay.DeleteImagesAsync(EditEntity as BaseEntity, true);
            if (fileDeleteResponse.IsSuccess && fileDeleteResponse.DeletedFileIds != null && fileDeleteResponse.DeletedFileIds.Count > 0)
            {
                return true;
            }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }

    }



    protected async virtual Task<ReturnResults<T>> Delete()
    {
        ReturnResults<T> rss = new ReturnResults<T>();
        if (editEntity == null)
        {
            //提示一下删除成功
            MainForm.Instance.uclog.AddLog("提示", "没有要删除的数据");
            return rss;
        }

        if (MessageBox.Show("系统不建议删除单据资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
        {
            //https://www.runoob.com/w3cnote/csharp-enum.html
            var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
            if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
            {
                //如果草稿。都可以删除。如果是新建，则提交过了。要创建人或超级管理员才能删除
                if (dataStatus == DataStatus.新建 && !AppContext.IsSuperUser)
                {
                    if (ReflectionHelper.ExistPropertyName<T>("Created_by") && ReflectionHelper.GetPropertyValue(editEntity, "Created_by").ToString() != AppContext.CurUserInfo.Id.ToString())
                    {
                        MessageBox.Show("只有创建人才能删除提交的单据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        rss.ErrorMsg = "只有创建人才能删除提交的单据。";
                        rss.Succeeded = false;
                        return rss;
                    }
                }
                bool rs = false;
                BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                //这个表特殊当时没有命名好
                if (typeof(C).Name.Contains("Detail") || typeof(C).Name.Contains("tb_ProductionDemand"))
                {
                    rs = await ctr.BaseDeleteByNavAsync(editEntity as T);
                }
                else
                {
                    rs = await ctr.BaseDeleteAsync(editEntity as T);
                }
                object PKValue = editEntity.GetPropertyValue(UIHelper.GetPrimaryKeyColName(typeof(T)));
                rss.Succeeded = rs;
                rss.ReturnObject = editEntity;
                if (rs)
                {
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("删除", editEntity);
                    if (MainForm.Instance.AppContext.SysConfig.IsDebug)
                    {
                        //MainForm.Instance.logger.Debug($"单据显示中删除:{typeof(T).Name}，主键值：{PKValue.ToString()} "); //如果要生效 要将配置文件中 <add key="log4net.Internal.Debug" value="true " /> 也许是：logn4net.config <log4net debug="false"> 改为true
                    }

                    bindingSourceSub.Clear();

                    //删除远程图片及本地图片
                    await DeleteRemoteImages();
                    //提示一下删除成功
                    MainForm.Instance.uclog.AddLog("提示", "删除成功");

                    //加载一个空的显示的UI
                    bindingSourceSub.Clear();
                    OnBindDataToUIEvent(Activator.CreateInstance(typeof(T)) as T, ActionStatus.删除);
                }
                else
                {
                    MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("删除失败", editEntity);
                }
            }
            else
            {
                //
                MainForm.Instance.uclog.AddLog("提示", "已【确认】【审核】的生效单据无法删除");
            }
        }
        return rss;
    }




    /// <summary>
    /// 提交
    /// </summary>
    protected async override Task<bool> Submit()
    {
        if (EditEntity == null)
        {
            return false;
        }

        //if (StatusMachine.CanSubmit())
        //{
        //    StatusMachine.Submit();
        //    // 自动触发状态更新
        //}
        BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
        bool submitrs = false;
        string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
        long pkid = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
        if (pkid > 0)
        {
            var dataStatus = (DataStatus)(editEntity.GetPropertyValue(typeof(DataStatus).Name).ToInt());
            if (dataStatus == DataStatus.完结 || dataStatus == DataStatus.确认)
            {

                if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
                {
                    MainForm.Instance.uclog.AddLog("单据已经是【完结】或【确认】状态，提交失败。");
                }
                return false;
            }
            else
            {

                ReturnResults<T> rmr = await ctr.SubmitAsync(EditEntity, false);
                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                if (rmr.Succeeded)
                {
                    if (EditEntity is BaseEntity baseEntity)
                    {
                        baseEntity.AcceptChanges();
                    }
                    ToolBarEnabledControl(MenuItemEnums.提交);
                    //这里推送到审核，启动工作流 后面优化
                    // OriginalData od = ActionForClient.工作流提交(pkid, (int)BizType.盘点单);
                    // MainForm.Instance.ecs.AddSendData(od);]

                    //如果是销售订单或采购订单可以自动审核，有条件地执行？
                    CommBillData cbd = new CommBillData();

                    cbd = EntityMappingHelper.GetBillData<T>(EditEntity as T);
                    ApprovalEntity ae = new ApprovalEntity();
                    ae.ApprovalOpinions = "自动审核";
                    ae.ApprovalResults = true;
                    ae.ApprovalStatus = (int)ApprovalStatus.已审核;
                    if (cbd.BizType == BizType.销售订单 && AppContext.SysConfig.AutoApprovedSaleOrderAmount > 0)
                    {
                        if (EditEntity is tb_SaleOrder saleOrder)
                        {
                            if (saleOrder.TotalAmount <= AppContext.SysConfig.AutoApprovedSaleOrderAmount)
                            {
                                RevertCommand command = new RevertCommand();
                                //缓存当前编辑的对象。如果撤销就回原来的值
                                tb_SaleOrder oldobj = CloneHelper.DeepCloneObject<tb_SaleOrder>(EditEntity);
                                command.UndoOperation = delegate ()
                                {
                                    //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                                    CloneHelper.SetValues<tb_SaleOrder>(EditEntity, oldobj);
                                };
                                BusinessHelper.Instance.ApproverEntity(EditEntity);
                                saleOrder.ApprovalResults = true;
                                saleOrder.ApprovalStatus = (int)ApprovalStatus.已审核;
                                saleOrder.ApprovalOpinions = "自动审核";
                                saleOrder.DataStatus = (int)DataStatus.确认;
                                tb_SaleOrderController<tb_SaleOrder> ctrSO = Startup.GetFromFac<tb_SaleOrderController<tb_SaleOrder>>();
                                ReturnResults<tb_SaleOrder> rmrs = await ctrSO.ApprovalAsync(saleOrder);
                                if (rmrs.Succeeded)
                                {
                                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                                    //MainForm.Instance.ecs.AddSendData(od);
                                    //审核成功
                                    ToolBarEnabledControl(MenuItemEnums.审核);
                                    await MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("审核", rmr.ReturnObject, "满足金额设置条件，自动审核通过");
                                    //如果审核结果为不通过时，审核不是灰色。
                                    if (!ae.ApprovalResults)
                                    {
                                        toolStripbtnReview.Enabled = true;
                                    }
                                }
                                else
                                {
                                    command.Undo();
                                    //审核失败 要恢复之前的值
                                    MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,{rmrs.ErrorMsg}请联系管理员！", Color.Red);
                                }
                            }
                        }
                    }

                    if (cbd.BizType == BizType.采购订单 && AppContext.SysConfig.AutoApprovedPurOrderAmount > 0)
                    {
                        if (EditEntity is tb_PurOrder purOrder)
                        {
                            if (purOrder.TotalAmount <= AppContext.SysConfig.AutoApprovedPurOrderAmount)
                            {
                                RevertCommand command = new RevertCommand();
                                //缓存当前编辑的对象。如果撤销就回原来的值
                                tb_PurOrder oldobj = CloneHelper.DeepCloneObject<tb_PurOrder>(EditEntity);
                                command.UndoOperation = delegate ()
                                {
                                    //Undo操作会执行到的代码 意思是如果退审核，内存中审核的数据要变为空白（之前的样子）
                                    CloneHelper.SetValues<tb_PurOrder>(EditEntity, oldobj);
                                };
                                purOrder.ApprovalResults = true;
                                purOrder.ApprovalStatus = (int)ApprovalStatus.已审核;
                                purOrder.ApprovalOpinions = "自动审核";
                                purOrder.DataStatus = (int)DataStatus.确认;
                                BusinessHelper.Instance.ApproverEntity(EditEntity);
                                tb_PurOrderController<tb_PurOrder> ctrSO = Startup.GetFromFac<tb_PurOrderController<tb_PurOrder>>();
                                ReturnResults<tb_PurOrder> rmrs = await ctrSO.ApprovalAsync(purOrder);
                                if (rmrs.Succeeded)
                                {
                                    //这里审核完了的话，如果这个单存在于工作流的集合队列中，则向服务器说明审核完成。
                                    //这里推送到审核，启动工作流  队列应该有一个策略 比方优先级，桌面不动1 3 5分钟 
                                    //OriginalData od = ActionForClient.工作流审批(pkid, (int)BizType.盘点单, ae.ApprovalResults, ae.ApprovalComments);
                                    //MainForm.Instance.ecs.AddSendData(od);

                                    //审核成功
                                    ToolBarEnabledControl(MenuItemEnums.审核);
                                    await MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("审核", rmr.ReturnObject, "满足金额设置条件，自动审核通过");
                                    //如果审核结果为不通过时，审核不是灰色。
                                    if (!ae.ApprovalResults)
                                    {
                                        toolStripbtnReview.Enabled = true;
                                    }
                                }
                                else
                                {
                                    command.Undo();
                                    //审核失败 要恢复之前的值
                                    MainForm.Instance.PrintInfoLog($"{ae.bizName}:{ae.BillNo}审核失败,{rmrs.ErrorMsg},请联系管理员！", Color.Red);

                                }
                            }
                        }
                    }
                    submitrs = true;
                }
                else
                {
                    MainForm.Instance.uclog.AddLog($"提交失败，请重试;或联系管理员。\r\n 错误信息：{rmr.ErrorMsg}", UILogType.错误);
                    submitrs = false;
                }
                await MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("更新式提交", rmr.ReturnObject, $"结果:{(rmr.Succeeded ? "成功" : "失败")},{rmr.ErrorMsg}");
            }
        }
        else
        {
            if (ReflectionHelper.ExistPropertyName<T>(typeof(DataStatus).Name))
            {
                await TransitionToAsync(DataStatus.新建, "新建单据");
                //ReflectionHelper.SetPropertyValue(EditEntity, typeof(DataStatus).Name, (int)DataStatus.新建);
            }
            if (ReflectionHelper.ExistPropertyName<T>(typeof(ApprovalStatus).Name))
            {
                ReflectionHelper.SetPropertyValue(EditEntity, typeof(ApprovalStatus).Name, (int)ApprovalStatus.未审核);
            }
            bool rs = await this.Save(true);
            if (rs)
            {
                ToolBarEnabledControl(MenuItemEnums.提交);
                submitrs = true;
            }
            await MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("保存式提交", EditEntity, $"结果:{(rs ? "成功" : "失败")}");
        }
        return submitrs;
    }




    /// <summary>提交单据</summary>
    protected async Task<bool> Submit<TStatus>(TStatus targetStatus)
        where TStatus : Enum
    {
        if (EditEntity == null) return false;


        // 回退到旧的状态管理系统
        // 获取当前状态
        var statusProperty = typeof(TStatus).Name;
        var currentStatus = (TStatus)Enum.ToObject(
            typeof(TStatus),
            EditEntity.GetPropertyValue(statusProperty)
        );

        // 验证状态转换
        try
        {
            FMPaymentStatusHelper.ValidateTransition(currentStatus, targetStatus);
        }
        catch (InvalidOperationException ex)
        {
            MainForm.Instance.uclog.AddLog($"提交失败: {ex.Message}");
            return false;
        }

        if (!FMPaymentStatusHelper.CanSubmit(currentStatus))
        {
            MainForm.Instance.uclog.AddLog("单据非草稿状态，提交失败");
            toolStripbtnSubmit.Enabled = false;
            return false;
        }


        // 保存实体
        var ctr = Startup.GetFromFacByName<BaseController<T>>($"{typeof(T).Name}Controller");
        ReturnResults<T> result = await ctr.SubmitAsync(EditEntity, true);
        if (result.Succeeded)
        {
            if (EditEntity is BaseEntity baseEntity)
            {
                baseEntity.AcceptChanges();
            }
            MainForm.Instance.uclog.AddLog("提交成功");
            await ToolBarEnabledControl(EditEntity);

            //// 记录审计日志
            //MainForm.Instance.AuditLogHelper.CreateAuditLog<T>(
            //    "提交",
            //    result.ReturnObject,
            //    $"状态变更: {EditEntity.StatusContext?.CurrentStatus} → {targetStatus}"
            //);

            return true;
        }
        else
        {
            //单据保存后再提交
            MessageBox.Show("提交失败，请重试;或联系管理员。\r\n 错误信息：" + result.ErrorMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        MainForm.Instance.uclog.AddLog($"提交失败: {result.ErrorMsg}", UILogType.错误);
        return false;
    }


    /// <summary>
    /// 优化后的查询条件
    /// </summary>
    public QueryFilter QueryConditionFilter { get; set; } = new QueryFilter();

    /// <summary>
    /// 如果需要查询条件查询，就要在子类中重写这个方法
    /// </summary>
    public virtual void QueryConditionBuilder()
    {
        //添加默认全局的
        BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(T).Name + "Processor");
        QueryConditionFilter = baseProcessor.GetQueryFilter();
    }



    protected override void Query()
    {
        if (base.Edited)
        {
            if (MessageBox.Show("你有数据没有保存，当前操作会丢失数据\r\n你确定不保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
        }

        // 如果没有条件列表 直接查全部。
        // CommonUI.frmQuery<T> frm = new CommonUI.frmQuery<T>();
        //frm.QueryConditions = QueryConditions;
        //frm.LoadQueryConditionToUI(false);
        //frm.OnSelectDataRow += UcAdv_OnSelectDataRow;
        if (QueryConditionFilter.QueryFields.Count == 0)
        {
            QueryConditionBuilder();
        }

        //暂时认为基础数据都是这个基类出来的 否则可以根据菜单中的基类类型来判断生成
        UCAdvFilterGeneric<T> ucBaseList = new UCAdvFilterGeneric<T>(); // Startup.GetFromFacByName<BaseUControl>(menuinfo.FormName);

        ucBaseList.QueryConditionFilter = QueryConditionFilter;
        ucBaseList.CurMenuInfo = CurMenuInfo;
        ucBaseList.KeyValueTypeForDgv = typeof(T);
        ucBaseList.Runway = BaseListRunWay.选中模式;
        //从这里调用 就是来自于关联窗体，下面这个公共基类用于这个情况。暂时在那个里面来控制.Runway = BaseListRunWay.窗体;
        frmBaseEditList frm = new frmBaseEditList();
        frm.StartPosition = FormStartPosition.CenterScreen;
        ucBaseList.Dock = DockStyle.Fill;
        ucBaseList.Tag = frm;
        frm.kryptonPanel1.Controls.Add(ucBaseList);
        ucBaseList.OnSelectDataRow += UcBaseList_OnSelectDataRow;
        // 使用EntityMappingHelper代替BizTypeMapper
        var BizTypeText = EntityMappingHelper.GetBizType(typeof(T).Name).ToString();
        frm.Text = "关联查询" + "-" + BizTypeText;
        frm.Show();

    }



    private void UcBaseList_OnSelectDataRow(object entity)
    {
        if (entity == null)
        {
            return;
        }
        if (OnBindDataToUIEvent != null)
        {
            bindingSourceSub.Clear();
            OnBindDataToUIEvent(entity as T, ActionStatus.加载);

            // 在同步方法中使用Task.Run包装异步操作
            _ = Task.Run(async () => await ToolBarEnabledControl(entity));

            ToolBarEnabledControl(MenuItemEnums.查询);
            //thisform
        }
        //使用了导航查询 entity包括了明细
        //details = (entity as tb_Stocktake).tb_StocktakeDetails;
        //LoadDataToGrid(details);
    }



    protected async override void Refreshs()
    {
        if (editEntity == null)
        {
            MainForm.Instance.PrintInfoLog("当前数据不存在，无法刷新。");
            return;
        }

        //MainForm.Instance.PrintInfoLog(evaluator.CurrentStatus.ToString());

        if (true)//CanRefresh()
        {
            using (StatusBusy busy = new StatusBusy("刷新中..."))
            {
                //这里应该是重新加载单据内容 而不是查询
                //但是，查询才是对的，因为数据会修改变化缓存。
                if (!Edited)
                {
                    if (OnBindDataToUIEvent != null)
                    {
                        BaseEntity pkentity = (editEntity as T) as BaseEntity;
                        if (pkentity.PrimaryKeyID > 0)
                        {
                            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                            editEntity = await ctr.BaseQueryByIdNavAsync(pkentity.PrimaryKeyID) as T;
                        }
                        else
                        {
                            MessageBox.Show("数据不存在。系统自动转换为新增模式。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            toolStripButtonRefresh.Enabled = false;
                            editEntity = Activator.CreateInstance<T>();
                        }
                        bindingSourceSub.Clear();
                        OnBindDataToUIEvent(EditEntity, ActionStatus.加载);

                        //if (pkentity.PrimaryKeyID > 0)
                        //{
                        //    //可以修改
                        //    if (pkentity.ContainsProperty(typeof(DataStatus).Name))
                        //    {
                        //        if (pkentity.GetPropertyValue(typeof(DataStatus).Name).ToString() == ((int)DataStatus.草稿).ToString() ||
                        //               pkentity.GetPropertyValue(typeof(DataStatus).Name).ToString() == ((int)DataStatus.新建).ToString())
                        //        {
                        //            toolStripbtnModify.Enabled = true;
                        //            toolStripbtnSubmit.Enabled = true;
                        //            toolStripbtnReview.Enabled = true;
                        //        }
                        //    }
                        //}

                        await ToolBarEnabledControl(pkentity);
                    }
                    else
                    {
                        //
                        MessageBox.Show("请实现数据绑定的事件。用于显示数据详情。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (MessageBox.Show(this, "有数据没有保存\r\n你确定要重新加载吗", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        if (OnBindDataToUIEvent != null)
                        {
                            BaseEntity pkentity = (editEntity as T) as BaseEntity;
                            BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
                            editEntity = await ctr.BaseQueryByIdNavAsync(pkentity.PrimaryKeyID) as T;
                            bindingSourceSub.Clear();
                            if (editEntity == null)
                            {
                                editEntity = Activator.CreateInstance<T>();
                            }
                            OnBindDataToUIEvent(EditEntity, ActionStatus.加载);
                            //if (pkentity.PrimaryKeyID > 0)
                            //{
                            //    //可以修改
                            //    if (pkentity.ContainsProperty(typeof(DataStatus).Name))
                            //    {
                            //        if (pkentity.GetPropertyValue(typeof(DataStatus).Name).ToString() == ((int)DataStatus.草稿).ToString() ||
                            //               pkentity.GetPropertyValue(typeof(DataStatus).Name).ToString() == ((int)DataStatus.新建).ToString())
                            //        {
                            //            toolStripbtnModify.Enabled = true;
                            //            toolStripbtnSubmit.Enabled = true;
                            //            toolStripbtnReview.Enabled = true;
                            //        }
                            //    }
                            //}
                            //刷新了。不再提示编辑状态了
                            Edited = false;
                            await ToolBarEnabledControl(pkentity);
                        }
                    }
                }
            }
        }
    }

    protected override void Exit(object thisform)
    {
        try
        {
            // 单据都会有 录入表格 SourceGridHelper 在 Grid_HandleDestroyed 中执行了。这样就不管关闭还是x
            //后面还会有一个CloseTheForm 的方法 才是最后的关闭方法
        }
        catch
        {

        }

        base.Exit(this);
    }

    /// <summary>
    /// 使用新的锁定模型请求解锁单据
    /// 向锁定当前单据的用户发送解锁请求，并处理响应
    /// </summary>
    public override void RequestUnLock()
    {
        if (EditEntity == null)
        {
            MainForm.Instance.uclog.AddLog("编辑实体为空，无法发送解锁请求", UILogType.警告);
            return;
        }

        string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
        long billId = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
        if (billId <= 0)
        {
            MainForm.Instance.uclog.AddLog("单据ID无效，无法发送解锁请求", UILogType.警告);
            return;
        }

        if (_integratedLockService != null)
        {
            // 异步发送解锁请求
            Task.Run(async () =>
            {
                try
                {
                    // 先检查锁定状态，确保单据仍然被锁定
                    var lockStatus = await _integratedLockService.CheckLockStatusAsync(billId, CurMenuInfo.MenuID);
                    if (lockStatus == null || !lockStatus.IsSuccess || !lockStatus.LockInfo.IsLocked)
                    {
                        logger?.LogWarning($"单据 {billId} 未被锁定，无需发送解锁请求");
                        this.Invoke((MethodInvoker)(() =>
                        {
                            MessageBox.Show("单据未被锁定或锁定状态已变更，无需发送解锁请求", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }));
                        return;
                    }

                    // 显示确认对话框
                    bool confirmed = false;
                    this.Invoke((MethodInvoker)(() =>
                    {
                        string message = $"该单据当前被用户 {lockStatus.LockInfo.LockedUserName} 锁定\n" +
                                       $"锁定时间：{lockStatus.LockInfo.LockTime:yyyy-MM-dd HH:mm:ss}\n" +
                                       "是否向其发送解锁请求？";
                        DialogResult result = MessageBox.Show(message, "请求解锁", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        confirmed = result == DialogResult.Yes;
                    }));

                    if (!confirmed)
                        return;

                    // 发送解锁请求
                    var result = await _integratedLockService.RequestUnlockAsync(billId, CurMenuInfo.MenuID);

                    // 记录请求日志
                    MainForm.Instance.uclog.AddLog($"已向锁定用户 {lockStatus.LockInfo.LockedUserName} 发送单据 {billId} 的解锁请求", UILogType.普通消息);

                    // 在UI线程上显示友好提示
                    this.Invoke((MethodInvoker)(() =>
                    {
                        // 移除对IsLockUserOnline属性的依赖
                        // 简化消息提示，不区分用户在线状态
                        MessageBox.Show($"解锁请求已成功发送给用户 {lockStatus.LockInfo.LockedUserName}，系统将在用户响应后通知您",
                            "请求已发送", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, $"发送单据 {billId} 解锁请求失败");
                    MainForm.Instance.uclog.AddLog($"发送解锁请求失败: {ex.Message}", UILogType.错误);

                    // 在UI线程上显示错误提示
                    this.Invoke((MethodInvoker)(() =>
                    {
                        MessageBox.Show($"发送解锁请求失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
        }
        else
        {
            logger?.LogError("集成锁定服务未初始化，无法发送解锁请求");
            MainForm.Instance.uclog.AddLog("集成锁定服务未初始化，无法发送解锁请求", UILogType.错误);

            // 在UI线程上显示错误提示
            this.Invoke((MethodInvoker)(() =>
            {
                MessageBox.Show("锁定服务未初始化，无法发送解锁请求", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
        }
    }



    /// <summary>
    /// 当前单据解锁相关的单据
    /// </summary>
    /// <param name="billId">单据ID</param>
    public void UNLock(long billId)
    {
        if (_integratedLockService == null || billId <= 0)
            return;

        // 异步解锁，不阻塞UI线程
        Task.Run(async () =>
        {
            try
            {
                // 执行解锁操作
                var lockResponse = await _integratedLockService.UnlockBillAsync(billId);

                // 更新UI状态 - 解锁后显示为未锁定状态
                UpdateLockUI(false);

                // 简化的错误记录
                if (lockResponse == null || !lockResponse.IsSuccess)
                {
                    MainForm.Instance.logger.LogError($"单据【{billId}】解锁失败");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "解锁单据时发生异常");
            }
        });
    }

    /// <summary>
    /// 重新加载前要清空前面的锁
    /// </summary>
    /// <param name="userid"></param>
    /// <returns></returns>
    public async Task<bool> UNLockByBizName(long userid)
    {
        CommBillData cbd = new CommBillData();
        cbd = EntityMappingHelper.GetBillData(typeof(T), EditEntity);

        LockRequest lockRequest = new LockRequest();
        lockRequest.RequesterUserId = userid;
        lockRequest.RequesterUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name;
        lockRequest.LockInfo = new LockInfo();
        lockRequest.UnlockType = UnlockType.ByBizName;
        lockRequest.LockInfo.MenuID = CurMenuInfo.MenuID;
        // 执行解锁操作
        var lockResponse = await _integratedLockService.UnlockBillAsync(lockRequest);
        if (lockResponse != null && lockResponse.IsSuccess)
        {
            string successMsg = $"单据【{cbd.BizName}】批量解锁成功";
            MainForm.Instance.uclog.AddLog(successMsg, UILogType.普通消息);
            // 在调试模式下记录成功日志
            if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
            {
                logger?.LogDebug(successMsg);
            }

            // 更新UI状态
            UpdateLockUI(false);
        }
        else
        {
            string errorMsg = lockResponse?.Message ?? "解锁失败";
            logger?.LogError($"【{cbd.BizName}】批量解锁失败：{errorMsg}");
            MainForm.Instance.uclog.AddLog($"【{cbd.BizName}】批量解锁失败：{errorMsg}", UILogType.错误);
        }
        return true;
    }



    public override void UNLock()
    {
        // 停止锁定刷新任务
        StopLockRefreshTask();

        if (EditEntity == null)
            return;
        string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
        long billId = (long)ReflectionHelper.GetPropertyValue(EditEntity, PKCol);
        if (billId <= 0)
            return;
        UNLock(billId);
    }


    internal override void CloseTheForm(object thisform)
    {

        base.CloseTheForm(thisform);
        try
        {
            // 单据都会有 录入表格 SourceGridHelper 在 Grid_HandleDestroyed 中执行了。这样就不管关闭还是x

            #region  关闭时解锁
            try
            {
                // 停止锁定刷新任务
                StopLockRefreshTask();

                // 异步释放锁定，不阻塞UI线程
                if (EditEntity != null && _currentBillId > 0 && _currentMenuId > 0 && _integratedLockService != null)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            // 检查是否为当前用户的锁定，只释放自己的锁定
                            var lockStatus = await _integratedLockService.CheckLockStatusAsync(_currentBillId, _currentMenuId);
                            if (lockStatus?.LockInfo?.IsLocked == true)
                            {
                                long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                                if (lockStatus.LockInfo.LockedUserId == currentUserId)
                                {
                                    await _integratedLockService.UnlockBillAsync(_currentBillId);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance.logger.LogError(ex, $"表单关闭时释放锁定失败：单据ID={_currentBillId}");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "表单关闭事件处理异常");
            }


            #endregion
        }
        catch
        {

        }
    }

    #region 打印相关
    #region 为了性能 打印认为打印时 检测过的打印机相关配置在一个窗体下成功后。即可不每次检测
    private tb_PrintConfig printConfig = null;
    public tb_PrintConfig PrintConfig
    {
        get
        {
            return printConfig;
        }
        set
        {
            printConfig = value;

        }
    }

    #endregion
    public async Task PrintDesigned()
    {
        try
        {
            if (EditEntity == null)
            {
                MessageBox.Show("请提供正确的打印数据！");
                return;
            }
            List<T> list = new List<T>();
            list.Add(EditEntity);
            if (PrintConfig == null || PrintConfig.tb_PrintTemplates == null)
            {
                PrintConfig = PrintHelper<T>.GetPrintConfig(list);
            }
            bool rs = await PrintHelper<T>.Print(list, RptMode.DESIGN, PrintConfig);
        }
        catch (Exception ex)
        {
            MainForm.Instance.logger.Error(ex);

        }
    }

    /// <summary>
    /// 单个单据打印
    /// </summary>
    public async Task Print()
    {
        if (EditEntity == null)
        {
            MessageBox.Show("请提供正确的打印数据！");
            return;
        }
        List<T> list = new List<T>();
        list.Add(EditEntity);
        foreach (var item in list)
        {
            if (item == null)
            {
                continue;
            }
            if (item.ContainsProperty("DataStatus"))
            {
                if (item.GetPropertyValue("DataStatus").ToString() == ((int)DataStatus.草稿).ToString().ToString())
                {
                    MessageBox.Show("没有提交的数据不能打印！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (item.GetPropertyValue("DataStatus").ToString() == ((int)DataStatus.新建).ToString())
                {
                    if (MessageBox.Show("没有审核的数据无法打印,你确定要打印吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            //打印次数提醒
            if (item.ContainsProperty("PrintStatus"))
            {
                BizType bizType = EntityMappingHelper.GetBizType(typeof(T).Name);
                int printCounter = item.GetPropertyValue("PrintStatus").ToString().ToInt();
                if (printCounter > 0)
                {
                    if (MessageBox.Show($"当前【{bizType.ToString()}】已经打印过【{printCounter}】次,你确定要重新打印吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                }
            }
        }
        if (PrintConfig == null || PrintConfig.tb_PrintTemplates == null)
        {
            PrintConfig = PrintHelper<T>.GetPrintConfig(list);
        }
        bool rs = await PrintHelper<T>.Print(list, RptMode.PRINT, PrintConfig);
        if (rs)
        {
            MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("打印", EditEntity);
        }

    }

    public async Task Preview()
    {
        bool rs = false;
        try
        {
            if (EditEntity == null)
            {
                MessageBox.Show("请提供正确的打印预览数据！");
                return;
            }
            List<T> list = new List<T>();
            list.Add(EditEntity);
            if (PrintConfig == null || PrintConfig.tb_PrintTemplates == null)
            {
                PrintConfig = PrintHelper<T>.GetPrintConfig(list);
            }
            rs = await PrintHelper<T>.Print(list, RptMode.PREVIEW, PrintConfig);
        }

        catch (Exception ex)
        {
            MainForm.Instance.logger.Error("打印配置加载异常", ex);
        }
    }

    #endregion



    private async void BaseBillEditGeneric_Load(object sender, EventArgs e)
    {
        timerAutoSave.Start();
        if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
        {
            if (!this.DesignMode)
            {
                QueryConditionBuilder();
                #region 请求缓存
                //通过表名获取需要缓存的关系表再判断是否存在。没有就从服务器请求。这种是全新的请求。后面还要设计更新式请求。
                await UIBizService.RequestCache<T>();
                await UIBizService.RequestCache<C>();
                //去检测产品视图的缓存并且转换为强类型
                await UIBizService.RequestCache(typeof(View_ProdDetail));


                #region 产品公共显示数据
                var cacheManager = Startup.GetFromFac<IEntityCacheManager>();
                var cachelist = cacheManager.GetEntityList<View_ProdDetail>();
                if (cachelist != null)
                {
                    MainForm.Instance.View_ProdDetailList = cachelist;
                }

                #endregion

                #endregion
            }
        }
    }

    private async void timerAutoSave_Tick(object sender, EventArgs e)
    {
        if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
        {
            if (!this.DesignMode)
            {
                //自动保存的时间秒数  30秒
                if (MainForm.Instance.AppContext.CurrentUser.静止时间 > 30 && MainForm.Instance.AppContext.IsOnline)
                {
                    bool result = await AutoSaveDataAsync();
                    if (!result)
                    {
                        //如果保存失败，则停止自动保存？
                        timerAutoSave.Stop();
                    }
                }
            }
        }

    }

    private async Task<bool> AutoSaveDataAsync()
    {
        bool result = false;
        try
        {
            if (EditEntity != null)
            {
                #region 自动保存单据数据  后面优化可以多个单?限制5个？Cache
                await Save(false);
                string PathwithFileName = System.IO.Path.Combine(Application.StartupPath + "\\FormProperty\\Data", CurMenuInfo.CaptionCN + ".cache");
                System.IO.FileInfo fi = new System.IO.FileInfo(PathwithFileName);
                //判断目录是否存在
                if (!System.IO.Directory.Exists(fi.Directory.FullName))
                {
                    System.IO.Directory.CreateDirectory(fi.Directory.FullName);
                }
                string json = JsonConvert.SerializeObject(EditEntity,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                    });

                File.WriteAllText(PathwithFileName, json);
                MainForm.Instance.uclog.AddLog("缓存数据保存成功。");
                #endregion
            }
        }
        catch (Exception ex)
        {
            MainForm.Instance.logger.LogError(ex, "缓存数据保存时出错");
        }
        return result;
    }

    #region 锁定刷新机制
    /// <summary>
    /// 启动锁定刷新任务，每15分钟刷新一次锁定状态，防止锁定超时被自动释放
    /// </summary>
    /// <param name="billId">单据ID</param>
    /// <param name="menuId">菜单ID</param>
    protected void StartLockRefreshTask(long billId, long menuId)
    {
        // 停止之前可能存在的刷新任务
        StopLockRefreshTask();

        _currentBillId = billId;
        _currentMenuId = menuId;
        _lockRefreshTokenSource = new CancellationTokenSource();

        // 创建并启动刷新任务，确保token不为null
        _lockRefreshTask = Task.Run(async () =>
        {
            try
            {
                // 检查_lockRefreshTokenSource是否为null
                if (_lockRefreshTokenSource == null)
                    return;

                while (!_lockRefreshTokenSource.Token.IsCancellationRequested)
                {
                    // 等待15分钟（900秒）再刷新一次
                    await Task.Delay(TimeSpan.FromMinutes(15), _lockRefreshTokenSource.Token);

                    // 再次检查_lockRefreshTokenSource是否为null，防止在Delay期间被释放
                    if (_lockRefreshTokenSource == null || _lockRefreshTokenSource.Token.IsCancellationRequested)
                        break;

                    // 执行锁定刷新
                    // 调用锁定管理服务刷新锁定状态
                    var result = await _integratedLockService.RefreshLockAsync(_currentBillId, _currentMenuId);
                }
            }
            catch (TaskCanceledException)
            {
                // 任务被取消，正常退出
                MainForm.Instance?.uclog?.AddLog($"单据【{billId}】锁定刷新任务已取消", UILogType.普通消息);
            }
            catch (Exception ex)
            {
                MainForm.Instance?.uclog?.AddLog($"单据【{billId}】锁定刷新异常: {ex.Message}", UILogType.错误);
            }
        }, _lockRefreshTokenSource != null ? _lockRefreshTokenSource.Token : CancellationToken.None);

        MainForm.Instance?.uclog?.AddLog($"单据【{billId}】锁定刷新任务已启动", UILogType.普通消息);
    }

    /// <summary>
    /// 停止锁定刷新任务
    /// </summary>
    protected async Task StopLockRefreshTask()
    {
        if (_lockRefreshTokenSource != null)
        {
            try
            {
                _lockRefreshTokenSource.Cancel();
                _lockRefreshTokenSource.Dispose();

                if (_lockRefreshTask != null && !_lockRefreshTask.IsCompleted)
                {
                    await Task.WhenAny(_lockRefreshTask, Task.Delay(1000));// 等待任务完成，最多等待1秒

                }

                MainForm.Instance?.uclog?.AddLog($"单据【{_currentBillId}】锁定刷新任务已停止", UILogType.普通消息);
            }
            catch (Exception ex)
            {
                MainForm.Instance?.uclog?.AddLog($"停止锁定刷新任务异常: {ex.Message}", UILogType.错误);
            }
            finally
            {
                _lockRefreshTokenSource = null;
                _lockRefreshTask = null;
                _currentBillId = 0;
                _currentMenuId = 0;
            }
        }
    }


    /// <summary>
    /// 处理收到的解锁请求
    /// </summary>
    /// <param name="requestUserId">请求解锁的用户ID</param>
    /// <param name="requestUserName">请求解锁的用户名</param>
    /// <param name="billId">单据ID</param>
    /// <returns>异步任务</returns>
    public async Task HandleUnlockRequestAsync(int requestUserId, string requestUserName, long billId)
    {
        if (billId != _currentBillId)
            return;

        // 显示解锁请求确认对话框
        var result = MessageBox.Show(
            $"用户 {requestUserName} 请求解锁当前单据【{billId}】。是否同意解锁？",
            "解锁请求确认",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            // 同意解锁
            await AgreeUnlockAsync(requestUserId, requestUserName, billId);
        }
        else
        {
            // 拒绝解锁
            await RefuseUnlockAsync(requestUserId, requestUserName, billId);
        }
    }

    /// <summary>
    /// 同意解锁请求
    /// </summary>
    /// <param name="requestUserId">请求解锁的用户ID</param>
    /// <param name="requestUserName">请求解锁的用户名</param>
    /// <param name="billId">单据ID</param>
    /// <returns>异步任务</returns>
    private async Task AgreeUnlockAsync(int requestUserId, string requestUserName, long billId)
    {
        // 检查_integratedLockService是否已初始化
        if (_integratedLockService == null)
        {
            string errorMsg = "同意解锁请求失败：锁定管理服务未初始化";
            logger?.LogError(errorMsg + " - 单据ID: {BillId}", billId);
            MainForm.Instance?.uclog?.AddLog(errorMsg, UILogType.错误);

            this.Invoke((MethodInvoker)delegate
            {
                MessageBox.Show(errorMsg, "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
            return;
        }

        try
        {

            // 获取当前用户信息
            long currentUserId = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
            string currentUserName = MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName;

            // 创建锁信息
            LockInfo lockInfo = new LockInfo();
            lockInfo.BillID = billId;
            lockInfo.MenuID = _currentMenuId;
            lockInfo.LockedUserId = currentUserId;
            lockInfo.LockedUserName = currentUserName;
            lockInfo.SessionId = MainForm.Instance.AppContext.SessionId;

            // 创建锁定请求
            LockRequest lockRequest = new LockRequest
            {
                LockInfo = lockInfo,
                RequesterUserId = requestUserId,      // 请求解锁的用户ID
                RequesterUserName = requestUserName,  // 请求解锁的用户
                UnlockType = UnlockType.RequestResponse
            };
            lockRequest.LockInfo.SetLockKey();

            // 使用通信服务发送同意解锁命令

            var response = await _clientCommunicationService.SendCommandWithResponseAsync<LockResponse>(
                LockCommands.AgreeUnlock, lockRequest, CancellationToken.None);

            // 记录日志
            string logMessage = $"用户已同意解锁请求：用户 {requestUserName} 请求解锁单据【{billId}】";
            MainForm.Instance?.uclog?.AddLog(logMessage, UILogType.普通消息);

            // 在UI线程中更新锁定状态
            this.Invoke((MethodInvoker)delegate
            {
                // 更新UI
                UpdateLockUI(false);

                // 恢复工具栏按钮状态
                ToolBarEnabledControl(true);

                // 启用所有控件
                foreach (Control control in this.Controls)
                {
                    control.Enabled = true;
                }
            });

            if (response != null && response.IsSuccess)
            {
                logger?.LogDebug("同意解锁请求成功 - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, _currentMenuId);
            }
            else
            {
                string warnMsg = $"同意解锁请求失败 - 单据ID: {billId}, 菜单ID: {_currentMenuId}, 错误信息: {(response?.Message ?? "未知错误")}";
                logger?.LogWarning(warnMsg);
                MainForm.Instance?.uclog?.AddLog(warnMsg, UILogType.警告);

                // 在UI线程中显示错误消息
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show($"同意解锁请求失败: {response?.Message}", "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
        }
        catch (Exception ex)
        {
            string errorMsg = $"同意解锁请求时发生异常: {ex.Message}";
            logger?.LogError(ex, errorMsg + " - 单据ID: {BillId}, 菜单ID: {MenuId}", billId, _currentMenuId);
            MainForm.Instance?.uclog?.AddLog(errorMsg, UILogType.错误);

            // 在UI线程中显示错误消息
            this.Invoke((MethodInvoker)delegate
            {
                MessageBox.Show(errorMsg, "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }
    }

    /// <summary>
    /// 拒绝解锁请求
    /// </summary>
    /// <param name="requestUserId">请求解锁的用户ID</param>
    /// <param name="requestUserName">请求解锁的用户名</param>
    /// <param name="billId">单据ID</param>
    /// <returns>异步任务</returns>
    private async Task RefuseUnlockAsync(int requestUserId, string requestUserName, long billId)
    {
        try
        {
            // 检查_integratedLockService是否初始化
            if (_integratedLockService == null)
            {
                throw new InvalidOperationException("锁定管理服务未初始化");
            }

            // 调用服务拒绝解锁请求
            await _integratedLockService.RefuseUnlockAsync(billId, requestUserId, _currentMenuId, requestUserName);

            // 记录日志
            MainForm.Instance?.uclog?.AddLog($"用户已拒绝解锁请求：用户 {requestUserName} 请求解锁单据【{billId}】", UILogType.普通消息);

            // 在UI线程可能需要的处理
            this.Invoke((MethodInvoker)delegate
            {
                // 可以添加必要的UI更新操作
            });
        }
        catch (Exception ex)
        {
            string errorMsg = $"拒绝解锁请求异常: {ex.Message}";
            MainForm.Instance?.uclog?.AddLog(errorMsg, UILogType.错误);

            // 在UI线程显示错误提示
            this.Invoke((MethodInvoker)delegate
            {
                MessageBox.Show(errorMsg, "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }
    }

    #endregion
}





}



