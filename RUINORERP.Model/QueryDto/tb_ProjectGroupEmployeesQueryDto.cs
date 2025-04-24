
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 14:14:54
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
    /// 项目及成员关系表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProjectGroupEmployees")]
    public partial class tb_ProjectGroupEmployeesQueryDto:BaseEntityDto
    {
        public tb_ProjectGroupEmployeesQueryDto()
        {

        }

    
     

        private long _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProjectGroup_ID",IsNullable = false,ColumnDescription = "项目组" )]
        [FKRelationAttribute("tb_ProjectGroup","ProjectGroup_ID")]
        public long ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}
            set{SetProperty(ref _ProjectGroup_ID, value);}
        }
     

        private long _Employee_ID;
        /// <summary>
        /// 员工
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "员工")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = false,ColumnDescription = "员工" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private bool _Assigned= false;
        /// <summary>
        /// 已分配
        /// </summary>
        [AdvQueryAttribute(ColName = "Assigned",ColDesc = "已分配")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Assigned",IsNullable = false,ColumnDescription = "已分配" )]
        public bool Assigned 
        { 
            get{return _Assigned;}
            set{SetProperty(ref _Assigned, value);}
        }
     

        private bool _DefaultGroup= false;
        /// <summary>
        /// 默认组
        /// </summary>
        [AdvQueryAttribute(ColName = "DefaultGroup",ColDesc = "默认组")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "DefaultGroup",IsNullable = false,ColumnDescription = "默认组" )]
        public bool DefaultGroup 
        { 
            get{return _DefaultGroup;}
            set{SetProperty(ref _DefaultGroup, value);}
        }


       
    }
}



