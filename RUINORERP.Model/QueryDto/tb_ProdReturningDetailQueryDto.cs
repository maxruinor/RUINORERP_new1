
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:12
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
    /// 归还单明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProdReturningDetail")]
    public partial class tb_ProdReturningDetailQueryDto:BaseEntityDto
    {
        public tb_ProdReturningDetailQueryDto()
        {

        }

    
     

        private long _ReturnID;
        /// <summary>
        /// 入库
        /// </summary>
        [AdvQueryAttribute(ColName = "ReturnID",ColDesc = "入库")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ReturnID",IsNullable = false,ColumnDescription = "入库" )]
        [FKRelationAttribute("tb_ProdReturning","ReturnID")]
        public long ReturnID 
        { 
            get{return _ReturnID;}
            set{SetProperty(ref _ReturnID, value);}
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
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Qty",ColDesc = "数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Qty",IsNullable = false,ColumnDescription = "数量" )]
        public int Qty 
        { 
            get{return _Qty;}
            set{SetProperty(ref _Qty, value);}
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
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Summary",Length=1000,IsNullable = true,ColumnDescription = "摘要" )]
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



