
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:49:00
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
    /// 销售出库明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_SaleOutDetail")]
    public partial class tb_SaleOutDetailQueryDto:BaseEntityDto
    {
        public tb_SaleOutDetailQueryDto()
        {

        }

    
     

        private long _ProdDetailID;
        /// <summary>
        /// 产品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "产品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID 
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
     

        private long? _Rack_ID;
        /// <summary>
        /// 货架
        /// </summary>
        [AdvQueryAttribute(ColName = "Rack_ID",ColDesc = "货架")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Rack_ID",IsNullable = true,ColumnDescription = "货架" )]
        [FKRelationAttribute("tb_StorageRack","Rack_ID")]
        public long? Rack_ID 
        { 
            get{return _Rack_ID;}
            set{SetProperty(ref _Rack_ID, value);}
        }
     

        private long _SaleOut_MainID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleOut_MainID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "SaleOut_MainID",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_SaleOut","SaleOut_MainID")]
        public long SaleOut_MainID 
        { 
            get{return _SaleOut_MainID;}
            set{SetProperty(ref _SaleOut_MainID, value);}
        }
     

        private int _Quantity= ((0));
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Quantity",IsNullable = false,ColumnDescription = "数量" )]
        public int Quantity 
        { 
            get{return _Quantity;}
            set{SetProperty(ref _Quantity, value);}
        }
     

        private decimal _TransactionPrice= ((0));
        /// <summary>
        /// 成交单价
        /// </summary>
        [AdvQueryAttribute(ColName = "TransactionPrice",ColDesc = "成交单价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TransactionPrice",IsNullable = false,ColumnDescription = "成交单价" )]
        public decimal TransactionPrice 
        { 
            get{return _TransactionPrice;}
            set{SetProperty(ref _TransactionPrice, value);}
        }
     

        private decimal _SubtotalAmount= ((0));
        /// <summary>
        /// 成交小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalAmount",ColDesc = "成交小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalAmount",IsNullable = false,ColumnDescription = "成交小计" )]
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
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Summary",Length=255,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary 
        { 
            get{return _Summary;}
            set{SetProperty(ref _Summary, value);}
        }
     

        private string _CustomerPartNo;
        /// <summary>
        /// 客户型号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerPartNo",ColDesc = "客户型号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CustomerPartNo",Length=50,IsNullable = true,ColumnDescription = "客户型号" )]
        public string CustomerPartNo 
        { 
            get{return _CustomerPartNo;}
            set{SetProperty(ref _CustomerPartNo, value);}
        }
     

        private decimal _Cost;
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Cost",IsNullable = false,ColumnDescription = "成本" )]
        public decimal Cost 
        { 
            get{return _Cost;}
            set{SetProperty(ref _Cost, value);}
        }
     

        private decimal _TotalCostAmount;
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalCostAmount",ColDesc = "成本小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalCostAmount",IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal TotalCostAmount 
        { 
            get{return _TotalCostAmount;}
            set{SetProperty(ref _TotalCostAmount, value);}
        }
     

        private decimal _TaxRate;
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
     

        private int _OrderTotalOutQty= ((0));
        /// <summary>
        /// 订单累计数
        /// </summary>
        [AdvQueryAttribute(ColName = "OrderTotalOutQty",ColDesc = "订单累计数")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "OrderTotalOutQty",IsNullable = false,ColumnDescription = "订单累计数" )]
        public int OrderTotalOutQty 
        { 
            get{return _OrderTotalOutQty;}
            set{SetProperty(ref _OrderTotalOutQty, value);}
        }
     

        private int _OrderReturnTotalQty= ((0));
        /// <summary>
        /// 订单退回数
        /// </summary>
        [AdvQueryAttribute(ColName = "OrderReturnTotalQty",ColDesc = "订单退回数")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "OrderReturnTotalQty",IsNullable = false,ColumnDescription = "订单退回数" )]
        public int OrderReturnTotalQty 
        { 
            get{return _OrderReturnTotalQty;}
            set{SetProperty(ref _OrderReturnTotalQty, value);}
        }
     

        private bool? _Gift= false;
        /// <summary>
        /// 赠品
        /// </summary>
        [AdvQueryAttribute(ColName = "Gift",ColDesc = "赠品")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Gift",IsNullable = true,ColumnDescription = "赠品" )]
        public bool? Gift 
        { 
            get{return _Gift;}
            set{SetProperty(ref _Gift, value);}
        }
     

        private bool _IncludingTax= false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IncludingTax",ColDesc = "含税")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IncludingTax",IsNullable = false,ColumnDescription = "含税" )]
        public bool IncludingTax 
        { 
            get{return _IncludingTax;}
            set{SetProperty(ref _IncludingTax, value);}
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
     

        private decimal? _Discount;
        /// <summary>
        /// 折扣
        /// </summary>
        [AdvQueryAttribute(ColName = "Discount",ColDesc = "折扣")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Discount",IsNullable = true,ColumnDescription = "折扣" )]
        public decimal? Discount 
        { 
            get{return _Discount;}
            set{SetProperty(ref _Discount, value);}
        }
     

        private decimal? _UntaxedAmount;
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "UntaxedAmount",ColDesc = "未税本位币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "UntaxedAmount",IsNullable = true,ColumnDescription = "未税本位币" )]
        public decimal? UntaxedAmount 
        { 
            get{return _UntaxedAmount;}
            set{SetProperty(ref _UntaxedAmount, value);}
        }
     

        private decimal? _CommissionAmount;
        /// <summary>
        /// 抽成金额
        /// </summary>
        [AdvQueryAttribute(ColName = "CommissionAmount",ColDesc = "抽成金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "CommissionAmount",IsNullable = true,ColumnDescription = "抽成金额" )]
        public decimal? CommissionAmount 
        { 
            get{return _CommissionAmount;}
            set{SetProperty(ref _CommissionAmount, value);}
        }
     

        private int _DeliveredQty;
        /// <summary>
        /// 抽成金已出库数量额
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveredQty",ColDesc = "抽成金已出库数量额")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "DeliveredQty",IsNullable = false,ColumnDescription = "抽成金已出库数量额" )]
        public int DeliveredQty 
        { 
            get{return _DeliveredQty;}
            set{SetProperty(ref _DeliveredQty, value);}
        }
     

        private decimal? _TaxSubtotalAmount;
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxSubtotalAmount",ColDesc = "税额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TaxSubtotalAmount",IsNullable = true,ColumnDescription = "税额" )]
        public decimal? TaxSubtotalAmount 
        { 
            get{return _TaxSubtotalAmount;}
            set{SetProperty(ref _TaxSubtotalAmount, value);}
        }


       
    }
}



