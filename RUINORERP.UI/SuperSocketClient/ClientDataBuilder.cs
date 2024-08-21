using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction.DataPortal;
using TransInstruction;

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
    }
}
