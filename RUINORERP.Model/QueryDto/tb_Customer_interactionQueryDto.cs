
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:56
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
    /// 客户交互表，CRM系统中使用      
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Customer_interaction")]
    public partial class tb_Customer_interactionQueryDto:BaseEntityDto
    {
        public tb_Customer_interactionQueryDto()
        {

        }

    
     

        private long? _Customer_id;
        /// <summary>
        /// 意向客户
        /// </summary>
        [AdvQueryAttribute(ColName = "Customer_id",ColDesc = "意向客户")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Customer_id",IsNullable = true,ColumnDescription = "意向客户" )]
        [FKRelationAttribute("tb_Customer","Customer_id")]
        public long? Customer_id 
        { 
            get{return _Customer_id;}
            set{SetProperty(ref _Customer_id, value);}
        }
     

        private long? _Employee_ID;
        /// <summary>
        /// 对接人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "对接人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "对接人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
            set{SetProperty(ref _Employee_ID, value);}
        }
     

        private string _interaction_date;
        /// <summary>
        /// 交互日期
        /// </summary>
        [AdvQueryAttribute(ColName = "interaction_date",ColDesc = "交互日期")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "interaction_date",Length=50,IsNullable = true,ColumnDescription = "交互日期" )]
        public string interaction_date 
        { 
            get{return _interaction_date;}
            set{SetProperty(ref _interaction_date, value);}
        }
     

        private string _interaction_type;
        /// <summary>
        /// 交互类型
        /// </summary>
        [AdvQueryAttribute(ColName = "interaction_type",ColDesc = "交互类型")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "interaction_type",Length=100,IsNullable = true,ColumnDescription = "交互类型" )]
        public string interaction_type 
        { 
            get{return _interaction_type;}
            set{SetProperty(ref _interaction_type, value);}
        }
     

        private string _interaction_detail;
        /// <summary>
        /// 交互详情
        /// </summary>
        [AdvQueryAttribute(ColName = "interaction_detail",ColDesc = "交互详情")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "interaction_detail",Length=2147483647,IsNullable = true,ColumnDescription = "交互详情" )]
        public string interaction_detail 
        { 
            get{return _interaction_detail;}
            set{SetProperty(ref _interaction_detail, value);}
        }


       
    }
}



