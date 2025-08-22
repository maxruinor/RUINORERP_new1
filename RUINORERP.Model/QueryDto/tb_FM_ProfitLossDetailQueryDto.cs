
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:06
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
    /// 损益费用单
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_ProfitLossDetail")]
    public partial class tb_FM_ProfitLossDetailQueryDto:BaseEntityDto
    {
        public tb_FM_ProfitLossDetailQueryDto()
        {

        }

    
     

        private long? _ProfitLossId;
        /// <summary>
        /// 损益费用单
        /// </summary>
        [AdvQueryAttribute(ColName = "ProfitLossId",ColDesc = "损益费用单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProfitLossId",IsNullable = true,ColumnDescription = "损益费用单" )]
        [FKRelationAttribute("tb_FM_ProfitLoss","ProfitLossId")]
        public long? ProfitLossId 
        { 
            get{return _ProfitLossId;}
            set{SetProperty(ref _ProfitLossId, value);}
        }
     

        private long? _ProdDetailID;
        /// <summary>
        /// 产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "产品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{SetProperty(ref _ProdDetailID, value);}
        }
     

        private string _property;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "property",Length=255,IsNullable = true,ColumnDescription = "属性" )]
        public string property 
        { 
            get{return _property;}
            set{SetProperty(ref _property, value);}
        }
     

        private long? _ExpenseType_id;
        /// <summary>
        /// 费用类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpenseType_id",ColDesc = "费用类型")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ExpenseType_id",IsNullable = true,ColumnDescription = "费用类型" )]
        [FKRelationAttribute("tb_FM_ExpenseType","ExpenseType_id")]
        public long? ExpenseType_id 
        { 
            get{return _ExpenseType_id;}
            set{SetProperty(ref _ExpenseType_id, value);}
        }
     

        private string _ExpenseDescription;
        /// <summary>
        /// 费用说明
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpenseDescription",ColDesc = "费用说明")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ExpenseDescription",Length=300,IsNullable = true,ColumnDescription = "费用说明" )]
        public string ExpenseDescription 
        { 
            get{return _ExpenseDescription;}
            set{SetProperty(ref _ExpenseDescription, value);}
        }
     

        private decimal? _UnitPrice;
        /// <summary>
        /// 单价
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitPrice",ColDesc = "单价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "UnitPrice",IsNullable = true,ColumnDescription = "单价" )]
        public decimal? UnitPrice 
        { 
            get{return _UnitPrice;}
            set{SetProperty(ref _UnitPrice, value);}
        }
     

        private decimal? _Quantity;
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Quantity",IsNullable = true,ColumnDescription = "数量" )]
        public decimal? Quantity 
        { 
            get{return _Quantity;}
            set{SetProperty(ref _Quantity, value);}
        }
     

        private decimal _SubtotalAmont= ((0));
        /// <summary>
        /// 金额小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalAmont",ColDesc = "金额小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalAmont",IsNullable = false,ColumnDescription = "金额小计" )]
        public decimal SubtotalAmont 
        { 
            get{return _SubtotalAmont;}
            set{SetProperty(ref _SubtotalAmont, value);}
        }
     

        private decimal _UntaxedSubtotalAmont= ((0));
        /// <summary>
        /// 未税小计
        /// </summary>
        [AdvQueryAttribute(ColName = "UntaxedSubtotalAmont",ColDesc = "未税小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "UntaxedSubtotalAmont",IsNullable = false,ColumnDescription = "未税小计" )]
        public decimal UntaxedSubtotalAmont 
        { 
            get{return _UntaxedSubtotalAmont;}
            set{SetProperty(ref _UntaxedSubtotalAmont, value);}
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
     

        private decimal _TaxSubtotalAmont= ((0));
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxSubtotalAmont",ColDesc = "税额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TaxSubtotalAmont",IsNullable = false,ColumnDescription = "税额" )]
        public decimal TaxSubtotalAmont 
        { 
            get{return _TaxSubtotalAmont;}
            set{SetProperty(ref _TaxSubtotalAmont, value);}
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



