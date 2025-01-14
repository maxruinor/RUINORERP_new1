﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:35
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
    /// 文档表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Documents")]
    public partial class tb_DocumentsQueryDto:BaseEntityDto
    {
        public tb_DocumentsQueryDto()
        {

        }

    
     

        private string _Files_Path;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Files_Path",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Files_Path",Length=500,IsNullable = true,ColumnDescription = "" )]
        public string Files_Path 
        { 
            get{return _Files_Path;}
            set{SetProperty(ref _Files_Path, value);}
        }


       
    }
}



