
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:48
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
    /// 字段信息表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ButtonInfo")]
    public partial class tb_ButtonInfoQueryDto:BaseEntityDto
    {
        public tb_ButtonInfoQueryDto()
        {

        }

    
     

        private long? _MenuID;
        /// <summary>
        /// 所属菜单
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuID",ColDesc = "所属菜单")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "MenuID",IsNullable = true,ColumnDescription = "所属菜单" )]
        [FKRelationAttribute("tb_MenuInfo","MenuID")]
        public long? MenuID 
        { 
            get{return _MenuID;}
            set{SetProperty(ref _MenuID, value);}
        }
     

        private string _BtnName;
        /// <summary>
        /// 按钮名称
        /// </summary>
        [AdvQueryAttribute(ColName = "BtnName",ColDesc = "按钮名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "BtnName",Length=255,IsNullable = true,ColumnDescription = "按钮名称" )]
        public string BtnName 
        { 
            get{return _BtnName;}
            set{SetProperty(ref _BtnName, value);}
        }
     

        private string _BtnText;
        /// <summary>
        /// 按钮文本
        /// </summary>
        [AdvQueryAttribute(ColName = "BtnText",ColDesc = "按钮文本")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "BtnText",Length=250,IsNullable = true,ColumnDescription = "按钮文本" )]
        public string BtnText 
        { 
            get{return _BtnText;}
            set{SetProperty(ref _BtnText, value);}
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
     

        private bool? _IsForm;
        /// <summary>
        /// 是否为窗体
        /// </summary>
        [AdvQueryAttribute(ColName = "IsForm",ColDesc = "是否为窗体")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsForm",IsNullable = true,ColumnDescription = "是否为窗体" )]
        public bool? IsForm 
        { 
            get{return _IsForm;}
            set{SetProperty(ref _IsForm, value);}
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
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }


       
    }
}



