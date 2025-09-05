
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:28
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
    /// 销售出库退回单
    /// </summary>
    [Serializable()]
    [Description("销售出库退回单")]
    [SugarTable("tb_SaleOutRe")]
    public partial class tb_SaleOutRe : BaseEntity, ICloneable
    {
        public tb_SaleOutRe()
        {

            if (!PK_FK_ID_Check())
            {
                throw new Exception("销售出库退回单tb_SaleOutRe" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _SaleOutRe_ID;
        /// <summary>
        /// 销售退回单
        /// </summary>

        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "SaleOutRe_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "销售退回单", IsPrimaryKey = true)]
        public long SaleOutRe_ID
        {
            get { return _SaleOutRe_ID; }
            set
            {
                SetProperty(ref _SaleOutRe_ID, value);
                base.PrimaryKeyID = _SaleOutRe_ID;
            }
        }

        private string _ReturnNo;
        /// <summary>
        /// 退回单号
        /// </summary>
        [AdvQueryAttribute(ColName = "ReturnNo", ColDesc = "退回单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ReturnNo", Length = 50, IsNullable = true, ColumnDescription = "退回单号")]
        public string ReturnNo
        {
            get { return _ReturnNo; }
            set
            {
                SetProperty(ref _ReturnNo, value);
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
        /// 退款状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PayStatus", ColDesc = "退款状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "PayStatus", DecimalDigits = 0, IsNullable = true, ColumnDescription = "退款状态")]
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
        /// 退款类型
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_ID", ColDesc = "退款类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Paytype_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "退款类型")]
        [FKRelationAttribute("tb_PaymentMethod", "Paytype_ID")]
        public long? Paytype_ID
        {
            get { return _Paytype_ID; }
            set
            {
                SetProperty(ref _Paytype_ID, value);
            }
        }
        private int? _RefundStatus;
        /// <summary>
        /// 退货退款状态
        /// </summary>
        [AdvQueryAttribute(ColName = "RefundStatus", ColDesc = "退货退款状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "RefundStatus", DecimalDigits = 0, IsNullable = true, ColumnDescription = "退货退款状态")]
        public int? RefundStatus
        {
            get { return _RefundStatus; }
            set
            {
                SetProperty(ref _RefundStatus, value);
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
        /// 退货客户
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID", ColDesc = "退货客户")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "CustomerVendor_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "退货客户")]
        [FKRelationAttribute("tb_CustomerVendor", "CustomerVendor_ID")]
        public long? CustomerVendor_ID
        {
            get { return _CustomerVendor_ID; }
            set
            {
                SetProperty(ref _CustomerVendor_ID, value);
            }
        }

        private long? _SaleOut_MainID;
        /// <summary>
        /// 销售出库单
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleOut_MainID", ColDesc = "销售出库单")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "SaleOut_MainID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "销售出库单")]
        [FKRelationAttribute("tb_SaleOut", "SaleOut_MainID")]
        public long? SaleOut_MainID
        {
            get { return _SaleOut_MainID; }
            set
            {
                SetProperty(ref _SaleOut_MainID, value);
            }
        }

        private string _SaleOut_NO;
        /// <summary>
        /// 销售出库单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleOut_NO", ColDesc = "销售出库单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "SaleOut_NO", Length = 50, IsNullable = true, ColumnDescription = "销售出库单号")]
        public string SaleOut_NO
        {
            get { return _SaleOut_NO; }
            set
            {
                SetProperty(ref _SaleOut_NO, value);
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

        private int _TotalQty = ((0));
        /// <summary>
        /// 退回总数量
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalQty", ColDesc = "退回总数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "TotalQty", DecimalDigits = 0, IsNullable = false, ColumnDescription = "退回总数量")]
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
        /// 退款金额合计
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount", ColDesc = "退款金额合计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "退款金额合计")]
        public decimal TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                SetProperty(ref _TotalAmount, value);
            }
        }

        private DateTime? _ReturnDate;
        /// <summary>
        /// 退货日期
        /// 因为建退货时。可能并不知道什么时候退回来
        /// </summary>
        [AdvQueryAttribute(ColName = "ReturnDate", ColDesc = "退货日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "ReturnDate", IsNullable = true, ColumnDescription = "退货日期")]
        public DateTime? ReturnDate
        {
            get { return _ReturnDate; }
            set
            {
                SetProperty(ref _ReturnDate, value);
            }
        }

        private decimal _FreightIncome = ((0));
        /// <summary>
        /// 需退运费
        /// </summary>
        [AdvQueryAttribute(ColName = "FreightIncome", ColDesc = "需退运费")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "FreightIncome", DecimalDigits = 4, IsNullable = false, ColumnDescription = "需退运费")]
        public decimal FreightIncome
        {
            get { return _FreightIncome; }
            set
            {
                SetProperty(ref _FreightIncome, value);
            }
        }

        private decimal _ForeignFreightIncome = ((0));
        /// <summary>
        /// 需退运费外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignFreightIncome", ColDesc = "需退运费外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ForeignFreightIncome", DecimalDigits = 4, IsNullable = false, ColumnDescription = "需退运费外币")]
        public decimal ForeignFreightIncome
        {
            get { return _ForeignFreightIncome; }
            set
            {
                SetProperty(ref _ForeignFreightIncome, value);
            }
        }
        private decimal _TotalCommissionAmount = ((0));
        /// <summary>
        /// 返还佣金金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalCommissionAmount", ColDesc = "返还佣金金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalCommissionAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "返还佣金金额")]
        public decimal TotalCommissionAmount
        {
            get { return _TotalCommissionAmount; }
            set
            {
                SetProperty(ref _TotalCommissionAmount, value);
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

        private string _ReturnReason;
        /// <summary>
        /// 退货原因
        /// </summary>
        [AdvQueryAttribute(ColName = "ReturnReason", ColDesc = "退货原因")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ReturnReason", Length = 1000, IsNullable = true, ColumnDescription = "退货原因")]
        public string ReturnReason
        {
            get { return _ReturnReason; }
            set
            {
                SetProperty(ref _ReturnReason, value);
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

        private bool _RefundOnly;
        /// <summary>
        /// 仅退款
        /// </summary>
        [AdvQueryAttribute(ColName = "RefundOnly", ColDesc = "仅退款")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "RefundOnly", IsNullable = false, ColumnDescription = "仅退款")]
        public bool RefundOnly
        {
            get { return _RefundOnly; }
            set
            {
                SetProperty(ref _RefundOnly, value);
            }
        }

        private bool? _IsIncludeTax = false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IsIncludeTax", ColDesc = "含税")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "IsIncludeTax", IsNullable = true, ColumnDescription = "含税")]
        public bool? IsIncludeTax
        {
            get { return _IsIncludeTax; }
            set
            {
                SetProperty(ref _IsIncludeTax, value);
            }
        }


        private bool? _OfflineRefund = false;
        /// <summary>
        /// 线下退款
        /// </summary>
        [AdvQueryAttribute(ColName = "OfflineRefund", ColDesc = "线下退款")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "OfflineRefund", IsNullable = true, ColumnDescription = "线下退款")]
        public bool? OfflineRefund
        {
            get { return _OfflineRefund; }
            set
            {
                SetProperty(ref _OfflineRefund, value);
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

        private bool? _GenerateVouchers = false;
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

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Paytype_ID))]
        public virtual tb_PaymentMethod tb_paymentmethod { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProjectGroup_ID))]
        public virtual tb_ProjectGroup tb_projectgroup { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(SaleOut_MainID))]
        public virtual tb_SaleOut tb_saleout { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutReDetail.SaleOutRe_ID))]
        public virtual List<tb_SaleOutReDetail> tb_SaleOutReDetails { get; set; }
        //tb_SaleOutReDetail.SaleOutRe_ID)
        //SaleOutRe_ID.FK_SALEOUTREDETAIL_RE_SALEOUTRE)
        //tb_SaleOutRe.SaleOutRe_ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutReRefurbishedMaterialsDetail.SaleOutRe_ID))]
        public virtual List<tb_SaleOutReRefurbishedMaterialsDetail> tb_SaleOutReRefurbishedMaterialsDetails { get; set; }
        //tb_SaleOutReRefurbishedMaterialsDetail.SaleOutRe_ID)
        //SaleOutRe_ID.FK_TB_SALEO_REFERENCE_TB_SALEO)
        //tb_SaleOutRe.SaleOutRe_ID)


        #endregion




        //如果为false,则不可以。
        private bool PK_FK_ID_Check()
        {
            bool rs = true;
            return rs;
        }







        public override object Clone()
        {
            tb_SaleOutRe loctype = (tb_SaleOutRe)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

