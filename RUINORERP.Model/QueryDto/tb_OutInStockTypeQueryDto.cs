
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2024 14:01:32
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
    /// 出入库类型  非生产领料/退料  借出，归还  报损报溢？单独处理？
    /// </summary>
    [Serializable()]
    [SugarTable("tb_OutInStockType")]
    public partial class tb_OutInStockTypeQueryDto:BaseEntityDto
    {
        public tb_OutInStockTypeQueryDto()
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
     

        private bool _OutIn= false;
        /// <summary>
        /// 出入类型
        /// </summary>
        [AdvQueryAttribute(ColName = "OutIn",ColDesc = "出入类型")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "OutIn",IsNullable = false,ColumnDescription = "出入类型" )]
        public bool OutIn 
        { 
            get{return _OutIn;}
            set{SetProperty(ref _OutIn, value);}
        }
     

        private bool _Is_enabled= true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否可用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_enabled",IsNullable = false,ColumnDescription = "是否可用" )]
        public bool Is_enabled 
        { 
            get{return _Is_enabled;}
            set{SetProperty(ref _Is_enabled, value);}
        }


       
    }
}



