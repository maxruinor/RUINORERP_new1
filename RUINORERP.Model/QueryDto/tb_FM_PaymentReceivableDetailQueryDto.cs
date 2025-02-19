
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:05
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
    /// 应收明细 如果一个销售订单多次发货时，销售出库单即可对应这里的明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_PaymentReceivableDetail")]
    public partial class tb_FM_PaymentReceivableDetailQueryDto:BaseEntityDto
    {
        public tb_FM_PaymentReceivableDetailQueryDto()
        {

        }

    
     

        private long? _PaymentReceivableID;
        /// <summary>
        /// 应付款单
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentReceivableID",ColDesc = "应付款单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PaymentReceivableID",IsNullable = true,ColumnDescription = "应付款单" )]
        [FKRelationAttribute("tb_FM_PaymentReceivable","PaymentReceivableID")]
        public long? PaymentReceivableID 
        { 
            get{return _PaymentReceivableID;}
            set{SetProperty(ref _PaymentReceivableID, value);}
        }
     

        private bool? _HasPrePayment= false;
        /// <summary>
        /// 为预付款
        /// </summary>
        [AdvQueryAttribute(ColName = "HasPrePayment",ColDesc = "为预付款")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "HasPrePayment",IsNullable = true,ColumnDescription = "为预付款" )]
        public bool? HasPrePayment 
        { 
            get{return _HasPrePayment;}
            set{SetProperty(ref _HasPrePayment, value);}
        }
     

        private DateTime? _PaymentDate;
        /// <summary>
        /// 付款日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentDate",ColDesc = "付款日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "PaymentDate",IsNullable = true,ColumnDescription = "付款日期" )]
        public DateTime? PaymentDate 
        { 
            get{return _PaymentDate;}
            set{SetProperty(ref _PaymentDate, value);}
        }
     

        private string _Reason;
        /// <summary>
        /// 付款用途
        /// </summary>
        [AdvQueryAttribute(ColName = "Reason",ColDesc = "付款用途")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Reason",Length=200,IsNullable = true,ColumnDescription = "付款用途" )]
        public string Reason 
        { 
            get{return _Reason;}
            set{SetProperty(ref _Reason, value);}
        }
     

        private decimal _TaxRate= ((0));
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "TaxRate",IsNullable = false,ColumnDescription = "税率" )]
        public decimal TaxRate 
        { 
            get{return _TaxRate;}
            set{SetProperty(ref _TaxRate, value);}
        }
     

        private decimal _TaxAmount= ((0));
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxAmount",ColDesc = "税额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TaxAmount",IsNullable = false,ColumnDescription = "税额" )]
        public decimal TaxAmount 
        { 
            get{return _TaxAmount;}
            set{SetProperty(ref _TaxAmount, value);}
        }
     

        private bool _IsIncludeTax= false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IsIncludeTax",ColDesc = "含税")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsIncludeTax",IsNullable = false,ColumnDescription = "含税" )]
        public bool IsIncludeTax 
        { 
            get{return _IsIncludeTax;}
            set{SetProperty(ref _IsIncludeTax, value);}
        }
     

        private decimal _UntaxedAmont= ((0));
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "UntaxedAmont",ColDesc = "未税本位币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "UntaxedAmont",IsNullable = false,ColumnDescription = "未税本位币" )]
        public decimal UntaxedAmont 
        { 
            get{return _UntaxedAmont;}
            set{SetProperty(ref _UntaxedAmont, value);}
        }
     

        private decimal? _PayableAmount;
        /// <summary>
        /// 应付金额
        /// </summary>
        [AdvQueryAttribute(ColName = "PayableAmount",ColDesc = "应付金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "PayableAmount",IsNullable = true,ColumnDescription = "应付金额" )]
        public decimal? PayableAmount 
        { 
            get{return _PayableAmount;}
            set{SetProperty(ref _PayableAmount, value);}
        }
     

        private decimal? _PaidAmount;
        /// <summary>
        /// 已付金额
        /// </summary>
        [AdvQueryAttribute(ColName = "PaidAmount",ColDesc = "已付金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "PaidAmount",IsNullable = true,ColumnDescription = "已付金额" )]
        public decimal? PaidAmount 
        { 
            get{return _PaidAmount;}
            set{SetProperty(ref _PaidAmount, value);}
        }
     

        private int? _SourceBill_BizType;
        /// <summary>
        /// 来源业务
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBill_BizType",ColDesc = "来源业务")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "SourceBill_BizType",IsNullable = true,ColumnDescription = "来源业务" )]
        public int? SourceBill_BizType 
        { 
            get{return _SourceBill_BizType;}
            set{SetProperty(ref _SourceBill_BizType, value);}
        }
     

        private long? _SourceBill_ID;
        /// <summary>
        /// 来源单据
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBill_ID",ColDesc = "来源单据")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "SourceBill_ID",IsNullable = true,ColumnDescription = "来源单据" )]
        public long? SourceBill_ID 
        { 
            get{return _SourceBill_ID;}
            set{SetProperty(ref _SourceBill_ID, value);}
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
     

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Summary",Length=300,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary 
        { 
            get{return _Summary;}
            set{SetProperty(ref _Summary, value);}
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
     

        private int? _PayStatus;
        /// <summary>
        /// 付款状态
        /// </summary>
        [AdvQueryAttribute(ColName = "PayStatus",ColDesc = "付款状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "PayStatus",IsNullable = true,ColumnDescription = "付款状态" )]
        public int? PayStatus 
        { 
            get{return _PayStatus;}
            set{SetProperty(ref _PayStatus, value);}
        }


       
    }
}



