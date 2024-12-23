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


        public void Execute(object parameters = null)
        {
            ReceiveMessage(od);
        }

        //public async Task ExecuteAsync(CancellationToken cancellationToken)
        //{
        //    // 添加逻辑
        //    await Task.Run(
        //        () =>
        //        //只是一行做对。为了编译通过

        //        //ProductService.AddProduct(productName, price)
        //        {
        //            MessagePrompt messager = new MessagePrompt();
        //            messager.Content = message;
        //            messager.Show();
        //            messager.TopMost = true;
        //        }

        //        ,

        //        cancellationToken
        //        );
        //}
        public async Task ExecuteAsync(CancellationToken cancellationToken, object parameters = null)
        {

            if (parameters is OriginalData originalData)
            {
                await Task.Run(() =>
                {
                    ReceiveMessage(originalData);
                }, cancellationToken);
            }
            else
            {
                throw new ArgumentException("接收到的数据不能为空。或格式不正确。");
            }
        }


        public string ReceiveMessage(OriginalData gd)
        {
            string Message = "";
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                string sendTime = ByteDataAnalysis.GetString(gd.Two, ref index);
                switch (messageType)
                {
                    case MessageType.Text:
                        Message = ByteDataAnalysis.GetString(gd.Two, ref index);
                        break;
                    case MessageType.BusinessData:
                        break;
                    case MessageType.Event:
                        break;

                    default:
                        MessagePrompt messager = new MessagePrompt();
                        messager.Content = string.Empty;
                        messager.Show();
                        messager.TopMost = true;
                        break;
                }
                string json = ByteDataAnalysis.GetString(gd.Two, ref index);
                // 将item转换为JObject
                var obj = JObject.Parse(json);
                //if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                //{
                //    MainForm.Instance.PrintInfoLog($"接收转发更新缓存{tableName}成功！");
                //}
                MainForm.Instance.PrintInfoLog(Message);
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器提示消息:" + ex.Message);
            }
            return Message;

        }
    }
}
