﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:02
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
    /// 付款单明细
    /// </summary>
    [Serializable()]
    [Description("付款单明细")]
    [SugarTable("tb_FM_PaymentBillDetail")]
    public partial class tb_FM_PaymentBillDetail: BaseEntity, ICloneable
    {
        public tb_FM_PaymentBillDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("付款单明细tb_FM_PaymentBillDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PaymentDetail_id;
        /// <summary>
        /// 付款单
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PaymentDetail_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "付款单" , IsPrimaryKey = true)]
        public long PaymentDetail_id
        { 
            get{return _PaymentDetail_id;}
            set{
            SetProperty(ref _PaymentDetail_id, value);
                base.PrimaryKeyID = _PaymentDetail_id;
            }
        }

        private bool? _IsAdvancePayment;
        /// <summary>
        /// 为预付款
        /// </summary>
        [AdvQueryAttribute(ColName = "IsAdvancePayment",ColDesc = "为预付款")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsAdvancePayment" ,IsNullable = true,ColumnDescription = "为预付款" )]
        public bool? IsAdvancePayment
        { 
            get{return _IsAdvancePayment;}
            set{
            SetProperty(ref _IsAdvancePayment, value);
                        }
        }

        private long? _PrePaymentBill_id;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "PrePaymentBill_id",ColDesc = "业务类型")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PrePaymentBill_id" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "业务类型" )]
        public long? PrePaymentBill_id
        { 
            get{return _PrePaymentBill_id;}
            set{
            SetProperty(ref _PrePaymentBill_id, value);
                        }
        }

        private DateTime? _BillDate;
        /// <summary>
        /// 单据日期
        /// </summary>
        [AdvQueryAttribute(ColName = "BillDate",ColDesc = "单据日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "BillDate" ,IsNullable = true,ColumnDescription = "单据日期" )]
        public DateTime? BillDate
        { 
            get{return _BillDate;}
            set{
            SetProperty(ref _BillDate, value);
                        }
        }

        private DateTime? _PaymentDate;
        /// <summary>
        /// 付款日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentDate",ColDesc = "付款日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PaymentDate" ,IsNullable = true,ColumnDescription = "付款日期" )]
        public DateTime? PaymentDate
        { 
            get{return _PaymentDate;}
            set{
            SetProperty(ref _PaymentDate, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=30,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        private string _VoucherNumber;
        /// <summary>
        /// 凭证号码
        /// </summary>
        [AdvQueryAttribute(ColName = "VoucherNumber",ColDesc = "凭证号码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "VoucherNumber" ,Length=50,IsNullable = true,ColumnDescription = "凭证号码" )]
        public string VoucherNumber
        { 
            get{return _VoucherNumber;}
            set{
            SetProperty(ref _VoucherNumber, value);
                        }
        }

        private bool? _GenerateVoucher;
        /// <summary>
        /// 产生凭证
        /// </summary>
        [AdvQueryAttribute(ColName = "GenerateVoucher",ColDesc = "产生凭证")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "GenerateVoucher" ,IsNullable = true,ColumnDescription = "产生凭证" )]
        public bool? GenerateVoucher
        { 
            get{return _GenerateVoucher;}
            set{
            SetProperty(ref _GenerateVoucher, value);
                        }
        }

        private string _Reason;
        /// <summary>
        /// 付款原因
        /// </summary>
        [AdvQueryAttribute(ColName = "Reason",ColDesc = "付款原因")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Reason" ,Length=200,IsNullable = true,ColumnDescription = "付款原因" )]
        public string Reason
        { 
            get{return _Reason;}
            set{
            SetProperty(ref _Reason, value);
                        }
        }

        private decimal? _TotalAmount;
        /// <summary>
        /// 付款总金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "付款总金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalAmount" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "付款总金额" )]
        public decimal? TotalAmount
        { 
            get{return _TotalAmount;}
            set{
            SetProperty(ref _TotalAmount, value);
                        }
        }

        private int? _SourceBill_BizType;
        /// <summary>
        /// 来源业务
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBill_BizType",ColDesc = "来源业务")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "SourceBill_BizType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "来源业务" )]
        public int? SourceBill_BizType
        { 
            get{return _SourceBill_BizType;}
            set{
            SetProperty(ref _SourceBill_BizType, value);
                        }
        }

        private long? _SourceBill_ID;
        /// <summary>
        /// 来源单据
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBill_ID",ColDesc = "来源单据")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SourceBill_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "来源单据" )]
        public long? SourceBill_ID
        { 
            get{return _SourceBill_ID;}
            set{
            SetProperty(ref _SourceBill_ID, value);
                        }
        }

        private string _SourceBillNO;
        /// <summary>
        /// 来源单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillNO",ColDesc = "来源单号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SourceBillNO" ,Length=30,IsNullable = true,ColumnDescription = "来源单号" )]
        public string SourceBillNO
        { 
            get{return _SourceBillNO;}
            set{
            SetProperty(ref _SourceBillNO, value);
                        }
        }

        private decimal? _OverpaymentAmount;
        /// <summary>
        /// 超付金额
        /// </summary>
        [AdvQueryAttribute(ColName = "OverpaymentAmount",ColDesc = "超付金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "OverpaymentAmount" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "超付金额" )]
        public decimal? OverpaymentAmount
        { 
            get{return _OverpaymentAmount;}
            set{
            SetProperty(ref _OverpaymentAmount, value);
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

        private string _Notes2;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes2",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes2" ,Length=1500,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes2
        { 
            get{return _Notes2;}
            set{
            SetProperty(ref _Notes2, value);
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

        private int? _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "数据状态" )]
        public int? DataStatus
        { 
            get{return _DataStatus;}
            set{
            SetProperty(ref _DataStatus, value);
                        }
        }

        #endregion

        #region 扩展属性


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
                    Type type = typeof(tb_FM_PaymentBillDetail);
                    
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
            tb_FM_PaymentBillDetail loctype = (tb_FM_PaymentBillDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

