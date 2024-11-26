using AutoUpdateTools;
using FastReport.Table;
using Microsoft.Extensions.Caching.Memory;
using Netron.GraphLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Windows.Forms;
using TransInstruction;
using TransInstruction.DataPortal;

namespace RUINORERP.UI.SuperSocketClient
{
    /// <summary>
    /// 解析来自服务器的指令及含义
    /// </summary>
    public class ClientService
    {

        public static void 请求缓存(string tableName)
        {
            OriginalData odforCache = ActionForClient.请求发送缓存(tableName);
            byte[] buffer = TransInstruction.CryptoProtocol.EncryptClientPackToServer(odforCache);
            MainForm.Instance.ecs.client.Send(buffer);
        }

        /// <summary>
        /// 解析服务器的回复 意义是？
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
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
                    long userid = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                    string userName = ByteDataAnalysis.GetString(gd.Two, ref index);
                    string empName = ByteDataAnalysis.GetString(gd.Two, ref index);
                    UserInfo onlineuser = new UserInfo();
                    onlineuser.SessionId = SessionID;
                    onlineuser.UserID = userid;
                    onlineuser.用户名 = userName;
                    onlineuser.姓名 = empName;

                    MainForm.Instance.AppContext.OnlineUser = onlineuser;
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("用户登陆:" + ex.Message);
            }
            return rs;

        }


        /// <summary>
        /// 接收回复用户已经登陆
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        public static bool 接收回复用户重复登陆(OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                bool alreadyLogged = ByteDataAnalysis.Getbool(gd.Two, ref index);
                if (alreadyLogged)
                {
                    rs = alreadyLogged;
                    string SessionID = ByteDataAnalysis.GetString(gd.Two, ref index);
                    //如果正好是自己登陆的。不算已经登陆
                    if (MainForm.Instance.AppContext.OnlineUser.SessionId == SessionID)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收回复用户重复登陆:" + ex.Message);
            }
            return rs;
        }

        /// <summary>
        /// 告诉服务器，我已登陆，原来用户在线也要强制下线。
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        public static bool 请求强制用户下线(string UserName)
        {
            bool rs = false;
            try
            {
                ByteBuff tx = new ByteBuff(100);
                tx.PushString(System.DateTime.Now.ToString());
                tx.PushString(UserName);
                //排除自己当前的SessionId
                tx.PushString(MainForm.Instance.AppContext.OnlineUser.SessionId);
                OriginalData gd = new OriginalData();
                gd.cmd = (byte)ClientCmdEnum.请求强制用户下线;
                gd.One = null;
                gd.Two = tx.toByte();
                byte[] buffer = TransInstruction.CryptoProtocol.EncryptClientPackToServer(gd);
                MainForm.Instance.ecs.client.Send(buffer);
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("请求强制用户下线:" + ex.Message);
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
                //清空
                MainForm.Instance.UserInfos = new List<UserInfo>();
                string json = ByteDataAnalysis.GetString(gd.Two, ref index);
                if (!string.IsNullOrEmpty(json))
                {
                    if (json != "null")
                    {

                        UserInfo[] userInfoList = JsonConvert.DeserializeObject<UserInfo[]>(json);
                        if (userInfoList != null)//(Newtonsoft.Json.Linq.JArray))
                        {
                            foreach (var item in userInfoList)
                            {
                                //上面已经清空了。
                                MainForm.Instance.UserInfos.Add(item);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收在线用户列表:" + ex.Message);
            }
            return rs;

        }
        public static bool 接收缓存信息列表(OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);

                //清空
                MainForm.Instance.CacheInfoList = new System.Collections.Concurrent.ConcurrentDictionary<string, CacheInfo>();
                string json = ByteDataAnalysis.GetString(gd.Two, ref index);
                if (!string.IsNullOrEmpty(json))
                {
                    if (json != "null")
                    {
                        CacheInfo[] CacheInfoList = JsonConvert.DeserializeObject<CacheInfo[]>(json);
                        if (CacheInfoList != null)//(Newtonsoft.Json.Linq.JArray))
                        {
                            foreach (var item in CacheInfoList)
                            {
                                MainForm.Instance.CacheInfoList.TryAdd(item.CacheName, item);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收缓存信息列表:" + ex.Message);
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
                string tablename = ByteDataAnalysis.GetString(gd.Two, ref index);
                string json = ByteDataAnalysis.GetString(gd.Two, ref index);
                if (!string.IsNullOrEmpty(json))
                {
                    if (json != "null")
                    {
                        object objList = JsonConvert.DeserializeObject(json);
                        if (objList != null && objList.GetType().Name == "JArray")//(Newtonsoft.Json.Linq.JArray))
                        {
                            var jsonlist = objList as Newtonsoft.Json.Linq.JArray;
                            MyCacheManager.Instance.UpdateEntityList(tablename, jsonlist);
                        }
                    }
                    if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                    {
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

        public static string 接收服务器提示消息(OriginalData gd)
        {
            string Message = "";
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                Message = ByteDataAnalysis.GetString(gd.Two, ref index);
                MainForm.Instance.PrintInfoLog(Message);
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器提示消息:" + ex.Message);
            }
            return Message;

        }

        public static string 接收服务器弹窗消息(OriginalData gd)
        {
            string Message = "";
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                string sendtime = ByteDataAnalysis.GetString(gd.Two, ref index);
                string SessionID = ByteDataAnalysis.GetString(gd.Two, ref index);
                string 发送者姓名 = ByteDataAnalysis.GetString(gd.Two, ref index);
                Message = ByteDataAnalysis.GetString(gd.Two, ref index);
                bool MustDisplay = ByteDataAnalysis.Getbool(gd.Two, ref index);
                if (!MustDisplay)
                {
                    MainForm.Instance.PrintInfoLog(Message);
                }
                else
                {
                    TranMessage MessageInfo = new TranMessage();
                    MessageInfo.SendTime = sendtime;
                    //  MessageInfo.SenderID = SessionID;
                    MessageInfo.SenderName = 发送者姓名;
                    MessageInfo.Content = Message;
                    MainForm.Instance.MessageList.Enqueue(MessageInfo);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器弹窗消息:" + ex.Message);
            }
            return Message;

        }


        public static void 接收服务器转发异常消息(OriginalData gd)
        {
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                string sendtime = ByteDataAnalysis.GetString(gd.Two, ref index);
                string SessionID = ByteDataAnalysis.GetString(gd.Two, ref index);
                string 发送者姓名 = ByteDataAnalysis.GetString(gd.Two, ref index);
                string 电脑机器名 = ByteDataAnalysis.GetString(gd.Two, ref index);
                string IP = ByteDataAnalysis.GetString(gd.Two, ref index);
                string Msg = ByteDataAnalysis.GetString(gd.Two, ref index);
                string ExCode = ByteDataAnalysis.GetString(gd.Two, ref index);
                bool MustDisplay = ByteDataAnalysis.Getbool(gd.Two, ref index);

                TranMessage MessageInfo = new TranMessage();
                MessageInfo.SendTime = sendtime;
                //  MessageInfo.Id = SessionID;
                MessageInfo.SenderName = 发送者姓名;
                MessageInfo.Content = Msg;
                MainForm.Instance.MessageList.Enqueue(MessageInfo);
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器转发异常消息:" + ex.Message);
            }

        }


        public static void 接收服务器转发的协助处理请求(OriginalData gd)
        {
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                string sendtime = ByteDataAnalysis.GetString(gd.Two, ref index);
                long RequestUserID = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                string RequestContent = ByteDataAnalysis.GetString(gd.Two, ref index);
                string BillData = ByteDataAnalysis.GetString(gd.Two, ref index);
                string BillType = ByteDataAnalysis.GetString(gd.Two, ref index);
                var userinfo = MainForm.Instance.UserInfos.FirstOrDefault(c => c.UserID == RequestUserID);
                TranMessage MessageInfo = new TranMessage();
                MessageInfo.SendTime = sendtime;
                //  MessageInfo.Id = SessionID;
                MessageInfo.SenderName = userinfo.姓名;
                MessageInfo.Content = RequestContent;
                //保存最新的协助处理请求信息 单据信息
                string PathwithFileName = System.IO.Path.Combine(Application.StartupPath + $"\\FormProperty\\Data\\{userinfo.姓名}", BillType + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".cache");
                System.IO.FileInfo fi = new System.IO.FileInfo(PathwithFileName);
                //判断目录是否存在
                if (!System.IO.Directory.Exists(fi.Directory.FullName))
                {
                    System.IO.Directory.CreateDirectory(fi.Directory.FullName);
                }
                File.WriteAllText(PathwithFileName, BillData);
                MainForm.Instance.MessageList.Enqueue(MessageInfo);

                if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                {
                    MainForm.Instance.PrintInfoLog($"收到服务器转发{userinfo.姓名}的协助处理请求！");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器转发的协助处理请求:" + ex.Message);
            }

        }

        internal static void 接收转发更新缓存(OriginalData gd)
        {
            try
            {
                int index = 0;
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                string tableName = ByteDataAnalysis.GetString(gd.Two, ref index);
                string json = ByteDataAnalysis.GetString(gd.Two, ref index);

                // 将item转换为JObject
                var obj = JObject.Parse(json);
                MyCacheManager.Instance.UpdateEntityList(tableName, obj);
                if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                {
                    MainForm.Instance.PrintInfoLog($"接收转发更新缓存{tableName}成功！");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收转发更新缓存:" + ex.Message);
            }
        }

        internal static void 接收转发删除缓存(OriginalData gd)
        {
            try
            {
                int index = 0;
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                string tableName = ByteDataAnalysis.GetString(gd.Two, ref index);
                string PKColName = ByteDataAnalysis.GetString(gd.Two, ref index);
                long PKValue = ByteDataAnalysis.GetInt64(gd.Two, ref index);

                MyCacheManager.Instance.DeleteEntityList(tableName, PKColName, PKValue);
                if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                {
                    MainForm.Instance.PrintInfoLog($"接收转发删除缓存{tableName}成功！");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收转发删除缓存:" + ex.Message);
            }
        }

        internal static void 接收转发单据锁定(OriginalData gd)
        {
            try
            {

                int index = 0;
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                long lockUserid = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                string lockName = ByteDataAnalysis.GetString(gd.Two, ref index);
                long billid = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                int BizType = ByteDataAnalysis.GetInt(gd.Two, ref index);

                if (!MainForm.Instance.LockInfoList.ContainsKey(billid))
                {
                    BillLockInfo lockInfo = new BillLockInfo();
                    lockInfo.LockedName = lockName;
                    lockInfo.BillID = billid;
                    lockInfo.LockedUserID = lockUserid;
                    lockInfo.Available = true;
                    lockInfo.BizType = BizType;
                    MainForm.Instance.LockInfoList.AddOrUpdate(billid, lockInfo, (key, oldValue) => lockInfo);
                    if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                    {
                        MainForm.Instance.PrintInfoLog($"接收转发单据锁定{BizType}成功！");
                    }
                }

                //using (ICacheEntry cacheEntry = MainForm.Instance.CacheLockTheOrder.CreateEntry(billid))
                //{

                //}

            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收转发单据锁定:" + ex.Message);
            }
        }

        internal static void 接收转发单据锁定释放(OriginalData gd)
        {
            try
            {
                int index = 0;
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                long lockUserid = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                string lockName = ByteDataAnalysis.GetString(gd.Two, ref index);
                long billid = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                int BizType = ByteDataAnalysis.GetInt(gd.Two, ref index);

                BillLockInfo lockInfo = null;
                if (MainForm.Instance.LockInfoList.TryGetValue(billid, out lockInfo))
                {
                    MainForm.Instance.LockInfoList.TryRemove(billid, out lockInfo);
                    if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                    {
                        MainForm.Instance.PrintInfoLog($"接收转发单据锁定释放{BizType}成功！");
                    }
                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收转发单据锁定释放:" + ex.Message);
            }
        }

        /// <summary>
        /// 服务器根据掉线的用户释放锁。主动发到客户端
        /// </summary>
        /// <param name="gd"></param>
        internal static void 接收根据锁定用户释放(OriginalData gd)
        {
            try
            {
                int index = 0;
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                long lockUserid = ByteDataAnalysis.GetInt64(gd.Two, ref index);

                var tempList = MainForm.Instance.LockInfoList.Where(c => c.Value.LockedUserID == lockUserid).ToList();
                foreach (var item in tempList)
                {
                    BillLockInfo lockInfo = null;
                    MainForm.Instance.LockInfoList.TryRemove(item.Key, out lockInfo);
                    if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                    {
                        MainForm.Instance.PrintInfoLog($"根据锁定用户释放{item.Key}成功！");
                    }
                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收转发单据锁定释放:" + ex.Message);
            }
        }

    }
}
