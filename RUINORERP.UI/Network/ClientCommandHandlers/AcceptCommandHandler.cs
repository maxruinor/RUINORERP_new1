using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using System;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Windows.Forms;
using RUINORERP.UI.Network;
using System.IO;
using System.Threading;

using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 被动接收命令处理器
    /// 处理服务器推送的命令：版本更新、系统退出、计算机状态查询
    /// </summary>
    [ClientCommandHandler("AcceptCommandHandler", 60)]
    public class AcceptCommandHandler : BaseClientCommandHandler
    {
        private readonly ILogger<AcceptCommandHandler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public AcceptCommandHandler(ILogger<AcceptCommandHandler> logger)
            : base(logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));

            // 注册支持的命令
            SetSupportedCommands(SystemCommands.SystemManagement);
            SetSupportedCommands(SystemCommands.ExceptionReport);
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
                _logger.LogDebug("命令处理器初始化成功");
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

            _logger.LogDebug($"收到命令: {packet.CommandId.FullCode}");

            // 根据命令ID处理不同的命令
            if (packet.CommandId == SystemCommands.SystemManagement)
            {
                await HandleSystemManagementCommandAsync(packet);
            }
            else if (packet.CommandId == SystemCommands.ExceptionReport)
            {
                await HandleExceptionReportCommandAsync(packet);
            }
            else if (packet.CommandId == SystemCommands.ComputerStatus)
            {
                await HandleComputerStatusCommandAsync(packet);
            }
            else
            {
                _logger.LogWarning($"未处理的命令ID: {packet.CommandId.FullCode}");
            }
        }

        /// <summary>
        /// 处理版本更新推送命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleSystemManagementCommandAsync(PacketModel packet)
        {
            try
            {
                if (packet.Request is SystemCommandRequest commandRequest)
                {
                    if (commandRequest.CommandType == SystemManagementType.ExitERPSystem)
                    {
                        // ✅ 修复：被强制下线时，先响应确认，再主动断开连接，最后退出
                        await Task.Run(async () =>
                        {
                            try
                            {
                                int delaySeconds = commandRequest.DelaySeconds;
                                
                                _logger.LogInformation("[强制下线] 收到强制下线指令，准备执行下线流程");
                                
                                // ✅ 第1步：立即向服务器发送确认响应
                                bool confirmationSent = false;
                                try
                                {
                                    var commService = RUINORERP.UI.Startup.GetFromFac<ClientCommunicationService>();
                                    if (commService != null && commService.ConnectionManager.IsConnected)
                                    {
                                        var confirmationResponse = new ResponseBase
                                        {
                                            IsSuccess = true,
                                            Message = "已同意强制下线，正在断开连接"
                                        };
                                        
                                        // ✅ 使用SendResponseAsync发送响应，使用原始请求的RequestId
                                        string originalRequestId = packet.ExecutionContext?.RequestId ?? string.Empty;
                                        await commService.SendResponseAsync(
                                            RUINORERP.PacketSpec.Commands.SystemCommands.SystemManagement, 
                                            confirmationResponse,
                                            originalRequestId);
                                        
                                        confirmationSent = true;
                                        _logger.LogInformation("[强制下线] 已向服务器发送确认响应, RequestId={RequestId}", originalRequestId);
                                    }
                                    else
                                    {
                                        _logger.LogWarning("[强制下线] 通信服务不可用或已断开");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning(ex, "[强制下线] 发送确认响应失败");
                                }
                                
                                // ✅ 第2步：短暂延迟，确保服务器收到确认
                                await Task.Delay(300);
                                
                                // ✅ 第3步：主动断开与服务器的连接
                                try
                                {
                                    var commService = RUINORERP.UI.Startup.GetFromFac<ClientCommunicationService>();
                                    if (commService != null && commService.ConnectionManager.IsConnected)
                                    {
                                        _logger.LogInformation("[强制下线] 正在主动断开与服务器的连接...");
                                        await commService.Disconnect();
                                        _logger.LogInformation("[强制下线] 已成功断开与服务器的连接");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning(ex, "[强制下线] 断开连接时发生异常");
                                }
                                
                                // ✅ 关键修复：清理本地Token和会话状态，避免重新启动时状态冲突
                                try
                                {
                                    var userLoginService = RUINORERP.UI.Startup.GetFromFac<UserLoginService>();
                                    if (userLoginService != null)
                                    {
                                        _logger.LogInformation("[强制下线] 正在清理本地Token和会话状态...");
                                        await userLoginService.CleanupLoginStateAsync();
                                        _logger.LogInformation("[强制下线] 本地状态清理完成");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning(ex, "[强制下线] 清理本地状态时发生异常");
                                }
                                
                                // ✅ 第4步：再短暂延迟，确保服务器处理完断开事件
                                await Task.Delay(500);
                                
                                // ✅ 第5步：执行系统退出
                                if (delaySeconds > 0)
                                {
                                    // 显示倒计时提示
                                    string message = $"系统将在 {delaySeconds} 秒后退出，这是管理员要求的操作。";
                                    string title = "系统即将退出";
                                    
                                    // 使用Task.Delay异步等待，避免占用线程池线程
                                    _ = Task.Run(async () =>
                                    {
                                        try
                                        {
                                            await Task.Delay(delaySeconds * 1000);
                                            
                                            // 延时后执行系统退出
                                            System.Windows.Forms.Application.Exit();
                                        }
                                        catch (Exception ex)
                                        {
                                            _logger.LogError(ex, "执行延时退出时发生异常");
                                        }
                                    });
                                    
                                    // 显示提示信息
                                    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    // 立即执行系统退出
                                    _logger.LogInformation("[强制下线] 立即退出系统");
                                    System.Windows.Forms.Application.Exit();
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "处理退出系统命令时发生异常");
                            }
                        });
                    }
                    else if (commandRequest.CommandType == SystemManagementType.ShutdownComputer)
                    {
                        // 在UI线程显示关机提示并执行关机
                        await Task.Run(() =>
                        {
                            try
                            {
                                int delaySeconds = commandRequest.DelaySeconds;
                                
                                if (delaySeconds > 0)
                                {
                                    // 显示倒计时提示
                                    string message = $"计算机将在 {delaySeconds} 秒后关闭，这是管理员要求的操作。\n请立即保存您的工作！";
                                    string title = "计算机即将关机";
                                    
                                    // 使用Task.Delay异步等待，避免占用线程池线程
                                    _ = Task.Run(async () =>
                                    {
                                        try
                                        {
                                            await Task.Delay(delaySeconds * 1000);
                                            
                                            // 延时后执行关机
                                            ExecuteSystemShutdown();
                                        }
                                        catch (Exception ex)
                                        {
                                            _logger.LogError(ex, "执行延时关机时发生异常");
                                        }
                                    });
                                    
                                    // 显示提示信息
                                    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    // 立即执行关机，先显示提示
                                    string message = "计算机将立即关闭，这是管理员要求的操作。\n请立即保存您的工作！";
                                    string title = "计算机即将关机";
                                    
                                    if (MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                                    {
                                        // 用户确认后执行关机
                                        ExecuteSystemShutdown();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "处理关机命令时发生异常");
                            }
                        });
                    }

                    if (commandRequest.CommandType==SystemManagementType.PushVersionUpdate)
                    {
                        // 提取版本更新信息
                        VersionUpdateInfo updateInfo = ExtractVersionUpdateInfo(packet);

                        if (updateInfo != null)
                        {
                            // ✅ 简化:直接使用 CurrentLoginStatus 判断锁定状态
                            bool isSystemLocked = MainForm.Instance?.CurrentLoginStatus == MainForm.LoginStatus.Locked;
                            bool isLoginScreenVisible = IsLoginScreenVisible();
                            
                            if (isSystemLocked || isLoginScreenVisible)
                            {
                                _logger.LogWarning($"系统处于锁定状态或登录界面显示中，暂时不显示更新提示。锁定状态: {isSystemLocked}, 登录界面显示: {isLoginScreenVisible}");
                                
                                // 将更新信息存储起来，等系统解锁或登录界面关闭后再显示
                                StorePendingUpdateInfo(updateInfo);
                                return; // 提前返回，不显示更新对话框
                            }
                            
                            // 在UI线程显示更新提示 - 使用BeginInvoke避免阻塞网络线程
                            if (MainForm.Instance.InvokeRequired)
                            {
                                MainForm.Instance.BeginInvoke(new Action(() =>
                                {
                                    DialogResult result = MessageBox.Show(
                                        $"发现新版本: {updateInfo.Version}\n{updateInfo.Description}\n是否立即更新？",
                                        "版本更新",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Information);

                                    if (result == DialogResult.Yes)
                                    {
                                        // 启动更新程序
                                        StartUpdateProcess(updateInfo);
                                    }
                                }));
                            }
                            else
                            {
                                DialogResult result = MessageBox.Show(
                                    $"发现新版本: {updateInfo.Version}\n{updateInfo.Description}\n是否立即更新？",
                                    "版本更新",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Information);

                                if (result == DialogResult.Yes)
                                {
                                    // 启动更新程序
                                    StartUpdateProcess(updateInfo);
                                }
                            }
                        }
                        else
                        {
                            _logger.LogWarning("版本更新命令缺少必要信息");
                        }
                    }
                }
             
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理版本更新命令时发生异常");
            }
        }

        /// <summary>
        /// 处理系统退出命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleExceptionReportCommandAsync(PacketModel packet)
        {
            try
            {
                _logger.LogDebug("收到服务器发送的退出系统命令");
                
                // 获取退出原因
                string exitReason = "服务器要求退出系统";
                if (packet.Request is GeneralRequest generalRequest && generalRequest.Data is Dictionary<string, object> data)
                {
                    if (data.TryGetValue("Reason", out object reasonObj))
                    {
                        exitReason = reasonObj.ToString();
                    }
                }
                
            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理退出系统命令时发生异常");
            }
        }

        /// <summary>
        /// 处理计算机状态查询命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleComputerStatusCommandAsync(PacketModel packet)
        {
            try
            {
                _logger.LogDebug("收到服务器发送的计算机状态查询命令");
                
                // 收集计算机状态信息
                Dictionary<string, object> statusInfo = CollectComputerStatus();
                
                // 构建响应数据
                var responseData = new
                {
                    Status = "Success",
                    Data = statusInfo
                };
                
                // 发送响应给服务器
                await SendResponseAsync(packet, responseData);
                
                _logger.LogDebug("计算机状态信息已发送至服务器");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理计算机状态查询命令时发生异常");
            }
        }


        /// <summary>
        /// 提取版本更新信息
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>版本更新信息</returns>
        private VersionUpdateInfo ExtractVersionUpdateInfo(PacketModel packet)
        {
            if (packet.Request is GeneralRequest generalRequest)
            {
                try
                {
                    // 尝试不同格式的数据解析
                    if (generalRequest.Data is Dictionary<string, object> dataDict)
                    {
                        var updateInfo = new VersionUpdateInfo();
                        
                        // 提取必要信息
                        if (dataDict.TryGetValue("Version", out object versionObj))
                            updateInfo.Version = versionObj.ToString();
                        
                        if (dataDict.TryGetValue("DownloadUrl", out object urlObj))
                            updateInfo.DownloadUrl = urlObj.ToString();
                        
                        if (dataDict.TryGetValue("Description", out object descObj))
                            updateInfo.Description = descObj.ToString();
                        
                        if (dataDict.TryGetValue("ForceUpdate", out object forceObj))
                        {
                            bool forceUpdate;
                            if (bool.TryParse(forceObj.ToString(), out forceUpdate))
                            {
                                updateInfo.ForceUpdate = forceUpdate;
                            }
                        }
                        
                        // 验证必要字段
                        if (!string.IsNullOrEmpty(updateInfo.Version) && !string.IsNullOrEmpty(updateInfo.DownloadUrl))
                            return updateInfo;
                    }
                    else if (generalRequest.Data is string jsonData)
                    {
                        // 尝试JSON字符串解析
                        return JsonConvert.DeserializeObject<VersionUpdateInfo>(jsonData);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "解析版本更新信息失败");
                }
            }
            
            return null;
        }

        /// <summary>
        /// 检查登录界面是否可见
        /// </summary>
        /// <returns>登录界面是否可见</returns>
        private bool IsLoginScreenVisible()
        {
            try
            {
                // 检查是否有登录窗口实例正在显示
                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType().Name.Contains("FrmLogin"))
                    {
                        return form.Visible;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查登录界面可见性时发生异常");
                return false;
            }
        }
        
        /// <summary>
        /// 存储待处理的更新信息
        /// </summary>
        /// <param name="updateInfo">版本更新信息</param>
        private void StorePendingUpdateInfo(VersionUpdateInfo updateInfo)
        {
            try
            {
                // 使用MainForm的静态方法存储待处理更新信息
                MainForm.SetPendingUpdate(updateInfo);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "存储待处理更新信息时发生异常");
            }
        }
        
        /// <summary>
        /// 启动更新程序
        /// </summary>
        /// <param name="updateInfo">版本更新信息</param>
        private void StartUpdateProcess(VersionUpdateInfo updateInfo)
        {
            try
            {
                _logger.LogDebug("启动更新程序");
                
                // 假设更新程序路径
                string updateExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoUpdate\\AutoUpdate.exe");
                
                if (File.Exists(updateExePath))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = updateExePath,
                        Arguments = $"--version={updateInfo.Version} --url={updateInfo.DownloadUrl} --force={updateInfo.ForceUpdate}",
                        UseShellExecute = true
                    };
                    
                    Process.Start(startInfo);
                    
                    // 如果强制更新，退出当前应用
                    if (updateInfo.ForceUpdate)
                    {
                        // 使用异步方式退出应用，避免阻塞
                        _ = Task.Run(async () =>
                        {
                            await Task.Delay(1000); // 等待1秒让更新程序启动
                            System.Windows.Forms.Application.Exit();
                        });
                    }
                }
                else
                {
                    _logger.LogError($"更新程序不存在: {updateExePath}");
                    // 确保在UI线程中显示消息框 - 使用BeginInvoke避免阻塞网络线程
                    if (MainForm.Instance.InvokeRequired)
                    {
                        MainForm.Instance.BeginInvoke(new Action(() =>
                        {
                            MessageBox.Show("更新程序不存在，请联系管理员。", "更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                    else
                    {
                        MessageBox.Show("更新程序不存在，请联系管理员。", "更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动更新程序失败");
                // 确保在UI线程中显示消息框 - 使用BeginInvoke避免阻塞网络线程
                if (MainForm.Instance.InvokeRequired)
                {
                    MainForm.Instance.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show($"启动更新程序失败: {ex.Message}", "更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show($"启动更新程序失败: {ex.Message}", "更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 收集计算机状态信息
        /// </summary>
        /// <returns>计算机状态信息字典</returns>
        private Dictionary<string, object> CollectComputerStatus()
        {
            var statusInfo = new Dictionary<string, object>();
            
            try
            {
                // 系统信息
                statusInfo["MachineName"] = Environment.MachineName;
                statusInfo["UserName"] = Environment.UserName;
                statusInfo["OSVersion"] = Environment.OSVersion.ToString();
                statusInfo["WorkingSet"] = Environment.WorkingSet;
                statusInfo["SystemDirectory"] = Environment.SystemDirectory;
                
                // 进程信息
                Process currentProcess = Process.GetCurrentProcess();
                statusInfo["ProcessId"] = currentProcess.Id;
                statusInfo["ProcessName"] = currentProcess.ProcessName;
                statusInfo["StartTime"] = currentProcess.StartTime;
                statusInfo["TotalProcessorTime"] = currentProcess.TotalProcessorTime.ToString();
                
                // 内存信息
                using (PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes"))
                {
                    statusInfo["AvailableMemoryMB"] = ramCounter.NextValue();
                }
                
                // CPU使用率
                using (PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
                {
                    cpuCounter.NextValue(); // 第一次调用返回0
                    System.Threading.Thread.Sleep(100);      // 等待短暂时间（CPU采样需要，此处阻塞可接受）
                    statusInfo["CpuUsagePercent"] = cpuCounter.NextValue();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "收集计算机状态信息失败");
            }
            
            return statusInfo;
        }

        /// <summary>
        /// 执行系统关机
        /// </summary>
        private void ExecuteSystemShutdown()
        {
            try
            {
                _logger.LogDebug("开始执行系统关机");
                
                // 使用Windows API执行关机
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "shutdown.exe",
                    Arguments = "/s /t 0 /f",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                
                Process.Start(startInfo);
                
                _logger.LogInformation("系统关机命令已执行");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行系统关机失败");
                
                // 显示错误提示
                MessageBox.Show(
                    $"执行关机失败: {ex.Message}\n\n请手动关闭计算机。",
                    "关机失败",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 发送响应给服务器
        /// </summary>
        /// <param name="requestPacket">请求数据包</param>
        /// <param name="responseData">响应数据</param>
        /// <returns>发送结果</returns>
        private async Task SendResponseAsync(PacketModel requestPacket, object responseData)
        {
            try
            {
                // 创建响应数据包
                //var responsePacket = new PacketModel
                //{
                //    CommandId = requestPacket.CommandId,
                //    RequestId = requestPacket.RequestId,
                //    Response = new ResponseBase
                //    {
                //        IsSuccess = true,
                //        date = responseData
                //    }
                //};
                
                //// 使用通信管理器发送响应
                //var communicationManager = Startup.GetFromFac<IClientCommunicationManager>();
                //if (communicationManager != null)
                //{
                //    await communicationManager.SendAsync(responsePacket);
                //}
                //else
                //{
                //    _logger.LogError("无法获取通信管理器实例");
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送响应给服务器失败");
            }
        }
    }

    /// <summary>
    /// 版本更新信息类
    /// </summary>
    public class VersionUpdateInfo
    {
        /// <summary>
        /// 新版本号
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadUrl { get; set; }
        
        /// <summary>
        /// 更新描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool ForceUpdate { get; set; }
    }
}