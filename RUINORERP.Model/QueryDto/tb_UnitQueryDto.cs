
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/14/2024 15:01:02
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
    /// 基本单位
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Unit")]
    public partial class tb_UnitQueryDto:BaseEntityDto
    {
        public tb_UnitQueryDto()
        {

        }

    
     

        private string _UnitName;
        /// <summary>
        /// 单位名称
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitName",ColDesc = "单位名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "UnitName",Length=255,IsNullable = false,ColumnDescription = "单位名称" )]
        public string UnitName 
        { 
            get{return _UnitName;}
            set{SetProperty(ref _UnitName, value);}
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
     

        private bool _is_measurement_unit= false;
        /// <summary>
        /// 是否可换算
        /// </summary>
        [AdvQueryAttribute(ColName = "is_measurement_unit",ColDesc = "是否可换算")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "is_measurement_unit",IsNullable = false,ColumnDescription = "是否可换算" )]
        public bool is_measurement_unit 
        { 
            get{return _is_measurement_unit;}
            set{SetProperty(ref _is_measurement_unit, value);}
        }


       
    }
}



