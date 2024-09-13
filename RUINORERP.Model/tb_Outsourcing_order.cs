
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:58
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
    [Description("tb_Outsourcing_order")]
    [SugarTable("tb_Outsourcing_order")]
    public partial class tb_Outsourcing_order: BaseEntity, ICloneable
    {
        public tb_Outsourcing_order()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Outsourcing_order" + "外键ID与对应主主键名称不一致。请修改数据库");
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
            base.PrimaryKeyID = _ID;
            SetProperty(ref _ID, value);
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
                    Type type = typeof(tb_Outsourcing_order);
                    
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
            tb_Outsourcing_order loctype = (tb_Outsourcing_order)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

