
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/11/2024 18:42:54
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
    /// 价格记录表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_PriceRecord")]
    public partial class tb_PriceRecordQueryDto:BaseEntityDto
    {
        public tb_PriceRecordQueryDto()
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
     

        private long _Employee_ID;
        /// <summary>
        /// 经办人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = false,ColumnDescription = "经办人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private DateTime? _PurDate;
        /// <summary>
        /// 采购日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PurDate",ColDesc = "采购日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "PurDate",IsNullable = true,ColumnDescription = "采购日期" )]
        public DateTime? PurDate 
        { 
            get{return _PurDate;}
            set{SetProperty(ref _PurDate, value);}
        }
     

        private DateTime? _SaleDate;
        /// <summary>
        /// 销售日期
        /// </summary>
        [AdvQueryAttribute(ColName = "SaleDate",ColDesc = "销售日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "SaleDate",IsNullable = true,ColumnDescription = "销售日期" )]
        public DateTime? SaleDate 
        { 
            get{return _SaleDate;}
            set{SetProperty(ref _SaleDate, value);}
        }
     

        private decimal _PurPrice= ((0));
        /// <summary>
        /// 采购价
        /// </summary>
        [AdvQueryAttribute(ColName = "PurPrice",ColDesc = "采购价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "PurPrice",IsNullable = false,ColumnDescription = "采购价" )]
        public decimal PurPrice 
        { 
            get{return _PurPrice;}
            set{SetProperty(ref _PurPrice, value);}
        }
     

        private decimal _SalePrice= ((0));
        /// <summary>
        /// 销售价
        /// </summary>
        [AdvQueryAttribute(ColName = "SalePrice",ColDesc = "销售价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SalePrice",IsNullable = false,ColumnDescription = "销售价" )]
        public decimal SalePrice 
        { 
            get{return _SalePrice;}
            set{SetProperty(ref _SalePrice, value);}
        }


       
    }
}



