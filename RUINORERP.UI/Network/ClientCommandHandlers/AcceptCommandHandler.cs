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
            SetSupportedCommands(SystemCommands.ComputerStatus);
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
                        // 在UI线程显示退出提示并执行退出
                        await Task.Run(() =>
                        {
                            // 执行系统退出
                            System.Windows.Forms.Application.Exit();

                        });

                    }

                    if (commandRequest.CommandType==SystemManagementType.PushVersionUpdate)
                    {
                        // 提取版本更新信息
                        VersionUpdateInfo updateInfo = ExtractVersionUpdateInfo(packet);

                        if (updateInfo != null)
                        {
                            // 在UI线程显示更新提示
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
                _logger.LogInformation("收到服务器发送的退出系统命令");
                
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
                _logger.LogInformation("收到服务器发送的计算机状态查询命令");
                
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
        /// 启动更新程序
        /// </summary>
        /// <param name="updateInfo">版本更新信息</param>
        private void StartUpdateProcess(VersionUpdateInfo updateInfo)
        {
            try
            {
                _logger.LogInformation("启动更新程序");
                
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
                        System.Windows.Forms.Application.Exit();
                    }
                }
                else
                {
                    _logger.LogError($"更新程序不存在: {updateExePath}");
                    MessageBox.Show("更新程序不存在，请联系管理员。", "更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动更新程序失败");
                MessageBox.Show($"启动更新程序失败: {ex.Message}", "更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    Thread.Sleep(100);      // 等待短暂时间
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