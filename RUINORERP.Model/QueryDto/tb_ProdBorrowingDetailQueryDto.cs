
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2025 21:05:39
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
    /// 借出单明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProdBorrowingDetail")]
    public partial class tb_ProdBorrowingDetailQueryDto:BaseEntityDto
    {
        public tb_ProdBorrowingDetailQueryDto()
        {

        }

    
     

        private long _BorrowID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "BorrowID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "BorrowID",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ProdBorrowing","BorrowID")]
        public long BorrowID 
        { 
            get{return _BorrowID;}
            set{SetProperty(ref _BorrowID, value);}
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
     

        private int _Qty= ((0));
        /// <summary>
        /// 借出数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Qty",ColDesc = "借出数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Qty",IsNullable = false,ColumnDescription = "借出数量" )]
        public int Qty 
        { 
            get{return _Qty;}
            set{SetProperty(ref _Qty, value);}
        }
     

        private int _ReQty= ((0));
        /// <summary>
        /// 归还数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ReQty",ColDesc = "归还数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ReQty",IsNullable = false,ColumnDescription = "归还数量" )]
        public int ReQty 
        { 
            get{return _ReQty;}
            set{SetProperty(ref _ReQty, value);}
        }
     

        private decimal _Price= ((0));
        /// <summary>
        /// 售价
        /// </summary>
        [AdvQueryAttribute(ColName = "Price",ColDesc = "售价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Price",IsNullable = false,ColumnDescription = "售价" )]
        public decimal Price 
        { 
            get{return _Price;}
            set{SetProperty(ref _Price, value);}
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
     

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Summary",Length=500,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary 
        { 
            get{return _Summary;}
            set{SetProperty(ref _Summary, value);}
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
     

        private decimal _SubtotalPirceAmount= ((0));
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



