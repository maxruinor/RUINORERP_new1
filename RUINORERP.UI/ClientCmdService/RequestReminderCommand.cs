/*****************************************************************************************
 * 【过时作废文件】OBSOLETE - DEPRECATED - DO NOT USE
 * 此文件已被废弃，不再维护和使用
 * 原因：ClientCmdService目录下的所有文件都已过时，实际已排除在项目外
 * 替代方案：请使用新的命令处理机制
 * 创建日期：系统自动标识
 *****************************************************************************************/

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
using TransInstruction.Enums;

namespace RUINORERP.UI.ClientCmdService
{
    /// <summary>
    /// 工作流提醒相关的请求
    /// </summary>
    public class RequestReminderCommand : IClientCommand
    {
        public CmdOperation OperationType { get; set; }
        public OriginalData DataPacket { get; set; }
        public RequestReminderType requestType { get; set; }
        public ReminderData requestInfo { get; set; }

        public bool AnalyzeDataPacket(OriginalData gd)
        {
            throw new NotImplementedException();
        }

        public void BuildDataPacket(object request = null)
        {
            throw new NotImplementedException();
        }
 

        public async Task ExecuteAsync(CancellationToken cancellationToken, object parameters)
        {
            await Task.Run(
               () =>
                    {
                        #region 执行方法
                        switch (OperationType)
                        {
                            case CmdOperation.Send:
                                BuildDataPacket(null);
                                break;
                            case CmdOperation.Receive:
                                AnalyzeDataPacket(DataPacket);
                                break;
                            default:
                                break;
                        }
                        #endregion
                    }
                   ,
                cancellationToken
                   );
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
                string sendtime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendtime);
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
