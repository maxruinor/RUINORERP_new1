
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 17:35:22
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
    /// 货物类型  成品  半成品  包装材料 下脚料这种内容
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProductType")]
    public partial class tb_ProductTypeQueryDto:BaseEntityDto
    {
        public tb_ProductTypeQueryDto()
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
     

        private bool _ForSale;
        /// <summary>
        /// 待销类型
        /// </summary>
        [AdvQueryAttribute(ColName = "ForSale",ColDesc = "待销类型")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "ForSale",IsNullable = false,ColumnDescription = "待销类型" )]
        public bool ForSale 
        { 
            get{return _ForSale;}
            set{SetProperty(ref _ForSale, value);}
        }


       
    }
}



