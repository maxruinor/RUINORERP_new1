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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using TransInstruction;
using TransInstruction.CommandService;
using TransInstruction.DataPortal;
using static RUINORERP.Business.CommService.LockManager;
using MessageType = RUINORERP.Model.TransModel.MessageType;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 锁单指令
    /// 类似客户端的代理作用 请求一个指令后 等待返回值就触发事件 LockManagerProxy
    /// </summary>
    public class RequestReceiveLockManagerCmd : IClientCommand
    {
        public CmdOperation OperationType { get; set; }
        public OriginalData DataPacket { get; set; }

        public event LockChangedHandler LockChanged;
        public LockRequestInfo lockRequest { get; set; }
        /// <summary>
        /// 接收时不用指定。发送时要指定。打包在指令包中
        /// </summary>
        public LockCmd lockCmd { get; set; }

        //public NextProcesszStep nextProcesszStep { get; set; } = NextProcesszStep.无;





        public RequestReceiveLockManagerCmd(CmdOperation operation)
        {
            OperationType = operation;
        }

        /// <summary>
        /// 收到被锁或释放 影响到其它的UI啥的事件来通知
        /// </summary>
        public event LockManagerEventHandler MessageReceived;

        public void ReceiveMessage(string message, string senderName, string senderSessionID)
        {
            MessageReceived?.Invoke(this, new LockManagerEventArgs
            {
                Message = message,
                SenderName = senderName,
                SenderSessionID = senderSessionID
            });
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
                long BillID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                switch (lockCmd)
                {
                    //客户端请求锁定后 服务器返回结果 本地要更新状态
                    case LockCmd.LOCK:
                    case LockCmd.UNLOCK:
                        long LockedUserID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                        string LockedUserName = ByteDataAnalysis.GetString(gd.Two, ref index);
                        int BillBizType = ByteDataAnalysis.GetInt(gd.Two, ref index);
                        long MenuID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                        if (lockCmd == LockCmd.LOCK)
                        {
                            MainForm.Instance.lockManager.TryLock(BillID, LockedUserID);
                            LockChanged?.Invoke(this, new LockChangedEventArgs(BillID, true, LockedUserID));
                        }
                        if (lockCmd == LockCmd.UNLOCK)
                        {
                            MainForm.Instance.lockManager.Unlock(BillID, LockedUserID);
                            LockChanged?.Invoke(this, new LockChangedEventArgs(BillID, true, LockedUserID));
                        }
                        break;

                    case LockCmd.RequestReleaseLock:

                        LockedUserID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                        string UserName = ByteDataAnalysis.GetString(gd.Two, ref index);
                        ReminderData MessageInfo = new ReminderData();
                        MessageInfo.BizKeyID = BillID;
                        MessageInfo.SendTime = sendTime;
                        MessageInfo.SenderEmployeeName = UserName;
                        MessageInfo.ReminderContent = $"【{UserName}】请求释放锁！请关闭单据窗口。";
                        MessageInfo.messageCmd = MessageCmdType.UnLockRequest;
                        MainForm.Instance.MessageList.Enqueue(MessageInfo);
                        break;

                    //case LockCmd.GetLockedBy:
                    //    long LockeduserID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                    //    LockChanged?.Invoke(this, new LockChangedEventArgs(BillID, true, LockeduserID));

                    //MainForm.Instance.lockManager.UpdateLockStatus()

                    //case PromptType.确认窗口:
                    //    MessageBox.Show(obj["Content"].ToString(), "提示");
                    //    break;
                    //case PromptType.日志消息:
                    //    MainForm.Instance.PrintInfoLog(obj["Content"].ToString());
                    //    break;
                    //string message = ByteDataAnalysis.GetString(gd.Two, ref index);
                    //NextProcesszStep nextProcesszStep = (NextProcesszStep)ByteDataAnalysis.GetInt(gd.Two, ref index);
                    //string fromSessionID = ByteDataAnalysis.GetString(gd.Two, ref index);
                    //MessageContent = message;
                    //这里指定的是IM的，则会影响IM窗体
                    //判断是不是打开。打开就显示内容。弹出提示？
                    //break;

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
                            BuildDataPacket(lockRequest);
                            break;
                        case LockCmd.RequestReleaseLock:
                            BuildDataPacket(lockRequest);
                            break;
                        case LockCmd.RefuseReleaseLock:
                            BuildDataPacket(lockRequest);
                            break;
                        //case LockCmd.GetLockedBy:
                        //    BuildDataPacket(lockRequest);
                        //    break;
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
                        case LockCmd.RequestReleaseLock:
                            AnalyzeDataPacket(DataPacket);
                            break;
                        case LockCmd.RefuseReleaseLock:
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
                                if (lockCmd == LockCmd.RefuseReleaseLock)
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
            LockRequestInfo lockRequest = request as LockRequestInfo;
            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuff(100);
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushInt((int)lockCmd);
                if (lockCmd == LockCmd.RefuseReleaseLock)
                {
                    tx.PushInt64(lockRequest.BillID);
                    tx.PushInt64(lockRequest.LockedUserID);
                    tx.PushString(lockRequest.RequestReleaseUserName);
                    tx.PushInt((int)lockRequest.BillBizType);
                    tx.PushInt64(lockRequest.MenuID);
                }
                else
                {
                    tx.PushInt64(lockRequest.BillID);
                    tx.PushInt64(lockRequest.LockedUserID);
                    tx.PushString(lockRequest.RequestReleaseUserName);
                    tx.PushInt((int)lockRequest.BillBizType);
                    tx.PushInt64(lockRequest.MenuID);
                }

                // tx.PushInt((int)nextProcesszStep); //请求谁开锁？

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


    }

    public class LockManagerEventArgs : EventArgs
    {
        public string Message { get; set; }
        public string SenderName { get; internal set; }
        public string SenderSessionID { get; internal set; }
    }
    public delegate void LockManagerEventHandler(object sender, LockManagerEventArgs e);

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
