
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:53
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
    /// 对账单
    /// </summary>
    [Serializable()]
    [Description("对账单")]
    [SugarTable("tb_FM_Statement")]
    public partial class tb_FM_Statement : BaseEntity, ICloneable
    {
        public tb_FM_Statement()
        {

            if (!PK_FK_ID_Check())
            {
                throw new Exception("对账单tb_FM_Statement" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _StatementId;
        /// <summary>
        /// 对账单
        /// </summary>

        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "StatementId", DecimalDigits = 0, IsNullable = false, ColumnDescription = "对账单", IsPrimaryKey = true)]
        public long StatementId
        {
            get { return _StatementId; }
            set
            {
                SetProperty(ref _StatementId, value);
                base.PrimaryKeyID = _StatementId;
            }
        }

        private string _StatementNo;
        /// <summary>
        /// 对账单号
        /// </summary>
        [AdvQueryAttribute(ColName = "StatementNo", ColDesc = "对账单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "StatementNo", Length = 30, IsNullable = true, ColumnDescription = "对账单号")]
        public string StatementNo
        {
            get { return _StatementNo; }
            set
            {
                SetProperty(ref _StatementNo, value);
            }
        }

        private long _CustomerVendor_ID;
        /// <summary>
        /// 往来单位
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID", ColDesc = "往来单位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "CustomerVendor_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "往来单位")]
        [FKRelationAttribute("tb_CustomerVendor", "CustomerVendor_ID")]
        public long CustomerVendor_ID
        {
            get { return _CustomerVendor_ID; }
            set
            {
                SetProperty(ref _CustomerVendor_ID, value);
            }
        }

        private string _ARAPNos;
        /// <summary>
        /// 应收付单号
        /// </summary>
        [AdvQueryAttribute(ColName = "ARAPNos", ColDesc = "应收付单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ARAPNos", Length = 1000, IsNullable = true, ColumnDescription = "应收付单号")]
        public string ARAPNos
        {
            get { return _ARAPNos; }
            set
            {
                SetProperty(ref _ARAPNos, value);
            }
        }

        private long? _Account_id;
        /// <summary>
        /// 公司账户
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_id", ColDesc = "公司账户")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Account_id", DecimalDigits = 0, IsNullable = true, ColumnDescription = "公司账户")]
        [FKRelationAttribute("tb_FM_Account", "Account_id")]
        public long? Account_id
        {
            get { return _Account_id; }
            set
            {
                SetProperty(ref _Account_id, value);
            }
        }

        private long? _PayeeInfoID;
        /// <summary>
        /// 收款信息
        /// </summary>
        [AdvQueryAttribute(ColName = "PayeeInfoID", ColDesc = "收款信息")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "PayeeInfoID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "收款信息")]
        [FKRelationAttribute("tb_FM_PayeeInfo", "PayeeInfoID")]
        public long? PayeeInfoID
        {
            get { return _PayeeInfoID; }
            set
            {
                SetProperty(ref _PayeeInfoID, value);
            }
        }

        private string _PayeeAccountNo;
        /// <summary>
        /// 收款账号
        /// </summary>
        [AdvQueryAttribute(ColName = "PayeeAccountNo", ColDesc = "收款账号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "PayeeAccountNo", Length = 100, IsNullable = true, ColumnDescription = "收款账号")]
        public string PayeeAccountNo
        {
            get { return _PayeeAccountNo; }
            set
            {
                SetProperty(ref _PayeeAccountNo, value);
            }
        }

        private int _StatementType = ((1));
        /// <summary>
        /// 对账类型
        /// </summary>
        [AdvQueryAttribute(ColName = "StatementType", ColDesc = "对账类型")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "StatementType", DecimalDigits = 0, IsNullable = false, ColumnDescription = "对账类型")]
        public int StatementType
        {
            get { return _StatementType; }
            set
            {
                SetProperty(ref _StatementType, value);
            }
        }

        private int _ReceivePaymentType;
        /// <summary>
        /// 收付类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ReceivePaymentType", ColDesc = "收付类型")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "ReceivePaymentType", DecimalDigits = 0, IsNullable = false, ColumnDescription = "收付类型")]
        public int ReceivePaymentType
        {
            get { return _ReceivePaymentType; }
            set
            {
                SetProperty(ref _ReceivePaymentType, value);
            }
        }

        private DateTime _StartDate;
        /// <summary>
        /// 对账周期起
        /// </summary>
        [AdvQueryAttribute(ColName = "StartDate", ColDesc = "对账周期起")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "StartDate", IsNullable = false, ColumnDescription = "对账周期起")]
        public DateTime StartDate
        {
            get { return _StartDate; }
            set
            {
                SetProperty(ref _StartDate, value);
            }
        }

        private DateTime _EndDate;
        /// <summary>
        /// 对账周期止
        /// </summary>
        [AdvQueryAttribute(ColName = "EndDate", ColDesc = "对账周期止")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "EndDate", IsNullable = false, ColumnDescription = "对账周期止")]
        public DateTime EndDate
        {
            get { return _EndDate; }
            set
            {
                SetProperty(ref _EndDate, value);
            }
        }

        private decimal _OpeningBalanceForeignAmount = ((0));
        /// <summary>
        /// 期初余额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "OpeningBalanceForeignAmount", ColDesc = "期初余额外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "OpeningBalanceForeignAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "期初余额外币")]
        public decimal OpeningBalanceForeignAmount
        {
            get { return _OpeningBalanceForeignAmount; }
            set
            {
                SetProperty(ref _OpeningBalanceForeignAmount, value);
            }
        }

        private decimal _OpeningBalanceLocalAmount = ((0));
        /// <summary>
        /// 期初余额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "OpeningBalanceLocalAmount", ColDesc = "期初余额本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "OpeningBalanceLocalAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "期初余额本币")]
        public decimal OpeningBalanceLocalAmount
        {
            get { return _OpeningBalanceLocalAmount; }
            set
            {
                SetProperty(ref _OpeningBalanceLocalAmount, value);
            }
        }

        private decimal _TotalReceivableForeignAmount = ((0));
        /// <summary>
        /// 期间应收外币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalReceivableForeignAmount", ColDesc = "期间应收外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalReceivableForeignAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "期间应收外币")]
        public decimal TotalReceivableForeignAmount
        {
            get { return _TotalReceivableForeignAmount; }
            set
            {
                SetProperty(ref _TotalReceivableForeignAmount, value);
            }
        }

        private decimal _TotalReceivableLocalAmount = ((0));
        /// <summary>
        /// 期间应收本币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalReceivableLocalAmount", ColDesc = "期间应收本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalReceivableLocalAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "期间应收本币")]
        public decimal TotalReceivableLocalAmount
        {
            get { return _TotalReceivableLocalAmount; }
            set
            {
                SetProperty(ref _TotalReceivableLocalAmount, value);
            }
        }

        private decimal _TotalPayableForeignAmount = ((0));
        /// <summary>
        /// 期间应付外币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalPayableForeignAmount", ColDesc = "期间应付外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalPayableForeignAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "期间应付外币")]
        public decimal TotalPayableForeignAmount
        {
            get { return _TotalPayableForeignAmount; }
            set
            {
                SetProperty(ref _TotalPayableForeignAmount, value);
            }
        }

        private decimal _TotalPayableLocalAmount = ((0));
        /// <summary>
        /// 期间应付本币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalPayableLocalAmount", ColDesc = "期间应付本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalPayableLocalAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "期间应付本币")]
        public decimal TotalPayableLocalAmount
        {
            get { return _TotalPayableLocalAmount; }
            set
            {
                SetProperty(ref _TotalPayableLocalAmount, value);
            }
        }

        private decimal _TotalReceivedForeignAmount = ((0));
        /// <summary>
        /// 期间收款外币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalReceivedForeignAmount", ColDesc = "期间收款外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalReceivedForeignAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "期间收款外币")]
        public decimal TotalReceivedForeignAmount
        {
            get { return _TotalReceivedForeignAmount; }
            set
            {
                SetProperty(ref _TotalReceivedForeignAmount, value);
            }
        }

        private decimal _TotalReceivedLocalAmount = ((0));
        /// <summary>
        /// 期间收款本币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalReceivedLocalAmount", ColDesc = "期间收款本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalReceivedLocalAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "期间收款本币")]
        public decimal TotalReceivedLocalAmount
        {
            get { return _TotalReceivedLocalAmount; }
            set
            {
                SetProperty(ref _TotalReceivedLocalAmount, value);
            }
        }

        private decimal _TotalPaidForeignAmount = ((0));
        /// <summary>
        /// 期间付款外币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalPaidForeignAmount", ColDesc = "期间付款外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalPaidForeignAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "期间付款外币")]
        public decimal TotalPaidForeignAmount
        {
            get { return _TotalPaidForeignAmount; }
            set
            {
                SetProperty(ref _TotalPaidForeignAmount, value);
            }
        }

        private decimal _TotalPaidLocalAmount = ((0));
        /// <summary>
        /// 期间付款本币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalPaidLocalAmount", ColDesc = "期间付款本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalPaidLocalAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "期间付款本币")]
        public decimal TotalPaidLocalAmount
        {
            get { return _TotalPaidLocalAmount; }
            set
            {
                SetProperty(ref _TotalPaidLocalAmount, value);
            }
        }

        private decimal _ClosingBalanceForeignAmount = ((0));
        /// <summary>
        /// 期末余额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ClosingBalanceForeignAmount", ColDesc = "期末余额外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ClosingBalanceForeignAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "期末余额外币")]
        public decimal ClosingBalanceForeignAmount
        {
            get { return _ClosingBalanceForeignAmount; }
            set
            {
                SetProperty(ref _ClosingBalanceForeignAmount, value);
            }
        }

        private decimal _ClosingBalanceLocalAmount = ((0));
        /// <summary>
        /// 期末余额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "ClosingBalanceLocalAmount", ColDesc = "期末余额本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ClosingBalanceLocalAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "期末余额本币")]
        public decimal ClosingBalanceLocalAmount
        {
            get { return _ClosingBalanceLocalAmount; }
            set
            {
                SetProperty(ref _ClosingBalanceLocalAmount, value);
            }
        }

        private long _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID", ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Employee_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "经办人")]
        [FKRelationAttribute("tb_Employee", "Employee_ID")]
        public long Employee_ID
        {
            get { return _Employee_ID; }
            set
            {
                SetProperty(ref _Employee_ID, value);
            }
        }

        private int? _StatementStatus;
        /// <summary>
        /// 对账状态
        /// </summary>
        [AdvQueryAttribute(ColName = "StatementStatus", ColDesc = "对账状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "StatementStatus", DecimalDigits = 0, IsNullable = true, ColumnDescription = "对账状态")]
        public int? StatementStatus
        {
            get { return _StatementStatus; }
            set
            {
                SetProperty(ref _StatementStatus, value);
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

        private string _PamountInWords;
        /// <summary>
        /// 大写金额
        /// </summary>
        [AdvQueryAttribute(ColName = "PamountInWords", ColDesc = "大写金额")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "PamountInWords", Length = 200, IsNullable = false, ColumnDescription = "大写金额")]
        public string PamountInWords
        {
            get { return _PamountInWords; }
            set
            {
                SetProperty(ref _PamountInWords, value);
            }
        }


        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions", ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ApprovalOpinions", Length = 255, IsNullable = true, ColumnDescription = "审批意见")]
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

        private string _Summary;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary", ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Summary", Length = 500, IsNullable = true, ColumnDescription = "备注")]
        public string Summary
        {
            get { return _Summary; }
            set
            {
                SetProperty(ref _Summary, value);
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

        private bool _IsMergeStatement = false;
        /// <summary>
        /// 是否合并对账单
        /// </summary>
        [AdvQueryAttribute(ColName = "IsMergeStatement", ColDesc = "是否合并对账单")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "IsMergeStatement", IsNullable = false, ColumnDescription = "是否合并对账单")]
        public bool IsMergeStatement
        {
            get { return _IsMergeStatement; }
            set
            {
                SetProperty(ref _IsMergeStatement, value);
            }
        }

        private string _MergedCustomerVendorIDs;
        /// <summary>
        /// 合并的往来单位ID列表（JSON数组格式）
        /// </summary>
        [AdvQueryAttribute(ColName = "MergedCustomerVendorIDs", ColDesc = "合并的往来单位ID列表")]
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType = "String", ColumnName = "MergedCustomerVendorIDs", Length = 1000, IsNullable = true, ColumnDescription = "合并的往来单位ID列表")]
        public string MergedCustomerVendorIDs
        {
            get { return _MergedCustomerVendorIDs; }
            set
            {
                SetProperty(ref _MergedCustomerVendorIDs, value);
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
        [Navigate(NavigateType.OneToOne, nameof(Account_id))]
        public virtual tb_FM_Account tb_fm_account { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(PayeeInfoID))]
        public virtual tb_FM_PayeeInfo tb_fm_payeeinfo { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_StatementDetail.StatementId))]
        public virtual List<tb_FM_StatementDetail> tb_FM_StatementDetails { get; set; }
        //tb_FM_StatementDetail.StatementId)
        //StatementId.FK_TB_FM_ST_REFERENCE_TB_FM_ST)
        //tb_FM_Statement.StatementId)


        #endregion




        //如果为false,则不可以。
        private bool PK_FK_ID_Check()
        {
            bool rs = true;
            return rs;
        }









        public override object Clone()
        {
            tb_FM_Statement loctype = (tb_FM_Statement)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

