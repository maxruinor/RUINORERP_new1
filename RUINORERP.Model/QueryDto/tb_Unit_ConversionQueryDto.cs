
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/21/2025 19:17:35
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
    /// 单位换算表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Unit_Conversion")]
    public partial class tb_Unit_ConversionQueryDto:BaseEntityDto
    {
        public tb_Unit_ConversionQueryDto()
        {

        }

    
     

        private string _UnitConversion_Name;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitConversion_Name",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "UnitConversion_Name",Length=100,IsNullable = false,ColumnDescription = "备注" )]
        public string UnitConversion_Name 
        { 
            get{return _UnitConversion_Name;}
            set{SetProperty(ref _UnitConversion_Name, value);}
        }
     

        private long _Source_unit_id;
        /// <summary>
        /// 来源单位
        /// </summary>
        [AdvQueryAttribute(ColName = "Source_unit_id",ColDesc = "来源单位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Source_unit_id",IsNullable = false,ColumnDescription = "来源单位" )]
        [FKRelationAttribute("tb_Unit","Source_unit_id")]
        public long Source_unit_id 
        { 
            get{return _Source_unit_id;}
            set{SetProperty(ref _Source_unit_id, value);}
        }
     

        private long _Target_unit_id;
        /// <summary>
        /// 目标单位
        /// </summary>
        [AdvQueryAttribute(ColName = "Target_unit_id",ColDesc = "目标单位")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Target_unit_id",IsNullable = false,ColumnDescription = "目标单位" )]
        [FKRelationAttribute("tb_Unit","Target_unit_id")]
        public long Target_unit_id 
        { 
            get{return _Target_unit_id;}
            set{SetProperty(ref _Target_unit_id, value);}
        }
     

        private decimal _Conversion_ratio= ((0));
        /// <summary>
        /// 换算比例
        /// </summary>
        [AdvQueryAttribute(ColName = "Conversion_ratio",ColDesc = "换算比例")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Conversion_ratio",IsNullable = false,ColumnDescription = "换算比例" )]
        public decimal Conversion_ratio 
        { 
            get{return _Conversion_ratio;}
            set{SetProperty(ref _Conversion_ratio, value);}
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



