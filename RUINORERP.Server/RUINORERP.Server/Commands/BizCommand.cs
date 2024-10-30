using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Server.Commands;
using RUINORERP.Server.Lib;
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
        public BizCommand(IMemoryCache cache)
        {
            _cache = cache;
        }
        public async ValueTask ExecuteAsync(IAppSession session, BizPackageInfo package)
        {
            SessionforBiz Player = session as SessionforBiz;
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

                    }
                    switch (CleintCmd)
                    {
                        //按类型 工作流的 可以做子指令，暂时为了测试快。先不处理了
                        case ClientCmdEnum.工作流审批:

                            WorkflowServiceReceiver.接收审批结果事件推送(Player, gd);
                            break;
                        case ClientCmdEnum.工作流启动:

                            WorkflowServiceReceiver.启动工作流(Player, gd);

                            //  WorkflowServiceReceiver.接收工作流提交(Player, gd);
                            break;
                        case ClientCmdEnum.工作流指令:

                            WorkflowServiceReceiver.接收工作流审批(Player, gd);
                            break;
                        case ClientCmdEnum.准备登陆:
                            byte[] source = gd.Two;
                            try
                            {
                                string msg = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                            }
                            catch (Exception ex)
                            {
                                rs = ex.Message;

                            }
                            break;

                        case ClientCmdEnum.用户登陆:
                            _cache.Set("用户登陆", "用户登陆");

                            // var obj = CacheHelper.Instance.GetEntity<tb_CustomerVendor>(1740971599693221888);

                            tb_UserInfo user = await UserService.接收用户登陆指令(Player, gd);
                            if (UserService.用户登陆回复(Player, user))
                            {
                                UserService.发送在线列表(Player);
                            }
                            break;

                        case ClientCmdEnum.请求缓存:
                            index = 0;
                            string datatime = ByteDataAnalysis.GetLongString(gd.Two, ref index);
                            string tn = ByteDataAnalysis.GetLongString(gd.Two, ref index);
                            foreach (var tableName in BizCacheHelper.Manager.NewTableList.Keys)
                            {
                                UserService.发送缓存数据列表(Player, tableName);
                            }
                            break;

                        case ClientCmdEnum.发送消息:
                            UserService.转发消息(Player, gd);
                            UserService.转发消息结果(Player);

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



                        default:
                            //
                            string ss = "";
                            //角色上线后的操作指令(session, package);
                            ss = ActionForServer.Try解析封包为实际逻辑(gd);
                            Tools.ShowMsg("========要处理的其它指令=========" + ss);

                            break;
                    }
                }
                catch (Exception ex)
                {
                    Tools.ShowMsg("指令测试结果异常：" + ex.Message + ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException != null)
                        {
                            Tools.ShowMsg("sql：" + ex.InnerException.Message);
                        }

                    }
                }
                finally
                {
                    //InstructionService.OtherEvent -= InstructionService_OtherEvent;
                }

                //if (!(rs.Contains("心跳") || rs.Contains("等待")))
                //{
                //    Tools.ShowMsg("客户端指令测试结果：" + rs);
                //}
            }
        }

        private void InstructionService_OtherEvent(ClientCmdEnum msg, OriginalData gd)
        {
            if (!msg.ToString().Contains("心跳"))
            {
                Tools.ShowMsg("OtherEvent：" + msg.ToString());
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
        Timer Timer时间检测定时器;
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
                    //人物
                    //byte r = ByteDataAnalysis.Getbyte(gd.Two, ref index);
                    string empName = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                    string ver = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                    long ComputerFreeTime = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                    string strCurrentFormUI = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                    string strCurrentModule = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                    PlayerSession.User.心跳数 = 累加数.ObjToInt();
                    PlayerSession.User.最后心跳时间 = DateTime.Now;
                    PlayerSession.User.客户端版本 = ver;
                    PlayerSession.User.静止时间 = ComputerFreeTime;
                    PlayerSession.User.当前窗体 = strCurrentFormUI;
                    PlayerSession.User.当前模块 = strCurrentModule;

                    //ByteBuff tx = new ByteBuff(100);
                    //tx.PushInt(frmMain.Instance.sessionListBiz.Count);
                    //foreach (var item in frmMain.Instance.sessionListBiz)
                    //{

                    //    tx.PushString(item.Value.SessionID);
                    //    tx.PushString(item.Value.User.用户名);
                    //    tx.PushString(item.Value.User.姓名);
                    //}
                    // frmMain.Instance.PrintInfoLog($"收到{PlayerSession.User.姓名 + "|" + empName}心跳包,累加数{累加数}");
                    //不用回复，如果规则不对，直接T出？
                    // UserService.回复心跳(PlayerSession, tx);

                    #endregion
                }
                catch (Exception ex)
                {


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