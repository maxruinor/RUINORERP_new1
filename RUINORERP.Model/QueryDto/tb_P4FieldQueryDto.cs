
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:00
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
    /// 字段权限表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_P4Field")]
    public partial class tb_P4FieldQueryDto:BaseEntityDto
    {
        public tb_P4FieldQueryDto()
        {

        }

    
     

        private long? _FieldInfo_ID;
        /// <summary>
        /// 字段
        /// </summary>
        [AdvQueryAttribute(ColName = "FieldInfo_ID",ColDesc = "字段")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "FieldInfo_ID",IsNullable = true,ColumnDescription = "字段" )]
        [FKRelationAttribute("tb_FieldInfo","FieldInfo_ID")]
        public long? FieldInfo_ID 
        { 
            get{return _FieldInfo_ID;}
            set{SetProperty(ref _FieldInfo_ID, value);}
        }
     

        private long? _RoleID;
        /// <summary>
        /// 角色
        /// </summary>
        [AdvQueryAttribute(ColName = "RoleID",ColDesc = "角色")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "RoleID",IsNullable = true,ColumnDescription = "角色" )]
        [FKRelationAttribute("tb_RoleInfo","RoleID")]
        public long? RoleID 
        { 
            get{return _RoleID;}
            set{SetProperty(ref _RoleID, value);}
        }
     

        private long? _MenuID;
        /// <summary>
        /// 菜单
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuID",ColDesc = "菜单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "MenuID",IsNullable = true,ColumnDescription = "菜单" )]
        [FKRelationAttribute("tb_MenuInfo","MenuID")]
        public long? MenuID 
        { 
            get{return _MenuID;}
            set{SetProperty(ref _MenuID, value);}
        }
     

        private bool _IsVisble;
        /// <summary>
        /// 是否可见
        /// </summary>
        [AdvQueryAttribute(ColName = "IsVisble",ColDesc = "是否可见")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsVisble",IsNullable = false,ColumnDescription = "是否可见" )]
        public bool IsVisble 
        { 
            get{return _IsVisble;}
            set{SetProperty(ref _IsVisble, value);}
        }
     

        private bool _CanReadWrite;
        /// <summary>
        /// 可读写
        /// </summary>
        [AdvQueryAttribute(ColName = "CanReadWrite",ColDesc = "可读写")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "CanReadWrite",IsNullable = false,ColumnDescription = "可读写" )]
        public bool CanReadWrite 
        { 
            get{return _CanReadWrite;}
            set{SetProperty(ref _CanReadWrite, value);}
        }
     

        private bool _OnlyRead;
        /// <summary>
        /// 只读
        /// </summary>
        [AdvQueryAttribute(ColName = "OnlyRead",ColDesc = "只读")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "OnlyRead",IsNullable = false,ColumnDescription = "只读" )]
        public bool OnlyRead 
        { 
            get{return _OnlyRead;}
            set{SetProperty(ref _OnlyRead, value);}
        }
     

        private bool _HideValue;
        /// <summary>
        /// 隐藏值
        /// </summary>
        [AdvQueryAttribute(ColName = "HideValue",ColDesc = "隐藏值")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "HideValue",IsNullable = false,ColumnDescription = "隐藏值" )]
        public bool HideValue 
        { 
            get{return _HideValue;}
            set{SetProperty(ref _HideValue, value);}
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



