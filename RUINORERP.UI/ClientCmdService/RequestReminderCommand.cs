using Newtonsoft.Json;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.CommandService;
using TransInstruction.DataPortal;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 工作流提醒相关的请求
    /// </summary>
    public class RequestReminderCommand : IClientCommand
    {
        public RequestReminderType requestType { get; set; }
        public ReminderData requestInfo { get; set; }

        public bool AnalyzeDataPacket(OriginalData gd)
        {
            throw new NotImplementedException();
        }

        public OriginalData BuildDataPacket(object request = null)
        {
            throw new NotImplementedException();
        }

        //public void Execute(object parameters = null)
        //{
        //    ExecuteAsync(CancellationToken.None, parameters).GetAwaiter().GetResult();
        //}

        public async Task ExecuteAsync(CancellationToken cancellationToken, object parameters)
        {
            if (parameters != null)
            {
                await Task.Run(
                   () =>
                  (
                   #region 执行方法

                   #endregion
                      //MainForm.Instance.ecs.AddSendData(beatDataDel)
                      1 == 1
                  )


                   ,
                cancellationToken
                   );
            }
            else
            {
                MainForm.Instance.ecs.AddSendData(工作流请求(requestInfo));
            }

        }


        public OriginalData 工作流请求(ReminderData request)
        {
            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuff(100);
                string json = JsonConvert.SerializeObject(request,
           new JsonSerializerSettings
           {
               ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
           });
                tx.PushString(System.DateTime.Now.ToString());
                tx.PushString(json);
                tx.PushInt((int)requestType);
                //将来再加上提醒配置规则,或加在请求实体中
                gd.cmd = (byte)ClientCmdEnum.复合型工作流请求;
                gd.One = new byte[] { (byte)requestType };
                gd.Two = tx.toByte();
            }
            catch (Exception)
            {

            }
            return gd;
        }


    }



}
