
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/09/2026 20:34:50
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
    /// 链路与规则关联表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ReminderLinkRuleRelation")]
    public partial class tb_ReminderLinkRuleRelationQueryDto:BaseEntityDto
    {
        public tb_ReminderLinkRuleRelationQueryDto()
        {

        }

    
     

        private long? _LinkId;
        /// <summary>
        /// 链路ID
        /// </summary>
        [AdvQueryAttribute(ColName = "LinkId",ColDesc = "链路ID")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "LinkId",IsNullable = true,ColumnDescription = "链路ID" )]
        [FKRelationAttribute("tb_ReminderObjectLink","LinkId")]
        public long? LinkId 
        { 
            get{return _LinkId;}
            set{SetProperty(ref _LinkId, value);}
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
     

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
            set{SetProperty(ref _Created_at, value);}
        }
     

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by 
        { 
            get{return _Created_by;}
            set{SetProperty(ref _Created_by, value);}
        }


       
    }
}



