﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:26:58
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
    /// 付款申请单-目前代替纸的申请单将来完善明细则用付款单的主子表来完成系统可以根据客户来自动生成经人确认
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_PaymentApplication")]
    public partial class tb_FM_PaymentApplicationQueryDto:BaseEntityDto
    {
        public tb_FM_PaymentApplicationQueryDto()
        {

        }

    
     

        private string _ApplicationNo;
        /// <summary>
        /// 申请单号
        /// </summary>
        [AdvQueryAttribute(ColName = "ApplicationNo",ColDesc = "申请单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ApplicationNo",Length=30,IsNullable = false,ColumnDescription = "申请单号" )]
        public string ApplicationNo 
        { 
            get{return _ApplicationNo;}
            set{SetProperty(ref _ApplicationNo, value);}
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
     

        private long _Employee_ID;
        /// <summary>
        /// 制单人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "制单人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = false,ColumnDescription = "制单人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private long _CustomerVendor_ID;
        /// <summary>
        /// 收款单位
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "收款单位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CustomerVendor_ID",IsNullable = false,ColumnDescription = "收款单位" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{SetProperty(ref _CustomerVendor_ID, value);}
        }
     

        private long _PayeeInfoID;
        /// <summary>
        /// 收款信息
        /// </summary>
        [AdvQueryAttribute(ColName = "PayeeInfoID",ColDesc = "收款信息")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PayeeInfoID",IsNullable = false,ColumnDescription = "收款信息" )]
        [FKRelationAttribute("tb_FM_PayeeInfo","PayeeInfoID")]
        public long PayeeInfoID 
        { 
            get{return _PayeeInfoID;}
            set{SetProperty(ref _PayeeInfoID, value);}
        }
     

        private string _PayeeAccountNo;
        /// <summary>
        /// 收款账号
        /// </summary>
        [AdvQueryAttribute(ColName = "PayeeAccountNo",ColDesc = "收款账号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PayeeAccountNo",Length=100,IsNullable = false,ColumnDescription = "收款账号" )]
        public string PayeeAccountNo 
        { 
            get{return _PayeeAccountNo;}
            set{SetProperty(ref _PayeeAccountNo, value);}
        }
     

        private long _Currency_ID;
        /// <summary>
        /// 币别
        /// </summary>
        [AdvQueryAttribute(ColName = "Currency_ID",ColDesc = "币别")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Currency_ID",IsNullable = false,ColumnDescription = "币别" )]
        [FKRelationAttribute("tb_Currency","Currency_ID")]
        public long Currency_ID 
        { 
            get{return _Currency_ID;}
            set{SetProperty(ref _Currency_ID, value);}
        }
     

        private long? _Account_id;
        /// <summary>
        /// 付款账户
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_id",ColDesc = "付款账户")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Account_id",IsNullable = true,ColumnDescription = "付款账户" )]
        [FKRelationAttribute("tb_FM_Account","Account_id")]
        public long? Account_id 
        { 
            get{return _Account_id;}
            set{SetProperty(ref _Account_id, value);}
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
     

        private long? _PrePaymentBill_id;
        /// <summary>
        /// 预付单
        /// </summary>
        [AdvQueryAttribute(ColName = "PrePaymentBill_id",ColDesc = "预付单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PrePaymentBill_id",IsNullable = true,ColumnDescription = "预付单" )]
        public long? PrePaymentBill_id 
        { 
            get{return _PrePaymentBill_id;}
            set{SetProperty(ref _PrePaymentBill_id, value);}
        }
     

        private string _PayReasonItems;
        /// <summary>
        /// 付款项目/原因
        /// </summary>
        [AdvQueryAttribute(ColName = "PayReasonItems",ColDesc = "付款项目/原因")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PayReasonItems",Length=1000,IsNullable = false,ColumnDescription = "付款项目/原因" )]
        public string PayReasonItems 
        { 
            get{return _PayReasonItems;}
            set{SetProperty(ref _PayReasonItems, value);}
        }
     

        private DateTime _InvoiceDate;
        /// <summary>
        /// 制单日期
        /// </summary>
        [AdvQueryAttribute(ColName = "InvoiceDate",ColDesc = "制单日期")]
        [SugarColumn(ColumnDataType = "date",SqlParameterDbType ="DateTime",ColumnName = "InvoiceDate",IsNullable = false,ColumnDescription = "制单日期" )]
        public DateTime InvoiceDate 
        { 
            get{return _InvoiceDate;}
            set{SetProperty(ref _InvoiceDate, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=300,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }
     

        private decimal? _TotalAmount;
        /// <summary>
        /// 付款金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "付款金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalAmount",IsNullable = true,ColumnDescription = "付款金额" )]
        public decimal? TotalAmount 
        { 
            get{return _TotalAmount;}
            set{SetProperty(ref _TotalAmount, value);}
        }
     

        private string _PamountInWords;
        /// <summary>
        /// 大写金额
        /// </summary>
        [AdvQueryAttribute(ColName = "PamountInWords",ColDesc = "大写金额")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PamountInWords",Length=100,IsNullable = false,ColumnDescription = "大写金额" )]
        public string PamountInWords 
        { 
            get{return _PamountInWords;}
            set{SetProperty(ref _PamountInWords, value);}
        }
     

        private decimal? _OverpaymentAmount;
        /// <summary>
        /// 超付金额
        /// </summary>
        [AdvQueryAttribute(ColName = "OverpaymentAmount",ColDesc = "超付金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "OverpaymentAmount",IsNullable = true,ColumnDescription = "超付金额" )]
        public decimal? OverpaymentAmount 
        { 
            get{return _OverpaymentAmount;}
            set{SetProperty(ref _OverpaymentAmount, value);}
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
     

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "isdeleted",IsNullable = false,ColumnDescription = "逻辑删除" )]
        public bool isdeleted 
        { 
            get{return _isdeleted;}
            set{SetProperty(ref _isdeleted, value);}
        }
     

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ApprovalOpinions",Length=255,IsNullable = true,ColumnDescription = "审批意见" )]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}
            set{SetProperty(ref _ApprovalOpinions, value);}
        }
     

        private long? _Approver_by;
        /// <summary>
        /// 审批人
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_by",ColDesc = "审批人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Approver_by",IsNullable = true,ColumnDescription = "审批人" )]
        public long? Approver_by 
        { 
            get{return _Approver_by;}
            set{SetProperty(ref _Approver_by, value);}
        }
     

        private DateTime? _Approver_at;
        /// <summary>
        /// 审批时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_at",ColDesc = "审批时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Approver_at",IsNullable = true,ColumnDescription = "审批时间" )]
        public DateTime? Approver_at 
        { 
            get{return _Approver_at;}
            set{SetProperty(ref _Approver_at, value);}
        }
     

        private int? _ApprovalStatus= ((0));
        /// <summary>
        /// 审批状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint",SqlParameterDbType ="SByte",ColumnName = "ApprovalStatus",IsNullable = true,ColumnDescription = "审批状态" )]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}
            set{SetProperty(ref _ApprovalStatus, value);}
        }
     

        private bool? _ApprovalResults;
        /// <summary>
        /// 审批结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审批结果")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "ApprovalResults",IsNullable = true,ColumnDescription = "审批结果" )]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}
            set{SetProperty(ref _ApprovalResults, value);}
        }
     

        private int? _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "DataStatus",IsNullable = true,ColumnDescription = "数据状态" )]
        public int? DataStatus 
        { 
            get{return _DataStatus;}
            set{SetProperty(ref _DataStatus, value);}
        }
     

        private int _PrintStatus= ((0));
        /// <summary>
        /// 打印状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintStatus",ColDesc = "打印状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "PrintStatus",IsNullable = false,ColumnDescription = "打印状态" )]
        public int PrintStatus 
        { 
            get{return _PrintStatus;}
            set{SetProperty(ref _PrintStatus, value);}
        }
     

        private string _CloseCaseImagePath;
        /// <summary>
        /// 结案凭证
        /// </summary>
        [AdvQueryAttribute(ColName = "CloseCaseImagePath",ColDesc = "结案凭证")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "CloseCaseImagePath",Length=300,IsNullable = true,ColumnDescription = "结案凭证" )]
        public string CloseCaseImagePath 
        { 
            get{return _CloseCaseImagePath;}
            set{SetProperty(ref _CloseCaseImagePath, value);}
        }
     

        private string _CloseCaseOpinions;
        /// <summary>
        /// 结案意见
        /// </summary>
        [AdvQueryAttribute(ColName = "CloseCaseOpinions",ColDesc = "结案意见")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CloseCaseOpinions",Length=200,IsNullable = true,ColumnDescription = "结案意见" )]
        public string CloseCaseOpinions 
        { 
            get{return _CloseCaseOpinions;}
            set{SetProperty(ref _CloseCaseOpinions, value);}
        }


       
    }
}



