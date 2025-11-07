
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:00
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
    /// 外发加工订单表
    /// </summary>
    [Serializable()]
    [Description("外发加工订单表")]
    [SugarTable("tb_Outsourcing_order")]
    public partial class tb_Outsourcing_order: BaseEntity, ICloneable
    {
        public tb_Outsourcing_order()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("外发加工订单表tb_Outsourcing_order" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
                base.PrimaryKeyID = _ID;
            }
        }

        private int _Quantity= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        public int Quantity
        { 
            get{return _Quantity;}
            set{
            SetProperty(ref _Quantity, value);
                        }
        }

        private decimal _Unit_price= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Unit_price",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Unit_price" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "" )]
        public decimal Unit_price
        { 
            get{return _Unit_price;}
            set{
            SetProperty(ref _Unit_price, value);
                        }
        }

        private decimal _Total_amount= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Total_amount",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Total_amount" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "" )]
        public decimal Total_amount
        { 
            get{return _Total_amount;}
            set{
            SetProperty(ref _Total_amount, value);
                        }
        }

        private int? _Status;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Status",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Status" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public int? Status
        { 
            get{return _Status;}
            set{
            SetProperty(ref _Status, value);
                        }
        }

        private DateTime? _Order_date;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Order_date",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Order_date" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? Order_date
        { 
            get{return _Order_date;}
            set{
            SetProperty(ref _Order_date, value);
                        }
        }

        private DateTime? _Delivery_date;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Delivery_date",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Delivery_date" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? Delivery_date
        { 
            get{return _Delivery_date;}
            set{
            SetProperty(ref _Delivery_date, value);
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
            tb_Outsourcing_order loctype = (tb_Outsourcing_order)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

