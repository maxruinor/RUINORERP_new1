using HLH.Lib.Security;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.BizService;
using RUINORERP.Server.ServerSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.DataPortal;
using TransInstruction.Enums;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RUINORERP.Server.CommandService
{
    /// <summary>
    /// 用户登陆
    /// </summary>
    public class LoginCommand : IServerCommand
    {
        public LoginProcessType requestType { get; set; }
        public LoginCommand()
        {

        }

        public event Action<bool, LoginCommand> OnLoginSuccess;
        public event Action OnLoginFailure; // 登录失败的事件

        /// <summary>
        /// 登陆的会话
        /// </summary>
        public SessionforBiz RequestSession { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public tb_UserInfo user { get; set; }
        public OriginalData gd { get; set; }
        public CmdOperation OperationType { get; set; }
        public OriginalData DataPacket { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // 登录逻辑
            //await Task.Run(() => session.Login(), cancellationToken);
            // 登录逻辑，例如验证用户名和密码
            // 模拟登录逻辑，这里可以替换为实际的登录逻辑
            await Task.Run(async () =>
            {

                // 登录逻辑，例如验证用户名和密码
                {

                    try
                    {
                        switch (requestType)
                        {
                            case LoginProcessType.用户登陆:
                                AnalyzeDataPacket(gd, RequestSession);
                                bool success = await Login(Username, Password);
                                if (success)
                                {
                                    if (frmMain.Instance.IsDebug)
                                    {
                                        frmMain.Instance.PrintMsg($"登陆{Username}的指令成功,接下来重复判断");
                                    }
                                    //优化判断相同用户
                                    //判断 是不是有相同的用户已经登陆了。有的话，则提示新登陆的人是不是T掉旧的用户。不是的话自己退出。
                                    var ExistSession = frmMain.Instance.sessionListBiz.Values.FirstOrDefault(c => c.User != null && !c.SessionID.Equals(RequestSession.SessionID) && c.User.用户名 == user.UserName);
                                    if (ExistSession != null)
                                    {
                                        回复用户重复登陆(RequestSession, ExistSession);
                                    }
                                    else
                                    {
                                        if (frmMain.Instance.IsDebug)
                                        {
                                            frmMain.Instance.PrintMsg($"登陆{Username}的指令成功,重复判断通过，接下来人数限制");
                                        }

                                        //正常注册时不会为0，只有自己的情况
                                        if (frmMain.Instance.registrationInfo.ConcurrentUsers == 0)
                                        {
                                            frmMain.Instance.registrationInfo.ConcurrentUsers = 1000;
                                        }

                                        //登陆成功时。
                                        if (frmMain.Instance.sessionListBiz.Count > frmMain.Instance.registrationInfo.ConcurrentUsers)
                                        {
                                            //超出人数时：提示一下再T掉第一个人
                                            //优先T重复的人。
                                            //提示被T的人。发送一个弹窗信息后，断开连接

                                            //按登陆时间算
                                            List<SessionforBiz> sessionList = frmMain.Instance.sessionListBiz.Values.ToList().OrderBy(c => c.StartTime).ToList();
                                            foreach (var item in sessionList)
                                            {
                                                // 创建一个命令实例 
                                                var message = new ReceiveResponseMessageCmd(CmdOperation.Send, null, item);
                                                message.messageType = MessageType.Prompt;
                                                message.MessageContent = "因超出在线人数限制，您即将被强制下线。";
                                                message.promptType = PromptType.确认窗口;
                                                message.ToSession = item;
                                                await message.ExecuteAsync(CancellationToken.None);
                                                // 等待 5 秒
                                                await Task.Delay(5000);
                                                UserService.强制用户退出(item);
                                                break;
                                            }


                                            


                                        }

                                  
                              

                                    }


                                    #region 如果加入了黑名单时登陆也提示
                                    if (RequestSession.RemoteEndPoint is IPEndPoint iP)
                                    {
                                        var remoteIp = iP.Address.ToString();
                                        // 检查IP是否被封禁
                                        if (BlacklistManager.IsIpBanned(remoteIp))
                                        {
                                            //发送通知:您被加入了黑名单，请联系管理员。
                                            // 创建一个命令实例 
                                            var message = new ReceiveResponseMessageCmd(CmdOperation.Send, null, RequestSession);
                                            message.messageType = MessageType.Prompt;
                                            message.MessageContent = "您被加入了黑名单，请联系管理员。";
                                            message.promptType = PromptType.确认窗口;
                                            message.ToSession = RequestSession;
                                            await message.ExecuteAsync(CancellationToken.None);
                                            // 等待 3 秒
                                            await Task.Delay(3000);

                                            UserService.强制用户退出(RequestSession);
                                            //延后3秒关闭sokcet连接
                                            //await RequestSession.CloseAsync(SuperSocket.Connection.CloseReason.ServerShutdown); // 立即关闭连接
                                            return;
                                        }
                                    }
                                    #endregion


                                    //确定成功时统一分发加载配置
                                    ReceiveEntityTransferCmd entityTransferCmd = new ReceiveEntityTransferCmd(CmdOperation.Send);
                                    entityTransferCmd.ToSession = RequestSession;
                                    entityTransferCmd.requestType = EntityTransferCmdType.处理动态配置;
                                    entityTransferCmd.TransferObject = frmMain.Instance.Globalconfig.CurrentValue;
                                    await entityTransferCmd.ExecuteAsync(cancellationToken);

                                    OnLoginSuccess?.Invoke(success, this);
                                }
                                else
                                {
                                    OnLoginFailure?.Invoke();
                                }


                                //通知客户端登陆成功
                                //if (success)
                                //{
                                //    new NotifyClientCommand().ExecuteAsync(cancellationToken);
                                //}


                                break;
                            case LoginProcessType.登陆回复:
                                break;
                            case LoginProcessType.已经在线:
                                break;
                            case LoginProcessType.超过限制:
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception)
                    {

                    }


                }
            }, cancellationToken);
        }

        private async Task<bool> Login(string UserName, string pwd)
        {
            bool rs = false;

            //password = EncryptionHelper.AesDecryptByHashKey(enPwd, username);
            string EnPassword = EncryptionHelper.AesEncryptByHashKey(pwd, UserName);

            user = await Program.AppContextData.Db.CopyNew().Queryable<tb_UserInfo>()
                   .Where(u => u.UserName == UserName && u.Password == EnPassword)
             .Includes(x => x.tb_employee)
                   .Includes(x => x.tb_User_Roles)
                   .SingleAsync();
            if (user != null)
            {

                //登陆成功
                RequestSession.User.用户名 = user.UserName;
                if (user.tb_employee != null)
                {
                    RequestSession.User.姓名 = user.tb_employee.Employee_Name;
                    RequestSession.User.Employee_ID = user.Employee_ID.Value;
                }
                //登陆时间
                RequestSession.User.登陆时间 = System.DateTime.Now;
                RequestSession.User.UserID = user.User_ID;
                RequestSession.User.超级用户 = user.IsSuperUser;
                RequestSession.User.在线状态 = true;
                RequestSession.User.授权状态 = true;
                rs = true;
                //通知客户端
                UserService.给客户端发提示消息(RequestSession, "用户【" + RequestSession.User.姓名 + "】登陆成功");
            }
            else
            {
                rs = false;
                //通知客户端
                UserService.给客户端发提示消息(RequestSession, "用户登陆出错，用户名或密码错误！");
            }
            return rs;
        }


        public bool AnalyzeDataPacket(OriginalData gd, SessionforBiz FromSession)
        {
            bool rs = false;
            try
            {
                int index = 0;
                string 登陆时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                Username = ByteDataAnalysis.GetString(gd.Two, ref index);
                Password = ByteDataAnalysis.GetString(gd.Two, ref index);
                rs = true;
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("接收登陆请求:" + ex.Message);
            }
            return rs;
        }

        public void BuildDataPacket(object request = null)
        {
            throw new NotImplementedException();
        }


        public static bool 回复用户重复登陆(SessionforBiz PlayerSession, SessionforBiz ExistSessionforBiz = null)
        {
            bool rs = false;
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {
                //PacketProcess pp = new PacketProcess(PlayerSession);
                ByteBuff tx = new ByteBuff(100);

                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushInt((int)LoginProcessType.已经在线);
                rs = true;
                tx.PushBool(rs);

                PlayerSession.AddSendData((byte)ServerCmdEnum.复合型登陆处理, null, tx.toByte());
                return rs;
            }
            catch (Exception ex)
            {
                rs = false;
            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过
            return rs;
        }




    }
}
