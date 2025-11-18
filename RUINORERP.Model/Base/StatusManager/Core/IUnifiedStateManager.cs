/**
 * 文件: IUnifiedStateManager.cs
 * 说明: 统一状态管理器接口 - 优化版
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base.StatusManager.Core
{
    /// <summary>
    /// 统一状态管理器接口
    /// </summary>
    public interface IUnifiedStateManager
    {
        /// <summary>
        /// 获取实体的状态信息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>实体状态信息</returns>
        EntityStatus GetEntityStatus(BaseEntity entity);
        
        /// <summary>
        /// 获取当前数据性状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>当前数据性状态</returns>
        DataStatus GetDataStatus(BaseEntity entity);
        
        /// <summary>
        /// 设置数据性状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        Task<bool> SetDataStatusAsync(BaseEntity entity, DataStatus status, string reason = null);
        
        /// <summary>
        /// 获取业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>业务性状态</returns>
        T GetBusinessStatus<T>(BaseEntity entity) where T : Enum;
        
        /// <summary>
        /// 设置业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        Task<bool> SetBusinessStatusAsync<T>(BaseEntity entity, T status, string reason = null) where T : Enum;
        
        /// <summary>
        /// 获取当前操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>当前操作状态</returns>
        ActionStatus GetActionStatus(BaseEntity entity);
        
        /// <summary>
        /// 设置操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        Task<bool> SetActionStatusAsync(BaseEntity entity, ActionStatus status, string reason = null);
        
        /// <summary>
        /// 验证数据性状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        Task<StateTransitionResult> ValidateDataStatusTransitionAsync(BaseEntity entity, DataStatus targetStatus);
        
        /// <summary>
        /// 验证业务性状态转换
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync<T>(BaseEntity entity, T targetStatus) where T : Enum;
        
        /// <summary>
        /// 验证业务性状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync(BaseEntity entity, Type statusType, object targetStatus);
        
        /// <summary>
        /// 验证操作状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        Task<StateTransitionResult> ValidateActionStatusTransitionAsync(BaseEntity entity, ActionStatus targetStatus);
        
        /// <summary>
        /// 获取可转换的数据性状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        IEnumerable<DataStatus> GetAvailableDataStatusTransitions(BaseEntity entity);
        
        /// <summary>
        /// 获取可转换的业务性状态列表
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        IEnumerable<T> GetAvailableBusinessStatusTransitions<T>(BaseEntity entity) where T : Enum;
        
        /// <summary>
        /// 获取可转换的业务性状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>可转换的状态列表</returns>
        IEnumerable<object> GetAvailableBusinessStatusTransitions(BaseEntity entity, Type statusType);
        
        /// <summary>
        /// 获取可转换的操作状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        IEnumerable<ActionStatus> GetAvailableActionStatusTransitions(BaseEntity entity);
        
        /// <summary>
        /// 检查是否可以转换到目标数据性状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        Task<bool> CanTransitionToDataStatus(BaseEntity entity, DataStatus targetStatus);
        
        /// <summary>
        /// 异步检查是否可以转换到目标数据性状态，并输出错误信息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>包含转换结果和错误消息的元组</returns>
        Task<(bool CanTransition, string ErrorMessage)> CanTransitionToDataStatusAsync(BaseEntity entity, DataStatus targetStatus);
        
        /// <summary>
        /// 检查是否可以转换到目标业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        Task<bool> CanTransitionToBusinessStatus<T>(BaseEntity entity, T targetStatus) where T : Enum;
        
        /// <summary>
        /// 检查是否可以转换到目标操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        Task<bool> CanTransitionToActionStatus(BaseEntity entity, ActionStatus targetStatus);
        
        /// <summary>
        /// 请求状态转换
        /// </summary>
        /// <param name="context">状态转换上下文</param>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <param name="userId">操作用户ID</param>
        /// <returns>状态转换结果</returns>
        StateTransitionResult RequestTransition(IStatusTransitionContext context, object targetStatus, string reason = null, long userId = 0);
        
        /// <summary>
        /// 批量设置实体状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="dataStatus">数据状态</param>
        /// <param name="businessStatus">业务状态（可选）</param>
        /// <param name="actionStatus">操作状态（可选）</param>
        /// <returns>设置是否成功</returns>
        Task<bool> SetStatesAsync(object entity, DataStatus dataStatus, Enum businessStatus = null, ActionStatus actionStatus = ActionStatus.无操作);
        
        /// <summary>
        /// 检查实体是否可以更改为目标状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="errorMessage">错误信息输出参数</param>
        /// <returns>是否可以更改</returns>
        bool CanChangeStatus(object entity, Enum targetStatus, out string errorMessage);
        
        /// <summary>
        /// 检查实体是否处于指定状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statuses">要检查的状态列表</param>
        /// <returns>是否处于任一指定状态</returns>
        bool IsInStatus(object entity, params Enum[] statuses);
        
        /// <summary>
        /// 获取状态转换失败的详细错误信息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>错误信息</returns>
        string GetTransitionErrorMessage(object entity, Enum targetStatus);
        
        /// <summary>
        /// 状态变更事件
        /// </summary>
        event EventHandler<StateTransitionEventArgs> StatusChanged;

        /// <summary>
        /// 清理缓存
        /// </summary>
        void ClearCache();
    }
}