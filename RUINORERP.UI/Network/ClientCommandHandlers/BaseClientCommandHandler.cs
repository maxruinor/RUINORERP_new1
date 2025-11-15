using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 客户端命令处理器基类
    /// 提供客户端命令处理器的通用实现，简化具体处理器的开发
    /// </summary>
    public abstract class BaseClientCommandHandler : IClientCommandHandler
    {
        private readonly object _lockObject = new object();
        private readonly ILogger<BaseClientCommandHandler> _logger;
        /// <summary>
        /// 处理器唯一标识
        /// </summary>
        public string HandlerId { get; private set; }

        /// <summary>
        /// 处理器名称
        /// </summary>
        public string Name { get; private set; }
        public string HandlerName { get; private set; }

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 处理器支持的命令ID列表
        /// </summary>
        public IReadOnlyList<CommandId> SupportedCommands { get; private set; }

        /// <summary>
        /// 处理器状态
        /// </summary>
        public ClientHandlerStatus Status { get; private set; } = ClientHandlerStatus.Uninitialized;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseClientCommandHandler(ILogger<BaseClientCommandHandler> logger)
        {
            _logger = logger;
            HandlerId = GenerateHandlerId();
            InitializeHandlerInfo();
        }

        /// <summary>
        /// 生成处理器唯一ID
        /// </summary>
        /// <returns>唯一ID</returns>
        private string GenerateHandlerId()
        {
            return $"{GetType().Name}_{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        /// <summary>
        /// 初始化处理器信息
        /// 从特性中获取名称、优先级
        /// </summary>
        private void InitializeHandlerInfo()
        {
            var attribute = GetType().GetCustomAttributes(typeof(ClientCommandHandlerAttribute), false)
                .FirstOrDefault() as ClientCommandHandlerAttribute;
            
            if (attribute != null)
            {
                HandlerName = attribute.Name;
                Priority = attribute.Priority;
            }
            else
            {
                HandlerName = GetType().Name;
                Priority = 50; // 默认优先级
            }
            
            // 初始化支持的命令列表，实际命令将通过SetSupportedCommands方法设置
            SupportedCommands = Array.Empty<CommandId>().ToList();
        }

        /// <summary>
        /// 设置支持的命令列表
        /// </summary>
        /// <param name="commands">命令ID集合</param>
        protected void SetSupportedCommands(params CommandId[] commands)
        {
            lock (_lockObject)
            {
                if (commands == null)
                {
                    SupportedCommands = Array.Empty<CommandId>();
                    return;
                }

                var commandList = commands.ToList();
                SupportedCommands = commandList;
            }
        }

        /// <summary>
        /// 初始化处理器
        /// </summary>
        /// <returns>初始化是否成功</returns>
        public virtual Task<bool> InitializeAsync()
        {
            lock (_lockObject)
            {
                if (Status == ClientHandlerStatus.Uninitialized || Status == ClientHandlerStatus.Stopped)
                {
                    Status = ClientHandlerStatus.Initialized;
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 启动处理器
        /// </summary>
        /// <returns>启动是否成功</returns>
        public virtual Task<bool> StartAsync()
        {
            lock (_lockObject)
            {
                if (Status == ClientHandlerStatus.Initialized)
                {
                    Status = ClientHandlerStatus.Running;
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 停止处理器
        /// </summary>
        /// <returns>停止是否成功</returns>
        public virtual Task<bool> StopAsync()
        {
            lock (_lockObject)
            {
                if (Status == ClientHandlerStatus.Running)
                {
                    Status = ClientHandlerStatus.Stopped;
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 检查是否能处理指定的命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>是否能处理</returns>
        public virtual bool CanHandle(PacketModel packet)
        {
            if (packet == null)
                return false;

            // 如果处理器已停止或出错，不能处理命令
            if (Status == ClientHandlerStatus.Stopped || Status == ClientHandlerStatus.Error)
                return false;

            // 检查是否支持该命令ID
            return SupportedCommands.Any(cmd => cmd.FullCode == packet.CommandId.FullCode);
        }

        /// <summary>
        /// 处理命令
        /// 子类应重写此方法实现具体的命令处理逻辑
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        public abstract Task HandleAsync(PacketModel packet);

          
         
    }
}