
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:33
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
    /// 销售机会
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Sales_Chance")]
    public partial class tb_Sales_ChanceQueryDto:BaseEntityDto
    {
        public tb_Sales_ChanceQueryDto()
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
     

        private string _opportunity_name;
        /// <summary>
        /// 机会名称
        /// </summary>
        [AdvQueryAttribute(ColName = "opportunity_name",ColDesc = "机会名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "opportunity_name",Length=50,IsNullable = true,ColumnDescription = "机会名称" )]
        public string opportunity_name 
        { 
            get{return _opportunity_name;}
            set{SetProperty(ref _opportunity_name, value);}
        }
     

        private string _opportunity_amount;
        /// <summary>
        /// 机会金额
        /// </summary>
        [AdvQueryAttribute(ColName = "opportunity_amount",ColDesc = "机会金额")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "opportunity_amount",Length=100,IsNullable = true,ColumnDescription = "机会金额" )]
        public string opportunity_amount 
        { 
            get{return _opportunity_amount;}
            set{SetProperty(ref _opportunity_amount, value);}
        }
     

        private string _opportunity_stage;
        /// <summary>
        /// 机会阶段
        /// </summary>
        [AdvQueryAttribute(ColName = "opportunity_stage",ColDesc = "机会阶段")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "opportunity_stage",Length=200,IsNullable = true,ColumnDescription = "机会阶段" )]
        public string opportunity_stage 
        { 
            get{return _opportunity_stage;}
            set{SetProperty(ref _opportunity_stage, value);}
        }


       
    }
}



