
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:58
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
    /// 外发加工订单表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Outsourcing_order")]
    public partial class tb_Outsourcing_orderQueryDto:BaseEntityDto
    {
        public tb_Outsourcing_orderQueryDto()
        {

        }

    
     

        private int _Quantity= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Quantity",IsNullable = false,ColumnDescription = "" )]
        public int Quantity 
        { 
            get{return _Quantity;}
            set{SetProperty(ref _Quantity, value);}
        }
     

        private decimal _Unit_price= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Unit_price",ColDesc = "")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Unit_price",IsNullable = false,ColumnDescription = "" )]
        public decimal Unit_price 
        { 
            get{return _Unit_price;}
            set{SetProperty(ref _Unit_price, value);}
        }
     

        private decimal _Total_amount= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Total_amount",ColDesc = "")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Total_amount",IsNullable = false,ColumnDescription = "" )]
        public decimal Total_amount 
        { 
            get{return _Total_amount;}
            set{SetProperty(ref _Total_amount, value);}
        }
     

        private int? _Status;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Status",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Status",IsNullable = true,ColumnDescription = "" )]
        public int? Status 
        { 
            get{return _Status;}
            set{SetProperty(ref _Status, value);}
        }
     

        private DateTime? _Order_date;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Order_date",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Order_date",IsNullable = true,ColumnDescription = "" )]
        public DateTime? Order_date 
        { 
            get{return _Order_date;}
            set{SetProperty(ref _Order_date, value);}
        }
     

        private DateTime? _Delivery_date;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Delivery_date",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Delivery_date",IsNullable = true,ColumnDescription = "" )]
        public DateTime? Delivery_date 
        { 
            get{return _Delivery_date;}
            set{SetProperty(ref _Delivery_date, value);}
        }


       
    }
}



