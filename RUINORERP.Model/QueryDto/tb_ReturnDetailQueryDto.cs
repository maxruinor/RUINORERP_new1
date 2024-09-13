
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:25
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
    /// 返厂明细
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ReturnDetail")]
    public partial class tb_ReturnDetailQueryDto:BaseEntityDto
    {
        public tb_ReturnDetailQueryDto()
        {

        }

    
     

        private long? _MainID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "MainID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "MainID",IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_Return","MainID")]
        public long? MainID 
        { 
            get{return _MainID;}
            set{SetProperty(ref _MainID, value);}
        }
     

        private long? _ProdDetailID;
        /// <summary>
        /// 货品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品详情")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "货品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{SetProperty(ref _ProdDetailID, value);}
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
     

        private decimal _Cost= ((0));
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Cost",IsNullable = false,ColumnDescription = "成本" )]
        public decimal Cost 
        { 
            get{return _Cost;}
            set{SetProperty(ref _Cost, value);}
        }
     

        private decimal _Price= ((0));
        /// <summary>
        /// 单价
        /// </summary>
        [AdvQueryAttribute(ColName = "Price",ColDesc = "单价")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Price",IsNullable = false,ColumnDescription = "单价" )]
        public decimal Price 
        { 
            get{return _Price;}
            set{SetProperty(ref _Price, value);}
        }
     

        private decimal _SubtotalAmount= ((0));
        /// <summary>
        /// 金额小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalAmount",ColDesc = "金额小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalAmount",IsNullable = false,ColumnDescription = "金额小计" )]
        public decimal SubtotalAmount 
        { 
            get{return _SubtotalAmount;}
            set{SetProperty(ref _SubtotalAmount, value);}
        }
     

        private decimal _SubtotalCost= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalCost",ColDesc = "成本小计")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "SubtotalCost",IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal SubtotalCost 
        { 
            get{return _SubtotalCost;}
            set{SetProperty(ref _SubtotalCost, value);}
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


       
    }
}



