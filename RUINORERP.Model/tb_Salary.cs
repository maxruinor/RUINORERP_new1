
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:51
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
    [SugarTable("tb_Salary")]
    public partial class tb_Salary: BaseEntity, ICloneable
    {
        public tb_Salary()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Salary" + "外键ID与对应主主键名称不一致。请修改数据库");
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
            base.PrimaryKeyID = _SalaryID;
            SetProperty(ref _SalaryID, value);
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






        #region 字段描述对应列表
        private ConcurrentDictionary<string, string> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_Salary);
                    
                       foreach (PropertyInfo field in type.GetProperties())
                            {
                                foreach (Attribute attr in field.GetCustomAttributes(true))
                                {
                                    entityAttr = attr as SugarColumn;
                                    if (null != entityAttr)
                                    {
                                        if (entityAttr.ColumnDescription == null)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsIdentity)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsPrimaryKey)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.ColumnDescription.Trim().Length > 0)
                                        {
                                            fieldNameList.TryAdd(field.Name, entityAttr.ColumnDescription);
                                        }
                                    }
                                }
                            }
                }
                
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }
        #endregion
        

        public override object Clone()
        {
            tb_Salary loctype = (tb_Salary)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

