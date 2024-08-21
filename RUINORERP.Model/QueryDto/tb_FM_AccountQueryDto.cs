
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:11
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
    /// 账户管理，财务系统中使用
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_Account")]
    public partial class tb_FM_AccountQueryDto:BaseEntityDto
    {
        public tb_FM_AccountQueryDto()
        {

        }

    
     

        private long? _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "部门")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}
            set{SetProperty(ref _DepartmentID, value);}
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
     

        private long? _Currency_ID;
        /// <summary>
        /// 币种
        /// </summary>
        [AdvQueryAttribute(ColName = "Currency_ID",ColDesc = "币种")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Currency_ID",IsNullable = true,ColumnDescription = "币种" )]
        [FKRelationAttribute("tb_Currency","Currency_ID")]
        public long? Currency_ID 
        { 
            get{return _Currency_ID;}
            set{SetProperty(ref _Currency_ID, value);}
        }
     

        private string _account_name;
        /// <summary>
        /// 账户名称
        /// </summary>
        [AdvQueryAttribute(ColName = "account_name",ColDesc = "账户名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "account_name",Length=50,IsNullable = true,ColumnDescription = "账户名称" )]
        public string account_name 
        { 
            get{return _account_name;}
            set{SetProperty(ref _account_name, value);}
        }
     

        private string _account_No;
        /// <summary>
        /// 账号
        /// </summary>
        [AdvQueryAttribute(ColName = "account_No",ColDesc = "账号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "account_No",Length=100,IsNullable = true,ColumnDescription = "账号" )]
        public string account_No 
        { 
            get{return _account_No;}
            set{SetProperty(ref _account_No, value);}
        }
     

        private int? _account_type;
        /// <summary>
        /// 账户类型
        /// </summary>
        [AdvQueryAttribute(ColName = "account_type",ColDesc = "账户类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "account_type",IsNullable = true,ColumnDescription = "账户类型" )]
        public int? account_type 
        { 
            get{return _account_type;}
            set{SetProperty(ref _account_type, value);}
        }
     

        private string _Bank;
        /// <summary>
        /// 所属银行
        /// </summary>
        [AdvQueryAttribute(ColName = "Bank",ColDesc = "所属银行")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Bank",Length=30,IsNullable = true,ColumnDescription = "所属银行" )]
        public string Bank 
        { 
            get{return _Bank;}
            set{SetProperty(ref _Bank, value);}
        }
     

        private decimal? _OpeningBalance;
        /// <summary>
        /// 初始余额
        /// </summary>
        [AdvQueryAttribute(ColName = "OpeningBalance",ColDesc = "初始余额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "OpeningBalance",IsNullable = true,ColumnDescription = "初始余额" )]
        public decimal? OpeningBalance 
        { 
            get{return _OpeningBalance;}
            set{SetProperty(ref _OpeningBalance, value);}
        }
     

        private decimal? _CurrentBalance;
        /// <summary>
        /// 当前余额
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrentBalance",ColDesc = "当前余额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "CurrentBalance",IsNullable = true,ColumnDescription = "当前余额" )]
        public decimal? CurrentBalance 
        { 
            get{return _CurrentBalance;}
            set{SetProperty(ref _CurrentBalance, value);}
        }


       
    }
}



