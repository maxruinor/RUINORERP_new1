
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:29
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 销售出库单
    /// </summary>
    [Serializable()]
    [Description("tb_SaleOut")]
    [SugarTable("tb_SaleOut")]
    public partial class tb_SaleOut : BaseEntity, ICloneable
    {
        public tb_SaleOut()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_SaleOut" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _SaleOut_MainID;
        /// <summary>
        /// 
        /// </summary>

        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "SaleOut_MainID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "", IsPrimaryKey = true)]
        public long SaleOut_MainID
        {
            get { return _SaleOut_MainID; }
            set
            {
                base.PrimaryKeyID = _SaleOut_MainID;
                SetProperty(ref _SaleOut_MainID, value);
            }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 业务员
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID", ColDesc = "业务员")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Employee_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "业务员")]
        [FKRelationAttribute("tb_Employee", "Employee_ID")]
        public long? Employee_ID
        {
            get { return _Employee_ID; }
            set
            {
                SetProperty(ref _Employee_ID, value);
            }
        }

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 客户
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID", ColDesc = "客户")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "CustomerVendor_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "客户")]
        [FKRelationAttribute("tb_CustomerVendor", "CustomerVendor_ID")]
        public long? CustomerVendor_ID
        {
            get { return _CustomerVendor_ID; }
            set
            {
                SetProperty(ref _CustomerVendor_ID, value);
            }
        }

        private long? _SOrder_ID;
        /// <summary>
        /// 引用订单
        /// </summary>
        [AdvQueryAttribute(ColName = "SOrder_ID", ColDesc = "引用订单")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "SOrder_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "引用订单")]
        [FKRelationAttribute("tb_SaleOrder", "SOrder_ID")]
        public long? SOrder_ID
        {
            get { return _SOrder_ID; }
            set
            {
                SetProperty(ref _SOrder_ID, value);
            }
        }

        private string _SaleOrderNo;
        /// <summary>
        /// 销售订单编号
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleOrderNo", ColDesc = "销售订单编号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "SaleOrderNo", Length = 50, IsNullable = true, ColumnDescription = "销售订单编号")]
        public string SaleOrderNo
        {
            get { return _SaleOrderNo; }
            set
            {
                SetProperty(ref _SaleOrderNo, value);
            }
        }

        private string _SaleOutNo;
        /// <summary>
        /// 出库单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleOutNo", ColDesc = "出库单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "SaleOutNo", Length = 50, IsNullable = false, ColumnDescription = "出库单号")]
        public string SaleOutNo
        {
            get { return _SaleOutNo; }
            set
            {
                SetProperty(ref _SaleOutNo, value);
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

        private int? _PayStatus;
        /// <summary>
        /// 付款状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PayStatus", ColDesc = "付款状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "PayStatus", DecimalDigits = 0, IsNullable = true, ColumnDescription = "付款状态")]
        public int? PayStatus
        {
            get { return _PayStatus; }
            set
            {
                SetProperty(ref _PayStatus, value);
            }
        }

        private long? _Paytype_ID;
        /// <summary>
        /// 付款类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_ID", ColDesc = "付款类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Paytype_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "付款类型")]
        [FKRelationAttribute("tb_PaymentMethod", "Paytype_ID")]
        public long? Paytype_ID
        {
            get { return _Paytype_ID; }
            set
            {
                SetProperty(ref _Paytype_ID, value);
            }
        }

        private decimal _ShipCost = ((0));
        /// <summary>
        /// 运费
        /// </summary>
        [AdvQueryAttribute(ColName = "ShipCost", ColDesc = "运费")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ShipCost", DecimalDigits = 6, IsNullable = false, ColumnDescription = "运费")]
        public decimal ShipCost
        {
            get { return _ShipCost; }
            set
            {
                SetProperty(ref _ShipCost, value);
            }
        }

        private int _TotalQty = ((0));
        /// <summary>
        /// 总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty", ColDesc = "总数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "TotalQty", DecimalDigits = 0, IsNullable = false, ColumnDescription = "总数量")]
        public int TotalQty
        {
            get { return _TotalQty; }
            set
            {
                SetProperty(ref _TotalQty, value);
            }
        }

        private decimal _TotalAmount = ((0));
        /// <summary>
        /// 总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount", ColDesc = "总金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalAmount", DecimalDigits = 6, IsNullable = false, ColumnDescription = "总金额")]
        public decimal TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                SetProperty(ref _TotalAmount, value);
            }
        }

        private DateTime _OutDate;
        /// <summary>
        /// 出库日期
        /// </summary>
        [AdvQueryAttribute(ColName = "OutDate", ColDesc = "出库日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "OutDate", IsNullable = false, ColumnDescription = "出库日期")]
        public DateTime OutDate
        {
            get { return _OutDate; }
            set
            {
                SetProperty(ref _OutDate, value);
            }
        }

        private DateTime? _DeliveryDate;
        /// <summary>
        /// 发货日期
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveryDate", ColDesc = "发货日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "DeliveryDate", IsNullable = true, ColumnDescription = "发货日期")]
        public DateTime? DeliveryDate
        {
            get { return _DeliveryDate; }
            set
            {
                SetProperty(ref _DeliveryDate, value);
            }
        }

        private string _ShippingAddress;
        /// <summary>
        /// 发货地址
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingAddress", ColDesc = "发货地址")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ShippingAddress", Length = 500, IsNullable = true, ColumnDescription = "发货地址")]
        public string ShippingAddress
        {
            get { return _ShippingAddress; }
            set
            {
                SetProperty(ref _ShippingAddress, value);
            }
        }

        private string _ShippingWay;
        /// <summary>
        /// 发货方式
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingWay", ColDesc = "发货方式")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ShippingWay", Length = 50, IsNullable = true, ColumnDescription = "发货方式")]
        public string ShippingWay
        {
            get { return _ShippingWay; }
            set
            {
                SetProperty(ref _ShippingWay, value);
            }
        }

        private string _PlatformOrderNo;
        /// <summary>
        /// 平台单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PlatformOrderNo", ColDesc = "平台单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "PlatformOrderNo", Length = 100, IsNullable = true, ColumnDescription = "平台单号")]
        public string PlatformOrderNo
        {
            get { return _PlatformOrderNo; }
            set
            {
                SetProperty(ref _PlatformOrderNo, value);
            }
        }

        private bool _IsFromPlatform;
        /// <summary>
        /// 平台单
        /// </summary>
        [AdvQueryAttribute(ColName = "IsFromPlatform", ColDesc = "平台单")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "IsFromPlatform", IsNullable = false, ColumnDescription = "平台单")]
        public bool IsFromPlatform
        {
            get { return _IsFromPlatform; }
            set
            {
                SetProperty(ref _IsFromPlatform, value);
            }
        }

        private string _TrackNo;
        /// <summary>
        /// 物流单号
        /// </summary>
        [AdvQueryAttribute(ColName = "TrackNo", ColDesc = "物流单号")]
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType = "String", ColumnName = "TrackNo", Length = 50, IsNullable = true, ColumnDescription = "物流单号")]
        public string TrackNo
        {
            get { return _TrackNo; }
            set
            {
                SetProperty(ref _TrackNo, value);
            }
        }

        private decimal? _CollectedMoney = ((0));
        /// <summary>
        /// 实收金额
        /// </summary>
        [AdvQueryAttribute(ColName = "CollectedMoney", ColDesc = "实收金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "CollectedMoney", DecimalDigits = 6, IsNullable = true, ColumnDescription = "实收金额")]
        public decimal? CollectedMoney
        {
            get { return _CollectedMoney; }
            set
            {
                SetProperty(ref _CollectedMoney, value);
            }
        }

        private bool _isdeleted = false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted", ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "isdeleted", IsNullable = false, ColumnDescription = "逻辑删除")]
        [Browsable(false)]
        public bool isdeleted
        {
            get { return _isdeleted; }
            set
            {
                SetProperty(ref _isdeleted, value);
            }
        }

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at", ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Created_at", IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? Created_at
        {
            get { return _Created_at; }
            set
            {
                SetProperty(ref _Created_at, value);
            }
        }

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by", ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Created_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "创建人")]
        public long? Created_by
        {
            get { return _Created_by; }
            set
            {
                SetProperty(ref _Created_by, value);
            }
        }

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at", ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Modified_at", IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? Modified_at
        {
            get { return _Modified_at; }
            set
            {
                SetProperty(ref _Modified_at, value);
            }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by", ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Modified_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "修改人")]
        public long? Modified_by
        {
            get { return _Modified_by; }
            set
            {
                SetProperty(ref _Modified_by, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes", ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Notes", Length = 1500, IsNullable = true, ColumnDescription = "备注")]
        public string Notes
        {
            get { return _Notes; }
            set
            {
                SetProperty(ref _Notes, value);
            }
        }

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions", ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ApprovalOpinions", Length = 200, IsNullable = true, ColumnDescription = "审批意见")]
        public string ApprovalOpinions
        {
            get { return _ApprovalOpinions; }
            set
            {
                SetProperty(ref _ApprovalOpinions, value);
            }
        }

        private long? _Approver_by;
        /// <summary>
        /// 审批人
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_by", ColDesc = "审批人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Approver_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "审批人")]
        public long? Approver_by
        {
            get { return _Approver_by; }
            set
            {
                SetProperty(ref _Approver_by, value);
            }
        }

        private DateTime? _Approver_at;
        /// <summary>
        /// 审批时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_at", ColDesc = "审批时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Approver_at", IsNullable = true, ColumnDescription = "审批时间")]
        public DateTime? Approver_at
        {
            get { return _Approver_at; }
            set
            {
                SetProperty(ref _Approver_at, value);
            }
        }

        private int? _ApprovalStatus = ((0));
        /// <summary>
        /// 审批状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalStatus", ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType = "SByte", ColumnName = "ApprovalStatus", DecimalDigits = 0, IsNullable = true, ColumnDescription = "审批状态")]
        public int? ApprovalStatus
        {
            get { return _ApprovalStatus; }
            set
            {
                SetProperty(ref _ApprovalStatus, value);
            }
        }

        private bool? _ApprovalResults;
        /// <summary>
        /// 审批结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults", ColDesc = "审批结果")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "ApprovalResults", IsNullable = true, ColumnDescription = "审批结果")]
        public bool? ApprovalResults
        {
            get { return _ApprovalResults; }
            set
            {
                SetProperty(ref _ApprovalResults, value);
            }
        }

        private int? _KeepAccountsType;
        /// <summary>
        /// 立帐类型
        /// </summary>
        [AdvQueryAttribute(ColName = "KeepAccountsType", ColDesc = "立帐类型")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "KeepAccountsType", DecimalDigits = 0, IsNullable = true, ColumnDescription = "立帐类型")]
        public int? KeepAccountsType
        {
            get { return _KeepAccountsType; }
            set
            {
                SetProperty(ref _KeepAccountsType, value);
            }
        }

        private decimal? _Deposit;
        /// <summary>
        /// 订金
        /// </summary>
        [AdvQueryAttribute(ColName = "Deposit", ColDesc = "订金")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "Deposit", DecimalDigits = 6, IsNullable = true, ColumnDescription = "订金")]
        public decimal? Deposit
        {
            get { return _Deposit; }
            set
            {
                SetProperty(ref _Deposit, value);
            }
        }

        private int? _TaxDeductionType;
        /// <summary>
        /// 扣税类型
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxDeductionType", ColDesc = "扣税类型")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "TaxDeductionType", DecimalDigits = 0, IsNullable = true, ColumnDescription = "扣税类型")]
        public int? TaxDeductionType
        {
            get { return _TaxDeductionType; }
            set
            {
                SetProperty(ref _TaxDeductionType, value);
            }
        }

        private int _PrintStatus = ((0));
        /// <summary>
        /// 打印状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintStatus", ColDesc = "打印状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "PrintStatus", DecimalDigits = 0, IsNullable = false, ColumnDescription = "打印状态")]
        public int PrintStatus
        {
            get { return _PrintStatus; }
            set
            {
                SetProperty(ref _PrintStatus, value);
            }
        }

        private int _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus", ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "DataStatus", DecimalDigits = 0, IsNullable = false, ColumnDescription = "数据状态")]
        public int DataStatus
        {
            get { return _DataStatus; }
            set
            {
                SetProperty(ref _DataStatus, value);
            }
        }

        private decimal _TotalCost;
        /// <summary>
        /// 总成本
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalCost", ColDesc = "总成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalCost", DecimalDigits = 6, IsNullable = false, ColumnDescription = "总成本")]
        public decimal TotalCost
        {
            get { return _TotalCost; }
            set
            {
                SetProperty(ref _TotalCost, value);
            }
        }

        private decimal? _TaxRate;
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate", ColDesc = "税率")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType = "Decimal", ColumnName = "TaxRate", DecimalDigits = 3, IsNullable = true, ColumnDescription = "税率")]
        public decimal? TaxRate
        {
            get { return _TaxRate; }
            set
            {
                SetProperty(ref _TaxRate, value);
            }
        }

        private decimal _TotalTaxAmount;
        /// <summary>
        /// 总税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalTaxAmount", ColDesc = "总税额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalTaxAmount", DecimalDigits = 6, IsNullable = false, ColumnDescription = "总税额")]
        public decimal TotalTaxAmount
        {
            get { return _TotalTaxAmount; }
            set
            {
                SetProperty(ref _TotalTaxAmount, value);
            }
        }

        private decimal _TotalUntaxedAmount;
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalUntaxedAmount", ColDesc = "未税本位币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalUntaxedAmount", DecimalDigits = 6, IsNullable = false, ColumnDescription = "未税本位币")]
        public decimal TotalUntaxedAmount
        {
            get { return _TotalUntaxedAmount; }
            set
            {
                SetProperty(ref _TotalUntaxedAmount, value);
            }
        }

        private bool? _GenerateVouchers;
        /// <summary>
        /// 生成凭证
        /// </summary>
        [AdvQueryAttribute(ColName = "GenerateVouchers", ColDesc = "生成凭证")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "GenerateVouchers", IsNullable = true, ColumnDescription = "生成凭证")]
        public bool? GenerateVouchers
        {
            get { return _GenerateVouchers; }
            set
            {
                SetProperty(ref _GenerateVouchers, value);
            }
        }

        private decimal? _DiscountAmount;
        /// <summary>
        /// 优惠金额
        /// </summary>
        [AdvQueryAttribute(ColName = "DiscountAmount", ColDesc = "优惠金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "DiscountAmount", DecimalDigits = 6, IsNullable = true, ColumnDescription = "优惠金额")]
        public decimal? DiscountAmount
        {
            get { return _DiscountAmount; }
            set
            {
                SetProperty(ref _DiscountAmount, value);
            }
        }

        private decimal _PrePayMoney;
        /// <summary>
        /// 预收款
        /// </summary>
        [AdvQueryAttribute(ColName = "PrePayMoney", ColDesc = "预收款")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "PrePayMoney", DecimalDigits = 6, IsNullable = false, ColumnDescription = "预收款")]
        public decimal PrePayMoney
        {
            get { return _PrePayMoney; }
            set
            {
                SetProperty(ref _PrePayMoney, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(SOrder_ID))]
        public virtual tb_SaleOrder tb_saleorder { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Paytype_ID))]
        public virtual tb_PaymentMethod tb_paymentmethod { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutRe.SaleOut_MainID))]
        public virtual List<tb_SaleOutRe> tb_SaleOutRes { get; set; }
        //tb_SaleOutRe.SaleOut_MainID)
        //SaleOut_MainID.FK_SORE_RE_SALEOUT)
        //tb_SaleOut.SaleOut_MainID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutDetail.SaleOut_MainID))]
        public virtual List<tb_SaleOutDetail> tb_SaleOutDetails { get; set; }
        //tb_SaleOutDetail.SaleOut_MainID)
        //SaleOut_MainID.FK_SALEOUTDETAIL_REF_SALEOUT)
        //tb_SaleOut.SaleOut_MainID)


        #endregion




        //如果为false,则不可以。
        private bool PK_FK_ID_Check()
        {
            bool rs = true;
            return rs;
        }






        #region 字段描述对应列表
        private ConcurrentDictionary<string, string> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_SaleOut);

                    foreach (PropertyInfo field in type.GetProperties())
                    {
                        foreach (Attribute attr in field.GetCustomAttributes(true))
                        {
                            entityAttr = attr as SugarColumn;
                            if (null != entityAttr)
                            {
                                if (entityAttr.ColumnDescription == null)
                                {
                                    continue;
                                }
                                if (entityAttr.IsIdentity)
                                {
                                    continue;
                                }
                                if (entityAttr.IsPrimaryKey)
                                {
                                    continue;
                                }
                                if (entityAttr.ColumnDescription.Trim().Length > 0)
                                {
                                    fieldNameList.TryAdd(field.Name, entityAttr.ColumnDescription);
                                }
                            }
                        }
                    }
                }

                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }
        #endregion


        public override object Clone()
        {
            tb_SaleOut loctype = (tb_SaleOut)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

