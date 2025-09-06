
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/06/2025 15:41:53
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
    /// 应收应付明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FM_ReceivablePayableDetail")]
    public partial class tb_FM_ReceivablePayableDetailQueryDto:BaseEntityDto
    {
        public tb_FM_ReceivablePayableDetailQueryDto()
        {

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
     

        private bool _IncludeTax= false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IncludeTax",ColDesc = "含税")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IncludeTax",IsNullable = false,ColumnDescription = "含税" )]
        public bool IncludeTax 
        { 
            get{return _IncludeTax;}
            set{SetProperty(ref _IncludeTax, value);}
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
     

        private decimal _TaxLocalAmount= ((0));
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxLocalAmount",ColDesc = "税额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TaxLocalAmount",IsNullable = false,ColumnDescription = "税额" )]
        public decimal TaxLocalAmount 
        { 
            get{return _TaxLocalAmount;}
            set{SetProperty(ref _TaxLocalAmount, value);}
        }
     

        private decimal _LocalPayableAmount= ((0));
        /// <summary>
        /// 金额小计
        /// </summary>
        [AdvQueryAttribute(ColName = "LocalPayableAmount",ColDesc = "金额小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "LocalPayableAmount",IsNullable = false,ColumnDescription = "金额小计" )]
        public decimal LocalPayableAmount 
        { 
            get{return _LocalPayableAmount;}
            set{SetProperty(ref _LocalPayableAmount, value);}
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



