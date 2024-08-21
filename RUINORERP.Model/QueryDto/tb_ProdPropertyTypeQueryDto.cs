
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:20
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
    /// 产品属性类型EVA
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProdPropertyType")]
    public partial class tb_ProdPropertyTypeQueryDto:BaseEntityDto
    {
        public tb_ProdPropertyTypeQueryDto()
        {

        }

    
     

        private string _PropertyTypeName;
        /// <summary>
        /// 属性类型名称
        /// </summary>
        [AdvQueryAttribute(ColName = "PropertyTypeName",ColDesc = "属性类型名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PropertyTypeName",Length=50,IsNullable = false,ColumnDescription = "属性类型名称" )]
        public string PropertyTypeName 
        { 
            get{return _PropertyTypeName;}
            set{SetProperty(ref _PropertyTypeName, value);}
        }
     

        private string _PropertyTypeDesc;
        /// <summary>
        /// 属性类型描述
        /// </summary>
        [AdvQueryAttribute(ColName = "PropertyTypeDesc",ColDesc = "属性类型描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "PropertyTypeDesc",Length=100,IsNullable = true,ColumnDescription = "属性类型描述" )]
        public string PropertyTypeDesc 
        { 
            get{return _PropertyTypeDesc;}
            set{SetProperty(ref _PropertyTypeDesc, value);}
        }


       
    }
}



