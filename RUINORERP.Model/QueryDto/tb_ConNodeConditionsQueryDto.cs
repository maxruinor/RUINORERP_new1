
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:50
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
    /// 流程步骤 为转移条件集合，Field为条件左参数，Operator为操作操作符如果值类型为String则表达式只能为==或者!=，Value为表达式值
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ConNodeConditions")]
    public partial class tb_ConNodeConditionsQueryDto:BaseEntityDto
    {
        public tb_ConNodeConditionsQueryDto()
        {

        }

    
     

        private string _Field;
        /// <summary>
        /// 表达式
        /// </summary>
        [AdvQueryAttribute(ColName = "Field",ColDesc = "表达式")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Field",Length=50,IsNullable = false,ColumnDescription = "表达式" )]
        public string Field 
        { 
            get{return _Field;}
            set{SetProperty(ref _Field, value);}
        }
     

        private string _Operator;
        /// <summary>
        /// 操作符
        /// </summary>
        [AdvQueryAttribute(ColName = "Operator",ColDesc = "操作符")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Operator",Length=50,IsNullable = true,ColumnDescription = "操作符" )]
        public string Operator 
        { 
            get{return _Operator;}
            set{SetProperty(ref _Operator, value);}
        }
     

        private string _Value;
        /// <summary>
        /// 表达式值
        /// </summary>
        [AdvQueryAttribute(ColName = "Value",ColDesc = "表达式值")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Value",Length=50,IsNullable = true,ColumnDescription = "表达式值" )]
        public string Value 
        { 
            get{return _Value;}
            set{SetProperty(ref _Value, value);}
        }


       
    }
}



