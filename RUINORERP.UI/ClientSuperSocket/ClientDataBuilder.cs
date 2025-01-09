using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction.DataPortal;
using TransInstruction;
using RUINORERP.Model;
using RUINORERP.Global;
using Newtonsoft.Json;
using Netron.GraphLib;
using NPOI.SS.Formula.Functions;
using RUINORERP.Model.TransModel;

namespace RUINORERP.UI.SuperSocketClient
{

    /// <summary>
    /// 客户端数据组合后发送到服务器的服务
    /// </summary>
    public class ClientDataBuilder
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static OriginalData BaseInfoChangeBuilder(string TableName)
        {
            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuff(100);
                tx.PushString(System.DateTime.Now.ToString());
                WorkflowBizType workflowBiz = WorkflowBizType.基础数据信息推送;
                tx.PushInt((int)workflowBiz);
                tx.PushString(TableName);
                gd.cmd = (byte)ClientCmdEnum.工作流启动;
                gd.One = null;
                gd.Two = tx.toByte();
            }
            catch (Exception)
            {

            }
            return gd;
        }
 
        /// <summary>
        /// 将要提醒的跟进计划数据上传到服务器
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ReceiverID"></param>
        /// <param name="StartTime"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static OriginalData 工作流提醒请求(ReminderData request)
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
                //将来再加上提醒配置规则,或加在请求实体中
                gd.cmd = (byte)ClientCmdEnum.工作流提醒请求;
               // gd.One = new byte[] { (byte)BizType.CRM跟进计划 };
                gd.Two = tx.toByte();
            }
            catch (Exception)
            {

            }
            return gd;
        }


        public static OriginalData 工作流提醒回复(ClientResponseData response)
        {
            //要定一个回复规则
            OriginalData gd = new OriginalData();
            try
            {
                var tx = new ByteBuff(100);
                //发送缓存数据
                string json = JsonConvert.SerializeObject(response,
                   new JsonSerializerSettings
                   {
                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
                   });
                tx.PushString(System.DateTime.Now.ToString());
                tx.PushString(json);
                //将来再加上提醒配置规则

                gd.cmd = (byte)ClientCmdEnum.工作流提醒回复;
                gd.One = new byte[] { (byte)BizType.CRM跟进计划 };
                gd.Two = tx.toByte();
            }
            catch (Exception)
            {

            }
            return gd;
        }
    }
}
