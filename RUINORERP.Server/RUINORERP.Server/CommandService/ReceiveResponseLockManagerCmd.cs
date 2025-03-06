using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.CommService;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.ServerSession;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransInstruction;
using TransInstruction.CommandService;
using TransInstruction.DataPortal;
using static RUINORERP.Business.CommService.LockManager;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using LockCmd = RUINORERP.Model.TransModel.LockCmd;

namespace RUINORERP.Server.CommandService
{
    /// <summary>
    /// Server 锁单指令
    /// </summary>
    public class ReceiveResponseLockManagerCmd : IServerCommand
    {
        public event LockChangedHandler LockChanged;
        public LockCmd lockCmd { get; set; }
        public LockRequestInfo lockRequest { get; set; }
        public SessionforBiz FromSession { get; set; }
        public SessionforBiz ToSession { get; set; }
        public CmdOperation OperationType { get; set; }
        public OriginalData DataPacket { get; set; }

        public ReceiveResponseLockManagerCmd(CmdOperation operation, SessionforBiz FromSession = null, SessionforBiz ToSession = null)
        {
            this.OperationType = operation;
            this.FromSession = FromSession;
            this.ToSession = ToSession;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // 添加产品逻辑
            await Task.Run(
                () =>
              {
                  switch (OperationType)
                  {
                      case CmdOperation.Receive:
                          AnalyzeDataPacket(DataPacket, FromSession);
                          break;
                      case CmdOperation.Send:
                          BuildDataPacket(lockRequest);
                          break;
                      default:
                          break;
                  }
              }
                ,

                cancellationToken
                );
        }






        public bool AnalyzeDataPacket(OriginalData gd, SessionforBiz FromSession)
        {
            bool rs = false;
            try
            {
                var tx = new ByteBuff(100);
                int index = 0;
                //当前
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                LockCmd lockCmd = (LockCmd)ByteDataAnalysis.GetInt(gd.Two, ref index);
                if (lockCmd == LockCmd.Broadcast)
                {
                    //广播
                    BuildDataPacketforward();
                    return true;
                }
                long BillID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                if (lockRequest == null)
                {
                    lockRequest = new LockRequestInfo();
                }
                lockRequest.BillID = BillID;
                //目前服务器主要是转发作用的功能
                switch (lockCmd)
                {
                    case LockCmd.LOCK:
                    case LockCmd.UNLOCK:
                        long LockedUserID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                        //如果是请求解锁就是请求人。否则就是锁定人
                        string UserName = ByteDataAnalysis.GetString(gd.Two, ref index);
                        int BillBizType = ByteDataAnalysis.GetInt(gd.Two, ref index);
                        long MenuID = ByteDataAnalysis.GetInt64(gd.Two, ref index);

                        lockRequest.LockedUserName = UserName;
                        lockRequest.BillBizType = BillBizType;
                        lockRequest.MenuID = MenuID;
                        lockRequest.LockedUserID = LockedUserID;

                        if (lockCmd == LockCmd.UNLOCK)
                        {
                            var isUnlocked = frmMain.Instance.lockManager.Unlock(BillID, LockedUserID);
                            if (isUnlocked)
                            {
                                //通知所有人。这个单被锁了 包含锁单本人
                                BuildDataPacket(lockRequest);
                                OnLockChanged(BillID, false, LockedUserID);
                            }
                        }
                        else
                        {
                            var isLocked = frmMain.Instance.lockManager.TryLock(BillID, LockedUserID);
                            if (isLocked)
                            {
                                //通知所有人。这个单被锁了 包含锁单本人
                                BuildDataPacket(lockRequest);
                                OnLockChanged(BillID, true, LockedUserID);
                            }
                        }
                        break;

                    case LockCmd.RequestReleaseLock:
                        //有人申请解锁 通知其它所有人[所有人收到判断是不是自己 是的才弹出窗口]。申请人没有锁不用通知
                         LockedUserID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                        //如果是请求解锁就是请求人。否则就是锁定人
                         UserName = ByteDataAnalysis.GetString(gd.Two, ref index);
                         BillBizType = ByteDataAnalysis.GetInt(gd.Two, ref index);
                        MenuID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                      
                        lockRequest.RequestReleaseUserName = UserName;
                        lockRequest.BillBizType = BillBizType;
                        lockRequest.MenuID = MenuID;
                        lockRequest.LockedUserID = LockedUserID;

                        tx = new ByteBuff(100);
                        string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        tx.PushString(sendtime);
                        tx.PushInt((int)lockCmd);
                        tx.PushInt64(BillID);
                        tx.PushInt64(lockRequest.LockedUserID);
                        tx.PushString(lockRequest.RequestReleaseUserName);
                        tx.PushInt((int)lockRequest.BillBizType);
                        tx.PushInt64(lockRequest.MenuID);

                        //将来再加上提醒配置规则,或加在请求实体中
                        //gd.cmd = (byte)ServerCmdEnum.复合型锁单处理;
                        //gd.One = new byte[] { (byte)lockCmd };
                        //gd.Two = tx.toByte();

                        foreach (var item in frmMain.Instance.sessionListBiz)
                        {
                            //跳过自己
                            if (FromSession != null && item.Value.SessionID == FromSession.SessionID)
                            {
                                continue;
                            }
                            //有指定目标时  其它人就不发了。
                            if (ToSession != null && item.Value.SessionID == ToSession.SessionID)
                            {
                                item.Value.AddSendData((byte)ServerCmdEnum.复合型锁单处理, new byte[] { (byte)lockCmd }, tx.toByte());
                                break;
                            }
                            item.Value.AddSendData((byte)ServerCmdEnum.复合型锁单处理, new byte[] { (byte)lockCmd }, tx.toByte());
                        }
                        break;

                    case LockCmd.Broadcast:
                        break;

                    default:
                        break;
                }



            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("接收请求:" + ex.Message);
            }
            return rs;
        }


        protected virtual void OnLockChanged(long documentId, bool isLocked, long lockedBy)
        {
            LockChanged?.Invoke(this, new LockChangedEventArgs(documentId, isLocked, lockedBy));
        }


        /// <summary>
        /// 原数据转发broadcast
        /// </summary>
        /// <param name="request"></param>
        public void BuildDataPacketforward()
        {
            try
            {
                var tx = new ByteBuff(100);
                var sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushInt((int)LockCmd.Broadcast);
                tx.PushString(frmMain.Instance.lockManager.GetLockStatusToJson());

                //将来再加上提醒配置规则,或加在请求实体中
                //gd.cmd = (byte)ServerCmdEnum.复合型锁单处理;
                //gd.One = new byte[] { (byte)lockCmd };
                //gd.Two = tx.toByte();
                //谁请求就给谁
                foreach (var item in frmMain.Instance.sessionListBiz)
                {
                    if (FromSession != null && item.Value.SessionID == FromSession.SessionID)
                    {
                        item.Value.AddSendData((byte)ServerCmdEnum.复合型锁单处理, new byte[] { (byte)lockCmd }, tx.toByte());
                        break;
                    }
                }

            }
            catch (Exception ex)
            {

            }


        }

        //回应给客户端
        public void BuildDataPacket(object request = null)
        {
            ByteBuff tx = new ByteBuff(100);
            try
            {
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushInt((int)lockCmd);
                //发送消息数据 
                //将消息转换为一个实体先
                switch (lockCmd)
                {
                    case LockCmd.LOCK:
                    case LockCmd.UNLOCK:
                        tx.PushInt64(lockRequest.BillID);
                        tx.PushInt64(lockRequest.LockedUserID);
                        tx.PushString(lockRequest.LockedUserName);
                        tx.PushInt((int)lockRequest.BillBizType);
                        tx.PushInt64(lockRequest.MenuID);
                        break;
                    default:
                        break;
                }
                if (ToSession == null)
                {
                    foreach (var item in frmMain.Instance.sessionListBiz)
                    {
                        item.Value.AddSendData((byte)ServerCmdEnum.复合型锁单处理, new byte[] { (byte)lockCmd }, tx.toByte());
                    }
                }
                else
                {
                    ToSession.AddSendData((byte)ServerCmdEnum.复合型锁单处理, new byte[] { (byte)lockCmd }, tx.toByte());
                }
            }
            catch (Exception ex)
            {

            }


        }
    }
}
