using Azure;
using Azure.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.CommService;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
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
using TransInstruction.DataModel;
using TransInstruction.DataPortal;
using static RUINORERP.Business.CommService.LockManager;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using LockCmd = RUINORERP.Model.TransModel.LockCmd;

namespace RUINORERP.Server.CommandService
{
    /// <summary>
    /// Server 锁单指令
    /// </summary>
    public class ServerLockManagerCmd : IServerCommand
    {
        public event LockChangedHandler LockChanged;
        public LockCmd lockCmd { get; set; }

        public LockRequestBaseInfo lockRequest { get; set; }
        public SessionforBiz FromSession { get; set; }
        public SessionforBiz ToSession { get; set; }
        public CmdOperation OperationType { get; set; }
        public OriginalData DataPacket { get; set; }

        public ServerLockManagerCmd(CmdOperation operation, SessionforBiz FromSession = null, SessionforBiz ToSession = null)
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
                          ResponseToClient(false, null);
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
                lockCmd = (LockCmd)ByteDataAnalysis.GetInt(gd.Two, ref index);

                //目前服务器主要是转发作用的功能
                switch (lockCmd)
                {
                    case LockCmd.LOCK:
                        string json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        JObject obj = JObject.Parse(json);
                        LockedInfo lockRequest = obj.ToObject<LockedInfo>();
                        var isLocked = frmMain.Instance.lockManager.TryLock(lockRequest.BillID,
                            lockRequest.BillData.BillNo, lockRequest.BillData.BizName, lockRequest.LockedUserID);
                        if (isLocked)
                        {
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintInfoLog($"服务器锁{lockRequest.BillID}成功");
                            }
                        }
                        //通知所有人。这个单被锁了 包含锁单本人
                        ResponseToClient(isLocked, lockRequest);


                        break;
                    case LockCmd.UNLOCK:
                        json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        obj = JObject.Parse(json);
                        UnLockInfo unLockInfo = obj.ToObject<UnLockInfo>();
                        var isUnlocked = frmMain.Instance.lockManager.Unlock(unLockInfo.BillID, unLockInfo.LockedUserID);
                        if (isUnlocked)
                        {
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintInfoLog($"服务器解锁{unLockInfo.BillData.BillNo}成功");
                            }
                            //通知所有人。这个单被锁了 包含锁单本人
                            ResponseToClient(isUnlocked, unLockInfo);
                        }

                        //OnLockChanged(lockCmd,unLockInfo.BillID, isUnlocked);

                        break;
                    case LockCmd.UnLockByBizName:
                        json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        obj = JObject.Parse(json);
                        UnLockInfo unLockInfoBizName = obj.ToObject<UnLockInfo>();
                        var isUnlockedBizName = frmMain.Instance.lockManager.RemoveLockByBizName( unLockInfoBizName.LockedUserID, unLockInfoBizName.BillData.BizName);
                        if (isUnlockedBizName)
                        {
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintInfoLog($"服务器解锁业务类型{unLockInfoBizName.BillData.BizName }成功");
                            }
                            //通知所有人。这个类型的单被解锁了， 包含锁单本人
                            ResponseToClient(isUnlockedBizName, unLockInfoBizName);
                        }

                        //OnLockChanged(lockCmd,unLockInfo.BillID, isUnlocked);

                        break;
                    case LockCmd.RequestUnLock:
                        //接收每个人的请求，但是通知谁要看谁锁定了
                        json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        obj = JObject.Parse(json);
                        RequestUnLockInfo requestUnLockInfo = obj.ToObject<RequestUnLockInfo>();
                        //转发请求
                        //OnLockChanged(lockCmd,requestUnLockInfo.BillID, true);

                        tx = new ByteBuff(100);
                        string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        tx.PushString(sendtime);
                        tx.PushInt((int)lockCmd);
                        tx.PushString(json);
                        //将来再加上提醒配置规则,或加在请求实体中
                        gd.cmd = (byte)ServerCmdEnum.复合型锁单处理;
                        gd.One = new byte[] { (byte)lockCmd };
                        gd.Two = tx.toByte();
                        //通知拥有锁的人
                        foreach (var item in frmMain.Instance.sessionListBiz.ToArray())
                        {
                            //跳过自己
                            if (FromSession != null && item.Value.SessionID == FromSession.SessionID)
                            {
                                continue;
                            }
                            //有指定目标时  其它人就不发了。
                            if (ToSession != null && item.Value.SessionID == ToSession.SessionID)
                            {
                                item.Value.AddSendData(gd);
                                break;
                            }
                            //向锁定的人发送消息 请求解锁
                            if (item.Value.User.UserID == requestUnLockInfo.LockedUserID)
                            {
                                item.Value.AddSendData(gd);
                                break;
                            }
                        }
                        break;
                    case LockCmd.RefuseUnLock:
                        json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        obj = JObject.Parse(json);
                        RefuseUnLockInfo refuseUnLockInfo = obj.ToObject<RefuseUnLockInfo>();

                        tx = new ByteBuff(100);
                        sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        tx.PushString(sendtime);
                        tx.PushInt((int)lockCmd);
                        tx.PushString(json);
                        //将来再加上提醒配置规则,或加在请求实体中
                        gd.cmd = (byte)ServerCmdEnum.复合型锁单处理;
                        gd.One = new byte[] { (byte)lockCmd };
                        gd.Two = tx.toByte();

                        //通知请求的人
                        foreach (var item in frmMain.Instance.sessionListBiz.ToArray())
                        {
                            //跳过自己
                            if (FromSession != null && item.Value.SessionID == FromSession.SessionID)
                            {
                                continue;
                            }
                            //有指定目标时  其它人就不发了。
                            if (ToSession != null && item.Value.SessionID == ToSession.SessionID)
                            {
                                item.Value.AddSendData(gd);
                                break;
                            }
                            if (item.Value.User.UserID == refuseUnLockInfo.RequestUserID)
                            {
                                item.Value.AddSendData(gd);
                                break;
                            }
                        }
                        break;
                    case LockCmd.Broadcast:
                        //广播
                        BuildDataPacketBroadcastLockStatus();
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


        /// <summary>
        /// 原数据转发broadcast
        /// </summary>
        /// <param name="request"></param>
        public void BuildDataPacketBroadcastLockStatus()
        {
            try
            {
                var tx = new ByteBuff(100);
                var sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushInt((int)LockCmd.Broadcast);
                tx.PushString(frmMain.Instance.lockManager.GetLockStatusToJson());
                //广播到在线所有人
                foreach (var item in frmMain.Instance.sessionListBiz.ToArray())
                {
                    item.Value.AddSendData((byte)ServerCmdEnum.复合型锁单处理, new byte[] { (byte)lockCmd }, tx.toByte());
                }
            }
            catch (Exception ex)
            {

            }
        }

        //回应给客户端
        public void BuildDataPacket(object request = null)
        {

        }

        public void ResponseToClient(bool isSuccess, object request = null)
        {
            ByteBuff tx = new ByteBuff(100);
            try
            {
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushInt((int)lockCmd);
                lockRequest = request as LockRequestBaseInfo;
                string json = JsonConvert.SerializeObject(lockRequest,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
               });
                tx.PushString(json);
                //发送消息数据 
                //将消息转换为一个实体先
                switch (lockCmd)
                {
                    case LockCmd.LOCK:
                    case LockCmd.UNLOCK:
                    case LockCmd.UnLockByBizName:
                        tx.PushBool(isSuccess);
                        break;
                    case LockCmd.RequestUnLock:
                        break;
                    default:
                        break;
                }
                if (ToSession == null)
                {
                    foreach (var item in frmMain.Instance.sessionListBiz.ToArray())
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
