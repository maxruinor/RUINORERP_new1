
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:41
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
    /// 业务类型 报销，员工借支还款，运费
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_ExpenseType")]
    public partial class tb_FM_ExpenseTypeQueryDto:BaseEntityDto
    {
        public tb_FM_ExpenseTypeQueryDto()
        {

        }

    
     

        private long? _subject_id;
        /// <summary>
        /// 科目
        /// </summary>
        [AdvQueryAttribute(ColName = "subject_id",ColDesc = "科目")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "subject_id",IsNullable = true,ColumnDescription = "科目" )]
        [FKRelationAttribute("tb_FM_Subject","subject_id")]
        public long? subject_id 
        { 
            get{return _subject_id;}
            set{SetProperty(ref _subject_id, value);}
        }
     

        private string _Expense_name;
        /// <summary>
        /// 费用业务名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Expense_name",ColDesc = "费用业务名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Expense_name",Length=50,IsNullable = true,ColumnDescription = "费用业务名称" )]
        public string Expense_name 
        { 
            get{return _Expense_name;}
            set{SetProperty(ref _Expense_name, value);}
        }
     

        private bool _EXPOrINC= true;
        /// <summary>
        /// 收支标识
        /// </summary>
        [AdvQueryAttribute(ColName = "EXPOrINC",ColDesc = "收支标识")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "EXPOrINC",IsNullable = false,ColumnDescription = "收支标识" )]
        public bool EXPOrINC 
        { 
            get{return _EXPOrINC;}
            set{SetProperty(ref _EXPOrINC, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=30,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



