using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.StatusManagerService;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 单据编辑窗体状态管理扩展
    /// 提供简化的状态管理方法，支持DataStatus与业务状态的自动处理
    /// </summary>
    public static class BaseBillEditStatusExtensions
    {
        /// <summary>
        /// 初始化状态管理
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="form">单据编辑窗体</param>
        /// <param name="entity">实体对象</param>
        /// <param name="stateProvider">状态提供程序</param>
        /// <returns>状态管理器</returns>
        public static IUnifiedStateManager InitializeStatusManagement<T>(
            this BaseBillEditGeneric<T> form,
            T entity,
            IUnifiedStateManager stateProvider = null) where T : BaseEntity
        {
            if (form == null || entity == null)
                return null;

            try
            {
                // 如果没有提供状态提供程序，使用默认的
                var stateManager = stateProvider ?? new UnifiedStateManager(
                    form.ServiceProvider?.GetService<ILogger<UnifiedStateManager>>());

                // 设置状态管理器
                if (form.StateManager == null)
                {
                    form.StateManager = stateManager;
                }

                // 初始化状态
                var entityStatus = stateManager.GetCompleteEntityStatus(entity);
                if (entityStatus != null)
                {
                    // 同步状态到UI
                    stateManager.SyncStatusToUI(entity, form.GetControlMapping());
                }

                return stateManager;
            }
            catch (Exception ex)
            {
                // 记录错误日志
                form.Logger?.LogError(ex, "初始化状态管理失败：实体类型 {EntityType}", typeof(T).Name);
                return null;
            }
        }

        /// <summary>
        /// 获取控件映射字典
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="form">单据编辑窗体</param>
        /// <returns>控件映射字典</returns>
        public static Dictionary<string, Control> GetControlMapping<T>(this BaseBillEditGeneric<T> form) where T : BaseEntity
        {
            if (form == null)
                return new Dictionary<string, Control>();

            var mapping = new Dictionary<string, Control>();

            try
            {
                // 获取工具栏按钮
                var toolStripButtons = form.GetType()
                    .GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    .Where(f => f.FieldType == typeof(KryptonButton) || f.FieldType == typeof(Button))
                    .ToDictionary(f => f.Name, f => f.GetValue(form) as Control);

                foreach (var button in toolStripButtons)
                {
                    if (button.Value != null)
                    {
                        mapping[button.Key] = button.Value;
                    }
                }

                // 添加常见按钮的映射
                AddCommonButtonMappings(form, mapping);
            }
            catch (Exception ex)
            {
                form.Logger?.LogError(ex, "获取控件映射失败");
            }

            return mapping;
        }

        /// <summary>
        /// 添加常见按钮映射
        /// </summary>
        private static void AddCommonButtonMappings<T>(BaseBillEditGeneric<T> form, Dictionary<string, Control> mapping) where T : BaseEntity
        {
            try
            {
                // 尝试通过反射获取常见按钮控件
                var commonButtonNames = new[]
                {
                    "toolStripbtnAdd", "toolStripbtnModify", "toolStripButtonSave", "toolStripbtnDelete",
                    "toolStripbtnSubmit", "toolStripbtnReview", "toolStripBtnReverseReview",
                    "toolStripButtonCaseClosed", "toolStripButtonAntiClosed", "toolStripbtnPrint",
                    "toolStripButtonExport"
                };

                foreach (var buttonName in commonButtonNames)
                {
                    try
                    {
                        var field = form.GetType().GetField(buttonName, 
                            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                        if (field != null)
                        {
                            var button = field.GetValue(form) as Control;
                            if (button != null)
                            {
                                mapping[buttonName] = button;
                            }
                        }
                    }
                    catch
                    {
                        // 忽略获取单个按钮失败的情况
                    }
                }
            }
            catch (Exception ex)
            {
                form.Logger?.LogError(ex, "添加常见按钮映射失败");
            }
        }

        /// <summary>
        /// 执行状态操作
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="form">单据编辑窗体</param>
        /// <param name="action">操作类型</param>
        /// <param name="reason">操作原因</param>
        /// <returns>操作是否成功</returns>
        public static async Task<bool> ExecuteStatusActionAsync<T>(
            this BaseBillEditGeneric<T> form,
            MenuItemEnums action,
            string reason = null) where T : BaseEntity
        {
            if (form == null || form.EditEntity == null || form.StateManager == null)
                return false;

            try
            {
                var entity = form.EditEntity;

                // 根据操作类型执行相应的状态变更
                switch (action)
                {
                    case MenuItemEnums.新增:
                        // 新增操作通常不改变状态，只是设置ActionStatus
                        await form.StateManager.SetActionStatusAsync(entity, ActionStatus.新增, reason ?? "新增操作");
                        break;

                    case MenuItemEnums.修改:
                        await form.StateManager.SetActionStatusAsync(entity, ActionStatus.修改, reason ?? "修改操作");
                        break;

                    case MenuItemEnums.保存:
                        // 保存操作根据实体类型决定状态变更
                        await HandleSaveOperation(form, entity, reason);
                        break;

                    case MenuItemEnums.删除:
                        // 删除操作可能需要特殊处理
                        await form.StateManager.SetActionStatusAsync(entity, ActionStatus.删除, reason ?? "删除操作");
                        break;

                    case MenuItemEnums.提交:
                        await HandleSubmitOperation(form, entity, reason);
                        break;

                    case MenuItemEnums.审核:
                        // 审核操作需要更多参数，这里只是示例
                        await HandleApproveOperation(form, entity, reason);
                        break;

                    case MenuItemEnums.反审:
                        await HandleUnapproveOperation(form, entity, reason);
                        break;

                    case MenuItemEnums.结案:
                        await HandleCloseOperation(form, entity, reason);
                        break;

                    case MenuItemEnums.反结案:
                        await HandleUncloseOperation(form, entity, reason);
                        break;

                    default:
                        // 其他操作
                        await form.StateManager.SetActionStatusAsync(entity, ActionStatus.无操作, reason ?? $"{action}操作");
                        break;
                }

                // 同步状态到UI
                form.StateManager.SyncStatusToUI(entity, form.GetControlMapping());

                return true;
            }
            catch (Exception ex)
            {
                form.Logger?.LogError(ex, "执行状态操作失败：操作 {Action}，实体类型 {EntityType}", action, typeof(T).Name);
                return false;
            }
        }

        /// <summary>
        /// 处理保存操作
        /// </summary>
        private static async Task HandleSaveOperation<T>(BaseBillEditGeneric<T> form, T entity, string reason) where T : BaseEntity
        {
            // 根据实体是否有主键值判断是新增还是修改
            var isNew = entity.PrimaryKeyID <= 0;
            
            if (isNew)
            {
                // 新增保存
                if (entity.ContainsProperty(nameof(DataStatus)))
                {
                    await form.StateManager.SetDataStatusAsync(entity, DataStatus.新建, reason ?? "新增保存");
                }
            }
            else
            {
                // 修改保存
                if (entity.ContainsProperty(nameof(DataStatus)))
                {
                    await form.StateManager.SetDataStatusAsync(entity, DataStatus.草稿, reason ?? "修改保存");
                }
            }

            await form.StateManager.SetActionStatusAsync(entity, ActionStatus.保存, reason ?? "保存操作");
        }

        /// <summary>
        /// 处理提交操作
        /// </summary>
        private static async Task HandleSubmitOperation<T>(BaseBillEditGeneric<T> form, T entity, string reason) where T : BaseEntity
        {
            // 提交操作通常是将状态从草稿或新建变为待审核
            if (entity.ContainsProperty(nameof(DataStatus)))
            {
                await form.StateManager.SetDataStatusAsync(entity, DataStatus.新建, reason ?? "提交审核");
            }
            else
            {
                // 如果没有DataStatus，可能是业务状态，根据具体情况处理
                var statusType = form.StateManager.GetStatusType(entity);
                if (statusType != null)
                {
                    // 这里需要根据具体业务状态类型处理
                    // 例如：对于支付单，可能是从"未提交"到"已提交"
                    // 这里只是示例，具体实现需要根据业务状态枚举定义
                }
            }

            await form.StateManager.SetActionStatusAsync(entity, ActionStatus.提交, reason ?? "提交操作");
        }

        /// <summary>
        /// 处理审核操作
        /// </summary>
        private static async Task HandleApproveOperation<T>(BaseBillEditGeneric<T> form, T entity, string reason) where T : BaseEntity
        {
            // 执行审核操作
            await form.StateManager.ExecuteApprovalAsync(entity, 1, true, reason ?? "审核通过");

            await form.StateManager.SetActionStatusAsync(entity, ActionStatus.审核, reason ?? "审核操作");
        }

        /// <summary>
        /// 处理反审操作
        /// </summary>
        private static async Task HandleUnapproveOperation<T>(BaseBillEditGeneric<T> form, T entity, string reason) where T : BaseEntity
        {
            if (entity.ContainsProperty(nameof(DataStatus)))
            {
                await form.StateManager.SetDataStatusAsync(entity, DataStatus.新建, reason ?? "反审核");
            }

            await form.StateManager.SetActionStatusAsync(entity, ActionStatus.反审, reason ?? "反审核操作");
        }

        /// <summary>
        /// 处理结案操作
        /// </summary>
        private static async Task HandleCloseOperation<T>(BaseBillEditGeneric<T> form, T entity, string reason) where T : BaseEntity
        {
            if (entity.ContainsProperty(nameof(DataStatus)))
            {
                await form.StateManager.SetDataStatusAsync(entity, DataStatus.完结, reason ?? "结案");
            }

            await form.StateManager.SetActionStatusAsync(entity, ActionStatus.结案, reason ?? "结案操作");
        }

        /// <summary>
        /// 处理反结案操作
        /// </summary>
        private static async Task HandleUncloseOperation<T>(BaseBillEditGeneric<T> form, T entity, string reason) where T : BaseEntity
        {
            if (entity.ContainsProperty(nameof(DataStatus)))
            {
                await form.StateManager.SetDataStatusAsync(entity, DataStatus.确认, reason ?? "反结案");
            }

            await form.StateManager.SetActionStatusAsync(entity, ActionStatus.反结案, reason ?? "反结案操作");
        }

        /// <summary>
        /// 执行审核操作（带详细参数）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="form">单据编辑窗体</param>
        /// <param name="approvalLevel">审核级别</param>
        /// <param name="approvalResult">审核结果</param>
        /// <param name="opinion">审核意见</param>
        /// <param name="approverId">审核人ID</param>
        /// <returns>操作是否成功</returns>
        public static async Task<bool> ExecuteApprovalAsync<T>(
            this BaseBillEditGeneric<T> form,
            int approvalLevel,
            bool approvalResult,
            string opinion = null,
            long? approverId = null) where T : BaseEntity
        {
            if (form == null || form.EditEntity == null || form.StateManager == null)
                return false;

            try
            {
                var entity = form.EditEntity;
                
                // 执行审核操作
                var success = await form.StateManager.ExecuteApprovalAsync(entity, approvalLevel, approvalResult, opinion, approverId);
                
                if (success)
                {
                    // 同步状态到UI
                    form.StateManager.SyncStatusToUI(entity, form.GetControlMapping());
                }

                return success;
            }
            catch (Exception ex)
            {
                form.Logger?.LogError(ex, "执行审核操作失败：实体类型 {EntityType}", typeof(T).Name);
                return false;
            }
        }

        /// <summary>
        /// 更新UI状态
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="form">单据编辑窗体</param>
        public static void UpdateUIStates<T>(this BaseBillEditGeneric<T> form) where T : BaseEntity
        {
            if (form == null || form.EditEntity == null || form.StateManager == null)
                return;

            try
            {
                var entity = form.EditEntity;
                form.StateManager.SyncStatusToUI(entity, form.GetControlMapping());
            }
            catch (Exception ex)
            {
                form.Logger?.LogError(ex, "更新UI状态失败：实体类型 {EntityType}", typeof(T).Name);
            }
        }

        /// <summary>
        /// 获取实体的完整状态信息
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="form">单据编辑窗体</param>
        /// <returns>实体状态信息</returns>
        public static EntityStatus GetEntityStatus<T>(this BaseBillEditGeneric<T> form) where T : BaseEntity
        {
            if (form == null || form.EditEntity == null || form.StateManager == null)
                return null;

            try
            {
                var entity = form.EditEntity;
                return form.StateManager.GetCompleteEntityStatus(entity);
            }
            catch (Exception ex)
            {
                form.Logger?.LogError(ex, "获取实体状态失败：实体类型 {EntityType}", typeof(T).Name);
                return null;
            }
        }
    }
}