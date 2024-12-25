using FastReport.Table;
using Force.DeepCloner;
using Newtonsoft.Json.Linq;
using RUINORERP.Extensions.Middlewares;
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
using TransInstruction.DataPortal;
using MessageType = RUINORERP.Model.TransModel.MessageType;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 消息处理指令
    /// 消息有多种型式
    /// </summary>
    public class ReceiveMessageCommand : IClientCommand
    {
        public ReceiveMessageCommand()
        {

        }
        public MessageType messageType { get; set; }
        public string message { get; set; }
        public OriginalData od { get; set; }
        public ReceiveMessageCommand(OriginalData od)
        {
            this.od = od;
        }

        public ReceiveMessageCommand(MessageType messageType, string message)
        {
            this.messageType = messageType;
            this.message = message;
        }




        public bool AnalyzeDataPacket(OriginalData gd)
        {
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                string sendTime = ByteDataAnalysis.GetString(gd.Two, ref index);
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

                if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                {
                    MainForm.Instance.PrintInfoLog($"接收复合型消息：{obj.ToString()}成功！");
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
            if (parameters is OriginalData originalData)
            {
                await Task.Run(() =>
                {
                    AnalyzeDataPacket(originalData);
                }, cancellationToken);
            }
            else
            {
                throw new ArgumentException("接收到的数据不能为空。或格式不正确。");
            }
        }

        public OriginalData BuildDataPacket(object request)
        {
            throw new NotImplementedException();
        }


    }
}
