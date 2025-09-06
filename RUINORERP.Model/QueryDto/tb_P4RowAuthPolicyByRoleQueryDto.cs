
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/06/2025 15:41:57
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
    /// 行级权限规则-角色关联表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_P4RowAuthPolicyByRole")]
    public partial class tb_P4RowAuthPolicyByRoleQueryDto:BaseEntityDto
    {
        public tb_P4RowAuthPolicyByRoleQueryDto()
        {

        }

    
     

        private long _PolicyId;
        /// <summary>
        /// 数据权限规则
        /// </summary>
        [AdvQueryAttribute(ColName = "PolicyId",ColDesc = "数据权限规则")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PolicyId",IsNullable = false,ColumnDescription = "数据权限规则" )]
        [FKRelationAttribute("tb_RowAuthPolicy","PolicyId")]
        public long PolicyId 
        { 
            get{return _PolicyId;}
            set{SetProperty(ref _PolicyId, value);}
        }
     

        private long _RoleID;
        /// <summary>
        /// 角色
        /// </summary>
        [AdvQueryAttribute(ColName = "RoleID",ColDesc = "角色")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "RoleID",IsNullable = false,ColumnDescription = "角色" )]
        [FKRelationAttribute("tb_RoleInfo","RoleID")]
        public long RoleID 
        { 
            get{return _RoleID;}
            set{SetProperty(ref _RoleID, value);}
        }
     

        private long _MenuID;
        /// <summary>
        /// 菜单
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuID",ColDesc = "菜单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "MenuID",IsNullable = false,ColumnDescription = "菜单" )]
        [FKRelationAttribute("tb_MenuInfo","MenuID")]
        public long MenuID 
        { 
            get{return _MenuID;}
            set{SetProperty(ref _MenuID, value);}
        }
     

        private bool _IsEnabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "IsEnabled",ColDesc = "是否启用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsEnabled",IsNullable = false,ColumnDescription = "是否启用" )]
        public bool IsEnabled 
        { 
            get{return _IsEnabled;}
            set{SetProperty(ref _IsEnabled, value);}
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


       
    }
}



