using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.PacketSpec.Services
{
    /// <summary>
    /// 数据包规格服务接口 - 对外提供的主要服务接口
    /// </summary>
    public interface IPacketSpecService
    {
        /// <summary>
        /// 命令调度器
        /// </summary>
        CommandDispatcher CommandDispatcher { get; }

        /// <summary>
        /// 命令调度器
        /// </summary>
        CommandScheduler CommandScheduler { get; }

        /// <summary>
        /// 初始化服务
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>初始化结果</returns>
        Task<bool> InitializeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="timeout">停止超时时间</param>
        /// <returns>停止结果</returns>
        Task<bool> StopAsync(TimeSpan? timeout = null);

        /// <summary>
        /// 处理接收到的数据包
        /// </summary>
        /// <param name="packageInfo">数据包信息</param>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        Task<CommandResult> ProcessPackageAsync(BasePackageInfo packageInfo, SessionInfo sessionInfo, CancellationToken cancellationToken = default);

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送结果</returns>
        Task<CommandResult> SendCommandAsync(ICommand command, CancellationToken cancellationToken = default);

        /// <summary>
        /// 调度命令（加入队列异步处理）
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>是否成功加入队列</returns>
        bool ScheduleCommand(ICommand command);

        /// <summary>
        /// 注册命令处理器
        /// </summary>
        /// <typeparam name="T">处理器类型</typeparam>
        void RegisterCommandHandler<T>() where T : class, ICommandHandler;

        /// <summary>
        /// 获取服务统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        PacketSpecServiceStats GetServiceStats();
    }

    /// <summary>
    /// 数据包规格服务实现
    /// </summary>
    public class PacketSpecService : IPacketSpecService, IDisposable
    {
        private readonly ICommandHandlerFactory _handlerFactory;
        private bool _disposed = false;
        private bool _initialized = false;

        /// <summary>
        /// 命令调度器
        /// </summary>
        public CommandDispatcher CommandDispatcher { get; private set; }

        /// <summary>
        /// 命令调度器
        /// </summary>
        public CommandScheduler CommandScheduler { get; private set; }

        /// <summary>
        /// 服务统计信息
        /// </summary>
        private readonly PacketSpecServiceStats _stats;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handlerFactory">处理器工厂</param>
        public PacketSpecService(ICommandHandlerFactory handlerFactory = null)
        {
            _handlerFactory = handlerFactory ?? new DefaultCommandHandlerFactory();
            _stats = new PacketSpecServiceStats();
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        public async Task<bool> InitializeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_initialized)
                {
                    return true;
                }

                LogInfo("初始化数据包规格服务...");

                // 初始化命令调度器
                CommandDispatcher = new CommandDispatcher(_handlerFactory);
                CommandScheduler = new CommandScheduler(CommandDispatcher);

                // 注册默认命令处理器
                RegisterDefaultHandlers();

                _stats.ServiceStartTime = DateTime.UtcNow;
                _initialized = true;

                LogInfo("数据包规格服务初始化完成");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"初始化数据包规格服务失败: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public async Task<bool> StopAsync(TimeSpan? timeout = null)
        {
            try
            {
                if (!_initialized)
                {
                    return true;
                }

                LogInfo("正在停止数据包规格服务...");

                // 停止命令调度器
                if (CommandScheduler != null)
                {
                    await CommandScheduler.StopAsync(timeout);
                }

                _initialized = false;
                LogInfo("数据包规格服务已停止");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"停止数据包规格服务失败: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 处理接收到的数据包
        /// </summary>
        public async Task<CommandResult> ProcessPackageAsync(BasePackageInfo packageInfo, SessionInfo sessionInfo, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!_initialized)
                {
                    return CommandResult.CreateError("服务未初始化");
                }

                _stats.TotalPackagesProcessed++;
                _stats.LastPackageProcessTime = DateTime.UtcNow;

                // 根据包信息创建相应的命令
                var command = CreateCommandFromPackage(packageInfo, sessionInfo);
                if (command == null)
                {
                    _stats.TotalErrors++;
                    return CommandResult.CreateError("无法创建命令对象");
                }

                // 执行命令
                var result = await CommandDispatcher.DispatchAsync(command, cancellationToken);
                
                if (result.Success)
                {
                    _stats.TotalSuccessfulProcesses++;
                }
                else
                {
                    _stats.TotalErrors++;
                }

                return result;
            }
            catch (Exception ex)
            {
                _stats.TotalErrors++;
                LogError($"处理数据包失败: {ex.Message}", ex);
                return CommandResult.CreateError($"处理数据包异常: {ex.Message}", ex.GetType().Name);
            }
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        public async Task<CommandResult> SendCommandAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!_initialized)
                {
                    return CommandResult.CreateError("服务未初始化");
                }

                command.OperationType = CmdOperation.Send;
                return await CommandDispatcher.DispatchAsync(command, cancellationToken);
            }
            catch (Exception ex)
            {
                LogError($"发送命令失败: {ex.Message}", ex);
                return CommandResult.CreateError($"发送命令异常: {ex.Message}", ex.GetType().Name);
            }
        }

        /// <summary>
        /// 调度命令（加入队列异步处理）
        /// </summary>
        public bool ScheduleCommand(ICommand command)
        {
            try
            {
                if (!_initialized)
                {
                    return false;
                }

                return CommandScheduler.EnqueueCommand(command);
            }
            catch (Exception ex)
            {
                LogError($"调度命令失败: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 注册命令处理器
        /// </summary>
        public void RegisterCommandHandler<T>() where T : class, ICommandHandler
        {
            CommandDispatcher?.RegisterHandler<T>();
        }

        /// <summary>
        /// 获取服务统计信息
        /// </summary>
        public PacketSpecServiceStats GetServiceStats()
        {
            _stats.CurrentQueueSize = CommandScheduler?.QueueCount ?? 0;
            _stats.IsRunning = _initialized;
            return _stats;
        }

        /// <summary>
        /// 注册默认命令处理器
        /// </summary>
        private void RegisterDefaultHandlers()
        {
            // 这里可以注册默认的命令处理器
            // RegisterCommandHandler<LoginCommandHandler>();
            // RegisterCommandHandler<MessageResponseCommandHandler>();
        }

        /// <summary>
        /// 从数据包创建命令对象
        /// </summary>
        private ICommand CreateCommandFromPackage(BasePackageInfo packageInfo, SessionInfo sessionInfo)
        {
            if (packageInfo is BizPackageInfo bizPackage)
            {
                return CreateBizCommand(bizPackage, sessionInfo);
            }
            else if (packageInfo is LanderPackageInfo landerPackage)
            {
                return CreateLanderCommand(landerPackage, sessionInfo);
            }

            return null;
        }

        /// <summary>
        /// 创建业务命令
        /// </summary>
        private ICommand CreateBizCommand(BizPackageInfo packageInfo, SessionInfo sessionInfo)
        {
            // 根据包的Command字段创建相应的命令
            // 这里需要根据具体的协议实现
            return null;
        }

        /// <summary>
        /// 创建登录命令
        /// </summary>
        private ICommand CreateLanderCommand(LanderPackageInfo packageInfo, SessionInfo sessionInfo)
        {
            // 根据包的内容创建登录相关命令
            return null;
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        private void LogInfo(string message)
        {
            Console.WriteLine($"[PacketSpecService] INFO: {message}");
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        private void LogError(string message, Exception ex = null)
        {
            Console.WriteLine($"[PacketSpecService] ERROR: {message}");
            if (ex != null)
            {
                Console.WriteLine($"[PacketSpecService] Exception: {ex}");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                StopAsync(TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
                
                CommandScheduler?.Dispose();
                CommandDispatcher?.Dispose();

                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// 数据包规格服务统计信息
    /// </summary>
    public class PacketSpecServiceStats
    {
        /// <summary>
        /// 服务启动时间
        /// </summary>
        public DateTime ServiceStartTime { get; set; }

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// 总处理包数
        /// </summary>
        public long TotalPackagesProcessed { get; set; }

        /// <summary>
        /// 总成功处理数
        /// </summary>
        public long TotalSuccessfulProcesses { get; set; }

        /// <summary>
        /// 总错误数
        /// </summary>
        public long TotalErrors { get; set; }

        /// <summary>
        /// 当前队列大小
        /// </summary>
        public int CurrentQueueSize { get; set; }

        /// <summary>
        /// 最后处理包时间
        /// </summary>
        public DateTime LastPackageProcessTime { get; set; }

        /// <summary>
        /// 服务运行时间
        /// </summary>
        public TimeSpan Uptime => DateTime.UtcNow - ServiceStartTime;

        /// <summary>
        /// 成功率
        /// </summary>
        public double SuccessRate => TotalPackagesProcessed > 0 
            ? (double)TotalSuccessfulProcesses / TotalPackagesProcessed * 100 
            : 0;

        /// <summary>
        /// 平均处理速度（包/秒）
        /// </summary>
        public double AverageProcessingRate => Uptime.TotalSeconds > 0 
            ? TotalPackagesProcessed / Uptime.TotalSeconds 
            : 0;
    }
}