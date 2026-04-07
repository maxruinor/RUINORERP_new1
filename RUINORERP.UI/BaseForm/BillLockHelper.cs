using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 单据锁定管理辅助类
    /// 提供简化的锁管理方法,用于在单据编辑基类中集成
    /// 
    /// 设计原则:
    /// 1. 简化API,减少调用复杂度
    /// 2. 统一异常处理和日志记录
    /// 3. 自动处理用户信息和会话
    /// 4. 提供友好的错误提示
    /// 
    /// 版本: 1.0.01
    /// 创建时间: 2025-01-25
    /// v2.1.1: 添加静态服务引用,避免频繁DI获取
    /// </summary>
    public static class BillLockHelper
    {
        #region 私有字段

        // 静态缓存服务引用,避免每次调用都通过DI获取
        private static Network.Services.ClientLocalLockCacheService _cacheService;
        private static Network.Services.ClientLockManagementService _lockService;

        /// <summary>
        /// 初始化服务引用(应在应用启动时调用一次)
        /// </summary>
        public static void Initialize()
        {
            try
            {
                _cacheService = Startup.GetFromFac<Network.Services.ClientLocalLockCacheService>();
                _lockService = Startup.GetFromFac<Network.Services.ClientLockManagementService>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BillLockHelper初始化失败: {ex.Message}");
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 尝试锁定单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="billNo">单据编号</param>
        /// <param name="bizType">业务类型</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="timeoutMinutes">超时时间(毫秒),默认10000ms</param>
        /// <param name="logger">日志记录器(可选)</param>
        /// <returns>锁定响应</returns>
        public static async Task<LockResponse> TryLockBillAsync(
            long billId,
            string billNo,
            BizType bizType,
            long menuId,
            int timeoutMinutes = 10000,
            ILogger logger = null)
        {
            try
            {
                if (billId <= 0)
                {
                    logger?.LogWarning("锁定单据失败: 单据ID无效 {BillId}", billId);
                    return LockResponseFactory.CreateFailedResponse("单据ID无效");
                }

                if (string.IsNullOrWhiteSpace(billNo))
                {
                    logger?.LogWarning("锁定单据失败: 单据编号为空");
                    return LockResponseFactory.CreateFailedResponse("单据编号不能为空");
                }

                // 获取锁管理服务 - 使用静态引用避免DI开销
                var lockService = _lockService ?? Startup.GetFromFac<Network.Services.ClientLockManagementService>();
                if (lockService == null)
                {
                    logger?.LogError("锁定单据失败: 锁管理服务未初始化");
                    return LockResponseFactory.CreateFailedResponse("锁管理服务未初始化");
                }

                // 执行锁定
                logger?.LogDebug("开始锁定单据: BillID={BillId}, BillNo={BillNo}", billId, billNo);
                var response = await lockService.LockBillAsync(billId, billNo, bizType, menuId, timeoutMinutes);

                if (response.IsSuccess)
                {
                    logger?.LogDebug("锁定单据成功: BillID={BillId}", billId);
                }
                else
                {
                    logger?.LogWarning("锁定单据失败: BillID={BillId}, 原因: {Message}", billId, response.Message);
                }

                return response;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "锁定单据时发生异常: BillID={BillId}", billId);
                return LockResponseFactory.CreateFailedResponse($"锁定异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 解锁单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="logger">日志记录器(可选)</param>
        /// <returns>解锁响应</returns>
        public static async Task<LockResponse> UnlockBillAsync(
            long billId,
            ILogger logger = null)
        {
            try
            {
                if (billId <= 0)
                {
                    logger?.LogWarning("解锁单据失败: 单据ID无效 {BillId}", billId);
                    return LockResponseFactory.CreateFailedResponse("单据ID无效");
                }

                // 获取锁管理服务 - 使用静态引用避免DI开销
                var lockService = _lockService ?? Startup.GetFromFac<Network.Services.ClientLockManagementService>();
                if (lockService == null)
                {
                    logger?.LogError("解锁单据失败: 锁管理服务未初始化");
                    return LockResponseFactory.CreateFailedResponse("锁管理服务未初始化");
                }

                // 执行解锁
                logger?.LogDebug("开始解锁单据: BillID={BillId}", billId);
                var response = await lockService.UnlockBillAsync(billId);

                if (response.IsSuccess)
                {
                    logger?.LogDebug("解锁单据成功: BillID={BillId}", billId);
                }
                else
                {
                    logger?.LogDebug("解锁单据失败: BillID={BillId}, 原因: {Message}", billId, response.Message);
                }

                return response;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "解锁单据时发生异常: BillID={BillId}", billId);
                return LockResponseFactory.CreateFailedResponse($"解锁异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查单据锁定状态
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID(可选)</param>
        /// <param name="logger">日志记录器(可选)</param>
        /// <returns>锁信息,如果查询失败返回null</returns>
        public static async Task<LockInfo> CheckBillLockStatusAsync(
            long billId,
            long menuId = 0,
            ILogger logger = null)
        {
            try
            {
                if (billId <= 0)
                {
                    logger?.LogWarning("检查锁状态失败: 单据ID无效 {BillId}", billId);
                    return null;
                }

                // 获取锁管理服务 - 使用静态引用避免DI开销
                var lockService = _lockService ?? Startup.GetFromFac<Network.Services.ClientLockManagementService>();
                if (lockService == null)
                {
                    logger?.LogError("检查锁状态失败: 锁管理服务未初始化");
                    return null;
                }

                // 执行查询
                logger?.LogDebug("检查单据锁状态: BillID={BillId}", billId);
                var response = await lockService.CheckLockStatusAsync(billId, menuId);

                if (response.IsSuccess && response.LockInfo != null)
                {
                    return response.LockInfo;
                }

                logger?.LogWarning("检查锁状态失败: BillID={BillId}, 原因: {Message}", billId, response.Message);
                return null;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "检查单据锁状态时发生异常: BillID={BillId}", billId);
                return null;
            }
        }

        /// <summary>
        /// 刷新单据锁定状态
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="logger">日志记录器(可选)</param>
        /// <returns>刷新响应</returns>
        public static async Task<LockResponse> RefreshBillLockAsync(
            long billId,
            long menuId,
            ILogger logger = null)
        {
            try
            {
                if (billId <= 0)
                {
                    logger?.LogWarning("刷新锁状态失败: 单据ID无效 {BillId}", billId);
                    return LockResponseFactory.CreateFailedResponse("单据ID无效");
                }

                // 获取锁管理服务 - 使用静态引用避免DI开销
                var lockService = _lockService ?? Startup.GetFromFac<Network.Services.ClientLockManagementService>();
                if (lockService == null)
                {
                    logger?.LogError("刷新锁状态失败: 锁管理服务未初始化");
                    return LockResponseFactory.CreateFailedResponse("锁管理服务未初始化");
                }

                // 执行刷新
                logger?.LogDebug("刷新单据锁状态: BillID={BillId}", billId);
                var response = await lockService.RefreshLockAsync(billId, menuId);

                if (response.IsSuccess)
                {
                    logger?.LogDebug("刷新锁状态成功: BillID={BillId}", billId);
                }
                else
                {
                    logger?.LogWarning("刷新锁状态失败: BillID={BillId}, 原因: {Message}", billId, response.Message);
                }

                return response;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "刷新单据锁状态时发生异常: BillID={BillId}", billId);
                return LockResponseFactory.CreateFailedResponse($"刷新异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 批量检查锁状态（委托给服务层）
        /// 使用ClientLockManagementService.BatchCheckLockStatusAsync实现
        /// </summary>
        /// <param name="billIds">单据ID数组</param>
        /// <param name="logger">日志记录器(可选)</param>
        /// <returns>锁状态字典，key为单据ID，value为锁信息</returns>
        public static async Task<Dictionary<long, LockInfo>> CheckBillLockStatusBatchAsync(
            long[] billIds,
            ILogger logger = null)
        {
            try
            {
                if (billIds == null || billIds.Length == 0)
                {
                    return new Dictionary<long, LockInfo>();
                }

                // 限制批量查询大小
                var limitedIds = billIds.Take(100).ToArray();
                
                logger?.LogDebug("批量检查锁状态: 单据数量={Count}", limitedIds.Length);
                
                // 获取锁管理服务
                var lockService = Startup.GetFromFac<Network.Services.ClientLockManagementService>();
                if (lockService == null)
                {
                    logger?.LogError("批量检查锁状态失败: 锁管理服务未初始化");
                    return new Dictionary<long, LockInfo>();
                }

                // 委托给服务层批量处理
                var responses = await lockService.BatchCheckLockStatusAsync(limitedIds.ToList());
                
                // 转换为字典返回
                var result = new Dictionary<long, LockInfo>();
                foreach (var kvp in responses)
                {
                    if (kvp.Value.IsSuccess && kvp.Value.LockInfo != null)
                    {
                        result[kvp.Key] = kvp.Value.LockInfo;
                    }
                }

                logger?.LogDebug("批量检查锁状态完成: 成功数量={SuccessCount}, 总数={TotalCount}", 
                    result.Count, limitedIds.Length);

                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "批量检查锁状态时发生异常: BillIDs={BillIds}", string.Join(",", billIds ?? Array.Empty<long>()));
                return new Dictionary<long, LockInfo>();
            }
        }

        /// <summary>
        /// 判断单据是否可编辑
        /// 优化：优先使用本地缓存，减少网络请求
        /// v2.1.1: 使用静态服务引用,避免频繁DI获取
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="currentUserId">当前用户ID</param>
        /// <param name="logger">日志记录器(可选)</param>
        /// <returns>true表示可编辑,false表示不可编辑</returns>
        public static async Task<bool> IsBillEditableAsync(
            long billId,
            long currentUserId,
            ILogger logger = null)
        {
            try
            {
                LockInfo lockInfo = null;

                // 优先检查本地缓存 - 使用静态引用避免DI开销
                if (_cacheService != null)
                {
                    var cachedInfo = await _cacheService.GetLockInfoAsync(billId);
                    if (cachedInfo != null && !cachedInfo.IsExpired)
                    {
                        lockInfo = cachedInfo;
                        logger?.LogDebug("使用本地缓存判断编辑权限: BillID={BillId}", billId);
                    }
                }

                // 缓存未命中或已过期，则查询网络
                if (lockInfo == null)
                {
                    lockInfo = await CheckBillLockStatusAsync(billId, 0, logger);
                }

                if (lockInfo == null)
                {
                    // 未锁定,可编辑
                    return true;
                }

                if (!lockInfo.IsLocked)
                {
                    // 未锁定,可编辑
                    return true;
                }

                if (lockInfo.LockedUserId == currentUserId)
                {
                    // 当前用户锁定的,可编辑
                    logger?.LogDebug("单据由当前用户锁定,可编辑: BillID={BillId}, UserID={UserID}", billId, currentUserId);
                    return true;
                }

                // 其他用户锁定,不可编辑
                logger?.LogDebug("单据由其他用户锁定,不可编辑: BillID={BillId}, LockedUserID={LockedUserID}, CurrentUserID={CurrentUserID}",
                    billId, lockInfo.LockedUserId, currentUserId);
                return false;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "判断单据可编辑性时发生异常: BillID={BillId}", billId);
                // 发生异常时,保守返回false
                return false;
            }
        }

        /// <summary>
        /// 格式化锁定信息用于显示
        /// v2.1.1: 委托给公共工具类,避免代码重复
        /// </summary>
        /// <param name="lockInfo">锁信息</param>
        /// <returns>格式化的锁定信息字符串</returns>
        public static string FormatLockInfoMessage(LockInfo lockInfo)
        {
            return LockInfoFormatter.FormatLockInfoMessage(lockInfo);
        }

        /// <summary>
        /// 计算锁定时长
        /// v2.1.1: 委托给公共工具类,避免代码重复
        /// </summary>
        /// <param name="lockTime">锁定时间</param>
        /// <returns>格式化的锁定时长字符串</returns>
        public static string CalculateLockDuration(DateTime lockTime)
        {
            return LockInfoFormatter.CalculateLockDuration(lockTime);
        }

        #endregion
    }
}
