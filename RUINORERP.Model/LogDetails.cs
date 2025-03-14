
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:40:51
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
    /// 
    /// </summary>
    [Serializable()]
    [Description("")]
    [SugarTable("LogDetails")]
    public partial class LogDetails: BaseEntity, ICloneable
    {
        public LogDetails()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("LogDetails" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private int _LogId;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "LogId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true, IsIdentity = true)]
        public int LogId
        { 
            get{return _LogId;}
            set{
            SetProperty(ref _LogId, value);
                base.PrimaryKeyID = _LogId;
            }
        }

        private DateTime _LogDate = System.DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "LogDate",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "LogDate" ,IsNullable = false,ColumnDescription = "" )]
        public DateTime LogDate
        { 
            get{return _LogDate;}
            set{
            SetProperty(ref _LogDate, value);
                        }
        }

        private string _LogThread;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "LogThread",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "LogThread" ,Length=255,IsNullable = false,ColumnDescription = "" )]
        public string LogThread
        { 
            get{return _LogThread;}
            set{
            SetProperty(ref _LogThread, value);
                        }
        }

        private string _LogLevel;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "LogLevel",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "LogLevel" ,Length=50,IsNullable = false,ColumnDescription = "" )]
        public string LogLevel
        { 
            get{return _LogLevel;}
            set{
            SetProperty(ref _LogLevel, value);
                        }
        }

        private string _LogLogger;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "LogLogger",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "LogLogger" ,Length=255,IsNullable = false,ColumnDescription = "" )]
        public string LogLogger
        { 
            get{return _LogLogger;}
            set{
            SetProperty(ref _LogLogger, value);
                        }
        }

        private string _LogActionClick;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "LogActionClick",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "LogActionClick" ,Length=1000,IsNullable = true,ColumnDescription = "" )]
        public string LogActionClick
        { 
            get{return _LogActionClick;}
            set{
            SetProperty(ref _LogActionClick, value);
                        }
        }

        private string _LogMessage;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "LogMessage",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "LogMessage" ,Length=-1,IsNullable = false,ColumnDescription = "" )]
        public string LogMessage
        { 
            get{return _LogMessage;}
            set{
            SetProperty(ref _LogMessage, value);
                        }
        }

        private string _UserName;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "UserName",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "UserName" ,Length=255,IsNullable = true,ColumnDescription = "" )]
        public string UserName
        { 
            get{return _UserName;}
            set{
            SetProperty(ref _UserName, value);
                        }
        }

        private string _UserIP;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "UserIP",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "UserIP" ,Length=45,IsNullable = true,ColumnDescription = "" )]
        public string UserIP
        { 
            get{return _UserIP;}
            set{
            SetProperty(ref _UserIP, value);
                        }
        }

        #endregion

        #region 扩展属性


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
                    Type type = typeof(LogDetails);
                    
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
            LogDetails loctype = (LogDetails)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

