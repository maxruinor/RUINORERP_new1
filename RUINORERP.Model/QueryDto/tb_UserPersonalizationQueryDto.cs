
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:40
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
    /// 用户个性化设置表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_UserPersonalization")]
    public partial class tb_UserPersonalizationQueryDto:BaseEntityDto
    {
        public tb_UserPersonalizationQueryDto()
        {

        }

    
     

        private long? _User_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "User_ID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "User_ID",IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_UserInfo","User_ID")]
        public long? User_ID 
        { 
            get{return _User_ID;}
            set{SetProperty(ref _User_ID, value);}
        }


       
    }
}



