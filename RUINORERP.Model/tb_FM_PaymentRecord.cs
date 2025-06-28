
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/20/2025 16:20:06
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
    /// 收付款记录表
    /// </summary>
    [Serializable()]
    [Description("收付款记录表")]
    [SugarTable("tb_FM_PaymentRecord")]
    public partial class tb_FM_PaymentRecord: BaseEntity, ICloneable
    {
        public tb_FM_PaymentRecord()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("收付款记录表tb_FM_PaymentRecord" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PaymentId;
        /// <summary>
        /// 支付记录
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PaymentId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "支付记录" , IsPrimaryKey = true)]
        public long PaymentId
        { 
            get{return _PaymentId;}
            set{
            SetProperty(ref _PaymentId, value);
                base.PrimaryKeyID = _PaymentId;
            }
        }

        private string _PaymentNo;
        /// <summary>
        /// 支付单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentNo",ColDesc = "支付单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PaymentNo" ,Length=30,IsNullable = false,ColumnDescription = "支付单号" )]
        public string PaymentNo
        { 
            get{return _PaymentNo;}
            set{
            SetProperty(ref _PaymentNo, value);
                        }
        }

        private int _ReceivePaymentType;
        /// <summary>
        /// 收付类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ReceivePaymentType",ColDesc = "收付类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ReceivePaymentType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "收付类型" )]
        public int ReceivePaymentType
        { 
            get{return _ReceivePaymentType;}
            set{
            SetProperty(ref _ReceivePaymentType, value);
                        }
        }

        private long? _Account_id;
        /// <summary>
        /// 公司账户
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_id",ColDesc = "公司账户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Account_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "公司账户" )]
        [FKRelationAttribute("tb_FM_Account","Account_id")]
        public long? Account_id
        { 
            get{return _Account_id;}
            set{
            SetProperty(ref _Account_id, value);
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

        private long? _PayeeInfoID;
        /// <summary>
        /// 收款信息
        /// </summary>
        [AdvQueryAttribute(ColName = "PayeeInfoID",ColDesc = "收款信息")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PayeeInfoID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "收款信息" )]
        [FKRelationAttribute("tb_FM_PayeeInfo","PayeeInfoID")]
        public long? PayeeInfoID
        { 
            get{return _PayeeInfoID;}
            set{
            SetProperty(ref _PayeeInfoID, value);
                        }
        }

        private string _PayeeAccountNo;
        /// <summary>
        /// 收款账号
        /// </summary>
        [AdvQueryAttribute(ColName = "PayeeAccountNo",ColDesc = "收款账号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PayeeAccountNo" ,Length=100,IsNullable = true,ColumnDescription = "收款账号" )]
        public string PayeeAccountNo
        { 
            get{return _PayeeAccountNo;}
            set{
            SetProperty(ref _PayeeAccountNo, value);
                        }
        }
        private string _SourceBillNos;
        /// <summary>
        /// 来源单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillNos", ColDesc = "来源单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "SourceBillNos", Length = 1000, IsNullable = false, ColumnDescription = "来源单号")]
        public string SourceBillNos
        {
            get { return _SourceBillNos; }
            set
            {
                SetProperty(ref _SourceBillNos, value);
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

        private decimal _TotalForeignAmount= ((0));
        /// <summary>
        /// 支付金额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalForeignAmount",ColDesc = "支付金额外币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalForeignAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "支付金额外币" )]
        public decimal TotalForeignAmount
        { 
            get{return _TotalForeignAmount;}
            set{
            SetProperty(ref _TotalForeignAmount, value);
                        }
        }

        private decimal _TotalLocalAmount= ((0));
        /// <summary>
        /// 支付金额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalLocalAmount",ColDesc = "支付金额本币")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalLocalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "支付金额本币" )]
        public decimal TotalLocalAmount
        { 
            get{return _TotalLocalAmount;}
            set{
            SetProperty(ref _TotalLocalAmount, value);
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

        private long? _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "经办人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
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

        private int _PaymentStatus= ((0));
        /// <summary>
        /// 支付状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentStatus",ColDesc = "支付状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PaymentStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "支付状态" )]
        public int PaymentStatus
        { 
            get{return _PaymentStatus;}
            set{
            SetProperty(ref _PaymentStatus, value);
                        }
        }

        private string _PaymentImagePath;
        /// <summary>
        /// 付款凭证
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentImagePath",ColDesc = "付款凭证")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "PaymentImagePath" ,Length=300,IsNullable = true,ColumnDescription = "付款凭证" )]
        public string PaymentImagePath
        { 
            get{return _PaymentImagePath;}
            set{
            SetProperty(ref _PaymentImagePath, value);
                        }
        }

        private string _ReferenceNo;
        /// <summary>
        /// 交易参考号
        /// </summary>
        [AdvQueryAttribute(ColName = "ReferenceNo",ColDesc = "交易参考号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReferenceNo" ,Length=300,IsNullable = true,ColumnDescription = "交易参考号" )]
        public string ReferenceNo
        { 
            get{return _ReferenceNo;}
            set{
            SetProperty(ref _ReferenceNo, value);
                        }
        }

        private bool _IsReversed= false;
        /// <summary>
        /// 是否冲销
        /// </summary>
        [AdvQueryAttribute(ColName = "IsReversed",ColDesc = "是否冲销")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsReversed" ,IsNullable = false,ColumnDescription = "是否冲销" )]
        public bool IsReversed
        { 
            get{return _IsReversed;}
            set{
            SetProperty(ref _IsReversed, value);
                        }
        }

        private long? _ReversedOriginalId;
        /// <summary>
        /// 冲销记录
        /// </summary>
        [AdvQueryAttribute(ColName = "ReversedOriginalId",ColDesc = "冲销记录")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReversedOriginalId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "冲销记录" )]
        [FKRelationAttribute("tb_FM_PaymentRecord","ReversedOriginalId")]
        public long? ReversedOriginalId
        { 
            get{return _ReversedOriginalId;}
            set{
            SetProperty(ref _ReversedOriginalId, value);
                        }
        }

        private string _ReversedOriginalNo;
        /// <summary>
        /// 冲销单号
        /// </summary>
        [AdvQueryAttribute(ColName = "ReversedOriginalNo",ColDesc = "冲销单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReversedOriginalNo" ,Length=30,IsNullable = true,ColumnDescription = "冲销单号" )]
        public string ReversedOriginalNo
        { 
            get{return _ReversedOriginalNo;}
            set{
            SetProperty(ref _ReversedOriginalNo, value);
                        }
        }

        private long? _ReversedByPaymentId;
        /// <summary>
        /// 被冲销记录
        /// </summary>
        [AdvQueryAttribute(ColName = "ReversedByPaymentId",ColDesc = "被冲销记录")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReversedByPaymentId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "被冲销记录" )]
        [FKRelationAttribute("tb_FM_PaymentRecord","ReversedByPaymentId")]
        public long? ReversedByPaymentId
        { 
            get{return _ReversedByPaymentId;}
            set{
            SetProperty(ref _ReversedByPaymentId, value);
                        }
        }

        private string _ReversedByPaymentNo;
        /// <summary>
        /// 被冲销单号
        /// </summary>
        [AdvQueryAttribute(ColName = "ReversedByPaymentNo",ColDesc = "被冲销单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReversedByPaymentNo" ,Length=30,IsNullable = true,ColumnDescription = "被冲销单号" )]
        public string ReversedByPaymentNo
        { 
            get{return _ReversedByPaymentNo;}
            set{
            SetProperty(ref _ReversedByPaymentNo, value);
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
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(PayeeInfoID))]
        public virtual tb_FM_PayeeInfo tb_fm_payeeinfo { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Paytype_ID))]
        public virtual tb_PaymentMethod tb_paymentmethod { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Account_id))]
        public virtual tb_FM_Account tb_fm_account { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ReversedByPaymentId))]
        public virtual tb_FM_PaymentRecord tb_fm_paymentrecord_Reversed { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ReversedOriginalId))]
        public virtual tb_FM_PaymentRecord tb_fm_paymentrecord_Original { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentRecordDetail.PaymentId))]
        public virtual List<tb_FM_PaymentRecordDetail> tb_FM_PaymentRecordDetails { get; set; }
        //tb_FM_PaymentRecordDetail.PaymentId)
        //PaymentId.FK_FM_PaymentRecordDetail_REF_TB_FM_PAymentRecord)
        //tb_FM_PaymentRecord.PaymentId)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentRecord.ReversedByPaymentId))]
        public virtual List<tb_FM_PaymentRecord> tb_FM_PaymentRecords_Reversed { get; set; }
        //tb_FM_PaymentRecord.PaymentId)
        //PaymentId.FK_TB_FM_PA_REFERENCE_TB_FM_PA_ReversedByPaymentId)
        //tb_FM_PaymentRecord.ReversedByPaymentId)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FM_PaymentRecord.ReversedOriginalId))]
        public virtual List<tb_FM_PaymentRecord> tb_FM_PaymentRecords_Original{ get; set; }
        //tb_FM_PaymentRecord.PaymentId)
        //PaymentId.FK_TB_FM_PA_REFERENCE_TB_FM_PA_ReversedOriginalId)
        //tb_FM_PaymentRecord.ReversedOriginalId)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
         if("PaymentId"!="ReversedByPaymentId")
        {
        // rs=false;
        }
         if("PaymentId"!="ReversedOriginalId")
        {
        // rs=false;
        }
return rs;
}






       
        

        public override object Clone()
        {
            tb_FM_PaymentRecord loctype = (tb_FM_PaymentRecord)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

