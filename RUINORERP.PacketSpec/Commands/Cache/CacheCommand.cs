using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums.Core; // 引用BaseCommand

namespace RUINORERP.PacketSpec.Commands.Cache
{
    /// <summary>
    /// 缓存同步命令 - 用于同步客户端与服务器之间的缓存数据
    /// </summary>
    [PacketCommand("CacheSync", CommandCategory.Cache)]
    public class CacheCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => CacheCommands.CacheSync;

        /// <summary>
        /// 需要同步的缓存键列表
        /// </summary>
        public List<string> CacheKeys { get; set; }
        
        /// <summary>
        /// 缓存键枚举器
        /// </summary>
        public IAsyncEnumerable<string> CacheKeysEnumerator { get; set; }

        /// <summary>
        /// 同步模式
        /// </summary>
        public string SyncMode { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheCommand()
        {
            CacheKeys = new List<string>();
            SyncMode = "FULL"; // 默认全量同步
            Direction = PacketDirection.Request;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheKeys">缓存键列表</param>
        /// <param name="syncMode">同步模式</param>
        public CacheCommand(List<string> cacheKeys, string syncMode = "FULL")
        {
            CacheKeys = cacheKeys ?? new List<string>();
            SyncMode = syncMode;
            Direction = PacketDirection.Request;
            TimeoutMs= 10000;
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheKeysEnumerator">缓存键枚举器</param>
        /// <param name="syncMode">同步模式</param>
        public CacheCommand(IAsyncEnumerable<string> cacheKeysEnumerator, string syncMode = "FULL")
        {
            CacheKeysEnumerator = cacheKeysEnumerator;
            SyncMode = syncMode;
            Direction = PacketDirection.Request;
            TimeoutMs = 10000;
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
        private async IAsyncEnumerable<T> EnumerateList<T>(List<T> list)
        {
            foreach (var item in list)
            {
                yield return item;
            }
        }

        /// <summary>
        /// 验证命令参数
        /// </summary>
        /// <returns>验证结果</returns>
        public override CommandValidationResult Validate()
        {
            var result = base.Validate();
            if (!result.IsValid)
            {
                return result;
            }

            // 验证同步模式
            if (string.IsNullOrWhiteSpace(SyncMode))
            {
                return CommandValidationResult.Failure("同步模式不能为空", "INVALID_SYNC_MODE");
            }

            // 验证缓存键列表
            if (CacheKeys == null && CacheKeysEnumerator == null)
            {
                return CommandValidationResult.Failure("缓存键列表不能为空", "INVALID_CACHE_KEYS");
            }

            return CommandValidationResult.Success();
        }

        /// <summary>
        /// 执行命令的核心逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        protected override Task<ResponseBase> OnExecuteAsync(CancellationToken cancellationToken)
        {
            // 缓存同步命令契约只定义数据结构，实际的业务逻辑在Handler中实现
            var result = ResponseBase.CreateSuccess("缓存同步命令构建成功")
                .WithMetadata("Data", new { SyncMode = SyncMode });
            return Task.FromResult((ResponseBase)result);
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
        
        private class EmptyAsyncEnumerator<T> : IAsyncEnumerator<T>
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
