
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
using RUINORERP.Model.Base;

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable()]
    [SugarTable("LogDetails")]
    public partial class LogDetailsQueryDto:BaseEntityDto
    {
        public LogDetailsQueryDto()
        {

        }

    
     

        private DateTime _LogDate= '(getdate())';
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "LogDate",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "LogDate",IsNullable = false,ColumnDescription = "" )]
        public DateTime LogDate 
        { 
            get{return _LogDate;}
            set{SetProperty(ref _LogDate, value);}
        }
     

        private string _LogThread;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "LogThread",ColDesc = "")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "LogThread",Length=255,IsNullable = false,ColumnDescription = "" )]
        public string LogThread 
        { 
            get{return _LogThread;}
            set{SetProperty(ref _LogThread, value);}
        }
     

        private string _LogLevel;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "LogLevel",ColDesc = "")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "LogLevel",Length=50,IsNullable = false,ColumnDescription = "" )]
        public string LogLevel 
        { 
            get{return _LogLevel;}
            set{SetProperty(ref _LogLevel, value);}
        }
     

        private string _LogLogger;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "LogLogger",ColDesc = "")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "LogLogger",Length=255,IsNullable = false,ColumnDescription = "" )]
        public string LogLogger 
        { 
            get{return _LogLogger;}
            set{SetProperty(ref _LogLogger, value);}
        }
     

        private string _LogActionClick;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "LogActionClick",ColDesc = "")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "LogActionClick",Length=1000,IsNullable = true,ColumnDescription = "" )]
        public string LogActionClick 
        { 
            get{return _LogActionClick;}
            set{SetProperty(ref _LogActionClick, value);}
        }
     

        private string _LogMessage;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "LogMessage",ColDesc = "")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "LogMessage",Length=-1,IsNullable = false,ColumnDescription = "" )]
        public string LogMessage 
        { 
            get{return _LogMessage;}
            set{SetProperty(ref _LogMessage, value);}
        }
     

        private string _UserName;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "UserName",ColDesc = "")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "UserName",Length=255,IsNullable = true,ColumnDescription = "" )]
        public string UserName 
        { 
            get{return _UserName;}
            set{SetProperty(ref _UserName, value);}
        }
     

        private string _UserIP;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "UserIP",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "UserIP",Length=45,IsNullable = true,ColumnDescription = "" )]
        public string UserIP 
        { 
            get{return _UserIP;}
            set{SetProperty(ref _UserIP, value);}
        }


       
    }
}



