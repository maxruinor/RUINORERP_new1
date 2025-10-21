using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Plugin
{
    /// <summary>
    /// 示例插件：演示如何与主程序进行数据交互
    /// </summary>
    public class SampleDataInteractionPlugin : PluginBase
    {
        public override string Name => "数据交互示例插件";

        public override string Description => "演示插件如何与主程序进行数据交互";

        public override Version Version => new Version(1, 0, 0);

        protected override void OnStart()
        {
            // 插件启动时，可以向主程序发送初始化数据
            var initData = new Dictionary<string, object>
            {
                { "pluginName", this.Name },
                { "startTime", DateTime.Now },
                { "message", "插件已启动" }
            };

            // 发送数据到主程序
            CommunicationChannel?.SendDataToHost(this.Name, initData);
        }

        protected override void OnStop()
        {
            // 插件停止时，可以向主程序发送结束数据
            var endData = new Dictionary<string, object>
            {
                { "pluginName", this.Name },
                { "endTime", DateTime.Now },
                { "message", "插件已停止" }
            };

            // 发送数据到主程序
            CommunicationChannel?.SendDataToHost(this.Name, endData);
        }

        public override void Execute()
        {
            // 演示不同的数据交互方式

            // 1. 发送数据到主程序
            SendMessageToHost();

            // 2. 从主程序请求数据
            RequestDataFromHost();

            // 3. 调用主程序服务
            InvokeHostService();
        }

        private void SendMessageToHost()
        {
            var messageData = new Dictionary<string, object>
            {
                { "type", "message" },
                { "content", "这是来自插件的消息" },
                { "timestamp", DateTime.Now }
            };

            bool success = CommunicationChannel?.SendDataToHost(this.Name, messageData) ?? false;
            if (success)
            {
                ShowInfo("已成功向主程序发送消息");
            }
            else
            {
                ShowError("发送消息到主程序失败");
            }
        }

        private void RequestDataFromHost()
        {
            var requestData = new Dictionary<string, object>
            {
                { "type", "request" },
                { "resource", "user_info" },
                { "parameters", new { userId = 123 } }
            };

            var response = CommunicationChannel?.RequestDataFromHost(this.Name, requestData);
            if (response != null)
            {
                ShowInfo($"从主程序获取到数据: {response}");
            }
            else
            {
                ShowWarning("未能从主程序获取到数据");
            }
        }

        private void InvokeHostService()
        {
            var serviceParams = new Dictionary<string, object>
            {
                { "operation", "log" },
                { "message", "插件正在调用主程序服务" },
                { "level", "info" }
            };

            var result = CommunicationChannel?.InvokeHostService(this.Name, "LoggingService", serviceParams);
            if (result != null)
            {
                ShowInfo($"服务调用成功，返回结果: {result}");
            }
            else
            {
                ShowWarning("服务调用未返回结果");
            }
        }
    }
}