
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:23
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
    /// 采购退回单
    /// </summary>
    [Serializable()]
    [SugarTable("tb_PurOrderReDetail")]
    public partial class tb_PurOrderReDetailQueryDto:BaseEntityDto
    {
        public tb_PurOrderReDetailQueryDto()
        {

        }

    
     

        private long _PurRetrunID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PurRetrunID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PurRetrunID",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_PurOrderRe","PurRetrunID")]
        public long PurRetrunID 
        { 
            get{return _PurRetrunID;}
            set{SetProperty(ref _PurRetrunID, value);}
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
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Location_ID",IsNullable = false,ColumnDescription = "" )]
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
     

        private int _Quantity;
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
     

        private decimal? _TransactionPrice;
        /// <summary>
        /// 成交单价
        /// </summary>
        [AdvQueryAttribute(ColName = "TransactionPrice",ColDesc = "成交单价")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "TransactionPrice",IsNullable = true,ColumnDescription = "成交单价" )]
        public decimal? TransactionPrice 
        { 
            get{return _TransactionPrice;}
            set{SetProperty(ref _TransactionPrice, value);}
        }
     

        private decimal? _TotalAmount;
        /// <summary>
        /// 小计
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalAmount",ColDesc = "小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "TotalAmount",IsNullable = true,ColumnDescription = "小计" )]
        public decimal? TotalAmount 
        { 
            get{return _TotalAmount;}
            set{SetProperty(ref _TotalAmount, value);}
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
     

        private string _CustomerType;
        /// <summary>
        /// 客户型号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerType",ColDesc = "客户型号")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "CustomerType",Length=100,IsNullable = true,ColumnDescription = "客户型号" )]
        public string CustomerType 
        { 
            get{return _CustomerType;}
            set{SetProperty(ref _CustomerType, value);}
        }
     

        private decimal? _commission;
        /// <summary>
        /// 抽成金额
        /// </summary>
        [AdvQueryAttribute(ColName = "commission",ColDesc = "抽成金额")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "commission",IsNullable = true,ColumnDescription = "抽成金额" )]
        public decimal? commission 
        { 
            get{return _commission;}
            set{SetProperty(ref _commission, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



