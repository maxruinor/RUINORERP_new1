
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/11/2025 15:24:56
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
     

        private long _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Location_ID",IsNullable = false,ColumnDescription = "库位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID 
        { 
            get{return _Location_ID;}
            set{SetProperty(ref _Location_ID, value);}
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
     

        private decimal _Original_UnitPrice_NoTax= ((0));
        /// <summary>
        /// 原未税单价
        /// </summary>
        [AdvQueryAttribute(ColName = "Original_UnitPrice_NoTax",ColDesc = "原未税单价")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Original_UnitPrice_NoTax",IsNullable = false,ColumnDescription = "原未税单价" )]
        public decimal Original_UnitPrice_NoTax 
        { 
            get{return _Original_UnitPrice_NoTax;}
            set{SetProperty(ref _Original_UnitPrice_NoTax, value);}
        }
     

        private decimal _Correct_UnitPrice_NoTax= ((0));
        /// <summary>
        /// 新未税单价
        /// </summary>
        [AdvQueryAttribute(ColName = "Correct_UnitPrice_NoTax",ColDesc = "新未税单价")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Correct_UnitPrice_NoTax",IsNullable = false,ColumnDescription = "新未税单价" )]
        public decimal Correct_UnitPrice_NoTax 
        { 
            get{return _Correct_UnitPrice_NoTax;}
            set{SetProperty(ref _Correct_UnitPrice_NoTax, value);}
        }
     

        private decimal _Original_TaxRate= ((0));
        /// <summary>
        /// 原税率
        /// </summary>
        [AdvQueryAttribute(ColName = "Original_TaxRate",ColDesc = "原税率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Original_TaxRate",IsNullable = false,ColumnDescription = "原税率" )]
        public decimal Original_TaxRate 
        { 
            get{return _Original_TaxRate;}
            set{SetProperty(ref _Original_TaxRate, value);}
        }
     

        private decimal _Correct_TaxRate= ((0));
        /// <summary>
        /// 新税率
        /// </summary>
        [AdvQueryAttribute(ColName = "Correct_TaxRate",ColDesc = "新税率")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Correct_TaxRate",IsNullable = false,ColumnDescription = "新税率" )]
        public decimal Correct_TaxRate 
        { 
            get{return _Correct_TaxRate;}
            set{SetProperty(ref _Correct_TaxRate, value);}
        }
     

        private decimal _Original_UnitPrice_WithTax= ((0));
        /// <summary>
        /// 原含税单价
        /// </summary>
        [AdvQueryAttribute(ColName = "Original_UnitPrice_WithTax",ColDesc = "原含税单价")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Original_UnitPrice_WithTax",IsNullable = false,ColumnDescription = "原含税单价" )]
        public decimal Original_UnitPrice_WithTax 
        { 
            get{return _Original_UnitPrice_WithTax;}
            set{SetProperty(ref _Original_UnitPrice_WithTax, value);}
        }
     

        private decimal _Correct_UnitPrice_WithTax= ((0));
        /// <summary>
        /// 新含税单价
        /// </summary>
        [AdvQueryAttribute(ColName = "Correct_UnitPrice_WithTax",ColDesc = "新含税单价")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Correct_UnitPrice_WithTax",IsNullable = false,ColumnDescription = "新含税单价" )]
        public decimal Correct_UnitPrice_WithTax 
        { 
            get{return _Correct_UnitPrice_WithTax;}
            set{SetProperty(ref _Correct_UnitPrice_WithTax, value);}
        }
     

        private decimal _UnitPrice_NoTax_Diff= ((0));
        /// <summary>
        /// 未税单价差异
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitPrice_NoTax_Diff",ColDesc = "未税单价差异")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "UnitPrice_NoTax_Diff",IsNullable = false,ColumnDescription = "未税单价差异" )]
        public decimal UnitPrice_NoTax_Diff 
        { 
            get{return _UnitPrice_NoTax_Diff;}
            set{SetProperty(ref _UnitPrice_NoTax_Diff, value);}
        }
     

        private decimal _UnitPrice_WithTax_Diff= ((0));
        /// <summary>
        /// 含税单价差异
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitPrice_WithTax_Diff",ColDesc = "含税单价差异")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "UnitPrice_WithTax_Diff",IsNullable = false,ColumnDescription = "含税单价差异" )]
        public decimal UnitPrice_WithTax_Diff 
        { 
            get{return _UnitPrice_WithTax_Diff;}
            set{SetProperty(ref _UnitPrice_WithTax_Diff, value);}
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
     

        private decimal _Original_TaxAmount= ((0));
        /// <summary>
        /// 原始税额
        /// </summary>
        [AdvQueryAttribute(ColName = "Original_TaxAmount",ColDesc = "原始税额")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Original_TaxAmount",IsNullable = false,ColumnDescription = "原始税额" )]
        public decimal Original_TaxAmount 
        { 
            get{return _Original_TaxAmount;}
            set{SetProperty(ref _Original_TaxAmount, value);}
        }
     

        private decimal _Correct_TaxAmount= ((0));
        /// <summary>
        /// 新调税额
        /// </summary>
        [AdvQueryAttribute(ColName = "Correct_TaxAmount",ColDesc = "新调税额")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Correct_TaxAmount",IsNullable = false,ColumnDescription = "新调税额" )]
        public decimal Correct_TaxAmount 
        { 
            get{return _Correct_TaxAmount;}
            set{SetProperty(ref _Correct_TaxAmount, value);}
        }
     

        private decimal _TaxAmount_Diff= ((0));
        /// <summary>
        /// 税额差异
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxAmount_Diff",ColDesc = "税额差异")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "TaxAmount_Diff",IsNullable = false,ColumnDescription = "税额差异" )]
        public decimal TaxAmount_Diff 
        { 
            get{return _TaxAmount_Diff;}
            set{SetProperty(ref _TaxAmount_Diff, value);}
        }
     

        private decimal _TotalAmount_Diff_NoTax= ((0));
        /// <summary>
        /// 总未税差异金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount_Diff_NoTax",ColDesc = "总未税差异金额")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "TotalAmount_Diff_NoTax",IsNullable = false,ColumnDescription = "总未税差异金额" )]
        public decimal TotalAmount_Diff_NoTax 
        { 
            get{return _TotalAmount_Diff_NoTax;}
            set{SetProperty(ref _TotalAmount_Diff_NoTax, value);}
        }
     

        private decimal _TotalAmount_Diff_WithTax= ((0));
        /// <summary>
        /// 总含税差异金额价
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount_Diff_WithTax",ColDesc = "总含税差异金额价")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "TotalAmount_Diff_WithTax",IsNullable = false,ColumnDescription = "总含税差异金额价" )]
        public decimal TotalAmount_Diff_WithTax 
        { 
            get{return _TotalAmount_Diff_WithTax;}
            set{SetProperty(ref _TotalAmount_Diff_WithTax, value);}
        }
     

        private decimal _TotalAmount_Diff= ((0));
        /// <summary>
        /// 总差异金额
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount_Diff",ColDesc = "总差异金额")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "TotalAmount_Diff",IsNullable = false,ColumnDescription = "总差异金额" )]
        public decimal TotalAmount_Diff 
        { 
            get{return _TotalAmount_Diff;}
            set{SetProperty(ref _TotalAmount_Diff, value);}
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
     

        private string _AdjustReason;
        /// <summary>
        /// 调整原因
        /// </summary>
        [AdvQueryAttribute(ColName = "AdjustReason",ColDesc = "调整原因")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "AdjustReason",Length=500,IsNullable = true,ColumnDescription = "调整原因" )]
        public string AdjustReason 
        { 
            get{return _AdjustReason;}
            set{SetProperty(ref _AdjustReason, value);}
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



