
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:38
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
    /// 流程图定义
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FlowchartDefinition")]
    public partial class tb_FlowchartDefinitionQueryDto:BaseEntityDto
    {
        public tb_FlowchartDefinitionQueryDto()
        {

        }

    
     

        private long? _ModuleID;
        /// <summary>
        /// 模块
        /// </summary>
        [AdvQueryAttribute(ColName = "ModuleID",ColDesc = "模块")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ModuleID",IsNullable = true,ColumnDescription = "模块" )]
        [FKRelationAttribute("tb_ModuleDefinition","ModuleID")]
        public long? ModuleID 
        { 
            get{return _ModuleID;}
            set{SetProperty(ref _ModuleID, value);}
        }
     

        private string _FlowchartNo;
        /// <summary>
        /// 流程图编号
        /// </summary>
        [AdvQueryAttribute(ColName = "FlowchartNo",ColDesc = "流程图编号")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "FlowchartNo",Length=50,IsNullable = false,ColumnDescription = "流程图编号" )]
        public string FlowchartNo 
        { 
            get{return _FlowchartNo;}
            set{SetProperty(ref _FlowchartNo, value);}
        }
     

        private string _FlowchartName;
        /// <summary>
        /// 流程图名称
        /// </summary>
        [AdvQueryAttribute(ColName = "FlowchartName",ColDesc = "流程图名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "FlowchartName",Length=20,IsNullable = false,ColumnDescription = "流程图名称" )]
        public string FlowchartName 
        { 
            get{return _FlowchartName;}
            set{SetProperty(ref _FlowchartName, value);}
        }


       
    }
}



