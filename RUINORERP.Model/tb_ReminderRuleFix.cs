
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
using RUINORERP.Model.Utilities;

namespace RUINORERP.Model
{

    public partial class tb_ReminderRule : BaseEntity, ICloneable, IReminderRule
    {
        #region 通知渠道
        private string _NotifyChannelDisplayText = string.Empty;
        /// <summary>
        /// 通知渠道
        /// </summary>

        [SugarColumn(IsIgnore = true, ColumnDescription = "通知渠道")]
        [Browsable(true)]
        [DisplayTextAttribute(true)]
        public string NotifyChannelDisplayText
        {
            get
            {
                //if (NotifyChannels.Count > 0)
                //{
                //    //转换为枚举的值以,号隔开
                //    _NotifyChannelDisplayText = string.Join(",", NotifyChannels.Select(x => x.ToString()));
                //}
                //return _NotifyChannelDisplayText;

                if (NotifyChannels == null || NotifyChannels.Count == 0)
                {
                    return string.Empty;
                }

                var displayTexts = NotifyChannels
                    .Select(value =>
                    {
                        // 获取枚举名称
                        if (Enum.IsDefined(typeof(NotifyChannel), value))
                        {
                            var field = typeof(NotifyChannel).GetField(((NotifyChannel)value).ToString());
                            var descAttr = field?.GetCustomAttribute<DescriptionAttribute>(false);
                            return descAttr?.Description ?? value.ToString();
                        }
                        return value.ToString(); // 如果值不在枚举中，直接返回数字
                    })
                    .ToList();

                return string.Join("，", displayTexts);

            }
        }


        private List<int> _NotifyChannels = new List<int>();

        /// <summary>
        /// 存储多选枚举值（数据库中为逗号分隔字符串）
        /// </summary>
        [AdvQueryAttribute(ColName = "NotifyChannels", ColDesc = "通知渠道")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = typeof(ListIntToStringConverter), ColumnName = "NotifyChannels", Length = 50, IsNullable = false, ColumnDescription = "通知渠道")]
        [Browsable(false)]
        [DisplayTextAttribute(false)]
        public List<int> NotifyChannels
        {
            get { return _NotifyChannels; }
            set
            {
                SetProperty(ref _NotifyChannels, value);
            }
        }


        #endregion


        #region 接收通知人员


        /// <summary>
        /// 通知渠道
        /// </summary>

        [SugarColumn(IsIgnore = true, ColumnDescription = "通知接收人员")]
        [Browsable(true)]
        [DisplayTextAttribute(true)]
        public string NotifyRecipientDisplayText { get; set; }


        private List<long> _NotifyRecipients = new List<long>();


        /// <summary>
        /// 通知接收人员ID
        /// </summary>
        [AdvQueryAttribute(ColName = "NotifyRecipients", ColDesc = "通知接收人员")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = typeof(ListLongToStringConverter), ColumnName = "NotifyRecipients", Length = 100000, IsNullable = false, ColumnDescription = "通知接收人员ID")]
        [Browsable(false)]
        [DisplayTextAttribute(false)]
        public List<long> NotifyRecipients
        {
            get { return _NotifyRecipients; }
            set
            {
                SetProperty(ref _NotifyRecipients, value);
            }
        }

        #endregion



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

