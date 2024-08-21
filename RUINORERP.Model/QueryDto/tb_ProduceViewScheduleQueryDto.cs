
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:23
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
    /// 可视化排程
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProduceViewSchedule")]
    public partial class tb_ProduceViewScheduleQueryDto:BaseEntityDto
    {
        public tb_ProduceViewScheduleQueryDto()
        {

        }

    
     

        private int? _product_id;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "product_id",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "product_id",IsNullable = true,ColumnDescription = "" )]
        public int? product_id 
        { 
            get{return _product_id;}
            set{SetProperty(ref _product_id, value);}
        }
     

        private int? _quantity;
        /// <summary>
        /// 生产数量
        /// </summary>
        [AdvQueryAttribute(ColName = "quantity",ColDesc = "生产数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "quantity",IsNullable = true,ColumnDescription = "生产数量" )]
        public int? quantity 
        { 
            get{return _quantity;}
            set{SetProperty(ref _quantity, value);}
        }
     

        private DateTime? _start_date;
        /// <summary>
        /// 计划开始日期
        /// </summary>
        [AdvQueryAttribute(ColName = "start_date",ColDesc = "计划开始日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "start_date",IsNullable = true,ColumnDescription = "计划开始日期" )]
        public DateTime? start_date 
        { 
            get{return _start_date;}
            set{SetProperty(ref _start_date, value);}
        }
     

        private DateTime? _end_date;
        /// <summary>
        /// 计划完成日期
        /// </summary>
        [AdvQueryAttribute(ColName = "end_date",ColDesc = "计划完成日期")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "end_date",IsNullable = true,ColumnDescription = "计划完成日期" )]
        public DateTime? end_date 
        { 
            get{return _end_date;}
            set{SetProperty(ref _end_date, value);}
        }


       
    }
}



