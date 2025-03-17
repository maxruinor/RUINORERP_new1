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
using TransInstruction;
using TransInstruction.CommandService;
using TransInstruction.DataModel;
using TransInstruction.DataPortal;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 工作流提醒相关的请求
    /// </summary>
    public class RequestLoginCommand : IClientCommand
    {
        public CmdOperation OperationType { get; set; }
        public OriginalData DataPacket { get; set; }
        public LoginProcessType requestType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        //public void Execute(object parameters = null)
        //{
        //    ExecuteAsync(CancellationToken.None, parameters).GetAwaiter().GetResult();
        //}

        public RequestLoginCommand(CmdOperation operation)
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
                var tx = new ByteBuff(50);
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
            ByteBuff bg = new ByteBuff(gd.Two);
            string sendTime = ByteDataAnalysis.GetString(gd.Two, ref index);
            LoginProcessType loginCmd = (LoginProcessType)ByteDataAnalysis.GetInt(gd.Two, ref index);
            switch (loginCmd)
            {
                case LoginProcessType.登陆回复:
                    #region
                    try
                    {
                        //MainForm.Instance.AppContext.CurrentUser.授权状态 = false;
                        bool isSuccess = ByteDataAnalysis.Getbool(gd.Two, ref index);
                        if (isSuccess)
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
                    bool isOnline = ByteDataAnalysis.Getbool(gd.Two, ref index);
                    Program.AppContextData.AlreadyLogged = isOnline;
                    break;
                case LoginProcessType.超过限制:
                    #region
                    try
                    {
                        //被拒绝后的 得到服务器的通知
                        string json = ByteDataAnalysis.GetString(gd.Two, ref index);
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
