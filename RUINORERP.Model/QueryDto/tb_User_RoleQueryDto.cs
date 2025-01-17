
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/17/2025 11:45:03
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
    /// 用户角色关系表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_User_Role")]
    public partial class tb_User_RoleQueryDto:BaseEntityDto
    {
        public tb_User_RoleQueryDto()
        {

        }

    
     

        private long _User_ID;
        /// <summary>
        /// 用户
        /// </summary>
        [AdvQueryAttribute(ColName = "User_ID",ColDesc = "用户")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "User_ID",IsNullable = false,ColumnDescription = "用户" )]
        [FKRelationAttribute("tb_UserInfo","User_ID")]
        public long User_ID 
        { 
            get{return _User_ID;}
            set{SetProperty(ref _User_ID, value);}
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
     

        private bool _Authorized= false;
        /// <summary>
        /// 已授权
        /// </summary>
        [AdvQueryAttribute(ColName = "Authorized",ColDesc = "已授权")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Authorized",IsNullable = false,ColumnDescription = "已授权" )]
        public bool Authorized 
        { 
            get{return _Authorized;}
            set{SetProperty(ref _Authorized, value);}
        }
     

        private bool _DefaultRole= false;
        /// <summary>
        /// 默认角色
        /// </summary>
        [AdvQueryAttribute(ColName = "DefaultRole",ColDesc = "默认角色")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "DefaultRole",IsNullable = false,ColumnDescription = "默认角色" )]
        public bool DefaultRole 
        { 
            get{return _DefaultRole;}
            set{SetProperty(ref _DefaultRole, value);}
        }


       
    }
}



