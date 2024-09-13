
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:39
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
    /// 流程图线
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FlowchartLine")]
    public partial class tb_FlowchartLineQueryDto:BaseEntityDto
    {
        public tb_FlowchartLineQueryDto()
        {

        }

    
     

        private string _PointToString1;
        /// <summary>
        /// 大小
        /// </summary>
        [AdvQueryAttribute(ColName = "PointToString1",ColDesc = "大小")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "PointToString1",Length=100,IsNullable = true,ColumnDescription = "大小" )]
        public string PointToString1 
        { 
            get{return _PointToString1;}
            set{SetProperty(ref _PointToString1, value);}
        }
     

        private string _PointToString2;
        /// <summary>
        /// 位置
        /// </summary>
        [AdvQueryAttribute(ColName = "PointToString2",ColDesc = "位置")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "PointToString2",Length=100,IsNullable = true,ColumnDescription = "位置" )]
        public string PointToString2 
        { 
            get{return _PointToString2;}
            set{SetProperty(ref _PointToString2, value);}
        }


       
    }
}



