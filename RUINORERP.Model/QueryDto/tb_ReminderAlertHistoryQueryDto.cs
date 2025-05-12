
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
using RUINORERP.Model.Base;

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 提醒信息是通过什么规则通知了什么内容给谁在什么时间。通知记录  暂时不处理
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ReminderAlertHistory")]
    public partial class tb_ReminderAlertHistoryQueryDto:BaseEntityDto
    {
        public tb_ReminderAlertHistoryQueryDto()
        {

        }

    
     

        private long _AlertId;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "AlertId",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "AlertId",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ReminderAlert","AlertId")]
        public long AlertId 
        { 
            get{return _AlertId;}
            set{SetProperty(ref _AlertId, value);}
        }
     

        private long _User_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "User_ID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "User_ID",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_UserInfo","User_ID")]
        public long User_ID 
        { 
            get{return _User_ID;}
            set{SetProperty(ref _User_ID, value);}
        }
     

        private bool _IsRead;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "IsRead",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsRead",IsNullable = false,ColumnDescription = "" )]
        public bool IsRead 
        { 
            get{return _IsRead;}
            set{SetProperty(ref _IsRead, value);}
        }
     

        private string _Message;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Message",ColDesc = "")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "Message",Length=2147483647,IsNullable = false,ColumnDescription = "" )]
        public string Message 
        { 
            get{return _Message;}
            set{SetProperty(ref _Message, value);}
        }
     

        private DateTime _TriggerTime;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "TriggerTime",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "TriggerTime",IsNullable = false,ColumnDescription = "" )]
        public DateTime TriggerTime 
        { 
            get{return _TriggerTime;}
            set{SetProperty(ref _TriggerTime, value);}
        }
     

        private int _ReminderBizType;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ReminderBizType",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ReminderBizType",IsNullable = false,ColumnDescription = "" )]
        public int ReminderBizType 
        { 
            get{return _ReminderBizType;}
            set{SetProperty(ref _ReminderBizType, value);}
        }


       
    }
}



