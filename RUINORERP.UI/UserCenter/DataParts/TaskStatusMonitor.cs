using RUINORERP.Global;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.UI.UserCenter.DataParts
{
    /// <summary>
    /// 任务状态监控器 - 状态变更处理器
    /// 单一职责：处理本地和网络任务状态更新，维护状态历史记录
    /// 作为状态变更的协调组件，确保状态更新能正确地分发给订阅者
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
            _monitoredBusinessTypes = new List<BizType>();
        }
        #endregion

        #region 字段
        private readonly List<BizType> _monitoredBusinessTypes; // 监控的业务类型列表
        private bool _isMonitoring = false;
        private readonly object _monitorLock = new object();
        #endregion

        #region 公共方法

        /// <summary>
        /// 注册要监控的业务类型
        /// 注：现在使用基于网络通知的实时同步，不再需要轮询数据库
        /// </summary>
        /// <param name="businessTypes">要监控的业务类型列表</param>
        public void StartMonitoring(List<BizType> businessTypes)
        {
            if (businessTypes == null || !businessTypes.Any())
                return;

            lock (_monitorLock)
            {
                // 添加新的监控业务类型
                foreach (var bizType in businessTypes)
                {
                    if (!_monitoredBusinessTypes.Contains(bizType))
                    {
                        _monitoredBusinessTypes.Add(bizType);
                    }
                }

                // 标记为监控中状态，但不再启动轮询任务
                _isMonitoring = true;
            }
        }

        /// <summary>
        /// 停止监控指定的业务类型
        /// </summary>
        /// <param name="businessTypes">要停止监控的业务类型列表</param>
        public void StopMonitoring(List<BizType> businessTypes)
        {
            if (businessTypes == null || !businessTypes.Any())
                return;

            lock (_monitorLock)
            {
                // 移除指定的业务类型
                foreach (var bizType in businessTypes)
                {
                    _monitoredBusinessTypes.Remove(bizType);
                }

                // 如果没有监控的业务类型了，停止监控
                if (!_monitoredBusinessTypes.Any())
                {
                    _isMonitoring = false;
                }
            }
        }



        /// <summary>
        /// 处理来自网络的任务状态更新
        /// </summary>
        /// <param name="update">网络任务状态更新信息</param>
        public void HandleNetworkTodoUpdate(TodoUpdate update)
        {
            if (update == null)
                return;
            // 发布更新到本地订阅者
            TodoSyncManager.Instance.PublishUpdate(update);

        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 清理不再使用的监控资源
        /// </summary>
        public void Cleanup()
        {
            lock (_monitorLock)
            {
                _isMonitoring = false;
                _monitoredBusinessTypes.Clear();
            }
        }
        #endregion
    }
}