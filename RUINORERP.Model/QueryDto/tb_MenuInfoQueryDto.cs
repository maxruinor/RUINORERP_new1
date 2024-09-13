
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:54
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
    /// 菜单程序集信息表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_MenuInfo")]
    public partial class tb_MenuInfoQueryDto:BaseEntityDto
    {
        public tb_MenuInfoQueryDto()
        {

        }

    
     

        private long? _ModuleID;
        /// <summary>
        /// 模块
        /// </summary>
        [AdvQueryAttribute(ColName = "ModuleID",ColDesc = "模块")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ModuleID",IsNullable = true,ColumnDescription = "模块" )]
        [FKRelationAttribute("tb_ModuleDefinition","ModuleID")]
        public long? ModuleID 
        { 
            get{return _ModuleID;}
            set{SetProperty(ref _ModuleID, value);}
        }
     

        private string _MenuName;
        /// <summary>
        /// 菜单名称
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuName",ColDesc = "菜单名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "MenuName",Length=255,IsNullable = true,ColumnDescription = "菜单名称" )]
        public string MenuName 
        { 
            get{return _MenuName;}
            set{SetProperty(ref _MenuName, value);}
        }
     

        private string _MenuType;
        /// <summary>
        /// 菜单类型
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuType",ColDesc = "菜单类型")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "MenuType",Length=20,IsNullable = false,ColumnDescription = "菜单类型" )]
        public string MenuType 
        { 
            get{return _MenuType;}
            set{SetProperty(ref _MenuType, value);}
        }
     

        private string _BIBaseForm;
        /// <summary>
        /// 注入基类
        /// </summary>
        [AdvQueryAttribute(ColName = "BIBaseForm",ColDesc = "注入基类")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "BIBaseForm",Length=100,IsNullable = true,ColumnDescription = "注入基类" )]
        public string BIBaseForm 
        { 
            get{return _BIBaseForm;}
            set{SetProperty(ref _BIBaseForm, value);}
        }
     

        private int? _BizType;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BizType",ColDesc = "业务类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "BizType",IsNullable = true,ColumnDescription = "业务类型" )]
        public int? BizType 
        { 
            get{return _BizType;}
            set{SetProperty(ref _BizType, value);}
        }
     

        private int? _UIType;
        /// <summary>
        /// 窗体类型
        /// </summary>
        [AdvQueryAttribute(ColName = "UIType",ColDesc = "窗体类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "UIType",IsNullable = true,ColumnDescription = "窗体类型" )]
        public int? UIType 
        { 
            get{return _UIType;}
            set{SetProperty(ref _UIType, value);}
        }
     

        private string _CaptionCN;
        /// <summary>
        /// 中文显示
        /// </summary>
        [AdvQueryAttribute(ColName = "CaptionCN",ColDesc = "中文显示")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CaptionCN",Length=250,IsNullable = true,ColumnDescription = "中文显示" )]
        public string CaptionCN 
        { 
            get{return _CaptionCN;}
            set{SetProperty(ref _CaptionCN, value);}
        }
     

        private string _CaptionEN;
        /// <summary>
        /// 英文显示
        /// </summary>
        [AdvQueryAttribute(ColName = "CaptionEN",ColDesc = "英文显示")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "CaptionEN",Length=250,IsNullable = true,ColumnDescription = "英文显示" )]
        public string CaptionEN 
        { 
            get{return _CaptionEN;}
            set{SetProperty(ref _CaptionEN, value);}
        }
     

        private string _FormName;
        /// <summary>
        /// 窗体名称
        /// </summary>
        [AdvQueryAttribute(ColName = "FormName",ColDesc = "窗体名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "FormName",Length=255,IsNullable = true,ColumnDescription = "窗体名称" )]
        public string FormName 
        { 
            get{return _FormName;}
            set{SetProperty(ref _FormName, value);}
        }
     

        private string _ClassPath;
        /// <summary>
        /// 类路径
        /// </summary>
        [AdvQueryAttribute(ColName = "ClassPath",ColDesc = "类路径")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ClassPath",Length=500,IsNullable = true,ColumnDescription = "类路径" )]
        public string ClassPath 
        { 
            get{return _ClassPath;}
            set{SetProperty(ref _ClassPath, value);}
        }
     

        private string _EntityName;
        /// <summary>
        /// 关联实体名
        /// </summary>
        [AdvQueryAttribute(ColName = "EntityName",ColDesc = "关联实体名")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "EntityName",Length=100,IsNullable = true,ColumnDescription = "关联实体名" )]
        public string EntityName 
        { 
            get{return _EntityName;}
            set{SetProperty(ref _EntityName, value);}
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
     

        private long? _Parent_id;
        /// <summary>
        /// 父ID
        /// </summary>
        [AdvQueryAttribute(ColName = "Parent_id",ColDesc = "父ID")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Parent_id",IsNullable = true,ColumnDescription = "父ID" )]
        public long? Parent_id 
        { 
            get{return _Parent_id;}
            set{SetProperty(ref _Parent_id, value);}
        }
     

        private string _Discription;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Discription",ColDesc = "描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Discription",Length=250,IsNullable = true,ColumnDescription = "描述" )]
        public string Discription 
        { 
            get{return _Discription;}
            set{SetProperty(ref _Discription, value);}
        }
     

        private string _MenuNo;
        /// <summary>
        /// 菜单编码
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuNo",ColDesc = "菜单编码")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "MenuNo",Length=250,IsNullable = true,ColumnDescription = "菜单编码" )]
        public string MenuNo 
        { 
            get{return _MenuNo;}
            set{SetProperty(ref _MenuNo, value);}
        }
     

        private int? _MenuLevel;
        /// <summary>
        /// 菜单级别
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuLevel",ColDesc = "菜单级别")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "MenuLevel",IsNullable = true,ColumnDescription = "菜单级别" )]
        public int? MenuLevel 
        { 
            get{return _MenuLevel;}
            set{SetProperty(ref _MenuLevel, value);}
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
     

        private int _Sort;
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "排序")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Sort",IsNullable = false,ColumnDescription = "排序" )]
        public int Sort 
        { 
            get{return _Sort;}
            set{SetProperty(ref _Sort, value);}
        }
     

        private string _HotKey;
        /// <summary>
        /// 热键
        /// </summary>
        [AdvQueryAttribute(ColName = "HotKey",ColDesc = "热键")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "HotKey",Length=50,IsNullable = true,ColumnDescription = "热键" )]
        public string HotKey 
        { 
            get{return _HotKey;}
            set{SetProperty(ref _HotKey, value);}
        }
     

        private string _DefaultLayout;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "DefaultLayout",ColDesc = "")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "DefaultLayout",Length=2147483647,IsNullable = true,ColumnDescription = "" )]
        public string DefaultLayout 
        { 
            get{return _DefaultLayout;}
            set{SetProperty(ref _DefaultLayout, value);}
        }


       
    }
}



