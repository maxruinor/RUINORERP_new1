using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.ClientCommandHandlers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 锁管理命令处理器 v2.0.0
    /// 负责处理与分布式锁相关的命令，包括锁请求、释放、状态查询等
    /// 
    /// 更新说明：
    /// - v2.0.0: 集成缓存状态同步，配合新的IntegratedLockManagementService
    /// - 支持智能缓存失效和批量状态更新
    /// - 增强异常处理和重连机制
    /// - v2.1.0: 添加锁状态通知服务，支持实时UI更新
    /// </summary>
    [ClientCommandHandler("LockCommandHandler", 60)]
    public class LockCommandHandler : BaseClientCommandHandler
    {
        private readonly ILogger<LockCommandHandler> _logger;

        /// <summary>
        /// 缓存服务引用 - v2.0.0新增
        /// 用于在接收到服务器推送时更新本地缓存
        /// </summary>
        private ClientLocalLockCacheService _lockCache;

        /// <summary>
        /// 锁状态通知服务 - v2.1.0新增
        /// 用于通知UI窗体锁状态变化
        /// </summary>
        private LockStatusNotificationService _notificationService;

        /// <summary>
        /// 构造函数 v2.0.0
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="lockCache">客户端缓存服务（可选）</param>
        /// <param name="notificationService">锁状态通知服务（可选）</param>
        public LockCommandHandler(
            ILogger<LockCommandHandler> logger, 
            ClientLocalLockCacheService lockCache = null,
            LockStatusNotificationService notificationService = null)
            : base(logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lockCache = lockCache; // v2.0.0: 可选的缓存服务引用
            _notificationService = notificationService; // v2.1.0: 可选的通知服务引用

            // 注册支持的锁管理相关命令（使用已定义的 LockCommands）
            SetSupportedCommands(
                LockCommands.Lock,
                LockCommands.Unlock,
                LockCommands.CheckLockStatus,
                LockCommands.ForceUnlock,
                LockCommands.RequestUnlock,
                LockCommands.RefuseUnlock,
                LockCommands.AgreeUnlock,
                LockCommands.BroadcastLockStatus
            );
        }

        /// <summary>
        /// 初始化处理器
        /// </summary>
        /// <returns>初始化是否成功</returns>
        public override async Task<bool> InitializeAsync()
        {
            bool initialized = await base.InitializeAsync();
            if (initialized)
            {
                _logger.LogDebug("锁管理命令处理器初始化成功");
            }
            return initialized;
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        public override async Task HandleAsync(PacketModel packet)
        {
            if (packet == null || packet.CommandId == null)
            {
                _logger.LogError("收到无效的数据包");
                return;
            }

            try
            {
                _logger.LogDebug($"收到锁管理命令: {packet.CommandId.FullCode}");

                // 根据命令ID处理不同的锁管理命令
                if (packet.CommandId == LockCommands.Lock)
                {
                    await HandleLockRequestAsync(packet);
                }
                else if (packet.CommandId == LockCommands.Unlock)
                {
                    await HandleLockReleaseAsync(packet);
                }
                else if (packet.CommandId == LockCommands.CheckLockStatus)
                {
                    await HandleLockStatusAsync(packet);
                }
                else if (packet.CommandId == LockCommands.ForceUnlock)
                {
                    await HandleForceUnlockAsync(packet);
                }
                else if (packet.CommandId == LockCommands.BroadcastLockStatus)
                {
                    await HandleLockBroadcastAsync(packet);
                }
                else if (packet.CommandId == LockCommands.RequestUnlock)
                {
                    await HandleRequestUnlockAsync(packet);
                }
                else if (packet.CommandId == LockCommands.RefuseUnlock)
                {
                    await HandleRefuseUnlockAsync(packet);
                }
                else if (packet.CommandId == LockCommands.AgreeUnlock)
                {
                    await HandleAgreeUnlockAsync(packet);
                }
                else
                {
                    _logger.LogWarning($"未处理的锁管理命令ID: {packet.CommandId.FullCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理锁管理命令 {packet.CommandId.ToString()} 时发生错误");
            }
        }

        /// <summary>
        /// 处理锁请求命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleLockRequestAsync(PacketModel packet)
        {
            try
            {
                if (packet.Response is LockResponse lockResponse)
                {
                    _logger.LogDebug($"收到锁请求响应: 资源ID={lockResponse.LockInfo.BillID}, 成功={lockResponse.IsSuccess}");

                    // 处理锁请求响应，通知相关UI组件
                    if (lockResponse.IsSuccess)
                    {
                        // 锁获取成功，可以更新UI状态
                        // 更新本地缓存，确保缓存与服务器状态一致
                        if (_lockCache != null && lockResponse.LockInfo != null)
                        {
                            _lockCache.UpdateCacheItem(lockResponse.LockInfo);
                            _logger.LogDebug("锁获取成功，更新本地缓存: 资源ID={BillId}", lockResponse.LockInfo.BillID);
                        }
                        // 触发锁获取成功事件或更新UI组件状态
                        // 例如：通知正在编辑的表单可以开始编辑
                    }
                    else
                    {
                        // 锁获取失败
                        _logger.LogWarning($"获取资源 '{lockResponse.LockInfo.BillID}' 的锁失败: {lockResponse.Message}");

                        // 如果有当前锁定信息，可以显示给用户
                        if (lockResponse.LockInfo != null)
                        {
                            string message = $"资源已被锁定\n" +
                                            $"锁定用户: {lockResponse.LockInfo.LockedUserName}\n" +
                                            $"锁定时间: {lockResponse.LockInfo.LockTime}\n" +
                                            $"客户端ID: {lockResponse.LockInfo.SessionId}";
                            // 在UI线程显示提示
                            InvokeOnUiThread(() =>
                            {
                                MessageBox.Show(message, "锁定提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            });
                        }
                        else
                        {
                            // 显示通用的锁定失败消息
                            InvokeOnUiThread(() =>
                            {
                                MessageBox.Show($"锁定失败：{lockResponse.Message}", "锁定失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            });
                        }
                    }
                }
                // 新增处理锁定请求冲突的逻辑
                else if (packet.Request is LockRequest lockRequest)
                {
                    _logger.LogDebug($"收到锁定请求: BillID={lockRequest.LockInfo.BillID}, UserID={lockRequest.LockInfo.LockedUserId}");

                    // 在UI线程显示锁定请求确认对话框
                    InvokeOnUiThread(() =>
                    {
                        DialogResult result = MessageBox.Show(
                            $"用户 {lockRequest.LockInfo.LockedUserName} 请求锁定您当前正在编辑的单据 {lockRequest.LockInfo.BillID}。\n\n是否允许锁定？",
                            "锁定请求",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        // 根据用户选择处理
                        if (result == DialogResult.Yes)
                        {
                            _logger.LogDebug("用户允许其他用户锁定单据: BillID={BillID}", lockRequest.LockInfo.BillID);
                            // 实现解锁当前单据的逻辑并更新本地缓存
                            if (_lockCache != null && lockRequest.LockInfo != null)
                            {
                                // 清除当前单据的锁定缓存，确保本地状态与服务器状态一致
                                _lockCache.ClearCache(lockRequest.LockInfo.BillID);
                                _logger.LogDebug("用户允许锁定，清除本地缓存: BillID={BillId}", lockRequest.LockInfo.BillID);
                            }
                            // 可以触发其他解锁操作，如通知编辑中的表单关闭或只读模式
                        }
                        else
                        {
                            _logger.LogDebug("用户拒绝其他用户锁定单据: BillID={BillID}", lockRequest.LockInfo.BillID);
                        }
                    });
                }
                else
                {
                    _logger.LogWarning("锁请求响应类型无效");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理锁请求命令时发生错误");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 处理锁释放命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleLockReleaseAsync(PacketModel packet)
        {
            try
            {
                if (packet.Response is LockResponse lockResponse)
                {
                    _logger.LogDebug($"收到锁释放响应: 资源ID={lockResponse.LockInfo.BillID}, 成功={lockResponse.IsSuccess}");

                    if (lockResponse.IsSuccess)
                    {
                        _logger.LogDebug($"成功释放资源 '{lockResponse.LockInfo.BillID}' 的锁");
                        // 更新本地缓存，清除锁定信息
                        if (_lockCache != null && lockResponse.LockInfo != null)
                        {
                            _lockCache.ClearCache(lockResponse.LockInfo.BillID);
                            _logger.LogDebug("锁释放成功，清除本地缓存: 资源ID={BillId}", lockResponse.LockInfo.BillID);
                        }
                        // 触发锁释放成功事件或更新UI组件状态
                    }
                    else
                    {
                        _logger.LogWarning($"释放资源 '{lockResponse.LockInfo.BillID}' 的锁失败: {lockResponse.Message}");
                        // 触发锁释放失败事件
                    }
                }
                else
                {
                    _logger.LogWarning("锁释放响应类型无效");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理锁释放响应时发生错误");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 处理锁状态查询命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleLockStatusAsync(PacketModel packet)
        {
            try
            {
                if (packet.Response is LockResponse lockResponse)
                {
                    _logger.LogDebug($"收到锁状态查询响应: 资源ID={lockResponse.LockInfo.BillID}, 状态={lockResponse.IsSuccess}");

                    if (lockResponse.IsSuccess)
                    {
                        _logger.LogDebug($"资源 '{lockResponse.LockInfo.BillID}' 的锁状态查询成功");

                        if (lockResponse.LockInfo != null)
                        {
                            _logger.LogDebug($"锁信息: 用户={lockResponse.LockInfo.LockedUserName}, 时间={lockResponse.LockInfo.LockTime}");
                            // 更新本地缓存，确保缓存与服务器状态一致
                            if (_lockCache != null)
                            {
                                _lockCache.UpdateCacheItem(lockResponse.LockInfo);
                                _logger.LogDebug("锁状态查询成功，更新本地缓存: 资源ID={BillId}", lockResponse.LockInfo.BillID);
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"资源 '{lockResponse.LockInfo.BillID}' 的锁状态查询失败: {lockResponse.Message}");
                    }
                }
                else
                {
                    _logger.LogWarning("锁状态查询响应类型无效");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理锁状态查询响应时发生错误");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 处理强制解锁命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleForceUnlockAsync(PacketModel packet)
        {
            try
            {
                if (packet.Response is LockResponse lockResponse)
                {
                    _logger.LogDebug($"收到强制解锁响应: 资源ID={lockResponse.LockInfo.BillID}, 成功={lockResponse.IsSuccess}");

                    if (lockResponse.IsSuccess)
                    {
                        _logger.LogDebug($"成功强制释放资源 '{lockResponse.LockInfo.BillID}' 的锁");
                        // 更新本地缓存，清除锁定信息
                        if (_lockCache != null && lockResponse.LockInfo != null)
                        {
                            _lockCache.ClearCache(lockResponse.LockInfo.BillID);
                            _logger.LogDebug("强制解锁成功，清除本地缓存: 资源ID={BillId}", lockResponse.LockInfo.BillID);
                        }
                        // 触发强制解锁成功事件或更新UI组件状态
                    }
                    else
                    {
                        _logger.LogWarning($"强制释放资源 '{lockResponse.LockInfo.BillID}' 的锁失败: {lockResponse.Message}");
                        // 触发强制解锁失败事件
                    }
                }
                else
                {
                    _logger.LogWarning("强制解锁响应类型无效");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理强制解锁响应时发生错误");
            }
            await Task.CompletedTask;
        }


        /// <summary>
        /// 处理锁广播命令 v2.0.0
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleLockBroadcastAsync(PacketModel packet)
        {
            try
            {
                // 解析广播消息，通知相关组件更新锁状态
                if (packet.Request is LockRequest lockRequest)
                {

                    if (_lockCache != null && lockRequest.LockedDocuments != null)
                    {
                        for (int i = 0; i < lockRequest.LockedDocuments.Count; i++)
                        {
                            var lockInfo = lockRequest.LockedDocuments[i];

                            if (lockInfo.IsLocked)
                            {
                                // 锁定广播：更新缓存中的锁定信息
                                _lockCache.UpdateCacheItem(lockInfo);
                                
                                // 通知订阅者锁状态变化
                                _notificationService?.NotifyLockStatusChanged(
                                    lockInfo.BillID, 
                                    lockInfo, 
                                    LockStatusChangeType.Locked);
                            }
                            else
                            {
                                // 解锁广播：清除缓存中的锁定信息
                                _lockCache.ClearCache(lockInfo.BillID);
                                
                                // 通知订阅者锁状态变化
                                _notificationService?.NotifyLockStatusChanged(
                                    lockInfo.BillID, 
                                    lockInfo, 
                                    LockStatusChangeType.Unlocked);
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理锁广播命令时发生错误");
            }
            await Task.CompletedTask;
        }

      
        /// <summary>
        /// 处理解锁请求命令
        /// 当其他用户请求解锁当前用户锁定的资源时
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleRequestUnlockAsync(PacketModel packet)
        {
            try
            {
                if (packet.Request is LockRequest unlockRequest)
                {
                    _logger.LogDebug($"收到解锁请求: 单据ID={unlockRequest.LockInfo?.BillID ?? 0}, 请求用户={unlockRequest.RequesterUserName}");

                    // 在UI线程显示确认对话框
                    InvokeOnUiThread(async () =>
                    {
                        try
                        {
                            DialogResult result = MessageBox.Show(
                                $"用户 {unlockRequest.RequesterUserName} 请求解锁您锁定的单据 {unlockRequest.LockInfo?.BillID ?? 0}，是否同意解锁？",
                                "解锁请求",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            // 获取ClientLockManagementService实例
                            var lockManagementService = Startup.GetFromFac<ClientLockManagementService>();

                            if (result == DialogResult.Yes)
                            {
                                // 用户同意解锁，调用AgreeUnlockAsync方法
                                _logger.LogDebug($"用户同意解锁单据: {unlockRequest.LockInfo?.BillID ?? 0}");
                                await lockManagementService.AgreeUnlockAsync(
                                    unlockRequest.LockInfo?.BillID ?? 0,
                                    unlockRequest.LockInfo?.MenuID ?? 0,
                                    unlockRequest.RequesterUserId,
                                    unlockRequest.RequesterUserName ?? string.Empty);
                            }
                            else
                            {
                                // 用户拒绝解锁，调用RefuseUnlockAsync方法
                                _logger.LogDebug($"用户拒绝解锁单据: {unlockRequest.LockInfo?.BillID ?? 0}");
                                await lockManagementService.RefuseUnlockAsync(
                                    unlockRequest.LockInfo?.BillID ?? 0,
                                    unlockRequest.LockInfo?.MenuID ?? 0,
                                    unlockRequest.RequesterUserId,
                                    unlockRequest.RequesterUserName ?? string.Empty);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "处理解锁请求响应时发生错误");
                            MessageBox.Show($"处理解锁请求时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理解锁请求命令时发生错误");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 处理拒绝解锁命令
        /// 当其他用户拒绝当前用户的解锁请求时
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleRefuseUnlockAsync(PacketModel packet)
        {
            try
            {
                if (packet.Request is LockRequest refuseInfo)
                {
                    _logger.LogDebug($"收到拒绝解锁: 单据ID={refuseInfo.LockInfo?.BillID ?? 0}, 拒绝用户={refuseInfo.RequesterUserName}");

                    // 在UI线程显示提示
                    InvokeOnUiThread(() =>
                    {
                        MessageBox.Show(
                            $"用户 {refuseInfo.RequesterUserName} 拒绝了您的解锁请求",
                            "解锁请求被拒绝",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理拒绝解锁命令时发生错误");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 处理同意解锁命令
        /// 当其他用户同意当前用户的解锁请求时
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleAgreeUnlockAsync(PacketModel packet)
        {
            try
            {
                if (packet.Request is LockRequest agreeInfo)
                {
                    _logger.LogDebug($"收到同意解锁: 单据ID={agreeInfo.LockInfo?.BillID ?? 0}, 同意用户={agreeInfo.RequesterUserName}");

                    // 在UI线程显示提示
                    InvokeOnUiThread(() =>
                    {
                        MessageBox.Show(
                            $"用户 {agreeInfo.RequesterUserName} 同意了您的解锁请求，您现在可以编辑此单据",
                            "解锁请求已同意",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理同意解锁命令时发生错误");
            }
            await Task.CompletedTask;
        }

        // Helper to marshal actions to WinForms UI thread
        private void InvokeOnUiThread(Action action)
        {
            try
            {
                if (Application.OpenForms != null && Application.OpenForms.Count > 0)
                {
                    var form = Application.OpenForms[0];
                    if (form != null && form.InvokeRequired)
                    {
                        form.Invoke((MethodInvoker)delegate { action(); });
                        return;
                    }
                }

                // If no open forms or invoke not required, execute directly
                action();
            }
            catch
            {
                // Swallow exceptions from UI invoke to avoid cascading failures
                try { action(); } catch { }
            }
        }

    }
}