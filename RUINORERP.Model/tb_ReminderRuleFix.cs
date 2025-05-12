
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:26
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using Newtonsoft.Json;
using RUINORERP.Global.EnumExt;
using Newtonsoft.Json.Linq;
using RUINORERP.Model.ReminderModel;

namespace RUINORERP.Model
{
  
    public partial class tb_ReminderRule: BaseEntity, ICloneable,IReminderRule
    {

        // 用于存储特定业务场景的配置
        [JsonConverter(typeof(ReminderBizTypeJsonConverter))]
        public virtual object BusinessConfig { get; set; }

        // 序列化为 JSON 字符串保存到数据库
        //  public string JsonConfig => JsonConvert.SerializeObject(BusinessConfig);

        // 辅助属性，用于处理JSON配置
        public ReminderConfig Config { get; set; }

        public List<NotificationChannel> Channels { get; set; }
        public JObject Metadata { get; set; } // 扩展字段
    }

    public class ReminderConfig
    {
        public List<int> ProductIds { get; set; }
        public int? MinStockThreshold { get; set; }
        public int? MaxStockThreshold { get; set; }
        public string Frequency { get; set; }
        public string Time { get; set; }
        public List<string> ResponsiblePersons { get; set; }
        // 可以根据需要添加更多配置属性
    }
}

