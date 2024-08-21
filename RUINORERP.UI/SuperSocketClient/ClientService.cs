using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.DataModel;
using TransInstruction.DataPortal;

namespace RUINORERP.UI.SuperSocketClient
{
    /// <summary>
    /// 解析来自服务器的指令及含义
    /// </summary>
    public class ClientService
    {


        public static bool 用户登陆回复(OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                bool islogin = ByteDataAnalysis.Getbool(gd.Two, ref index);
                rs = islogin;
                if (islogin)
                {
                    string SessionID = ByteDataAnalysis.GetString(gd.Two, ref index);
                    int userid = ByteDataAnalysis.GetInt(gd.Two, ref index);
                    string userName = ByteDataAnalysis.GetString(gd.Two, ref index);
                    string empName = ByteDataAnalysis.GetString(gd.Two, ref index);
                    OnlineUserInfo onlineuser = new TransInstruction.DataModel.OnlineUserInfo();
                    onlineuser.SessionId = SessionID;
                    onlineuser.UserID = userid;
                    onlineuser.UserName = userName;
                    onlineuser.EmpName = empName;
                    onlineuser.Online = true;
                    MainForm.Instance.ecs.CurrentUser = onlineuser;
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("用户登陆:" + ex.Message);
            }
            return rs;

        }


        public static bool 接收在线用户列表(OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                int counter = ByteDataAnalysis.GetInt(gd.Two, ref index);
                //清空
                MainForm.Instance.ecs.UserInfos = new List<TransInstruction.DataModel.OnlineUserInfo>();
                for (int i = 0; i < counter; i++)
                {
                    string sessionID = ByteDataAnalysis.GetString(gd.Two, ref index);
                    string userName = ByteDataAnalysis.GetString(gd.Two, ref index);
                    string EmpName = ByteDataAnalysis.GetString(gd.Two, ref index);

                    TransInstruction.DataModel.OnlineUserInfo userinfo = new TransInstruction.DataModel.OnlineUserInfo();
                    userinfo.SessionId = sessionID;
                    userinfo.UserName = userName;
                    userinfo.EmpName = EmpName;
                    userinfo.Online = true;
                    MainForm.Instance.ecs.UserInfos.Add(userinfo);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("用户登陆:" + ex.Message);
            }
            return rs;

        }



    }
}
