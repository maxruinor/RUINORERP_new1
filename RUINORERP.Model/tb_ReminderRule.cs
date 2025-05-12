
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

namespace RUINORERP.Model
{
    /// <summary>
    /// 库存策略通过这里设置的条件查询出一个库存集合提醒给用户
    /// </summary>
    [Serializable()]
    [Description("库存策略通过这里设置的条件查询出一个库存集合提醒给用户")]
    [SugarTable("tb_ReminderRule")]
    public partial class tb_ReminderRule: BaseEntity, ICloneable
    {
        public tb_ReminderRule()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("库存策略通过这里设置的条件查询出一个库存集合提醒给用户tb_ReminderRule" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _RuleId;
        /// <summary>
        /// 提醒规则
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RuleId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "提醒规则" , IsPrimaryKey = true)]
        public long RuleId
        { 
            get{return _RuleId;}
            set{
            SetProperty(ref _RuleId, value);
                base.PrimaryKeyID = _RuleId;
            }
        }

        private string _RuleName;
        /// <summary>
        /// 规则名称
        /// </summary>
        [AdvQueryAttribute(ColName = "RuleName",ColDesc = "规则名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RuleName" ,Length=100,IsNullable = false,ColumnDescription = "规则名称" )]
        public string RuleName
        { 
            get{return _RuleName;}
            set{
            SetProperty(ref _RuleName, value);
                        }
        }

        private string _Description;
        /// <summary>
        /// 规则 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "规则 描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Description" ,Length=500,IsNullable = false,ColumnDescription = "规则 描述" )]
        public string Description
        { 
            get{return _Description;}
            set{
            SetProperty(ref _Description, value);
                        }
        }

        private int _RuleEngineType;
        /// <summary>
        /// 引擎类型
        /// </summary>
        [AdvQueryAttribute(ColName = "RuleEngineType", ColDesc = "引擎类型")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "RuleEngineType", DecimalDigits = 0, IsNullable = false, ColumnDescription = "引擎类型")]
        public int RuleEngineType
        {
            get { return _RuleEngineType; }
            set
            {
                SetProperty(ref _RuleEngineType, value);
            }
        }


        private int _ReminderBizType;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ReminderBizType",ColDesc = "业务类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ReminderBizType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "业务类型" )]
        public int ReminderBizType
        { 
            get{return _ReminderBizType;}
            set{
            SetProperty(ref _ReminderBizType, value);
                        }
        }

        private int _Priority;
        /// <summary>
        /// 优先级
        /// </summary>
        [AdvQueryAttribute(ColName = "Priority",ColDesc = "优先级")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Priority" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "优先级" )]
        public int Priority
        { 
            get{return _Priority;}
            set{
            SetProperty(ref _Priority, value);
                        }
        }

        private bool _IsEnabled= false;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "IsEnabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsEnabled" ,IsNullable = false,ColumnDescription = "是否启用" )]
        public bool IsEnabled
        { 
            get{return _IsEnabled;}
            set{
            SetProperty(ref _IsEnabled, value);
                        }
        }

        private string _NotifyChannels;
        /// <summary>
        /// 通知渠道
        /// </summary>
        [AdvQueryAttribute(ColName = "NotifyChannels",ColDesc = "通知渠道")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "NotifyChannels" ,Length=50,IsNullable = false,ColumnDescription = "通知渠道" )]
        public string NotifyChannels
        { 
            get{return _NotifyChannels;}
            set{
            SetProperty(ref _NotifyChannels, value);
                        }
        }

        private DateTime _EffectiveDate;
        /// <summary>
        /// 生效日期
        /// </summary>
        [AdvQueryAttribute(ColName = "EffectiveDate",ColDesc = "生效日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "EffectiveDate" ,IsNullable = false,ColumnDescription = "生效日期" )]
        public DateTime EffectiveDate
        { 
            get{return _EffectiveDate;}
            set{
            SetProperty(ref _EffectiveDate, value);
                        }
        }

        private DateTime _ExpireDate;
        /// <summary>
        /// 过期时间
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpireDate",ColDesc = "过期时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ExpireDate" ,IsNullable = false,ColumnDescription = "过期时间" )]
        public DateTime ExpireDate
        { 
            get{return _ExpireDate;}
            set{
            SetProperty(ref _ExpireDate, value);
                        }
        }

        private string _Condition;
        /// <summary>
        /// 规则条件
        /// </summary>
        [AdvQueryAttribute(ColName = "Condition",ColDesc = "规则条件")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "Condition" ,Length=2147483647,IsNullable = false,ColumnDescription = "规则条件" )]
        public string Condition
        { 
            get{return _Condition;}
            set{
            SetProperty(ref _Condition, value);
                        }
        }

        private string _NotifyRecipients;
        /// <summary>
        /// 知接收人
        /// </summary>
        [AdvQueryAttribute(ColName = "NotifyRecipients",ColDesc = "知接收人")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "NotifyRecipients" ,Length=2147483647,IsNullable = false,ColumnDescription = "知接收人" )]
        public string NotifyRecipients
        { 
            get{return _NotifyRecipients;}
            set{
            SetProperty(ref _NotifyRecipients, value);
                        }
        }

        private string _NotifyMessage;
        /// <summary>
        /// 通知消息模板
        /// </summary>
        [AdvQueryAttribute(ColName = "NotifyMessage",ColDesc = "通知消息模板")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "NotifyMessage" ,Length=2147483647,IsNullable = false,ColumnDescription = "通知消息模板" )]
        public string NotifyMessage
        { 
            get{return _NotifyMessage;}
            set{
            SetProperty(ref _NotifyMessage, value);
                        }
        }

        private string _JsonConfig;
        /// <summary>
        /// 扩展JSON配置
        /// </summary>
        [AdvQueryAttribute(ColName = "JsonConfig",ColDesc = "扩展JSON配置")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "JsonConfig" ,Length=2147483647,IsNullable = false,ColumnDescription = "扩展JSON配置" )]
        public string JsonConfig
        { 
            get{return _JsonConfig;}
            set{
            SetProperty(ref _JsonConfig, value);
                        }
        }

        private DateTime _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = false,ColumnDescription = "创建时间" )]
        public DateTime Created_at
        { 
            get{return _Created_at;}
            set{
            SetProperty(ref _Created_at, value);
                        }
        }

        private long _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "创建人" )]
        public long Created_by
        { 
            get{return _Created_by;}
            set{
            SetProperty(ref _Created_by, value);
                        }
        }

        private DateTime _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = false,ColumnDescription = "修改时间" )]
        public DateTime Modified_at
        { 
            get{return _Modified_at;}
            set{
            SetProperty(ref _Modified_at, value);
                        }
        }

        private long _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "修改人" )]
        public long Modified_by
        { 
            get{return _Modified_by;}
            set{
            SetProperty(ref _Modified_by, value);
                        }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ReminderAlert.RuleId))]
        public virtual List<tb_ReminderAlert> tb_ReminderAlerts { get; set; }
        //tb_ReminderAlert.RuleId)
        //RuleId.FK_REMINDERALERT_REF_REMINDERRULE)
        //tb_ReminderRule.RuleId)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






        #region 字段描述对应列表
        private ConcurrentDictionary<string, string> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_ReminderRule);
                    
                       foreach (PropertyInfo field in type.GetProperties())
                            {
                                foreach (Attribute attr in field.GetCustomAttributes(true))
                                {
                                    entityAttr = attr as SugarColumn;
                                    if (null != entityAttr)
                                    {
                                        if (entityAttr.ColumnDescription == null)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsIdentity)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsPrimaryKey)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.ColumnDescription.Trim().Length > 0)
                                        {
                                            fieldNameList.TryAdd(field.Name, entityAttr.ColumnDescription);
                                        }
                                    }
                                }
                            }
                }
                
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }
        #endregion
        

        public override object Clone()
        {
            tb_ReminderRule loctype = (tb_ReminderRule)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

