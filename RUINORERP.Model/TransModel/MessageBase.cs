﻿using Newtonsoft.Json;
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

    public enum PromptType
    {
        
        提示窗口,


        通知窗口,


        确认窗口,

        //只是显示到日志行
        日志消息,

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


     




}