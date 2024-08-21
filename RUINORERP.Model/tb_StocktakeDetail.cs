
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:49:14
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
    /// 盘点明细表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_StocktakeDetail")]
    public partial class tb_StocktakeDetail: BaseEntity, ICloneable
    {
        public tb_StocktakeDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_StocktakeDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _SubID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SubID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long SubID
        { 
            get{return _SubID;}
            set{
            base.PrimaryKeyID = _SubID;
            SetProperty(ref _SubID, value);
            }
        }

        private long _MainID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "MainID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MainID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_Stocktake","MainID")]
        public long MainID
        { 
            get{return _MainID;}
            set{
            SetProperty(ref _MainID, value);
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "产品")]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
            }
        }

        private long? _Rack_ID;
        /// <summary>
        /// 货架
        /// </summary>
        [AdvQueryAttribute(ColName = "Rack_ID",ColDesc = "货架")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Rack_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "货架" )]
        [FKRelationAttribute("tb_StorageRack","Rack_ID")]
        public long? Rack_ID
        { 
            get{return _Rack_ID;}
            set{
            SetProperty(ref _Rack_ID, value);
            }
        }
        private string _property;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property", ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "property", Length = 255, IsNullable = true, ColumnDescription = "属性")]
        public string property
        {
            get { return _property; }
            set
            {
                SetProperty(ref _property, value);
            }
        }
        private decimal _Cost= ((0));
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Cost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本" )]
        public decimal Cost
        { 
            get{return _Cost;}
            set{
            SetProperty(ref _Cost, value);
            }
        }

        private int _CarryinglQty= ((0));
        /// <summary>
        /// 载账数量
        /// </summary>
        [AdvQueryAttribute(ColName = "CarryinglQty",ColDesc = "载账数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CarryinglQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "载账数量" )]
        public int CarryinglQty
        { 
            get{return _CarryinglQty;}
            set{
            SetProperty(ref _CarryinglQty, value);
            }
        }

        private decimal _CarryingSubtotalAmount= ((0));
        /// <summary>
        /// 载账小计
        /// </summary>
        [AdvQueryAttribute(ColName = "CarryingSubtotalAmount",ColDesc = "载账小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CarryingSubtotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "载账小计" )]
        public decimal CarryingSubtotalAmount
        { 
            get{return _CarryingSubtotalAmount;}
            set{
            SetProperty(ref _CarryingSubtotalAmount, value);
            }
        }

        private int _DiffQty= ((0));
        /// <summary>
        /// 差异数量
        /// </summary>
        [AdvQueryAttribute(ColName = "DiffQty",ColDesc = "差异数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DiffQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "差异数量" )]
        public int DiffQty
        { 
            get{return _DiffQty;}
            set{
            SetProperty(ref _DiffQty, value);
            }
        }

        private decimal _DiffSubtotalAmount= ((0));
        /// <summary>
        /// 差异小计
        /// </summary>
        [AdvQueryAttribute(ColName = "DiffSubtotalAmount",ColDesc = "差异小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "DiffSubtotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "差异小计" )]
        public decimal DiffSubtotalAmount
        { 
            get{return _DiffSubtotalAmount;}
            set{
            SetProperty(ref _DiffSubtotalAmount, value);
            }
        }

        private int _CheckQty= ((0));
        /// <summary>
        /// 盘点数量
        /// </summary>
        [AdvQueryAttribute(ColName = "CheckQty",ColDesc = "盘点数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CheckQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "盘点数量" )]
        public int CheckQty
        { 
            get{return _CheckQty;}
            set{
            SetProperty(ref _CheckQty, value);
            }
        }

        private decimal _CheckSubtotalAmount= ((0));
        /// <summary>
        /// 盘点小计
        /// </summary>
        [AdvQueryAttribute(ColName = "CheckSubtotalAmount",ColDesc = "盘点小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CheckSubtotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "盘点小计" )]
        public decimal CheckSubtotalAmount
        { 
            get{return _CheckSubtotalAmount;}
            set{
            SetProperty(ref _CheckSubtotalAmount, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
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
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }
        //public virtual tb_ProdDetail tb_ProdDetailID { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(MainID))]
        public virtual tb_Stocktake tb_stocktake { get; set; }
        //public virtual tb_Stocktake tb_MainID { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Rack_ID))]
        public virtual tb_StorageRack tb_storagerack { get; set; }
        //public virtual tb_StorageRack tb_Rack_ID { get; set; }



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
                    Type type = typeof(tb_StocktakeDetail);
                    
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
            tb_StocktakeDetail loctype = (tb_StocktakeDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

