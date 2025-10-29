/*****************************************************************************************
 * 【过时作废文件】OBSOLETE - DEPRECATED - DO NOT USE
 * 此文件已被废弃，不再维护和使用
 * 原因：ClientCmdService目录下的所有文件都已过时，实际已排除在项目外
 * 替代方案：请使用新的命令处理机制
 * 创建日期：系统自动标识
 *****************************************************************************************/

using RUINORERP.PacketSpec.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 客户端锁管理器 - 管理客户端的锁状态
    /// </summary>
    [Obsolete("此类已过时，不再使用")]
    public class ClientLockManager
    {
        /// <summary>
        /// 锁定的单据字典
        /// </summary>
        private readonly ConcurrentDictionary<long, LockedInfo> _lockedDocuments;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ClientLockManager()
        {
            _lockedDocuments = new ConcurrentDictionary<long, LockedInfo>();
        }

        /// <summary>
        /// 尝试锁定单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="billNo">单据编号</param>
        /// <param name="bizName">业务名称</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否锁定成功</returns>
        public bool TryLock(long billId, string billNo, string bizName, long userId)
        {
            var lockInfo = new LockedInfo
            {
                BillID = billId,
                BillData = new CommBillData
                {
                    BillNo = billNo,
                    BizName = bizName
                },
                LockedUserID = userId,
                LockedUserName = GetUserNameById(userId)
            };

            return _lockedDocuments.TryAdd(billId, lockInfo);
        }

        /// <summary>
        /// 解锁单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否解锁成功</returns>
        public bool Unlock(long billId, long userId)
        {
            if (_lockedDocuments.TryGetValue(billId, out var lockInfo))
            {
                if (lockInfo.LockedUserID == userId)
                {
                    return _lockedDocuments.TryRemove(billId, out _);
                }
            }
            return false;
        }

        /// <summary>
        /// 检查单据是否被锁定
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>是否被锁定</returns>
        public bool IsLocked(long billId)
        {
            return _lockedDocuments.ContainsKey(billId);
        }

        /// <summary>
        /// 检查单据是否被当前用户锁定
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否被当前用户锁定</returns>
        public bool IsLockedByUser(long billId, long userId)
        {
            if (_lockedDocuments.TryGetValue(billId, out var lockInfo))
            {
                return lockInfo.LockedUserID == userId;
            }
            return false;
        }

        /// <summary>
        /// 获取锁定单据的用户ID
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定用户ID，如果未锁定则返回0</returns>
        public long GetLockedUserId(long billId)
        {
            if (_lockedDocuments.TryGetValue(billId, out var lockInfo))
            {
                return lockInfo.LockedUserID;
            }
            return 0;
        }

        /// <summary>
        /// 获取所有锁定的单据
        /// </summary>
        /// <returns>锁定的单据列表</returns>
        public List<LockedInfo> GetAllLockedDocuments()
        {
            return _lockedDocuments.Values.ToList();
        }

        /// <summary>
        /// 根据业务类型移除锁定
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="bizName">业务名称</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveLockByBizName(long userId, string bizName)
        {
            var keysToRemove = new List<long>();

            foreach (var kvp in _lockedDocuments)
            {
                if (kvp.Value.LockedUserID == userId && kvp.Value.BillData?.BizName == bizName)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            bool allRemoved = true;
            foreach (var key in keysToRemove)
            {
                if (!_lockedDocuments.TryRemove(key, out _))
                {
                    allRemoved = false;
                }
            }

            return allRemoved;
        }

        /// <summary>
        /// 更新锁状态通过JSON
        /// </summary>
        /// <param name="json">JSON字符串</param>
        public void UpdateLockStatusByJson(string json)
        {
            try
            {
                var lockedDocuments = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LockedInfo>>(json);
                
                // 清空当前锁状态
                _lockedDocuments.Clear();
                
                // 更新锁状态
                foreach (var lockInfo in lockedDocuments)
                {
                    _lockedDocuments.TryAdd(lockInfo.BillID, lockInfo);
                }
            }
            catch (Exception ex)
            {
                // 记录日志
                Console.WriteLine($"更新锁状态异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据用户ID获取用户名
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户名</returns>
        private string GetUserNameById(long userId)
        {
            // 这里应该从用户服务获取用户名
            // 暂时返回默认值
            return "Unknown";
        }
    }
}