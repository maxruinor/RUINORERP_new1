
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
    /// 项目组信息 用于业务分组小团队
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProjectGroup")]
    public partial class tb_ProjectGroupQueryDto:BaseEntityDto
    {
        public tb_ProjectGroupQueryDto()
        {

        }

    
     

        private long? _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "部门")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "部门" )]
        [FKRelationAttribute("tb_Department","DepartmentID")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}
            set{SetProperty(ref _DepartmentID, value);}
        }
     

        private string _ProjectGroupCode;
        /// <summary>
        /// 项目组代号
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroupCode",ColDesc = "项目组代号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ProjectGroupCode",Length=50,IsNullable = true,ColumnDescription = "项目组代号" )]
        public string ProjectGroupCode 
        { 
            get{return _ProjectGroupCode;}
            set{SetProperty(ref _ProjectGroupCode, value);}
        }
     

        private string _ProjectGroupName;
        /// <summary>
        /// 项目组名称
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroupName",ColDesc = "项目组名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ProjectGroupName",Length=50,IsNullable = true,ColumnDescription = "项目组名称" )]
        public string ProjectGroupName 
        { 
            get{return _ProjectGroupName;}
            set{SetProperty(ref _ProjectGroupName, value);}
        }
     

        private string _ResponsiblePerson;
        /// <summary>
        /// 负责人
        /// </summary>
        [AdvQueryAttribute(ColName = "ResponsiblePerson",ColDesc = "负责人")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ResponsiblePerson",Length=50,IsNullable = true,ColumnDescription = "负责人" )]
        public string ResponsiblePerson 
        { 
            get{return _ResponsiblePerson;}
            set{SetProperty(ref _ResponsiblePerson, value);}
        }
     

        private string _Phone;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Phone",ColDesc = "电话")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Phone",Length=255,IsNullable = true,ColumnDescription = "电话" )]
        public string Phone 
        { 
            get{return _Phone;}
            set{SetProperty(ref _Phone, value);}
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
     

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
            set{SetProperty(ref _Created_at, value);}
        }
     

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by 
        { 
            get{return _Created_by;}
            set{SetProperty(ref _Created_by, value);}
        }
     

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
            set{SetProperty(ref _Modified_at, value);}
        }
     

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
            set{SetProperty(ref _Modified_by, value);}
        }
     

        private DateTime? _StartDate;
        /// <summary>
        /// 启动时间
        /// </summary>
        [AdvQueryAttribute(ColName = "StartDate",ColDesc = "启动时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "StartDate",IsNullable = true,ColumnDescription = "启动时间" )]
        public DateTime? StartDate 
        { 
            get{return _StartDate;}
            set{SetProperty(ref _StartDate, value);}
        }
     

        private bool? _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_enabled",IsNullable = true,ColumnDescription = "是否启用" )]
        public bool? Is_enabled 
        { 
            get{return _Is_enabled;}
            set{SetProperty(ref _Is_enabled, value);}
        }
     

        private DateTime? _EndDate;
        /// <summary>
        /// 结束时间
        /// </summary>
        [AdvQueryAttribute(ColName = "EndDate",ColDesc = "结束时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "EndDate",IsNullable = true,ColumnDescription = "结束时间" )]
        public DateTime? EndDate 
        { 
            get{return _EndDate;}
            set{SetProperty(ref _EndDate, value);}
        }


       
    }
}



