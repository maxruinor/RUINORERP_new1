
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:34
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
    /// 图片表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Images")]
    public partial class tb_ImagesQueryDto:BaseEntityDto
    {
        public tb_ImagesQueryDto()
        {

        }

    
     

        private string _Images;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Images",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Images",Length=255,IsNullable = true,ColumnDescription = "" )]
        public string Images 
        { 
            get{return _Images;}
            set{SetProperty(ref _Images, value);}
        }
     

        private string _Images_Path;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Images_Path",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Images_Path",Length=500,IsNullable = true,ColumnDescription = "" )]
        public string Images_Path 
        { 
            get{return _Images_Path;}
            set{SetProperty(ref _Images_Path, value);}
        }


       
    }
}



