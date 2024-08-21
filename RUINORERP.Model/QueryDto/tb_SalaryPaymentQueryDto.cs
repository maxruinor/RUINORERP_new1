
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:52
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
    /// 薪资发放表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_SalaryPayment")]
    public partial class tb_SalaryPaymentQueryDto:BaseEntityDto
    {
        public tb_SalaryPaymentQueryDto()
        {

        }

    
     

        private DateTime? _salary_month;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "salary_month",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "salary_month",IsNullable = true,ColumnDescription = "" )]
        public DateTime? salary_month 
        { 
            get{return _salary_month;}
            set{SetProperty(ref _salary_month, value);}
        }
     

        private decimal? _amount;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "amount",ColDesc = "")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "amount",IsNullable = true,ColumnDescription = "" )]
        public decimal? amount 
        { 
            get{return _amount;}
            set{SetProperty(ref _amount, value);}
        }


       
    }
}



