
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:31
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
    /// 合同明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ContractDetail")]
    public partial class tb_ContractDetailQueryDto:BaseEntityDto
    {
        public tb_ContractDetailQueryDto()
        {

        }

    
     

        private long? _ContractID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ContractID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ContractID",IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_Contract","ContractID")]
        public long? ContractID 
        { 
            get{return _ContractID;}
            set{SetProperty(ref _ContractID, value);}
        }
     

        private long? _ProdDetailID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "货品" )]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{SetProperty(ref _ProdDetailID, value);}
        }
     

        private int? _Qty;
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Qty",ColDesc = "数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Qty",IsNullable = true,ColumnDescription = "数量" )]
        public int? Qty 
        { 
            get{return _Qty;}
            set{SetProperty(ref _Qty, value);}
        }
     

        private decimal? _Price;
        /// <summary>
        /// 售价
        /// </summary>
        [AdvQueryAttribute(ColName = "Price",ColDesc = "售价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Price",IsNullable = true,ColumnDescription = "售价" )]
        public decimal? Price 
        { 
            get{return _Price;}
            set{SetProperty(ref _Price, value);}
        }
     

        private decimal? _Cost;
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Cost",IsNullable = true,ColumnDescription = "成本" )]
        public decimal? Cost 
        { 
            get{return _Cost;}
            set{SetProperty(ref _Cost, value);}
        }
     

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Summary",Length=255,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary 
        { 
            get{return _Summary;}
            set{SetProperty(ref _Summary, value);}
        }
     

        private int _SubtotalQty;
        /// <summary>
        /// 数量小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalQty",ColDesc = "数量小计")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "SubtotalQty",IsNullable = false,ColumnDescription = "数量小计" )]
        public int SubtotalQty 
        { 
            get{return _SubtotalQty;}
            set{SetProperty(ref _SubtotalQty, value);}
        }
     

        private decimal _SubtotalCostAmount;
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalCostAmount",ColDesc = "成本小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalCostAmount",IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal SubtotalCostAmount 
        { 
            get{return _SubtotalCostAmount;}
            set{SetProperty(ref _SubtotalCostAmount, value);}
        }
     

        private decimal _SubtotalPirceAmount;
        /// <summary>
        /// 金额小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalPirceAmount",ColDesc = "金额小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalPirceAmount",IsNullable = false,ColumnDescription = "金额小计" )]
        public decimal SubtotalPirceAmount 
        { 
            get{return _SubtotalPirceAmount;}
            set{SetProperty(ref _SubtotalPirceAmount, value);}
        }


       
    }
}



