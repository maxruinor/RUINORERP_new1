
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:32
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
    /// 预付款表明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_PrePaymentBillDetail")]
    public partial class tb_FM_PrePaymentBillDetailQueryDto:BaseEntityDto
    {
        public tb_FM_PrePaymentBillDetailQueryDto()
        {

        }

    
     

        private long? _PrePaymentBill_id;
        /// <summary>
        /// 预收付单
        /// </summary>
        [AdvQueryAttribute(ColName = "PrePaymentBill_id",ColDesc = "预收付单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PrePaymentBill_id",IsNullable = true,ColumnDescription = "预收付单" )]
        [FKRelationAttribute("tb_FM_PrePaymentBill","PrePaymentBill_id")]
        public long? PrePaymentBill_id 
        { 
            get{return _PrePaymentBill_id;}
            set{SetProperty(ref _PrePaymentBill_id, value);}
        }
     

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 厂商
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "厂商")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CustomerVendor_ID",IsNullable = true,ColumnDescription = "厂商" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{SetProperty(ref _CustomerVendor_ID, value);}
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
     

        private long? _account_id;
        /// <summary>
        /// 账户
        /// </summary>
        [AdvQueryAttribute(ColName = "account_id",ColDesc = "账户")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "account_id",IsNullable = true,ColumnDescription = "账户" )]
        [FKRelationAttribute("tb_FM_Account","account_id")]
        public long? account_id 
        { 
            get{return _account_id;}
            set{SetProperty(ref _account_id, value);}
        }
     

        private string _PaymentReason;
        /// <summary>
        /// 事由
        /// </summary>
        [AdvQueryAttribute(ColName = "PaymentReason",ColDesc = "事由")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PaymentReason",Length=50,IsNullable = true,ColumnDescription = "事由" )]
        public string PaymentReason 
        { 
            get{return _PaymentReason;}
            set{SetProperty(ref _PaymentReason, value);}
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
     

        private string _Reason;
        /// <summary>
        /// 原因
        /// </summary>
        [AdvQueryAttribute(ColName = "Reason",ColDesc = "原因")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Reason",Length=50,IsNullable = true,ColumnDescription = "原因" )]
        public string Reason 
        { 
            get{return _Reason;}
            set{SetProperty(ref _Reason, value);}
        }
     

        private string _OffsetMethod;
        /// <summary>
        /// 冲销方式
        /// </summary>
        [AdvQueryAttribute(ColName = "OffsetMethod",ColDesc = "冲销方式")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "OffsetMethod",Length=50,IsNullable = true,ColumnDescription = "冲销方式" )]
        public string OffsetMethod 
        { 
            get{return _OffsetMethod;}
            set{SetProperty(ref _OffsetMethod, value);}
        }
     

        private decimal? _Amount;
        /// <summary>
        /// 金额
        /// </summary>
        [AdvQueryAttribute(ColName = "Amount",ColDesc = "金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Amount",IsNullable = true,ColumnDescription = "金额" )]
        public decimal? Amount 
        { 
            get{return _Amount;}
            set{SetProperty(ref _Amount, value);}
        }
     

        private decimal? _PrepaidAmount;
        /// <summary>
        /// 已转金额
        /// </summary>
        [AdvQueryAttribute(ColName = "PrepaidAmount",ColDesc = "已转金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "PrepaidAmount",IsNullable = true,ColumnDescription = "已转金额" )]
        public decimal? PrepaidAmount 
        { 
            get{return _PrepaidAmount;}
            set{SetProperty(ref _PrepaidAmount, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=30,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



