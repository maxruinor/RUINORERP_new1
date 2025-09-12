using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令调度器 - 负责管理命令队列和异步处理
    /// </summary>
    public class CommandScheduler : IDisposable
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly BlockingCollection<ICommand> _commandQueue;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _processingTask;
        private bool _disposed = false;

        /// <summary>
        /// 队列中的命令数量
        /// </summary>
        public int QueueCount => _commandQueue.Count;

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandDispatcher">命令分发器</param>
        /// <param name="maxQueueSize">最大队列大小</param>
        public CommandScheduler(CommandDispatcher commandDispatcher, int maxQueueSize = 1000)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _commandQueue = new BlockingCollection<ICommand>(maxQueueSize);
            _cancellationTokenSource = new CancellationTokenSource();
            
            // 启动后台处理任务
            _processingTask = Task.Run(ProcessCommandsAsync);
            IsRunning = true;
        }

        /// <summary>
        /// 添加命令到队列
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>是否成功添加</returns>
        public bool EnqueueCommand(ICommand command)
        {
            if (_disposed || command == null)
            {
                return false;
            }

            try
            {
                _commandQueue.Add(command, _cancellationTokenSource.Token);
                return true;
            }
            catch (InvalidOperationException)
            {
                // 队列已关闭
                return false;
            }
            catch (OperationCanceledException)
            {
                // 操作被取消
                return false;
            }
        }

        /// <summary>
        /// 异步添加命令到队列
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>是否成功添加</returns>
        public async Task<bool> EnqueueCommandAsync(ICommand command, TimeSpan? timeout = null)
        {
            if (_disposed || command == null)
            {
                return false;
            }

            return await Task.Run(() =>
            {
                try
                {
                    var cancellationToken = timeout.HasValue
                        ? new CancellationTokenSource(timeout.Value).Token
                        : _cancellationTokenSource.Token;

                    _commandQueue.Add(command, cancellationToken);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// 立即执行命令（不经过队列）
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        public async Task<CommandResult> ExecuteImmediateAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            if (_disposed || command == null)
            {
                return CommandResult.CreateError("调度器已关闭或命令无效");
            }

            return await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }

        /// <summary>
        /// 处理队列中的命令
        /// </summary>
        private async Task ProcessCommandsAsync()
        {
            LogInfo("命令处理器启动");

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    // 尝试从队列中取出命令，设置超时以便定期检查取消令牌
                    if (_commandQueue.TryTake(out ICommand command, 1000, _cancellationTokenSource.Token))
                    {
                        // 处理命令
                        await ProcessSingleCommandAsync(command);
                    }
                }
                catch (OperationCanceledException)
                {
                    // 正常取消，退出循环
                    break;
                }
                catch (Exception ex)
                {
                    LogError($"处理命令队列时出错: {ex.Message}", ex);
                    
                    // 短暂延迟后继续处理
                    await Task.Delay(100, _cancellationTokenSource.Token);
                }
            }

            LogInfo("命令处理器停止");
        }

        /// <summary>
        /// 处理单个命令
        /// </summary>
        /// <param name="command">命令对象</param>
        private async Task ProcessSingleCommandAsync(ICommand command)
        {
            try
            {
                LogDebug($"开始处理命令: {command.GetType().Name}");

                var result = await _commandDispatcher.DispatchAsync(_commandQueue, command, _cancellationTokenSource.Token);

                if (result.Success)
                {
                    LogDebug($"命令处理成功: {command.GetType().Name} - {result.ExecutionTimeMs}ms");
                }
                else
                {
                    LogWarning($"命令处理失败: {command.GetType().Name} - {result.Message}");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理命令 {command.GetType().Name} 时出现异常: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 停止调度器
        /// </summary>
        /// <param name="timeout">停止超时时间</param>
        public async Task StopAsync(TimeSpan? timeout = null)
        {
            if (_disposed || !IsRunning)
            {
                return;
            }

            LogInfo("正在停止命令处理器...");

            // 标记队列完成，不再接受新命令
            _commandQueue.CompleteAdding();

            // 取消处理
            _cancellationTokenSource.Cancel();

            try
            {
                // 等待处理任务完成
                var timeoutMs = timeout?.TotalMilliseconds ?? 5000; // 默认5秒超时
                await _processingTask.WaitAsync(TimeSpan.FromMilliseconds(timeoutMs));
            }
            catch (TimeoutException)
            {
                LogWarning("命令处理器停止超时");
            }

            IsRunning = false;
            LogInfo("命令处理器已停止");
        }

        /// <summary>
        /// 获取队列统计信息
        /// </summary>
        public CommandQueueStats GetQueueStats()
        {
            return new CommandQueueStats
            {
                QueueCount = _commandQueue.Count,
                IsRunning = IsRunning,
                IsCompleted = _commandQueue.IsCompleted,
                ProcessingTaskStatus = _processingTask.Status
            };
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        private void LogInfo(string message)
        {
            Console.WriteLine($"[CommandScheduler] INFO: {message}");
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        private void LogDebug(string message)
        {
            #if DEBUG
            Console.WriteLine($"[CommandScheduler] DEBUG: {message}");
            #endif
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        private void LogWarning(string message)
        {
            Console.WriteLine($"[CommandScheduler] WARNING: {message}");
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        private void LogError(string message, Exception ex = null)
        {
            Console.WriteLine($"[CommandScheduler] ERROR: {message}");
            if (ex != null)
            {
                Console.WriteLine($"[CommandScheduler] Exception: {ex}");
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

                _commandQueue?.Dispose();
                _cancellationTokenSource?.Dispose();
                _processingTask?.Dispose();

                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// 命令队列统计信息
    /// </summary>
    public class CommandQueueStats
    {
        /// <summary>
        /// 队列中的命令数量
        /// </summary>
        public int QueueCount { get; set; }

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// 队列是否已完成
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// 处理任务状态
        /// </summary>
        public TaskStatus ProcessingTaskStatus { get; set; }
    }
}