
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:04
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
    /// 付款账号管理
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
     

        private long? _Subject_id;
        /// <summary>
        /// 会计科目
        /// </summary>
        [AdvQueryAttribute(ColName = "Subject_id",ColDesc = "会计科目")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Subject_id",IsNullable = true,ColumnDescription = "会计科目" )]
        [FKRelationAttribute("tb_FM_Subject","Subject_id")]
        public long? Subject_id 
        { 
            get{return _Subject_id;}
            set{SetProperty(ref _Subject_id, value);}
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
     

        private string _Account_name;
        /// <summary>
        /// 账户名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_name",ColDesc = "账户名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Account_name",Length=50,IsNullable = true,ColumnDescription = "账户名称" )]
        public string Account_name 
        { 
            get{return _Account_name;}
            set{SetProperty(ref _Account_name, value);}
        }
     

        private string _Account_No;
        /// <summary>
        /// 账号
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_No",ColDesc = "账号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Account_No",Length=100,IsNullable = true,ColumnDescription = "账号" )]
        public string Account_No 
        { 
            get{return _Account_No;}
            set{SetProperty(ref _Account_No, value);}
        }
     

        private int? _Account_type;
        /// <summary>
        /// 账户类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_type",ColDesc = "账户类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Account_type",IsNullable = true,ColumnDescription = "账户类型" )]
        public int? Account_type 
        { 
            get{return _Account_type;}
            set{SetProperty(ref _Account_type, value);}
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



