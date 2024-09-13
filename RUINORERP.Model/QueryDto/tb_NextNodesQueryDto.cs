
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:56
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
    /// 流程步骤 转移条件集合
    /// </summary>
    [Serializable()]
    [SugarTable("tb_NextNodes")]
    public partial class tb_NextNodesQueryDto:BaseEntityDto
    {
        public tb_NextNodesQueryDto()
        {

        }

    
     

        private long? _ConNodeConditions_Id;
        /// <summary>
        /// 条件
        /// </summary>
        [AdvQueryAttribute(ColName = "ConNodeConditions_Id",ColDesc = "条件")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ConNodeConditions_Id",IsNullable = true,ColumnDescription = "条件" )]
        [FKRelationAttribute("tb_ConNodeConditions","ConNodeConditions_Id")]
        public long? ConNodeConditions_Id 
        { 
            get{return _ConNodeConditions_Id;}
            set{SetProperty(ref _ConNodeConditions_Id, value);}
        }
     

        private string _NexNodeName;
        /// <summary>
        /// 下节点名称
        /// </summary>
        [AdvQueryAttribute(ColName = "NexNodeName",ColDesc = "下节点名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "NexNodeName",Length=50,IsNullable = false,ColumnDescription = "下节点名称" )]
        public string NexNodeName 
        { 
            get{return _NexNodeName;}
            set{SetProperty(ref _NexNodeName, value);}
        }


       
    }
}



