﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/18/2025 13:55:16
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
    /// 预收付款单
    /// </summary>
    [Serializable()]
    [Description("预收付款单")]
    [SugarTable("tb_FM_PreReceivedPayment")]
    public partial class tb_FM_PreReceivedPayment : BaseEntity, ICloneable
    {
        public tb_FM_PreReceivedPayment()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("预收付款单tb_FM_PreReceivedPayment" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PreRPID;
        /// <summary>
        /// 预收付款单
        /// </summary>

        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "PreRPID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "预收付款单", IsPrimaryKey = true)]
        public long PreRPID
        {
            get { return _PreRPID; }
            set
            {
                SetProperty(ref _PreRPID, value);
                base.PrimaryKeyID = _PreRPID;
            }
        }

        private string _PreRPNO;
        /// <summary>
        /// 单据编号
        /// </summary>
        [AdvQueryAttribute(ColName = "PreRPNO", ColDesc = "单据编号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "PreRPNO", Length = 30, IsNullable = true, ColumnDescription = "单据编号")]
        public string PreRPNO
        {
            get { return _PreRPNO; }
            set
            {
                SetProperty(ref _PreRPNO, value);
            }
        }

        private long? _Account_id;
        /// <summary>
        /// 收付款账户
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_id", ColDesc = "收付款账户")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Account_id", DecimalDigits = 0, IsNullable = true, ColumnDescription = "收付款账户")]
        [FKRelationAttribute("tb_FM_Account", "Account_id")]
        public long? Account_id
        {
            get { return _Account_id; }
            set
            {
                SetProperty(ref _Account_id, value);
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

        private long? _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID", ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Employee_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "经办人")]
        [FKRelationAttribute("tb_Employee", "Employee_ID")]
        public long? Employee_ID
        {
            get { return _Employee_ID; }
            set
            {
                SetProperty(ref _Employee_ID, value);
            }
        }

        private long? _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID", ColDesc = "部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "DepartmentID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "部门")]
        [FKRelationAttribute("tb_Department", "DepartmentID")]
        public long? DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
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

        private long? _Paytype_ID;
        /// <summary>
        /// 付款方式
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_ID", ColDesc = "付款方式")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Paytype_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "付款方式")]
        [FKRelationAttribute("tb_PaymentMethod", "Paytype_ID")]
        public long? Paytype_ID
        {
            get { return _Paytype_ID; }
            set
            {
                SetProperty(ref _Paytype_ID, value);
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

        private decimal? _ExchangeRate;
        /// <summary>
        /// 汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "ExchangeRate", ColDesc = "汇率")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType = "Decimal", ColumnName = "ExchangeRate", DecimalDigits = 4, IsNullable = true, ColumnDescription = "汇率")]
        public decimal? ExchangeRate
        {
            get { return _ExchangeRate; }
            set
            {
                SetProperty(ref _ExchangeRate, value);
            }
        }

        private DateTime _PrePayDate;
        /// <summary>
        /// 付款日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PrePayDate", ColDesc = "付款日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "PrePayDate", IsNullable = false, ColumnDescription = "付款日期")]
        public DateTime PrePayDate
        {
            get { return _PrePayDate; }
            set
            {
                SetProperty(ref _PrePayDate, value);
            }
        }

        private string _PrePaymentReason;
        /// <summary>
        /// 事由
        /// </summary>
        [AdvQueryAttribute(ColName = "PrePaymentReason", ColDesc = "事由")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "PrePaymentReason", Length = 200, IsNullable = false, ColumnDescription = "事由")]
        public string PrePaymentReason
        {
            get { return _PrePaymentReason; }
            set
            {
                SetProperty(ref _PrePaymentReason, value);
            }
        }

        private int? _SourceBill_BizType;
        /// <summary>
        /// 来源业务
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBill_BizType", ColDesc = "来源业务")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "SourceBill_BizType", DecimalDigits = 0, IsNullable = true, ColumnDescription = "来源业务")]
        public int? SourceBill_BizType
        {
            get { return _SourceBill_BizType; }
            set
            {
                SetProperty(ref _SourceBill_BizType, value);
            }
        }

        private long? _SourceBill_ID;
        /// <summary>
        /// 来源单据
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBill_ID", ColDesc = "来源单据")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "SourceBill_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "来源单据")]
        public long? SourceBill_ID
        {
            get { return _SourceBill_ID; }
            set
            {
                SetProperty(ref _SourceBill_ID, value);
            }
        }

        private string _SourceBillNO;
        /// <summary>
        /// 来源单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillNO", ColDesc = "来源单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "SourceBillNO", Length = 30, IsNullable = true, ColumnDescription = "来源单号")]
        public string SourceBillNO
        {
            get { return _SourceBillNO; }
            set
            {
                SetProperty(ref _SourceBillNO, value);
            }
        }

        private int? _FMPaymentStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "FMPaymentStatus", ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "FMPaymentStatus", DecimalDigits = 0, IsNullable = true, ColumnDescription = "数据状态")]
        public int? FMPaymentStatus
        {
            get { return _FMPaymentStatus; }
            set
            {
                SetProperty(ref _FMPaymentStatus, value);
            }
        }

        private decimal _ForeignPrepaidAmount = ((0));
        /// <summary>
        /// 预定金额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignPrepaidAmount", ColDesc = "预定金额外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ForeignPrepaidAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "预定金额外币")]
        public decimal ForeignPrepaidAmount
        {
            get { return _ForeignPrepaidAmount; }
            set
            {
                SetProperty(ref _ForeignPrepaidAmount, value);
            }
        }

        private decimal _LocalPrepaidAmount = ((0));
        /// <summary>
        /// 预定金额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "LocalPrepaidAmount", ColDesc = "预定金额本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "LocalPrepaidAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "预定金额本币")]
        public decimal LocalPrepaidAmount
        {
            get { return _LocalPrepaidAmount; }
            set
            {
                SetProperty(ref _LocalPrepaidAmount, value);
            }
        }

        private string _LocalPrepaidAmountInWords;
        /// <summary>
        /// 大写预定金额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "LocalPrepaidAmountInWords", ColDesc = "大写预定金额本币")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "LocalPrepaidAmountInWords", Length = 150, IsNullable = false, ColumnDescription = "大写预定金额本币")]
        public string LocalPrepaidAmountInWords
        {
            get { return _LocalPrepaidAmountInWords; }
            set
            {
                SetProperty(ref _LocalPrepaidAmountInWords, value);
            }
        }

        private decimal _ForeignPaidAmount = ((0));
        /// <summary>
        /// 核销金额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignPaidAmount", ColDesc = "核销金额外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ForeignPaidAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "核销金额外币")]
        public decimal ForeignPaidAmount
        {
            get { return _ForeignPaidAmount; }
            set
            {
                SetProperty(ref _ForeignPaidAmount, value);
            }
        }

        private decimal _LocalPaidAmount = ((0));
        /// <summary>
        /// 核销金额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "LocalPaidAmount", ColDesc = "核销金额本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "LocalPaidAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "核销金额本币")]
        public decimal LocalPaidAmount
        {
            get { return _LocalPaidAmount; }
            set
            {
                SetProperty(ref _LocalPaidAmount, value);
            }
        }

        private decimal _ForeignBalanceAmount = ((0));
        /// <summary>
        /// 余额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignBalanceAmount", ColDesc = "余额外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ForeignBalanceAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "余额外币")]
        public decimal ForeignBalanceAmount
        {
            get { return _ForeignBalanceAmount; }
            set
            {
                SetProperty(ref _ForeignBalanceAmount, value);
            }
        }

        private decimal _LocalBalanceAmount = ((0));
        /// <summary>
        /// 余额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "LocalBalanceAmount", ColDesc = "余额本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "LocalBalanceAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "余额本币")]
        public decimal LocalBalanceAmount
        {
            get { return _LocalBalanceAmount; }
            set
            {
                SetProperty(ref _LocalBalanceAmount, value);
            }
        }

        private int _ReceivePaymentType = ((0));
        /// <summary>
        /// 收付款类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ReceivePaymentType", ColDesc = "收付款类型")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "ReceivePaymentType", DecimalDigits = 0, IsNullable = false, ColumnDescription = "收付款类型")]
        public int ReceivePaymentType
        {
            get { return _ReceivePaymentType; }
            set
            {
                SetProperty(ref _ReceivePaymentType, value);
            }
        }

        private string _PaymentImagePath;
        /// <summary>
        /// 付款凭证
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentImagePath", ColDesc = "付款凭证")]
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType = "String", ColumnName = "PaymentImagePath", Length = 300, IsNullable = true, ColumnDescription = "付款凭证")]
        public string PaymentImagePath
        {
            get { return _PaymentImagePath; }
            set
            {
                SetProperty(ref _PaymentImagePath, value);
            }
        }

        private string _Remark;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Remark", ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Remark", Length = 300, IsNullable = true, ColumnDescription = "备注")]
        public string Remark
        {
            get { return _Remark; }
            set
            {
                SetProperty(ref _Remark, value);
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

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Currency_ID))]
        public virtual tb_Currency tb_currency { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Account_id))]
        public virtual tb_FM_Account tb_fm_account { get; set; }

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
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(DepartmentID))]
        public virtual tb_Department tb_department { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }



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
                    Type type = typeof(tb_FM_PreReceivedPayment);

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
            tb_FM_PreReceivedPayment loctype = (tb_FM_PreReceivedPayment)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

