
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:27:24
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
    /// 对账单明细（关联应收单） 
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_StatementDetail")]
    public partial class tb_FM_StatementDetailQueryDto:BaseEntityDto
    {
        public tb_FM_StatementDetailQueryDto()
        {

        }

    
     

        private long _StatementId;
        /// <summary>
        /// 对账单
        /// </summary>
        [AdvQueryAttribute(ColName = "StatementId",ColDesc = "对账单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "StatementId",IsNullable = false,ColumnDescription = "对账单" )]
        [FKRelationAttribute("tb_FM_Statement","StatementId")]
        public long StatementId 
        { 
            get{return _StatementId;}
            set{SetProperty(ref _StatementId, value);}
        }
     

        private long? _ARAPId;
        /// <summary>
        /// 应收付款单
        /// </summary>
        [AdvQueryAttribute(ColName = "ARAPId",ColDesc = "应收付款单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ARAPId",IsNullable = true,ColumnDescription = "应收付款单" )]
        [FKRelationAttribute("tb_FM_ReceivablePayable","ARAPId")]
        public long? ARAPId 
        { 
            get{return _ARAPId;}
            set{SetProperty(ref _ARAPId, value);}
        }
     

        private long _Currency_ID;
        /// <summary>
        /// 币别
        /// </summary>
        [AdvQueryAttribute(ColName = "Currency_ID",ColDesc = "币别")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Currency_ID",IsNullable = false,ColumnDescription = "币别" )]
        [FKRelationAttribute("tb_Currency","Currency_ID")]
        public long Currency_ID 
        { 
            get{return _Currency_ID;}
            set{SetProperty(ref _Currency_ID, value);}
        }
     

        private decimal _ExchangeRate= ((1));
        /// <summary>
        /// 汇率
        /// </summary>
        [AdvQueryAttribute(ColName = "ExchangeRate",ColDesc = "汇率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "ExchangeRate",IsNullable = false,ColumnDescription = "汇率" )]
        public decimal ExchangeRate 
        { 
            get{return _ExchangeRate;}
            set{SetProperty(ref _ExchangeRate, value);}
        }
     

        private decimal _IncludedLocalAmount= ((0));
        /// <summary>
        /// 对账金额本币
        /// </summary>
        [AdvQueryAttribute(ColName = "IncludedLocalAmount",ColDesc = "对账金额本币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "IncludedLocalAmount",IsNullable = false,ColumnDescription = "对账金额本币" )]
        public decimal IncludedLocalAmount 
        { 
            get{return _IncludedLocalAmount;}
            set{SetProperty(ref _IncludedLocalAmount, value);}
        }
     

        private decimal _IncludedForeignAmount= ((0));
        /// <summary>
        /// 对账金额外币
        /// </summary>
        [AdvQueryAttribute(ColName = "IncludedForeignAmount",ColDesc = "对账金额外币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "IncludedForeignAmount",IsNullable = false,ColumnDescription = "对账金额外币" )]
        public decimal IncludedForeignAmount 
        { 
            get{return _IncludedForeignAmount;}
            set{SetProperty(ref _IncludedForeignAmount, value);}
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


       
    }
}



