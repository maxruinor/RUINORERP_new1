
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:49:06
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
    /// 步骤变量
    /// </summary>
    [Serializable()]
    [SugarTable("tb_StepBodyPara")]
    public partial class tb_StepBodyParaQueryDto:BaseEntityDto
    {
        public tb_StepBodyParaQueryDto()
        {

        }

    
     

        private string _Key;
        /// <summary>
        /// 参数key
        /// </summary>
        [AdvQueryAttribute(ColName = "Key",ColDesc = "参数key")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Key",Length=50,IsNullable = false,ColumnDescription = "参数key" )]
        public string Key 
        { 
            get{return _Key;}
            set{SetProperty(ref _Key, value);}
        }
     

        private string _Name;
        /// <summary>
        /// 参数名
        /// </summary>
        [AdvQueryAttribute(ColName = "Name",ColDesc = "参数名")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Name",Length=50,IsNullable = true,ColumnDescription = "参数名" )]
        public string Name 
        { 
            get{return _Name;}
            set{SetProperty(ref _Name, value);}
        }
     

        private string _DisplayName;
        /// <summary>
        /// 显示名称
        /// </summary>
        [AdvQueryAttribute(ColName = "DisplayName",ColDesc = "显示名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "DisplayName",Length=50,IsNullable = true,ColumnDescription = "显示名称" )]
        public string DisplayName 
        { 
            get{return _DisplayName;}
            set{SetProperty(ref _DisplayName, value);}
        }
     

        private string _Value;
        /// <summary>
        /// 参数值
        /// </summary>
        [AdvQueryAttribute(ColName = "Value",ColDesc = "参数值")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Value",Length=50,IsNullable = true,ColumnDescription = "参数值" )]
        public string Value 
        { 
            get{return _Value;}
            set{SetProperty(ref _Value, value);}
        }
     

        private string _StepBodyParaType;
        /// <summary>
        /// 参数类型
        /// </summary>
        [AdvQueryAttribute(ColName = "StepBodyParaType",ColDesc = "参数类型")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "StepBodyParaType",Length=50,IsNullable = true,ColumnDescription = "参数类型" )]
        public string StepBodyParaType 
        { 
            get{return _StepBodyParaType;}
            set{SetProperty(ref _StepBodyParaType, value);}
        }


       
    }
}



