using System;
using System.Threading.Tasks;

namespace RUINORERP.Common.Config.Extensions
{
    /// <summary>
    /// 异步锁扩展类
    /// 提供便捷的异步锁操作，支持using语句自动释放锁
    /// </summary>
    public static class SemaphoreSlimExtensions
    {
        /// <summary>
        /// 异步锁扩展方法
        /// 使用 using 语句自动释放锁
        /// </summary>
        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            return new DisposableAction(() => semaphore.Release());
        }

        /// <summary>
        /// 一次性动作类，用于实现using语句中的锁释放
        /// </summary>
        private class DisposableAction : IDisposable
        {
            private readonly Action _action;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="action">要执行的动作</param>
            public DisposableAction(Action action)
            {
                _action = action;
            }

            /// <summary>
            /// 释放资源
            /// </summary>
            public void Dispose()
            {
                _action();
            }
        }
    }
}