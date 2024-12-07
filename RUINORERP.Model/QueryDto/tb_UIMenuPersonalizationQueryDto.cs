
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/05/2024 23:44:21
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
    /// 用户角色菜单个性化设置表一个角色用户菜单 三个字段为联合主键 就一行数据
    /// </summary>
    [Serializable()]
    [SugarTable("tb_UIMenuPersonalization")]
    public partial class tb_UIMenuPersonalizationQueryDto:BaseEntityDto
    {
        public tb_UIMenuPersonalizationQueryDto()
        {

        }

    
     

        private long _MenuID;
        /// <summary>
        /// 关联菜单
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuID",ColDesc = "关联菜单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "MenuID",IsNullable = false,ColumnDescription = "关联菜单" )]
        [FKRelationAttribute("tb_MenuInfo","MenuID")]
        public long MenuID 
        { 
            get{return _MenuID;}
            set{SetProperty(ref _MenuID, value);}
        }
     

        private long? _UserPersonalizedID;
        /// <summary>
        /// 用户角色设置
        /// </summary>
        [AdvQueryAttribute(ColName = "UserPersonalizedID",ColDesc = "用户角色设置")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "UserPersonalizedID",IsNullable = true,ColumnDescription = "用户角色设置" )]
        [FKRelationAttribute("tb_UserPersonalized","UserPersonalizedID")]
        public long? UserPersonalizedID 
        { 
            get{return _UserPersonalizedID;}
            set{SetProperty(ref _UserPersonalizedID, value);}
        }
     

        private int? _QueryConditionCols;
        /// <summary>
        /// 条件显示列数量
        /// </summary>
        [AdvQueryAttribute(ColName = "QueryConditionCols",ColDesc = "条件显示列数量")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "QueryConditionCols",IsNullable = true,ColumnDescription = "条件显示列数量" )]
        public int? QueryConditionCols 
        { 
            get{return _QueryConditionCols;}
            set{SetProperty(ref _QueryConditionCols, value);}
        }
     

        private bool? _IsRelatedQuerySettings= false;
        /// <summary>
        /// 是关联查询设置
        /// </summary>
        [AdvQueryAttribute(ColName = "IsRelatedQuerySettings",ColDesc = "是关联查询设置")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsRelatedQuerySettings",IsNullable = true,ColumnDescription = "是关联查询设置" )]
        public bool? IsRelatedQuerySettings 
        { 
            get{return _IsRelatedQuerySettings;}
            set{SetProperty(ref _IsRelatedQuerySettings, value);}
        }
     

        private int? _FavoritesMenu= ((0));
        /// <summary>
        /// 收藏菜单
        /// </summary>
        [AdvQueryAttribute(ColName = "FavoritesMenu",ColDesc = "收藏菜单")]
        [SugarColumn(ColumnDataType = "tinyint",SqlParameterDbType ="SByte",ColumnName = "FavoritesMenu",IsNullable = true,ColumnDescription = "收藏菜单" )]
        public int? FavoritesMenu 
        { 
            get{return _FavoritesMenu;}
            set{SetProperty(ref _FavoritesMenu, value);}
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
     

        private int? _Sort;
        /// <summary>
        /// 排序
        /// </summary>
        [AdvQueryAttribute(ColName = "Sort",ColDesc = "排序")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Sort",IsNullable = true,ColumnDescription = "排序" )]
        public int? Sort 
        { 
            get{return _Sort;}
            set{SetProperty(ref _Sort, value);}
        }
     

        private string _DefaultLayout;
        /// <summary>
        /// 默认布局
        /// </summary>
        [AdvQueryAttribute(ColName = "DefaultLayout",ColDesc = "默认布局")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "DefaultLayout",Length=2147483647,IsNullable = true,ColumnDescription = "默认布局" )]
        public string DefaultLayout 
        { 
            get{return _DefaultLayout;}
            set{SetProperty(ref _DefaultLayout, value);}
        }
     

        private string _DefaultLayout2;
        /// <summary>
        /// 默认布局
        /// </summary>
        [AdvQueryAttribute(ColName = "DefaultLayout2",ColDesc = "默认布局")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "DefaultLayout2",Length=2147483647,IsNullable = true,ColumnDescription = "默认布局" )]
        public string DefaultLayout2 
        { 
            get{return _DefaultLayout2;}
            set{SetProperty(ref _DefaultLayout2, value);}
        }


       
    }
}



