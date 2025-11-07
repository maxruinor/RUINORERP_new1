
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:14
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 工资表
    /// </summary>
    [Serializable()]
    [Description("工资表")]
    [SugarTable("tb_Salary")]
    public partial class tb_Salary: BaseEntity, ICloneable
    {
        public tb_Salary()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("工资表tb_Salary" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _SalaryID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SalaryID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long SalaryID
        { 
            get{return _SalaryID;}
            set{
            SetProperty(ref _SalaryID, value);
                base.PrimaryKeyID = _SalaryID;
            }
        }

        private DateTime? _SalaryDate;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "SalaryDate",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "SalaryDate" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? SalaryDate
        { 
            get{return _SalaryDate;}
            set{
            SetProperty(ref _SalaryDate, value);
                        }
        }

        private decimal? _BaseSalary;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "BaseSalary",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "BaseSalary" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "" )]
        public decimal? BaseSalary
        { 
            get{return _BaseSalary;}
            set{
            SetProperty(ref _BaseSalary, value);
                        }
        }

        private decimal? _Bonus;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Bonus",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Bonus" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "" )]
        public decimal? Bonus
        { 
            get{return _Bonus;}
            set{
            SetProperty(ref _Bonus, value);
                        }
        }

        private decimal? _Deduction;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Deduction",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Deduction" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "" )]
        public decimal? Deduction
        { 
            get{return _Deduction;}
            set{
            SetProperty(ref _Deduction, value);
                        }
        }

        private decimal? _ActualSalary;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ActualSalary",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ActualSalary" , DecimalDigits = 4,IsNullable = true,ColumnDescription = "" )]
        public decimal? ActualSalary
        { 
            get{return _ActualSalary;}
            set{
            SetProperty(ref _ActualSalary, value);
                        }
        }

        #endregion

        #region 扩展属性


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_Salary loctype = (tb_Salary)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

