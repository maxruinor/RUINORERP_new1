
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:24
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
    /// 地区表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Region")]
    public partial class tb_RegionQueryDto:BaseEntityDto
    {
        public tb_RegionQueryDto()
        {

        }

    
     

        private string _Region_Name;
        /// <summary>
        /// 地区名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Region_Name",ColDesc = "地区名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Region_Name",Length=50,IsNullable = true,ColumnDescription = "地区名称" )]
        public string Region_Name 
        { 
            get{return _Region_Name;}
            set{SetProperty(ref _Region_Name, value);}
        }
     

        private string _Region_code;
        /// <summary>
        /// 地区代码
        /// </summary>
        [AdvQueryAttribute(ColName = "Region_code",ColDesc = "地区代码")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Region_code",Length=20,IsNullable = true,ColumnDescription = "地区代码" )]
        public string Region_code 
        { 
            get{return _Region_code;}
            set{SetProperty(ref _Region_code, value);}
        }
     

        private long? _Parent_region_id;
        /// <summary>
        ///  父地区
        /// </summary>
        [AdvQueryAttribute(ColName = "Parent_region_id",ColDesc = " 父地区")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Parent_region_id",IsNullable = true,ColumnDescription = " 父地区" )]
        [FKRelationAttribute("tb_Region","Parent_region_id")]
        public long? Parent_region_id 
        { 
            get{return _Parent_region_id;}
            set{SetProperty(ref _Parent_region_id, value);}
        }
     

        private long? _Customer_id;
        /// <summary>
        /// 意向客户
        /// </summary>
        [AdvQueryAttribute(ColName = "Customer_id",ColDesc = "意向客户")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Customer_id",IsNullable = true,ColumnDescription = "意向客户" )]
        [FKRelationAttribute("tb_Customer","Customer_id")]
        public long? Customer_id 
        { 
            get{return _Customer_id;}
            set{SetProperty(ref _Customer_id, value);}
        }


       
    }
}



