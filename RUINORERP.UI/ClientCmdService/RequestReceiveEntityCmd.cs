using Netron.GraphLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Extensions.ServiceExtensions;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransInstruction;
using TransInstruction.CommandService;
using TransInstruction.DataPortal;
using TransInstruction.Enums;
using static RUINORERP.Extensions.ServiceExtensions.EditConfigCommand;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 请求接收实体的指令
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestReceiveEntityCmd : IClientCommand
    {
        public CmdOperation OperationType { get; set; }
        public EntityTransferCmdType requestType { get; set; }

        /// <summary>
        /// 发送到服务器后。才可能指定下一步处理
        /// 客户端接收不用
        /// </summary>
        public NextProcesszStep nextProcesszStep { get; set; } = NextProcesszStep.无;

        public OriginalData DataPacket { get; set; }

        public RequestReceiveEntityCmd(CmdOperation _OperationType)
        {
            OperationType = _OperationType;
        }

        /// <summary>
        /// 处理的对象
        /// </summary>
        public object SendObject { get; set; }


        public bool AnalyzeDataPacket(OriginalData gd)
        {
            try
            {
                int index = 0;
                ByteBuff bg = new ByteBuff(gd.Two);
                string sendTime = ByteDataAnalysis.GetString(gd.Two, ref index);
                string TransferObjectName = ByteDataAnalysis.GetString(gd.Two, ref index);
                string json = ByteDataAnalysis.GetString(gd.Two, ref index);
                EntityTransferCmdType subRequestType = (EntityTransferCmdType)ByteDataAnalysis.GetInt(gd.Two, ref index);
                NextProcesszStep subNextProcesszStep = (NextProcesszStep)ByteDataAnalysis.GetInt(gd.Two, ref index);
                // 将item转换为JObject
                 var obj = JObject.Parse(json);
                switch (subRequestType)
                {
                    case EntityTransferCmdType.处理动态配置:
                        string basePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), GlobalConstants.DynamicConfigFileDirectory);
                        if (!Directory.Exists(basePath))
                        {
                            Directory.CreateDirectory(basePath);
                        }

                        ConfigFileReceiver _configFileReceiver = new ConfigFileReceiver(basePath + "/" + TransferObjectName + ".json");

                        // 创建一个新的JObject，并将SystemGlobalConfig作为根节点
                        JObject LastConfigJson = new JObject(new JProperty(TransferObjectName, JObject.FromObject(obj)));

                        //保存到本地
                        EditConfigCommand command = new EditConfigCommand(_configFileReceiver, LastConfigJson);
                        CommandManager _commandManager = new CommandManager();
                        _commandManager.ExecuteCommand(command);
                        break;
                    default:
                        break;
                }
                if (MainForm.Instance.authorizeController.GetDebugInfoAuth())
                {
                    MainForm.Instance.PrintInfoLog($"接收复合型实体处理：{obj.ToString()}成功！");
                }

            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog("接收服务器提示消息:" + ex.Message);
            }
            return true;

        }

        /// <summary>
        /// 构建请求的数据包
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void BuildDataPacket(object request = null)
        {

            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuff(2 + 4);
                //发送缓存数据
                string json = JsonConvert.SerializeObject(request,
                   new JsonSerializerSettings
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                   });
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
                tx.PushString(request.GetType().Name);
                tx.PushString(json);
                tx.PushInt((int)requestType);
                tx.PushInt((int)nextProcesszStep);
                gd.cmd = (byte)ClientCmdEnum.复合型实体请求;
                gd.One = null;
                gd.Two = tx.toByte();
                MainForm.Instance.ecs.AddSendData(gd);
            }
            catch (Exception)
            {

            }
          

        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, object parameters = null)
        {
            await Task.Run(
               () =>
               {
                   switch (OperationType)
                   {
                       case CmdOperation.Send:
                           #region 执行方法
                          
                               BuildDataPacket(SendObject);
                          

                           #endregion
                           break;
                       case CmdOperation.Receive:
                           #region 执行方法
                          
                               AnalyzeDataPacket(DataPacket);
                         

                           #endregion
                           break;
                       default:
                           break;
                   }

               }
               ,

               cancellationToken
               );

        }






    }

}
