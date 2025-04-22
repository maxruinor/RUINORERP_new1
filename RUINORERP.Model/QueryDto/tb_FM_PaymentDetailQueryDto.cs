
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:09
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
    /// 付款申请单明细-对应的应付单据项目
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_PaymentDetail")]
    public partial class tb_FM_PaymentDetailQueryDto:BaseEntityDto
    {
        public tb_FM_PaymentDetailQueryDto()
        {

        }

    
     

        private long? _PaymentID;
        /// <summary>
        /// 付款申请单
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentID",ColDesc = "付款申请单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PaymentID",IsNullable = true,ColumnDescription = "付款申请单" )]
        [FKRelationAttribute("tb_FM_Payment","PaymentID")]
        public long? PaymentID 
        { 
            get{return _PaymentID;}
            set{SetProperty(ref _PaymentID, value);}
        }
     

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProjectGroup_ID",IsNullable = true,ColumnDescription = "项目组" )]
        [FKRelationAttribute("tb_ProjectGroup","ProjectGroup_ID")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}
            set{SetProperty(ref _ProjectGroup_ID, value);}
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
     

        private long? _tb__DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "tb__DepartmentID",ColDesc = "部门")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "tb__DepartmentID",IsNullable = true,ColumnDescription = "部门" )]
        [FKRelationAttribute("tb_Department","tb__DepartmentID")]
        public long? tb__DepartmentID 
        { 
            get{return _tb__DepartmentID;}
            set{SetProperty(ref _tb__DepartmentID, value);}
        }
     

        private int? _SourceBill_BizType;
        /// <summary>
        /// 来源业务
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBill_BizType",ColDesc = "来源业务")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "SourceBill_BizType",IsNullable = true,ColumnDescription = "来源业务" )]
        public int? SourceBill_BizType 
        { 
            get{return _SourceBill_BizType;}
            set{SetProperty(ref _SourceBill_BizType, value);}
        }
     

        private long? _SourceBill_ID;
        /// <summary>
        /// 来源单据
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBill_ID",ColDesc = "来源单据")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "SourceBill_ID",IsNullable = true,ColumnDescription = "来源单据" )]
        public long? SourceBill_ID 
        { 
            get{return _SourceBill_ID;}
            set{SetProperty(ref _SourceBill_ID, value);}
        }
     

        private string _SourceBillNO;
        /// <summary>
        /// 来源单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillNO",ColDesc = "来源单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "SourceBillNO",Length=30,IsNullable = true,ColumnDescription = "来源单号" )]
        public string SourceBillNO 
        { 
            get{return _SourceBillNO;}
            set{SetProperty(ref _SourceBillNO, value);}
        }
     

        private bool? _IsAdvancePayment= false;
        /// <summary>
        /// 为预付款
        /// </summary>
        [AdvQueryAttribute(ColName = "IsAdvancePayment",ColDesc = "为预付款")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsAdvancePayment",IsNullable = true,ColumnDescription = "为预付款" )]
        public bool? IsAdvancePayment 
        { 
            get{return _IsAdvancePayment;}
            set{SetProperty(ref _IsAdvancePayment, value);}
        }
     

        private string _PayReasonItems;
        /// <summary>
        /// 付款项目/原因
        /// </summary>
        [AdvQueryAttribute(ColName = "PayReasonItems",ColDesc = "付款项目/原因")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PayReasonItems",Length=200,IsNullable = false,ColumnDescription = "付款项目/原因" )]
        public string PayReasonItems 
        { 
            get{return _PayReasonItems;}
            set{SetProperty(ref _PayReasonItems, value);}
        }
     

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Summary",Length=300,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary 
        { 
            get{return _Summary;}
            set{SetProperty(ref _Summary, value);}
        }
     

        private decimal? _SubAmount;
        /// <summary>
        /// 付款金额
        /// </summary>
        [AdvQueryAttribute(ColName = "SubAmount",ColDesc = "付款金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubAmount",IsNullable = true,ColumnDescription = "付款金额" )]
        public decimal? SubAmount 
        { 
            get{return _SubAmount;}
            set{SetProperty(ref _SubAmount, value);}
        }
     

        private string _SubPamountInWords;
        /// <summary>
        /// 大写金额
        /// </summary>
        [AdvQueryAttribute(ColName = "SubPamountInWords",ColDesc = "大写金额")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "SubPamountInWords",Length=100,IsNullable = false,ColumnDescription = "大写金额" )]
        public string SubPamountInWords 
        { 
            get{return _SubPamountInWords;}
            set{SetProperty(ref _SubPamountInWords, value);}
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
     

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
            set{SetProperty(ref _Modified_at, value);}
        }
     

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
            set{SetProperty(ref _Modified_by, value);}
        }


       
    }
}



