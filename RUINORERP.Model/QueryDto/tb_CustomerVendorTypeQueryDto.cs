
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:34
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
    /// 往来单位类型,如级别，电商，大客户，亚马逊等
    /// </summary>
    [Serializable()]
    [SugarTable("tb_CustomerVendorType")]
    public partial class tb_CustomerVendorTypeQueryDto:BaseEntityDto
    {
        public tb_CustomerVendorTypeQueryDto()
        {

        }

    
     

        private string _TypeName;
        /// <summary>
        /// 类型等级名称
        /// </summary>
        [AdvQueryAttribute(ColName = "TypeName",ColDesc = "类型等级名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "TypeName",Length=50,IsNullable = true,ColumnDescription = "类型等级名称" )]
        public string TypeName 
        { 
            get{return _TypeName;}
            set{SetProperty(ref _TypeName, value);}
        }
     

        private string _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Desc",ColDesc = "描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Desc",Length=100,IsNullable = true,ColumnDescription = "描述" )]
        public string Desc 
        { 
            get{return _Desc;}
            set{SetProperty(ref _Desc, value);}
        }


       
    }
}



