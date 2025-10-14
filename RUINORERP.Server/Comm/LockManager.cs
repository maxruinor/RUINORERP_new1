using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using RUINORERP.Model.TransModel;
using Newtonsoft.Json;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Models;
using RUINORERP.Business.CommService;
using RUINORERP.Extensions.Middlewares;

namespace RUINORERP.Server.Comm
{
    /// <summary>
    /// 服务器和客户端都维护一份列表，再通过事件同步
    /// </summary>
    public class LockManager
    {

        private ConcurrentDictionary<long, LockInfo> _lockDictionary = new ConcurrentDictionary<long, LockInfo>();

        public bool TryLock(long documentId, string billNo, string BizName, long userId)
        {
            // 检查单据是否已经被锁定
            if (_lockDictionary.TryGetValue(documentId, out var lockInfo))
            {
                if (lockInfo.IsLocked)
                {
                    // 如果单据已经被锁定，返回 false，表示加锁失败
                    return false;
                }
            }

            // 创建一个新的锁记录，标记单据为已锁定，并记录锁定用户和时间
            var newLockInfo = new LockInfo { BillID = documentId, BillNo = billNo, BizName = BizName, IsLocked = true, LockedByID = userId, LockTime = DateTime.Now };

            // 如果字典中没有该单据的锁记录，则添加新的记录
            if (!_lockDictionary.ContainsKey(documentId))
            {
                _lockDictionary.TryAdd(documentId, newLockInfo);
            }
            else
            {
                // 更新锁记录到字典中
                _lockDictionary.TryUpdate(documentId, newLockInfo, null);
            }

            // 返回 true，表示加锁成功
            return true;
        }

        public bool Unlock(long documentId, long userId)
        {
            // 检查单据是否被锁定，并且锁定用户是否是当前用户
            if (_lockDictionary.TryGetValue(documentId, out var lockInfo))
            {
                if (lockInfo.IsLocked && lockInfo.LockedByID == userId)
                {
                    // 如果更新成功，检查是否需要删除记录
                    return _lockDictionary.TryRemove(documentId, out _);

                    /*
                    // 创建一个新的锁记录，标记单据为未锁定
                    var newLockInfo = new LockInfo {IsLocked = false, LockedByID = 0, LockTime = DateTime.Now };

                    // 更新锁记录到字典中
                    if (_lockDictionary.TryUpdate(documentId, newLockInfo, lockInfo))
                    {
                        // 如果更新成功，检查是否需要删除记录
                        if (!newLockInfo.IsLocked)
                        {
                            _lockDictionary.TryRemove(documentId, out _);
                        }
                        // 返回 true，表示解锁成功
                        return true;
                    }
                    */
                }
            }

            // 返回 false，表示解锁失败
            return false;
        }

        public bool IsLocked(long documentId)
        {
            // 检查单据是否被锁定
            return _lockDictionary.TryGetValue(documentId, out var lockInfo) && lockInfo.IsLocked;
        }

        /// <summary>
        /// 获取锁定指定单据ID的用户的ID
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public long GetLockedBy(long documentId)
        {
            // 获取锁定用户的 ID
            if (_lockDictionary.TryGetValue(documentId, out var lockInfo))
            {
                return lockInfo.LockedByID;
            }
            return 0;
        }
        public bool RemoveLock(long documentId)
        {
            // 删除锁记录
            return _lockDictionary.TryRemove(documentId, out _);
        }

        /// <summary>
        /// 如果谁断开连接了。他相关的锁定全释放
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public void RemoveLockByUserID(long UserID)
        {
            var keysToRemove = new List<long>();
            foreach (var pair in _lockDictionary)
            {
                if (pair.Value.LockedByID == UserID)
                {
                    keysToRemove.Add(pair.Key);
                }
            }
            foreach (var documentId in keysToRemove)
            {
                _lockDictionary.TryRemove(documentId, out _);
            }
        }

        /// <summary>
        /// 一个业务一个人只会锁定一个单据。
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public bool RemoveLockByBizName(long UserID, string BizName)
        {
            bool rs = false;
            var keysToRemove = new List<long>();
            foreach (var pair in _lockDictionary)
            {
                if (pair.Value.LockedByID == UserID && pair.Value.BizName == BizName)
                {
                    keysToRemove.Add(pair.Key);
                }
            }
            foreach (var documentId in keysToRemove)
            {
                rs = _lockDictionary.TryRemove(documentId, out _);
            }
            return rs;
        }
        public void CheckLocks()
        {
            // 检查并释放超时的锁
            var timeout = TimeSpan.FromMinutes(120); // 锁超时时间
            var now = DateTime.Now;

            var keysToRemove = new List<long>();

            foreach (var pair in _lockDictionary)
            {
                var documentId = pair.Key;
                var lockInfo = pair.Value;

                if (lockInfo.IsLocked && (now - lockInfo.LockTime) > timeout)
                {
                    //// 创建一个新的锁记录，标记单据为未锁定
                    //var newLockInfo = new LockInfo { IsLocked = false, LockedByID = 0, LockTime = DateTime.Now };

                    //// 更新锁记录到字典中
                    //if (_lockDictionary.TryUpdate(documentId, newLockInfo, lockInfo))
                    //{
                    //    // 如果更新成功，检查是否需要删除记录
                    //    if (!newLockInfo.IsLocked)
                    //    {
                    keysToRemove.Add(documentId);
                    //    }
                    //}
                }
            }

            // 删除超时的锁记录
            foreach (var documentId in keysToRemove)
            {
                _lockDictionary.TryRemove(documentId, out _);
            }
        }

        //public void UpdateLockStatus(long documentId, bool isLocked, long lockedBy)
        //{
        //    var lockInfo = new LockInfo
        //    {
        //        IsLocked = isLocked,
        //        LockedByID = lockedBy,
        //        LockTime = DateTime.Now
        //    };
        //    _lockDictionary[documentId] = lockInfo;
        //}

        public LockInfo GetLockStatus(long documentId)
        {
            return _lockDictionary.TryGetValue(documentId, out var lockInfo) ? lockInfo : null;
        }

        public string GetLockStatusToJson()
        {
            //发送缓存数据
            string json = JsonConvert.SerializeObject(_lockDictionary,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
               });

            return json;
        }



        public void UpdateLockStatusByJson(string lockDictionaryJson)
        {
            // 反序列化 JSON 字符串为 ConcurrentDictionary
            var lockDictionary = JsonConvert.DeserializeObject<ConcurrentDictionary<long, LockInfo>>(lockDictionaryJson);
            // 输出反序列化后的数据
            //foreach (var item in lockDictionary)
            //{
            //    Console.WriteLine($"Key: {item.Key}, Value: {item.Value.IsLocked}, LockedBy: {item.Value.LockedBy}, LockTime: {item.Value.LockTime}");
            //}
            _lockDictionary = lockDictionary;
        }
        public List<LockInfo> GetLockItems()
        {
            return _lockDictionary.Values.ToList();
        }
        public int GetLockItemCount()
        {
            return _lockDictionary.Count;
        }
    }

    public delegate void LockChangedHandler(object sender, ServerLockCommandEventArgs e);


    public class LockInfo
    {


        /// <summary>
        /// 业务类型
        /// 一个业务类型同时会有锁定一个单
        /// </summary>
        public string BizName { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 单号ID
        /// </summary>
        public long BillID { get; set; }

        public bool IsLocked { get; set; } // 是否锁定
        public long LockedByID { get; set; } // 锁定用户

        private string _LockedByName;
        public string LockedByName
        {
            get
            {
                if (string.IsNullOrEmpty(_LockedByName))
                {
                    tb_UserInfo userInfo = MyCacheManager.Instance.GetEntity<tb_UserInfo>(LockedByID);
                    if (userInfo != null)
                    {
                        _LockedByName = userInfo.UserName;
                    }
                }
                return _LockedByName;
            }
            set
            {
                _LockedByName = value;
                if (string.IsNullOrEmpty(_LockedByName))
                {
                    tb_UserInfo userInfo = MyCacheManager.Instance.GetEntity<tb_UserInfo>(LockedByID);
                    if (userInfo != null)
                    {
                        _LockedByName = userInfo.UserName;
                    }
                }
            }
        }
        public DateTime LockTime { get; set; } // 锁定时间
    }



}