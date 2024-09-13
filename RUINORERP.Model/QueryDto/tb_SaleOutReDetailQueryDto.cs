
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 19:02:36
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
    /// 销售出库退回明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_SaleOutReDetail")]
    public partial class tb_SaleOutReDetailQueryDto:BaseEntityDto
    {
        public tb_SaleOutReDetailQueryDto()
        {

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
     

        private long _SaleOutRe_ID;
        /// <summary>
        /// 销售退回单
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleOutRe_ID",ColDesc = "销售退回单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "SaleOutRe_ID",IsNullable = false,ColumnDescription = "销售退回单" )]
        [FKRelationAttribute("tb_SaleOutRe","SaleOutRe_ID")]
        public long SaleOutRe_ID 
        { 
            get{return _SaleOutRe_ID;}
            set{SetProperty(ref _SaleOutRe_ID, value);}
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
     

        private int _Quantity= ((0));
        /// <summary>
        /// 退回数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "退回数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Quantity",IsNullable = false,ColumnDescription = "退回数量" )]
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
     

        private decimal _SubtotalTransAmount= ((0));
        /// <summary>
        /// 小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTransAmount",ColDesc = "小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalTransAmount",IsNullable = false,ColumnDescription = "小计" )]
        public decimal SubtotalTransAmount 
        { 
            get{return _SubtotalTransAmount;}
            set{SetProperty(ref _SubtotalTransAmount, value);}
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
     

        private decimal _TaxRate= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "TaxRate",IsNullable = false,ColumnDescription = "" )]
        public decimal TaxRate 
        { 
            get{return _TaxRate;}
            set{SetProperty(ref _TaxRate, value);}
        }
     

        private decimal _SubtotalTaxAmount= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTaxAmount",ColDesc = "")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalTaxAmount",IsNullable = false,ColumnDescription = "" )]
        public decimal SubtotalTaxAmount 
        { 
            get{return _SubtotalTaxAmount;}
            set{SetProperty(ref _SubtotalTaxAmount, value);}
        }
     

        private decimal? _CommissionAmount;
        /// <summary>
        /// 抽成返还
        /// </summary>
        [AdvQueryAttribute(ColName = "CommissionAmount",ColDesc = "抽成返还")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "CommissionAmount",IsNullable = true,ColumnDescription = "抽成返还" )]
        public decimal? CommissionAmount 
        { 
            get{return _CommissionAmount;}
            set{SetProperty(ref _CommissionAmount, value);}
        }


       
    }
}



