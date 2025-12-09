using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Model.Base.StatusManager;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// UI状态管理器接口
    /// 统一管理所有UI状态更新操作，避免代码重复
    /// </summary>
    public interface IUIStateManager
    {
        /// <summary>
        /// 更新所有UI状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        void UpdateAllUIStates(BaseEntity entity);
        
        /// <summary>
        /// 异步更新所有UI状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>异步任务</returns>
        Task UpdateAllUIStatesAsync(BaseEntity entity);
        
        /// <summary>
        /// 更新所有按钮状态
        /// </summary>
        /// <param name="currentStatus">当前状态</param>
        void UpdateAllButtonStates(DataStatus currentStatus);
        
        /// <summary>
        /// 异步更新所有按钮状态
        /// </summary>
        /// <param name="currentStatus">当前状态</param>
        /// <returns>异步任务</returns>
        Task UpdateAllButtonStatesAsync(DataStatus currentStatus);
        
        /// <summary>
        /// 更新UI控件状态
        /// </summary>
        /// <param name="currentStatus">当前状态</param>
        void UpdateUIControlsByState(DataStatus currentStatus);
        
        /// <summary>
        /// 更新状态显示
        /// </summary>
        void UpdateStateDisplay();
        
        /// <summary>
        /// 更新子表操作权限
        /// </summary>
        /// <param name="status">当前数据状态</param>
        void UpdateChildTableOperations(DataStatus status);
    }
}