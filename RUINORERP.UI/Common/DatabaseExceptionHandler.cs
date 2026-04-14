using System;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 数据库异常处理助手
    /// 用于在事务外捕获并显示友好的错误提示
    /// </summary>
    public static class DatabaseExceptionHandler
    {
        /// <summary>
        /// 检查并显示唯一键约束错误
        /// 应在 catch 块中调用，用于显示友好的错误提示
        /// </summary>
        /// <param name="ex">捕获的异常（可为null，会读取MainForm.LastUniqueConstraintError）</param>
        /// <returns>是否已处理（显示了友好提示）</returns>
        public static bool TryShowUniqueConstraintError(Exception ex = null)
        {
            // 检查是否有存储的唯一键约束错误信息
            if (!string.IsNullOrEmpty(MainForm.LastUniqueConstraintError))
            {
                // 显示友好提示
                MessageBox.Show(
                    MainForm.LastUniqueConstraintError,
                    "数据验证错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                
                // 清除已显示的错误信息，避免重复显示
                string errorMsg = MainForm.LastUniqueConstraintError;
                MainForm.LastUniqueConstraintError = null;
                
                // 记录日志
                System.Diagnostics.Debug.WriteLine($"[唯一键约束提示] {errorMsg}");
                
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// 安全的执行业务操作并处理异常
        /// 自动处理唯一键约束等常见数据库错误
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <param name="successMessage">成功时的提示消息（可选）</param>
        /// <param name="showSuccessToast">是否显示成功提示</param>
        public static void ExecuteWithDbErrorHandling(
            Action action, 
            string successMessage = null,
            bool showSuccessToast = false)
        {
            try
            {
                action();
                
                // 操作成功
                if (showSuccessToast && !string.IsNullOrEmpty(successMessage))
                {
                    MessageBox.Show(
                        successMessage,
                        "操作成功",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                // 优先尝试显示唯一键约束错误
                if (TryShowUniqueConstraintError(ex))
                {
                    return; // 已显示友好提示，直接返回
                }
                
                // 其他异常：记录日志并显示通用错误
                System.Diagnostics.Debug.WriteLine($"[数据库操作异常] {ex.Message}\n{ex.StackTrace}");
                
                MessageBox.Show(
                    $"操作失败：{ex.Message}",
                    "系统错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        
        /// <summary>
        /// 异步版本的异常处理
        /// </summary>
        /// <param name="action">要执行的异步操作</param>
        /// <param name="successMessage">成功时的提示消息（可选）</param>
        /// <param name="showSuccessToast">是否显示成功提示</param>
        public static async System.Threading.Tasks.Task ExecuteWithDbErrorHandlingAsync(
            Func<System.Threading.Tasks.Task> action, 
            string successMessage = null,
            bool showSuccessToast = false)
        {
            try
            {
                await action();
                
                if (showSuccessToast && !string.IsNullOrEmpty(successMessage))
                {
                    MessageBox.Show(
                        successMessage,
                        "操作成功",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                if (TryShowUniqueConstraintError(ex))
                {
                    return;
                }
                
                System.Diagnostics.Debug.WriteLine($"[数据库操作异常] {ex.Message}\n{ex.StackTrace}");
                
                MessageBox.Show(
                    $"操作失败：{ex.Message}",
                    "系统错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        
        /// <summary>
        /// 获取友好的数据库错误提示
        /// 不会弹出对话框，只返回错误消息字符串
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns>友好的错误提示，如果无法解析则返回null</returns>
        public static string GetFriendlyErrorMessage(Exception ex)
        {
            if (ex == null)
            {
                return MainForm.LastUniqueConstraintError;
            }
            
            // 检查是否是唯一键约束错误
            if (!string.IsNullOrEmpty(MainForm.LastUniqueConstraintError))
            {
                return MainForm.LastUniqueConstraintError;
            }
            
            // 检查异常消息中是否包含唯一键约束信息
            string errorMsg = ex.Message?.ToLower() ?? "";
            if (errorMsg.Contains("unique key") || errorMsg.Contains("重复键"))
            {
                return $"数据唯一性验证失败：{ex.Message}";
            }
            
            return null;
        }
    }
}
