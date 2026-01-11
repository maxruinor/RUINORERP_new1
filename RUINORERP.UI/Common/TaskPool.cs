using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// Task对象池，用于复用Task对象，减少频繁创建和销毁的开销
    /// 适用于高并发场景，如大量缓存加载请求
    /// </summary>
    public class TaskPool<TResult> : IDisposable
    {
        private readonly ConcurrentBag<TaskCompletionSource<TResult>> _taskPool;
        private readonly int _maxPoolSize;
        private readonly bool _isDynamicSize;
        private int _currentSize;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="initialSize">初始池大小</param>
        /// <param name="maxPoolSize">最大池大小</param>
        /// <param name="isDynamicSize">是否动态调整大小</param>
        public TaskPool(int initialSize = 10, int maxPoolSize = 100, bool isDynamicSize = true)
        {
            if (initialSize < 0)
                throw new ArgumentOutOfRangeException(nameof(initialSize), "初始池大小不能为负数");
            
            if (maxPoolSize < initialSize)
                throw new ArgumentOutOfRangeException(nameof(maxPoolSize), "最大池大小不能小于初始池大小");

            _taskPool = new ConcurrentBag<TaskCompletionSource<TResult>>();
            _maxPoolSize = maxPoolSize;
            _isDynamicSize = isDynamicSize;
            _currentSize = 0;

            // 初始化对象池
            for (int i = 0; i < initialSize; i++)
            {
                _taskPool.Add(CreateNewTaskCompletionSource());
            }
        }

        /// <summary>
        /// 从池中获取一个TaskCompletionSource对象
        /// </summary>
        /// <returns>TaskCompletionSource对象</returns>
        public TaskCompletionSource<TResult> Get()
        {
            if (_taskPool.TryTake(out var tcs))
            {
                return tcs;
            }

            // 如果池为空且允许动态扩展，创建新对象
            if (_isDynamicSize && _currentSize < _maxPoolSize)
            {
                Interlocked.Increment(ref _currentSize);
                return CreateNewTaskCompletionSource();
            }

            // 如果池已满且不允许动态扩展，创建临时对象
            return CreateNewTaskCompletionSource();
        }

        /// <summary>
        /// 将TaskCompletionSource对象归还到池中
        /// </summary>
        /// <param name="tcs">要归还的TaskCompletionSource对象</param>
        public void Return(TaskCompletionSource<TResult> tcs)
        {
            if (tcs == null)
                throw new ArgumentNullException(nameof(tcs));

            // 如果池未满，归还对象
            if (_taskPool.Count < _maxPoolSize)
            {
                _taskPool.Add(tcs);
            }
            else
            {
                // 如果池已满，减少当前大小计数
                Interlocked.Decrement(ref _currentSize);
            }
        }

        /// <summary>
        /// 创建新的TaskCompletionSource对象
        /// </summary>
        /// <returns>新的TaskCompletionSource对象</returns>
        private TaskCompletionSource<TResult> CreateNewTaskCompletionSource()
        {
            // 使用RunContinuationsAsynchronously选项，避免同步上下文死锁
            return new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            // 清空对象池
            while (_taskPool.TryTake(out _))
            {
                // 释放对象池中的所有对象
            }
            
            _currentSize = 0;
        }
    }
}