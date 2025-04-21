
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/21/2025 20:12:37
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
    /// 应收应付表
    /// </summary>
    [Serializable()]
    [Description("应收应付表")]
    [SugarTable("tb_FM_ReceivablePayable")]
    public partial class tb_FM_ReceivablePayable: BaseEntity, ICloneable
    {
        public tb_FM_ReceivablePayable()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("应收应付表tb_FM_ReceivablePayable" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ARAPId;
        /// <summary>
        /// 应收付款单
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ARAPId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "应收付款单" , IsPrimaryKey = true)]
        public long ARAPId
        { 
            get{return _ARAPId;}
            set{
            SetProperty(ref _ARAPId, value);
                base.PrimaryKeyID = _ARAPId;
            }
        }

        private string _ARAPNo;
        /// <summary>
        /// 单据编号
        /// </summary>
        [AdvQueryAttribute(ColName = "ARAPNo",ColDesc = "单据编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ARAPNo" ,Length=30,IsNullable = true,ColumnDescription = "单据编号" )]
        public string ARAPNo
        { 
            get{return _ARAPNo;}
            set{
            SetProperty(ref _ARAPNo, value);
                        }
        }

        private long? _PreRPID;
        /// <summary>
        /// 预收付款单
        /// </summary>
        [AdvQueryAttribute(ColName = "PreRPID",ColDesc = "预收付款单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PreRPID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "预收付款单" )]
        [FKRelationAttribute("tb_FM_PreReceivedPayment","PreRPID")]
        public long? PreRPID
        { 
            get{return _PreRPID;}
            set{
            SetProperty(ref _PreRPID, value);
                        }
        }

        private long _CustomerVendor_ID;
        /// <summary>
        /// 往来单位
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "往来单位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "往来单位" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
                        }
        }

        private long _Currency_ID;
        /// <summary>
        /// 币别
        /// </summary>
        [AdvQueryAttribute(ColName = "Currency_ID",ColDesc = "币别")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Currency_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "币别" )]
        [FKRelationAttribute("tb_Currency","Currency_ID")]
        public long Currency_ID
        { 
            get{return _Currency_ID;}
            set{
            SetProperty(ref _Currency_ID, value);
                        }
        }

        private decimal? _ExchangeRate;
        /// <summary>
        /// 汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "ExchangeRate",ColDesc = "汇率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "ExchangeRate" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "汇率" )]
        public decimal? ExchangeRate
        { 
            get{return _ExchangeRate;}
            set{
            SetProperty(ref _ExchangeRate, value);
                        }
        }

        private long? _ReceivePaymentType;
        /// <summary>
        /// 收付类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ReceivePaymentType",ColDesc = "收付类型")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReceivePaymentType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "收付类型" )]
        public long? ReceivePaymentType
        { 
            get{return _ReceivePaymentType;}
            set{
            SetProperty(ref _ReceivePaymentType, value);
                        }
        }

        private decimal _TotalForeignPayableAmount= ((0));
        /// <summary>
        /// 总金额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalForeignPayableAmount",ColDesc = "总金额外币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalForeignPayableAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "总金额外币" )]
        public decimal TotalForeignPayableAmount
        { 
            get{return _TotalForeignPayableAmount;}
            set{
            SetProperty(ref _TotalForeignPayableAmount, value);
                        }
        }

        private decimal _TotalLocalPayableAmount= ((0));
        /// <summary>
        /// 总金额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalLocalPayableAmount",ColDesc = "总金额本币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalLocalPayableAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "总金额本币" )]
        public decimal TotalLocalPayableAmount
        { 
            get{return _TotalLocalPayableAmount;}
            set{
            SetProperty(ref _TotalLocalPayableAmount, value);
                        }
        }

        private decimal _ForeignPaidAmount= ((0));
        /// <summary>
        /// 已核销外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignPaidAmount",ColDesc = "已核销外币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ForeignPaidAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "已核销外币" )]
        public decimal ForeignPaidAmount
        { 
            get{return _ForeignPaidAmount;}
            set{
            SetProperty(ref _ForeignPaidAmount, value);
                        }
        }

        private decimal _LocalPaidAmount= ((0));
        /// <summary>
        /// 已核销本币
        /// </summary>
        [AdvQueryAttribute(ColName = "LocalPaidAmount",ColDesc = "已核销本币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "LocalPaidAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "已核销本币" )]
        public decimal LocalPaidAmount
        { 
            get{return _LocalPaidAmount;}
            set{
            SetProperty(ref _LocalPaidAmount, value);
                        }
        }

        private decimal _ForeignBalanceAmount= ((0));
        /// <summary>
        /// 未核销外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignBalanceAmount",ColDesc = "未核销外币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ForeignBalanceAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "未核销外币" )]
        public decimal ForeignBalanceAmount
        { 
            get{return _ForeignBalanceAmount;}
            set{
            SetProperty(ref _ForeignBalanceAmount, value);
                        }
        }

        private decimal _LocalBalanceAmount= ((0));
        /// <summary>
        /// 未核销本币
        /// </summary>
        [AdvQueryAttribute(ColName = "LocalBalanceAmount",ColDesc = "未核销本币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "LocalBalanceAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "未核销本币" )]
        public decimal LocalBalanceAmount
        { 
            get{return _LocalBalanceAmount;}
            set{
            SetProperty(ref _LocalBalanceAmount, value);
                        }
        }

        private DateTime? _PaymentDate;
        /// <summary>
        /// 支付日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentDate",ColDesc = "支付日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PaymentDate" ,IsNullable = true,ColumnDescription = "支付日期" )]
        public DateTime? PaymentDate
        { 
            get{return _PaymentDate;}
            set{
            SetProperty(ref _PaymentDate, value);
                        }
        }

        private DateTime? _DueDate;
        /// <summary>
        /// 到期日
        /// </summary>
        [AdvQueryAttribute(ColName = "DueDate",ColDesc = "到期日")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "DueDate" ,IsNullable = true,ColumnDescription = "到期日" )]
        public DateTime? DueDate
        { 
            get{return _DueDate;}
            set{
            SetProperty(ref _DueDate, value);
                        }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "经办人" )]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
                        }
        }

        private long? _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "部门")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "部门" )]
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
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "项目组" )]
        public long? ProjectGroup_ID
        { 
            get{return _ProjectGroup_ID;}
            set{
            SetProperty(ref _ProjectGroup_ID, value);
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

        private decimal _TaxTotalAmount= ((0));
        /// <summary>
        /// 税额总计
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxTotalAmount",ColDesc = "税额总计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxTotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "税额总计" )]
        public decimal TaxTotalAmount
        { 
            get{return _TaxTotalAmount;}
            set{
            SetProperty(ref _TaxTotalAmount, value);
                        }
        }

        private decimal _UntaxedTotalAmont= ((0));
        /// <summary>
        /// 未税总计
        /// </summary>
        [AdvQueryAttribute(ColName = "UntaxedTotalAmont",ColDesc = "未税总计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UntaxedTotalAmont" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "未税总计" )]
        public decimal UntaxedTotalAmont
        { 
            get{return _UntaxedTotalAmont;}
            set{
            SetProperty(ref _UntaxedTotalAmont, value);
                        }
        }

        private int? _FMPaymentStatus;
        /// <summary>
        /// 付款状态
        /// </summary>
        [AdvQueryAttribute(ColName = "FMPaymentStatus",ColDesc = "付款状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "FMPaymentStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "付款状态" )]
        public int? FMPaymentStatus
        { 
            get{return _FMPaymentStatus;}
            set{
            SetProperty(ref _FMPaymentStatus, value);
                        }
        }

        private string _Remark;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Remark",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Remark" ,Length=300,IsNullable = true,ColumnDescription = "备注" )]
        public string Remark
        { 
            get{return _Remark;}
            set{
            SetProperty(ref _Remark, value);
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

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ApprovalOpinions" ,Length=255,IsNullable = true,ColumnDescription = "审批意见" )]
        public string ApprovalOpinions
        { 
            get{return _ApprovalOpinions;}
            set{
            SetProperty(ref _ApprovalOpinions, value);
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

        private int? _ApprovalStatus= ((0));
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
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(PreRPID))]
        public virtual tb_FM_PreReceivedPayment tb_fm_prereceivedpayment { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_ReceivablePayableDetail.ARAPId))]
        public virtual List<tb_FM_ReceivablePayableDetail> tb_FM_ReceivablePayableDetails { get; set; }
        //tb_FM_ReceivablePayableDetail.ARAPId)
        //ARAPId.FK_TB_FM_RE_REFERENCE_TB_FM_RE)
        //tb_FM_ReceivablePayable.ARAPId)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
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
                    Type type = typeof(tb_FM_ReceivablePayable);
                    
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
            tb_FM_ReceivablePayable loctype = (tb_FM_ReceivablePayable)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

