using Azure;
using Azure.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Server.Comm;
using RUINORERP.Server.Network.Interfaces.Services;
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
using RUINORERP.PacketSpec.Models;
using static RUINORERP.Server.Comm.LockManager;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Enums;

namespace RUINORERP.Server.CommandService
{
    /// <summary>
    /// Server 锁单指令
    /// </summary>
    public class ServerLockManagerCmd 
    {
        private readonly ISessionService _sessionService;
        
        public event LockChangedHandler LockChanged;
        public LockCmd lockCmd { get; set; }

        public LockRequestBaseInfo lockRequest { get; set; }
        public SessionforBiz FromSession { get; set; }
        public SessionforBiz ToSession { get; set; }
        public CmdOperation OperationType { get; set; }
        public PacketSpec.Protocol.OriginalData DataPacket { get; set; }

        public ServerLockManagerCmd(CmdOperation operation, SessionforBiz FromSession = null, SessionforBiz ToSession = null)
        {
            this.OperationType = operation;
            this.FromSession = FromSession;
            this.ToSession = ToSession;
            _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>();
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
                        if (json == "null")
                        {
                            return false;
                        }
                        JObject obj = JObject.Parse(json);
                        LockedInfo lockRequest = obj.ToObject<LockedInfo>();
                        var isLocked = frmMainNew.Instance.lockManager.TryLock(lockRequest.BillID,
                            lockRequest.BillData.BillNo, lockRequest.BillData.BizName, lockRequest.LockedUserID);
                        if (isLocked)
                        {
                            if (frmMainNew.Instance.IsDebug)
                        {
                            frmMainNew.Instance.PrintInfoLog($"服务器锁{lockRequest.BillID}成功");
                        }
                        }
                        //通知所有人。这个单被锁了 包含锁单本人
                        ResponseToClient(isLocked, lockRequest);


                        break;
                    case LockCmd.UNLOCK:
                        json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        obj = JObject.Parse(json);
                        UnLockInfo unLockInfo = obj.ToObject<UnLockInfo>();
                        var isUnlocked = frmMainNew.Instance.lockManager.Unlock(unLockInfo.BillID, unLockInfo.LockedUserID);
                        if (isUnlocked)
                        {
                            if (frmMainNew.Instance.IsDebug)
                            {
                                frmMainNew.Instance.PrintInfoLog($"服务器解锁{unLockInfo.BillData.BillNo}成功");
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
                        var isUnlockedBizName = frmMainNew.Instance.lockManager.RemoveLockByBizName(unLockInfoBizName.LockedUserID, unLockInfoBizName.BillData.BizName);
                        if (isUnlockedBizName)
                        {
                            if (frmMainNew.Instance.IsDebug)
                            {
                                frmMainNew.Instance.PrintInfoLog($"服务器解锁业务类型{unLockInfoBizName.BillData.BizName}成功");
                            }
                            //通知所有人。这个类型的单被解锁了， 包含锁单本人
                            ResponseToClient(isUnlockedBizName, unLockInfoBizName);
                        }

                        //OnLockChanged(lockCmd,unLockInfo.BillID, isUnlocked);

                        break;
                    case LockCmd.RequestUnLock:
                        //接收每个人的请求，但是通知谁要看谁锁定了
                        json = PacketSpec.Serialization..GetString(gd.Two, ref index);
                        obj = JObject.Parse(json);
                        RequestUnLockInfo requestUnLockInfo = obj.ToObject<RequestUnLockInfo>();
                        //转发请求
                        //OnLockChanged(lockCmd,requestUnLockInfo.BillID, true);

                        // 构建请求数据
                        var requestData = new
                        {
                            Command = "REQUEST_UNLOCK",
                            BillID = requestUnLockInfo.BillID,
                            BillNo = requestUnLockInfo.BillNo,
                            RequestUserID = requestUnLockInfo.RequestUserID,
                            RequestUserName = requestUnLockInfo.RequestUserName,
                            LockedUserID = requestUnLockInfo.LockedUserID,
                            RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            RequestReason = requestUnLockInfo.RequestReason
                        };

                        // 获取所有会话
                        var sessions = _sessionService.GetAllUserSessions();
                        bool sentSuccessfully = false;

                        foreach (var session in sessions)
                        {
                            // 跳过自己
                            if (FromSession != null && session.SessionID == FromSession.SessionID)
                            {
                                continue;
                            }

                            // 有指定目标时，其它人就不发了
                            if (ToSession != null && session.SessionID == ToSession.SessionID)
                            {
                                var result = _sessionService.SendCommandToSession(session.SessionID, "LOCK_MANAGEMENT", requestData);
                                if (result.IsSuccess)
                                {
                                    sentSuccessfully = true;
                                }
                                break;
                            }

                            // 向锁定的人发送消息 请求解锁
                            if (session.UserID == requestUnLockInfo.LockedUserID)
                            {
                                var result = _sessionService.SendCommandToSession(session.SessionID, "LOCK_MANAGEMENT", requestData);
                                if (result.IsSuccess)
                                {
                                    sentSuccessfully = true;
                                    frmMainNew.Instance.PrintInfoLog($"已向用户 {session.UserName} 发送解锁请求");
                                }
                                else
                                {
                                    frmMainNew.Instance.PrintInfoLog($"向用户 {session.UserName} 发送解锁请求失败: {result.ErrorMessage}");
                                }
                                break;
                            }
                        }

                        if (!sentSuccessfully)
                        {
                            frmMainNew.Instance.PrintInfoLog($"未能找到锁定用户 {requestUnLockInfo.LockedUserID} 的在线会话");
                        }
                        break;
                    case LockCmd.RefuseUnLock:
                        json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        obj = JObject.Parse(json);
                        RefuseUnLockInfo refuseUnLockInfo = obj.ToObject<RefuseUnLockInfo>();

                        // 构建拒绝解锁数据
                        var refuseData = new
                        {
                            Command = "REFUSE_UNLOCK",
                            BillID = refuseUnLockInfo.BillID,
                            BillNo = refuseUnLockInfo.BillNo,
                            RequestUserID = refuseUnLockInfo.RequestUserID,
                            RefuseUserID = refuseUnLockInfo.RefuseUserID,
                            RefuseUserName = refuseUnLockInfo.RefuseUserName,
                            RefuseTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            RefuseReason = refuseUnLockInfo.RefuseReason
                        };

                        // 获取所有会话
                        var sessionsRefuse = _sessionService.GetAllUserSessions();
                        bool refuseSentSuccessfully = false;

                        foreach (var session in sessionsRefuse)
                        {
                            // 跳过自己
                            if (FromSession != null && session.SessionID == FromSession.SessionID)
                            {
                                continue;
                            }

                            // 有指定目标时，其它人就不发了
                            if (ToSession != null && session.SessionID == ToSession.SessionID)
                            {
                                var result = _sessionService.SendCommandToSession(session.SessionID, "LOCK_MANAGEMENT", refuseData);
                                if (result.IsSuccess)
                                {
                                    refuseSentSuccessfully = true;
                                }
                                break;
                            }

                            // 向请求的人发送拒绝消息
                            if (session.UserID == refuseUnLockInfo.RequestUserID)
                            {
                                var result = _sessionService.SendCommandToSession(session.SessionID, "LOCK_MANAGEMENT", refuseData);
                                if (result.IsSuccess)
                                {
                                    refuseSentSuccessfully = true;
                                    frmMainNew.Instance.PrintInfoLog($"已向用户 {session.UserName} 发送拒绝解锁通知");
                                }
                                else
                                {
                                    frmMainNew.Instance.PrintInfoLog($"向用户 {session.UserName} 发送拒绝解锁通知失败: {result.ErrorMessage}");
                                }
                                break;
                            }
                        }

                        if (!refuseSentSuccessfully)
                        {
                            frmMainNew.Instance.PrintInfoLog($"未能找到请求用户 {refuseUnLockInfo.RequestUserID} 的在线会话");
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
                if (FromSession != null)
                {
                    string userMsg = FromSession.User.用户名 + FromSession.User.客户端IP;
                    frmMain.Instance._logger.LogError("ShowExceptionMsg:" + userMsg, ex);
                }
                else
                {
                    Comm.CommService.ShowExceptionMsg("接收请求4:" + ex.Message);
                }

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
                    item.Value.AddSendData((byte)ServerCommand.锁管理, new byte[] { (byte)lockCmd }, tx.toByte());
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
                        item.Value.AddSendData((byte)ServerCommand.锁管理, new byte[] { (byte)lockCmd }, tx.toByte());
                    }
                }
                else
                {
                    ToSession.AddSendData((byte)ServerCommand.锁管理, new byte[] { (byte)lockCmd }, tx.toByte());
                }
            }
            catch (Exception ex)
            {

            }


        }
    }
}
