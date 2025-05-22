
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:27
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
    [Description("销售出库单")]
    [SugarTable("tb_SaleOut")]
    public partial class tb_SaleOut : BaseEntity, ICloneable
    {
        public tb_SaleOut()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("销售出库单tb_SaleOut" + "外键ID与对应主主键名称不一致。请修改数据库");
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
                SetProperty(ref _SaleOut_MainID, value);
                base.PrimaryKeyID = _SaleOut_MainID;
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

        private long _CustomerVendor_ID;
        /// <summary>
        /// 客户
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID", ColDesc = "客户")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "CustomerVendor_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "客户")]
        [FKRelationAttribute("tb_CustomerVendor", "CustomerVendor_ID")]
        public long CustomerVendor_ID
        {
            get { return _CustomerVendor_ID; }
            set
            {
                SetProperty(ref _CustomerVendor_ID, value);
            }
        }

        private long? _SOrder_ID;
        /// <summary>
        /// 销售订单
        /// </summary>
        [AdvQueryAttribute(ColName = "SOrder_ID", ColDesc = "销售订单")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "SOrder_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "销售订单")]
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

        private long _Paytype_ID;
        /// <summary>
        /// 付款方式
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_ID", ColDesc = "付款方式")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Paytype_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "付款方式")]
        [FKRelationAttribute("tb_PaymentMethod", "Paytype_ID")]
        public long Paytype_ID
        {
            get { return _Paytype_ID; }
            set
            {
                SetProperty(ref _Paytype_ID, value);
            }
        }

        private decimal _ShipCost = ((0));
        /// <summary>
        /// 运费本币
        /// </summary>
        [AdvQueryAttribute(ColName = "ShipCost", ColDesc = "运费本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ShipCost", DecimalDigits = 4, IsNullable = false, ColumnDescription = "运费本币")]
        public decimal ShipCost
        {
            get { return _ShipCost; }
            set
            {
                SetProperty(ref _ShipCost, value);
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


        private decimal _FreightCost = ((0));
        /// <summary>
        /// 运费成本本币
        /// </summary>
        [AdvQueryAttribute(ColName = "FreightCost", ColDesc = "运费成本本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "FreightCost", DecimalDigits = 4, IsNullable = false, ColumnDescription = "运费成本本币")]
        public decimal FreightCost
        {
            get { return _FreightCost; }
            set
            {
                SetProperty(ref _FreightCost, value);
            }
        }

        private decimal _ForeignFreightCost = ((0));
        /// <summary>
        /// 运费成本外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignFreightCost", ColDesc = "运费成本外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ForeignFreightCost", DecimalDigits = 4, IsNullable = false, ColumnDescription = "运费成本外币")]
        public decimal ForeignFreightCost
        {
            get { return _ForeignFreightCost; }
            set
            {
                SetProperty(ref _ForeignFreightCost, value);
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

        private long? _Currency_ID;
        /// <summary>
        /// 币别
        /// </summary>
        [AdvQueryAttribute(ColName = "Currency_ID", ColDesc = "币别")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Currency_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "币别")]
        [FKRelationAttribute("tb_Currency", "Currency_ID")]
        public long? Currency_ID
        {
            get { return _Currency_ID; }
            set
            {
                SetProperty(ref _Currency_ID, value);
            }
        }

        private decimal _ExchangeRate = 1;
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


        private decimal _TotalAmount = ((0));
        /// <summary>
        /// 总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount", ColDesc = "总金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "总金额")]
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
        /// 出库日期-以这个日期为标准来计算回款时间 如果是账期时
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


        private DateTime? _DueDate;
        /// <summary>
        /// 账期届满日
        /// </summary>
        [AdvQueryAttribute(ColName = "DueDate", ColDesc = "账期届满日")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "DueDate", IsNullable = true, ColumnDescription = "账期届满日")]
        public DateTime? DueDate
        {
            get { return _DueDate; }
            set
            {
                SetProperty(ref _DueDate, value);
            }
        }


        private string _ShippingAddress;
        /// <summary>
        /// 收货地址
        /// </summary>
        [AdvQueryAttribute(ColName = "ShippingAddress", ColDesc = "收货地址")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ShippingAddress", Length = 500, IsNullable = true, ColumnDescription = "收货地址")]
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
        private bool _IsCustomizedOrder;
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
        private string _TrackNo;
        /// <summary>
        /// 物流单号
        /// </summary>
        [AdvQueryAttribute(ColName = "TrackNo", ColDesc = "物流单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "TrackNo", Length = 50, IsNullable = true, ColumnDescription = "物流单号")]
        public string TrackNo
        {
            get { return _TrackNo; }
            set
            {
                SetProperty(ref _TrackNo, value);
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
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalCost", DecimalDigits = 4, IsNullable = false, ColumnDescription = "总成本")]
        public decimal TotalCost
        {
            get { return _TotalCost; }
            set
            {
                SetProperty(ref _TotalCost, value);
            }
        }

       

        private decimal _TotalTaxAmount;
        /// <summary>
        /// 总税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalTaxAmount", ColDesc = "总税额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalTaxAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "总税额")]
        public decimal TotalTaxAmount
        {
            get { return _TotalTaxAmount; }
            set
            {
                SetProperty(ref _TotalTaxAmount, value);
            }
        }

        private decimal _ForeignTotalAmount = ((0));
        /// <summary>
        /// 总金额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignTotalAmount", ColDesc = "总金额外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ForeignTotalAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "总金额外币")]
        public decimal ForeignTotalAmount
        {
            get { return _ForeignTotalAmount; }
            set
            {
                SetProperty(ref _ForeignTotalAmount, value);
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

 

        private bool _ReplaceOut = false;
        /// <summary>
        /// 替代出库
        /// </summary>
        [AdvQueryAttribute(ColName = "ReplaceOut", ColDesc = "替代出库")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "ReplaceOut", IsNullable = false, ColumnDescription = "替代出库")]
        public bool ReplaceOut
        {
            get { return _ReplaceOut; }
            set
            {
                SetProperty(ref _ReplaceOut, value);
            }
        }


        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(SOrder_ID))]
        public virtual tb_SaleOrder tb_saleorder { get; set; }

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
        [Navigate(NavigateType.OneToOne, nameof(ProjectGroup_ID))]
        public virtual tb_ProjectGroup tb_projectgroup { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutRe.SaleOut_MainID))]
        public virtual List<tb_SaleOutRe> tb_SaleOutRes { get; set; }
        //tb_SaleOutRe.SaleOut_MainID)
        //SaleOut_MainID.FK_SORE_RE_SALEOUT)
        //tb_SaleOut.SaleOut_MainID)

        //[Browsable(false)]打印报表时的数据源会不显示
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



 

        public override object Clone()
        {
            tb_SaleOut loctype = (tb_SaleOut)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

