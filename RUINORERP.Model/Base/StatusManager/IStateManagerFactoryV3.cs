/**
 * 文件: IStateManagerFactoryV3.cs
 * 说明: 状态管理器工厂V3接口 - 定义状态管理器工厂的接口
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态管理器工厂V3接口 - 定义状态管理器工厂的接口
    /// </summary>
    public interface IStateManagerFactoryV3
    {
        #region 方法

        /// <summary>
        /// 获取统一状态管理器
        /// </summary>
        /// <returns>统一状态管理器实例</returns>
        IUnifiedStateManager GetStateManager();

        /// <summary>
        /// 获取状态转换引擎
        /// </summary>
        /// <returns>状态转换引擎实例</returns>
        IStatusTransitionEngine GetTransitionEngine();

        /// <summary>
        /// 创建状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>状态转换上下文</returns>
        IStatusTransitionContext CreateTransitionContext(object entity, Type statusType);

        /// <summary>
        /// 创建状态转换上下文
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>状态转换上下文</returns>
        IStatusTransitionContext CreateTransitionContext<T>(object entity) where T : Enum;

        /// <summary>
        /// 释放资源
        /// </summary>
        void Dispose();

        #endregion
    }
}