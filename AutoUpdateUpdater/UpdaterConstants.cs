using System;

namespace AutoUpdateUpdater
{
    /// <summary>
    /// AutoUpdateUpdater 项目统一常量定义
    /// 注意：这是独立项目的常量，与 AutoUpdate 项目解耦
    /// </summary>
    public static class UpdaterConstants
    {
        #region 进程等待相关
        
        /// <summary>
        /// 进程退出超时时间（毫秒）
        /// </summary>
        public const int ProcessExitTimeoutMs = 8000;
        
        /// <summary>
        /// 进程退出后额外等待时间（毫秒），确保资源完全释放
        /// </summary>
        public const int ExtraWaitAfterExitMs = 500;
        
        /// <summary>
        /// 文件句柄释放等待时间（毫秒）
        /// </summary>
        public const int FileHandleReleaseWaitMs = 1500;
        
        /// <summary>
        /// 强制解锁后等待时间（毫秒）
        /// </summary>
        public const int ForceUnlockWaitMs = 500;
        
        /// <summary>
        /// 轮询检查间隔（毫秒）
        /// </summary>
        public const int WaitIntervalMs = 300;
        
        /// <summary>
        /// 最大等待尝试次数
        /// </summary>
        public const int MaxWaitAttempts = 30; // 总共等待约9秒
        
        #endregion
        
        #region 重试配置
        
        /// <summary>
        /// 最大重试次数
        /// </summary>
        public const int MaxRetryAttempts = 3;
        
        /// <summary>
        /// 重试基础延迟（毫秒）
        /// </summary>
        public const int RetryDelayBaseMs = 500;
        
        /// <summary>
        /// 配置文件读取重试次数
        /// </summary>
        public const int ConfigReadRetryCount = 3;
        
        /// <summary>
        /// 配置文件读取重试延迟（毫秒）
        /// </summary>
        public const int ConfigReadRetryDelayMs = 300;
        
        /// <summary>
        /// 安全操作重试延迟基础值（毫秒）
        /// </summary>
        public const int SafeRetryDelayBaseMs = 500;
        
        #endregion
        
        #region 日志管理
        
        /// <summary>
        /// 日志保留天数
        /// </summary>
        public const int LogMaxDays = 7;
        
        /// <summary>
        /// 单个日志文件最大大小（字节）- 10MB
        /// </summary>
        public const long LogMaxSizeBytes = 10 * 1024 * 1024;
        
        /// <summary>
        /// 默认日志文件名
        /// </summary>
        public const string DefaultLogFileName = "AutoUpdateUpdaterLog.txt";
        
        #endregion
        
        #region 文件操作
        
        /// <summary>
        /// 文件操作最大重试次数
        /// </summary>
        public const int FileOperationMaxRetries = 5;
        
        /// <summary>
        /// 文件锁定检测关键词
        /// </summary>
        public static readonly string[] FileLockKeywords = new[]
        {
            "正由另一进程使用",
            "being used by another process",
            "The process cannot access the file"
        };
        
        #endregion
        
        #region 辅助方法
        
        /// <summary>
        /// 检查错误消息是否表示文件被锁定
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>如果是文件锁定错误返回true</returns>
        public static bool IsFileLockError(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
                return false;
            
            foreach (var keyword in FileLockKeywords)
            {
                // 【修复】.NET Framework 中 Contains 不支持 StringComparison，使用 IndexOf 替代
                if (errorMessage.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// 计算指数退避等待时间
        /// </summary>
        /// <param name="attempt">当前尝试次数（从1开始）</param>
        /// <param name="baseDelay">基础延迟</param>
        /// <returns>等待时间（毫秒）</returns>
        public static int CalculateExponentialBackoff(int attempt, int baseDelay = SafeRetryDelayBaseMs)
        {
            // 指数退避策略：1秒、2秒、4秒、8秒、16秒
            return (int)Math.Pow(2, attempt - 1) * baseDelay;
        }
        
        #endregion
    }
}
