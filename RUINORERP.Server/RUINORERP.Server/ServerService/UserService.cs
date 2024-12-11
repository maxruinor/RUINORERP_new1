using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using NetTaste;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Helper;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.Server.Comm;
using RUINORERP.Server.ServerService;
using RUINORERP.Server.ServerSession;
using RUINORERP.Services;
using RUINORERP.WF.BizOperation.Condition;
using SharpYaml.Tokens;
using SuperSocket.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.DataPortal;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RUINORERP.Server.BizService
{
    public class UserService
    {
        public static void 发送缓存数据(SessionforBiz PlayerSession, string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return;
            }
            try
            {
                if (BizCacheHelper.Manager.NewTableList.ContainsKey(tableName))
                {
                    //发送缓存数据
                    var cvOjb = BizCacheHelper.Instance.GetEntity<tb_CustomerVendor>(1740971599693221888);

                    string json = JsonConvert.SerializeObject(cvOjb,
                       new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                       });

                    ByteBuff tx = new ByteBuff(100);
                    tx.PushString(tableName);
                    tx.PushInt(frmMain.Instance.sessionListBiz.Count);
                    foreach (var item in frmMain.Instance.sessionListBiz)
                    {
                        tx.PushString(item.Value.SessionID);
                        tx.PushString(item.Value.User.用户名);
                        tx.PushString(item.Value.User.姓名);
                    }

                    PlayerSession.AddSendData((byte)ServerCmdEnum.发送缓存数据, null, tx.toByte());
                }
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据:" + ex.Message);
            }

        }

        public static void 发送缓存数据列表(SessionforBiz PlayerSession, string tableName)
        {
            try
            {
                if (BizCacheHelper.Manager.NewTableList.ContainsKey(tableName))
                {
                    //发送缓存数据
                    var CacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                    if (CacheList == null)
                    {
                        //启动时服务器都没有加载缓存，则不发送
                        BizCacheHelper.Instance.SetDictDataSource(tableName, true);
                        CacheList = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                    }
                    if (CacheList is JArray)
                    {
                        //暂时认为服务器的都是泛型形式保存的
                    }

                    if (TypeHelper.IsGenericList(CacheList.GetType()))
                    {
                        var lastlist = ((IEnumerable<dynamic>)CacheList).ToList();
                        if (lastlist != null)
                        {
                            int pageSize = 100; // 每页100行
                            for (int i = 0; i < lastlist.Count; i += pageSize)
                            {
                                // 计算当前页的结束索引，确保不会超出数组界限
                                int endIndex = Math.Min(i + pageSize, lastlist.Count);

                                // 获取当前页的JArray片段
                                object page = lastlist.Skip(i).Take(endIndex - i).ToArray();

                                // 处理当前页
                                发送缓存数据(PlayerSession, tableName, page);

                                // 如果当前页是最后一页，可能不足200行，需要特殊处理
                                if (endIndex == lastlist.Count)
                                {
                                    //处理最后一页的逻辑，如果需要的话
                                    //发送完成！
                                    if (frmMain.Instance.IsDebug)
                                    {
                                        frmMain.Instance.PrintMsg($"{tableName}最后一页发送完成,总行数:{endIndex}");
                                    }

                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据列表:" + ex.Message);
            }

        }


        private static void 发送缓存数据(SessionforBiz PlayerSession, string tableName, object list)
        {
            try
            {
                string json = JsonConvert.SerializeObject(list,
                      new JsonSerializerSettings
                      {
                          Converters = new List<JsonConverter> { new CustomCollectionJsonConverter() },
                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                      });
                ByteBuff tx = new ByteBuff(200);
                tx.PushString(tableName);
                tx.PushString(json);
                PlayerSession.AddSendData((byte)ServerCmdEnum.发送缓存数据列表, null, tx.toByte());
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("发送缓存数据:" + ex.Message);
            }

        }

        /// <summary>
        /// 同时分发出去了
        /// </summary>
        /// <param name="UserSession"></param>
        /// <param name="gd"></param>
        public static void 接收更新缓存指令(SessionforBiz UserSession, OriginalData gd)
        {
            try
            {
                int index = 0;
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                string tableName = ByteDataAnalysis.GetString(gd.Two, ref index);
                string json = ByteDataAnalysis.GetString(gd.Two, ref index);
                //更新服务器的缓存
                // 将item转换为JObject
                JObject obj = JObject.Parse(json);

                MyCacheManager.Instance.UpdateEntityList(tableName, obj);
                //再转发给其他客户端

                //发送缓存数据

                ByteBuff tx = new ByteBuff(200);
                tx.PushString(时间);
                tx.PushString(tableName);
                tx.PushString(json);

                foreach (var item in frmMain.Instance.sessionListBiz)
                {
                    //排除更新者自己
                    if (item.Key == UserSession.SessionID)
                    {
                        continue;
                    }
                    SessionforBiz sessionforBiz = item.Value as SessionforBiz;
                    sessionforBiz.AddSendData((byte)ServerCmdEnum.转发更新缓存, null, tx.toByte());

                    if (frmMain.Instance.IsDebug)
                    {
                        frmMain.Instance.PrintMsg($"转发更新的缓存{tableName}给：" + item.Value.User.姓名);
                    }
                }

                //如果是产品表有变化 还要需要更新产品视图的缓存
                if (tableName == nameof(tb_Prod))
                {
                    var prod = obj.ToObject<tb_Prod>();
                    BroadcastProdCatchData(UserSession, prod);
                }
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("接收更新缓存指令:" + ex.Message);
            }

        }


        /// <summary>
        /// 同时分发出去了。
        /// </summary>
        /// <param name="UserSession"></param>
        /// <param name="gd"></param>
        public static void 接收删除缓存指令(SessionforBiz UserSession, OriginalData gd)
        {
            try
            {
                int index = 0;
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                string tableName = ByteDataAnalysis.GetString(gd.Two, ref index);
                string PKColName = ByteDataAnalysis.GetString(gd.Two, ref index);
                long PKValue = ByteDataAnalysis.GetInt64(gd.Two, ref index);
                //删除服务器的缓存中的数据行
                MyCacheManager.Instance.DeleteEntityList(tableName, PKColName, PKValue);
                //再转发给其他客户端
                //发送缓存数据

                ByteBuff tx = new ByteBuff(200);
                tx.PushString(时间);
                tx.PushString(tableName);
                tx.PushString(PKColName);
                tx.PushInt64(PKValue);

                foreach (var item in frmMain.Instance.sessionListBiz)
                {
                    //排除更新者自己
                    if (item.Key == UserSession.SessionID)
                    {
                        continue;
                    }
                    SessionforBiz sessionforBiz = item.Value as SessionforBiz;
                    sessionforBiz.AddSendData((byte)ServerCmdEnum.转发删除缓存, null, tx.toByte());

                    if (frmMain.Instance.IsDebug)
                    {
                        frmMain.Instance.PrintMsg($"转发删除缓存{tableName}给：" + item.Value.User.姓名);
                    }
                }

                //如果是产品表有变化 还要需要更新产品视图的缓存
                if (tableName == nameof(tb_Prod))
                {
                    // var prod = obj.ToObject<tb_Prod>();
                    // BroadcastProdCatchData(UserSession, prod);
                }
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("接收更新缓存指令:" + ex.Message);
            }

        }




        private async static void BroadcastProdCatchData(SessionforBiz UserSession, tb_Prod prod)
        {
            View_ProdDetail ViewProdDetail = new View_ProdDetail();
            ViewProdDetail = await Program.AppContextData.Db.CopyNew().Queryable<View_ProdDetail>()
                .SingleAsync(p => p.ProdBaseID == prod.ProdBaseID);
            MyCacheManager.Instance.UpdateEntityList<View_ProdDetail>(ViewProdDetail);
            //发送缓存数据
            string json = JsonConvert.SerializeObject(ViewProdDetail,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
               });

            string tableName = nameof(View_ProdDetail);
            ByteBuff tx = new ByteBuff(200);
            tx.PushString(System.DateTime.Now.ToString());
            tx.PushString(tableName);
            tx.PushString(json);

            foreach (var item in frmMain.Instance.sessionListBiz)
            {
                //排除更新者自己
                if (item.Key == UserSession.SessionID)
                {
                    continue;
                }
                SessionforBiz sessionforBiz = item.Value as SessionforBiz;
                sessionforBiz.AddSendData((byte)ServerCmdEnum.转发更新缓存, null, tx.toByte());

                if (frmMain.Instance.IsDebug)
                {
                    frmMain.Instance.PrintMsg($"转发更新缓存{tableName}给：" + item.Value.User.姓名);
                }
            }
        }

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
                    UserSession.User.用户名 = user.UserName;
                    if (user.tb_employee != null)
                    {
                        UserSession.User.姓名 = user.tb_employee.Employee_Name;
                    }
                    //登陆时间
                    UserSession.User.登陆时间 = System.DateTime.Now;
                    UserSession.User.UserID = user.User_ID;
                    UserSession.User.超级用户 = user.IsSuperUser;
                    UserSession.User.在线状态 = true;
                    UserSession.User.授权状态 = true;
                    //通知客户端
                    UserService.给客户端发提示消息(UserSession, "用户【" + UserSession.User.姓名 + "】登陆成功");
                }
                else
                {
                    //通知客户端
                    UserService.给客户端发提示消息(UserSession, "用户登陆出错，用户名或密码错误！");
                }
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("接收用户登陆指令:" + ex.Message);
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


        public static bool 回复用户重复登陆(SessionforBiz PlayerSession, SessionforBiz ExistSessionforBiz = null)
        {
            bool rs = false;
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {
                //PacketProcess pp = new PacketProcess(PlayerSession);
                ByteBuff tx = new ByteBuff(100);
                if (ExistSessionforBiz== null)
                {
                    rs = false;
                    tx.PushBool(rs);
                }
                else
                {
                    if (ExistSessionforBiz.User != null)
                    {
                        rs = true;
                        tx.PushBool(rs);
                        tx.PushString(ExistSessionforBiz.User.SessionId);
                    }
                    else
                    {
                        rs = false;
                        tx.PushBool(rs);
                    }
                }
                
                PlayerSession.AddSendData((byte)ServerCmdEnum.回复用户重复登陆, null, tx.toByte());
                return rs;
            }
            catch (Exception ex)
            {
                rs = false;
            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过
            return rs;
        }

        /// <summary>
        /// 有人上线掉线都要通知客户端 可以优化
        /// </summary>
        /// <param name="PlayerSession"></param>
        public static void 发送在线列表(SessionforBiz PlayerSession)
        {
            try
            {
                ByteBuff tx = new ByteBuff(100);

                List<UserInfo> userInfos = new List<UserInfo>();

                // tx.PushInt(frmMain.Instance.sessionListBiz.Count);
                foreach (var item in frmMain.Instance.sessionListBiz)
                {
                    if (item.Value != null && item.Value.User != null)
                    {
                        userInfos.Add(item.Value.User);
                    }

                    //tx.PushString(item.Value.SessionID);
                    //tx.PushString(item.Value.User.用户名);
                    //tx.PushString(item.Value.User.姓名);
                    //tx.PushInt64(item.Value.User.UserID);
                }
                string json = JsonConvert.SerializeObject(userInfos,
                      new JsonSerializerSettings
                      {
                          Converters = new List<JsonConverter> { new CustomCollectionJsonConverter() },
                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                      });


                tx.PushString(json);


                PlayerSession.AddSendData((byte)ServerCmdEnum.发送在线列表, null, tx.toByte());
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("用户登陆:" + ex.Message);
            }

        }


        /// <summary>
        /// 将服务器缓存信息情况发过去。当客户端空闲时再请求缓存数据
        /// </summary>
        /// <param name="PlayerSession"></param>
        public static void 发送缓存信息列表(SessionforBiz PlayerSession)
        {
            try
            {
                ByteBuff tx = new ByteBuff(100);
                List<CacheInfo> CacheInfos = new List<CacheInfo>();
                foreach (var item in BizCacheHelper.Manager.NewTableList)
                {
                    CacheInfo cacheInfo = MyCacheManager.Instance.CacheInfoList.Get(item.Key) as CacheInfo;
                    if (cacheInfo != null)
                    {
                        CacheInfos.Add(cacheInfo);
                    }
                }
 
                string json = JsonConvert.SerializeObject(CacheInfos,
                      new JsonSerializerSettings
                      {
                          Converters = new List<JsonConverter> { new CustomCollectionJsonConverter() },
                          ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                      });


                tx.PushString(json);
                PlayerSession.AddSendData((byte)ServerCmdEnum.发送缓存信息列表, null, tx.toByte());
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("用户登陆:" + ex.Message);
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
                Comm.CommService.ShowExceptionMsg("用户登陆:" + ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PlayerSession"></param>
        /// <param name="Message"></param>
        /// <param name="MustDisplay"></param>
        /// <returns></returns>
        public static bool 给客户端发消息(SessionforBiz PlayerSession, string Message, bool MustDisplay)
        {
            bool rs = false;
            try
            {
                SessionforBiz sb = null;
                frmMain.Instance.sessionListBiz.TryGetValue(PlayerSession.SessionID, out sb);
                if (sb != null)
                {
                    string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ByteBuff tx = new ByteBuff(100);
                    rs = true;
                    tx.PushString(sendtime);
                    tx.PushString(sb.SessionID);
                    tx.PushString(sb.User.姓名);//sender
                    tx.PushString(Message);
                    tx.PushBool(MustDisplay);
                    sb.AddSendData((byte)ServerCmdEnum.给客户端发提示消息, null, tx.toByte());
                }
                return rs;
            }
            catch (Exception ex)
            {
                rs = false;
            }

            return rs;
        }
        public static bool 转发弹窗消息(SessionforBiz PlayerSession, OriginalData gd)
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
                    tx.PushString(sb.User.姓名);//sender
                    tx.PushString(Message);
                    sb.AddSendData((byte)ServerCmdEnum.转发弹窗消息, null, tx.toByte());
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

        public static bool 给客户端发提示消息(SessionforBiz UserSession, string Message)
        {
            bool rs = false;
            try
            {
                //通知客户端一条消息
                OriginalData exMsg = new OriginalData();
                exMsg.cmd = (byte)ServerCmdEnum.给客户端发提示消息;
                exMsg.One = null;
                ByteBuff tx = new ByteBuff(100);
                tx.PushString(Message);
                exMsg.Two = tx.toByte();
                UserSession.AddSendData(exMsg);

            }
            catch (Exception ex)
            {
                rs = false;
            }
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


        public static void 处理请求强制用户下线(OriginalData gd)
        {
            try
            {
                int index = 0;
                string 登陆时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                var UserName = ByteDataAnalysis.GetString(gd.Two, ref index);
                //要保存的用户。其它下线
                string SaveSessionId = ByteDataAnalysis.GetString(gd.Two, ref index);

                SessionforBiz UserSession = frmMain.Instance.sessionListBiz
                    .Values.FirstOrDefault(c => c.User.用户名 == UserName && !c.User.SessionId.Equals(SaveSessionId));
                if (UserSession != null && UserSession.State == SuperSocket.Server.Abstractions.SessionState.Connected)
                {
                    强制用户退出(UserSession, "有相同账号登陆，系统强制下线");
                }
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("处理请求强制用户下线:" + ex.Message);
            }

        }


        /// <summary>
        /// 这里是强制用户退出，让客户端自动断开服务器。
        /// T人也是用这个。因为如何从服务器断开。客户端还会重新连接。并不会关掉软件。
        /// </summary>
        /// <param name="PlayerSession"></param>
        public static void 强制用户退出(SessionforBiz PlayerSession, string Message = "")
        {
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {
                ByteBuff tx = new ByteBuff(50);
                tx.PushString(System.DateTime.Now.ToString());
                tx.PushString("强制用户退出");
                tx.PushString(Message);
                PlayerSession.AddSendData((byte)ServerCmdEnum.强制用户退出, null, tx.toByte());
            }
            catch (Exception ex)
            {
                Console.WriteLine("强制用户退出时出错" + ex.Message);
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
                Console.WriteLine("删除列配置文件时出错" + ex.Message);
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
                Console.WriteLine("强制用户关机时出错" + ex.Message);
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
                tx.PushString("你好，这只是一个消息测试");
                PlayerSession.AddSendData((byte)ServerCmdEnum.给客户端发提示消息, null, tx.toByte());
            }
            catch (Exception ex)
            {
                Console.WriteLine("发消息给客户端时出错" + ex.Message);
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
                Console.WriteLine("推送版本更新时出错" + ex.Message);
            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过

        }

    }

}

