
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 14:22:22
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
    /// 发票明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_InvoiceDetail")]
    public partial class tb_FM_InvoiceDetailQueryDto:BaseEntityDto
    {
        public tb_FM_InvoiceDetailQueryDto()
        {

        }

    
     

        private long? _InvoiceId;
        /// <summary>
        /// 发票
        /// </summary>
        [AdvQueryAttribute(ColName = "InvoiceId",ColDesc = "发票")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "InvoiceId",IsNullable = true,ColumnDescription = "发票" )]
        [FKRelationAttribute("tb_FM_Invoice","InvoiceId")]
        public long? InvoiceId 
        { 
            get{return _InvoiceId;}
            set{SetProperty(ref _InvoiceId, value);}
        }
     

        private int? _SourceBizType;
        /// <summary>
        /// 来源业务
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBizType",ColDesc = "来源业务")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "SourceBizType",IsNullable = true,ColumnDescription = "来源业务" )]
        public int? SourceBizType 
        { 
            get{return _SourceBizType;}
            set{SetProperty(ref _SourceBizType, value);}
        }
     

        private long? _SourceBillId;
        /// <summary>
        /// 来源单据
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillId",ColDesc = "来源单据")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "SourceBillId",IsNullable = true,ColumnDescription = "来源单据" )]
        public long? SourceBillId 
        { 
            get{return _SourceBillId;}
            set{SetProperty(ref _SourceBillId, value);}
        }
     

        private string _SourceBillNo;
        /// <summary>
        /// 来源单号
        /// </summary>
        [AdvQueryAttribute(ColName = "SourceBillNo",ColDesc = "来源单号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "SourceBillNo",Length=30,IsNullable = true,ColumnDescription = "来源单号" )]
        public string SourceBillNo 
        { 
            get{return _SourceBillNo;}
            set{SetProperty(ref _SourceBillNo, value);}
        }
     

        private int? _ItemType;
        /// <summary>
        /// 项目类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ItemType",ColDesc = "项目类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ItemType",IsNullable = true,ColumnDescription = "项目类型" )]
        public int? ItemType 
        { 
            get{return _ItemType;}
            set{SetProperty(ref _ItemType, value);}
        }
     

        private long? _ProdDetailID;
        /// <summary>
        /// 产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "产品" )]
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
        public long? Unit_ID 
        { 
            get{return _Unit_ID;}
            set{SetProperty(ref _Unit_ID, value);}
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
     

        private decimal _TaxSubtotalAmount= ((0));
        /// <summary>
        /// 明细税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxSubtotalAmount",ColDesc = "明细税额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TaxSubtotalAmount",IsNullable = false,ColumnDescription = "明细税额" )]
        public decimal TaxSubtotalAmount 
        { 
            get{return _TaxSubtotalAmount;}
            set{SetProperty(ref _TaxSubtotalAmount, value);}
        }
     

        private decimal _SubtotalAmount= ((0));
        /// <summary>
        /// 明细金额
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalAmount",ColDesc = "明细金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalAmount",IsNullable = false,ColumnDescription = "明细金额" )]
        public decimal SubtotalAmount 
        { 
            get{return _SubtotalAmount;}
            set{SetProperty(ref _SubtotalAmount, value);}
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



