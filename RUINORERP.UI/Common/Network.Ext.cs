﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public static class Network
    {       
        
        
        
        // 手动实现 WaitAsync 功能
        public static async Task<T> WaitAsync<T>(this Task<T> task, TimeSpan timeout, CancellationToken cancellationToken)
        {
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                var delayTask = Task.Delay(timeout, cts.Token);
                var completedTask = await Task.WhenAny(task, delayTask);

                if (completedTask == delayTask)
                {
                    throw new TimeoutException("操作超时");
                }

                cts.Cancel(); // 取消延迟任务
                return await task; // 重新抛出原始异常（如果有）
            }
        }


        // 自定义 WaitAsync 实现
        public static async Task WaitAsync(this Task task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetCanceled(), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
                await task;
            }
        }

        // 为Task<T>添加只接受CancellationToken的WaitAsync方法
        public static async Task<T> WaitAsync<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetCanceled(), tcs))
            {
                var completedTask = await Task.WhenAny(task, tcs.Task);
                if (completedTask == tcs.Task)
                {
                    throw new OperationCanceledException(cancellationToken);
                }
                return await task; // 重新抛出原始异常（如果有）
            }
        }

    }
}
