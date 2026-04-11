using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 单据锁定管理辅助类(轻量级封装)
    /// 
    /// 设计原则:
    /// 1. 提供简化的API,屏蔽底层复杂性
    /// 2. 统一异常处理,避免UI层重复try-catch
    /// 3. 自动处理用户信息获取
    /// 4. 保留高频使用的业务逻辑封装
    /// 
    /// 注意: 此类仅为便捷封装,核心逻辑在ClientLockManagementService中实现
    /// 
    /// 版本: 2.0.0 (简化版)
    /// 更新时间: 2025-01-28
    /// </summary>
    public static class BillLockHelper
    {
        #region 私有字段

        // 静态服务引用,避免每次调用都通过DI获取
        private static ClientLocalLockCacheService _cacheService;
        private static ClientLockManagementService _lockService;

        /// <summary>
        /// 初始化服务引用(应在应用启动时调用一次)
        /// </summary>
        public static void Initialize()
        {
            try
            {
                _cacheService = Startup.GetFromFac<ClientLocalLockCacheService>();
                _lockService = Startup.GetFromFac<ClientLockManagementService>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BillLockHelper初始化失败: {ex.Message}");
            }
        }

        #endregion

        #region 公共方法 - 简化封装

        /// <summary>
        /// 锁定单据(简化封装)
        /// </summary>
        public static async Task<LockResponse> LockBillAsync(
            long billId, string billNo, BizType bizType, long menuId,
            int timeoutMs = 10000, ILogger logger = null)
        {
            try
            {
                if (billId <= 0)
                    return LockResponseFactory.CreateFailedResponse("单据ID无效");
                if (string.IsNullOrWhiteSpace(billNo))
                    return LockResponseFactory.CreateFailedResponse("单据编号不能为空");

                var service = _lockService ?? Startup.GetFromFac<ClientLockManagementService>();
                if (service == null)
                    return LockResponseFactory.CreateFailedResponse("锁定服务未初始化");

                return await service.LockBillAsync(billId, billNo, bizType, menuId, timeoutMs);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "锁定单据异常: BillID={BillId}", billId);
                return LockResponseFactory.CreateFailedResponse($"锁定异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 解锁单据(简化封装)
        /// </summary>
        public static async Task<LockResponse> UnlockBillAsync(long billId, ILogger logger = null)
        {
            try
            {
                if (billId <= 0)
                    return LockResponseFactory.CreateFailedResponse("单据ID无效");

                var service = _lockService ?? Startup.GetFromFac<ClientLockManagementService>();
                if (service == null)
                    return LockResponseFactory.CreateFailedResponse("锁定服务未初始化");

                return await service.UnlockBillAsync(billId);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "解锁单据异常: BillID={BillId}", billId);
                return LockResponseFactory.CreateFailedResponse($"解锁异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 按业务类型批量解锁(新增)
        /// 用于打开新单据前清理同业务类型的旧单据锁
        /// </summary>
        public static async Task<LockResponse> UnlockByBizTypeAsync(
            long userId, BizType bizType, ILogger logger = null)
        {
            try
            {
                if (userId <= 0)
                    return LockResponseFactory.CreateFailedResponse("用户ID无效");

                var service = _lockService ?? Startup.GetFromFac<ClientLockManagementService>();
                if (service == null)
                    return LockResponseFactory.CreateFailedResponse("锁定服务未初始化");

                var userName = MainForm.Instance?.AppContext?.CurUserInfo?.UserInfo?.tb_employee?.Employee_Name;
                return await service.UnlockByBizTypeAsync(userId, bizType, userName);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "按业务类型解锁异常: UserId={UserId}, BizType={BizType}", userId, bizType);
                return LockResponseFactory.CreateFailedResponse($"解锁异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查单据锁定状态
        /// </summary>
        public static async Task<LockInfo> CheckLockStatusAsync(long billId, long menuId = 0, ILogger logger = null)
        {
            try
            {
                if (billId <= 0)
                    return null;

                var service = _lockService ?? Startup.GetFromFac<ClientLockManagementService>();
                var response = await service.CheckLockStatusAsync(billId, menuId);
                return response?.IsSuccess == true ? response.LockInfo : null;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "查询锁状态异常: BillID={BillId}", billId);
                return null;
            }
        }



        /// <summary>
        /// 判断单据是否可编辑
        /// 优先使用本地缓存,减少网络请求
        /// </summary>
        public static async Task<bool> IsBillEditableAsync(long billId, long currentUserId, ILogger logger = null)
        {
            try
            {
                if (billId <= 0)
                    return false;

                // 优先查缓存
                if (_cacheService != null)
                {
                    var cached = await _cacheService.GetLockInfoAsync(billId);
                    if (cached != null && !cached.IsExpired)
                    {
                        return !cached.IsLocked || cached.LockedUserId == currentUserId;
                    }
                }

                // 缓存未命中则查服务器
                var lockInfo = await CheckLockStatusAsync(billId, 0, logger);
                return lockInfo == null || !lockInfo.IsLocked || lockInfo.LockedUserId == currentUserId;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "判断可编辑性异常: BillID={BillId}", billId);
                return false; // 保守策略
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
