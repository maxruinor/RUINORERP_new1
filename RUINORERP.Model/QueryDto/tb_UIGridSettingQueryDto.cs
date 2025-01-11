
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2025 15:31:55
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
    /// UI表格设置
    /// </summary>
    [Serializable()]
    [SugarTable("tb_UIGridSetting")]
    public partial class tb_UIGridSettingQueryDto:BaseEntityDto
    {
        public tb_UIGridSettingQueryDto()
        {

        }

    
     

        private long? _UIMenuPID;
        /// <summary>
        /// 菜单设置
        /// </summary>
        [AdvQueryAttribute(ColName = "UIMenuPID",ColDesc = "菜单设置")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "UIMenuPID",IsNullable = true,ColumnDescription = "菜单设置" )]
        [FKRelationAttribute("tb_UIMenuPersonalization","UIMenuPID")]
        public long? UIMenuPID 
        { 
            get{return _UIMenuPID;}
            set{SetProperty(ref _UIMenuPID, value);}
        }
     

        private string _GridKeyName;
        /// <summary>
        /// 表格名称
        /// </summary>
        [AdvQueryAttribute(ColName = "GridKeyName",ColDesc = "表格名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "GridKeyName",Length=255,IsNullable = true,ColumnDescription = "表格名称" )]
        public string GridKeyName 
        { 
            get{return _GridKeyName;}
            set{SetProperty(ref _GridKeyName, value);}
        }
     

        private string _ColsSetting;
        /// <summary>
        /// 列设置信息
        /// </summary>
        [AdvQueryAttribute(ColName = "ColsSetting",ColDesc = "列设置信息")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "ColsSetting",Length=2147483647,IsNullable = true,ColumnDescription = "列设置信息" )]
        public string ColsSetting 
        { 
            get{return _ColsSetting;}
            set{SetProperty(ref _ColsSetting, value);}
        }


       
    }
}



