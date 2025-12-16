using System;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Global;

namespace RUINORERP.UI.Examples
{
    /// <summary>
    /// 统一状态管理器使用示例
    /// 展示如何在应用层使用状态管理器进行状态判断和操作
    /// </summary>
    public class StateManagementExample
    {
        private readonly IUnifiedStateManager _stateManager;

        public StateManagementExample(IUnifiedStateManager stateManager)
        {
            _stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));
        }

        /// <summary>
        /// 检查实体是否可以提交
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否可以提交</returns>
        public bool CanEntitySubmit(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 使用统一状态管理器检查是否可以提交
            return _stateManager.CanSubmitEntity(entity);
        }

        /// <summary>
        /// 检查实体是否可以审核
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否可以审核</returns>
        public bool CanEntityApprove(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 使用统一状态管理器检查是否可以审核
            return _stateManager.CanApproveEntity(entity);
        }

        /// <summary>
        /// 检查实体是否可以反审
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否可以反审</returns>
        public bool CanEntityAntiApprove(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 使用统一状态管理器检查是否可以反审
            return _stateManager.CanAntiApproveEntity(entity);
        }

        /// <summary>
        /// 检查实体是否为终态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否为终态</returns>
        public bool IsEntityFinalStatus(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 使用统一状态管理器检查是否为终态
            return _stateManager.IsFinalStatus(entity);
        }

        /// <summary>
        /// 检查实体是否可以修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否可以修改</returns>
        public bool CanEntityModify(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 使用统一状态管理器检查是否可以修改
            return _stateManager.CanModify(entity);
        }

        /// <summary>
        /// 设置实体状态并触发事件
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">状态变更原因</param>
        /// <returns>状态设置结果</returns>
        public async Task<StateTransitionResult> SetEntityStatusAsync(BaseEntity entity, Enum newStatus, string reason = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (newStatus == null)
                throw new ArgumentNullException(nameof(newStatus));

            // 使用统一状态管理器设置状态
            return await _stateManager.SetBusinessStatusAsync(entity, newStatus, reason);
        }

        /// <summary>
        /// 检查操作是否可以执行
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>操作结果</returns>
        public (bool CanExecute, string Message) CheckActionExecution(BaseEntity entity, MenuItemEnums action)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 使用统一状态管理器检查操作是否可以执行
            return _stateManager.CanExecuteActionWithMessage(entity, action);
        }

        /// <summary>
        /// 获取实体对应的UI控件状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>UI控件状态字典</returns>
        public System.Collections.Generic.Dictionary<string, bool> GetUIControlStates(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 使用统一状态管理器获取UI控件状态
            return _stateManager.GetUIControlStates(entity);
        }

        /// <summary>
        /// 获取特定按钮的状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="buttonName">按钮名称</param>
        /// <returns>按钮状态</returns>
        public bool GetButtonState(BaseEntity entity, string buttonName)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (string.IsNullOrEmpty(buttonName))
                throw new ArgumentException("按钮名称不能为空", nameof(buttonName));

            // 使用统一状态管理器获取按钮状态
            return _stateManager.GetButtonState(entity, buttonName);
        }
    }
}