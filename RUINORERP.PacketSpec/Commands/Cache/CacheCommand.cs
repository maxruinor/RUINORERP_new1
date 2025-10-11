﻿using RUINORERP.PacketSpec.Models.Core;
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
        public CacheCommand() : base(PacketDirection.ClientToServer)
        {
            CacheKeys = new List<string>();
            SyncMode = "FULL"; // 默认全量同步
            Direction = PacketDirection.Request;
            CommandIdentifier = CacheCommands.CacheRequest;
            Priority = CommandPriority.Normal;
            TimeoutMs = 30000; // 设置默认超时时间为30秒
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
            CommandIdentifier = CacheCommands.CacheSync;
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
        /// 验证命令参数
        /// </summary>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.ValidateAsync(cancellationToken);
            if (!result.IsValid)
            {
                return result;
            }

            // 验证同步模式
            if (string.IsNullOrWhiteSpace(SyncMode))
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(SyncMode), "同步模式不能为空") });
            }

            // 验证缓存键列表
            if (CacheKeys == null && CacheKeysEnumerator == null)
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(CacheKeys), "缓存键列表不能为空") });
            }

            return new ValidationResult();
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
