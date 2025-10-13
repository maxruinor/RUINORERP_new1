using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;
using RUINORERP.Server.Network.Interfaces.Services;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace RUINORERP.Server.Workflow.WFPush
{
    /// <summary>
    /// 功能将传过来的数据，发出去
    /// </summary>
    public class PushDataStep : StepBody
    {
        private  ISessionService _sessionService;
        
        public string TagetTableName { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _sessionService = Startup.GetFromFac<ISessionService>();

            var sessions = _sessionService.GetAllUserSessions();
            foreach (var session in sessions)
            {
                try
                {
                    // 构建推送消息
                    var messageData = new
                    {
                        Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        TableName = TagetTableName,
                        Message = "给客户端发提示消息测试！分发测试" + TagetTableName.ToString()
                    };
                    
                    var messageJson = System.Text.Json.JsonSerializer.Serialize(messageData);
                    
                    // 发送工作流数据推送命令
                    var success = _sessionService.SendCommandToSession(
                        session.SessionID, 
                        "WORKFLOW_DATA_PUSH", 
                        messageJson
                    );
                    
                    if (success)
                    {
                        frmMain.Instance.PrintInfoLog($"工作流数据推送到 {session.UserName} 成功");
                    }
                    else
                    {
                        frmMain.Instance.PrintInfoLog($"工作流数据推送到 {session.UserName} 失败");
                    }
                }
                catch (Exception ex)
                {
                    frmMain.Instance.PrintInfoLog("服务器收到客户端基础信息变更分布失败:" + session.UserName + ex.Message);
                }
            }
            return ExecutionResult.Next();
        }
    }
}
