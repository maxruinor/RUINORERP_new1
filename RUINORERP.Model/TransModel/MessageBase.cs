using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.TransModel
{
    [Serializable]
    public class MessageBase
    {
        public long Id { get; set; } // 消息唯一标识
        public string Type { get; set; } // 消息类型，用于区分消息内容或业务数据
        public string Content { get; set; } // 消息内容，可以是文本、JSON、XML等
        public DateTime Timestamp { get; set; } // 时间戳，记录消息发送的时间
    }

    public enum MessageType
    {
        Text, // 文本消息
        BusinessData, // 业务数据
        Event // 事件通知
    }


    public class TextMessage : MessageBase
    {
        public string Text { get; set; }
    }

    public class BusinessDataMessage : MessageBase
    {
        public object Data { get; set; } // 业务数据，可以是复杂对象
    }

    public class EventMessage : MessageBase
    {
        public string EventName { get; set; }
        public object EventArgs { get; set; } // 事件参数
    }

    public static class MessageSerializer
    {
        public static string Serialize(object message)
        {
            return JsonConvert.SerializeObject(message);
        }

        public static object Deserialize(string message, Type type)
        {
            return JsonConvert.DeserializeObject(message, type);
        }
    }


    public class MessageProcessingService
    {
        public void ProcessMessage(MessageBase message)
        {
            switch (message)
            {
                case TextMessage textMessage:
                    HandleTextMessage(textMessage);
                    break;
                case BusinessDataMessage businessDataMessage:
                    HandleBusinessDataMessage(businessDataMessage);
                    break;
                case EventMessage eventMessage:
                    HandleEventMessage(eventMessage);
                    break;
                default:
                    HandleUnknownMessageType(message);
                    break;
            }
        }

        private void HandleTextMessage(TextMessage message)
        {
            // 处理文本消息逻辑
            Console.WriteLine($"Received text message: {message.Text}");
        }

        private void HandleBusinessDataMessage(BusinessDataMessage message)
        {
            // 处理业务数据逻辑
            Console.WriteLine($"Received business data: {message.Data}");
        }

        private void HandleEventMessage(EventMessage message)
        {
            // 触发事件逻辑
            Console.WriteLine($"Received event: {message.EventName} with arguments: {message.EventArgs}");
        }

        private void HandleUnknownMessageType(MessageBase message)
        {
            // 未知消息类型处理逻辑
            Console.WriteLine($"Received unknown message type: {message.Type}");
        }
    }


    //使用

    //var messageService = new MessageProcessingService();

    //// 模拟接收消息
    //var textMessage = new TextMessage { Text = "Hello, World!" };
    //messageService.ProcessMessage(textMessage);

    //    var businessDataMessage = new BusinessDataMessage { Data = new { Value = 42 } };
    //messageService.ProcessMessage(businessDataMessage);

    //    var eventMessage = new EventMessage { EventName = "UserLoggedIn", EventArgs = new { UserId = 123 } };
    //messageService.ProcessMessage(eventMessage);





}
