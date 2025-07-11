﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:21
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
    /// 采购入库单
    /// </summary>
    [Serializable()]
    [Description("采购入库单")]
    [SugarTable("tb_PurEntryDetail")]
    public partial class tb_PurEntryDetail: BaseEntity, ICloneable
    {
        public tb_PurEntryDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("采购入库单tb_PurEntryDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PurEntryDetail_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurEntryDetail_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PurEntryDetail_ID
        { 
            get{return _PurEntryDetail_ID;}
            set{
            SetProperty(ref _PurEntryDetail_ID, value);
                base.PrimaryKeyID = _PurEntryDetail_ID;
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
        /// 货品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品详情")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
                        }
        }
        private string _VendorModelCode;
        /// <summary>
        /// 厂商型号
        /// </summary>
        [AdvQueryAttribute(ColName = "VendorModelCode", ColDesc = "厂商型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "VendorModelCode", Length = 50, IsNullable = true, ColumnDescription = "厂商型号")]
        public string VendorModelCode
        {
            get { return _VendorModelCode; }
            set
            {
                SetProperty(ref _VendorModelCode, value);
            }
        }

        private long _PurEntryID;
        /// <summary>
        /// 采购入库单
        /// </summary>
        [AdvQueryAttribute(ColName = "PurEntryID",ColDesc = "采购入库单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurEntryID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "采购入库单" )]
        [FKRelationAttribute("tb_PurEntry","PurEntryID")]
        public long PurEntryID
        { 
            get{return _PurEntryID;}
            set{
            SetProperty(ref _PurEntryID, value);
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
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitPrice" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "单价" )]
        public decimal UnitPrice
        { 
            get{return _UnitPrice;}
            set{
            SetProperty(ref _UnitPrice, value);
                        }
        }

        private decimal _CustomizedCost = ((0));
        /// <summary>
        /// 定制成本
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomizedCost", ColDesc = "定制成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "CustomizedCost", DecimalDigits = 4, IsNullable = false, ColumnDescription = "定制成本")]
        public decimal CustomizedCost
        {
            get { return _CustomizedCost; }
            set
            {
                SetProperty(ref _CustomizedCost, value);
            }
        }
        private decimal _UntaxedCustomizedCost = ((0));
        /// <summary>
        /// 未税定制成本
        /// </summary>
        [AdvQueryAttribute(ColName = "UntaxedCustomizedCost", ColDesc = "未税定制成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "UntaxedCustomizedCost", DecimalDigits = 4, IsNullable = false, ColumnDescription = "未税定制成本")]
        public decimal UntaxedCustomizedCost
        {
            get { return _UntaxedCustomizedCost; }
            set
            {
                SetProperty(ref _UntaxedCustomizedCost, value);
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
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "税额" )]
        public decimal TaxAmount
        { 
            get{return _TaxAmount;}
            set{
            SetProperty(ref _TaxAmount, value);
                        }
        }

        private decimal _UntaxedUnitPrice=0;
        /// <summary>
        /// 未税单价
        /// </summary>
        [AdvQueryAttribute(ColName = "UntaxedUnitPrice", ColDesc = "未税单价")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "UntaxedUnitPrice", DecimalDigits = 4, IsNullable = false, ColumnDescription = "未税单价")]
        public decimal UntaxedUnitPrice
        {
            get { return _UntaxedUnitPrice; }
            set
            {
                SetProperty(ref _UntaxedUnitPrice, value);
            }
        }

        private decimal _SubtotalAmount= ((0));
        /// <summary>
        /// 小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalAmount",ColDesc = "小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "小计" )]
        public decimal SubtotalAmount
        { 
            get{return _SubtotalAmount;}
            set{
            SetProperty(ref _SubtotalAmount, value);
                        }
        }
        private decimal _SubtotalUntaxedAmount;
        /// <summary>
        /// 未税小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalUntaxedAmount", ColDesc = "未税小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "SubtotalUntaxedAmount", DecimalDigits = 4, IsNullable = false, ColumnDescription = "未税小计")]
        public decimal SubtotalUntaxedAmount
        {
            get { return _SubtotalUntaxedAmount; }
            set
            {
                SetProperty(ref _SubtotalUntaxedAmount, value);
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
 

        private int _ReturnedQty= ((0));
        /// <summary>
        /// 退回数
        /// </summary>
        [AdvQueryAttribute(ColName = "ReturnedQty",ColDesc = "退回数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ReturnedQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "退回数" )]
        public int ReturnedQty
        { 
            get{return _ReturnedQty;}
            set{
            SetProperty(ref _ReturnedQty, value);
                        }
        }
        private decimal _AllocatedFreightCost = ((0));
        /// <summary>
        /// 运费成本分摊
        /// </summary>
        [AdvQueryAttribute(ColName = "AllocatedFreightCost", ColDesc = "运费成本分摊")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "AllocatedFreightCost", DecimalDigits = 4, IsNullable = false, ColumnDescription = "运费成本分摊")]
        public decimal AllocatedFreightCost
        {
            get { return _AllocatedFreightCost; }
            set
            {
                SetProperty(ref _AllocatedFreightCost, value);
            }
        }
        private int? _FreightAllocationRules;
        /// <summary>
        /// 分摊规则
        /// </summary>
        [AdvQueryAttribute(ColName = "FreightAllocationRules", ColDesc = "分摊规则")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "FreightAllocationRules", DecimalDigits = 0, IsNullable = true, ColumnDescription = "分摊规则")]
        public int? FreightAllocationRules
        {
            get { return _FreightAllocationRules; }
            set
            {
                SetProperty(ref _FreightAllocationRules, value);
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

        private long? _PurOrder_ChildID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PurOrder_ChildID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurOrder_ChildID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public long? PurOrder_ChildID
        { 
            get{return _PurOrder_ChildID;}
            set{
            SetProperty(ref _PurOrder_ChildID, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(PurEntryID))]
        public virtual tb_PurEntry tb_purentry { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Rack_ID))]
        public virtual tb_StorageRack tb_storagerack { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}



 

        public override object Clone()
        {
            tb_PurEntryDetail loctype = (tb_PurEntryDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

