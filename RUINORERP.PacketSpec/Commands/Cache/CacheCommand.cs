using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Models.Responses.Cache;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums.Core;
using FluentValidation.Results;
using MessagePack; // 引用BaseCommand

namespace RUINORERP.PacketSpec.Commands.Cache
{
    /// <summary>
    /// 缓存同步命令 - 简化版
    /// 用于同步客户端与服务器之间的缓存数据
    /// </summary>
    [PacketCommand("CacheCommand", CommandCategory.Cache)]
    [MessagePackObject(AllowPrivate = true)]
    public class CacheCommand : BaseCommand<CacheRequest, CacheResponse>
    {
        /// <summary>
        /// 缓存请求数据
        /// </summary>
        [Key(1000)]
        public CacheRequest CacheRequest
        {
            get => Request;
            set => Request = value;
        }

        /// <summary>
        /// 缓存响应数据
        /// </summary>
        [Key(1001)]
        public CacheResponse CacheResponse
        {
            get => Response;
            set => Response = value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheCommand() : base()
        {
            // 使用统一的缓存操作命令作为默认
            CommandIdentifier = CacheCommands.CacheOperation;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="operation">操作类型</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        public CacheCommand(string tableName, CacheOperation operation = CacheOperation.Get, bool forceRefresh = false)
            : this()
        {
            // 手动创建CacheRequest对象，因为Create方法不支持三个参数
            Request = new CacheRequest
            {
                TableName = tableName,
                Operation = operation,
                ForceRefresh = forceRefresh
            };
            CommandIdentifier = CacheCommands.CacheOperation;
        }
        
        /// <summary>
        /// 构造函数 - 用于订阅操作
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="subscribeAction">订阅操作类型</param>
        public CacheCommand(string tableName, SubscribeAction subscribeAction)
            : this()
        {
            Request = CacheRequest.CreateSubscriptionRequest(tableName, subscribeAction);
            CommandIdentifier = CacheCommands.CacheSubscription;
        }
        
        /// <summary>
        /// 构造函数（用于内部会话处理）
        /// </summary>
        /// <param name="session">会话对象</param>
        public CacheCommand(object session) : this()
        {
            // 仅用于兼容性，在内部处理会话相关逻辑
        }

        /// <summary>
        /// 创建缓存同步命令
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cacheData">缓存数据</param>
        /// <returns>缓存命令对象</returns>
        public static CacheCommand CreateSyncCommand(string tableName, CacheData cacheData)
        {
            var command = new CacheCommand
            {
                CommandIdentifier = CacheCommands.CacheSync,
                Request = CacheRequest.Create(tableName, false) // 使用正确的Create方法重载
            };
            
            if (command.Response == null)
            {
                command.Response = new CacheResponse();
            }
            command.Response.CacheData = cacheData;
            command.Response.TableName = tableName;
            
            return command;
        }
        
        /// <summary>
        /// 创建缓存同步命令（重载）
        /// 用于广播缓存变更到订阅的客户端
        /// </summary>
        /// <param name="response">缓存响应对象</param>
        /// <returns>缓存同步命令</returns>
        public static CacheCommand CreateSyncCommand(CacheResponse response)
        {
            var command = new CacheCommand
            {
                CommandIdentifier = CacheCommands.CacheSync,
                Request = new CacheRequest
                {
                    TableName = response.TableName,
                    Operation = CacheOperation.Get
                },
                Response = response
            };
            
            return command;
        }

        /// <summary>
        /// 验证命令数据
        /// 包含缓存请求特定的验证逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.ValidateAsync(cancellationToken);
            
            // 添加缓存特定的验证
            if (Request != null)
            {
                if (string.IsNullOrWhiteSpace(Request.TableName))
                    result.Errors.Add(new ValidationFailure(nameof(Request.TableName), "表名不能为空"));
            }
            
            return result;
        }

       
    }
    

}
