// 示例：将 SupportedCommands 从 uint 改为 CommandId 类型的实现

// 1. 首先更新接口定义
public interface ICommandHandler
{
    /// <summary>
    /// 支持的命令类型 - 改为 CommandId 类型
    /// </summary>
    IReadOnlyList<CommandId> SupportedCommands { get; }
    
    // ... 其他成员保持不变
}

// 2. 更新基类实现
public abstract class BaseCommandHandler : ICommandHandler
{
    /// <summary>
    /// 支持的命令类型 - 使用 CommandId 类型
    /// </summary>
    public virtual IReadOnlyList<CommandId> SupportedCommands { get; protected set; } = Array.Empty<CommandId>();

    /// <summary>
    /// 设置支持的命令列表（安全方法）- CommandId 版本
    /// </summary>
    /// <param name="commands">CommandId 数组</param>
    protected void SetSupportedCommands(params CommandId[] commands)
    {
        SupportedCommands = commands ?? Array.Empty<CommandId>();
    }

    /// <summary>
    /// 设置支持的命令列表（安全方法）- CommandId 版本
    /// </summary>
    /// <param name="commands">CommandId 集合</param>
    protected void SetSupportedCommands(IEnumerable<CommandId> commands)
    {
        SupportedCommands = commands?.ToList() ?? new List<CommandId>();
    }

    /// <summary>
    /// 判断是否可以处理指定的命令 - CommandId 版本
    /// </summary>
    /// <param name="commandId">命令ID</param>
    /// <returns>是否可以处理</returns>
    public virtual bool CanHandle(CommandId commandId)
    {
        var supportedCommands = SupportedCommands ?? Array.Empty<CommandId>();
        return supportedCommands.Contains(commandId);
    }

    /// <summary>
    /// 判断是否可以处理指定的命令 - uint 兼容版本
    /// </summary>
    /// <param name="commandCode">命令代码</param>
    /// <returns>是否可以处理</returns>
    public virtual bool CanHandle(uint commandCode)
    {
        var supportedCommands = SupportedCommands ?? Array.Empty<CommandId>();
        return supportedCommands.Any(cmd => cmd.FullCode == commandCode);
    }

    /// <summary>
    /// 判断是否可以处理指定的命令 - ICommand 版本
    /// </summary>
    /// <param name="command">命令对象</param>
    /// <returns>是否可以处理</returns>
    public virtual bool CanHandle(ICommand command)
    {
        if (command?.CommandIdentifier == null)
            return false;
            
        return CanHandle(command.CommandIdentifier);
    }
}

// 3. 处理器实现示例 - LockCommandHandler
public class LockCommandHandler : BaseCommandHandler
{
    /// <summary>
    /// 支持的命令类型 - 使用 CommandId
    /// </summary>
    public override IReadOnlyList<CommandId> SupportedCommands { get; protected set; }

    public LockCommandHandler(ILogger<LockCommandHandler> logger) : base(logger)
    {
        _logger = logger;
        
        // 使用 CommandId 直接设置，更加直观
        SetSupportedCommands(
            LockCommands.LockRequest,      // CommandId 类型
            LockCommands.LockRelease,      // CommandId 类型
            LockCommands.LockStatus,       // CommandId 类型
            LockCommands.ForceUnlock,      // CommandId 类型
            LockCommands.RequestUnlock,    // CommandId 类型
            LockCommands.RefuseUnlock,     // CommandId 类型
            LockCommands.RequestLock,      // CommandId 类型
            LockCommands.ReleaseLock,      // CommandId 类型
            LockCommands.ForceReleaseLock, // CommandId 类型
            LockCommands.QueryLockStatus,  // CommandId 类型
            LockCommands.BroadcastLockStatus // CommandId 类型
        );
    }

    /// <summary>
    /// 核心处理方法，使用 CommandId 进行判断
    /// </summary>
    protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
    {
        try
        {
            var commandId = cmd.Command.CommandIdentifier;
            
            // 使用 CommandId 直接比较，更加清晰
            if (commandId == LockCommands.LockRequest)
            {
                return await HandleLockRequestAsync(cmd.Command, cancellationToken);
            }
            else if (commandId == LockCommands.LockRelease)
            {
                return await HandleLockReleaseAsync(cmd.Command, cancellationToken);
            }
            else if (commandId == LockCommands.ForceUnlock)
            {
                return await HandleForceUnlockAsync(cmd.Command, cancellationToken);
            }
            // ... 其他命令处理
            else
            {
                return BaseCommand<IResponse>.CreateError(
                    $"不支持的锁命令类型: {commandId.Name} (0x{commandId.FullCode:X4})", 
                    UnifiedErrorCodes.Biz_ValidationFailed.Code
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"处理锁命令异常: {ex.Message}");
            return BaseCommand<IResponse>.CreateError(
                $"处理锁命令异常: {ex.Message}", 
                UnifiedErrorCodes.System_InternalError.Code
            );
        }
    }
}

// 4. 命令分发器更新
public class CommandDispatcher
{
    private readonly ConcurrentDictionary<CommandId, ICommandHandler> _commandHandlerMap;
    private readonly ConcurrentDictionary<uint, ICommandHandler> _commandHandlerMapUint; // 向后兼容

    private void UpdateCommandHandlerMapping(ICommandHandler handler)
    {
        if (handler == null)
        {
            _logger.LogWarning("尝试添加null处理器到映射中");
            return;
        }

        try
        {
            var supportedCommands = handler.SupportedCommands ?? Array.Empty<CommandId>();
            
            foreach (var commandId in supportedCommands)
            {
                // 使用 CommandId 作为键
                if (_commandHandlerMap.ContainsKey(commandId))
                {
                    _logger.LogWarning(
                        $"命令 {commandId.Name} (0x{commandId.FullCode:X4}) 已被处理器 {_commandHandlerMap[commandId].GetType().Name} 处理，" +
                        $"将被新处理器 {handler.GetType().Name} 替换"
                    );
                }
                
                _commandHandlerMap[commandId] = handler;
                
                // 同时维护 uint 版本用于向后兼容
                _commandHandlerMapUint[commandId.FullCode] = handler;
                
                _logger.LogDebug(
                    $"命令 {commandId.Name} (0x{commandId.FullCode:X4}) 已映射到处理器 {handler.GetType().Name}"
                );
            }
            
            _logger.LogInformation(
                $"处理器 {handler.GetType().Name} 的命令映射更新完成，支持 {supportedCommands.Count} 个命令"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"更新处理器 {handler.GetType().Name} 的命令映射时发生异常");
            throw;
        }
    }

    /// <summary>
    /// 获取命令处理器 - CommandId 版本
    /// </summary>
    public ICommandHandler GetCommandHandler(CommandId commandId)
    {
        _commandHandlerMap.TryGetValue(commandId, out var handler);
        return handler;
    }

    /// <summary>
    /// 获取命令处理器 - uint 兼容版本
    /// </summary>
    public ICommandHandler GetCommandHandler(uint commandCode)
    {
        _commandHandlerMapUint.TryGetValue(commandCode, out var handler);
        return handler;
    }
}

// 5. 使用示例
public class UsageExample
{
    public void DemonstrateUsage()
    {
        var handler = new LockCommandHandler(logger);
        
        // 检查支持情况
        var canHandleLock = handler.CanHandle(LockCommands.LockRequest); // true
        var canHandleHeartbeat = handler.CanHandle(SystemCommands.Heartbeat); // false
        
        // 日志输出更有可读性
        logger.LogInformation($"处理器支持以下命令: {string.Join(", ", handler.SupportedCommands.Select(c => c.Name))}");
        // 输出: "处理器支持以下命令: LockRequest, LockRelease, LockStatus, ..."
        
        // 错误信息更清晰
        var errorCmd = new CommandId(CommandCategory.System, 0x99, "UnknownCommand");
        logger.LogError($"不支持的命令: {errorCmd.Name} (类别: {errorCmd.Category}, 代码: 0x{errorCmd.FullCode:X4})");
        // 输出: "不支持的命令: UnknownCommand (类别: System, 代码: 0x0099)"
    }
}