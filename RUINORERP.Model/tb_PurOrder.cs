
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:22
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据
    /// </summary>
    [Serializable()]
    [Description("采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据")]
    [SugarTable("tb_PurOrder")]
    public partial class tb_PurOrder: BaseEntity, ICloneable
    {
        public tb_PurOrder()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据tb_PurOrder" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PurOrder_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurOrder_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PurOrder_ID
        { 
            get{return _PurOrder_ID;}
            set{
            SetProperty(ref _PurOrder_ID, value);
                base.PrimaryKeyID = _PurOrder_ID;
            }
        }

        private string _PurOrderNo;
        /// <summary>
        /// 采购单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PurOrderNo",ColDesc = "采购单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PurOrderNo" ,Length=100,IsNullable = false,ColumnDescription = "采购单号" )]
        public string PurOrderNo
        { 
            get{return _PurOrderNo;}
            set{
            SetProperty(ref _PurOrderNo, value);
                        }
        }

        private long _CustomerVendor_ID;
        /// <summary>
        /// 厂商
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "厂商")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "厂商" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
                        }
        }

        private long _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "经办人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private long? _DepartmentID;
        /// <summary>
        /// 使用部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "使用部门")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "使用部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID
        { 
            get{return _DepartmentID;}
            set{
            SetProperty(ref _DepartmentID, value);
                        }
        }

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID", ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "ProjectGroup_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "项目组")]
        [FKRelationAttribute("tb_ProjectGroup", "ProjectGroup_ID")]
        public long? ProjectGroup_ID
        {
            get { return _ProjectGroup_ID; }
            set
            {
                SetProperty(ref _ProjectGroup_ID, value);
            }
        }

        private int _PayStatus;
        /// <summary>
        /// 付款状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PayStatus", ColDesc = "付款状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "PayStatus", DecimalDigits = 0, IsNullable = false, ColumnDescription = "付款状态")]
        public int PayStatus
        {
            get { return _PayStatus; }
            set
            {
                SetProperty(ref _PayStatus, value);
            }
        }
        private long? _Paytype_ID;
        /// <summary>
        /// 付款方式
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_ID",ColDesc = "付款方式")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Paytype_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "付款方式" )]
        [FKRelationAttribute("tb_PaymentMethod","Paytype_ID")]
        public long? Paytype_ID
        { 
            get{return _Paytype_ID;}
            set{
            SetProperty(ref _Paytype_ID, value);
                        }
        }
 

        private long _Currency_ID;
        /// <summary>
        /// 币别
        /// </summary>
        [AdvQueryAttribute(ColName = "Currency_ID", ColDesc = "币别")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Currency_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "币别")]
        [FKRelationAttribute("tb_Currency", "Currency_ID")]
        public long Currency_ID
        {
            get { return _Currency_ID; }
            set
            {
                SetProperty(ref _Currency_ID, value);
            }
        }

        private decimal _ExchangeRate = ((1));
        /// <summary>
        /// 汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "ExchangeRate", ColDesc = "汇率")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType = "Decimal", ColumnName = "ExchangeRate", DecimalDigits = 4, IsNullable = false, ColumnDescription = "汇率")]
        public decimal ExchangeRate
        {
            get { return _ExchangeRate; }
            set
            {
                SetProperty(ref _ExchangeRate, value);
            }
        }
        private bool _IsCustomizedOrder = false;
        /// <summary>
        /// 定制单
        /// </summary>
        [AdvQueryAttribute(ColName = "IsCustomizedOrder", ColDesc = "定制单")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "IsCustomizedOrder", IsNullable = false, ColumnDescription = "定制单")]
        public bool IsCustomizedOrder
        {
            get { return _IsCustomizedOrder; }
            set
            {
                SetProperty(ref _IsCustomizedOrder, value);
            }
        }
        private long? _SOrder_ID;
        /// <summary>
        /// 销售订单
        /// </summary>
        [AdvQueryAttribute(ColName = "SOrder_ID",ColDesc = "销售订单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SOrder_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "销售订单" )]
        [FKRelationAttribute("tb_SaleOrder","SOrder_ID")]
        public long? SOrder_ID
        { 
            get{return _SOrder_ID;}
            set{
            SetProperty(ref _SOrder_ID, value);
                        }
        }

        private long? _PDID;
        /// <summary>
        /// 生产需求
        /// </summary>
        [AdvQueryAttribute(ColName = "PDID",ColDesc = "生产需求")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PDID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "生产需求" )]
        [FKRelationAttribute("tb_ProductionDemand","PDID")]
        public long? PDID
        { 
            get{return _PDID;}
            set{
            SetProperty(ref _PDID, value);
                        }
        }

        private DateTime _PurDate;
        /// <summary>
        /// 采购日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PurDate",ColDesc = "采购日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PurDate" ,IsNullable = false,ColumnDescription = "采购日期" )]
        public DateTime PurDate
        { 
            get{return _PurDate;}
            set{
            SetProperty(ref _PurDate, value);
                        }
        }

        private DateTime? _PreDeliveryDate;
        /// <summary>
        /// 预交日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PreDeliveryDate",ColDesc = "预交日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PreDeliveryDate" ,IsNullable = true,ColumnDescription = "预交日期" )]
        public DateTime? PreDeliveryDate
        { 
            get{return _PreDeliveryDate;}
            set{
            SetProperty(ref _PreDeliveryDate, value);
                        }
        }

        private int _TotalQty;
        /// <summary>
        /// 总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty",ColDesc = "总数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "总数量" )]
        public int TotalQty
        { 
            get{return _TotalQty;}
            set{
            SetProperty(ref _TotalQty, value);
                        }
        }

        private decimal _ShippingCost;
        /// <summary>
        /// 运费
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingCost",ColDesc = "运费")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ShippingCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "运费" )]
        public decimal ShippingCost
        { 
            get{return _ShippingCost;}
            set{
            SetProperty(ref _ShippingCost, value);
                        }
        }
        private decimal _ForeignShipCost = ((0));
        /// <summary>
        /// 运费外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignShipCost", ColDesc = "运费外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ForeignShipCost", DecimalDigits = 4, IsNullable = false, ColumnDescription = "运费外币")]
        public decimal ForeignShipCost
        {
            get { return _ForeignShipCost; }
            set
            {
                SetProperty(ref _ForeignShipCost, value);
            }
        }
        private decimal _TotalTaxAmount;
        /// <summary>
        /// 总税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalTaxAmount",ColDesc = "总税额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalTaxAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "总税额" )]
        public decimal TotalTaxAmount
        { 
            get{return _TotalTaxAmount;}
            set{
            SetProperty(ref _TotalTaxAmount, value);
                        }
        }
        private decimal _ForeignTotalAmount = ((0));
        /// <summary>
        /// 金额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignTotalAmount", ColDesc = "金额外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ForeignTotalAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "金额外币")]
        public decimal ForeignTotalAmount
        {
            get { return _ForeignTotalAmount; }
            set
            {
                SetProperty(ref _ForeignTotalAmount, value);
            }
        }
        private decimal _TotalAmount;
        /// <summary>
        /// 货款金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "货款金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "货款金额" )]
        public decimal TotalAmount
        { 
            get{return _TotalAmount;}
            set{
            SetProperty(ref _TotalAmount, value);
                        }
        }



        private DateTime? _Arrival_date;
        /// <summary>
        /// 到货日期
        /// </summary>
        [AdvQueryAttribute(ColName = "Arrival_date",ColDesc = "到货日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Arrival_date" ,IsNullable = true,ColumnDescription = "到货日期" )]
        public DateTime? Arrival_date
        { 
            get{return _Arrival_date;}
            set{
            SetProperty(ref _Arrival_date, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=1500,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        private bool _IsIncludeTax= false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IsIncludeTax",ColDesc = "含税")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsIncludeTax" ,IsNullable = false,ColumnDescription = "含税" )]
        public bool IsIncludeTax
        { 
            get{return _IsIncludeTax;}
            set{
            SetProperty(ref _IsIncludeTax, value);
                        }
        }

        private int? _KeepAccountsType;
        /// <summary>
        /// 立帐类型
        /// </summary>
        [AdvQueryAttribute(ColName = "KeepAccountsType",ColDesc = "立帐类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "KeepAccountsType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "立帐类型" )]
        public int? KeepAccountsType
        { 
            get{return _KeepAccountsType;}
            set{
            SetProperty(ref _KeepAccountsType, value);
                        }
        }
 

        private decimal _Deposit;
        /// <summary>
        /// 订金
        /// </summary>
        [AdvQueryAttribute(ColName = "Deposit",ColDesc = "订金")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Deposit" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "订金" )]
        public decimal Deposit
        { 
            get{return _Deposit;}
            set{
            SetProperty(ref _Deposit, value);
                        }
        }

        private decimal _ForeignDeposit = ((0));
        /// <summary>
        /// 订金外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignDeposit", ColDesc = "订金外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ForeignDeposit", DecimalDigits = 4, IsNullable = false, ColumnDescription = "订金外币")]
        public decimal ForeignDeposit
        {
            get { return _ForeignDeposit; }
            set
            {
                SetProperty(ref _ForeignDeposit, value);
            }
        }

        private int? _TaxDeductionType;
        /// <summary>
        /// 扣税类型
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxDeductionType",ColDesc = "扣税类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TaxDeductionType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "扣税类型" )]
        public int? TaxDeductionType
        { 
            get{return _TaxDeductionType;}
            set{
            SetProperty(ref _TaxDeductionType, value);
                        }
        }

        private int _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "数据状态" )]
        public int DataStatus
        { 
            get{return _DataStatus;}
            set{
            SetProperty(ref _DataStatus, value);
                        }
        }

        private string _CloseCaseOpinions;
        /// <summary>
        /// 结案意见
        /// </summary>
        [AdvQueryAttribute(ColName = "CloseCaseOpinions",ColDesc = "结案意见")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CloseCaseOpinions" ,Length=200,IsNullable = true,ColumnDescription = "结案意见" )]
        public string CloseCaseOpinions
        { 
            get{return _CloseCaseOpinions;}
            set{
            SetProperty(ref _CloseCaseOpinions, value);
                        }
        }

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=200,IsNullable = true,ColumnDescription = "审批意见" )]
        public string ApprovalOpinions
        { 
            get{return _ApprovalOpinions;}
            set{
            SetProperty(ref _ApprovalOpinions, value);
                        }
        }

        private int? _ApprovalStatus;
        /// <summary>
        /// 审批状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "审批状态")] 
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType ="SByte",  ColumnName = "ApprovalStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "审批状态" )]
        public int? ApprovalStatus
        { 
            get{return _ApprovalStatus;}
            set{
            SetProperty(ref _ApprovalStatus, value);
                        }
        }

        private bool? _ApprovalResults;
        /// <summary>
        /// 审批结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审批结果")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "ApprovalResults" ,IsNullable = true,ColumnDescription = "审批结果" )]
        public bool? ApprovalResults
        { 
            get{return _ApprovalResults;}
            set{
            SetProperty(ref _ApprovalResults, value);
                        }
        }

        private long? _Approver_by;
        /// <summary>
        /// 审批人
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_by",ColDesc = "审批人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Approver_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "审批人" )]
        public long? Approver_by
        { 
            get{return _Approver_by;}
            set{
            SetProperty(ref _Approver_by, value);
                        }
        }

        private DateTime? _Approver_at;
        /// <summary>
        /// 审批时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_at",ColDesc = "审批时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Approver_at" ,IsNullable = true,ColumnDescription = "审批时间" )]
        public DateTime? Approver_at
        { 
            get{return _Approver_at;}
            set{
            SetProperty(ref _Approver_at, value);
                        }
        }

        private int _PrintStatus= ((0));
        /// <summary>
        /// 打印状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintStatus",ColDesc = "打印状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PrintStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "打印状态" )]
        public int PrintStatus
        { 
            get{return _PrintStatus;}
            set{
            SetProperty(ref _PrintStatus, value);
                        }
        }

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = false,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        public bool isdeleted
        { 
            get{return _isdeleted;}
            set{
            SetProperty(ref _isdeleted, value);
                        }
        }

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at
        { 
            get{return _Created_at;}
            set{
            SetProperty(ref _Created_at, value);
                        }
        }

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by
        { 
            get{return _Created_by;}
            set{
            SetProperty(ref _Created_by, value);
                        }
        }

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at
        { 
            get{return _Modified_at;}
            set{
            SetProperty(ref _Modified_at, value);
                        }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by
        { 
            get{return _Modified_by;}
            set{
            SetProperty(ref _Modified_by, value);
                        }
        }

        private long? _RefBillID;
        /// <summary>
        /// 转入单
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBillID",ColDesc = "转入单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RefBillID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "转入单" )]
        public long? RefBillID
        { 
            get{return _RefBillID;}
            set{
            SetProperty(ref _RefBillID, value);
                        }
        }

        private string _RefNO;
        /// <summary>
        /// 引用单据
        /// </summary>
        [AdvQueryAttribute(ColName = "RefNO",ColDesc = "引用单据")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RefNO" ,Length=50,IsNullable = true,ColumnDescription = "引用单据" )]
        public string RefNO
        { 
            get{return _RefNO;}
            set{
            SetProperty(ref _RefNO, value);
                        }
        }

        private int? _RefBizType;
        /// <summary>
        /// 单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBizType",ColDesc = "单据类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "RefBizType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "单据类型" )]
        public int? RefBizType
        { 
            get{return _RefBizType;}
            set{
            SetProperty(ref _RefBizType, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Paytype_ID))]
        public virtual tb_PaymentMethod tb_paymentmethod { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(SOrder_ID))]
        public virtual tb_SaleOrder tb_saleorder { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(PDID))]
        public virtual tb_ProductionDemand tb_productiondemand { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProjectGroup_ID))]
        public virtual tb_ProjectGroup tb_projectgroup { get; set; }

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntry.PurOrder_ID))]
        public virtual List<tb_PurEntry> tb_PurEntries { get; set; }
        //tb_PurEntry.PurOrder_ID)
        //PurOrder_ID.FK_TB_PUREN_REFERENCE_TB_PUROR)
        //tb_PurOrder.PurOrder_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrderDetail.PurOrder_ID))]
        public virtual List<tb_PurOrderDetail> tb_PurOrderDetails { get; set; }
        //tb_PurOrderDetail.PurOrder_ID)
        //PurOrder_ID.FK_PURORDTAIL_REF_PURORDER)
        //tb_PurOrder.PurOrder_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrderRe.PurOrder_ID))]
        public virtual List<tb_PurOrderRe> tb_PurOrderRes { get; set; }
        //tb_PurOrderRe.PurOrder_ID)
        //PurOrder_ID.FK_PURORDERRE_RE_PURORDER)
        //tb_PurOrder.PurOrder_ID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}



 

        public override object Clone()
        {
            tb_PurOrder loctype = (tb_PurOrder)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

