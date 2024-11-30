
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/30/2024 00:18:30
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

    
     

        private long? _UIPID;
        /// <summary>
        /// 个性化
        /// </summary>
        [AdvQueryAttribute(ColName = "UIPID",ColDesc = "个性化")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "UIPID",IsNullable = true,ColumnDescription = "个性化" )]
        [FKRelationAttribute("tb_UIMenuPersonalization","UIPID")]
        public long? UIPID 
        { 
            get{return _UIPID;}
            set{SetProperty(ref _UIPID, value);}
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


       
    }
}



