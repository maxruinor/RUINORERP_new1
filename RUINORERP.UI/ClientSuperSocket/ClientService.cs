using AutoUpdateTools;
using FastReport.DevComponents.DotNetBar;
using FastReport.Table;
using Fireasy.Common.Serialization;
using Krypton.Navigator;
using Microsoft.Extensions.Caching.Memory;
using MySqlX.XDevAPI;
using Netron.GraphLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Security;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Core.DataProcessing;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.IM;
using RUINORERP.UI.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.WebSockets;
using System.Windows.Forms;



namespace RUINORERP.UI.SuperSocketClient
{
    /// <summary>
    /// 解析来自服务器的指令及含义
    /// </summary>
    public class ClientService
    {

        //public static void 请求缓存(string tableName)
        //{
        //    OriginalData odforCache = ActionForClient.请求发送缓存(tableName);
        //    byte[] buffer = TransInstruction.CryptoProtocol.EncryptClientPackToServer(odforCache);
        //    MainForm.Instance.ecs.client.Send(buffer);

        //    if (AuthorizeController.GetShowDebugInfoAuthorization(MainForm.Instance.AppContext))
        //    {
        //        MainForm.Instance.uclog.AddLog($"请求缓存：{tableName}");
        //    }
        //}

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
                ByteBuffer bg = new ByteBuffer(gd.Two);
                bool islogin = ByteOperations.GetBool(gd.Two, ref index);
                rs = islogin;
                if (islogin)
                {
                    string SessionID = ByteOperations.GetString(gd.Two, ref index);
                    long userid = ByteOperations.GetInt64(gd.Two, ref index);
                    string userName = ByteOperations.GetString(gd.Two, ref index);
                    string empName = ByteOperations.GetString(gd.Two, ref index);
                    UserInfo onlineuser = new UserInfo();
                    onlineuser.SessionId = SessionID;
                    onlineuser.UserID = userid;
                    onlineuser.用户名 = userName;
                    onlineuser.姓名 = empName;

                    MainForm.Instance.AppContext.CurrentUser = onlineuser;
                    MainForm.Instance.AppContext.CurrentUser.客户端版本 = Program.ERPVersion;
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
                ByteBuffer bg = new ByteBuffer(gd.Two);
                bool alreadyLogged = ByteOperations.GetBool(gd.Two, ref index);
                if (alreadyLogged)
                {
                    rs = alreadyLogged;
                    string SessionID = ByteOperations.GetString(gd.Two, ref index);
                    //如果正好是自己登陆的。不算已经登陆
                    if (MainForm.Instance.AppContext.CurrentUser.SessionId == SessionID)
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
                ByteBuffer tx = new ByteBuffer(100);
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushString(UserName);
                //排除自己当前的SessionId
                tx.PushString(MainForm.Instance.AppContext.CurrentUser.SessionId);
                OriginalData gd = new OriginalData();
                gd.Cmd = (byte)ClientCmdEnum.请求强制用户下线;
                gd.One = null;
                gd.Two = tx.ToByteArray();
                byte[] buffer = TransInstruction.CryptoProtocol.EncryptClientPackToServer(gd);
                MainForm.Instance.ecs.client.Send(buffer);
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("请求强制用户下线:" + ex.Message);
            }
            return rs;
        }


        /// <summary>
        /// 告诉服务器，我已登陆，原来用户在线也要强制下线。
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        public static bool 请求强制登陆上线(string userName)
        {
            bool rs = false;
            try
            {
                ByteBuffer tx = new ByteBuffer(100);
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushString(userName);
                //排除自己当前的SessionId
                tx.PushString(MainForm.Instance.AppContext.CurrentUser.SessionId);
                OriginalData gd = new OriginalData();
                gd.Cmd = (byte)ClientCmdEnum.请求强制登陆上线;
                gd.One = null;
                gd.Two = tx.ToByteArray();
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
                ByteBuffer bg = new ByteBuffer(gd.Two);
                //清空
                MainForm.Instance.UserInfos = new List<UserInfo>();
                string json = ByteOperations.GetString(gd.Two, ref index);
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
                //ByteBuffer bg = new ByteBuffer(gd.Two);

                //清空
                MainForm.Instance.CacheInfoList = new System.Collections.Concurrent.ConcurrentDictionary<string, CacheInfo>();
                string json = ByteOperations.GetString(gd.Two, ref index);
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

        /// <summary>
        /// 是真正的缓存数据。按表名取的
        /// 2025-2-08 建议在服务器发送时加一个标记。是批量发送时。接收时只要添加式更新
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        public static bool 接收缓存数据列表(OriginalData gd)
        {
            bool rs = false;
            try
            {
                int index = 0;
                ByteBuffer bg = new ByteBuffer(gd.Two);
                string tablename = ByteOperations.GetString(gd.Two, ref index);
                string json = ByteOperations.GetString(gd.Two, ref index);
                if (!string.IsNullOrEmpty(json))
                {
                    if (json != "null")
                    {
                        object objList = JsonConvert.DeserializeObject(json);
                        if (objList != null && objList.GetType().Name == "JArray")//(Newtonsoft.Json.Linq.JArray))
                        {
                            JArray jsonlist = objList as Newtonsoft.Json.Linq.JArray;

                            // 转换为 List<T>，其中 T 是 MyClass 的类型
                            List<object> myList = ConvertJArrayToList(jsonlist, $"RUINORERP.Model.{tablename},RUINORERP.Model");
                            //List<dynamic> last = new List<dynamic>();
                            #region  转换为 List<T>
                            // Type elementType = TypeHelper.GetFirstArgumentType(listType);
                            Type elementType = null;
                            if (MyCacheManager.Instance.NewTableTypeList.TryGetValue(tablename, out elementType))
                            {
                                //foreach (var item in myList)
                                //{
                                //    try
                                //    {
                                //        var convertedItem = Convert.ChangeType(item, elementType);
                                //        //last.Add(convertedItem);
                                //    }
                                //    catch (InvalidCastException)
                                //    {
                                //        // 处理类型转换失败的情况
                                //    }
                                //}
                                //var newInstance = Activator.CreateInstance(elementType);
                                //// 这里需要根据具体情况实现属性值的复制 这个方法也可以。但是还要处理赋值，麻烦一些
                                //tlist.Add(newInstance as T);

                                #region  强类型 转换失败
                                //var lastlist = ((IEnumerable<dynamic>)convertedList).Select(item => Activator.CreateInstance(elementType)).ToList();
                                //tlist = lastlist as List<T>;
                                #endregion
                            }
                            #endregion
                            //var ss = (last as List<T>).GetType().FullName;
                            MyCacheManager.Instance.UpdateEntityList(tablename, myList);
                        }
                    }
                    if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                    {
                        // MainForm.Instance.PrintInfoLog($"接收缓存数据{tablename}成功！");
                    }

                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("用户登陆:" + ex.Message);
            }
            return rs;

        }


        // 假设 T 是具体类型的类名，例如 "MyNamespace.MyClass"
        //Type type = Type.GetType("MyNamespace.MyClass, MyAssembly");
        public static List<object> ConvertJArrayToList(JArray jsonlist, string typeName)
        {
            if (jsonlist == null)
                throw new ArgumentNullException(nameof(jsonlist));

            if (string.IsNullOrEmpty(typeName))
                throw new ArgumentException("typeName 不能为空", nameof(typeName));

            try
            {
                Type type = Type.GetType(typeName);
                if (type == null)
                    throw new ArgumentException($"无法找到类型: {typeName}");

                //MethodInfo toObjectMethod = typeof(JToken).GetMethod("ToObject");
                MethodInfo toObjectMethod = typeof(JToken).GetMethod("ToObject", new Type[] { });
                MethodInfo genericToObjectMethod = toObjectMethod.MakeGenericMethod(type);

                List<object> list = new List<object>(jsonlist.Count);
                foreach (JToken token in jsonlist)
                {
                    object item = genericToObjectMethod.Invoke(token, null);
                    list.Add(item);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("转换失败", ex);
            }
        }

        public static async Task 接收切换服务器消息(OriginalData gd)
        {
            try
            {
                int index = 0;
                ByteBuffer bg = new ByteBuffer(gd.Two);
                string sendtime = ByteOperations.GetString(gd.Two, ref index);
                string IpPort = ByteOperations.GetString(gd.Two, ref index);
                try
                {
                    string newIP = IpPort.Split(':')[0];
                    int newport = IpPort.Split(':')[1].ObjToInt();

                    if (MainForm.Instance.ecs == null)
                    {
                        MainForm.Instance.ecs = new EasyClientService();
                    }
                    if (MainForm.Instance.ecs.IsConnected)
                    {
                        MainForm.Instance.ecs.ServerIp = newIP;
                        MainForm.Instance.ecs.Port = newport;
                        MainForm.Instance.ecs.client.Closed -= MainForm.Instance.ecs.OnClientClosed;
                        bool stop = await MainForm.Instance.ecs.Stop();
                        if (stop)
                        {
                            bool connect = await MainForm.Instance.ecs.Reconnect();
                            if (connect)
                            {
                                //进行登陆验证。逻辑上一定通过

                                ServerAuthorizer serverAuthorizer = new ServerAuthorizer();
                                bool result = await serverAuthorizer.loginRunningOperationAsync(MainForm.Instance.ecs, UserGlobalConfig.Instance.UseName, UserGlobalConfig.Instance.PassWord, 3);
                                //UITools.SuperSleep(1000);
                                if (result)
                                {
                                    UserGlobalConfig.Instance.ServerIP = newIP;
                                    UserGlobalConfig.Instance.ServerPort = newport.ToString();
                                    UserGlobalConfig.Instance.Serialize();
                                    MainForm.Instance.PrintInfoLog("切换服务器成功");
                          
                                }
                                else
                                {
                                    MainForm.Instance.PrintInfoLog("切换服务器失败");
                                    MainForm.Instance.LogLock();
                                }
                            }
                        }
                        //bool connect = await MainForm.Instance.ecs.Connect();
                    }
                }
                catch (Exception ex)
                {

                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器提示消息:" + ex.Message);
            }
        }
        public static async Task 接收服务器提示消息(OriginalData gd)
        {
            try
            {
                int index = 0;
                ByteBuffer bg = new ByteBuffer(gd.Two);
                string modelName = ByteOperations.GetString(gd.Two, ref index);
                string messageJson = ByteOperations.GetString(gd.Two, ref index);
                JObject obj = JObject.Parse(messageJson);
                MessageModel MessageInfo = obj.ToObject<MessageModel>();
                bool MustDisplay = ByteOperations.GetBool(gd.Two, ref index);
                MainForm.Instance.PrintInfoLog(MessageInfo.msg);
                if (MessageInfo.msg.Contains("换IP"))
                {
                    try
                    {
                        string newIP = "192.168.0.254";
                        int port = 3001;
                        bool stop = await MainForm.Instance.ecs.Stop();
                        if (MainForm.Instance.ecs == null)
                        {
                            MainForm.Instance.ecs = new EasyClientService();
                        }
                        if (!MainForm.Instance.ecs.IsConnected)
                        {
                            MainForm.Instance.ecs.ServerIp = newIP;
                            MainForm.Instance.ecs.Port = port;
                            bool connect = await MainForm.Instance.ecs.Connect();
                        }
                    }
                    catch (Exception ex)
                    {


                    }

                }

                if (MustDisplay)
                {
                    if (MainForm.Instance.IsHandleCreated)
                    {
                        MainForm.Instance.Invoke(new Action(() =>
                        {
                            InstructionsPrompt instructionsPrompt = new InstructionsPrompt();
                            instructionsPrompt.btnAgree.Visible = false;
                            instructionsPrompt.btnRefuse.Visible = false;
                            instructionsPrompt.txtSender.Text = "服务器提示";
                            instructionsPrompt.txtSubject.Text = "服务器提示";
                            instructionsPrompt.Content = MessageInfo.msg;
                            instructionsPrompt.Show();
                            instructionsPrompt.TopMost = true;

                        }));
                    }

                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器提示消息:" + ex.Message);
            }
        }
        public static string 接收服务器心跳回复(OriginalData gd)
        {
            string Message = "";
            try
            {
                int index = 0;
                ByteBuffer bg = new ByteBuffer(gd.Two);

                //清空
                MainForm.Instance.UserInfos = new List<UserInfo>();
                string json = ByteOperations.GetString(gd.Two, ref index);
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
                MainForm.Instance.PrintInfoLog("接收服务器心跳回复:" + ex.Message);
            }
            return Message;

        }

        public static string 接收服务器弹窗消息(OriginalData gd)
        {
            string Message = "";
            if (gd.Two == null)
            {
                return Message;
            }
            try
            {
                int index = 0;
                ByteBuffer bg = new ByteBuffer(gd.Two);
                string sendtime = ByteOperations.GetString(gd.Two, ref index);
                string SessionID = ByteOperations.GetString(gd.Two, ref index);
                string 发送者姓名 = ByteOperations.GetString(gd.Two, ref index);
                Message = ByteOperations.GetString(gd.Two, ref index);
                bool MustDisplay = ByteOperations.GetBool(gd.Two, ref index);
                if (!MustDisplay)
                {
                    MainForm.Instance.PrintInfoLog(Message);
                }
                else
                {
                    ReminderData MessageInfo = new ReminderData();
                    MessageInfo.SendTime = sendtime;
                    //  MessageInfo.SenderID = SessionID;
                    MessageInfo.SenderEmployeeName = 发送者姓名;
                    MessageInfo.ReminderContent = Message;
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
                ByteBuffer bg = new ByteBuffer(gd.Two);
                string sendtime = ByteOperations.GetString(gd.Two, ref index);
                string SessionID = ByteOperations.GetString(gd.Two, ref index);
                string 发送者姓名 = ByteOperations.GetString(gd.Two, ref index);
                string 电脑机器名 = ByteOperations.GetString(gd.Two, ref index);
                string IP = ByteOperations.GetString(gd.Two, ref index);
                string Msg = ByteOperations.GetString(gd.Two, ref index);
                string ExCode = ByteOperations.GetString(gd.Two, ref index);
                bool MustDisplay = ByteOperations.GetBool(gd.Two, ref index);

                ReminderData MessageInfo = new ReminderData();
                MessageInfo.SendTime = sendtime;
                MessageInfo.messageCmd = MessageCmdType.ExceptionLog;
                MessageInfo.SenderEmployeeName = 发送者姓名;
                MessageInfo.ReminderContent = Msg;
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
                ByteBuffer bg = new ByteBuffer(gd.Two);

                string sendtime = ByteOperations.GetString(gd.Two, ref index);
                long RequestUserID = ByteOperations.GetInt64(gd.Two, ref index);
                string RequestEmpName = ByteOperations.GetString(gd.Two, ref index);
                string RequestContent = ByteOperations.GetString(gd.Two, ref index);
                string BillType = ByteOperations.GetString(gd.Two, ref index);
                string BillData = ByteOperations.GetString(gd.Two, ref index);
                var userinfo = MainForm.Instance.UserInfos.FirstOrDefault(c => c.UserID == RequestUserID);
                if (userinfo == null)
                {
                    userinfo = new();
                    userinfo.姓名 = RequestEmpName;
                }
                ReminderData MessageInfo = new ReminderData();
                MessageInfo.SendTime = sendtime;
                //  MessageInfo.Id = SessionID;
                MessageInfo.SenderEmployeeName = userinfo.姓名;
                MessageInfo.ReminderContent = RequestContent + "-" + BillType;
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

        /// <summary>
        /// 管理员发送到服务器。服务器再发到其它客户端，如果客户端不在线。则在线后接收
        /// </summary>
        /// <param name="gd"></param>
        internal static void 接收转发更新动态配置(OriginalData gd)
        {
            try
            {
                int index = 0;
                string 时间 = ByteOperations.GetString(gd.Two, ref index);
                string tableName = ByteOperations.GetString(gd.Two, ref index);
                string json = ByteOperations.GetString(gd.Two, ref index);

                // 将item转换为JObject
                var obj = JObject.Parse(json);
                MyCacheManager.Instance.UpdateEntityList(tableName, obj);
                if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                {
                    MainForm.Instance.PrintInfoLog($"接收转发更新动态配置{tableName}成功！");
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收转发更新动态配置:" + ex.Message);
            }
        }


        internal static void 接收转发更新缓存(OriginalData gd)
        {
            try
            {
                int index = 0;
                string 时间 = ByteOperations.GetString(gd.Two, ref index);
                string tableName = ByteOperations.GetString(gd.Two, ref index);
                string json = ByteOperations.GetString(gd.Two, ref index);

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
                string 时间 = ByteOperations.GetString(gd.Two, ref index);
                string tableName = ByteOperations.GetString(gd.Two, ref index);
                string PKColName = ByteOperations.GetString(gd.Two, ref index);
                long PKValue = ByteOperations.GetInt64(gd.Two, ref index);

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


        /*
      internal static void 接收转发单据锁定(OriginalData gd)
      {
          try
          {
              int index = 0;
              string 时间 = ByteOperations.GetString(gd.Two, ref index);
              long lockUserid = ByteOperations.GetInt64(gd.Two, ref index);
              string lockName = ByteOperations.GetString(gd.Two, ref index);
              long billid = ByteOperations.GetInt64(gd.Two, ref index);
              int BizType = ByteOperations.GetInt(gd.Two, ref index);

              if (!MainForm.Instance.LockInfoList.ContainsKey(billid))
              {
                  BillLockInfo lockInfo = new BillLockInfo();
                  lockInfo.LockedName = lockName;
                  lockInfo.BillID = billid;
                  lockInfo.LockedUserID = lockUserid;
                  lockInfo.Available = true;
                  lockInfo.BizType = BizType;
                  MainForm.Instance.LockInfoList.AddOrUpdate(billid, lockInfo, (key, oldValue) => lockInfo);
                  //因为启动系统就会接收数据。还没有等加载初始化对象
                  if (MainForm.Instance.authorizeController == null)
                  {
                      MainForm.Instance.authorizeController = Startup.GetFromFac<AuthorizeController>();
                  }
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
              string 时间 = ByteOperations.GetString(gd.Two, ref index);
              long lockUserid = ByteOperations.GetInt64(gd.Two, ref index);
              string lockName = ByteOperations.GetString(gd.Two, ref index);
              long billid = ByteOperations.GetInt64(gd.Two, ref index);
              int BizType = ByteOperations.GetInt(gd.Two, ref index);
              //菜单ID
              long MenuID = ByteOperations.GetInt(gd.Two, ref index);

              BillLockInfo lockInfo = null;
              if (MainForm.Instance.LockInfoList.TryGetValue(billid, out lockInfo))
              {
                  bool result = MainForm.Instance.LockInfoList.TryRemove(billid, out lockInfo);
                  if (result)
                  {
                      //应该通知界面解锁
                      if (MainForm.Instance.kryptonDockableWorkspace1.ActiveCell != null)
                      {
                          foreach (KryptonPage kp in MainForm.Instance.kryptonDockableWorkspace1.ActiveCell.Pages)
                          {
                              if (kp.Controls.Count > 0)
                              {
                                  if (kp.Controls[0].GetType().BaseType == typeof(BaseBillEdit))
                                  {
                                      ((BaseBillEdit)kp.Controls[0]).ReleaseLock(lockInfo);
                                  }
                              }
                          }
                      }

                  }

                  if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                  {
                      if (result)
                      {
                          MainForm.Instance.PrintInfoLog($"接收转发单据锁定释放{billid}{BizType}成功！");
                      }
                      else
                      {
                          MainForm.Instance.PrintInfoLog($"接收转发单据锁定释放{billid}{BizType}成功！");
                      }

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
              string 时间 = ByteOperations.GetString(gd.Two, ref index);
              long lockUserid = ByteOperations.GetInt64(gd.Two, ref index);

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
      */


        public static string 接收工作流的提醒消息(OriginalData gd)
        {
            string Message = "";
            try
            {
                int index = 0;
                ByteBuffer bg = new ByteBuffer(gd.Two);
                string sendtime = ByteOperations.GetString(gd.Two, ref index);
                string json = ByteOperations.GetString(gd.Two, ref index);

                // 将item转换为JObject
                var obj = JObject.Parse(json);
                bool MustDisplay = ByteOperations.GetBool(gd.Two, ref index);
                if (!MustDisplay)
                {
                    MainForm.Instance.PrintInfoLog(Message);
                }
                else
                {
                    ReminderData reminderData = obj.ToObject<ReminderData>();
                    MainForm.Instance.MessageList.Enqueue(reminderData);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器工作流的提醒消息:" + ex.Message);
            }
            return Message;

        }

    }
}
