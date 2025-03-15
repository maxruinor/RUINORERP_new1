using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Server.Commands;
using RUINORERP.Server.ServerSession;
using SuperSocket;
using SuperSocket.Command;
using TransInstruction;
using TransInstruction.DataPortal;
using RUINORERP.Server.BizService;
using RUINORERP.Model;
using RUINORERP.Server.ServerService;
using Microsoft.Extensions.Caching.Memory;
using RUINORERP.Model.Base;
using RUINORERP.Server.Comm;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RUINORERP.Business.CommService;
using Microsoft.Extensions.Logging;
using SuperSocket.Server.Abstractions.Session;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using System.Numerics;
using TransInstruction.CommandService;
using RUINORERP.Server.CommandService;
using RUINORERP.Model.TransModel;

namespace RUINORERP.Server.Commands
{
    ///重点部分2021-11-27
    /// <summary>
    /// 0xFF 0xFE
    /// </summary>
    // [Command(Key = unchecked((short) (0xFF * 256 + 0xFE)))]
    [Command(Key = "KXGame")]
    public class BizCommand : IAsyncCommand<BizPackageInfo>
    {
        private IMemoryCache _cache;

        //保存在Log4net_cath
        public ILogger<BizCommand> _logger { get; set; }
        private readonly CommandDispatcher _commandDispatcher;
        //private CommandQueue _commandQueue;
        private CommandScheduler _commandScheduler;

        public BizCommand(IMemoryCache cache, ILogger<BizCommand> logger,
            CommandDispatcher dispatcher)
        {
            _cache = cache;
            _logger = logger;

            _commandDispatcher = dispatcher;
            _commandScheduler = new CommandScheduler(_commandDispatcher);
            _commandScheduler.StartProcessing();
        }




        public async ValueTask ExecuteAsync(IAppSession session, BizPackageInfo package, CancellationToken cancellationToken)
        {
            SessionforBiz fromPlayer = session as SessionforBiz;
            await Task.Delay(0);

            OriginalData gd = new OriginalData();
            gd.cmd = package.kd.cmd;
            gd.One = package.kd.One;
            gd.Two = package.kd.Two;

            if (package.Body.Length <= 18)
            {

            }
            if (package.Flag == "空包")
            {

            }
            else
            {
                string rs = string.Empty;
                try
                {
                    TransService ise = new TransService();
                    //ise.OtherEvent += InstructionService_OtherEvent;
                    TransProtocol gp = new TransProtocol();
                    ClientCmdEnum CleintCmd = (ClientCmdEnum)gd.cmd;// gp.GetClientCmd(gd);
                    TransPackProcess tpp = new TransPackProcess();
                    //rs = gp.CheckClientCmd(gd);//这里提取中文 耗资源
                    //CmdService cs = new CmdService();
                    //cs.GameRole = PlayerSession;
                    int index = 0;
                    if (CleintCmd != ClientCmdEnum.客户端心跳包 && CleintCmd != ClientCmdEnum.角色处于等待)
                    {
                        if (frmMain.Instance.IsDebug)
                        {
                            frmMain.Instance.PrintMsg($"收到客户端{fromPlayer.User.用户名}的指令:" + CleintCmd.ToString());
                        }
                    }
                    switch (CleintCmd)
                    {

                        case ClientCmdEnum.复合型消息处理:
                            // 创建一个命令实例 
                            var message = new ReceiveResponseMessageCmd(CmdOperation.Receive, fromPlayer);
                            message.DataPacket = gd;
                            await message.ExecuteAsync(CancellationToken.None);
                            break;

                        case ClientCmdEnum.复合型登陆请求:
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintMsg(fromPlayer.User.用户名 + "复合型登陆请求");
                            }
                            // 创建一个命令实例 
                            var MixLoginCommand = new LoginCommand();
                            MixLoginCommand.gd = gd;
                            MixLoginCommand.RequestSession = fromPlayer;
                            _commandScheduler.queue.Add(MixLoginCommand);

                            break;

                        case ClientCmdEnum.复合型工作流请求:
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintMsg(fromPlayer.User.用户名 + "复合型工作流请求");
                            }
                            // 创建一个命令实例 
                            var receiveReminderCmd = new ReceiveReminderCmd(gd, fromPlayer);
                            // _dispatcher.RegisterHandler<LoginCommand>(loginCommand); // 注册命令处理函数
                            _commandScheduler.queue.Add(receiveReminderCmd);
                            break;
                        case ClientCmdEnum.复合型实体请求:
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintMsg(fromPlayer.User.用户名 + "复合型实体请求");
                            }
                            // 创建一个命令实例 
                            var receiveEntityTransferCmd = new ReceiveEntityTransferCmd(gd, fromPlayer, CmdOperation.Receive);
                            _commandScheduler.queue.Add(receiveEntityTransferCmd);
                            break;
                        case ClientCmdEnum.更新动态配置:
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintMsg(fromPlayer.User.用户名 + "更新动态配置");
                            }
                            UserService.接收更新动态配置指令(fromPlayer, gd);
                            break;

                        case ClientCmdEnum.工作流提醒回复:

                            WorkflowServiceReceiver.接收工作流提醒回复(fromPlayer, gd);
                            break;


                        case ClientCmdEnum.工作流提醒请求:

                            WorkflowServiceReceiver.接收工作流提醒请求(fromPlayer, gd);
                            break;


                        //按类型 工作流的 可以做子指令，暂时为了测试快。先不处理了
                        case ClientCmdEnum.工作流审批:

                            WorkflowServiceReceiver.接收审批结果事件推送(fromPlayer, gd);
                            break;
                        case ClientCmdEnum.工作流启动:

                            WorkflowServiceReceiver.启动工作流(fromPlayer, gd);
                            //  WorkflowServiceReceiver.接收工作流提交(Player, gd);
                            break;
                        case ClientCmdEnum.工作流指令:

                            WorkflowServiceReceiver.接收工作流审批(fromPlayer, gd);
                            break;
                        case ClientCmdEnum.请求强制用户下线:
                            //T掉指定用户。
                            //请求强制用户下线
                            UserService.处理请求强制用户下线(gd);
                            break;
                        case ClientCmdEnum.请求强制登陆上线:
                            //T掉指定用户。
                            //请求强制用户下线
                            UserService.处理请求强制登陆上线(gd);
                            break;
                        case ClientCmdEnum.准备登陆:
                            byte[] source = gd.Two;
                            try
                            {
                                string msg = ByteDataAnalysis.GetString(gd.Two, ref index);
                            }
                            catch (Exception ex)
                            {
                                rs = ex.Message;
                            }
                            break;

                            /*
                        case ClientCmdEnum.用户登陆:
                            _cache.Set("用户登陆", "用户登陆");

                            tb_UserInfo user = await UserService.接收用户登陆指令(fromPlayer, gd);
                            if (UserService.用户登陆回复(fromPlayer, user))
                            {
                                //判断 是不是有相同的用户已经登陆了。有的话，则提示新登陆的人是不是T掉旧的用户。不是的话自己退出。
                                var ExistSession = frmMain.Instance.sessionListBiz.Values.FirstOrDefault(c => c.User != null && !c.SessionID.Equals(fromPlayer.SessionID) && c.User.用户名 == user.UserName);
                                if (ExistSession != null)
                                {
                                    UserService.回复用户重复登陆(fromPlayer, ExistSession);
                                }
                                else
                                {
                                    UserService.回复用户重复登陆(fromPlayer, ExistSession);
                                }

                                //登陆成功时。
                                if (frmMain.Instance.sessionListBiz.Count > frmMain.Instance.registrationInfo.ConcurrentUsers)
                                {
                                    //超出人数时：提示一下再T掉第一个人
                                    //优先T重复的人。
                                    if (ExistSession != null)
                                    {

                                    }
                                    else
                                    {

                                    }

                                }

                                UserService.发送在线列表(fromPlayer);
                                UserService.发送缓存信息列表(fromPlayer);


                            }


                            break;
                            */
                        case ClientCmdEnum.实时汇报异常:
                            foreach (var item in frmMain.Instance.sessionListBiz.ToArray())
                            {
                                SessionforBiz sessionforBiz = item.Value as SessionforBiz;
                                //自己的不会上传。 只转给超级管理员。
                                if (sessionforBiz.User.超级用户)
                                {
                                    SystemService.转发异常数据(fromPlayer, sessionforBiz, gd);
                                }
                            }

                            break;

                        case ClientCmdEnum.请求缓存:
                            index = 0;
                            string datatime = ByteDataAnalysis.GetString(gd.Two, ref index);
                            string RequestTableName = ByteDataAnalysis.GetString(gd.Two, ref index);
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintMsg(fromPlayer.User.用户名 + "请求缓存表：" + RequestTableName);
                            }
                            Stopwatch stopwatchSender = Stopwatch.StartNew();
                            stopwatchSender.Start();
                            //如果指定了表名，则只发送指定表的数据，否则全部发送
                            if (!string.IsNullOrEmpty(RequestTableName) && BizCacheHelper.Manager.NewTableList.Keys.Contains(RequestTableName))
                            {
                                UserService.发送缓存数据列表(fromPlayer, RequestTableName);
                                if (frmMain.Instance.IsDebug)
                                {
                                    frmMain.Instance.PrintInfoLog($"发送缓存数据列表{RequestTableName}给{fromPlayer.User.用户名} 耗时：{stopwatchSender.ElapsedMilliseconds} 毫秒");
                                }
                            }
                            else
                            {
                                if (frmMain.Instance.IsDebug)
                                {
                                    frmMain.Instance.PrintInfoLog($"发送缓存数据列表{RequestTableName}给{fromPlayer.User.用户名} 耗时：{stopwatchSender.ElapsedMilliseconds} 毫秒");
                                }
                            }
                            stopwatchSender.Stop();
                           

                            break;

                        case ClientCmdEnum.更新缓存:
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintMsg(fromPlayer.User.用户名 + "更新缓存");
                            }
                            UserService.接收更新缓存指令(fromPlayer, gd);

                            break;
                        case ClientCmdEnum.删除缓存:
                            UserService.接收删除缓存指令(fromPlayer, gd);
                            break;
                        case ClientCmdEnum.请求协助处理:
                            SystemService.process请求协助处理(gd);
                            break;
                        //case ClientCmdEnum.单据锁定:

                        //    //意思是如:审核了销售出库单时，订单是无法再操作了。转发到所有电脑。保存等各种操作都要判断一下？
                        //    //这种主要是比方业务订单UI没有关掉。仓库出库了。业务还可以反审等？不在线的不管。会重新打开。这时状态不一样。会判断好。
                        //    //所有缓存到客户机。服务器不用缓存列表了？
                        //    //或者相同的一个单。A在编辑没有完成。B再接着编辑保存。这时B打开时就会看到锁定状态。B保存时就会提示
                        //    //退出后 解除锁定
                        //    SystemService.process单据审核锁定(fromPlayer, gd);
                        //    break;
                        //case ClientCmdEnum.单据锁定释放:
                        //    SystemService.process单据审核锁定释放(fromPlayer, gd);
                        //    break;

                        case ClientCmdEnum.复合型锁单处理:
                            // 创建一个命令实例 
                            //  实际收到一个转发指令
                            var lockcmd = new ServerLockManagerCmd(CmdOperation.Receive, fromPlayer);
                            lockcmd.DataPacket = gd;
                            await lockcmd.ExecuteAsync(CancellationToken.None);
                            break;

                        case ClientCmdEnum.发送弹窗消息:
                            UserService.转发弹窗消息(fromPlayer, gd);
                            UserService.转发消息结果(fromPlayer);
                            break;

                        case ClientCmdEnum.客户端心跳包:
                            HeartbeatCmd(session, package);
                            break;
                        case ClientCmdEnum.换线登陆:
                            index = 2 + 4 + 2;
                            int userID = ByteDataAnalysis.GetInt(gd.Two, ref index);
                            userID = 1;
                            //Player.User = SephirothDB.BLL.Res_users.GetItem(userID);
                            //ServerService.发送服务器名称(Player);
                            ////发送角色信息在登陆前显示在，再点一下才能进去
                            //RoleService.f发送角色列表(Player);
                            //Tools.ShowMsg(Player.SessionID);
                            break;
                        case ClientCmdEnum.空指令:
                            break;
                        default:
                            string exMsg = string.Empty;
                            //这里日志都能生效
                            exMsg = ActionForServer.Try解析封包为实际逻辑(gd);
                            exMsg += "========要处理的其它指令=========" + exMsg;
                            Comm.CommService.ShowExceptionMsg(exMsg);
                            _logger.Error(exMsg);
                            // _logger.LogError("LogErrorLogError");

                            // frmMain.Instance._logger.Error("启动了服务器556677889999");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Comm.CommService.ShowExceptionMsg("指令测试结果异常：" + ex.Message + ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException != null)
                        {
                            Comm.CommService.ShowExceptionMsg("sql：" + ex.InnerException.Message);
                        }

                    }
                }
                finally
                {
                    //InstructionService.OtherEvent -= InstructionService_OtherEvent;
                }

            }
        }

        private void InstructionService_OtherEvent(ClientCmdEnum msg, OriginalData gd)
        {
            if (!msg.ToString().Contains("心跳"))
            {
                Comm.CommService.ShowExceptionMsg("OtherEvent：" + msg.ToString());
            }
        }

        private void loginPorcess(IAppSession session, BizPackageInfo package)
        {
            SessionforBiz Player = session as SessionforBiz;
            if (package != null)
            {

            }
            ByteBuff bg = new ByteBuff(package.kd.Two);
            //在这里有两种，一种是密码串，一种是用户重新登录
            string 密码串;
            if (package.kd.Two.Length == 30)
            {
                bg.Step(26);
                int 原ID = bg.GetInt();
                // 密码串 = S游戏玩家.m_二次登陆.GetValueOrDefault(原ID, "");
                //UserService.Userlist.TryGetValue(原ID, out 密码串);
                //if (密码串.Length == 0)
                //{
                //    throw new Exception("不存在的密码串!");
                //}
                //S游戏玩家.m_二次登陆.Remove(原ID);
                //UserService.Userlist.TryRemove(原ID, out 密码串);

            }
            else
            {
                bg.Step(18);//跳过开头 
                bg.Step(296 - 18 - 18);
                密码串 = bg.GetString();
            }



            //var find = UserService.Userlist.Where(p => p.Value == 密码串).FirstOrDefault();
            //int Playid = find.Key;
            //if (Playid == 0)
            //{
            //    Playid = 1;//暂时写死的 这个只是登陆器中的用户res_users
            //}

            //Player.User = SephirothDB.BLL.Res_users.GetItem(Playid);
            ////发送角色信息在登陆前显示在，再点一下才能进去
            //RoleService.f发送角色列表(Player);
        }


#pragma warning disable CS0169 // 从不使用字段“BizCommand.Timer时间检测定时器”
        System.Threading.Timer Timer时间检测定时器;
#pragma warning restore CS0169 // 从不使用字段“BizCommand.Timer时间检测定时器”

        private void HeartbeatCmd(IAppSession session, BizPackageInfo package)
        {
            //这里面的心跳中的 子方法不能异常。不然会导致心跳停止
            SessionforBiz PlayerSession = session as SessionforBiz;
            PacketProcess ap = new PacketProcess(PlayerSession);
            if (package != null)
            {
#pragma warning disable CS0168 // 声明了变量，但从未使用过
                try
                {
                    #region 收到客户机的心跳数据，作对应回复

                    KxData gd = new KxData();
                    gd = package.kd;
                    int index = 0;
                    Int16 累加数 = ByteDataAnalysis.GetInt16(gd.Two, ref index);
                    int falg = ByteDataAnalysis.GetInt(gd.Two, ref index);
                    Int64 userid = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                    string empName = ByteDataAnalysis.GetString(gd.Two, ref index);
                    string ver = ByteDataAnalysis.GetString(gd.Two, ref index);
                    long ComputerFreeTime = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                    string strCurrentFormUI = ByteDataAnalysis.GetString(gd.Two, ref index);
                    string strCurrentModule = ByteDataAnalysis.GetString(gd.Two, ref index);
                    bool 在线状态 = ByteDataAnalysis.Getbool(gd.Two, ref index);
                    bool 授权状态 = ByteDataAnalysis.Getbool(gd.Two, ref index);
                    string ClientDataTime = ByteDataAnalysis.GetString(gd.Two, ref index);
                    DateTime clientTime = DateTime.MinValue;
                    if (DateTime.TryParse(ClientDataTime, out clientTime))
                    {
                        DateTime currentTime = DateTime.Now;
                        TimeSpan timeDifference = currentTime - clientTime;
                        if (Math.Abs(timeDifference.TotalHours) > 1)
                        {
                            frmMain.Instance.PrintInfoLog($"{empName}的客户端电脑时间异常，请检查!");
                            //客户端时间与服务器时间相差1小时以上，则断开连接
                            if (PlayerSession.State == SuperSocket.Server.Abstractions.SessionState.Connected)
                            {
                                //发出一个提示？
                                // UserService.发消息给客户端()
                                //这里是强制用户退出，让客户端自动断开服务器。
                                _logger.LogError($"{empName}的客户端电脑时间与服务器的差值异常强制用户退出，请检查!");
                                UserService.强制用户退出(PlayerSession);
                            }
                        }
                    }

                    System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    //还可以添加锁定等状态
                    PlayerSession.User.心跳数 = 累加数.ObjToInt();
                    PlayerSession.User.最后心跳时间 = DateTime.Now.ToString();
                    PlayerSession.User.客户端版本 = ver;
                    PlayerSession.User.静止时间 = ComputerFreeTime;
                    PlayerSession.User.当前窗体 = strCurrentFormUI;
                    PlayerSession.User.当前模块 = strCurrentModule;
                    PlayerSession.User.在线状态 = 在线状态;
                    PlayerSession.User.授权状态 = 授权状态;


                    //回复是更新状态：包括在线用户列表
                    //UserService.回复心跳(PlayerSession);

                    #endregion
                }
                catch (Exception ex)
                {
                    Console.WriteLine("HeartbeatCmd时出错" + ex.Message);
                }
#pragma warning restore CS0168 // 声明了变量，但从未使用过
            }
        }
        private async Task time服务(SessionforBiz Player)
        {
            // CommandServer.MessageService.Send广播消息ByColor(Player, System.DateTime.Now.ToString() + "等待3秒");
            //等待3秒
            await Task.Delay(3000);
            while (true)
            {
                try
                {
                    Thread.Sleep(300);
                    //超时等待
                    //await Task.Delay(interval, token);
                    //等待3秒
                    //CommandServer.MessageService.Send广播消息ByColor(Player, System.DateTime.Now.ToString() + "等待5秒");
                    await Task.Delay(5000);
                }
                catch (Exception)
                {
                    break;
                }



            }
        }


    }

}