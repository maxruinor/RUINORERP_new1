
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 18:12:12
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
    /// 记录收款 与应收的匹配，核销表-支持多对多、行项级核销 
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_PaymentSettlement")]
    public partial class tb_FM_PaymentSettlementQueryDto:BaseEntityDto
    {
        public tb_FM_PaymentSettlementQueryDto()
        {

        }

    
     

        private long? _PaymentId;
        /// <summary>
        /// 收付款单
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentId",ColDesc = "收付款单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PaymentId",IsNullable = true,ColumnDescription = "收付款单" )]
        [FKRelationAttribute("tb_FM_PaymentRecord","PaymentId")]
        public long? PaymentId 
        { 
            get{return _PaymentId;}
            set{SetProperty(ref _PaymentId, value);}
        }
     

        private int? _BizType;
        /// <summary>
        /// 来源业务
        /// </summary>
        [AdvQueryAttribute(ColName = "BizType",ColDesc = "来源业务")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "BizType",IsNullable = true,ColumnDescription = "来源业务" )]
        public int? BizType 
        { 
            get{return _BizType;}
            set{SetProperty(ref _BizType, value);}
        }
     

        private long? _SourceBilllID;
        /// <summary>
        /// 来源单据
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBilllID",ColDesc = "来源单据")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "SourceBilllID",IsNullable = true,ColumnDescription = "来源单据" )]
        public long? SourceBilllID 
        { 
            get{return _SourceBilllID;}
            set{SetProperty(ref _SourceBilllID, value);}
        }
     

        private long? _SourceBillDetailID;
        /// <summary>
        /// 单据明细
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillDetailID",ColDesc = "单据明细")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "SourceBillDetailID",IsNullable = true,ColumnDescription = "单据明细" )]
        public long? SourceBillDetailID 
        { 
            get{return _SourceBillDetailID;}
            set{SetProperty(ref _SourceBillDetailID, value);}
        }
     

        private string _SourceBillNO;
        /// <summary>
        /// 来源单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillNO",ColDesc = "来源单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "SourceBillNO",Length=30,IsNullable = true,ColumnDescription = "来源单号" )]
        public string SourceBillNO 
        { 
            get{return _SourceBillNO;}
            set{SetProperty(ref _SourceBillNO, value);}
        }
     

        private long? _ReceivePaymentType;
        /// <summary>
        /// 收付类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ReceivePaymentType",ColDesc = "收付类型")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ReceivePaymentType",IsNullable = true,ColumnDescription = "收付类型" )]
        public long? ReceivePaymentType 
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
        public long? Account_id 
        { 
            get{return _Account_id;}
            set{SetProperty(ref _Account_id, value);}
        }
     

        private long _Employee_ID;
        /// <summary>
        /// 核销人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "核销人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = false,ColumnDescription = "核销人" )]
        public long Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private long _CustomerVendor_ID;
        /// <summary>
        /// 往来单位
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "往来单位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CustomerVendor_ID",IsNullable = false,ColumnDescription = "往来单位" )]
        public long CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{SetProperty(ref _CustomerVendor_ID, value);}
        }
     

        private long _Currency_ID;
        /// <summary>
        /// 币别
        /// </summary>
        [AdvQueryAttribute(ColName = "Currency_ID",ColDesc = "币别")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Currency_ID",IsNullable = false,ColumnDescription = "币别" )]
        public long Currency_ID 
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
     

        private decimal _SettledForeignAmount= ((0));
        /// <summary>
        /// 核销金额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "SettledForeignAmount",ColDesc = "核销金额外币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SettledForeignAmount",IsNullable = false,ColumnDescription = "核销金额外币" )]
        public decimal SettledForeignAmount 
        { 
            get{return _SettledForeignAmount;}
            set{SetProperty(ref _SettledForeignAmount, value);}
        }
     

        private decimal _SettledLocalAmount= ((0));
        /// <summary>
        /// 核销金额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "SettledLocalAmount",ColDesc = "核销金额本币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SettledLocalAmount",IsNullable = false,ColumnDescription = "核销金额本币" )]
        public decimal SettledLocalAmount 
        { 
            get{return _SettledLocalAmount;}
            set{SetProperty(ref _SettledLocalAmount, value);}
        }
     

        private string _PamountInWords;
        /// <summary>
        /// 大写收款金额
        /// </summary>
        [AdvQueryAttribute(ColName = "PamountInWords",ColDesc = "大写收款金额")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PamountInWords",Length=100,IsNullable = false,ColumnDescription = "大写收款金额" )]
        public string PamountInWords 
        { 
            get{return _PamountInWords;}
            set{SetProperty(ref _PamountInWords, value);}
        }
     

        private DateTime _SettleDate;
        /// <summary>
        /// 核销日期
        /// </summary>
        [AdvQueryAttribute(ColName = "SettleDate",ColDesc = "核销日期")]
        [SugarColumn(ColumnDataType = "date",SqlParameterDbType ="DateTime",ColumnName = "SettleDate",IsNullable = false,ColumnDescription = "核销日期" )]
        public DateTime SettleDate 
        { 
            get{return _SettleDate;}
            set{SetProperty(ref _SettleDate, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=300,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }
     

        private int _SettlementType= ((0));
        /// <summary>
        /// 付款状态
        /// </summary>
        [AdvQueryAttribute(ColName = "SettlementType",ColDesc = "付款状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "SettlementType",IsNullable = false,ColumnDescription = "付款状态" )]
        public int SettlementType 
        { 
            get{return _SettlementType;}
            set{SetProperty(ref _SettlementType, value);}
        }
     

        private string _EvidenceImagePath;
        /// <summary>
        /// 凭证图
        /// </summary>
        [AdvQueryAttribute(ColName = "EvidenceImagePath",ColDesc = "凭证图")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "EvidenceImagePath",Length=300,IsNullable = true,ColumnDescription = "凭证图" )]
        public string EvidenceImagePath 
        { 
            get{return _EvidenceImagePath;}
            set{SetProperty(ref _EvidenceImagePath, value);}
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



