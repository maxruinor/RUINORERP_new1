
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/29/2025 11:22:26
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
    /// 记录收款 与应收的匹配，核销表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_PaymentSettlement")]
    public partial class tb_FM_PaymentSettlementQueryDto:BaseEntityDto
    {
        public tb_FM_PaymentSettlementQueryDto()
        {

        }

    
     

        private string _SettlementNo;
        /// <summary>
        /// 来源单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SettlementNo",ColDesc = "来源单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "SettlementNo",Length=30,IsNullable = true,ColumnDescription = "来源单号" )]
        public string SettlementNo 
        { 
            get{return _SettlementNo;}
            set{SetProperty(ref _SettlementNo, value);}
        }
     

        private int? _BizType;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BizType",ColDesc = "业务类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "BizType",IsNullable = true,ColumnDescription = "业务类型" )]
        public int? BizType 
        { 
            get{return _BizType;}
            set{SetProperty(ref _BizType, value);}
        }
     

        private long? _SourceBillID;
        /// <summary>
        /// 来源单据
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillID",ColDesc = "来源单据")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "SourceBillID",IsNullable = true,ColumnDescription = "来源单据" )]
        public long? SourceBillID 
        { 
            get{return _SourceBillID;}
            set{SetProperty(ref _SourceBillID, value);}
        }
     

        private string _SourceBillNO;
        /// <summary>
        /// 来源单据编号
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillNO",ColDesc = "来源单据编号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "SourceBillNO",Length=30,IsNullable = true,ColumnDescription = "来源单据编号" )]
        public string SourceBillNO 
        { 
            get{return _SourceBillNO;}
            set{SetProperty(ref _SourceBillNO, value);}
        }
     

        private int? _SourceBizType;
        /// <summary>
        /// 来源单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBizType",ColDesc = "来源单据类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "SourceBizType",IsNullable = true,ColumnDescription = "来源单据类型" )]
        public int? SourceBizType 
        { 
            get{return _SourceBizType;}
            set{SetProperty(ref _SourceBizType, value);}
        }
     

        private long? _SourceCurrencyID;
        /// <summary>
        /// 来源币种
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceCurrencyID",ColDesc = "来源币种")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "SourceCurrencyID",IsNullable = true,ColumnDescription = "来源币种" )]
        public long? SourceCurrencyID 
        { 
            get{return _SourceCurrencyID;}
            set{SetProperty(ref _SourceCurrencyID, value);}
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
     

        private int? _TargetBizType;
        /// <summary>
        /// 目标单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "TargetBizType",ColDesc = "目标单据类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "TargetBizType",IsNullable = true,ColumnDescription = "目标单据类型" )]
        public int? TargetBizType 
        { 
            get{return _TargetBizType;}
            set{SetProperty(ref _TargetBizType, value);}
        }
     

        private long? _TargetBillID;
        /// <summary>
        /// 目标单据
        /// </summary>
        [AdvQueryAttribute(ColName = "TargetBillID",ColDesc = "目标单据")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "TargetBillID",IsNullable = true,ColumnDescription = "目标单据" )]
        public long? TargetBillID 
        { 
            get{return _TargetBillID;}
            set{SetProperty(ref _TargetBillID, value);}
        }
     

        private string _TargetBillNO;
        /// <summary>
        /// 目标单据编号
        /// </summary>
        [AdvQueryAttribute(ColName = "TargetBillNO",ColDesc = "目标单据编号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "TargetBillNO",Length=30,IsNullable = true,ColumnDescription = "目标单据编号" )]
        public string TargetBillNO 
        { 
            get{return _TargetBillNO;}
            set{SetProperty(ref _TargetBillNO, value);}
        }
     

        private long? _TargetCurrencyID;
        /// <summary>
        /// 目标币种
        /// </summary>
        [AdvQueryAttribute(ColName = "TargetCurrencyID",ColDesc = "目标币种")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "TargetCurrencyID",IsNullable = true,ColumnDescription = "目标币种" )]
        public long? TargetCurrencyID 
        { 
            get{return _TargetCurrencyID;}
            set{SetProperty(ref _TargetCurrencyID, value);}
        }
     

        private long _ReceivePaymentType;
        /// <summary>
        /// 收付类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ReceivePaymentType",ColDesc = "收付类型")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ReceivePaymentType",IsNullable = false,ColumnDescription = "收付类型" )]
        public long ReceivePaymentType 
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
     

        private bool _IsAutoSettlement= false;
        /// <summary>
        /// 是否自动核销
        /// </summary>
        [AdvQueryAttribute(ColName = "IsAutoSettlement",ColDesc = "是否自动核销")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsAutoSettlement",IsNullable = false,ColumnDescription = "是否自动核销" )]
        public bool IsAutoSettlement 
        { 
            get{return _IsAutoSettlement;}
            set{SetProperty(ref _IsAutoSettlement, value);}
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
     

        private long? _ReversedSettlementID;
        /// <summary>
        /// 对冲记录
        /// </summary>
        [AdvQueryAttribute(ColName = "ReversedSettlementID",ColDesc = "对冲记录")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ReversedSettlementID",IsNullable = true,ColumnDescription = "对冲记录" )]
        [FKRelationAttribute("tb_FM_PaymentSettlement","ReversedSettlementID")]
        public long? ReversedSettlementID 
        { 
            get{return _ReversedSettlementID;}
            set{SetProperty(ref _ReversedSettlementID, value);}
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
        /// 核销状态
        /// </summary>
        [AdvQueryAttribute(ColName = "SettlementType",ColDesc = "核销状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "SettlementType",IsNullable = false,ColumnDescription = "核销状态" )]
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


       
    }
}



