﻿/*****************************************************************************************
 * 【过时作废文件】OBSOLETE - DEPRECATED - DO NOT USE
 * 此文件已被废弃，不再维护和使用
 * 原因：ClientCmdService目录下的所有文件都已过时，实际已排除在项目外
 * 替代方案：请使用新的命令处理机制
 * 创建日期：系统自动标识
 *****************************************************************************************/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Global;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.SuperSocketClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

using RUINORERP.PacketSpec.Models;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 工作流提醒相关的请求
    /// </summary>
    public class RequestLoginCommand : IClientCommand
    {
        public CommandDirection OperationType { get; set; }
        public OriginalData DataPacket { get; set; }
    
        public string Username { get; set; }
        public string Password { get; set; }

        //public void Execute(object parameters = null)
        //{
        //    ExecuteAsync(CancellationToken.None, parameters).GetAwaiter().GetResult();
        //}

        public RequestLoginCommand(CommandDirection operation)
        {
            OperationType = operation;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, object parameters = null)
        {
            #region 执行方法

            switch (OperationType)
            {
                case CmdOperation.Send:
                    BuildDataPacket();
                    break;
                case CmdOperation.Receive:
                    AnalyzeDataPacket(DataPacket);
                    break;
                default:
                    break;
            }

            #endregion
            await Task.Run(
               () =>
            {
            
            }
               ,
            cancellationToken
               );


        }


        public void BuildDataPacket(object request = null)
        {
            OriginalData gd = new OriginalData();
            try
            {
                
                var tx = new ByteBuffer(50);
                string json = JsonConvert.SerializeObject(request,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
               });
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushString(Username);
                tx.PushString(Password);
                tx.PushInt((int)requestType);
                //将来再加上提醒配置规则,或加在请求实体中
                gd.cmd = (byte)ClientCmdEnum.复合型登陆请求;
                gd.One = new byte[] { (byte)requestType };
                gd.Two = tx.toByte();
            }
            catch (Exception)
            {

            }
            MainForm.Instance.ecs.AddSendData(gd);

        }

        public bool AnalyzeDataPacket(OriginalData gd)
        {
            int index = 0;
            ByteBuffer bg = new ByteBuffer(gd.Two);
            string sendTime = ByteOperations.GetString(gd.Two, ref index);
            LoginProcessType loginCmd = (LoginProcessType)ByteOperations.GetInt(gd.Two, ref index);
            switch (loginCmd)
            {
                case LoginProcessType.登陆回复:
                    #region
                    try
                    {
                        //MainForm.Instance.AppContext.CurrentUser.授权状态 = false;
                        bool isSuccess = ByteOperations.Getbool(gd.Two, ref index);
                        if (isSuccess)
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

                            Console.WriteLine($"登陆回复: {Thread.CurrentThread.ManagedThreadId}");
                            MainForm.Instance.AppContext.CurrentUser.授权状态 = isSuccess;
                            MainForm.Instance.AppContext.CurrentUser = onlineuser;
                            MainForm.Instance.AppContext.CurrentUser.客户端版本 = Program.ERPVersion;
                            MainForm.Instance.PrintInfoLog("登陆成功");
                        }
                        MainForm.Instance.ecs.LoginStatus = isSuccess;
                        Program.AppContextData.IsOnline = isSuccess;
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.PrintInfoLog("接收服务器提示消息:" + ex.Message);
                    }
                    #endregion
                    break;
                case LoginProcessType.已经在线:
                    bool isOnline = ByteOperations.Getbool(gd.Two, ref index);
                    Program.AppContextData.AlreadyLogged = isOnline;
                    break;
                case LoginProcessType.超过限制:
                    #region
                    try
                    {
                        //被拒绝后的 得到服务器的通知
                        string json = ByteOperations.GetString(gd.Two, ref index);
                        JObject obj = JObject.Parse(json);
                        RefuseUnLockInfo lockRequest = obj.ToObject<RefuseUnLockInfo>();

                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.PrintInfoLog("接收服务器提示消息:" + ex.Message);
                    }
                    #endregion
                    break;

                default:
                    break;
            }
            return true;
        }
    }



}
