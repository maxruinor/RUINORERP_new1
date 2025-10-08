using RUINORERP.Model.CommonModel;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace RUINORERP.PacketSpec.Models
{
    [MessagePackObject]
    public class LockRequestBaseInfo
    {
        [Key(0)]
        public Guid PacketId { get; set; }
        [Key(1)]
        public long BillID { get; set; } // 单据ID
        /// <summary>
        /// 单据的信息
        /// </summary>
        [Key(2)]
        public CommBillData BillData { get; set; }

        [Key(3)]
        public string LockedUserName { get; set; } // 锁定用户的姓名

        [Key(4)]
        public long MenuID { get; set; } // 菜单ID

    }


    [MessagePackObject]
    public class LockedInfo : LockRequestBaseInfo
    {
        [Key(5)]
        public long LockedUserID { get; set; } // 锁定用户的ID

    }
    [MessagePackObject]
    public class UnLockInfo : LockRequestBaseInfo
    {
        [Key(5)]
        public long LockedUserID { get; set; } // 锁定用户的ID

    }

    /// <summary>
    /// 请求解锁信息
    /// </summary>
    [MessagePackObject]
    public class RequestUnLockInfo : LockRequestBaseInfo
    {
        [Key(5)]
        public long LockedUserID { get; set; } // 锁定用户的ID
        /// <summary>
        /// 请求人的姓名
        /// </summary>
        [Key(6)]
        public string RequestUserName { get; set; }

        [Key(7)]
        public long RequestUserID { get; set; }

    }

    /// <summary>
    /// 拒绝
    /// </summary>
    [MessagePackObject]
    public class RefuseUnLockInfo : LockRequestBaseInfo
    {
 
        /// <summary>
        /// 请求说来释放锁的ID
        /// 后面使用，比方看到锁了。但是我要修改。可以请求另一个人先释放
        /// 请求人的姓名
        /// </summary>
        [Key(5)]
        public string RequestUserName { get; set; }
        [Key(6)]
        public long RequestUserID { get; set; }
        [Key(7)]
        public string RefuseUserName { get; set; }
        [Key(8)]
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
