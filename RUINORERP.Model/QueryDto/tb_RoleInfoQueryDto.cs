
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:26
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
    /// 角色表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_RoleInfo")]
    public partial class tb_RoleInfoQueryDto:BaseEntityDto
    {
        public tb_RoleInfoQueryDto()
        {

        }

    
     

        private string _RoleName;
        /// <summary>
        /// 角色名称
        /// </summary>
        [AdvQueryAttribute(ColName = "RoleName",ColDesc = "角色名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "RoleName",Length=50,IsNullable = false,ColumnDescription = "角色名称" )]
        public string RoleName 
        { 
            get{return _RoleName;}
            set{SetProperty(ref _RoleName, value);}
        }
     

        private string _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Desc",ColDesc = "描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Desc",Length=250,IsNullable = true,ColumnDescription = "描述" )]
        public string Desc 
        { 
            get{return _Desc;}
            set{SetProperty(ref _Desc, value);}
        }
     

        private long? _RolePropertyID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "RolePropertyID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "RolePropertyID",IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_RolePropertyConfig","RolePropertyID")]
        public long? RolePropertyID 
        { 
            get{return _RolePropertyID;}
            set{SetProperty(ref _RolePropertyID, value);}
        }


       
    }
}



