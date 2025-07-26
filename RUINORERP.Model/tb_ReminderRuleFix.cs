
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:26
// **************************************
using System;
using SqlSugar;
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
using RUINORERP.Model.Base;
using System.Linq;
using RUINORERP.Model.ReminderModel.ReminderRules;

namespace RUINORERP.Model
{

    public partial class tb_ReminderRule : BaseEntity, ICloneable, IReminderRule
    {

        // 用于存储特定业务场景的配置
        //[JsonConverter(typeof(ReminderBizTypeJsonConverter))]
        //[SugarColumn(IsIgnore = true)]
        //public virtual object BusinessConfig { get; set; }

        // 序列化为 JSON 字符串保存到数据库
        //  public string JsonConfig => JsonConvert.SerializeObject(BusinessConfig);

        // 辅助属性，用于处理JSON配置
        //[SugarColumn(IsIgnore = true)]
        //public ReminderConfig Config { get; set; }

        //// 辅助属性，不参与JSON序列化
        //[JsonIgnore]
        //[SugarColumn(IsIgnore = true)]
        //public string NotifyRecipientNames
        //{
        //    get; set;
        //}


        /// <summary>
        /// 离线消息处理属性
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool PersistUntilDelivered { get; set; } = true;



        [SugarColumn(IsIgnore = true)]
        public List<NotifyChannel> Channels { get; set; }

        [SugarColumn(IsIgnore = true)]
        public JObject Metadata { get; set; } // 扩展字段
      
        //int IReminderRule.RuleEngineType { get; set; }



        public IRuleConfig GetConfig<T>()
        {
            // 使用示例
            JObject obj = SafeParseJson(JsonConfig);
            SafetyStockConfig safetyStockConfig = obj.ToObject<SafetyStockConfig>();
            if (safetyStockConfig == null)
            {
                safetyStockConfig = new SafetyStockConfig();
            }

            return safetyStockConfig as IRuleConfig;
        }


        

        public JObject SafeParseJson(string json)
        {
            try
            {
                return JObject.Parse(json);
            }
            catch (JsonReaderException)
            {
                return new JObject(); // 返回空对象或 null
            }
        }


    }

}

