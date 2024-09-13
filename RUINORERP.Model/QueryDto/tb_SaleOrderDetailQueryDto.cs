
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:28
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
    /// 销售订单明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_SaleOrderDetail")]
    public partial class tb_SaleOrderDetailQueryDto:BaseEntityDto
    {
        public tb_SaleOrderDetailQueryDto()
        {

        }

    
     

        private long _SOrder_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "SOrder_ID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "SOrder_ID",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_SaleOrder","SOrder_ID")]
        public long SOrder_ID 
        { 
            get{return _SOrder_ID;}
            set{SetProperty(ref _SOrder_ID, value);}
        }
     

        private long _ProdDetailID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "货品" )]
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
     

        private decimal _UnitPrice= ((0));
        /// <summary>
        /// 单价
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitPrice",ColDesc = "单价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "UnitPrice",IsNullable = false,ColumnDescription = "单价" )]
        public decimal UnitPrice 
        { 
            get{return _UnitPrice;}
            set{SetProperty(ref _UnitPrice, value);}
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
     

        private decimal _Discount= ((1));
        /// <summary>
        /// 折扣
        /// </summary>
        [AdvQueryAttribute(ColName = "Discount",ColDesc = "折扣")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Discount",IsNullable = false,ColumnDescription = "折扣" )]
        public decimal Discount 
        { 
            get{return _Discount;}
            set{SetProperty(ref _Discount, value);}
        }
     

        private decimal _TransactionPrice= ((0));
        /// <summary>
        /// 成交价
        /// </summary>
        [AdvQueryAttribute(ColName = "TransactionPrice",ColDesc = "成交价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TransactionPrice",IsNullable = false,ColumnDescription = "成交价" )]
        public decimal TransactionPrice 
        { 
            get{return _TransactionPrice;}
            set{SetProperty(ref _TransactionPrice, value);}
        }
     

        private decimal _SubtotalTransAmount= ((0));
        /// <summary>
        /// 成交小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTransAmount",ColDesc = "成交小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalTransAmount",IsNullable = false,ColumnDescription = "成交小计" )]
        public decimal SubtotalTransAmount 
        { 
            get{return _SubtotalTransAmount;}
            set{SetProperty(ref _SubtotalTransAmount, value);}
        }
     

        private decimal _Cost= ((0));
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
     

        private decimal _SubtotalCostAmount= ((0));
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
     

        private int _TotalDeliveredQty= ((0));
        /// <summary>
        /// 订单出库数
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalDeliveredQty",ColDesc = "订单出库数")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "TotalDeliveredQty",IsNullable = false,ColumnDescription = "订单出库数" )]
        public int TotalDeliveredQty 
        { 
            get{return _TotalDeliveredQty;}
            set{SetProperty(ref _TotalDeliveredQty, value);}
        }
     

        private decimal _CommissionAmount= ((0));
        /// <summary>
        /// 抽成金额
        /// </summary>
        [AdvQueryAttribute(ColName = "CommissionAmount",ColDesc = "抽成金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "CommissionAmount",IsNullable = false,ColumnDescription = "抽成金额" )]
        public decimal CommissionAmount 
        { 
            get{return _CommissionAmount;}
            set{SetProperty(ref _CommissionAmount, value);}
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
     

        private decimal _SubtotalTaxAmount= ((0));
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTaxAmount",ColDesc = "税额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalTaxAmount",IsNullable = false,ColumnDescription = "税额" )]
        public decimal SubtotalTaxAmount 
        { 
            get{return _SubtotalTaxAmount;}
            set{SetProperty(ref _SubtotalTaxAmount, value);}
        }
     

        private decimal _SubtotalUntaxedAmount= ((0));
        /// <summary>
        /// 未税本位币
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalUntaxedAmount",ColDesc = "未税本位币")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalUntaxedAmount",IsNullable = false,ColumnDescription = "未税本位币" )]
        public decimal SubtotalUntaxedAmount 
        { 
            get{return _SubtotalUntaxedAmount;}
            set{SetProperty(ref _SubtotalUntaxedAmount, value);}
        }
     

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Summary",Length=1000,IsNullable = true,ColumnDescription = "摘要" )]
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
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CustomerPartNo",Length=100,IsNullable = true,ColumnDescription = "客户型号" )]
        public string CustomerPartNo 
        { 
            get{return _CustomerPartNo;}
            set{SetProperty(ref _CustomerPartNo, value);}
        }
     

        private bool _Gift= false;
        /// <summary>
        /// 赠品
        /// </summary>
        [AdvQueryAttribute(ColName = "Gift",ColDesc = "赠品")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Gift",IsNullable = false,ColumnDescription = "赠品" )]
        public bool Gift 
        { 
            get{return _Gift;}
            set{SetProperty(ref _Gift, value);}
        }
     

        private int _TotalReturnedQty= ((0));
        /// <summary>
        /// 订单退回数
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalReturnedQty",ColDesc = "订单退回数")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "TotalReturnedQty",IsNullable = false,ColumnDescription = "订单退回数" )]
        public int TotalReturnedQty 
        { 
            get{return _TotalReturnedQty;}
            set{SetProperty(ref _TotalReturnedQty, value);}
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


       
    }
}



