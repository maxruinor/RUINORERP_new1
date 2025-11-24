
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/24/2025 17:01:20
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
    /// 
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProcessNavigation")]
    public partial class tb_ProcessNavigationQueryDto:BaseEntityDto
    {
        public tb_ProcessNavigationQueryDto()
        {

        }

    
     

        private string _ProcessNavName;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ProcessNavName",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ProcessNavName",Length=200,IsNullable = true,ColumnDescription = "" )]
        public string ProcessNavName 
        { 
            get{return _ProcessNavName;}
            set{SetProperty(ref _ProcessNavName, value);}
        }
     

        private string _Description;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Description",Length=500,IsNullable = true,ColumnDescription = "" )]
        public string Description 
        { 
            get{return _Description;}
            set{SetProperty(ref _Description, value);}
        }
     

        private int _Version= ((1));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Version",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Version",IsNullable = false,ColumnDescription = "" )]
        public int Version 
        { 
            get{return _Version;}
            set{SetProperty(ref _Version, value);}
        }
     

        private string _GraphXml;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "GraphXml",ColDesc = "")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "GraphXml",Length=2147483647,IsNullable = true,ColumnDescription = "" )]
        public string GraphXml 
        { 
            get{return _GraphXml;}
            set{SetProperty(ref _GraphXml, value);}
        }
     

        private string _GraphJson;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "GraphJson",ColDesc = "")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "GraphJson",Length=2147483647,IsNullable = true,ColumnDescription = "" )]
        public string GraphJson 
        { 
            get{return _GraphJson;}
            set{SetProperty(ref _GraphJson, value);}
        }
     

        private long? _ModuleID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ModuleID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ModuleID",IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ModuleDefinition","ModuleID")]
        public long? ModuleID 
        { 
            get{return _ModuleID;}
            set{SetProperty(ref _ModuleID, value);}
        }
     

        private int _NavigationLevel= ((2));
        /// <summary>
        /// 层级深度（别名：HierarchyLevel）
        /// </summary>
        [AdvQueryAttribute(ColName = "NavigationLevel",ColDesc = "层级深度（别名：HierarchyLevel）")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "NavigationLevel",IsNullable = false,ColumnDescription = "层级深度（别名：HierarchyLevel）" )]
        public int NavigationLevel 
        { 
            get{return _NavigationLevel;}
            set{SetProperty(ref _NavigationLevel, value);}
        }
     

        private long? _ParentNavigationID;
        /// <summary>
        /// 父流程导航图ID（别名：ParentProcessNavID）
        /// </summary>
        [AdvQueryAttribute(ColName = "ParentNavigationID",ColDesc = "父流程导航图ID（别名：ParentProcessNavID）")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ParentNavigationID",IsNullable = true,ColumnDescription = "父流程导航图ID（别名：ParentProcessNavID）" )]
        [FKRelationAttribute("tb_ProcessNavigation","ParentNavigationID")]
        public long? ParentNavigationID 
        { 
            get{return _ParentNavigationID;}
            set{SetProperty(ref _ParentNavigationID, value);}
        }
     

        private int _HierarchyLevel= ((1));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "HierarchyLevel",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "HierarchyLevel",IsNullable = false,ColumnDescription = "" )]
        public int HierarchyLevel 
        { 
            get{return _HierarchyLevel;}
            set{SetProperty(ref _HierarchyLevel, value);}
        }
     

        private int _SortOrder= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "SortOrder",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "SortOrder",IsNullable = false,ColumnDescription = "" )]
        public int SortOrder 
        { 
            get{return _SortOrder;}
            set{SetProperty(ref _SortOrder, value);}
        }
     

        private long? _CreateUserID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "CreateUserID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CreateUserID",IsNullable = true,ColumnDescription = "" )]
        public long? CreateUserID 
        { 
            get{return _CreateUserID;}
            set{SetProperty(ref _CreateUserID, value);}
        }
     

        private bool _IsActive= true;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "IsActive",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsActive",IsNullable = false,ColumnDescription = "" )]
        public bool IsActive 
        { 
            get{return _IsActive;}
            set{SetProperty(ref _IsActive, value);}
        }
     

        private bool _IsDefault= false;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "IsDefault",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsDefault",IsNullable = false,ColumnDescription = "" )]
        public bool IsDefault 
        { 
            get{return _IsDefault;}
            set{SetProperty(ref _IsDefault, value);}
        }
     

        private string _Category;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Category",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Category",Length=100,IsNullable = true,ColumnDescription = "" )]
        public string Category 
        { 
            get{return _Category;}
            set{SetProperty(ref _Category, value);}
        }
     

        private string _Tags;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Tags",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Tags",Length=300,IsNullable = true,ColumnDescription = "" )]
        public string Tags 
        { 
            get{return _Tags;}
            set{SetProperty(ref _Tags, value);}
        }
     

        private DateTime _CreateTime;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "CreateTime",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "CreateTime",IsNullable = false,ColumnDescription = "" )]
        public DateTime CreateTime 
        { 
            get{return _CreateTime;}
            set{SetProperty(ref _CreateTime, value);}
        }
     

        private DateTime _UpdateTime;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "UpdateTime",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "UpdateTime",IsNullable = false,ColumnDescription = "" )]
        public DateTime UpdateTime 
        { 
            get{return _UpdateTime;}
            set{SetProperty(ref _UpdateTime, value);}
        }


       
    }
}



