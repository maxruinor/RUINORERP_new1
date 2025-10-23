using RUINORERP.Model.CommonModel;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 移除了 MessagePack 引用

namespace RUINORERP.PacketSpec.Models
{
    
    public class LockRequestBaseInfo
    {
        public Guid PacketId { get; set; }
        public long BillID { get; set; } // 单据ID
        /// <summary>
        /// 单据的信息
        /// </summary>
        public CommBillData BillData { get; set; }

        // 移除了 [Key(3)] 标识
        public string LockedUserName { get; set; } // 锁定用户的姓名

        // 移除了 [Key(4)] 标识
        public long MenuID { get; set; } // 菜单ID

    }


    
    public class LockedInfo : LockRequestBaseInfo
    {
        // 移除了 [Key(5)] 标识
        public long LockedUserID { get; set; } // 锁定用户的ID

    }
    
    public class UnLockInfo : LockRequestBaseInfo
    {
        // 移除了 [Key(5)] 标识
        public long LockedUserID { get; set; } // 锁定用户的ID

    }

    /// <summary>
    /// 请求解锁信息
    /// </summary>
    
    public class RequestUnLockInfo : LockRequestBaseInfo
    {
        // 移除了 [Key(5)] 标识
        public long LockedUserID { get; set; } // 锁定用户的ID
        /// <summary>
        /// 请求人的姓名
        /// </summary>
        // 移除了 [Key(6)] 标识
        public string RequestUserName { get; set; }

        // 移除了 [Key(7)] 标识
        public long RequestUserID { get; set; }

    }

    /// <summary>
    /// 拒绝
    /// </summary>
    
    public class RefuseUnLockInfo : LockRequestBaseInfo
    {
 
        /// <summary>
        /// 请求说来释放锁的ID
        /// 后面使用，比方看到锁了。但是我要修改。可以请求另一个人先释放
        /// 请求人的姓名
        /// </summary>
        public string RequestUserName { get; set; }
        public long RequestUserID { get; set; }
        public string RefuseUserName { get; set; }
        public long RefuseUserID { get; set; }
    }



    public delegate void ServerLockCommandHandler(object sender, ServerLockCommandEventArgs e);
    public class ServerCommandEventArgs : EventArgs
    {
        public CommandId Command { get; set; }
        public LockRequestBaseInfo requestBaseInfo { get; set; }
    }

    /// <summary>
    /// 服务器返回的情况
    /// </summary>
    public class ServerLockCommandEventArgs : ServerCommandEventArgs
    {
        CommandId lockCmd { get; set; }
        public bool isSuccess { get; set; }
    }

}
