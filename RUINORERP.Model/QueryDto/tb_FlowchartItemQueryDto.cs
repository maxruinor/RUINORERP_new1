
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
    /// 流程图子项
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FlowchartItem")]
    public partial class tb_FlowchartItemQueryDto:BaseEntityDto
    {
        public tb_FlowchartItemQueryDto()
        {

        }

    
     

        private string _IconFile_Path;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "IconFile_Path",ColDesc = "")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "IconFile_Path",Length=500,IsNullable = true,ColumnDescription = "" )]
        public string IconFile_Path 
        { 
            get{return _IconFile_Path;}
            set{SetProperty(ref _IconFile_Path, value);}
        }
     

        private string _Title;
        /// <summary>
        /// 标题
        /// </summary>
        [AdvQueryAttribute(ColName = "Title",ColDesc = "标题")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "Title",Length=100,IsNullable = true,ColumnDescription = "标题" )]
        public string Title 
        { 
            get{return _Title;}
            set{SetProperty(ref _Title, value);}
        }
     

        private string _SizeString;
        /// <summary>
        /// 大小
        /// </summary>
        [AdvQueryAttribute(ColName = "SizeString",ColDesc = "大小")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "SizeString",Length=100,IsNullable = true,ColumnDescription = "大小" )]
        public string SizeString 
        { 
            get{return _SizeString;}
            set{SetProperty(ref _SizeString, value);}
        }
     

        private string _PointToString;
        /// <summary>
        /// 位置
        /// </summary>
        [AdvQueryAttribute(ColName = "PointToString",ColDesc = "位置")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "PointToString",Length=100,IsNullable = true,ColumnDescription = "位置" )]
        public string PointToString 
        { 
            get{return _PointToString;}
            set{SetProperty(ref _PointToString, value);}
        }


       
    }
}



