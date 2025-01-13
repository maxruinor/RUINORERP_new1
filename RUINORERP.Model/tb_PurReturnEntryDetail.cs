
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/16/2024 20:05:38
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
    /// 采购退货入库单明细
    /// </summary>
    [Serializable()]
    [Description("tb_PurReturnEntryDetail")]
    [SugarTable("tb_PurReturnEntryDetail")]
    public partial class tb_PurReturnEntryDetail: BaseEntity, ICloneable
    {
        public tb_PurReturnEntryDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_PurReturnEntryDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PurReEntry_CID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurReEntry_CID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PurReEntry_CID
        { 
            get{return _PurReEntry_CID;}
            set{
            base.PrimaryKeyID = _PurReEntry_CID;
            SetProperty(ref _PurReEntry_CID, value);
            }
        }


        private long _PurEntryRe_CID;
        /// <summary>
        /// 采购入库退回单明细ID在这里是以行为标准去对比。
        /// 一个退回单。从采购供应商回来后可以多次退回，按这个累计
        /// </summary>

        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "PurEntryRe_CID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "")]
        public long PurEntryRe_CID
        {
            get { return _PurEntryRe_CID; }
            set
            {
                base.PrimaryKeyID = _PurEntryRe_CID;
                SetProperty(ref _PurEntryRe_CID, value);
            }
        }


        private long _PurReEntry_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PurReEntry_ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurReEntry_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_PurReturnEntry","PurReEntry_ID")]
        public long PurReEntry_ID
        { 
            get{return _PurReEntry_ID;}
            set{
            SetProperty(ref _PurReEntry_ID, value);
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

        private string _CustomertModel;
        /// <summary>
        /// 客户型号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomertModel",ColDesc = "客户型号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomertModel" ,Length=50,IsNullable = true,ColumnDescription = "客户型号" )]
        public string CustomertModel
        { 
            get{return _CustomertModel;}
            set{
            SetProperty(ref _CustomertModel, value);
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
        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "属性" )]
        public string property
        { 
            get{return _property;}
            set{
            SetProperty(ref _property, value);
            }
        }

        private int _Quantity= ((0));
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "数量" )]
        public int Quantity
        { 
            get{return _Quantity;}
            set{
            SetProperty(ref _Quantity, value);
            }
        }

        private decimal _UnitPrice= ((0));
        /// <summary>
        /// 单价
        /// </summary>
        [AdvQueryAttribute(ColName = "UnitPrice",ColDesc = "单价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitPrice" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "单价" )]
        public decimal UnitPrice
        { 
            get{return _UnitPrice;}
            set{
            SetProperty(ref _UnitPrice, value);
            }
        }

        private decimal _Discount= ((1));
        /// <summary>
        /// 折扣
        /// </summary>
        [AdvQueryAttribute(ColName = "Discount",ColDesc = "折扣")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Discount" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "折扣" )]
        public decimal Discount
        { 
            get{return _Discount;}
            set{
            SetProperty(ref _Discount, value);
            }
        }

        private decimal _TransactionPrice= ((0));
        /// <summary>
        /// 成交单价
        /// </summary>
        [AdvQueryAttribute(ColName = "TransactionPrice",ColDesc = "成交单价")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TransactionPrice" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "成交单价" )]
        public decimal TransactionPrice
        { 
            get{return _TransactionPrice;}
            set{
            SetProperty(ref _TransactionPrice, value);
            }
        }

        private bool? _IsGift;
        /// <summary>
        /// 赠品
        /// </summary>
        [AdvQueryAttribute(ColName = "IsGift",ColDesc = "赠品")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsGift" ,IsNullable = true,ColumnDescription = "赠品" )]
        public bool? IsGift
        { 
            get{return _IsGift;}
            set{
            SetProperty(ref _IsGift, value);
            }
        }

        private decimal _TaxRate= ((0));
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TaxRate" , DecimalDigits = 2,IsNullable = false,ColumnDescription = "税率" )]
        public decimal TaxRate
        { 
            get{return _TaxRate;}
            set{
            SetProperty(ref _TaxRate, value);
            }
        }

        private decimal _TaxAmount= ((0));
        /// <summary>
        /// 税额
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxAmount",ColDesc = "税额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxAmount" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "税额" )]
        public decimal TaxAmount
        { 
            get{return _TaxAmount;}
            set{
            SetProperty(ref _TaxAmount, value);
            }
        }

        private decimal _SubtotalTrPriceAmount= ((0));
        /// <summary>
        /// 小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalTrPriceAmount",ColDesc = "小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalTrPriceAmount" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "小计" )]
        public decimal SubtotalTrPriceAmount
        { 
            get{return _SubtotalTrPriceAmount;}
            set{
            SetProperty(ref _SubtotalTrPriceAmount, value);
            }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=1000,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary
        { 
            get{return _Summary;}
            set{
            SetProperty(ref _Summary, value);
            }
        }

        private decimal _DiscountAmount= ((0));
        /// <summary>
        /// 优惠金额
        /// </summary>
        [AdvQueryAttribute(ColName = "DiscountAmount",ColDesc = "优惠金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "DiscountAmount" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "优惠金额" )]
        public decimal DiscountAmount
        { 
            get{return _DiscountAmount;}
            set{
            SetProperty(ref _DiscountAmount, value);
            }
        }

        private bool? _IsIncludeTax= false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IsIncludeTax",ColDesc = "含税")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsIncludeTax" ,IsNullable = true,ColumnDescription = "含税" )]
        public bool? IsIncludeTax
        { 
            get{return _IsIncludeTax;}
            set{
            SetProperty(ref _IsIncludeTax, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Rack_ID))]
        public virtual tb_StorageRack tb_storagerack { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(PurReEntry_ID))]
        public virtual tb_PurReturnEntry tb_purreturnentry { get; set; }



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
                    Type type = typeof(tb_PurReturnEntryDetail);
                    
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
            tb_PurReturnEntryDetail loctype = (tb_PurReturnEntryDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

