
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
using RUINORERP.Model.Base;

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 提醒内容
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ReminderAlert")]
    public partial class tb_ReminderAlertQueryDto:BaseEntityDto
    {
        public tb_ReminderAlertQueryDto()
        {

        }

    
     

        private long? _RuleId;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "RuleId",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "RuleId",IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ReminderRule","RuleId")]
        public long? RuleId 
        { 
            get{return _RuleId;}
            set{SetProperty(ref _RuleId, value);}
        }
     

        private DateTime? _AlertTime;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "AlertTime",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "AlertTime",IsNullable = true,ColumnDescription = "" )]
        public DateTime? AlertTime 
        { 
            get{return _AlertTime;}
            set{SetProperty(ref _AlertTime, value);}
        }
     

        private string _Message;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Message",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Message",Length=200,IsNullable = true,ColumnDescription = "" )]
        public string Message 
        { 
            get{return _Message;}
            set{SetProperty(ref _Message, value);}
        }


       
    }
}



