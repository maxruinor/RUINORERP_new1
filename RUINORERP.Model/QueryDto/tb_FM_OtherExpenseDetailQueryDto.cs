
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:42
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
    /// 其它费用记录表，账户管理，财务系统中使用,像基础资料一样单表操作简单
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_OtherExpenseDetail")]
    public partial class tb_FM_OtherExpenseDetailQueryDto:BaseEntityDto
    {
        public tb_FM_OtherExpenseDetailQueryDto()
        {

        }

    
     

        private long _ExpenseMainID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpenseMainID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ExpenseMainID",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_FM_OtherExpense","ExpenseMainID")]
        public long ExpenseMainID 
        { 
            get{return _ExpenseMainID;}
            set{SetProperty(ref _ExpenseMainID, value);}
        }
     

        private string _ExpenseName;
        /// <summary>
        /// 事由
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpenseName",ColDesc = "事由")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ExpenseName",Length=300,IsNullable = false,ColumnDescription = "事由" )]
        public string ExpenseName 
        { 
            get{return _ExpenseName;}
            set{SetProperty(ref _ExpenseName, value);}
        }
     

        private long? _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "经办人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private long? _DepartmentID;
        /// <summary>
        /// 发生部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "发生部门")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "发生部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}
            set{SetProperty(ref _DepartmentID, value);}
        }
     

        private long? _ExpenseType_id;
        /// <summary>
        /// 费用类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpenseType_id",ColDesc = "费用类型")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ExpenseType_id",IsNullable = true,ColumnDescription = "费用类型" )]
        [FKRelationAttribute("tb_FM_ExpenseType","ExpenseType_id")]
        public long? ExpenseType_id 
        { 
            get{return _ExpenseType_id;}
            set{SetProperty(ref _ExpenseType_id, value);}
        }
     

        private long? _account_id;
        /// <summary>
        /// 交易账号
        /// </summary>
        [AdvQueryAttribute(ColName = "account_id",ColDesc = "交易账号")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "account_id",IsNullable = true,ColumnDescription = "交易账号" )]
        [FKRelationAttribute("tb_FM_Account","account_id")]
        public long? account_id 
        { 
            get{return _account_id;}
            set{SetProperty(ref _account_id, value);}
        }
     

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 交易对象
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "交易对象")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CustomerVendor_ID",IsNullable = true,ColumnDescription = "交易对象" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{SetProperty(ref _CustomerVendor_ID, value);}
        }
     

        private long? _subject_id;
        /// <summary>
        /// 会计科目
        /// </summary>
        [AdvQueryAttribute(ColName = "subject_id",ColDesc = "会计科目")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "subject_id",IsNullable = true,ColumnDescription = "会计科目" )]
        [FKRelationAttribute("tb_FM_Subject","subject_id")]
        public long? subject_id 
        { 
            get{return _subject_id;}
            set{SetProperty(ref _subject_id, value);}
        }
     

        private DateTime _CheckOutDate;
        /// <summary>
        /// 交易日期
        /// </summary>
        [AdvQueryAttribute(ColName = "CheckOutDate",ColDesc = "交易日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "CheckOutDate",IsNullable = false,ColumnDescription = "交易日期" )]
        public DateTime CheckOutDate 
        { 
            get{return _CheckOutDate;}
            set{SetProperty(ref _CheckOutDate, value);}
        }
     

        private decimal _TotalAmount= ((0));
        /// <summary>
        /// 总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "总金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalAmount",IsNullable = false,ColumnDescription = "总金额" )]
        public decimal TotalAmount 
        { 
            get{return _TotalAmount;}
            set{SetProperty(ref _TotalAmount, value);}
        }
     

        private bool _IncludeTax= false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IncludeTax",ColDesc = "含税")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IncludeTax",IsNullable = false,ColumnDescription = "含税" )]
        public bool IncludeTax 
        { 
            get{return _IncludeTax;}
            set{SetProperty(ref _IncludeTax, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=100,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }
     

        private decimal? _TaxAmount;
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxAmount",ColDesc = "税额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TaxAmount",IsNullable = true,ColumnDescription = "税额" )]
        public decimal? TaxAmount 
        { 
            get{return _TaxAmount;}
            set{SetProperty(ref _TaxAmount, value);}
        }
     

        private decimal? _TaxRate;
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "TaxRate",IsNullable = true,ColumnDescription = "税率" )]
        public decimal? TaxRate 
        { 
            get{return _TaxRate;}
            set{SetProperty(ref _TaxRate, value);}
        }
     

        private decimal _UntaxedAmount;
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "UntaxedAmount",ColDesc = "未税本位币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "UntaxedAmount",IsNullable = false,ColumnDescription = "未税本位币" )]
        public decimal UntaxedAmount 
        { 
            get{return _UntaxedAmount;}
            set{SetProperty(ref _UntaxedAmount, value);}
        }
     

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 所属项目
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "所属项目")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProjectGroup_ID",IsNullable = true,ColumnDescription = "所属项目" )]
        [FKRelationAttribute("tb_ProjectGroup","ProjectGroup_ID")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}
            set{SetProperty(ref _ProjectGroup_ID, value);}
        }


       
    }
}



