
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:33
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
    /// 步骤定义
    /// </summary>
    [Serializable()]
    [SugarTable("tb_StepBody")]
    public partial class tb_StepBodyQueryDto:BaseEntityDto
    {
        public tb_StepBodyQueryDto()
        {

        }

    
     

        private long? _Para_Id;
        /// <summary>
        /// 输入参数
        /// </summary>
        [AdvQueryAttribute(ColName = "Para_Id",ColDesc = "输入参数")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Para_Id",IsNullable = true,ColumnDescription = "输入参数" )]
        [FKRelationAttribute("tb_StepBodyPara","Para_Id")]
        public long? Para_Id 
        { 
            get{return _Para_Id;}
            set{SetProperty(ref _Para_Id, value);}
        }
     

        private string _Name;
        /// <summary>
        /// 名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Name",ColDesc = "名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Name",Length=50,IsNullable = true,ColumnDescription = "名称" )]
        public string Name 
        { 
            get{return _Name;}
            set{SetProperty(ref _Name, value);}
        }
     

        private string _DisplayName;
        /// <summary>
        /// 显示名称
        /// </summary>
        [AdvQueryAttribute(ColName = "DisplayName",ColDesc = "显示名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "DisplayName",Length=50,IsNullable = true,ColumnDescription = "显示名称" )]
        public string DisplayName 
        { 
            get{return _DisplayName;}
            set{SetProperty(ref _DisplayName, value);}
        }
     

        private string _TypeFullName;
        /// <summary>
        /// 类型全名
        /// </summary>
        [AdvQueryAttribute(ColName = "TypeFullName",ColDesc = "类型全名")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "TypeFullName",Length=50,IsNullable = true,ColumnDescription = "类型全名" )]
        public string TypeFullName 
        { 
            get{return _TypeFullName;}
            set{SetProperty(ref _TypeFullName, value);}
        }
     

        private string _AssemblyFullName;
        /// <summary>
        /// 标题
        /// </summary>
        [AdvQueryAttribute(ColName = "AssemblyFullName",ColDesc = "标题")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "AssemblyFullName",Length=50,IsNullable = true,ColumnDescription = "标题" )]
        public string AssemblyFullName 
        { 
            get{return _AssemblyFullName;}
            set{SetProperty(ref _AssemblyFullName, value);}
        }


       
    }
}



