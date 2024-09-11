
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2024 13:38:37
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
    /// 借出单明细
    /// </summary>
    [Serializable()]
    [Description("tb_ProdBorrowingDetail")]
    [SugarTable("tb_ProdBorrowingDetail")]
    public partial class tb_ProdBorrowingDetail: BaseEntity, ICloneable
    {
        public tb_ProdBorrowingDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_ProdBorrowingDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _BorrowDetaill_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BorrowDetaill_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long BorrowDetaill_ID
        { 
            get{return _BorrowDetaill_ID;}
            set{
            base.PrimaryKeyID = _BorrowDetaill_ID;
            SetProperty(ref _BorrowDetaill_ID, value);
            }
        }

        private long _BorrowID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "BorrowID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BorrowID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ProdBorrowing","BorrowID")]
        public long BorrowID
        { 
            get{return _BorrowID;}
            set{
            SetProperty(ref _BorrowID, value);
            }
        }

        private long _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "库位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID
        { 
            get{return _Location_ID;}
            set{
            SetProperty(ref _Location_ID, value);
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 产品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "产品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
            }
        }

        private string _property;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "属性" )]
        public string property
        { 
            get{return _property;}
            set{
            SetProperty(ref _property, value);
            }
        }

        private int _Qty= ((0));
        /// <summary>
        /// 借出数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Qty",ColDesc = "借出数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Qty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "借出数量")]
        public int Qty
        { 
            get{return _Qty;}
            set{
            SetProperty(ref _Qty, value);
            }
        }

        private int _ReQty = ((0));
        /// <summary>
        /// 归还数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ReQty", ColDesc = "归还数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "ReQty", DecimalDigits = 0, IsNullable = false, ColumnDescription = "归还数量")]
        public int ReQty
        {
            get { return _ReQty; }
            set
            {
                SetProperty(ref _ReQty, value);
            }
        }

        private decimal _Price= ((0));
        /// <summary>
        /// 售价
        /// </summary>
        [AdvQueryAttribute(ColName = "Price",ColDesc = "售价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Price" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "售价" )]
        public decimal Price
        { 
            get{return _Price;}
            set{
            SetProperty(ref _Price, value);
            }
        }

        private decimal _Cost= ((0));
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Cost" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "成本" )]
        public decimal Cost
        { 
            get{return _Cost;}
            set{
            SetProperty(ref _Cost, value);
            }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=500,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary
        { 
            get{return _Summary;}
            set{
            SetProperty(ref _Summary, value);
            }
        }

        private decimal _SubtotalCostAmount= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalCostAmount",ColDesc = "成本小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalCostAmount" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal SubtotalCostAmount
        { 
            get{return _SubtotalCostAmount;}
            set{
            SetProperty(ref _SubtotalCostAmount, value);
            }
        }

        private decimal _SubtotalPirceAmount= ((0));
        /// <summary>
        /// 金额小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalPirceAmount",ColDesc = "金额小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalPirceAmount" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "金额小计" )]
        public decimal SubtotalPirceAmount
        { 
            get{return _SubtotalPirceAmount;}
            set{
            SetProperty(ref _SubtotalPirceAmount, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(BorrowID))]
        public virtual tb_ProdBorrowing tb_prodborrowing { get; set; }



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
                    Type type = typeof(tb_ProdBorrowingDetail);
                    
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
            tb_ProdBorrowingDetail loctype = (tb_ProdBorrowingDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

