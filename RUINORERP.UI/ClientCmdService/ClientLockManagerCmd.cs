using Autofac.Core;
using FastReport.Table;
using Force.DeepCloner;
using Krypton.Navigator;
using LightTalkChatBox;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.POIFS.Crypt.Dsig;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Security;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.IM;
using RUINORERP.UI.WorkFlowDesigner.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using TransInstruction;
using TransInstruction.CommandService;
using TransInstruction.DataModel;
using TransInstruction.DataPortal;
using static RUINORERP.Business.CommService.LockManager;
using MessageType = RUINORERP.Model.TransModel.MessageType;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 锁单指令
    /// 类似客户端的代理作用 请求一个指令后 等待返回值就触发事件 LockManagerProxy
    /// </summary>
    public class ClientLockManagerCmd : IClientCommand, IDisposable
    {

        public Guid PacketId { get; set; }

        private bool _disposed;
        public CmdOperation OperationType { get; set; }
        public OriginalData DataPacket { get; set; }

        public event LockChangedHandler LockChanged;
        public LockRequestBaseInfo RequestInfo { get; set; }
        /// <summary>
        /// 接收时不用指定。发送时要指定。打包在指令包中
        /// </summary>
        public LockCmd lockCmd { get; set; }

        //public NextProcesszStep nextProcesszStep { get; set; } = NextProcesszStep.无;

        public ClientLockManagerCmd(CmdOperation operation)
        {
            PacketId = Guid.NewGuid();
            OperationType = operation;
        }





        public bool AnalyzeBroadcastDataPacket(OriginalData gd)
        {
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                string sendTime = ByteDataAnalysis.GetString(gd.Two, ref index);
                LockCmd lockCmd = (LockCmd)ByteDataAnalysis.GetInt(gd.Two, ref index);
                if (lockCmd == LockCmd.Broadcast)
                {
                    string lockDictionaryJson = ByteDataAnalysis.GetString(gd.Two, ref index);
                    MainForm.Instance.lockManager.UpdateLockStatusByJson(lockDictionaryJson);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("AnalyzeBroadcastDataPacket:" + ex.Message);
            }
            return true;

        }

        public bool AnalyzeDataPacket(OriginalData gd)
        {
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                string sendTime = ByteDataAnalysis.GetString(gd.Two, ref index);
                LockCmd lockCmd = (LockCmd)ByteDataAnalysis.GetInt(gd.Two, ref index);

                switch (lockCmd)
                {
                    //客户端请求锁定后 服务器返回结果 本地要更新状态
                    case LockCmd.LOCK:
                        string json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        JObject obj = JObject.Parse(json);
                        LockedInfo lockRequest = obj.ToObject<LockedInfo>();
                        bool isSuccess = ByteDataAnalysis.Getbool(gd.Two, ref index);
                        if (isSuccess)
                        {
                            //自己也锁。实际服务器收到后还会广播一次 包括自己 只是这里已经锁，不会重复添加
                            if (MainForm.Instance.lockManager.TryLock(lockRequest.BillID, lockRequest.BillData.BillNo, lockRequest.LockedUserID))
                            {
                                if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
                                {
                                    MainForm.Instance.PrintInfoLog($"{lockRequest.BillData.BillNo}本地锁定成功");
                                }
                            }
                        }
                        ServerLockCommandEventArgs args = new ServerLockCommandEventArgs();
                        args.lockCmd = lockCmd;
                        args.isSuccess = isSuccess;
                        args.requestBaseInfo = lockRequest;
                        ClientEventManager.Instance.RaiseCommandEvent(lockRequest.PacketId, args);

                        break;
                    case LockCmd.UNLOCK:
                        json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        obj = JObject.Parse(json);
                        UnLockInfo unLockInfo = obj.ToObject<UnLockInfo>();
                        isSuccess = ByteDataAnalysis.Getbool(gd.Two, ref index);
                        if (isSuccess)
                        {
                            if (MainForm.Instance.lockManager.Unlock(unLockInfo.BillID, unLockInfo.LockedUserID))
                            {
                                if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
                                {
                                    MainForm.Instance.PrintInfoLog($"{unLockInfo.BillData.BillNo}本地解锁成功");
                                }
                            }
                        }
                        ServerLockCommandEventArgs UNLOCKargs = new ServerLockCommandEventArgs();
                        UNLOCKargs.lockCmd = lockCmd;
                        UNLOCKargs.isSuccess = isSuccess;
                        UNLOCKargs.requestBaseInfo = unLockInfo;
                        ClientEventManager.Instance.RaiseCommandEvent(unLockInfo.PacketId, UNLOCKargs);

                        break;

                    case LockCmd.RequestUnLock:

                        json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        obj = JObject.Parse(json);
                        RequestUnLockInfo requestUnLockInfo = obj.ToObject<RequestUnLockInfo>();

                        ////发送提醒
                        //LockChanged?.Invoke(this, new LockChangedEventArgs(lockCmd, requestUnLockInfo.BillID, true));

                        ReminderData MessageInfo = new ReminderData();
                        MessageInfo.BizKeyID = requestUnLockInfo.BillID;
                        MessageInfo.SendTime = sendTime;
                        MessageInfo.BizType = requestUnLockInfo.BillData.BizType;
                        MessageInfo.SenderEmployeeName = requestUnLockInfo.RequestUserName;
                        MessageInfo.ReminderContent = $"【{requestUnLockInfo.RequestUserName}】请求释放{requestUnLockInfo.BillData.BizName}:{requestUnLockInfo.BillData.BillNo}的锁！请保存数据后，关闭对应单据窗口。";
                        MessageInfo.messageCmd = MessageCmdType.UnLockRequest;
                        MainForm.Instance.MessageList.Enqueue(MessageInfo);
                        break;

                    case LockCmd.RefuseUnLock:

                        json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        obj = JObject.Parse(json);
                        RefuseUnLockInfo refuseUnLockInfo = obj.ToObject<RefuseUnLockInfo>();

                        ReminderData RefuseInfo = new ReminderData();
                        RefuseInfo.BizKeyID = refuseUnLockInfo.BillID;
                        RefuseInfo.SendTime = sendTime;
                        RefuseInfo.BizType = refuseUnLockInfo.BillData.BizType;
                        RefuseInfo.SenderEmployeeName = refuseUnLockInfo.RefuseUserName;
                        RefuseInfo.RemindSubject = $"【{refuseUnLockInfo.RefuseUserName}】拒绝释放";
                        RefuseInfo.ReminderContent = $"拒绝释放" +
                            $"{refuseUnLockInfo.BillData.BizName}:{refuseUnLockInfo.BillData.BillNo}的锁！。";
                        RefuseInfo.messageCmd = MessageCmdType.Notice;
                        MainForm.Instance.MessageList.Enqueue(RefuseInfo);
                        break;
                    case LockCmd.Broadcast:
                        json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        MainForm.Instance.lockManager.UpdateLockStatusByJson(json);
                        break;

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器提示消息:" + ex.Message);
            }
            return true;

        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, object parameters)
        {
            await Task.Run(() =>
            {
                #region 执行方法

                if (OperationType == CmdOperation.Send)
                {
                    switch (lockCmd)
                    {
                        case LockCmd.LOCK:
                        case LockCmd.UNLOCK:
                        case LockCmd.RequestUnLock:
                        case LockCmd.RefuseUnLock:
                            BuildDataPacket(RequestInfo);
                            break;
                        case LockCmd.Broadcast:
                            BuildRequestBroadcast();
                            break;
                        default:
                            break;
                    }
                }
                if (OperationType == CmdOperation.Receive)
                {
                    int index = 0;
                    ByteBuff bg = new ByteBuff(DataPacket.Two);
                    string sendTime = ByteDataAnalysis.GetString(DataPacket.Two, ref index);
                    lockCmd = (LockCmd)ByteDataAnalysis.GetInt(DataPacket.Two, ref index);
                    switch (lockCmd)
                    {
                        case LockCmd.LOCK:
                        case LockCmd.UNLOCK:
                        case LockCmd.RequestUnLock:
                            AnalyzeDataPacket(DataPacket);
                            break;
                        case LockCmd.RefuseUnLock:
                            OriginalData gd = DataPacket;
                            #region
                            try
                            {
                                index = 0;
                                bg = new ByteBuff(gd.Two);
                                sendTime = ByteDataAnalysis.GetString(gd.Two, ref index);
                                LockCmd lockCmd = (LockCmd)ByteDataAnalysis.GetInt(gd.Two, ref index);
                                long BillID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                                long LockedUserID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                                string RequestReleaseUserName = ByteDataAnalysis.GetString(gd.Two, ref index);
                                int BillBizType = ByteDataAnalysis.GetInt(gd.Two, ref index);
                                long MenuID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                                if (lockCmd == LockCmd.RefuseUnLock)
                                {
                                    long lockuserid = MainForm.Instance.lockManager.GetLockedBy(BillID);
                                    //锁定的人就是自己时才提醒
                                    long selfUserid = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID;
                                    if (lockuserid == selfUserid)
                                    {
                                        //MessageBox.Show("xx请您请求释放锁！", "提示");
                                        //RequestReleaseUserName
                                        //xx请您请求释放锁！弹出窗口确认 是否同意
                                        if (System.Windows.Forms.MessageBox.Show($"{RequestReleaseUserName}请求操作当前单据，是否同意释放锁！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                        {
                                            if (MainForm.Instance.lockManager.Unlock(BillID, LockedUserID))
                                            {
                                                //同意则向服务器发消息
                                                //BuildDataPacket(object request)
                                                //释放锁成功
                                                //MainForm.Instance.PrintInfoLog("释放锁成功");
                                                //通知对方 或广播一下？  
                                            }
                                        }
                                        else
                                        {
                                            //拒绝消息通知到对方

                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MainForm.Instance.PrintInfoLog("接收服务器提示消息:" + ex.Message);
                            }
                            #endregion
                            break;
                        case LockCmd.Broadcast:
                            AnalyzeBroadcastDataPacket(DataPacket);
                            break;
                        default:
                            break;
                    }

                }


                #endregion

            }, cancellationToken);

        }

        public void BuildRequestBroadcast()
        {
            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuff(100);
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushInt((int)lockCmd);

                //将来再加上提醒配置规则,或加在请求实体中
                gd.cmd = (byte)ClientCmdEnum.复合型锁单处理;
                gd.One = new byte[] { (byte)lockCmd };
                gd.Two = tx.toByte();
                MainForm.Instance.ecs.AddSendData(gd);
            }
            catch (Exception)
            {

            }
        }

        public void BuildDataPacket(object request)
        {

            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuff(100);
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushInt((int)lockCmd);

                switch (lockCmd)
                {
                    case LockCmd.LOCK:
                        LockedInfo lockInfo = request as LockedInfo;
                        string json = JsonConvert.SerializeObject(lockInfo,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                       });
                        tx.PushString(json);
                        break;
                    case LockCmd.UNLOCK:
                        UnLockInfo unLockInfo = request as UnLockInfo;
                        json = JsonConvert.SerializeObject(unLockInfo,
                      new JsonSerializerSettings
                      {
                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                      });
                        tx.PushString(json);
                        break;
                    case LockCmd.RequestUnLock:
                        RequestUnLockInfo requestUnLockInfo = request as RequestUnLockInfo;
                        json = JsonConvert.SerializeObject(requestUnLockInfo,
                      new JsonSerializerSettings
                      {
                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                      });
                        tx.PushString(json);

                        break;
                    case LockCmd.RefuseUnLock:
                        RefuseUnLockInfo refuseUnLockInfo = request as RefuseUnLockInfo;
                        json = JsonConvert.SerializeObject(refuseUnLockInfo,
                      new JsonSerializerSettings
                      {
                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                      });
                        tx.PushString(json);
                        break;
                    case LockCmd.Broadcast:
                        break;
                    default:
                        break;
                }


                //将来再加上提醒配置规则,或加在请求实体中
                gd.cmd = (byte)ClientCmdEnum.复合型锁单处理;
                gd.One = new byte[] { (byte)lockCmd };
                gd.Two = tx.toByte();
                MainForm.Instance.ecs.AddSendData(gd);
            }
            catch (Exception)
            {

            }
        }

        public void HandleLockEvent(object sender, ServerLockCommandEventArgs e)
        {
            if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
            {
                switch (e.lockCmd)
                {
                    case LockCmd.LOCK:
                        MainForm.Instance.PrintInfoLog($"成功锁定{e.requestBaseInfo.BillData.BillNo}");
                        break;
                    case LockCmd.UNLOCK:
                        MainForm.Instance.PrintInfoLog($"成功解锁{e.requestBaseInfo.BillData.BillNo}");
                        break;
                    case LockCmd.RequestUnLock:
                        break;
                    case LockCmd.RefuseUnLock:
                        break;
                    case LockCmd.Broadcast:
                        break;
                }
                
                // 处理锁定单据的逻辑
                //MainForm.Instance.uclog.AddLog($"收到锁定单据指令，结果为{e.lockCmd.ToString()}{e.isSuccess}:{e.BillID}");
            }

            LockChanged?.Invoke(this, e);
            if (sender is ServerLockCommandHandler commandHandler)
            {
               
                
            }
            //事件处理完了才删除
            ClientEventManager.Instance.RemoveCommandHandler(e.requestBaseInfo.PacketId, HandleLockEvent);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                // 移除处理器
                //ClientEventManager.Instance.RemoveCommandHandler(PacketId, HandleLockEvent);
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }




    public class LockRefusalInfo
    {
        public string RefusingUserID { get; set; } // 拒绝释放锁的用户的ID
        public string RefusingUserName { get; set; } // 拒绝释放锁的用户的姓名
        public string LockedUserID { get; set; } // 被拒绝释放锁的用户的ID
        public string LockedUserName { get; set; } // 被拒绝释放锁的用户的姓名
        public string BillID { get; set; } // 单据ID
        public int BillBizType { get; set; } // 单据业务类型
        public string RefusalReason { get; set; } // 拒绝释放锁的原因
        public DateTime RefusalTime { get; set; } // 拒绝释放锁的时间

        public LockRefusalInfo()
        {
            RefusalTime = DateTime.Now; // 默认设置为当前时间
        }
    }

}
