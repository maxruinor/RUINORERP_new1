
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/24/2025 18:44:35
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
    /// 价格调整单明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_PriceAdjustmentDetail")]
    public partial class tb_FM_PriceAdjustmentDetailQueryDto:BaseEntityDto
    {
        public tb_FM_PriceAdjustmentDetailQueryDto()
        {

        }

    
     

        private long? _AdjustId;
        /// <summary>
        /// 价格调整单
        /// </summary>
        [AdvQueryAttribute(ColName = "AdjustId",ColDesc = "价格调整单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "AdjustId",IsNullable = true,ColumnDescription = "价格调整单" )]
        [FKRelationAttribute("tb_FM_PriceAdjustment","AdjustId")]
        public long? AdjustId 
        { 
            get{return _AdjustId;}
            set{SetProperty(ref _AdjustId, value);}
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
     

        private string _Specifications;
        /// <summary>
        /// 规格
        /// </summary>
        [AdvQueryAttribute(ColName = "Specifications",ColDesc = "规格")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Specifications",Length=1000,IsNullable = true,ColumnDescription = "规格" )]
        public string Specifications 
        { 
            get{return _Specifications;}
            set{SetProperty(ref _Specifications, value);}
        }
     

        private long? _Unit_ID;
        /// <summary>
        /// 单位
        /// </summary>
        [AdvQueryAttribute(ColName = "Unit_ID",ColDesc = "单位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Unit_ID",IsNullable = true,ColumnDescription = "单位" )]
        [FKRelationAttribute("tb_Unit","Unit_ID")]
        public long? Unit_ID 
        { 
            get{return _Unit_ID;}
            set{SetProperty(ref _Unit_ID, value);}
        }
     

        private decimal? _ExchangeRate= ((1));
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
     

        private decimal _OriginalUnitPrice= ((0));
        /// <summary>
        /// 原始单价
        /// </summary>
        [AdvQueryAttribute(ColName = "OriginalUnitPrice",ColDesc = "原始单价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "OriginalUnitPrice",IsNullable = false,ColumnDescription = "原始单价" )]
        public decimal OriginalUnitPrice 
        { 
            get{return _OriginalUnitPrice;}
            set{SetProperty(ref _OriginalUnitPrice, value);}
        }
     

        private decimal _AdjustedUnitPrice= ((0));
        /// <summary>
        /// 调整后单价
        /// </summary>
        [AdvQueryAttribute(ColName = "AdjustedUnitPrice",ColDesc = "调整后单价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "AdjustedUnitPrice",IsNullable = false,ColumnDescription = "调整后单价" )]
        public decimal AdjustedUnitPrice 
        { 
            get{return _AdjustedUnitPrice;}
            set{SetProperty(ref _AdjustedUnitPrice, value);}
        }
     

        private decimal _DiffUnitPrice= ((0));
        /// <summary>
        /// 差异单价
        /// </summary>
        [AdvQueryAttribute(ColName = "DiffUnitPrice",ColDesc = "差异单价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "DiffUnitPrice",IsNullable = false,ColumnDescription = "差异单价" )]
        public decimal DiffUnitPrice 
        { 
            get{return _DiffUnitPrice;}
            set{SetProperty(ref _DiffUnitPrice, value);}
        }
     

        private decimal _Quantity= ((0));
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Quantity",IsNullable = false,ColumnDescription = "数量" )]
        public decimal Quantity 
        { 
            get{return _Quantity;}
            set{SetProperty(ref _Quantity, value);}
        }
     

        private string _CustomerPartNo;
        /// <summary>
        /// 往来单位料号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerPartNo",ColDesc = "往来单位料号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CustomerPartNo",Length=100,IsNullable = true,ColumnDescription = "往来单位料号" )]
        public string CustomerPartNo 
        { 
            get{return _CustomerPartNo;}
            set{SetProperty(ref _CustomerPartNo, value);}
        }
     

        private decimal _SubtotalDiffLocalAmount= ((0));
        /// <summary>
        /// 差异金额小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalDiffLocalAmount",ColDesc = "差异金额小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalDiffLocalAmount",IsNullable = false,ColumnDescription = "差异金额小计" )]
        public decimal SubtotalDiffLocalAmount 
        { 
            get{return _SubtotalDiffLocalAmount;}
            set{SetProperty(ref _SubtotalDiffLocalAmount, value);}
        }
     

        private string _Description;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Description",Length=300,IsNullable = true,ColumnDescription = "描述" )]
        public string Description 
        { 
            get{return _Description;}
            set{SetProperty(ref _Description, value);}
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
     

        private decimal _TaxDiffLocalAmount= ((0));
        /// <summary>
        /// 税额差异
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxDiffLocalAmount",ColDesc = "税额差异")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TaxDiffLocalAmount",IsNullable = false,ColumnDescription = "税额差异" )]
        public decimal TaxDiffLocalAmount 
        { 
            get{return _TaxDiffLocalAmount;}
            set{SetProperty(ref _TaxDiffLocalAmount, value);}
        }
     

        private decimal _TaxSubtotalDiffLocalAmount= ((0));
        /// <summary>
        /// 税额差异小计
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxSubtotalDiffLocalAmount",ColDesc = "税额差异小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TaxSubtotalDiffLocalAmount",IsNullable = false,ColumnDescription = "税额差异小计" )]
        public decimal TaxSubtotalDiffLocalAmount 
        { 
            get{return _TaxSubtotalDiffLocalAmount;}
            set{SetProperty(ref _TaxSubtotalDiffLocalAmount, value);}
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



