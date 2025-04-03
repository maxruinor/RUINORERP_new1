using FastReport.Table;
using Force.DeepCloner;
using Krypton.Navigator;
using LightTalkChatBox;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.IM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using TransInstruction;
using TransInstruction.CommandService;
using TransInstruction.DataPortal;
using MessageType = RUINORERP.Model.TransModel.MessageType;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 消息处理指令
    /// 消息有多种型式
    /// </summary>
    public class RequestReceiveMessageCmd : IClientCommand
    {
        public CmdOperation OperationType { get; set; }
        public OriginalData DataPacket { get; set; }

        /// <summary>
        /// 接收时不用指定。发送时要指定。打包在指令包中
        /// </summary>
        public MessageType MessageType { get; set; }

        public NextProcesszStep nextProcesszStep { get; set; } = NextProcesszStep.无;
        public string MessageContent { get; set; }

        /// <summary>
        /// 发送时接收消息的ID
        /// </summary>
        public string ReceiverSessionID { get; set; }

        public RequestReceiveMessageCmd(CmdOperation operation)
        {
            OperationType = operation;
        }

        public event MessageReceivedEventHandler MessageReceived;

        public void ReceiveMessage(string message, string senderName, string senderSessionID)
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs
            {
                Message = message,
                SenderName = senderName,
                SenderSessionID = senderSessionID
            });
        }






        public bool AnalyzeDataPacket(OriginalData gd)
        {
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                string sendTime = ByteDataAnalysis.GetString(gd.Two, ref index);
                //解析时的类型不是实例化时的类型
                MessageType messageType = (MessageType)ByteDataAnalysis.GetInt(gd.Two, ref index);
                switch (messageType)
                {
                    case MessageType.Text:
                        break;
                    case MessageType.BusinessData:
                        break;
                    case MessageType.Prompt:
                        string json = ByteDataAnalysis.GetString(gd.Two, ref index);
                        PromptType promptType = (PromptType)ByteDataAnalysis.GetInt(gd.Two, ref index);
                        // 将item转换为JObject
                        var obj = JObject.Parse(json);
                        switch (promptType)
                        {
                            case PromptType.提示窗口:
                                break;
                            case PromptType.通知窗口:
                                break;
                            case PromptType.确认窗口:
                                MessageBox.Show(obj["Content"].ToString(), "提示");
                                break;
                            case PromptType.日志消息:
                                MainForm.Instance.PrintInfoLog(obj["Content"].ToString());
                                break;
                            default:
                                break;
                        }
                        break;
                    case MessageType.Event:
                        break;
                    case MessageType.IM:
                        string message = ByteDataAnalysis.GetString(gd.Two, ref index);
                        NextProcesszStep nextProcesszStep = (NextProcesszStep)ByteDataAnalysis.GetInt(gd.Two, ref index);
                        string fromSessionID = ByteDataAnalysis.GetString(gd.Two, ref index);
                        MessageContent = message;
                        //这里指定的是IM的，则会影响IM窗体
                        //判断是不是打开。打开就显示内容。弹出提示？

                        var fromUserinfo = MainForm.Instance.UserInfos.FirstOrDefault(c => c.SessionId == fromSessionID);
                        if (fromUserinfo != null)
                        {
                            //MainForm.Instance.Invoke(new Action(() =>
                            //{
                            //    KryptonPage page = IM.UCMessager.Instance.kryptonNavigator1.Pages.FirstOrDefault(c => c.Name == fromUserinfo.姓名);
                            //    if (page == null)
                            //    {
                            //        page = IM.UCMessager.Instance.AddTopPage(fromUserinfo);
                            //    }
                            //    if (page.Controls[0] is UCChatBox ucchatBox)
                            //    {
                            //        ucchatBox.chatBox.addChatBubble(ChatBox.BubbleSide.LEFT, message, fromUserinfo.姓名, fromUserinfo.SessionId, @"IMResources\Profiles\face_default.jpg");
                            //    }

                            //}));
                            //不用执行
                            if (MessageReceived != null)
                            {
                                ReceiveMessage(MessageContent, fromSessionID, fromSessionID);
                            }
                        }

                        break;

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器提示消息:" + ex.Message);
            }
            return true;

        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, object parameters)
        {

            await Task.Run(() =>
            {
                #region 执行方法

                if (OperationType == CmdOperation.Send)
                {
                    switch (MessageType)
                    {
                        case MessageType.Text:
                            break;
                        case MessageType.BusinessData:
                            break;
                        case MessageType.Event:
                            break;
                        case MessageType.IM:
                            BuildDataPacket(null);
                          
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (MessageType)
                    {
                        case MessageType.Text:
                            break;
                        case MessageType.BusinessData:
                            break;
                        case MessageType.Event:
                            break;
                        case MessageType.IM:
                            AnalyzeDataPacket(DataPacket);
                            break;
                        default:
                            break;
                    }
                }


                #endregion

            }, cancellationToken);

        }

        public void BuildDataPacket(object request)
        {
            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuff(100);
                //     string json = JsonConvert.SerializeObject(request,
                //new JsonSerializerSettings
                //{
                //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                //});
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushInt((int)MessageType);
                tx.PushString(MessageContent);
                tx.PushInt((int)nextProcesszStep);
                tx.PushString(ReceiverSessionID);

                //将来再加上提醒配置规则,或加在请求实体中
                gd.cmd = (byte)ClientCmdEnum.复合型消息处理;
                gd.One = new byte[] { (byte)MessageType };
                gd.Two = tx.toByte();
                MainForm.Instance.ecs.AddSendData(gd);

            }
            catch (Exception)
            {

            }
        }


    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public string Message { get; set; }
        public string SenderName { get; internal set; }
        public string SenderSessionID { get; internal set; }
    }
    public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
}
