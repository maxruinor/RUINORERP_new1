using RUINORERP.UI.Network.Services;
using RUINORERP.IServices;
using System;
using System.Threading.Tasks;
using RUINORERP.UI.Models;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 锁定释放辅助类，提供自动释放锁定和资源清理功能
    /// </summary>
    public static class LockReleaseHelper
    {
        /// <summary>
        /// 获取当前用户信息，用于日志记录
        /// </summary>
        private static string CurrentUserInfo => $"用户={AppContext.CurrentUser?.UserName ?? '未知用户'}, SessionId={AppContext.CurrentUser?.SessionID ?? '未知'}";
        
        /// <summary>
        /// 异步释放锁定
        /// </summary>
        /// <param name="lockManagementService">锁定管理服务实例</param>
        /// <param name="logger">日志记录器实例</param>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="operationType">操作类型，如"表单关闭"、"单据切换"等</param>
        public static async Task ReleaseLockAsync(ILockManagementService lockManagementService, ILogger logger, long billId, string menuId, string operationType = "自动释放")
        {
            if (lockManagementService == null || billId <= 0 || string.IsNullOrEmpty(menuId))
            {
                logger?.Debug($"锁定释放参数无效: BillId={billId}, MenuId={menuId}");
                return;
            }

            // 操作前记录日志，形成完整链路
            logger?.Debug($"[锁定操作开始] {operationType} - {CurrentUserInfo}, BillId={billId}, MenuId={menuId}");
            
            try
            {
                await lockManagementService.UnlockBillAsync(billId, menuId);
                logger?.Info($"[锁定操作成功] {operationType} - {CurrentUserInfo}, BillId={billId}, MenuId={menuId}");
            }
            catch (Exception ex)
            {
                logger?.Error($"[锁定操作失败] {operationType} - {CurrentUserInfo}, BillId={billId}, MenuId={menuId}, 错误: {ex.Message}", ex);
            }
            finally
            {
                logger?.Debug($"[锁定操作结束] {operationType} - {CurrentUserInfo}, BillId={billId}, MenuId={menuId}");
            }
        }

        /// <summary>
        /// 在表单关闭时释放锁定（非阻塞方式）
        /// </summary>
        /// <param name="lockManagementService">锁定管理服务实例</param>
        /// <param name="logger">日志记录器实例</param>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        public static void ReleaseLockOnFormClose(ILockManagementService lockManagementService, ILogger logger, long billId, string menuId)
        {
            // 在后台异步执行，不阻塞UI线程
            Task.Run(async () => await ReleaseLockAsync(lockManagementService, logger, billId, menuId, "表单关闭"));
        }

        /// <summary>
        /// 在异常情况下清理锁定资源
        /// </summary>
        /// <param name="lockManagementService">锁定管理服务实例</param>
        /// <param name="logger">日志记录器实例</param>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="ex">捕获的异常</param>
        public static void CleanupLockOnException(ILockManagementService lockManagementService, ILogger logger, long billId, string menuId, Exception ex)
        {
            try
            {
                logger?.Error($"[异常处理] {CurrentUserInfo} - 尝试清理锁定资源: BillId={billId}, MenuId={menuId}, 异常类型={ex.GetType().Name}, 异常消息={ex.Message}", ex);
                
                // 在异常处理中，我们不能阻塞主线程，所以使用Task.Run在后台执行
                if (billId > 0 && !string.IsNullOrEmpty(menuId))
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            await ReleaseLockAsync(lockManagementService, logger, billId, menuId, "异常清理");
                            logger?.Warn($"[异常处理成功] {CurrentUserInfo} - 锁定资源清理完成: BillId={billId}");
                        }
                        catch (Exception cleanupEx)
                        {
                            // 在异常处理中再次发生异常时，只记录日志，不抛出
                            logger?.Error($"[异常处理失败] {CurrentUserInfo} - 锁定资源清理时发生异常: {cleanupEx.Message}", cleanupEx);
                        }
                    });
                }
            }
            catch (Exception)
            {
                // 在最外层捕获所有可能的异常，确保不会干扰主异常处理流程
                // 这里不记录日志，因为logger可能也不可用
            }
        }
        
        /// <summary>
        /// 记录锁定操作审计日志（用于管理员强制解锁等关键操作）
        /// </summary>
        /// <param name="logger">日志记录器实例</param>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="lockOwner">锁定拥有者</param>
        /// <param name="operationType">操作类型</param>
        public static void LogLockAudit(ILogger logger, long billId, string menuId, string lockOwner, string operationType)
        {
            try
            {
                logger?.Info($"[锁定审计日志] {operationType} - {CurrentUserInfo}, 操作对象: BillId={billId}, MenuId={menuId}, 锁定拥有者={lockOwner}");
            }
            catch (Exception)
            {
                // 审计日志记录本身的异常不应该影响主流程
            }
        }
    }
}