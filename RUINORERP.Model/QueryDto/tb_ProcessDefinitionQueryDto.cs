
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:08
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
    /// 流程定义 http://www.phpheidong.com/blog/article/68471/a3129f742e5e396e3d1e/
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProcessDefinition")]
    public partial class tb_ProcessDefinitionQueryDto:BaseEntityDto
    {
        public tb_ProcessDefinitionQueryDto()
        {

        }

    
     

        private long? _Step_Id;
        /// <summary>
        /// 流程定义
        /// </summary>
        [AdvQueryAttribute(ColName = "Step_Id",ColDesc = "流程定义")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Step_Id",IsNullable = true,ColumnDescription = "流程定义" )]
        [FKRelationAttribute("tb_ProcessStep","Step_Id")]
        public long? Step_Id 
        { 
            get{return _Step_Id;}
            set{SetProperty(ref _Step_Id, value);}
        }
     

        private string _Version;
        /// <summary>
        /// 版本
        /// </summary>
        [AdvQueryAttribute(ColName = "Version",ColDesc = "版本")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Version",Length=50,IsNullable = false,ColumnDescription = "版本" )]
        public string Version 
        { 
            get{return _Version;}
            set{SetProperty(ref _Version, value);}
        }
     

        private string _Title;
        /// <summary>
        /// 标题
        /// </summary>
        [AdvQueryAttribute(ColName = "Title",ColDesc = "标题")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Title",Length=50,IsNullable = true,ColumnDescription = "标题" )]
        public string Title 
        { 
            get{return _Title;}
            set{SetProperty(ref _Title, value);}
        }
     

        private string _Color;
        /// <summary>
        /// 颜色
        /// </summary>
        [AdvQueryAttribute(ColName = "Color",ColDesc = "颜色")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Color",Length=50,IsNullable = true,ColumnDescription = "颜色" )]
        public string Color 
        { 
            get{return _Color;}
            set{SetProperty(ref _Color, value);}
        }
     

        private string _Icon;
        /// <summary>
        /// 图标
        /// </summary>
        [AdvQueryAttribute(ColName = "Icon",ColDesc = "图标")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Icon",Length=250,IsNullable = true,ColumnDescription = "图标" )]
        public string Icon 
        { 
            get{return _Icon;}
            set{SetProperty(ref _Icon, value);}
        }
     

        private string _Description;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Description",Length=255,IsNullable = true,ColumnDescription = "描述" )]
        public string Description 
        { 
            get{return _Description;}
            set{SetProperty(ref _Description, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



