
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2026 23:59:01
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
    /// 用户接收提醒内容
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ReminderResult")]
    public partial class tb_ReminderResultQueryDto:BaseEntityDto
    {
        public tb_ReminderResultQueryDto()
        {

        }

    
     

        private long? _RuleId;
        /// <summary>
        /// 提醒规则
        /// </summary>
        [AdvQueryAttribute(ColName = "RuleId",ColDesc = "提醒规则")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "RuleId",IsNullable = true,ColumnDescription = "提醒规则" )]
        [FKRelationAttribute("tb_ReminderRule","RuleId")]
        public long? RuleId 
        { 
            get{return _RuleId;}
            set{SetProperty(ref _RuleId, value);}
        }
     

        private int _ReminderBizType;
        /// <summary>
        /// 提醒类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ReminderBizType",ColDesc = "提醒类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ReminderBizType",IsNullable = false,ColumnDescription = "提醒类型" )]
        public int ReminderBizType 
        { 
            get{return _ReminderBizType;}
            set{SetProperty(ref _ReminderBizType, value);}
        }
     

        private DateTime _TriggerTime;
        /// <summary>
        /// 提醒时间
        /// </summary>
        [AdvQueryAttribute(ColName = "TriggerTime",ColDesc = "提醒时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "TriggerTime",IsNullable = false,ColumnDescription = "提醒时间" )]
        public DateTime TriggerTime 
        { 
            get{return _TriggerTime;}
            set{SetProperty(ref _TriggerTime, value);}
        }
     

        private string _Message;
        /// <summary>
        /// 提醒内容
        /// </summary>
        [AdvQueryAttribute(ColName = "Message",ColDesc = "提醒内容")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Message",Length=200,IsNullable = true,ColumnDescription = "提醒内容" )]
        public string Message 
        { 
            get{return _Message;}
            set{SetProperty(ref _Message, value);}
        }
     

        private bool _IsRead= false;
        /// <summary>
        /// 已读
        /// </summary>
        [AdvQueryAttribute(ColName = "IsRead",ColDesc = "已读")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsRead",IsNullable = false,ColumnDescription = "已读" )]
        public bool IsRead 
        { 
            get{return _IsRead;}
            set{SetProperty(ref _IsRead, value);}
        }
     

        private DateTime? _ReadTime;
        /// <summary>
        /// 读取时间
        /// </summary>
        [AdvQueryAttribute(ColName = "ReadTime",ColDesc = "读取时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "ReadTime",IsNullable = true,ColumnDescription = "读取时间" )]
        public DateTime? ReadTime 
        { 
            get{return _ReadTime;}
            set{SetProperty(ref _ReadTime, value);}
        }
     

        private string _JsonResult;
        /// <summary>
        /// 扩展JSON结果
        /// </summary>
        [AdvQueryAttribute(ColName = "JsonResult",ColDesc = "扩展JSON结果")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "JsonResult",Length=2147483647,IsNullable = true,ColumnDescription = "扩展JSON结果" )]
        public string JsonResult 
        { 
            get{return _JsonResult;}
            set{SetProperty(ref _JsonResult, value);}
        }


       
    }
}



