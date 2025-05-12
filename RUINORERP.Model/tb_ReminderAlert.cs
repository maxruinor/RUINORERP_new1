
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:24
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
    /// 提醒内容
    /// </summary>
    [Serializable()]
    [Description("提醒内容")]
    [SugarTable("tb_ReminderAlert")]
    public partial class tb_ReminderAlert: BaseEntity, ICloneable
    {
        public tb_ReminderAlert()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("提醒内容tb_ReminderAlert" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _AlertId;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "AlertId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long AlertId
        { 
            get{return _AlertId;}
            set{
            SetProperty(ref _AlertId, value);
                base.PrimaryKeyID = _AlertId;
            }
        }

        private long? _RuleId;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "RuleId",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RuleId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ReminderRule","RuleId")]
        public long? RuleId
        { 
            get{return _RuleId;}
            set{
            SetProperty(ref _RuleId, value);
                        }
        }

        private DateTime? _AlertTime;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "AlertTime",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "AlertTime" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? AlertTime
        { 
            get{return _AlertTime;}
            set{
            SetProperty(ref _AlertTime, value);
                        }
        }

        private string _Message;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Message",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Message" ,Length=200,IsNullable = true,ColumnDescription = "" )]
        public string Message
        { 
            get{return _Message;}
            set{
            SetProperty(ref _Message, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(RuleId))]
        public virtual tb_ReminderRule tb_reminderrule { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ReminderAlertHistory.AlertId))]
        public virtual List<tb_ReminderAlertHistory> tb_ReminderAlertHistories { get; set; }
        //tb_ReminderAlertHistory.AlertId)
        //AlertId.FK_REMINDERALERTHISTORY_REF_REMINDERALERT)
        //tb_ReminderAlert.AlertId)


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
                    Type type = typeof(tb_ReminderAlert);
                    
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
            tb_ReminderAlert loctype = (tb_ReminderAlert)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

