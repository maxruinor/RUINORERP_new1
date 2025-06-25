
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/24/2025 20:48:59
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

namespace RUINORERP.Model
{
    /// <summary>
    /// 收付款单明细统计 用pb文件生成。选择要生成的视图.检查列的描述。描述不能全空
    /// </summary>
    [Serializable()]
    [SugarTable("View_FM_PaymentRecordItems")]
    public partial class View_FM_PaymentRecordItems:BaseViewEntity
    {
        public View_FM_PaymentRecordItems()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_FM_PaymentRecordItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private long? _PaymentId;
        
        
        /// <summary>
        /// 支付记录
        /// </summary>

        [AdvQueryAttribute(ColName = "PaymentId",ColDesc = "支付记录")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PaymentId" ,IsNullable = true,ColumnDescription = "支付记录" )]
        [Display(Name = "支付记录")]
        public long? PaymentId 
        { 
            get{return _PaymentId;}            set{                SetProperty(ref _PaymentId, value);                }
        }

        private string _PaymentNo;
        
        
        /// <summary>
        /// 支付单号
        /// </summary>

        [AdvQueryAttribute(ColName = "PaymentNo",ColDesc = "支付单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PaymentNo" ,Length=30,IsNullable = true,ColumnDescription = "支付单号" )]
        [Display(Name = "支付单号")]
        public string PaymentNo 
        { 
            get{return _PaymentNo;}            set{                SetProperty(ref _PaymentNo, value);                }
        }

        private string _SourceBillNo;
        
        
        /// <summary>
        /// 来源单号
        /// </summary>

        [AdvQueryAttribute(ColName = "SourceBillNo",ColDesc = "来源单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SourceBillNo" ,Length=30,IsNullable = true,ColumnDescription = "来源单号" )]
        [Display(Name = "来源单号")]
        public string SourceBillNo 
        { 
            get{return _SourceBillNo;}            set{                SetProperty(ref _SourceBillNo, value);                }
        }

        private int? _SourceBizType;
        
        
        /// <summary>
        /// 来源业务
        /// </summary>

        [AdvQueryAttribute(ColName = "SourceBizType",ColDesc = "来源业务")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "SourceBizType" ,IsNullable = true,ColumnDescription = "来源业务" )]
        [Display(Name = "来源业务")]
        public int? SourceBizType 
        { 
            get{return _SourceBizType;}            set{                SetProperty(ref _SourceBizType, value);                }
        }

        private long? _Account_id;
        
        
        /// <summary>
        /// 公司账户
        /// </summary>

        [AdvQueryAttribute(ColName = "Account_id",ColDesc = "公司账户")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Account_id" ,IsNullable = true,ColumnDescription = "公司账户" )]
        [Display(Name = "公司账户")]
        public long? Account_id 
        { 
            get{return _Account_id;}            set{                SetProperty(ref _Account_id, value);                }
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 往来单位
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "往来单位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" ,IsNullable = true,ColumnDescription = "往来单位" )]
        [Display(Name = "往来单位")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
        }

        private long? _PayeeInfoID;
        
        
        /// <summary>
        /// 收款信息
        /// </summary>

        [AdvQueryAttribute(ColName = "PayeeInfoID",ColDesc = "收款信息")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PayeeInfoID" ,IsNullable = true,ColumnDescription = "收款信息" )]
        [Display(Name = "收款信息")]
        public long? PayeeInfoID 
        { 
            get{return _PayeeInfoID;}            set{                SetProperty(ref _PayeeInfoID, value);                }
        }

        private string _PayeeAccountNo;
        
        
        /// <summary>
        /// 收款账号
        /// </summary>

        [AdvQueryAttribute(ColName = "PayeeAccountNo",ColDesc = "收款账号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PayeeAccountNo" ,Length=100,IsNullable = true,ColumnDescription = "收款账号" )]
        [Display(Name = "收款账号")]
        public string PayeeAccountNo 
        { 
            get{return _PayeeAccountNo;}            set{                SetProperty(ref _PayeeAccountNo, value);                }
        }

        private long? _Currency_ID;
        
        
        /// <summary>
        /// 币别
        /// </summary>

        [AdvQueryAttribute(ColName = "Currency_ID",ColDesc = "币别")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Currency_ID" ,IsNullable = true,ColumnDescription = "币别" )]
        [Display(Name = "币别")]
        public long? Currency_ID 
        { 
            get{return _Currency_ID;}            set{                SetProperty(ref _Currency_ID, value);                }
        }

        private decimal? _TotalForeignAmount;
        
        
        /// <summary>
        /// 支付金额外币
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalForeignAmount",ColDesc = "支付金额外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalForeignAmount" ,IsNullable = true,ColumnDescription = "支付金额外币" )]
        [Display(Name = "支付金额外币")]
        public decimal? TotalForeignAmount 
        { 
            get{return _TotalForeignAmount;}            set{                SetProperty(ref _TotalForeignAmount, value);                }
        }

        private decimal? _TotalLocalAmount;
        
        
        /// <summary>
        /// 支付金额本币
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalLocalAmount",ColDesc = "支付金额本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TotalLocalAmount" ,IsNullable = true,ColumnDescription = "支付金额本币" )]
        [Display(Name = "支付金额本币")]
        public decimal? TotalLocalAmount 
        { 
            get{return _TotalLocalAmount;}            set{                SetProperty(ref _TotalLocalAmount, value);                }
        }

        private DateTime? _PaymentDate;
        
        
        /// <summary>
        /// 支付日期
        /// </summary>

        [AdvQueryAttribute(ColName = "PaymentDate",ColDesc = "支付日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PaymentDate" ,IsNullable = true,ColumnDescription = "支付日期" )]
        [Display(Name = "支付日期")]
        public DateTime? PaymentDate 
        { 
            get{return _PaymentDate;}            set{                SetProperty(ref _PaymentDate, value);                }
        }

        private long? _Employee_ID;


        /// <summary>
        /// 经办人
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" ,IsNullable = true,ColumnDescription = "经办人")]
        [Display(Name = "经办人")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
        }

        private long? _Paytype_ID;
        
        
        /// <summary>
        /// 付款方式
        /// </summary>

        [AdvQueryAttribute(ColName = "Paytype_ID",ColDesc = "付款方式")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Paytype_ID" ,IsNullable = true,ColumnDescription = "付款方式" )]
        [Display(Name = "付款方式")]
        public long? Paytype_ID 
        { 
            get{return _Paytype_ID;}            set{                SetProperty(ref _Paytype_ID, value);                }
        }

        private int? _PaymentStatus;
        
        
        /// <summary>
        /// 支付状态
        /// </summary>

        [AdvQueryAttribute(ColName = "PaymentStatus",ColDesc = "支付状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PaymentStatus" ,IsNullable = true,ColumnDescription = "支付状态" )]
        [Display(Name = "支付状态")]
        public int? PaymentStatus 
        { 
            get{return _PaymentStatus;}            set{                SetProperty(ref _PaymentStatus, value);                }
        }

        private string _PaymentImagePath;
        
        
        /// <summary>
        /// 付款凭证
        /// </summary>

        [AdvQueryAttribute(ColName = "PaymentImagePath",ColDesc = "付款凭证")]
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "PaymentImagePath" ,Length=300,IsNullable = true,ColumnDescription = "付款凭证" )]
        [Display(Name = "付款凭证")]
        public string PaymentImagePath 
        { 
            get{return _PaymentImagePath;}            set{                SetProperty(ref _PaymentImagePath, value);                }
        }

        private string _ReferenceNo;
        
        
        /// <summary>
        /// 交易参考号
        /// </summary>

        [AdvQueryAttribute(ColName = "ReferenceNo",ColDesc = "交易参考号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReferenceNo" ,Length=300,IsNullable = true,ColumnDescription = "交易参考号" )]
        [Display(Name = "交易参考号")]
        public string ReferenceNo 
        { 
            get{return _ReferenceNo;}            set{                SetProperty(ref _ReferenceNo, value);                }
        }

        private bool? _IsReversed;
        
        
        /// <summary>
        /// 是否冲销
        /// </summary>

        [AdvQueryAttribute(ColName = "IsReversed",ColDesc = "是否冲销")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsReversed" ,IsNullable = true,ColumnDescription = "是否冲销" )]
        [Display(Name = "是否冲销")]
        public bool? IsReversed 
        { 
            get{return _IsReversed;}            set{                SetProperty(ref _IsReversed, value);                }
        }

        private long? _ReversedOriginalId;
        
        
        /// <summary>
        /// 冲销记录
        /// </summary>

        [AdvQueryAttribute(ColName = "ReversedOriginalId",ColDesc = "冲销记录")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReversedOriginalId" ,IsNullable = true,ColumnDescription = "冲销记录" )]
        [Display(Name = "冲销记录")]
        public long? ReversedOriginalId 
        { 
            get{return _ReversedOriginalId;}            set{                SetProperty(ref _ReversedOriginalId, value);                }
        }

        private string _ReversedOriginalNo;
        
        
        /// <summary>
        /// 冲销单号
        /// </summary>

        [AdvQueryAttribute(ColName = "ReversedOriginalNo",ColDesc = "冲销单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReversedOriginalNo" ,Length=30,IsNullable = true,ColumnDescription = "冲销单号" )]
        [Display(Name = "冲销单号")]
        public string ReversedOriginalNo 
        { 
            get{return _ReversedOriginalNo;}            set{                SetProperty(ref _ReversedOriginalNo, value);                }
        }

        private long? _ReversedByPaymentId;
        
        
        /// <summary>
        /// 被冲销记录
        /// </summary>

        [AdvQueryAttribute(ColName = "ReversedByPaymentId",ColDesc = "被冲销记录")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReversedByPaymentId" ,IsNullable = true,ColumnDescription = "被冲销记录" )]
        [Display(Name = "被冲销记录")]
        public long? ReversedByPaymentId 
        { 
            get{return _ReversedByPaymentId;}            set{                SetProperty(ref _ReversedByPaymentId, value);                }
        }

        private string _ReversedByPaymentNo;
        
        
        /// <summary>
        /// 被冲销单号
        /// </summary>

        [AdvQueryAttribute(ColName = "ReversedByPaymentNo",ColDesc = "被冲销单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ReversedByPaymentNo" ,Length=30,IsNullable = true,ColumnDescription = "被冲销单号" )]
        [Display(Name = "被冲销单号")]
        public string ReversedByPaymentNo 
        { 
            get{return _ReversedByPaymentNo;}            set{                SetProperty(ref _ReversedByPaymentNo, value);                }
        }

        private long? _PaymentDetailId;
        
        
        /// <summary>
        /// 付款明细
        /// </summary>

        [AdvQueryAttribute(ColName = "PaymentDetailId",ColDesc = "付款明细")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PaymentDetailId" ,IsNullable = true,ColumnDescription = "付款明细" )]
        [Display(Name = "付款明细")]
        public long? PaymentDetailId 
        { 
            get{return _PaymentDetailId;}            set{                SetProperty(ref _PaymentDetailId, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 部门
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" ,IsNullable = true,ColumnDescription = "部门" )]
        [Display(Name = "部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private long? _ProjectGroup_ID;
        
        
        /// <summary>
        /// 项目组
        /// </summary>

        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" ,IsNullable = true,ColumnDescription = "项目组" )]
        [Display(Name = "项目组")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}            set{                SetProperty(ref _ProjectGroup_ID, value);                }
        }

        private decimal? _ForeignAmount;
        
        
        /// <summary>
        /// 支付金额外币
        /// </summary>

        [AdvQueryAttribute(ColName = "ForeignAmount",ColDesc = "支付金额外币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ForeignAmount" ,IsNullable = true,ColumnDescription = "支付金额外币" )]
        [Display(Name = "支付金额外币")]
        public decimal? ForeignAmount 
        { 
            get{return _ForeignAmount;}            set{                SetProperty(ref _ForeignAmount, value);                }
        }

        private decimal? _LocalAmount;
        
        
        /// <summary>
        /// 支付金额本币
        /// </summary>

        [AdvQueryAttribute(ColName = "LocalAmount",ColDesc = "支付金额本币")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "LocalAmount" ,IsNullable = true,ColumnDescription = "支付金额本币" )]
        [Display(Name = "支付金额本币")]
        public decimal? LocalAmount 
        { 
            get{return _LocalAmount;}            set{                SetProperty(ref _LocalAmount, value);                }
        }

        private string _Summary;
        
        
        /// <summary>
        /// 摘要
        /// </summary>

        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=300,IsNullable = true,ColumnDescription = "摘要" )]
        [Display(Name = "摘要")]
        public string Summary 
        { 
            get{return _Summary;}            set{                SetProperty(ref _Summary, value);                }
        }

        private string _Remark;
        
        
        /// <summary>
        /// 备注
        /// </summary>

        [AdvQueryAttribute(ColName = "Remark",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Remark" ,Length=300,IsNullable = true,ColumnDescription = "备注" )]
        [Display(Name = "备注")]
        public string Remark 
        { 
            get{return _Remark;}            set{                SetProperty(ref _Remark, value);                }
        }

        private DateTime? _Created_at;
        
        
        /// <summary>
        /// 创建时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        [Display(Name = "创建时间")]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}            set{                SetProperty(ref _Created_at, value);                }
        }

        private long? _Created_by;
        
        
        /// <summary>
        /// 创建人
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" ,IsNullable = true,ColumnDescription = "创建人" )]
        [Display(Name = "创建人")]
        public long? Created_by 
        { 
            get{return _Created_by;}            set{                SetProperty(ref _Created_by, value);                }
        }

        private bool? _isdeleted;
        
        
        /// <summary>
        /// 逻辑删除
        /// </summary>

        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = true,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        [Display(Name = "逻辑删除")]
        public bool? isdeleted 
        { 
            get{return _isdeleted;}            set{                SetProperty(ref _isdeleted, value);                }
        }







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
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(View_FM_PaymentRecordItems);
                    
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
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

