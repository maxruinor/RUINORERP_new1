
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/05/2024 23:44:22
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
    /// 用户角色个性化设置表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_UserPersonalized")]
    public partial class tb_UserPersonalizedQueryDto:BaseEntityDto
    {
        public tb_UserPersonalizedQueryDto()
        {

        }

    
     

        private string _WorkCellSettings;
        /// <summary>
        /// 工作单元设置
        /// </summary>
        [AdvQueryAttribute(ColName = "WorkCellSettings",ColDesc = "工作单元设置")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "WorkCellSettings",Length=2147483647,IsNullable = true,ColumnDescription = "工作单元设置" )]
        public string WorkCellSettings 
        { 
            get{return _WorkCellSettings;}
            set{SetProperty(ref _WorkCellSettings, value);}
        }
     

        private string _WorkCellLayout;
        /// <summary>
        /// 工作台布局
        /// </summary>
        [AdvQueryAttribute(ColName = "WorkCellLayout",ColDesc = "工作台布局")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "WorkCellLayout",Length=2147483647,IsNullable = true,ColumnDescription = "工作台布局" )]
        public string WorkCellLayout 
        { 
            get{return _WorkCellLayout;}
            set{SetProperty(ref _WorkCellLayout, value);}
        }
     

        private long _ID;
        /// <summary>
        /// 用户角色
        /// </summary>
        [AdvQueryAttribute(ColName = "ID",ColDesc = "用户角色")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ID",IsNullable = false,ColumnDescription = "用户角色" )]
        [FKRelationAttribute("tb_User_Role","ID")]
        public long ID 
        { 
            get{return _ID;}
            set{SetProperty(ref _ID, value);}
        }


       
    }
}



