using System;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.ServerSession;
using RUINORERP.Server.BizService;
using RUINORERP.Server.Comm;
 
using System.Diagnostics;
using System.Linq;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Models;
using RUINORERP.Business.CommService;
using RUINORERP.PacketSpec.Commands.Business;
using RUINORERP.PacketSpec.Enums;

namespace RUINORERP.Server.Commands
{
    /// <summary>
    /// 缓存管理命令 - 统一处理请求缓存、更新缓存、删除缓存
    /// 替代BizCommand中的ClientCommand.请求缓存、ClientCommand.更新缓存、ClientCommand.删除缓存处理逻辑
    /// </summary>
    public class CacheManagementCommand : IServerCommand
    {
        public CacheOperation Operation { get; set; } = CacheOperation.Request;
        public string TableName { get; set; } = string.Empty;
        public string CacheKey { get; set; } = string.Empty;
        public object CacheData { get; set; }
        public TimeSpan? ExpireTime { get; set; }
        public bool BroadcastToAll { get; set; } = false;
        
        /// <summary>
        /// 会话信息
        /// </summary>
        public SessionInfo SessionInfo { get; set; } = new SessionInfo();
        
        /// <summary>
        /// 原始数据包
        /// </summary>
        public OriginalData? DataPacket { get; set; }
        
        public string Description => $"缓存管理命令: {Operation}";
        public bool RequiresAuthentication => true;
        public int TimeoutMs { get; set; } = 30000;
        public string CommandId { get; set; } = Guid.NewGuid().ToString();
        public CmdOperation OperationType { get; set; } = CmdOperation.Receive;
 
        
        /// <summary>
        /// 来源会话
        /// </summary>
        public SessionforBiz FromSession { get; set; }
        
        /// <summary>
        /// 目标会话
        /// </summary>
        public SessionforBiz ToSession { get; set; }
        
        /// <summary>
        /// 命令优先级
        /// </summary>
        public int Priority { get; set; } = 3;

        public CacheManagementCommand()
        {
        }

        public CacheManagementCommand(OriginalData data, SessionforBiz fromSession, CacheOperation operation)
        {
            DataPacket = data;
            FromSession = fromSession;
            Operation = operation;
            if (fromSession?.User != null)
            {
                SessionInfo.Username = fromSession.User.用户名;
                sessionInfo.SessionID = fromSession.SessionID;
            }
        }

        public async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // 解析数据包
                if (DataPacket.HasValue && FromSession != null)
                {
                    AnalyzeDataPacket(DataPacket.Value, FromSession);
                }

                // 根据操作类型执行相应的缓存操作
                switch (Operation)
                {
                    case CacheOperation.Request:
                        await HandleCacheRequestAsync();
                        break;
                        
                    case CacheOperation.Update:
                        await HandleCacheUpdateAsync();
                        break;
                        
                    case CacheOperation.Delete:
                        await HandleCacheDeleteAsync();
                        break;
                        
                    case CacheOperation.Clear:
                        await HandleCacheClearAsync();
                        break;
                        
                    case CacheOperation.Sync:
                        await HandleCacheSyncAsync();
                        break;
                        
                    default:
                        return CommandResult.CreateError($"不支持的缓存操作类型: {Operation}");
                }
                
                return CommandResult.CreateSuccess();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CacheManagementCommand执行时出错: {ex.Message}");
                await NotifyErrorAsync(ex.Message);
                return CommandResult.CreateError(ex.Message);
            }
        }
        
        public bool CanExecute()
        {
            return SessionInfo?.IsAuthenticated == true;
        }
        
        public bool AnalyzeDataPacket(OriginalData data, SessionInfo sessionInfo)
        {
            SessionInfo = sessionInfo;
            DataPacket = data;
            return true;
        }
        
        public void BuildDataPacket(object request = null)
        {
            // 简单实现
        }
        
        public RUINORERP.PacketSpec.Commands.ValidationResult ValidateParameters()
        {
            if (SessionInfo?.IsAuthenticated != true)
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("用户未认证");
            return RUINORERP.PacketSpec.Commands.ValidationResult.Success();
        }
        
        public string GetCommandId()
        {
            return CommandId;
        }

        /// <summary>
        /// 解析数据包
        /// </summary>
        private bool AnalyzeDataPacket(OriginalData gd, SessionforBiz fromSession)
        {
            try
            {
                int index = 0;
                var timestamp = ByteDataAnalysis.GetString(gd.Two, ref index);
                
                switch (Operation)
                {
                    case CacheOperation.Request:
                        TableName = ByteDataAnalysis.GetString(gd.Two, ref index);
                        break;
                        
                    case CacheOperation.Update:
                        TableName = ByteDataAnalysis.GetString(gd.Two, ref index);
                        CacheKey = ByteDataAnalysis.GetString(gd.Two, ref index);
                        // CacheData 通过其他方式传递，比如JSON序列化
                        var jsonData = ByteDataAnalysis.GetString(gd.Two, ref index);
                        if (!string.IsNullOrEmpty(jsonData))
                        {
                            // 这里可以根据需要反序列化CacheData
                            CacheData = jsonData;
                        }
                        BroadcastToAll = gd.Two.Length > index + 1 ? ByteDataAnalysis.Getbool(gd.Two, ref index) : false;
                        break;
                        
                    case CacheOperation.Delete:
                        TableName = ByteDataAnalysis.GetString(gd.Two, ref index);
                        CacheKey = ByteDataAnalysis.GetString(gd.Two, ref index);
                        BroadcastToAll = gd.Two.Length > index + 1 ? ByteDataAnalysis.Getbool(gd.Two, ref index) : false;
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"解析缓存管理数据包时出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 处理缓存请求
        /// </summary>
        private async Task HandleCacheRequestAsync()
        {
            try
            {
                if (frmMain.Instance.IsDebug)
                {
                    frmMain.Instance.PrintMsg($"{FromSession?.User?.用户名}请求缓存表：{TableName}");
                }

                var stopwatch = Stopwatch.StartNew();
                
                // 如果指定了表名，则只发送指定表的数据，否则全部发送
                if (!string.IsNullOrEmpty(TableName) && BizCacheHelper.Manager.NewTableList.Keys.Contains(TableName))
                {
                    await SendCacheDataAsync(TableName);
                }
                else if (string.IsNullOrEmpty(TableName))
                {
                    // 发送所有缓存数据
                    await SendAllCacheDataAsync();
                }
                else
                {
                    await NotifyErrorAsync($"请求的缓存表 {TableName} 不存在");
                }

                stopwatch.Stop();
                
                if (frmMain.Instance.IsDebug)
                {
                    frmMain.Instance.PrintInfoLog($"发送缓存数据列表{TableName}给{FromSession?.User?.用户名} 耗时：{stopwatch.ElapsedMilliseconds} 毫秒");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理缓存请求时出错: {ex.Message}");
                await NotifyErrorAsync($"缓存请求处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理缓存更新
        /// </summary>
        private async Task HandleCacheUpdateAsync()
        {
            try
            {
                if (frmMain.Instance.IsDebug)
                {
                    frmMain.Instance.PrintMsg($"{FromSession?.User?.用户名}更新缓存：{TableName}");
                }

                // 调用原有的更新缓存逻辑
                UserService.接收更新缓存指令(FromSession, DataPacket.Value);

                // 如果需要广播给所有客户端
                if (BroadcastToAll)
                {
                    await BroadcastCacheUpdateAsync();
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理缓存更新时出错: {ex.Message}");
                await NotifyErrorAsync($"缓存更新失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理缓存删除
        /// </summary>
        private async Task HandleCacheDeleteAsync()
        {
            try
            {
                if (frmMain.Instance.IsDebug)
                {
                    frmMain.Instance.PrintMsg($"{FromSession?.User?.用户名}删除缓存：{TableName}");
                }

                // 调用原有的删除缓存逻辑
                UserService.接收删除缓存指令(FromSession, DataPacket.Value);

                // 如果需要广播给所有客户端
                if (BroadcastToAll)
                {
                    await BroadcastCacheDeleteAsync();
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理缓存删除时出错: {ex.Message}");
                await NotifyErrorAsync($"缓存删除失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理缓存清空
        /// </summary>
        private async Task HandleCacheClearAsync()
        {
            try
            {
                if (frmMain.Instance.IsDebug)
                {
                    frmMain.Instance.PrintMsg($"{FromSession?.User?.用户名}清空缓存：{TableName}");
                }

                // 实现缓存清空逻辑
                //if (!string.IsNullOrEmpty(TableName))
                //{
                //    BizCacheHelper.Manager.ClearTableCache(TableName);
                //}
                //else
                //{
                //    BizCacheHelper.Manager.ClearAllCache();
                //}

                // 广播缓存清空通知
                if (BroadcastToAll)
                {
                    await BroadcastCacheClearAsync();
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理缓存清空时出错: {ex.Message}");
                await NotifyErrorAsync($"缓存清空失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理缓存同步
        /// </summary>
        private async Task HandleCacheSyncAsync()
        {
            try
            {
                if (frmMain.Instance.IsDebug)
                {
                    frmMain.Instance.PrintMsg($"{FromSession?.User?.用户名}同步缓存：{TableName}");
                }

                // 实现缓存同步逻辑
                if (!string.IsNullOrEmpty(TableName))
                {
                    await SyncSpecificCacheAsync(TableName);
                }
                else
                {
                    await SyncAllCacheAsync();
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理缓存同步时出错: {ex.Message}");
                await NotifyErrorAsync($"缓存同步失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送指定表的缓存数据
        /// </summary>
        private async Task SendCacheDataAsync(string tableName)
        {
            try
            {
                UserService.发送缓存数据列表(FromSession, tableName);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception($"发送缓存数据时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 发送所有缓存数据
        /// </summary>
        private async Task SendAllCacheDataAsync()
        {
            try
            {
                // 获取所有可用的缓存表
                foreach (var tableKey in BizCacheHelper.Manager.NewTableList.Keys)
                {
                    UserService.发送缓存数据列表(FromSession, tableKey);
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception($"发送所有缓存数据时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 广播缓存更新
        /// </summary>
        private async Task BroadcastCacheUpdateAsync()
        {
            try
            {
                var sessions = frmMain.Instance.sessionListBiz.Values.ToArray();
                foreach (var session in sessions)
                {
                    if (session != FromSession) // 不发送给自己
                    {
                        try
                        {
                            var command = new CacheManagementCommand(DataPacket.Value, session, CacheOperation.Update)
                            {
                                BroadcastToAll = false // 避免递归广播
                            };
                            await command.ExecuteAsync(CancellationToken.None);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"广播缓存更新给 {session.User?.用户名} 时出错: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"广播缓存更新时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 广播缓存删除
        /// </summary>
        private async Task BroadcastCacheDeleteAsync()
        {
            // 类似于BroadcastCacheUpdateAsync的实现
            await Task.CompletedTask;
        }

        /// <summary>
        /// 广播缓存清空
        /// </summary>
        private async Task BroadcastCacheClearAsync()
        {
            // 类似于BroadcastCacheUpdateAsync的实现
            await Task.CompletedTask;
        }

        /// <summary>
        /// 同步指定缓存
        /// </summary>
        private async Task SyncSpecificCacheAsync(string tableName)
        {
            // 实现具体的缓存同步逻辑
            await Task.CompletedTask;
        }

        /// <summary>
        /// 同步所有缓存
        /// </summary>
        private async Task SyncAllCacheAsync()
        {
            // 实现所有缓存的同步逻辑
            await Task.CompletedTask;
        }

        /// <summary>
        /// 通知错误
        /// </summary>
        private async Task NotifyErrorAsync(string errorMessage)
        {
            try
            {
                if (FromSession != null)
                {
                    //var message = new MessageHandlerCommand
                    //{
                    //    MessageType = MessageType.Prompt,
                    //    MessageContent = errorMessage,
                    //    PromptType = PromptType.Error,
                    //    ReceiverSessionId = FromSession.SessionID
                    //};
                    
                  //  await message.ExecuteAsync(CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"通知错误时出错: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 缓存操作类型枚举
    /// </summary>
    public enum CacheOperation
    {
        /// <summary>
        /// 请求缓存
        /// </summary>
        Request = 1,
        
        /// <summary>
        /// 更新缓存
        /// </summary>
        Update = 2,
        
        /// <summary>
        /// 删除缓存
        /// </summary>
        Delete = 3,
        
        /// <summary>
        /// 清空缓存
        /// </summary>
        Clear = 4,
        
        /// <summary>
        /// 同步缓存
        /// </summary>
        Sync = 5
    }
}