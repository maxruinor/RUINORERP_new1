using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    /// <summary>
    /// 服务器接收客户端提醒指令
    /// </summary>
    public class ReceiveReminderCmd : IServerCommand
    {
        public RequestReminderType requestType { get; set; }
        private PromptType promptType { get; set; }
        public ReminderData requestInfo { get; set; }

        public SessionforBiz FromSession { get; set; }

        public OriginalData gd { get; set; }

        public ReceiveReminderCmd(OriginalData gd, SessionforBiz FromSession)
        {
            this.FromSession = FromSession;
            this.gd = gd;
            接收请求(gd, FromSession);
        }
        public bool 接收请求(OriginalData gd, SessionforBiz FromSession)
        {
            bool rs = false;
            try
            {
                int index = 0;
                //当前
                string 时间 = ByteDataAnalysis.GetString(gd.Two, ref index);
                string json = ByteDataAnalysis.GetString(gd.Two, ref index);
                requestType = (RequestReminderType)ByteDataAnalysis.GetInt(gd.Two, ref index);
                // 将item转换为JObject
                JObject obj = JObject.Parse(json);
                requestInfo = obj.ToObject<ReminderData>();
            }
            catch (Exception ex)
            {
                Comm.CommService.ShowExceptionMsg("接收请求:" + ex.Message);
            }
            return rs;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // 添加产品逻辑
            await Task.Run(
                () =>
                {

                    try
                    {
                        #region 根据不同情况处理提醒数据
                        //只是一行做对。为了编译通过
                        if (requestType == RequestReminderType.删除提醒)
                        {
                            foreach (var item in frmMain.Instance.ReminderBizDataList)
                            {
                                if (item.Key == requestInfo.BizPrimaryKey)
                                {
                                    item.Value.IsCancelled = true;
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {


                    }

                }
                ,
                cancellationToken
                );
        }

        //public void Execute()
        //{
        //    SendMessage();
        //}


        public void SendMessage(SessionforBiz FromSession = null, SessionforBiz ToSession = null)
        {
            try
            {
                MessageBase messageBase = null;
                //发送消息数据 
                //将消息转换为一个实体先
                switch (requestType)
                {
                    case RequestReminderType.添加提醒:
                        // messageBase = new AddReminderMessage();
                        break;
                    case RequestReminderType.删除提醒:
                        // messageBase = new DeleteReminderMessage();
                        break;
                    default:
                        break;
                }

                string json = JsonConvert.SerializeObject(messageBase,
                   new JsonSerializerSettings
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                   });

                ByteBuff tx = new ByteBuff(100);
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushString(json);

                foreach (var item in frmMain.Instance.sessionListBiz)
                {
                    tx.PushString(item.Value.SessionID);
                    tx.PushString(item.Value.User.用户名);
                    tx.PushString(item.Value.User.姓名);
                }
                ToSession.AddSendData((byte)ServerCmdEnum.复合型消息推送, new byte[] { (byte)requestType }, tx.toByte());
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
