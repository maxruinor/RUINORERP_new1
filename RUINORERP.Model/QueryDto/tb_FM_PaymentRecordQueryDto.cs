
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/30/2025 15:18:06
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

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 收款付款记录表-记录所有资金流动一批订单可分账户分批付 记录真实资金流动，用户需确保其 与银行流水一致
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_PaymentRecord")]
    public partial class tb_FM_PaymentRecordQueryDto:BaseEntityDto
    {
        public tb_FM_PaymentRecordQueryDto()
        {

        }

    
     

        private string _PaymentNo;
        /// <summary>
        /// 支付单号
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentNo",ColDesc = "支付单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PaymentNo",Length=30,IsNullable = false,ColumnDescription = "支付单号" )]
        public string PaymentNo 
        { 
            get{return _PaymentNo;}
            set{SetProperty(ref _PaymentNo, value);}
        }
     

        private int _BizType;
        /// <summary>
        /// 来源业务
        /// </summary>
        [AdvQueryAttribute(ColName = "BizType",ColDesc = "来源业务")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "BizType",IsNullable = false,ColumnDescription = "来源业务" )]
        public int BizType 
        { 
            get{return _BizType;}
            set{SetProperty(ref _BizType, value);}
        }
     

        private long _SourceBilllID;
        /// <summary>
        /// 来源单据
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBilllID",ColDesc = "来源单据")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "SourceBilllID",IsNullable = false,ColumnDescription = "来源单据" )]
        public long SourceBilllID 
        { 
            get{return _SourceBilllID;}
            set{SetProperty(ref _SourceBilllID, value);}
        }
     

        private string _SourceBillNO;
        /// <summary>
        /// 来源单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillNO",ColDesc = "来源单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "SourceBillNO",Length=30,IsNullable = false,ColumnDescription = "来源单号" )]
        public string SourceBillNO 
        { 
            get{return _SourceBillNO;}
            set{SetProperty(ref _SourceBillNO, value);}
        }
     

        private int _ReceivePaymentType;
        /// <summary>
        /// 收付类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ReceivePaymentType",ColDesc = "收付类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ReceivePaymentType",IsNullable = false,ColumnDescription = "收付类型" )]
        public int ReceivePaymentType 
        { 
            get{return _ReceivePaymentType;}
            set{SetProperty(ref _ReceivePaymentType, value);}
        }
     

        private long? _Account_id;
        /// <summary>
        /// 公司账户
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_id",ColDesc = "公司账户")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Account_id",IsNullable = true,ColumnDescription = "公司账户" )]
        [FKRelationAttribute("tb_FM_Account","Account_id")]
        public long? Account_id 
        { 
            get{return _Account_id;}
            set{SetProperty(ref _Account_id, value);}
        }
     

        private long _CustomerVendor_ID;
        /// <summary>
        /// 往来单位
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "往来单位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CustomerVendor_ID",IsNullable = false,ColumnDescription = "往来单位" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{SetProperty(ref _CustomerVendor_ID, value);}
        }
     

        private long? _PayeeInfoID;
        /// <summary>
        /// 收款信息
        /// </summary>
        [AdvQueryAttribute(ColName = "PayeeInfoID",ColDesc = "收款信息")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PayeeInfoID",IsNullable = true,ColumnDescription = "收款信息" )]
        [FKRelationAttribute("tb_FM_PayeeInfo","PayeeInfoID")]
        public long? PayeeInfoID 
        { 
            get{return _PayeeInfoID;}
            set{SetProperty(ref _PayeeInfoID, value);}
        }
     

        private string _PayeeAccountNo;
        /// <summary>
        /// 收款账号
        /// </summary>
        [AdvQueryAttribute(ColName = "PayeeAccountNo",ColDesc = "收款账号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PayeeAccountNo",Length=100,IsNullable = true,ColumnDescription = "收款账号" )]
        public string PayeeAccountNo 
        { 
            get{return _PayeeAccountNo;}
            set{SetProperty(ref _PayeeAccountNo, value);}
        }
     

        private long? _Currency_ID;
        /// <summary>
        /// 币别
        /// </summary>
        [AdvQueryAttribute(ColName = "Currency_ID",ColDesc = "币别")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Currency_ID",IsNullable = true,ColumnDescription = "币别" )]
        [FKRelationAttribute("tb_Currency","Currency_ID")]
        public long? Currency_ID 
        { 
            get{return _Currency_ID;}
            set{SetProperty(ref _Currency_ID, value);}
        }
     

        private decimal? _ExchangeRate;
        /// <summary>
        /// 汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "ExchangeRate",ColDesc = "汇率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "ExchangeRate",IsNullable = true,ColumnDescription = "汇率" )]
        public decimal? ExchangeRate 
        { 
            get{return _ExchangeRate;}
            set{SetProperty(ref _ExchangeRate, value);}
        }
     

        private decimal _ForeignPaidAmount= ((0));
        /// <summary>
        /// 支付金额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "ForeignPaidAmount",ColDesc = "支付金额外币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "ForeignPaidAmount",IsNullable = false,ColumnDescription = "支付金额外币" )]
        public decimal ForeignPaidAmount 
        { 
            get{return _ForeignPaidAmount;}
            set{SetProperty(ref _ForeignPaidAmount, value);}
        }
     

        private decimal _LocalPaidAmount= ((0));
        /// <summary>
        /// 支付金额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "LocalPaidAmount",ColDesc = "支付金额本币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "LocalPaidAmount",IsNullable = false,ColumnDescription = "支付金额本币" )]
        public decimal LocalPaidAmount 
        { 
            get{return _LocalPaidAmount;}
            set{SetProperty(ref _LocalPaidAmount, value);}
        }
     

        private DateTime? _PaymentDate;
        /// <summary>
        /// 支付日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentDate",ColDesc = "支付日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "PaymentDate",IsNullable = true,ColumnDescription = "支付日期" )]
        public DateTime? PaymentDate 
        { 
            get{return _PaymentDate;}
            set{SetProperty(ref _PaymentDate, value);}
        }
     

        private long? _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "经办人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private long? _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "部门")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}
            set{SetProperty(ref _DepartmentID, value);}
        }
     

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProjectGroup_ID",IsNullable = true,ColumnDescription = "项目组" )]
        [FKRelationAttribute("tb_ProjectGroup","ProjectGroup_ID")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}
            set{SetProperty(ref _ProjectGroup_ID, value);}
        }
     

        private long? _Paytype_ID;
        /// <summary>
        /// 付款方式
        /// </summary>
        [AdvQueryAttribute(ColName = "Paytype_ID",ColDesc = "付款方式")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Paytype_ID",IsNullable = true,ColumnDescription = "付款方式" )]
        [FKRelationAttribute("tb_PaymentMethod","Paytype_ID")]
        public long? Paytype_ID 
        { 
            get{return _Paytype_ID;}
            set{SetProperty(ref _Paytype_ID, value);}
        }
     

        private long? _PaymentStatus;
        /// <summary>
        /// 支付状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentStatus",ColDesc = "支付状态")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PaymentStatus",IsNullable = true,ColumnDescription = "支付状态" )]
        public long? PaymentStatus 
        { 
            get{return _PaymentStatus;}
            set{SetProperty(ref _PaymentStatus, value);}
        }
     

        private string _PaymentImagePath;
        /// <summary>
        /// 付款凭证
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentImagePath",ColDesc = "付款凭证")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "PaymentImagePath",Length=300,IsNullable = true,ColumnDescription = "付款凭证" )]
        public string PaymentImagePath 
        { 
            get{return _PaymentImagePath;}
            set{SetProperty(ref _PaymentImagePath, value);}
        }
     

        private string _ReferenceNo;
        /// <summary>
        /// 交易参考号
        /// </summary>
        [AdvQueryAttribute(ColName = "ReferenceNo",ColDesc = "交易参考号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ReferenceNo",Length=300,IsNullable = true,ColumnDescription = "交易参考号" )]
        public string ReferenceNo 
        { 
            get{return _ReferenceNo;}
            set{SetProperty(ref _ReferenceNo, value);}
        }
     

        private bool _IsReversed= false;
        /// <summary>
        /// 是否冲销
        /// </summary>
        [AdvQueryAttribute(ColName = "IsReversed",ColDesc = "是否冲销")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsReversed",IsNullable = false,ColumnDescription = "是否冲销" )]
        public bool IsReversed 
        { 
            get{return _IsReversed;}
            set{SetProperty(ref _IsReversed, value);}
        }
     

        private long? _ReversedPaymentId;
        /// <summary>
        /// 对冲记录
        /// </summary>
        [AdvQueryAttribute(ColName = "ReversedPaymentId",ColDesc = "对冲记录")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ReversedPaymentId",IsNullable = true,ColumnDescription = "对冲记录" )]
        public long? ReversedPaymentId 
        { 
            get{return _ReversedPaymentId;}
            set{SetProperty(ref _ReversedPaymentId, value);}
        }
     

        private string _ReversedPaymentNo;
        /// <summary>
        /// 对冲单号
        /// </summary>
        [AdvQueryAttribute(ColName = "ReversedPaymentNo",ColDesc = "对冲单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ReversedPaymentNo",Length=30,IsNullable = true,ColumnDescription = "对冲单号" )]
        public string ReversedPaymentNo 
        { 
            get{return _ReversedPaymentNo;}
            set{SetProperty(ref _ReversedPaymentNo, value);}
        }
     

        private string _Remark;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Remark",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Remark",Length=300,IsNullable = true,ColumnDescription = "备注" )]
        public string Remark 
        { 
            get{return _Remark;}
            set{SetProperty(ref _Remark, value);}
        }
     

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
            set{SetProperty(ref _Created_at, value);}
        }
     

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by 
        { 
            get{return _Created_by;}
            set{SetProperty(ref _Created_by, value);}
        }
     

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
            set{SetProperty(ref _Modified_at, value);}
        }
     

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
            set{SetProperty(ref _Modified_by, value);}
        }
     

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "isdeleted",IsNullable = false,ColumnDescription = "逻辑删除" )]
        public bool isdeleted 
        { 
            get{return _isdeleted;}
            set{SetProperty(ref _isdeleted, value);}
        }
     

        private string _ApprovalOpinions;
        /// <summary>
        /// 审批意见
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalOpinions",ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ApprovalOpinions",Length=255,IsNullable = true,ColumnDescription = "审批意见" )]
        public string ApprovalOpinions 
        { 
            get{return _ApprovalOpinions;}
            set{SetProperty(ref _ApprovalOpinions, value);}
        }
     

        private long? _Approver_by;
        /// <summary>
        /// 审批人
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_by",ColDesc = "审批人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Approver_by",IsNullable = true,ColumnDescription = "审批人" )]
        public long? Approver_by 
        { 
            get{return _Approver_by;}
            set{SetProperty(ref _Approver_by, value);}
        }
     

        private DateTime? _Approver_at;
        /// <summary>
        /// 审批时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Approver_at",ColDesc = "审批时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Approver_at",IsNullable = true,ColumnDescription = "审批时间" )]
        public DateTime? Approver_at 
        { 
            get{return _Approver_at;}
            set{SetProperty(ref _Approver_at, value);}
        }
     

        private int? _ApprovalStatus= ((0));
        /// <summary>
        /// 审批状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalStatus",ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint",SqlParameterDbType ="SByte",ColumnName = "ApprovalStatus",IsNullable = true,ColumnDescription = "审批状态" )]
        public int? ApprovalStatus 
        { 
            get{return _ApprovalStatus;}
            set{SetProperty(ref _ApprovalStatus, value);}
        }
     

        private bool? _ApprovalResults;
        /// <summary>
        /// 审批结果
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalResults",ColDesc = "审批结果")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "ApprovalResults",IsNullable = true,ColumnDescription = "审批结果" )]
        public bool? ApprovalResults 
        { 
            get{return _ApprovalResults;}
            set{SetProperty(ref _ApprovalResults, value);}
        }
     

        private int _PrintStatus= ((0));
        /// <summary>
        /// 打印状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintStatus",ColDesc = "打印状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "PrintStatus",IsNullable = false,ColumnDescription = "打印状态" )]
        public int PrintStatus 
        { 
            get{return _PrintStatus;}
            set{SetProperty(ref _PrintStatus, value);}
        }


       
    }
}



