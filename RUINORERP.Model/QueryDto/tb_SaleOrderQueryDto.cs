
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/31/2024 20:20:01
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
    /// 销售订单
    /// </summary>
    [Serializable()]
    [SugarTable("tb_SaleOrder")]
    public partial class tb_SaleOrderQueryDto:BaseEntityDto
    {
        public tb_SaleOrderQueryDto()
        {

        }

    
     

        private string _SOrderNo;
        /// <summary>
        /// 订单编号
        /// </summary>
        [AdvQueryAttribute(ColName = "SOrderNo",ColDesc = "订单编号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "SOrderNo",Length=50,IsNullable = false,ColumnDescription = "订单编号" )]
        public string SOrderNo 
        { 
            get{return _SOrderNo;}
            set{SetProperty(ref _SOrderNo, value);}
        }
     

        private int? _PayStatus;
        /// <summary>
        /// 付款状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PayStatus",ColDesc = "付款状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "PayStatus",IsNullable = true,ColumnDescription = "付款状态" )]
        public int? PayStatus 
        { 
            get{return _PayStatus;}
            set{SetProperty(ref _PayStatus, value);}
        }
     

        private long? _Paytype_ID;
        /// <summary>
        /// 付款类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_ID",ColDesc = "付款类型")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Paytype_ID",IsNullable = true,ColumnDescription = "付款类型" )]
        [FKRelationAttribute("tb_PaymentMethod","Paytype_ID")]
        public long? Paytype_ID 
        { 
            get{return _Paytype_ID;}
            set{SetProperty(ref _Paytype_ID, value);}
        }
     

        private long _CustomerVendor_ID;
        /// <summary>
        /// 客户
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "客户")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CustomerVendor_ID",IsNullable = false,ColumnDescription = "客户" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{SetProperty(ref _CustomerVendor_ID, value);}
        }
     

        private long? _Employee_ID;
        /// <summary>
        /// 业务员
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "业务员")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "业务员" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
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
     

        private decimal _ShipCost= ((0));
        /// <summary>
        /// 运费
        /// </summary>
        [AdvQueryAttribute(ColName = "ShipCost",ColDesc = "运费")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "ShipCost",IsNullable = false,ColumnDescription = "运费" )]
        public decimal ShipCost 
        { 
            get{return _ShipCost;}
            set{SetProperty(ref _ShipCost, value);}
        }
     

        private int _TotalQty= ((0));
        /// <summary>
        /// 总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty",ColDesc = "总数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "TotalQty",IsNullable = false,ColumnDescription = "总数量" )]
        public int TotalQty 
        { 
            get{return _TotalQty;}
            set{SetProperty(ref _TotalQty, value);}
        }
     

        private decimal _TotalCost= ((0));
        /// <summary>
        /// 总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalCost",ColDesc = "总成本")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalCost",IsNullable = false,ColumnDescription = "总成本" )]
        public decimal TotalCost 
        { 
            get{return _TotalCost;}
            set{SetProperty(ref _TotalCost, value);}
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
     

        private decimal _TotalTaxAmount= ((0));
        /// <summary>
        /// 总税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalTaxAmount",ColDesc = "总税额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalTaxAmount",IsNullable = false,ColumnDescription = "总税额" )]
        public decimal TotalTaxAmount 
        { 
            get{return _TotalTaxAmount;}
            set{SetProperty(ref _TotalTaxAmount, value);}
        }
     

        private DateTime? _PreDeliveryDate;
        /// <summary>
        /// 预交日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PreDeliveryDate",ColDesc = "预交日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "PreDeliveryDate",IsNullable = true,ColumnDescription = "预交日期" )]
        public DateTime? PreDeliveryDate 
        { 
            get{return _PreDeliveryDate;}
            set{SetProperty(ref _PreDeliveryDate, value);}
        }
     

        private DateTime _SaleDate;
        /// <summary>
        /// 订单日期
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleDate",ColDesc = "订单日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "SaleDate",IsNullable = false,ColumnDescription = "订单日期" )]
        public DateTime SaleDate 
        { 
            get{return _SaleDate;}
            set{SetProperty(ref _SaleDate, value);}
        }
     

        private DateTime? _DeliveryDate;
        /// <summary>
        /// 发货日期
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveryDate",ColDesc = "发货日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "DeliveryDate",IsNullable = true,ColumnDescription = "发货日期" )]
        public DateTime? DeliveryDate 
        { 
            get{return _DeliveryDate;}
            set{SetProperty(ref _DeliveryDate, value);}
        }
     

        private string _ShippingAddress;
        /// <summary>
        /// 发货地址
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingAddress",ColDesc = "发货地址")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ShippingAddress",Length=500,IsNullable = true,ColumnDescription = "发货地址" )]
        public string ShippingAddress 
        { 
            get{return _ShippingAddress;}
            set{SetProperty(ref _ShippingAddress, value);}
        }
     

        private string _ShippingWay;
        /// <summary>
        /// 发货方式
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingWay",ColDesc = "发货方式")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ShippingWay",Length=50,IsNullable = true,ColumnDescription = "发货方式" )]
        public string ShippingWay 
        { 
            get{return _ShippingWay;}
            set{SetProperty(ref _ShippingWay, value);}
        }
     

        private string _TrackNo;
        /// <summary>
        /// 物流单号
        /// </summary>
        [AdvQueryAttribute(ColName = "TrackNo",ColDesc = "物流单号")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "TrackNo",Length=50,IsNullable = true,ColumnDescription = "物流单号" )]
        public string TrackNo 
        { 
            get{return _TrackNo;}
            set{SetProperty(ref _TrackNo, value);}
        }
     

        private decimal _CollectedMoney= ((0));
        /// <summary>
        /// 实收金额
        /// </summary>
        [AdvQueryAttribute(ColName = "CollectedMoney",ColDesc = "实收金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "CollectedMoney",IsNullable = false,ColumnDescription = "实收金额" )]
        public decimal CollectedMoney 
        { 
            get{return _CollectedMoney;}
            set{SetProperty(ref _CollectedMoney, value);}
        }
     

        private decimal _PrePayMoney= ((0));
        /// <summary>
        /// 预收款
        /// </summary>
        [AdvQueryAttribute(ColName = "PrePayMoney",ColDesc = "预收款")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "PrePayMoney",IsNullable = false,ColumnDescription = "预收款" )]
        public decimal PrePayMoney 
        { 
            get{return _PrePayMoney;}
            set{SetProperty(ref _PrePayMoney, value);}
        }
     

        private decimal _Deposit= ((0));
        /// <summary>
        /// 订金
        /// </summary>
        [AdvQueryAttribute(ColName = "Deposit",ColDesc = "订金")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Deposit",IsNullable = false,ColumnDescription = "订金" )]
        public decimal Deposit 
        { 
            get{return _Deposit;}
            set{SetProperty(ref _Deposit, value);}
        }
     

        private decimal? _TaxRate= ((0));
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
     

        private bool? _DeliveryDateConfirm;
        /// <summary>
        /// 交期确认
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveryDateConfirm",ColDesc = "交期确认")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "DeliveryDateConfirm",IsNullable = true,ColumnDescription = "交期确认" )]
        public bool? DeliveryDateConfirm 
        { 
            get{return _DeliveryDateConfirm;}
            set{SetProperty(ref _DeliveryDateConfirm, value);}
        }
     

        private decimal _TotalUntaxedAmount= ((0));
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalUntaxedAmount",ColDesc = "未税本位币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalUntaxedAmount",IsNullable = false,ColumnDescription = "未税本位币" )]
        public decimal TotalUntaxedAmount 
        { 
            get{return _TotalUntaxedAmount;}
            set{SetProperty(ref _TotalUntaxedAmount, value);}
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
     

        private string _CloseCaseOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "CloseCaseOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CloseCaseOpinions",Length=200,IsNullable = true,ColumnDescription = "审批意见" )]
        public string CloseCaseOpinions 
        { 
            get{return _CloseCaseOpinions;}
            set{SetProperty(ref _CloseCaseOpinions, value);}
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
     

        private int _OrderPriority= ((0));
        /// <summary>
        /// 紧急程度
        /// </summary>
        [AdvQueryAttribute(ColName = "OrderPriority",ColDesc = "紧急程度")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "OrderPriority",IsNullable = false,ColumnDescription = "紧急程度" )]
        public int OrderPriority 
        { 
            get{return _OrderPriority;}
            set{SetProperty(ref _OrderPriority, value);}
        }
     

        private string _PlatformOrderNo;
        /// <summary>
        /// 平台单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PlatformOrderNo",ColDesc = "平台单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PlatformOrderNo",Length=100,IsNullable = true,ColumnDescription = "平台单号" )]
        public string PlatformOrderNo 
        { 
            get{return _PlatformOrderNo;}
            set{SetProperty(ref _PlatformOrderNo, value);}
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
     

        private bool _IsNotFromPlatform;
        /// <summary>
        /// 非平台单
        /// </summary>
        [AdvQueryAttribute(ColName = "IsNotFromPlatform",ColDesc = "非平台单")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsNotFromPlatform",IsNullable = false,ColumnDescription = "非平台单" )]
        public bool IsNotFromPlatform 
        { 
            get{return _IsNotFromPlatform;}
            set{SetProperty(ref _IsNotFromPlatform, value);}
        }
     

        private long? _RefBillID;
        /// <summary>
        /// 引用单据
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBillID",ColDesc = "引用单据")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "RefBillID",IsNullable = true,ColumnDescription = "引用单据" )]
        public long? RefBillID 
        { 
            get{return _RefBillID;}
            set{SetProperty(ref _RefBillID, value);}
        }
     

        private string _RefNO;
        /// <summary>
        /// 引用单号
        /// </summary>
        [AdvQueryAttribute(ColName = "RefNO",ColDesc = "引用单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "RefNO",Length=50,IsNullable = true,ColumnDescription = "引用单号" )]
        public string RefNO 
        { 
            get{return _RefNO;}
            set{SetProperty(ref _RefNO, value);}
        }
     

        private int? _RefBizType;
        /// <summary>
        /// 引用单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBizType",ColDesc = "引用单据类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "RefBizType",IsNullable = true,ColumnDescription = "引用单据类型" )]
        public int? RefBizType 
        { 
            get{return _RefBizType;}
            set{SetProperty(ref _RefBizType, value);}
        }


       
    }
}



