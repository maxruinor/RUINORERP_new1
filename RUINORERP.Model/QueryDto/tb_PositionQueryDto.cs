
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:02
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
    /// 位置信息
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Position")]
    public partial class tb_PositionQueryDto:BaseEntityDto
    {
        public tb_PositionQueryDto()
        {

        }

    
     

        private string _Left;
        /// <summary>
        /// 左边距
        /// </summary>
        [AdvQueryAttribute(ColName = "Left",ColDesc = "左边距")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Left",Length=50,IsNullable = false,ColumnDescription = "左边距" )]
        public string Left 
        { 
            get{return _Left;}
            set{SetProperty(ref _Left, value);}
        }
     

        private string _Right;
        /// <summary>
        /// 右边距
        /// </summary>
        [AdvQueryAttribute(ColName = "Right",ColDesc = "右边距")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Right",Length=50,IsNullable = true,ColumnDescription = "右边距" )]
        public string Right 
        { 
            get{return _Right;}
            set{SetProperty(ref _Right, value);}
        }
     

        private string _Bottom;
        /// <summary>
        /// 下边距
        /// </summary>
        [AdvQueryAttribute(ColName = "Bottom",ColDesc = "下边距")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Bottom",Length=50,IsNullable = true,ColumnDescription = "下边距" )]
        public string Bottom 
        { 
            get{return _Bottom;}
            set{SetProperty(ref _Bottom, value);}
        }
     

        private string _Top;
        /// <summary>
        /// 上边距
        /// </summary>
        [AdvQueryAttribute(ColName = "Top",ColDesc = "上边距")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Top",Length=50,IsNullable = true,ColumnDescription = "上边距" )]
        public string Top 
        { 
            get{return _Top;}
            set{SetProperty(ref _Top, value);}
        }


       
    }
}



