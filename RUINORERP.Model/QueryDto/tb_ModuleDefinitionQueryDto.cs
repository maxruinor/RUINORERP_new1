
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:53
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
    /// 功能模块定义（仅限部分已经硬码并体现于菜单表中）
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ModuleDefinition")]
    public partial class tb_ModuleDefinitionQueryDto:BaseEntityDto
    {
        public tb_ModuleDefinitionQueryDto()
        {

        }

    
     

        private string _ModuleNo;
        /// <summary>
        /// 模块编号
        /// </summary>
        [AdvQueryAttribute(ColName = "ModuleNo",ColDesc = "模块编号")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "ModuleNo",Length=50,IsNullable = false,ColumnDescription = "模块编号" )]
        public string ModuleNo 
        { 
            get{return _ModuleNo;}
            set{SetProperty(ref _ModuleNo, value);}
        }
     

        private string _ModuleName;
        /// <summary>
        /// 模块名称
        /// </summary>
        [AdvQueryAttribute(ColName = "ModuleName",ColDesc = "模块名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ModuleName",Length=20,IsNullable = false,ColumnDescription = "模块名称" )]
        public string ModuleName 
        { 
            get{return _ModuleName;}
            set{SetProperty(ref _ModuleName, value);}
        }
     

        private bool _Visible;
        /// <summary>
        /// 是否可见
        /// </summary>
        [AdvQueryAttribute(ColName = "Visible",ColDesc = "是否可见")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Visible",IsNullable = false,ColumnDescription = "是否可见" )]
        public bool Visible 
        { 
            get{return _Visible;}
            set{SetProperty(ref _Visible, value);}
        }
     

        private bool? _Available;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Available",ColDesc = "是否可用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Available",IsNullable = true,ColumnDescription = "是否可用" )]
        public bool? Available 
        { 
            get{return _Available;}
            set{SetProperty(ref _Available, value);}
        }
     

        private string _IconFile_Path;
        /// <summary>
        /// 图标路径
        /// </summary>
        [AdvQueryAttribute(ColName = "IconFile_Path",ColDesc = "图标路径")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "IconFile_Path",Length=100,IsNullable = true,ColumnDescription = "图标路径" )]
        public string IconFile_Path 
        { 
            get{return _IconFile_Path;}
            set{SetProperty(ref _IconFile_Path, value);}
        }


       
    }
}



