
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/30/2024 00:18:28
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
    /// UI表格列设置
    /// </summary>
    [Serializable()]
    [SugarTable("tb_UIGridColsSetting")]
    public partial class tb_UIGridColsSettingQueryDto:BaseEntityDto
    {
        public tb_UIGridColsSettingQueryDto()
        {

        }

    
     

        private long? _FieldInfo_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "FieldInfo_ID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "FieldInfo_ID",IsNullable = true,ColumnDescription = "" )]
        public long? FieldInfo_ID 
        { 
            get{return _FieldInfo_ID;}
            set{SetProperty(ref _FieldInfo_ID, value);}
        }
     

        private int _ColDisplayIndex;
        /// <summary>
        /// 显示排序
        /// </summary>
        [AdvQueryAttribute(ColName = "ColDisplayIndex",ColDesc = "显示排序")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ColDisplayIndex",IsNullable = false,ColumnDescription = "显示排序" )]
        public int ColDisplayIndex 
        { 
            get{return _ColDisplayIndex;}
            set{SetProperty(ref _ColDisplayIndex, value);}
        }
     

        private int? _Sort;
        /// <summary>
        /// 数据排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "数据排序")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Sort",IsNullable = true,ColumnDescription = "数据排序" )]
        public int? Sort 
        { 
            get{return _Sort;}
            set{SetProperty(ref _Sort, value);}
        }
     

        private bool _Visible;
        /// <summary>
        /// 是否可见
        /// </summary>
        [AdvQueryAttribute(ColName = "Visible",ColDesc = "是否可见")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Visible",IsNullable = false,ColumnDescription = "是否可见" )]
        public bool Visible 
        { 
            get{return _Visible;}
            set{SetProperty(ref _Visible, value);}
        }
     

        private int? _ColWith;
        /// <summary>
        /// 值类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ColWith",ColDesc = "值类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "ColWith",IsNullable = true,ColumnDescription = "值类型" )]
        public int? ColWith 
        { 
            get{return _ColWith;}
            set{SetProperty(ref _ColWith, value);}
        }


       
    }
}



