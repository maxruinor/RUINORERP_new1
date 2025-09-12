using Azure.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.CommService;
using RUINORERP.Extensions.ServiceExtensions;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.ServerSession;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransInstruction;
using TransInstruction.DataPortal;
using TransInstruction.Enums;
using static RUINORERP.Extensions.ServiceExtensions.EditConfigCommand;
using MessageType = RUINORERP.Model.TransModel.MessageType;

namespace RUINORERP.Server.CommandService
{
    /// <summary>
    /// 服务器接收客户端传送的
    /// 管理员修改重要配置时，传到服务器会保存一份。再转发给在线的客户。如果不在线的。则会获取服务器上的那一份。
    /// 如果是转发有指定的人。则要等待这人上线后再转发。
    /// </summary>
    public class ReceiveEntityTransferCmd : IServerCommand
    {
        public EntityTransferCmdType requestType { get; set; }

        public NextProcesszStep nextProcesszStep { get; set; }

        /// <summary>
        /// 指定的目标接收实体的对象
        /// </summary>
        public SessionforBiz ToSession { get; set; }
        /// <summary>
        /// 处理的对象
        /// </summary>
        public object TransferObject { get; set; }

        public string TransferObjectName { get; set; }

        public SessionforBiz FromSession { get; set; }
        public CmdOperation OperationType { get; set; }
        public OriginalData DataPacket { get; set; }

        public ReceiveEntityTransferCmd(CmdOperation operation = CmdOperation.Receive)
        {
            OperationType = operation;
        }

        public ReceiveEntityTransferCmd(OriginalData gd, SessionforBiz FromSession, CmdOperation operation = CmdOperation.Receive)
        {
            OperationType = operation;
            this.FromSession = FromSession;
            this.DataPacket = gd;
        }


        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // 添加产品逻辑
            await Task.Run(
                () =>
                {
                    try
                    {
                        switch (OperationType)
                        {
                            case CmdOperation.Receive:
                                AnalyzeDataPacket(DataPacket, FromSession);

                                #region 根据不同情况处理提醒数据
                                //只是一行做对。为了编译通过
                                if (requestType == EntityTransferCmdType.处理动态配置)
                                {
                                    string basePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), GlobalConstants.DynamicConfigFileDirectory);
                                    ConfigFileReceiver _configFileReceiver = new ConfigFileReceiver(basePath + "/" + TransferObject.GetType().Name + ".json");
                                    //保存到本地
                                    //JObject obj = JObject.FromObject(TransferObject);
                                    // 创建一个新的JObject，并将GlobalValidatorConfig作为根节点
                                    JObject LastConfigJson = new JObject(new JProperty(TransferObject.GetType().Name, JObject.FromObject(TransferObject)));
                                    EditConfigCommand command = new EditConfigCommand(_configFileReceiver, LastConfigJson);
                                    CommandManager _commandManager = new CommandManager();
                                    _commandManager.ExecuteCommand(command);
                                    if (nextProcesszStep == NextProcesszStep.转发)
                                    {
                                        //动态配置转发到所有在线用户。不在线的。登陆时就会自动取服务器的一次
                                        BuildDataPacket(null);
                                    }

                                }
                                #endregion


                                break;
                            case CmdOperation.Send:
                                BuildDataPacket(null);
                                break;
                            default:
                                break;
                        }


                    }
                    catch (Exception ex)
                    {


                    }

                }
                ,
                cancellationToken
                );
        }


        public bool AnalyzeDataPacket(OriginalData gd, SessionforBiz FromSession)
        {
            bool rs = false;
            try
            {
                int index = 0;
                //当前
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                string TransferObjectName = ByteDataAnalysis.GetString(gd.Two, ref index);
                string json = ByteDataAnalysis.GetString(gd.Two, ref index);
                requestType = (EntityTransferCmdType)ByteDataAnalysis.GetInt(gd.Two, ref index);
                nextProcesszStep = (NextProcesszStep)ByteDataAnalysis.GetInt(gd.Two, ref index);

                // 将json转换为JObject
                JObject obj = JObject.Parse(json);
                switch (TransferObjectName)
                {
                    case nameof(GlobalValidatorConfig):
                        TransferObject = obj.ToObject<GlobalValidatorConfig>();
                        break;
                    case nameof(SystemGlobalconfig):
                        TransferObject = obj.ToObject<SystemGlobalconfig>();
                        break;
                }

            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("接收请求1:" + ex.Message);
            }
            return rs;
        }

        public void BuildDataPacket(object request = null)
        {
            try
            {
                //MessageBase messageBase = null;
                //发送消息数据 
                //将消息转换为一个实体先
                switch (requestType)
                {
                    case EntityTransferCmdType.接受实体数据:
                        // messageBase = new AddReminderMessage();
                        break;
                    case EntityTransferCmdType.处理动态配置:
                        // messageBase = new DeleteReminderMessage();
                        break;
                    default:
                        break;
                }

                ByteBuff tx = new ByteBuff(100);
                //发送缓存数据
                string json = JsonConvert.SerializeObject(TransferObject,
                   new JsonSerializerSettings
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                   });
                //怕时间格式不对。用这个保险
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushString(TransferObject.GetType().Name);
                tx.PushString(json);
                tx.PushInt((int)requestType);
                tx.PushInt((int)0);
                //tx.PushString(FromSession.SessionID);来源是不是要加上？

                foreach (var item in frmMain.Instance.sessionListBiz.ToArray())
                {
                    //跳过自己
                    if (FromSession != null && item.Value.SessionID == FromSession.SessionID)
                    {
                        continue;
                    }
                    //有指定目标时  其它人就不发了。
                    if (ToSession != null && item.Value.SessionID == ToSession.SessionID)
                    {
                        item.Value.AddSendData((byte)ServerCmdEnum.复合型实体处理, new byte[] { (byte)requestType }, tx.toByte());
                        break;
                    }
                    item.Value.AddSendData((byte)ServerCmdEnum.复合型实体处理, new byte[] { (byte)requestType }, tx.toByte());
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
