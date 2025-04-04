﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:17
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
    /// 采购入库单 供应商接到采购订单后，向企业发货，用户在收到货物时，可以先检验，对合格品进行入库，也可以直接入库，形成采购入库单。为了保证清楚地记录进货情况，对进货的管理就很重要，而在我们的系统中，凭证、收付款是根据进货单自动一环扣一环地切制，故详细输入进货单资料后，存货的数量、成本会随着改变，收付帐款也会跟着你的立帐方式变化；凭证亦会随着“您是否立即产生凭证”变化。采购入库单可以由采购订单、借入单、在途物资单转入，也可以手动录入新增单据。
    /// </summary>
    [Serializable()]
    [SugarTable("tb_PurEntry")]
    public partial class tb_PurEntryQueryDto:BaseEntityDto
    {
        public tb_PurEntryQueryDto()
        {

        }

    
     

        private string _PurEntryNo;
        /// <summary>
        /// 入库单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PurEntryNo",ColDesc = "入库单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PurEntryNo",Length=50,IsNullable = false,ColumnDescription = "入库单号" )]
        public string PurEntryNo 
        { 
            get{return _PurEntryNo;}
            set{SetProperty(ref _PurEntryNo, value);}
        }
     

        private long _CustomerVendor_ID;
        /// <summary>
        /// 厂商
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "厂商")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CustomerVendor_ID",IsNullable = false,ColumnDescription = "厂商" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{SetProperty(ref _CustomerVendor_ID, value);}
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
     

        private long? _Paytype_ID;
        /// <summary>
        /// 交易方式
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_ID",ColDesc = "交易方式")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Paytype_ID",IsNullable = true,ColumnDescription = "交易方式" )]
        [FKRelationAttribute("tb_PaymentMethod","Paytype_ID")]
        public long? Paytype_ID 
        { 
            get{return _Paytype_ID;}
            set{SetProperty(ref _Paytype_ID, value);}
        }
     

        private long? _PurOrder_ID;
        /// <summary>
        /// 采购订单
        /// </summary>
        [AdvQueryAttribute(ColName = "PurOrder_ID",ColDesc = "采购订单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PurOrder_ID",IsNullable = true,ColumnDescription = "采购订单" )]
        [FKRelationAttribute("tb_PurOrder","PurOrder_ID")]
        public long? PurOrder_ID 
        { 
            get{return _PurOrder_ID;}
            set{SetProperty(ref _PurOrder_ID, value);}
        }
     

        private decimal _TotalQty= ((0));
        /// <summary>
        /// 合计数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty",ColDesc = "合计数量")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalQty",IsNullable = false,ColumnDescription = "合计数量" )]
        public decimal TotalQty 
        { 
            get{return _TotalQty;}
            set{SetProperty(ref _TotalQty, value);}
        }
     

        private decimal _TotalAmount= ((0));
        /// <summary>
        /// 合计金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "合计金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalAmount",IsNullable = false,ColumnDescription = "合计金额" )]
        public decimal TotalAmount 
        { 
            get{return _TotalAmount;}
            set{SetProperty(ref _TotalAmount, value);}
        }
     

        private decimal _ActualAmount= ((0));
        /// <summary>
        /// 实付金额
        /// </summary>
        [AdvQueryAttribute(ColName = "ActualAmount",ColDesc = "实付金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "ActualAmount",IsNullable = false,ColumnDescription = "实付金额" )]
        public decimal ActualAmount 
        { 
            get{return _ActualAmount;}
            set{SetProperty(ref _ActualAmount, value);}
        }
     

        private decimal _TotalTaxAmount= ((0));
        /// <summary>
        /// 合计税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalTaxAmount",ColDesc = "合计税额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalTaxAmount",IsNullable = false,ColumnDescription = "合计税额" )]
        public decimal TotalTaxAmount 
        { 
            get{return _TotalTaxAmount;}
            set{SetProperty(ref _TotalTaxAmount, value);}
        }
     

        private decimal _DiscountAmount= ((0));
        /// <summary>
        /// 折扣金额总计
        /// </summary>
        [AdvQueryAttribute(ColName = "DiscountAmount",ColDesc = "折扣金额总计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "DiscountAmount",IsNullable = false,ColumnDescription = "折扣金额总计" )]
        public decimal DiscountAmount 
        { 
            get{return _DiscountAmount;}
            set{SetProperty(ref _DiscountAmount, value);}
        }
     

        private DateTime _EntryDate;
        /// <summary>
        /// 入库日期
        /// </summary>
        [AdvQueryAttribute(ColName = "EntryDate",ColDesc = "入库日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "EntryDate",IsNullable = false,ColumnDescription = "入库日期" )]
        public DateTime EntryDate 
        { 
            get{return _EntryDate;}
            set{SetProperty(ref _EntryDate, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=1500,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
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
     

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ApprovalOpinions",Length=200,IsNullable = true,ColumnDescription = "审批意见" )]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}
            set{SetProperty(ref _ApprovalOpinions, value);}
        }
     

        private int? _ApprovalStatus;
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
     

        private int _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "DataStatus",IsNullable = false,ColumnDescription = "数据状态" )]
        public int DataStatus 
        { 
            get{return _DataStatus;}
            set{SetProperty(ref _DataStatus, value);}
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
     

        private bool? _IsIncludeTax;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IsIncludeTax",ColDesc = "含税")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsIncludeTax",IsNullable = true,ColumnDescription = "含税" )]
        public bool? IsIncludeTax 
        { 
            get{return _IsIncludeTax;}
            set{SetProperty(ref _IsIncludeTax, value);}
        }
     

        private int? _KeepAccountsType;
        /// <summary>
        /// 立帐类型
        /// </summary>
        [AdvQueryAttribute(ColName = "KeepAccountsType",ColDesc = "立帐类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "KeepAccountsType",IsNullable = true,ColumnDescription = "立帐类型" )]
        public int? KeepAccountsType 
        { 
            get{return _KeepAccountsType;}
            set{SetProperty(ref _KeepAccountsType, value);}
        }
     

        private decimal? _Deposit;
        /// <summary>
        /// 订金
        /// </summary>
        [AdvQueryAttribute(ColName = "Deposit",ColDesc = "订金")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Deposit",IsNullable = true,ColumnDescription = "订金" )]
        public decimal? Deposit 
        { 
            get{return _Deposit;}
            set{SetProperty(ref _Deposit, value);}
        }
     

        private int? _TaxDeductionType;
        /// <summary>
        /// 扣税类型
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxDeductionType",ColDesc = "扣税类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "TaxDeductionType",IsNullable = true,ColumnDescription = "扣税类型" )]
        public int? TaxDeductionType 
        { 
            get{return _TaxDeductionType;}
            set{SetProperty(ref _TaxDeductionType, value);}
        }
     

        private bool? _ReceiptInvoiceClosed;
        /// <summary>
        /// 立帐结案
        /// </summary>
        [AdvQueryAttribute(ColName = "ReceiptInvoiceClosed",ColDesc = "立帐结案")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "ReceiptInvoiceClosed",IsNullable = true,ColumnDescription = "立帐结案" )]
        public bool? ReceiptInvoiceClosed 
        { 
            get{return _ReceiptInvoiceClosed;}
            set{SetProperty(ref _ReceiptInvoiceClosed, value);}
        }
     

        private bool? _GenerateVouchers;
        /// <summary>
        /// 生成凭证
        /// </summary>
        [AdvQueryAttribute(ColName = "GenerateVouchers",ColDesc = "生成凭证")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "GenerateVouchers",IsNullable = true,ColumnDescription = "生成凭证" )]
        public bool? GenerateVouchers 
        { 
            get{return _GenerateVouchers;}
            set{SetProperty(ref _GenerateVouchers, value);}
        }
     

        private string _VoucherNO;
        /// <summary>
        /// 凭证号码
        /// </summary>
        [AdvQueryAttribute(ColName = "VoucherNO",ColDesc = "凭证号码")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "VoucherNO",Length=50,IsNullable = true,ColumnDescription = "凭证号码" )]
        public string VoucherNO 
        { 
            get{return _VoucherNO;}
            set{SetProperty(ref _VoucherNO, value);}
        }
     

        private string _PurOrder_NO;
        /// <summary>
        /// 采购订单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PurOrder_NO",ColDesc = "采购订单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PurOrder_NO",Length=50,IsNullable = true,ColumnDescription = "采购订单号" )]
        public string PurOrder_NO 
        { 
            get{return _PurOrder_NO;}
            set{SetProperty(ref _PurOrder_NO, value);}
        }
     

        private decimal _ShippingCost= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingCost",ColDesc = "")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "ShippingCost",IsNullable = false,ColumnDescription = "" )]
        public decimal ShippingCost 
        { 
            get{return _ShippingCost;}
            set{SetProperty(ref _ShippingCost, value);}
        }


       
    }
}



