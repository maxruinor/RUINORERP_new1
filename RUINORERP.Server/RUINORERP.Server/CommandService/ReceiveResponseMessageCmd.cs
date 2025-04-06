using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.CommService;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.ServerSession;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransInstruction;
using TransInstruction.CommandService;
using TransInstruction.DataPortal;
using MessageType = RUINORERP.Model.TransModel.MessageType;

namespace RUINORERP.Server.CommandService
{

    public class ReceiveResponseMessageCmd : IServerCommand
    {
        public MessageType messageType { get; set; } = MessageType.Text;
        public PromptType promptType { get; set; }
        public string MessageContent { get; set; }
        public SessionforBiz FromSession { get; set; }
        public SessionforBiz ToSession { get; set; }
        public CmdOperation OperationType { get; set; }
        public OriginalData DataPacket { get; set; }

        public ReceiveResponseMessageCmd(CmdOperation operation, SessionforBiz FromSession = null, SessionforBiz ToSession = null)
        {
            this.OperationType = operation;
            this.ToSession = ToSession;
            this.FromSession = FromSession;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // 添加产品逻辑
            await Task.Run(
                () =>
              {
                  switch (OperationType)
                  {
                      case CmdOperation.Receive:
                          AnalyzeDataPacket(DataPacket, FromSession);
                          break;
                      case CmdOperation.Send:
                          BuildDataPacket(null);
                          break;
                      default:
                          break;
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
                messageType = (MessageType)ByteDataAnalysis.GetInt(gd.Two, ref index);
                switch (messageType)
                {
                    case MessageType.IM:
                        string message = ByteDataAnalysis.GetString(gd.Two, ref index);
                        NextProcesszStep nextProcesszStep = (NextProcesszStep)ByteDataAnalysis.GetInt(gd.Two, ref index);
                        string ReceiverSessionID = ByteDataAnalysis.GetString(gd.Two, ref index);
                        MessageContent = message;
                        if (nextProcesszStep == NextProcesszStep.转发)
                        {
                            if(frmMain.Instance.sessionListBiz.ContainsKey(ReceiverSessionID))
                            {
                                ToSession = frmMain.Instance.sessionListBiz[ReceiverSessionID];
                                BuildDataPacket(null);
                            }
                            else
                            {
                                //按姓名处理 保存到下次转发？
                            }
                           
                        }

                        break;
                    case MessageType.BusinessData:
                        //// 将item转换为JObject
                        //JObject obj = JObject.Parse(json);
                        //requestInfo = obj.ToObject<ReminderData>();
                        break;
                    default:
                        break;
                }



            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("接收请求3:" + ex.Message);
            }
            return rs;
        }

        public void BuildDataPacket(object request = null)
        {
            ByteBuff tx = new ByteBuff(100);
            try
            {

                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushInt((int)messageType);
                MessageBase messageBase = new();
                messageBase.Content = MessageContent;

                //发送消息数据 
                //将消息转换为一个实体先
                switch (messageType)
                {
                    case MessageType.Prompt:
                        tx.PushString(MessageContent);
                        tx.PushInt((int)promptType);
                        //如果有明确指向
                        if (ToSession != null)
                        {
                            tx.PushString(ToSession.SessionID);
                        }

                        break;
                    case MessageType.Text:
                        messageBase = new MessageBase();
                        break;
                    case MessageType.BusinessData:
                        string json = JsonConvert.SerializeObject(messageBase,
                  new JsonSerializerSettings
                  {
                      ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                  });
                        tx.PushString(json);
                        break;
                    case MessageType.Event:
                        break;
                    case MessageType.IM:
                        tx.PushString(MessageContent);
                        tx.PushInt((int)0);//告诉接收端没有下一步了
                        tx.PushString(FromSession.SessionID);
                        break;
                    default:
                        break;
                }
                if (ToSession == null)
                {
                    foreach (var item in frmMain.Instance.sessionListBiz.ToArray())
                    {
                        item.Value.AddSendData((byte)ServerCmdEnum.复合型消息处理, new byte[] { (byte)messageType }, tx.toByte());
                    }
                }
                else
                {
                    ToSession.AddSendData((byte)ServerCmdEnum.复合型消息处理, new byte[] { (byte)messageType }, tx.toByte());
                }
            }
            catch (Exception ex)
            {

            }


        }
    }
}
