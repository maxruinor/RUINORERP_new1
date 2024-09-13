
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:41
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
    /// 工作台配置表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_WorkCenterConfig")]
    public partial class tb_WorkCenterConfigQueryDto:BaseEntityDto
    {
        public tb_WorkCenterConfigQueryDto()
        {

        }

    
     

        private long _RoleID;
        /// <summary>
        /// 角色
        /// </summary>
        [AdvQueryAttribute(ColName = "RoleID",ColDesc = "角色")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "RoleID",IsNullable = false,ColumnDescription = "角色" )]
        public long RoleID 
        { 
            get{return _RoleID;}
            set{SetProperty(ref _RoleID, value);}
        }
     

        private long? _User_ID;
        /// <summary>
        /// 用户
        /// </summary>
        [AdvQueryAttribute(ColName = "User_ID",ColDesc = "用户")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "User_ID",IsNullable = true,ColumnDescription = "用户" )]
        public long? User_ID 
        { 
            get{return _User_ID;}
            set{SetProperty(ref _User_ID, value);}
        }
     

        private bool _Operable= false;
        /// <summary>
        /// 可操作
        /// </summary>
        [AdvQueryAttribute(ColName = "Operable",ColDesc = "可操作")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Operable",IsNullable = false,ColumnDescription = "可操作" )]
        public bool Operable 
        { 
            get{return _Operable;}
            set{SetProperty(ref _Operable, value);}
        }
     

        private bool _OnlyDisplay= false;
        /// <summary>
        /// 仅展示
        /// </summary>
        [AdvQueryAttribute(ColName = "OnlyDisplay",ColDesc = "仅展示")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "OnlyDisplay",IsNullable = false,ColumnDescription = "仅展示" )]
        public bool OnlyDisplay 
        { 
            get{return _OnlyDisplay;}
            set{SetProperty(ref _OnlyDisplay, value);}
        }
     

        private string _ToDoList;
        /// <summary>
        /// 待办事项
        /// </summary>
        [AdvQueryAttribute(ColName = "ToDoList",ColDesc = "待办事项")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "ToDoList",Length=500,IsNullable = true,ColumnDescription = "待办事项" )]
        public string ToDoList 
        { 
            get{return _ToDoList;}
            set{SetProperty(ref _ToDoList, value);}
        }
     

        private string _FrequentlyMenus;
        /// <summary>
        /// 常用菜单
        /// </summary>
        [AdvQueryAttribute(ColName = "FrequentlyMenus",ColDesc = "常用菜单")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "FrequentlyMenus",Length=200,IsNullable = true,ColumnDescription = "常用菜单" )]
        public string FrequentlyMenus 
        { 
            get{return _FrequentlyMenus;}
            set{SetProperty(ref _FrequentlyMenus, value);}
        }
     

        private string _DataOverview;
        /// <summary>
        /// 数据概览
        /// </summary>
        [AdvQueryAttribute(ColName = "DataOverview",ColDesc = "数据概览")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "DataOverview",Length=500,IsNullable = true,ColumnDescription = "数据概览" )]
        public string DataOverview 
        { 
            get{return _DataOverview;}
            set{SetProperty(ref _DataOverview, value);}
        }


       
    }
}



