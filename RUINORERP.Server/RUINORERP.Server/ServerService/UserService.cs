using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Server.Lib;
using RUINORERP.Server.ServerSession;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.DataPortal;

namespace RUINORERP.Server.BizService
{
    public class UserService
    {



        public async static Task<tb_UserInfo> 接收用户登陆指令(SessionforBiz UserSession, OriginalData gd)
        {
            tb_UserInfo user = null;
            try
            {
                int index = 0;
                string 登陆时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                var UserName = ByteDataAnalysis.GetString(gd.Two, ref index);
                var pwd = ByteDataAnalysis.GetString(gd.Two, ref index);

                string msg = string.Empty;

                user = await Program.AppContextData.Db.CopyNew().Queryable<tb_UserInfo>()
                    .Where(u => u.UserName == UserName && u.Password == pwd)
                    .Includes(x => x.tb_employee)
                    .Includes(x => x.tb_User_Roles)
                    .SingleAsync();
                if (user != null)
                {
                    //登陆成功
                    UserSession.User.UserName = user.UserName;
                    if (user.tb_employee != null)
                    {
                        UserSession.User.EmpName = user.tb_employee.Employee_Name;
                    }
                    //登陆时间
                    UserSession.User.ClientInfo.LoginTime = System.DateTime.Now;

                    //通知客户端
                    OriginalData exMsg = new OriginalData();
                    exMsg.cmd = (byte)ServerCmdEnum.给客户端发提示消息;
                    exMsg.One = null;
                    //这种可以写一个扩展方法
                    ByteBuff tx = new ByteBuff(100);
                    tx.PushString("用户【" + UserSession.User.EmpName + "】登陆成功");
                    exMsg.Two = tx.toByte();
                    UserSession.AddSendData(exMsg);
                }
                else
                {
                    //通知客户端
                    OriginalData exMsg = new OriginalData();
                    exMsg.cmd = (byte)ServerCmdEnum.给客户端发提示消息;
                    exMsg.One = null;
                    //这种可以写一个扩展方法
                    ByteBuff tx = new ByteBuff(100);
                    tx.PushString("用户登陆出错，用户名或密码错误！");
                    exMsg.Two = tx.toByte();
                    UserSession.AddSendData(exMsg);
                }
            }
            catch (Exception ex)
            {
                Tools.ShowMsg("用户登陆:" + ex.Message);
            }
            return user;
        }

        public static bool 用户登陆回复(SessionforBiz PlayerSession, tb_UserInfo user)
        {
            bool rs = false;
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {
                //PacketProcess pp = new PacketProcess(PlayerSession);
                ByteBuff tx = new ByteBuff(100);
                if (user != null)
                {
                    rs = true;
                    tx.PushBool(rs);
                    tx.PushString(PlayerSession.SessionID);
                    tx.PushInt64(user.User_ID);
                    tx.PushString(user.UserName);
                    tx.PushString(user.tb_employee.Employee_Name);

                }
                else
                {
                    rs = false;
                    tx.PushBool(rs);
                }
                PlayerSession.AddSendData((byte)ServerCmdEnum.用户登陆回复, null, tx.toByte());


                // PlayerSession.AddSendData(buffer);

                return rs;
            }
            catch (Exception ex)
            {
                rs = false;
            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过
            return rs;
        }

        public static void 发送在线列表(SessionforBiz PlayerSession)
        {
            try
            {
                ByteBuff tx = new ByteBuff(100);
                tx.PushInt(frmMain.Instance.sessionListBiz.Count);
                foreach (var item in frmMain.Instance.sessionListBiz)
                {

                    tx.PushString(item.Value.SessionID);
                    tx.PushString(item.Value.User.UserName);
                    tx.PushString(item.Value.User.EmpName);
                }

                PlayerSession.AddSendData((byte)ServerCmdEnum.发送在线列表, null, tx.toByte());
            }
            catch (Exception ex)
            {
                Tools.ShowMsg("用户登陆:" + ex.Message);
            }

        }

        public static void 回复心跳(SessionforBiz PlayerSession, ByteBuff tx)
        {
            try
            {

                PlayerSession.AddSendData((byte)ServerCmdEnum.心跳回复, null, tx.toByte());
            }
            catch (Exception ex)
            {
                Tools.ShowMsg("用户登陆:" + ex.Message);
            }

        }

        public static bool 转发消息(SessionforBiz PlayerSession, OriginalData gd)
        {
            bool rs = false;

#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {

                int index = 0;
                string sendtime = ByteDataAnalysis.GetString(gd.Two, ref index);
                string sessonid = ByteDataAnalysis.GetString(gd.Two, ref index);
                string receiver = ByteDataAnalysis.GetString(gd.Two, ref index);
                var Message = ByteDataAnalysis.GetString(gd.Two, ref index);
                SessionforBiz sb = null;
                frmMain.Instance.sessionListBiz.TryGetValue(sessonid, out sb);
                if (sb != null)
                {
                    ByteBuff tx = new ByteBuff(100);
                    rs = true;
                    tx.PushString(sendtime);
                    tx.PushString(sb.SessionID);
                    tx.PushString(sb.User.EmpName);//sender
                    tx.PushString(Message);
                    sb.AddSendData((byte)ServerCmdEnum.转发消息, null, tx.toByte());
                }
                return rs;
            }
            catch (Exception ex)
            {
                rs = false;
            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过
            return rs;
        }

        public static bool 转发消息结果(SessionforBiz PlayerSession)
        {
            bool rs = false;
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {
                //PacketProcess pp = new PacketProcess(PlayerSession);
                ByteBuff tx = new ByteBuff(100);
                rs = true;
                tx.PushBool(rs);
                // tx.PushString(Message);
                PlayerSession.AddSendData((byte)ServerCmdEnum.转发消息结果, null, tx.toByte());
                // PlayerSession.AddSendData(buffer);

                return rs;
            }
            catch (Exception ex)
            {
                rs = false;
            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过
            return rs;
        }
        public static void 强制用户退出(SessionforBiz PlayerSession)
        {
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {
                ByteBuff tx = new ByteBuff(50);
                tx.PushString(System.DateTime.Now.ToString());
                tx.PushString("强制用户退出");
                PlayerSession.AddSendData((byte)ServerCmdEnum.强制用户退出, null, tx.toByte());
            }
            catch (Exception ex)
            {

            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过

        }

        public static void 删除列配置文件(SessionforBiz PlayerSession)
        {
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {
                ByteBuff tx = new ByteBuff(50);
                tx.PushString(System.DateTime.Now.ToString());
                tx.PushString("删除列配置文件");
                PlayerSession.AddSendData((byte)ServerCmdEnum.删除列的配置文件, null, tx.toByte());
            }
            catch (Exception ex)
            {

            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过

        }

        public static void 强制用户关机(SessionforBiz PlayerSession)
        {
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {
                ByteBuff tx = new ByteBuff(50);
                tx.PushString(System.DateTime.Now.ToString());
                tx.PushString("强制用户关机");
                PlayerSession.AddSendData((byte)ServerCmdEnum.关机, null, tx.toByte());
            }
            catch (Exception ex)
            {

            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过

        }

        public static void 发消息给客户端(SessionforBiz PlayerSession)
        {
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {
                ByteBuff tx = new ByteBuff(50);
                tx.PushString(System.DateTime.Now.ToString());
                tx.PushString("admin");
                tx.PushString("Server");
                tx.PushString("你好，这只是一个消息测试");
                PlayerSession.AddSendData((byte)ServerCmdEnum.给客户端发提示消息, null, tx.toByte());
            }
            catch (Exception ex)
            {

            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过

        }


        public static void 推送版本更新(SessionforBiz PlayerSession)
        {
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {
                ByteBuff tx = new ByteBuff(50);
                tx.PushString(System.DateTime.Now.ToString());
                tx.PushString("你好，系统向您推送新的版本，需要您更新。");
                PlayerSession.AddSendData((byte)ServerCmdEnum.推送版本更新, null, tx.toByte());
            }
            catch (Exception ex)
            {

            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过

        }

    }

}

