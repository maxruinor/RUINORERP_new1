﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:52
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
    /// 批次表 在采购入库时和出库时保存批次ID
    /// </summary>
    [Serializable()]
    [Description("批次表 在采购入库时和出库时保存批次ID")]
    [SugarTable("tb_BatchNumber")]
    public partial class tb_BatchNumber: BaseEntity, ICloneable
    {
        public tb_BatchNumber()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("批次表 在采购入库时和出库时保存批次IDtb_BatchNumber" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Batch_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Batch_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Batch_ID
        { 
            get{return _Batch_ID;}
            set{
            SetProperty(ref _Batch_ID, value);
                base.PrimaryKeyID = _Batch_ID;
            }
        }

        private string _BatchNO;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "BatchNO",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BatchNO" ,Length=50,IsNullable = true,ColumnDescription = "" )]
        public string BatchNO
        { 
            get{return _BatchNO;}
            set{
            SetProperty(ref _BatchNO, value);
                        }
        }

        private string _采购单号;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "采购单号",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "采购单号" ,Length=20,IsNullable = true,ColumnDescription = "" )]
        public string 采购单号
        { 
            get{return _采购单号;}
            set{
            SetProperty(ref _采购单号, value);
                        }
        }

        private DateTime? _入库日期;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "入库日期",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "入库日期" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? 入库日期
        { 
            get{return _入库日期;}
            set{
            SetProperty(ref _入库日期, value);
                        }
        }

        private int? _供应商;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "供应商",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "供应商" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public int? 供应商
        { 
            get{return _供应商;}
            set{
            SetProperty(ref _供应商, value);
                        }
        }

        private decimal? _采购单价;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "采购单价",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "采购单价" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public decimal? 采购单价
        { 
            get{return _采购单价;}
            set{
            SetProperty(ref _采购单价, value);
                        }
        }

        private DateTime? _expiry_date;
        /// <summary>
        /// 过期日期
        /// </summary>
        [AdvQueryAttribute(ColName = "expiry_date",ColDesc = "过期日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "expiry_date" ,IsNullable = true,ColumnDescription = "过期日期" )]
        public DateTime? expiry_date
        { 
            get{return _expiry_date;}
            set{
            SetProperty(ref _expiry_date, value);
                        }
        }

        private DateTime? _production_date;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "production_date",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "production_date" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? production_date
        { 
            get{return _production_date;}
            set{
            SetProperty(ref _production_date, value);
                        }
        }

        private decimal? _sale_price;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "sale_price",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "sale_price" , DecimalDigits = 2,IsNullable = true,ColumnDescription = "" )]
        public decimal? sale_price
        { 
            get{return _sale_price;}
            set{
            SetProperty(ref _sale_price, value);
                        }
        }

        private int? _quantity;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "quantity",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "quantity" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public int? quantity
        { 
            get{return _quantity;}
            set{
            SetProperty(ref _quantity, value);
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
            tb_BatchNumber loctype = (tb_BatchNumber)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

