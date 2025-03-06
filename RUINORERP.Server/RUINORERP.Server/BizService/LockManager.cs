using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace RUINORERP.Server.BizService
{

    public class LockManager
    {
        private ConcurrentDictionary<int, LockInfo> _lockDictionary = new ConcurrentDictionary<int, LockInfo>();

        public bool TryLock(int documentId, string userId)
        {
            if (_lockDictionary.TryGetValue(documentId, out var lockInfo))
            {
                if (lockInfo.IsLocked)
                {
                    return false;
                }
            }

            var newLockInfo = new LockInfo { IsLocked = true, LockedBy = userId, LockTime = DateTime.Now };
            _lockDictionary.TryUpdate(documentId, newLockInfo, null);
            return true;
        }

        public bool Unlock(int documentId, string userId)
        {
            if (_lockDictionary.TryGetValue(documentId, out var lockInfo))
            {
                if (lockInfo.IsLocked && lockInfo.LockedBy == userId)
                {
                    var newLockInfo = new LockInfo { IsLocked = false, LockedBy = null, LockTime = DateTime.Now };
                    _lockDictionary.TryUpdate(documentId, newLockInfo, lockInfo);
                    return true;
                }
            }
            return false;
        }

        public bool IsLocked(int documentId)
        {
            return _lockDictionary.TryGetValue(documentId, out var lockInfo) && lockInfo.IsLocked;
        }

        public string GetLockedBy(int documentId)
        {
            if (_lockDictionary.TryGetValue(documentId, out var lockInfo))
            {
                return lockInfo.LockedBy;
            }
            return null;
        }

        public void CheckLocks()
        {
            var timeout = TimeSpan.FromMinutes(15); // 锁超时时间
            var now = DateTime.Now;

            foreach (var pair in _lockDictionary)
            {
                var documentId = pair.Key;
                var lockInfo = pair.Value;

                if (lockInfo.IsLocked && (now - lockInfo.LockTime) > timeout)
                {
                    var newLockInfo = new LockInfo { IsLocked = false, LockedBy = null, LockTime = DateTime.Now };
                    _lockDictionary.TryUpdate(documentId, newLockInfo, lockInfo);
                }
            }
        }

        public class LockInfo
        {
            public bool IsLocked { get; set; }
            public string LockedBy { get; set; }
            public DateTime LockTime { get; set; }
        }
    }
}
