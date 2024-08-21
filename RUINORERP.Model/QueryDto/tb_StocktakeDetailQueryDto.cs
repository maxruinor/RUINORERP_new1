
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:49:15
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
    /// 盘点明细表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_StocktakeDetail")]
    public partial class tb_StocktakeDetailQueryDto:BaseEntityDto
    {
        public tb_StocktakeDetailQueryDto()
        {

        }

    
     

        private long _MainID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "MainID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "MainID",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_Stocktake","MainID")]
        public long MainID 
        { 
            get{return _MainID;}
            set{SetProperty(ref _MainID, value);}
        }
     

        private long _ProdDetailID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{SetProperty(ref _ProdDetailID, value);}
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
     

        private int _CarryinglQty= ((0));
        /// <summary>
        /// 载账数量
        /// </summary>
        [AdvQueryAttribute(ColName = "CarryinglQty",ColDesc = "载账数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "CarryinglQty",IsNullable = false,ColumnDescription = "载账数量" )]
        public int CarryinglQty 
        { 
            get{return _CarryinglQty;}
            set{SetProperty(ref _CarryinglQty, value);}
        }
     

        private decimal _CarryingSubtotalAmount= ((0));
        /// <summary>
        /// 载账小计
        /// </summary>
        [AdvQueryAttribute(ColName = "CarryingSubtotalAmount",ColDesc = "载账小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "CarryingSubtotalAmount",IsNullable = false,ColumnDescription = "载账小计" )]
        public decimal CarryingSubtotalAmount 
        { 
            get{return _CarryingSubtotalAmount;}
            set{SetProperty(ref _CarryingSubtotalAmount, value);}
        }
     

        private int _DiffQty= ((0));
        /// <summary>
        /// 差异数量
        /// </summary>
        [AdvQueryAttribute(ColName = "DiffQty",ColDesc = "差异数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "DiffQty",IsNullable = false,ColumnDescription = "差异数量" )]
        public int DiffQty 
        { 
            get{return _DiffQty;}
            set{SetProperty(ref _DiffQty, value);}
        }
     

        private decimal _DiffSubtotalAmount= ((0));
        /// <summary>
        /// 差异小计
        /// </summary>
        [AdvQueryAttribute(ColName = "DiffSubtotalAmount",ColDesc = "差异小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "DiffSubtotalAmount",IsNullable = false,ColumnDescription = "差异小计" )]
        public decimal DiffSubtotalAmount 
        { 
            get{return _DiffSubtotalAmount;}
            set{SetProperty(ref _DiffSubtotalAmount, value);}
        }
     

        private int _CheckQty= ((0));
        /// <summary>
        /// 盘点数量
        /// </summary>
        [AdvQueryAttribute(ColName = "CheckQty",ColDesc = "盘点数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "CheckQty",IsNullable = false,ColumnDescription = "盘点数量" )]
        public int CheckQty 
        { 
            get{return _CheckQty;}
            set{SetProperty(ref _CheckQty, value);}
        }
     

        private decimal _CheckSubtotalAmount= ((0));
        /// <summary>
        /// 盘点小计
        /// </summary>
        [AdvQueryAttribute(ColName = "CheckSubtotalAmount",ColDesc = "盘点小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "CheckSubtotalAmount",IsNullable = false,ColumnDescription = "盘点小计" )]
        public decimal CheckSubtotalAmount 
        { 
            get{return _CheckSubtotalAmount;}
            set{SetProperty(ref _CheckSubtotalAmount, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



