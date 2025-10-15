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
    /// 缓存同步命令 - 用于同步客户端与服务器之间的缓存数据
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
        /// 需要同步的缓存键列表（向后兼容）
        /// </summary>
        [Key(1002)]
        [MessagePack.IgnoreMember]
        public List<string> CacheKeys { get; set; }
        
        /// <summary>
        /// 缓存键枚举器（向后兼容）
        /// </summary>
        [Key(1003)]
        [MessagePack.IgnoreMember]
        public IAsyncEnumerable<string> CacheKeysEnumerator { get; set; }

        /// <summary>
        /// 同步模式（向后兼容）
        /// </summary>
        [Key(1004)]
        [MessagePack.IgnoreMember]
        public string SyncMode { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheCommand() : base() // 移除 Direction 参数
        {
            CacheKeys = new List<string>();
            SyncMode = "FULL"; // 默认全量同步
            // 移除: Direction = PacketDirection.Request;
            CommandIdentifier = CacheCommands.CacheRequest;
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="forceRefresh">是否强制刷新</param>
        /// <param name="clientInfo">客户端信息</param>
        public CacheCommand(string tableName, bool forceRefresh = false)
            : this()
        {
            Request = CacheRequest.Create(tableName, forceRefresh);
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
        /// 枚举缓存键
        /// </summary>
        /// <returns>缓存键的异步可枚举集合</returns>
        public IAsyncEnumerable<string> EnumerateKeys()
        {
            if (CacheKeysEnumerator != null)
            {
                return CacheKeysEnumerator;
            }
            
            if (CacheKeys != null)
            {
                return EnumerateList(CacheKeys);
            }
            
            return EmptyAsyncEnumerable<string>.Instance;
        }
        
        /// <summary>
        /// 将列表转换为异步可枚举集合
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns>异步可枚举集合</returns>
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        private async IAsyncEnumerable<T> EnumerateList<T>(List<T> list)
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        {
            foreach (var item in list)
            {
                yield return item;
            }
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
    
    /// <summary>
    /// 空的异步可枚举集合
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    internal class EmptyAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        public static readonly EmptyAsyncEnumerable<T> Instance = new EmptyAsyncEnumerable<T>();
        
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new EmptyAsyncEnumerator<T>();
        }
        
#pragma warning disable CS0693 // 类型参数与外部类型中的类型参数同名
        private class EmptyAsyncEnumerator<T> : IAsyncEnumerator<T>
#pragma warning restore CS0693 // 类型参数与外部类型中的类型参数同名
        {
            public T Current => default;
            
            public ValueTask DisposeAsync()
            {
                return new ValueTask();
            }
            
            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(false);
            }
        }

    }
}
