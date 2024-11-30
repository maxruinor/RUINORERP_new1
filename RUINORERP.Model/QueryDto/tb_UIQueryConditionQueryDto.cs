
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/30/2024 00:18:30
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
    /// UI查询条件设置
    /// </summary>
    [Serializable()]
    [SugarTable("tb_UIQueryCondition")]
    public partial class tb_UIQueryConditionQueryDto:BaseEntityDto
    {
        public tb_UIQueryConditionQueryDto()
        {

        }

    
     

        private int? _Sort;
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "排序")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Sort",IsNullable = true,ColumnDescription = "排序" )]
        public int? Sort 
        { 
            get{return _Sort;}
            set{SetProperty(ref _Sort, value);}
        }
     

        private bool _IsVisble;
        /// <summary>
        /// 是否可见
        /// </summary>
        [AdvQueryAttribute(ColName = "IsVisble",ColDesc = "是否可见")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsVisble",IsNullable = false,ColumnDescription = "是否可见" )]
        public bool IsVisble 
        { 
            get{return _IsVisble;}
            set{SetProperty(ref _IsVisble, value);}
        }
     

        private string _ValueType;
        /// <summary>
        /// 值类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ValueType",ColDesc = "值类型")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ValueType",Length=50,IsNullable = true,ColumnDescription = "值类型" )]
        public string ValueType 
        { 
            get{return _ValueType;}
            set{SetProperty(ref _ValueType, value);}
        }
     

        private string _Default1;
        /// <summary>
        /// 默认值1
        /// </summary>
        [AdvQueryAttribute(ColName = "Default1",ColDesc = "默认值1")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Default1",Length=255,IsNullable = true,ColumnDescription = "默认值1" )]
        public string Default1 
        { 
            get{return _Default1;}
            set{SetProperty(ref _Default1, value);}
        }
     

        private string _Default2;
        /// <summary>
        /// 默认值2
        /// </summary>
        [AdvQueryAttribute(ColName = "Default2",ColDesc = "默认值2")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Default2",Length=255,IsNullable = true,ColumnDescription = "默认值2" )]
        public string Default2 
        { 
            get{return _Default2;}
            set{SetProperty(ref _Default2, value);}
        }
     

        private string _EnableDefault1;
        /// <summary>
        /// 启用默认值1
        /// </summary>
        [AdvQueryAttribute(ColName = "EnableDefault1",ColDesc = "启用默认值1")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "EnableDefault1",Length=255,IsNullable = true,ColumnDescription = "启用默认值1" )]
        public string EnableDefault1 
        { 
            get{return _EnableDefault1;}
            set{SetProperty(ref _EnableDefault1, value);}
        }
     

        private string _EnableDefault2;
        /// <summary>
        /// 启用默认值2
        /// </summary>
        [AdvQueryAttribute(ColName = "EnableDefault2",ColDesc = "启用默认值2")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "EnableDefault2",Length=255,IsNullable = true,ColumnDescription = "启用默认值2" )]
        public string EnableDefault2 
        { 
            get{return _EnableDefault2;}
            set{SetProperty(ref _EnableDefault2, value);}
        }


       
    }
}



