
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:57
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
    /// 期初存货来自期初盘点或业务上首次库存入库
    /// </summary>
    [Serializable()]
    [Description("tb_OpeningInventory")]
    [SugarTable("tb_OpeningInventory")]
    public partial class tb_OpeningInventory: BaseEntity, ICloneable
    {
        public tb_OpeningInventory()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_OpeningInventory" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _OI_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "OI_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long OI_ID
        { 
            get{return _OI_ID;}
            set{
            base.PrimaryKeyID = _OI_ID;
            SetProperty(ref _OI_ID, value);
            }
        }

        private long? _Inventory_ID;
        /// <summary>
        /// 库存
        /// </summary>
        [AdvQueryAttribute(ColName = "Inventory_ID",ColDesc = "库存")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Inventory_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "库存" )]
        [FKRelationAttribute("tb_Inventory","Inventory_ID")]
        public long? Inventory_ID
        { 
            get{return _Inventory_ID;}
            set{
            SetProperty(ref _Inventory_ID, value);
            }
        }

        private int _InitQty= ((0));
        /// <summary>
        /// 期初库存
        /// </summary>
        [AdvQueryAttribute(ColName = "InitQty",ColDesc = "期初库存")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "InitQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "期初库存" )]
        public int InitQty
        { 
            get{return _InitQty;}
            set{
            SetProperty(ref _InitQty, value);
            }
        }

        private decimal _Cost_price= ((0));
        /// <summary>
        /// 成本价格
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost_price",ColDesc = "成本价格")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Cost_price" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "成本价格" )]
        public decimal Cost_price
        { 
            get{return _Cost_price;}
            set{
            SetProperty(ref _Cost_price, value);
            }
        }

        private decimal _Subtotal_Cost_Price= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "Subtotal_Cost_Price",ColDesc = "成本小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Subtotal_Cost_Price" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal Subtotal_Cost_Price
        { 
            get{return _Subtotal_Cost_Price;}
            set{
            SetProperty(ref _Subtotal_Cost_Price, value);
            }
        }

        private DateTime? _InitInvDate;
        /// <summary>
        /// 期初日期
        /// </summary>
        [AdvQueryAttribute(ColName = "InitInvDate",ColDesc = "期初日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "InitInvDate" ,IsNullable = true,ColumnDescription = "期初日期" )]
        public DateTime? InitInvDate
        { 
            get{return _InitInvDate;}
            set{
            SetProperty(ref _InitInvDate, value);
            }
        }

        private long? _RefBillID;
        /// <summary>
        /// 转入单
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBillID",ColDesc = "转入单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RefBillID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "转入单" )]
        public long? RefBillID
        { 
            get{return _RefBillID;}
            set{
            SetProperty(ref _RefBillID, value);
            }
        }

        private string _RefNO;
        /// <summary>
        /// 引用单据
        /// </summary>
        [AdvQueryAttribute(ColName = "RefNO",ColDesc = "引用单据")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RefNO" ,Length=50,IsNullable = true,ColumnDescription = "引用单据" )]
        public string RefNO
        { 
            get{return _RefNO;}
            set{
            SetProperty(ref _RefNO, value);
            }
        }

        private string _RefBizType;
        /// <summary>
        /// 单据类型
        /// </summary>
        [AdvQueryAttribute(ColName = "RefBizType",ColDesc = "单据类型")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "RefBizType" ,Length=50,IsNullable = true,ColumnDescription = "单据类型" )]
        public string RefBizType
        { 
            get{return _RefBizType;}
            set{
            SetProperty(ref _RefBizType, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=100,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Inventory_ID))]
        public virtual tb_Inventory tb_inventory { get; set; }



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
                    Type type = typeof(tb_OpeningInventory);
                    
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
            tb_OpeningInventory loctype = (tb_OpeningInventory)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

