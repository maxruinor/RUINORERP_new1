using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.ClientCommandHandlers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 锁管理命令处理器
    /// 负责处理与分布式锁相关的命令，包括锁请求、释放、状态查询等
    /// </summary>
    [ClientCommandHandler("LockCommandHandler", 60)]
    public class LockCommandHandler : BaseClientCommandHandler
    {
        private readonly ILogger<LockCommandHandler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public LockCommandHandler(ILogger<LockCommandHandler> logger)
            : base(logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
                else if (packet.CommandId == LockCommands.CheckLockStatus) // 保留检查锁定的处理
                {
                    await HandleCheckLockAsync(packet);
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
                else if (packet.CommandId == LockCommands.Lock || packet.CommandId == LockCommands.Unlock || 
                         packet.CommandId == LockCommands.ForceUnlock || packet.CommandId == LockCommands.CheckLockStatus)
                {
                    await HandleDocumentLockCommandAsync(packet);
                }
                else if (packet.CommandId == LockCommands.BroadcastLockStatus) // 作为转发单据锁定的兼容处理
                {
                    await HandleForwardDocumentLockAsync(packet);
                }
                else
                {
                    _logger.LogWarning($"未处理的锁管理命令ID: {packet.CommandId.FullCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理锁管理命令 {packet.CommandId?.FullCode} 时发生错误");
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
                    _logger.LogDebug($"收到锁请求响应: 资源ID={lockResponse.ResourceId}, 成功={lockResponse.Success}");

                    // 处理锁请求响应，通知相关UI组件
                    if (lockResponse.Success)
                    {
                        // 锁获取成功，可以更新UI状态
                        _logger.LogInformation($"成功获取资源 '{lockResponse.ResourceId}' 的锁，锁ID: {lockResponse.LockId}");
                        
                        // 触发锁获取成功事件或更新UI组件状态
                        // 例如：通知正在编辑的表单可以开始编辑
                    }
                    else
                    {
                        // 锁获取失败
                        _logger.LogWarning($"获取资源 '{lockResponse.ResourceId}' 的锁失败: {lockResponse.ErrorMessage}");

                        // 如果有当前锁定信息，可以显示给用户
                        if (lockResponse.CurrentLockInfo != null)
                        {
                            string message = $"资源已被锁定\n" +
                                            $"锁定用户: {lockResponse.CurrentLockInfo.LockedUserName}\n" +
                                            $"锁定时间: {lockResponse.LockTime}\n" +
                                            $"客户端ID: {lockResponse.CurrentLockInfo.ClientId}";
                            // 在UI线程显示提示
                            Application.Current?.Dispatcher?.Invoke(() =>
                            {
                                MessageBox.Show(message, "锁定提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            });
                        }
                        else
                        {
                            // 显示通用的锁定失败消息
                            Application.Current?.Dispatcher?.Invoke(() =>
                            {
                                MessageBox.Show(
                                    $"获取锁失败: {lockResponse.ErrorMessage}", 
                                    "锁定失败", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Warning);
                            });
                        }
                    }
                }
                // 新增处理锁定请求冲突的逻辑
                else if (packet.Request is LockRequest lockRequest)
                {
                    _logger.LogDebug($"收到锁定请求: BillID={lockRequest.BillID}, UserID={lockRequest.UserId}");
                    
                    // 在UI线程显示锁定请求确认对话框
                    Application.Current?.Dispatcher?.Invoke(() =>
                    {
                        DialogResult result = MessageBox.Show(
                            $"用户 {lockRequest.UserName} 请求锁定您当前正在编辑的单据 {lockRequest.BillID}。\n\n是否允许锁定？",
                            "锁定请求",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        // 根据用户选择处理
                        if (result == DialogResult.Yes)
                        {
                            _logger.LogInformation("用户允许其他用户锁定单据: BillID={BillID}", lockRequest.BillID);
                            // 这里可以实现解锁当前单据的逻辑
                        }
                        else
                        {
                            _logger.LogInformation("用户拒绝其他用户锁定单据: BillID={BillID}", lockRequest.BillID);
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
                    _logger.LogDebug($"收到锁释放响应: 资源ID={lockResponse.ResourceId}, 成功={lockResponse.Success}");

                    if (lockResponse.Success)
                    {
                        _logger.LogInformation($"成功释放资源 '{lockResponse.ResourceId}' 的锁");
                        // 更新UI状态，通知相关组件锁已释放
                    }
                    else
                    {
                        _logger.LogWarning($"释放资源 '{lockResponse.ResourceId}' 的锁失败: {lockResponse.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理锁释放命令时发生错误");
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
                    _logger.LogDebug($"收到锁状态查询响应: 资源ID={lockResponse.ResourceId}, 是否已锁定={lockResponse.Success}");

                    // 处理锁状态信息，更新UI
                    if (lockResponse.CurrentLockInfo != null)
                    {
                        _logger.LogInformation($"资源 '{lockResponse.ResourceId}' 已被用户 '{lockResponse.CurrentLockInfo.LockedUserName}' 锁定");
                    }
                    else
                    {
                        _logger.LogInformation($"资源 '{lockResponse.ResourceId}' 当前未被锁定");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理锁状态查询命令时发生错误");
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
                    _logger.LogDebug($"收到强制解锁响应: 资源ID={lockResponse.ResourceId}, 成功={lockResponse.Success}");

                    if (lockResponse.Success)
                    {
                        _logger.LogInformation($"成功强制释放资源 '{lockResponse.ResourceId}' 的锁");
                        // 更新UI状态，通知相关组件锁已强制释放
                    }
                    else
                    {
                        _logger.LogWarning($"强制释放资源 '{lockResponse.ResourceId}' 的锁失败: {lockResponse.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理强制解锁命令时发生错误");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 处理锁检查命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleCheckLockAsync(PacketModel packet)
        {
            try
            {
                if (packet.Response is LockResponse lockResponse)
                {
                    _logger.LogDebug($"收到锁检查响应: 资源ID={lockResponse.ResourceId}, 可锁定={lockResponse.Success}");

                    // 处理锁检查结果，用于客户端决定是否可以进行操作
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理锁检查命令时发生错误");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 处理锁广播命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleLockBroadcastAsync(PacketModel packet)
        {
            try
            {
                // 解析广播消息，通知相关组件更新锁状态
                if (packet.Response is LockResponse lockResponse)
                {
                    _logger.LogDebug($"收到锁状态广播: 资源ID={lockResponse.ResourceId}, 状态={lockResponse.Success}");

                    // 这里可以触发事件或使用消息总线通知应用程序其他部分锁状态已更改
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
                    Application.Current?.Dispatcher?.Invoke(() =>
                    {
                        DialogResult result = MessageBox.Show(
                            $"用户 {unlockRequest.RequesterUserName} 请求解锁您锁定的单据 {unlockRequest.LockInfo?.BillID ?? 0}，是否同意解锁？",
                            "解锁请求",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        // 这里可以添加代码发送解锁响应
                        // 实际实现中需要调用相应的服务向服务器发送响应
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
                    Application.Current?.Dispatcher?.Invoke(() =>
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
        /// 处理单据锁定相关命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleDocumentLockCommandAsync(PacketModel packet)
        {
            try
            {
                if (packet.Response is LockResponse lockResponse)
                {
                    _logger.LogDebug($"收到单据锁定命令响应: 资源ID={lockResponse.ResourceId}, 成功={lockResponse.Success}, 命令={packet.CommandId.FullCode}");

                    // 根据不同的命令类型处理单据锁定响应
                    // 这里可以更新UI状态，通知相关组件
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理单据锁定命令 {packet.CommandId.FullCode} 时发生错误");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 处理转发单据锁定命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleForwardDocumentLockAsync(PacketModel packet)
        {
            try
            {
                _logger.LogDebug($"收到转发单据锁定命令: {packet.CommandId.FullCode}");
                // 处理转发的单据锁定命令，这里可以触发相应的业务逻辑
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理转发单据锁定命令时发生错误");
            }
            await Task.CompletedTask;
        }
    }
}