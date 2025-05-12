
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:25
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
    /// 提醒信息是通过什么规则通知了什么内容给谁在什么时间。通知记录  暂时不处理
    /// </summary>
    [Serializable()]
    [Description("提醒信息是通过什么规则通知了什么内容给谁在什么时间。通知记录  暂时不处理")]
    [SugarTable("tb_ReminderAlertHistory")]
    public partial class tb_ReminderAlertHistory: BaseEntity, ICloneable
    {
        public tb_ReminderAlertHistory()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("提醒信息是通过什么规则通知了什么内容给谁在什么时间。通知记录  暂时不处理tb_ReminderAlertHistory" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _HistoryId;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "HistoryId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long HistoryId
        { 
            get{return _HistoryId;}
            set{
            SetProperty(ref _HistoryId, value);
                base.PrimaryKeyID = _HistoryId;
            }
        }

        private long _AlertId;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "AlertId",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "AlertId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ReminderAlert","AlertId")]
        public long AlertId
        { 
            get{return _AlertId;}
            set{
            SetProperty(ref _AlertId, value);
                        }
        }

        private long _User_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "User_ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "User_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_UserInfo","User_ID")]
        public long User_ID
        { 
            get{return _User_ID;}
            set{
            SetProperty(ref _User_ID, value);
                        }
        }

        private bool _IsRead;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "IsRead",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsRead" ,IsNullable = false,ColumnDescription = "" )]
        public bool IsRead
        { 
            get{return _IsRead;}
            set{
            SetProperty(ref _IsRead, value);
                        }
        }

        private string _Message;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Message",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "Message" ,Length=2147483647,IsNullable = false,ColumnDescription = "" )]
        public string Message
        { 
            get{return _Message;}
            set{
            SetProperty(ref _Message, value);
                        }
        }

        private DateTime _TriggerTime;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "TriggerTime",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "TriggerTime" ,IsNullable = false,ColumnDescription = "" )]
        public DateTime TriggerTime
        { 
            get{return _TriggerTime;}
            set{
            SetProperty(ref _TriggerTime, value);
                        }
        }

        private int _ReminderBizType;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ReminderBizType",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ReminderBizType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        public int ReminderBizType
        { 
            get{return _ReminderBizType;}
            set{
            SetProperty(ref _ReminderBizType, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(AlertId))]
        public virtual tb_ReminderAlert tb_reminderalert { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(User_ID))]
        public virtual tb_UserInfo tb_userinfo { get; set; }



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
                    Type type = typeof(tb_ReminderAlertHistory);
                    
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
            tb_ReminderAlertHistory loctype = (tb_ReminderAlertHistory)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

