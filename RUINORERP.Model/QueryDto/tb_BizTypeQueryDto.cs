
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:42
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
    /// 业务类型
    /// </summary>
    [Serializable()]
    [SugarTable("tb_BizType")]
    public partial class tb_BizTypeQueryDto:BaseEntityDto
    {
        public tb_BizTypeQueryDto()
        {

        }

    
     

        private string _TypeName;
        /// <summary>
        /// 类型名称
        /// </summary>
        [AdvQueryAttribute(ColName = "TypeName",ColDesc = "类型名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "TypeName",Length=50,IsNullable = false,ColumnDescription = "类型名称" )]
        public string TypeName 
        { 
            get{return _TypeName;}
            set{SetProperty(ref _TypeName, value);}
        }
     

        private string _TypeDesc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "TypeDesc",ColDesc = "描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "TypeDesc",Length=100,IsNullable = true,ColumnDescription = "描述" )]
        public string TypeDesc 
        { 
            get{return _TypeDesc;}
            set{SetProperty(ref _TypeDesc, value);}
        }
     

        private string _Module;
        /// <summary>
        /// 所属模块
        /// </summary>
        [AdvQueryAttribute(ColName = "Module",ColDesc = "所属模块")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "Module",Length=50,IsNullable = true,ColumnDescription = "所属模块" )]
        public string Module 
        { 
            get{return _Module;}
            set{SetProperty(ref _Module, value);}
        }


       
    }
}



