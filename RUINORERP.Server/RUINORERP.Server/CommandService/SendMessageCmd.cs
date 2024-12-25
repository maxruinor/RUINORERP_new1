using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using RUINORERP.Business.CommService;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.ServerSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.CommandService;
using TransInstruction.DataPortal;
using MessageType = RUINORERP.Model.TransModel.MessageType;

namespace RUINORERP.Server.CommandService
{
    public class SendMessageCmd : IServerCommand
    {
        private MessageType messageType { get; set; }=MessageType.Text;
        public PromptType promptType { get; set; }
        public string MessageContent { get; set; }

        public SessionforBiz ToSession { get; set; }

        public SendMessageCmd(SessionforBiz ToSession)
        {

            this.ToSession = ToSession;

        }


        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // 添加产品逻辑
            await Task.Run(
                () =>
              {

                  SendMessage(null,ToSession);
              }

                ,

                cancellationToken
                );
        }




        public void SendMessage(SessionforBiz FromSession = null, SessionforBiz ToSession = null)
        {
            try
            {
                MessageBase messageBase = null;
                //发送消息数据 
                //将消息转换为一个实体先
                switch (messageType)
                {
                    case MessageType.Text:
                        messageBase = new MessageBase();
                        break;
                    case MessageType.BusinessData:
                        break;
                    case MessageType.Event:
                        break;
                    default:
                        break;
                }
                messageBase.Content = MessageContent;
                string json = JsonConvert.SerializeObject(messageBase,
                   new JsonSerializerSettings
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                   });

                ByteBuff tx = new ByteBuff(100);
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushString(json);
                tx.PushInt((int)promptType);
                //foreach (var item in frmMain.Instance.sessionListBiz)
                //{
                //    tx.PushString(item.Value.SessionID);
                //    tx.PushString(item.Value.User.用户名);
                //    tx.PushString(item.Value.User.姓名);
                //}
                ToSession.AddSendData((byte)ServerCmdEnum.复合型消息推送, new byte[] { (byte)messageType }, tx.toByte());
            }
            catch (Exception ex)
            {

            }


        }

        public bool AnalyzeDataPacket(OriginalData gd, SessionforBiz FromSession)
        {
            throw new NotImplementedException();
        }
    }
}
