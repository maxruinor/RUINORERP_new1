
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:18
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
    /// 产品信息汇总
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProdInfoSummary")]
    public partial class tb_ProdInfoSummaryQueryDto:BaseEntityDto
    {
        public tb_ProdInfoSummaryQueryDto()
        {

        }

    
     

        private decimal? _平均价格;
        /// <summary>
        /// 平均价格
        /// </summary>
        [AdvQueryAttribute(ColName = "平均价格",ColDesc = "平均价格")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "平均价格",IsNullable = true,ColumnDescription = "平均价格" )]
        public decimal? 平均价格 
        { 
            get{return _平均价格;}
            set{SetProperty(ref _平均价格, value);}
        }
     

        private int? _总销售量;
        /// <summary>
        /// 总销售量
        /// </summary>
        [AdvQueryAttribute(ColName = "总销售量",ColDesc = "总销售量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "总销售量",IsNullable = true,ColumnDescription = "总销售量" )]
        public int? 总销售量 
        { 
            get{return _总销售量;}
            set{SetProperty(ref _总销售量, value);}
        }
     

        private int? _库存总量;
        /// <summary>
        /// 库存总量
        /// </summary>
        [AdvQueryAttribute(ColName = "库存总量",ColDesc = "库存总量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "库存总量",IsNullable = true,ColumnDescription = "库存总量" )]
        public int? 库存总量 
        { 
            get{return _库存总量;}
            set{SetProperty(ref _库存总量, value);}
        }


       
    }
}



