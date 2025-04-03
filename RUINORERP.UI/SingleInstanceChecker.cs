using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI
{
    public static class SingleInstanceChecker
    {
        private static Mutex _mutex;
        private const string GlobalMutexName = @"Global\RUINORERP.UI-elebest-1234-5678-1"; // 替换为唯一标识

        /// <summary>
        /// 检测是否已有实例在运行，返回 true 表示已有实例
        /// </summary>
        public static bool IsAlreadyRunning()
        {
            try
            {
                // 尝试创建全局互斥体
                _mutex = new Mutex(initiallyOwned: true, GlobalMutexName, out bool isNew);
                return !isNew;
            }
            catch (UnauthorizedAccessException)
            {
                // 其他实例可能正在运行
                return true;
            }
        }

        /// <summary>
        /// 释放互斥体（在程序退出时调用）
        /// </summary>
        public static void Release()
        {
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
        }
    }
}
