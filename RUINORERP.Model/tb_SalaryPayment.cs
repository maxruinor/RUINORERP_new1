
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:26
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
    /// 薪资发放表
    /// </summary>
    [Serializable()]
    [Description("薪资发放表")]
    [SugarTable("tb_SalaryPayment")]
    public partial class tb_SalaryPayment: BaseEntity, ICloneable
    {
        public tb_SalaryPayment()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("薪资发放表tb_SalaryPayment" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _id;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long id
        { 
            get{return _id;}
            set{
            SetProperty(ref _id, value);
                base.PrimaryKeyID = _id;
            }
        }

        private DateTime? _salary_month;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "salary_month",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "salary_month" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? salary_month
        { 
            get{return _salary_month;}
            set{
            SetProperty(ref _salary_month, value);
                        }
        }

        private decimal? _amount;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "amount",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "amount" , DecimalDigits = 2,IsNullable = true,ColumnDescription = "" )]
        public decimal? amount
        { 
            get{return _amount;}
            set{
            SetProperty(ref _amount, value);
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
            tb_SalaryPayment loctype = (tb_SalaryPayment)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

