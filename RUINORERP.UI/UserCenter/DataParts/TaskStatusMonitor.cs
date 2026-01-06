using RUINORERP.Global;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.UI.UserCenter.DataParts
{
    /// <summary>
    /// 任务状态监控器 - 状态变更处理器
    /// 单一职责：作为网络任务状态更新的转发器
    /// </summary>
    public class TodoMonitor : IExcludeFromRegistration
    {
        #region 单例模式
        private static readonly Lazy<TodoMonitor> _instance = 
            new Lazy<TodoMonitor>(() => new TodoMonitor());

        /// <summary>
        /// 获取TodoMonitor的单例实例
        /// </summary>
        public static TodoMonitor Instance => _instance.Value;

        private TodoMonitor()
        {
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 注册要监控的业务类型
        /// 注：现在使用基于网络通知的实时同步，此方法仅用于保持API兼容性
        /// </summary>
        /// <param name="businessTypes">要监控的业务类型列表</param>
        public void StartMonitoring(List<BizType> businessTypes)
        {
            // 空实现，保持API兼容性
            // 现在使用基于网络通知的实时同步，不再需要注册监控业务类型
        }

        /// <summary>
        /// 停止监控指定的业务类型
        /// 注：现在使用基于网络通知的实时同步，此方法仅用于保持API兼容性
        /// </summary>
        /// <param name="businessTypes">要停止监控的业务类型列表</param>
        public void StopMonitoring(List<BizType> businessTypes)
        {
            // 空实现，保持API兼容性
        }

        /// <summary>
        /// 处理来自网络的任务状态更新
        /// </summary>
        /// <param name="update">网络任务状态更新信息</param>
        public void HandleNetworkTodoUpdate(TodoUpdate update)
        {
            if (update == null)
                return;
            // 直接转发给TodoSyncManager处理
            TodoSyncManager.Instance.PublishUpdate(update);
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 清理不再使用的监控资源
        /// 注：现在使用基于网络通知的实时同步，此方法仅用于保持API兼容性
        /// </summary>
        public void Cleanup()
        {
            // 空实现，保持API兼容性
        }
        #endregion
    }
}