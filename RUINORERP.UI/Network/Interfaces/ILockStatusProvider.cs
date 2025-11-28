using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Interfaces
{
    /// <summary>
    /// 锁状态提供者接口 - 解耦锁服务依赖
    /// 用于客户端缓存等组件避免循环依赖
    /// </summary>
    public interface ILockStatusProvider
    {
        /// <summary>
        /// 异步检查锁状态
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>锁状态响应</returns>
        Task<LockResponse> CheckLockStatusAsync(long billId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步锁定单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="timeoutMinutes">超时时间（分钟）</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>锁定响应</returns>
        Task<LockResponse> LockBillAsync(long billId, long menuId, int timeoutMinutes = 30, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步解锁单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>解锁响应</returns>
        Task<LockResponse> UnlockBillAsync(long billId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量检查锁状态
        /// </summary>
        /// <param name="billIds">单据ID列表</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>单据ID到锁定状态的映射字典</returns>
        Task<Dictionary<long, LockResponse>> BatchCheckLockStatusAsync(List<long> billIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// 刷新锁状态
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>刷新响应</returns>
        Task<LockResponse> RefreshLockAsync(long billId, long menuId, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 用户信息提供者接口 - 解耦用户信息获取
    /// </summary>
    public interface IUserInfoProvider
    {
        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns>用户ID</returns>
        long GetCurrentUserId();

        /// <summary>
        /// 获取当前用户名
        /// </summary>
        /// <returns>用户名</returns>
        string GetCurrentUserName();

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        (long userId, string userName) GetCurrentUserInfo();
    }

    /// <summary>
    /// 默认用户信息提供者实现
    /// </summary>
    public class DefaultUserInfoProvider : IUserInfoProvider
    {
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        public (long userId, string userName) GetCurrentUserInfo()
        {
            try
            {
                var userInfo = MainForm.Instance?.AppContext?.CurUserInfo?.UserInfo;
                if (userInfo != null)
                {
                    return (userInfo.User_ID, userInfo.UserName);
                }

                return (0, "Unknown");
            }
            catch (Exception)
            {
                return (0, "Unknown");
            }
        }

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns>用户ID</returns>
        public long GetCurrentUserId()
        {
            return GetCurrentUserInfo().userId;
        }

        /// <summary>
        /// 获取当前用户名
        /// </summary>
        /// <returns>用户名</returns>
        public string GetCurrentUserName()
        {
            return GetCurrentUserInfo().userName;
        }
    }
}