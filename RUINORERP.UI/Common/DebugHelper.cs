using System;
using System.Diagnostics;

namespace RUINORERP.UI.Common
{
        /// <summary>
        /// 调试辅助类 - 仅在DEBUG模式下输出到VS调试器
        /// 优点:
        /// 1. 不写入数据库日志,不影响性能
        /// 2. 只在DEBUG编译时生效,Release版本无任何性能影响
        /// 3. 直接输出到VS2022的"输出"窗口,方便调试
        /// </summary>
        public static class DebugHelper
        {
            /// <summary>
            /// 输出调试信息 - 仅在DEBUG模式下生效
            /// 与Console.WriteLine使用方式一致
            /// </summary>
            /// <param name="message">调试消息</param>
            [System.Diagnostics.Conditional("DEBUG")]
            public static void WriteLine(string message)
            {
                Debug.WriteLine(message);
            }

            /// <summary>
            /// 输出格式化调试信息 - 仅在DEBUG模式下生效
            /// 与Console.WriteLine使用方式一致
            /// </summary>
            /// <param name="format">格式化字符串</param>
            /// <param name="args">参数</param>
            [System.Diagnostics.Conditional("DEBUG")]
            public static void WriteLine(string format, params object[] args)
            {
                Debug.WriteLine(string.Format(format, args));
            }

            /// <summary>
            /// 输出调试信息 - 仅在DEBUG模式下生效
            /// </summary>
            /// <param name="message">调试消息</param>
            [System.Diagnostics.Conditional("DEBUG")]
            public static void Log(string message)
        {
            Debug.WriteLine($"[DEBUG] {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        /// <summary>
        /// 输出格式化调试信息 - 仅在DEBUG模式下生效
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogFormat(string format, params object[] args)
        {
            Debug.WriteLine($"[DEBUG] {DateTime.Now:HH:mm:ss.fff} - {string.Format(format, args)}");
        }

        /// <summary>
        /// 输出带分类的调试信息 - 仅在DEBUG模式下生效
        /// </summary>
        /// <param name="category">分类(如: SQL, UI, Network)</param>
        /// <param name="message">调试消息</param>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string category, string message)
        {
            Debug.WriteLine($"[DEBUG:{category}] {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        /// <summary>
        /// 输出带分类的格式化调试信息 - 仅在DEBUG模式下生效
        /// </summary>
        /// <param name="category">分类(如: SQL, UI, Network)</param>
        /// <param name="format">格式化字符串</param>
        /// <param name="args">参数</param>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogFormat(string category, string format, params object[] args)
        {
            Debug.WriteLine($"[DEBUG:{category}] {DateTime.Now:HH:mm:ss.fff} - {string.Format(format, args)}");
        }

        /// <summary>
        /// 输出异常信息 - 仅在DEBUG模式下生效
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="context">上下文信息</param>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogException(Exception ex, string context = "")
        {
            Debug.WriteLine($"[DEBUG:EXCEPTION] {DateTime.Now:HH:mm:ss.fff} - {context}");
            Debug.WriteLine($"  类型: {ex.GetType().Name}");
            Debug.WriteLine($"  消息: {ex.Message}");
            if (ex.StackTrace != null)
            {
                Debug.WriteLine($"  堆栈: {ex.StackTrace.Split('\n')[0]}");
            }
        }

        ///// <summary>
        ///// 输出性能计时开始
        ///// </summary>
        ///// <param name="operation">操作名称</param>
        ///// <returns>性能计时器ID</returns>
        //[System.Diagnostics.Conditional("DEBUG")]
        //public static IDisposable StartPerformance(string operation)
        //{
        //    return new PerformanceTimer(operation);
        //}

        /// <summary>
        /// 性能计时器
        /// </summary>
        private class PerformanceTimer : IDisposable
        {
            private readonly Stopwatch _stopwatch;
            private readonly string _operation;

            public PerformanceTimer(string operation)
            {
                _operation = operation;
                _stopwatch = Stopwatch.StartNew();
                Debug.WriteLine($"[DEBUG:PERF] 开始: {_operation}");
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                Debug.WriteLine($"[DEBUG:PERF] 完成: {_operation} - 耗时: {_stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }

    /// <summary>
    /// 扩展方法,方便使用
    /// </summary>
    public static class DebugExtensions
    {
        /// <summary>
        /// 输出对象内容(带缩进)
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Dump(this object obj, string title = null)
        {
            if (obj == null)
            {
                Debug.WriteLine("[DEBUG:DUMP] null");
                return;
            }

            if (!string.IsNullOrEmpty(title))
            {
                Debug.WriteLine($"[DEBUG:DUMP] {title}:");
            }
            Debug.WriteLine($"  类型: {obj.GetType().Name}");
            Debug.WriteLine($"  内容: {obj}");
        }
    }
}
