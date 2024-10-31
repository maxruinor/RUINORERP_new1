using AutoUpdateTools;
using Netron.GraphLib;
using Newtonsoft.Json;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction;
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
                    string SessionID = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                    int userid = ByteDataAnalysis.GetInt(gd.Two, ref index);
                    string userName = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                    string empName = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                    UserInfo onlineuser = new UserInfo();
                    onlineuser.SessionId = SessionID;
                    onlineuser.UserID = userid;
                    onlineuser.用户名 = userName;
                    onlineuser.姓名 = empName;
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
                MainForm.Instance.ecs.UserInfos = new List<UserInfo>();
                for (int i = 0; i < counter; i++)
                {
                    string sessionID = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                    string userName = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                    string EmpName = ByteDataAnalysis.GetShortString(gd.Two, ref index);

                    UserInfo userinfo = new UserInfo();
                    userinfo.SessionId = sessionID;
                    userinfo.用户名 = userName;
                    userinfo.姓名 = EmpName;
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

        public static bool 接收缓存数据列表(OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                string tablename = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                string json = ByteDataAnalysis.GetLongString(gd.Two, ref index);
                if (!string.IsNullOrEmpty(json))
                {
                    if (json != "null")
                    {
                        object objList = JsonConvert.DeserializeObject(json);
                        MyCacheManager.Instance.AddCacheEntityList(objList, tablename);
                        MainForm.Instance.PrintInfoLog($"接收缓存数据{tablename}成功！");
                    }
              
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("用户登陆:" + ex.Message);
            }
            return rs;

        }


        public static string 接收服务器消息(OriginalData gd)
        {
            string Message = "";
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                string sendtime = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                string SessionID = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                string 姓名 = ByteDataAnalysis.GetShortString(gd.Two, ref index);
                Message = ByteDataAnalysis.GetLongString(gd.Two, ref index);
                bool MustDisplay = ByteDataAnalysis.Getbool(gd.Two, ref index);
                MainForm.Instance.ShowMsg(Message);
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("用户登陆:" + ex.Message);
            }
            return Message;

        }
    }
}
