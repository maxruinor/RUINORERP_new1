
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:26
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
    /// 工资表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Salary")]
    public partial class tb_SalaryQueryDto:BaseEntityDto
    {
        public tb_SalaryQueryDto()
        {

        }

    
     

        private DateTime? _SalaryDate;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "SalaryDate",ColDesc = "")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "SalaryDate",IsNullable = true,ColumnDescription = "" )]
        public DateTime? SalaryDate 
        { 
            get{return _SalaryDate;}
            set{SetProperty(ref _SalaryDate, value);}
        }
     

        private decimal? _BaseSalary;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "BaseSalary",ColDesc = "")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "BaseSalary",IsNullable = true,ColumnDescription = "" )]
        public decimal? BaseSalary 
        { 
            get{return _BaseSalary;}
            set{SetProperty(ref _BaseSalary, value);}
        }
     

        private decimal? _Bonus;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Bonus",ColDesc = "")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Bonus",IsNullable = true,ColumnDescription = "" )]
        public decimal? Bonus 
        { 
            get{return _Bonus;}
            set{SetProperty(ref _Bonus, value);}
        }
     

        private decimal? _Deduction;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Deduction",ColDesc = "")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Deduction",IsNullable = true,ColumnDescription = "" )]
        public decimal? Deduction 
        { 
            get{return _Deduction;}
            set{SetProperty(ref _Deduction, value);}
        }
     

        private decimal? _ActualSalary;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ActualSalary",ColDesc = "")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "ActualSalary",IsNullable = true,ColumnDescription = "" )]
        public decimal? ActualSalary 
        { 
            get{return _ActualSalary;}
            set{SetProperty(ref _ActualSalary, value);}
        }


       
    }
}



