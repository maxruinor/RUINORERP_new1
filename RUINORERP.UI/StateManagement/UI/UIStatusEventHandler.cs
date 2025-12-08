/**
 * 文件: UIStatusEventHandler.cs
 * 说明: UI状态事件处理器 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.StateManagement.UI
{
    /// <summary>
    /// UI状态事件处理器 - v3版本
    /// 负责处理UI状态变更事件，并同步更新UI控件状态
    /// </summary>
    public class UIStatusEventHandler
    {
        #region 字段

        /// <summary>
        /// UI状态控制器
        /// </summary>
        private readonly IStatusUIController _uiController;

        /// <summary>
        /// 状态管理器
        /// </summary>
        private readonly IUnifiedStateManager _stateManager;

        /// <summary>
        /// 事件订阅列表
        /// </summary>
        private readonly Dictionary<object, List<Delegate>> _eventSubscriptions;

        /// <summary>
        /// 事件处理锁对象
        /// </summary>
        private readonly object _eventProcessingLock = new object();

        /// <summary>
        /// 事件处理状态标志
        /// </summary>
        private bool _isProcessingEvent = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化UI状态事件处理器
        /// </summary>
        /// <param name="uiController">UI状态控制器</param>
        /// <param name="stateManager">状态管理器</param>
        public UIStatusEventHandler(IStatusUIController uiController, IUnifiedStateManager stateManager)
        {
            _uiController = uiController ?? throw new ArgumentNullException(nameof(uiController));
            _stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));
            _eventSubscriptions = new Dictionary<object, List<Delegate>>();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 订阅状态变更事件
        /// </summary>
        /// <param name="eventSource">事件源对象</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件处理程序</param>
        public void SubscribeToStatusChange(object eventSource, string eventName, EventHandler<StateTransitionEventArgs> handler)
        {
            if (eventSource == null || string.IsNullOrEmpty(eventName) || handler == null)
                return;

            try
            {
                // 获取事件信息
                var eventInfo = eventSource.GetType().GetEvent(eventName);
                if (eventInfo == null)
                {
                    System.Diagnostics.Debug.WriteLine($"事件 {eventName} 在类型 {eventSource.GetType().Name} 中不存在");
                    return;
                }

                // 创建事件处理委托
                var delegateType = eventInfo.EventHandlerType;
                var eventDelegate = Delegate.CreateDelegate(delegateType, handler.Target, handler.Method);

                // 订阅事件
                eventInfo.AddEventHandler(eventSource, eventDelegate);

                // 记录订阅信息
                if (!_eventSubscriptions.ContainsKey(eventSource))
                {
                    _eventSubscriptions[eventSource] = new List<Delegate>();
                }
                _eventSubscriptions[eventSource].Add(eventDelegate);

                System.Diagnostics.Debug.WriteLine($"成功订阅事件: {eventName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"订阅事件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 取消订阅状态变更事件
        /// </summary>
        /// <param name="eventSource">事件源对象</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="handler">事件处理程序</param>
        public void UnsubscribeFromStatusChange(object eventSource, string eventName, EventHandler<StateTransitionEventArgs> handler)
        {
            if (eventSource == null || string.IsNullOrEmpty(eventName) || handler == null)
                return;

            try
            {
                // 获取事件信息
                var eventInfo = eventSource.GetType().GetEvent(eventName);
                if (eventInfo == null)
                    return;

                // 创建事件处理委托
                var delegateType = eventInfo.EventHandlerType;
                var eventDelegate = Delegate.CreateDelegate(delegateType, handler.Target, handler.Method);

                // 取消订阅事件
                eventInfo.RemoveEventHandler(eventSource, eventDelegate);

                // 移除订阅记录
                if (_eventSubscriptions.ContainsKey(eventSource))
                {
                    _eventSubscriptions[eventSource].Remove(eventDelegate);
                    if (_eventSubscriptions[eventSource].Count == 0)
                    {
                        _eventSubscriptions.Remove(eventSource);
                    }
                }

                System.Diagnostics.Debug.WriteLine($"成功取消订阅事件: {eventName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"取消订阅事件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理状态变更事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        public async Task HandleStatusChangeAsync(object sender, StateTransitionEventArgs e)
        {
            if (e == null)
                return;

            // 防循环调用机制：检查当前是否正在处理事件
            if (!TryAcquireEventProcessingLock())
            {
                System.Diagnostics.Debug.WriteLine("检测到可能的循环调用，跳过当前事件处理");
                return;
            }

            try
            {
                // 获取相关控件
                var controls = GetRelatedControls(sender);
                if (controls == null || !controls.Any())
                    return;

                // 根据状态类型更新UI
                switch (e.StatusType.Name)
                {
                    case "DataStatus":
                        await UpdateUIForDataStatusAsync(controls, (DataStatus)e.NewStatus);
                        break;
                    case "BusinessStatus":
                        await UpdateUIForBusinessStatusAsync(controls, (Enum)e.NewStatus);
                        break;
                    case "ActionStatus":
                        await UpdateUIForActionStatusAsync(controls, (ActionStatus)e.NewStatus);
                        break;
                    default:
                        // 尝试将事件参数转换为状态上下文
                        if (e is IStatusTransitionContext statusContext)
                        {
                            await UpdateUIForStatusContextAsync(controls, statusContext);
                        }
                        else
                        {
                            await UpdateUIForDefaultStatusAsync(controls, (Enum)e.NewStatus);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"处理状态变更事件失败: {ex.Message}");
            }
            finally
            {
                // 确保释放锁
                ReleaseEventProcessingLock();
            }
        }

        /// <summary>
        /// 订阅所有状态变更事件
        /// </summary>
        /// <param name="stateManager">状态管理器</param>
        public void SubscribeToAllEvents(IUnifiedStateManager stateManager)
        {
            if (stateManager == null)
                return;

            try
            {
                // 直接订阅状态变更事件
                stateManager.StatusChanged += async (sender, e) => await HandleStatusChangeAsync(sender, e);
                System.Diagnostics.Debug.WriteLine("成功订阅状态管理器的状态变更事件");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"订阅状态管理器事件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 清理所有事件订阅
        /// </summary>
        public void Cleanup()
        {
            foreach (var subscription in _eventSubscriptions)
            {
                var eventSource = subscription.Key;
                var eventDelegates = subscription.Value;

                foreach (var eventDelegate in eventDelegates)
                {
                    try
                    {
                        // 获取事件信息并取消订阅
                        var events = eventSource.GetType().GetEvents()
                            .Where(e => e.EventHandlerType == eventDelegate.GetType());

                        foreach (var eventInfo in events)
                        {
                            eventInfo.RemoveEventHandler(eventSource, eventDelegate);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"清理事件订阅失败: {ex.Message}");
                    }
                }
            }

            _eventSubscriptions.Clear();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 尝试获取事件处理锁，防止循环调用
        /// </summary>
        /// <returns>如果成功获取锁返回true，否则返回false</returns>
        private bool TryAcquireEventProcessingLock()
        {
            lock (_eventProcessingLock)
            {
                if (_isProcessingEvent)
                {
                    return false;
                }
                _isProcessingEvent = true;
                return true;
            }
        }

        /// <summary>
        /// 释放事件处理锁
        /// </summary>
        private void ReleaseEventProcessingLock()
        {
            lock (_eventProcessingLock)
            {
                _isProcessingEvent = false;
            }
        }

        /// <summary>
        /// 获取相关控件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <returns>相关控件集合</returns>
        private IEnumerable<Control> GetRelatedControls(object sender)
        {
            var controls = new List<Control>();

            try
            {
                // 如果发送者是控件，直接添加
                if (sender is Control control)
                {
                    controls.Add(control);
                }

                // 如果发送者是窗体，添加窗体上的所有控件
                if (sender is Form form)
                {
                    controls.AddRange(GetAllControls(form));
                }

                // 如果发送者是状态感知控件，获取其管理的控件
                if (sender is BaseBillEdit stateAwareControl)
                {
                    controls.AddRange(GetAllControls(stateAwareControl));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取相关控件失败: {ex.Message}");
            }

            return controls;
        }

        /// <summary>
        /// 获取容器中的所有控件
        /// </summary>
        /// <param name="container">控件容器</param>
        /// <returns>所有控件集合</returns>
        private IEnumerable<Control> GetAllControls(Control container)
        {
            var controls = new List<Control>();
            
            if (container == null)
                return controls;

            // 添加容器本身
            controls.Add(container);

            // 递归添加子控件
            foreach (Control control in container.Controls)
            {
                controls.AddRange(GetAllControls(control));
            }

            return controls;
        }

        /// <summary>
        /// 更新数据状态UI
        /// </summary>
        /// <param name="controls">控件集合</param>
        /// <param name="status">状态值</param>
        /// <returns>异步任务</returns>
        private async Task UpdateUIForDataStatusAsync(IEnumerable<Control> controls, DataStatus status)
        {
            await Task.Run(() =>
            {
                // 使用工厂方法创建状态上下文
                var statusContext = StatusTransitionContextFactory.CreateUIUpdateContext(
                    typeof(DataStatus),
                    status,
                    "UI状态更新"
                );
                
                _uiController.UpdateUIStatus(statusContext, controls);
            });
        }

        /// <summary>
        /// 更新业务状态UI
        /// </summary>
        /// <param name="controls">控件集合</param>
        /// <param name="status">状态值</param>
        /// <returns>异步任务</returns>
        private async Task UpdateUIForBusinessStatusAsync(IEnumerable<Control> controls, Enum status)
        {
            await Task.Run(() =>
            {
                // 使用工厂方法创建状态上下文
                var statusContext = StatusTransitionContextFactory.CreateUIUpdateContext(
                    status.GetType(),
                    status,
                    "UI状态更新"
                );
                
                _uiController.UpdateUIStatus(statusContext, controls);
            });
        }

        /// <summary>
        /// 更新操作状态UI
        /// </summary>
        /// <param name="controls">控件集合</param>
        /// <param name="status">状态值</param>
        /// <returns>异步任务</returns>
        private async Task UpdateUIForActionStatusAsync(IEnumerable<Control> controls, ActionStatus status)
        {
            await Task.Run(() =>
            {
                // 使用工厂方法创建状态上下文
                var statusContext = StatusTransitionContextFactory.CreateUIUpdateContext(
                    typeof(ActionStatus),
                    status,
                    "UI状态更新"
                );
                
                _uiController.UpdateUIStatus(statusContext, controls);
            });
        }

        /// <summary>
        /// 更新默认状态UI
        /// </summary>
        /// <param name="controls">控件集合</param>
        /// <param name="status">状态值</param>
        /// <returns>异步任务</returns>
        private async Task UpdateUIForDefaultStatusAsync(IEnumerable<Control> controls, object status)
        {
            await Task.Run(() =>
            {
                if (status == null)
                    return;

                // 使用工厂方法创建状态上下文
                var statusContext = StatusTransitionContextFactory.CreateUIUpdateContext(
                    status.GetType(),
                    status,
                    "UI状态更新"
                );
                
                _uiController.UpdateUIStatus(statusContext, controls);
            });
        }

        /// <summary>
        /// 更新状态UI（重载方法，支持IStatusTransitionContext）
        /// </summary>
        /// <param name="controls">控件集合</param>
        /// <param name="statusContext">状态上下文</param>
        /// <returns>异步任务</returns>
        private async Task UpdateUIForStatusContextAsync(IEnumerable<Control> controls, IStatusTransitionContext statusContext)
        {
            await Task.Run(() =>
            {
                _uiController.UpdateUIStatus(statusContext, controls);
            });
        }

        #endregion
    }

    /// <summary>
    /// UI状态事件处理器扩展方法
    /// </summary>
    public static class UIStatusEventHandlerExtensions
    {
        /// <summary>
        /// 为状态感知控件订阅状态变更事件
        /// </summary>
        /// <param name="eventHandler">事件处理器</param>
        /// <param name="stateAwareControl">状态感知控件</param>
        public static void SubscribeToStateAwareControl(this UIStatusEventHandler eventHandler, BaseBillEdit stateAwareControl)
        {
            if (eventHandler == null || stateAwareControl == null)
                return;

            // 订阅状态上下文变更事件
            eventHandler.SubscribeToStatusChange(
                stateAwareControl,
                nameof(stateAwareControl),
                async (sender, e) => await eventHandler.HandleStatusChangeAsync(sender, e));
        }

        /// <summary>
        /// 为状态管理器订阅状态变更事件
        /// </summary>
        /// <param name="eventHandler">事件处理器</param>
        /// <param name="stateManager">状态管理器</param>
        public static void SubscribeToStateManager(this UIStatusEventHandler eventHandler, IUnifiedStateManager stateManager)
        {
            if (eventHandler == null || stateManager == null)
                return;

            // 订阅状态变更事件
            eventHandler.SubscribeToStatusChange(
                stateManager,
                nameof(stateManager.StatusChanged),
                async (sender, e) => await eventHandler.HandleStatusChangeAsync(sender, e));
        }

        /// <summary>
        /// 为状态感知控件订阅状态管理器事件
        /// </summary>
        /// <param name="control">状态感知控件</param>
        /// <param name="stateManager">状态管理器</param>
        /// <returns>事件处理器</returns>
        public static UIStatusEventHandler SubscribeToStatusEvents(
            this Control control,
            IUnifiedStateManager stateManager)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            
            if (stateManager == null)
                throw new ArgumentNullException(nameof(stateManager));

            // 从服务容器获取UI控制器（现在服务已注册）
            var uiController = Startup.GetFromFac<IStatusUIController>();
            if (uiController == null)
            {
                throw new InvalidOperationException("无法从服务容器获取IStatusUIController，请确保状态管理服务已正确注册");
            }

            var handler = new UIStatusEventHandler(uiController, stateManager);
            handler.SubscribeToAllEvents(stateManager);
            return handler;
        }
    }
}