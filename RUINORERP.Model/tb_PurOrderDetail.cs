
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:23
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
    /// 采购订单明细表
    /// </summary>
    [Serializable()]
    [Description("采购订单明细表")]
    [SugarTable("tb_PurOrderDetail")]
    public partial class tb_PurOrderDetail: BaseEntity, ICloneable
    {
        public tb_PurOrderDetail()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("采购订单明细表tb_PurOrderDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PurOrder_ChildID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurOrder_ChildID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PurOrder_ChildID
        { 
            get{return _PurOrder_ChildID;}
            set{
            SetProperty(ref _PurOrder_ChildID, value);
                base.PrimaryKeyID = _PurOrder_ChildID;
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

        private long _PurOrder_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PurOrder_ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurOrder_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_PurOrder","PurOrder_ID")]
        public long PurOrder_ID
        { 
            get{return _PurOrder_ID;}
            set{
            SetProperty(ref _PurOrder_ID, value);
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

        private int _Quantity;
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

        private decimal _UnitPrice;
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
        private decimal _TaxRate= ((0));
        /// <summary>
        /// 税率
        /// </summary>
        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TaxRate" , DecimalDigits = 3,IsNullable = false,ColumnDescription = "税率" )]
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


        private decimal _SubtotalAmount;
        /// <summary>
        /// 成交金额
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalAmount",ColDesc = "成交金额")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalAmount" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成交金额" )]
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


        private bool? _IsGift= false;
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

        private DateTime _PreDeliveryDate;
        /// <summary>
        /// 预交日期
        /// </summary>
        [AdvQueryAttribute(ColName = "PreDeliveryDate",ColDesc = "预交日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PreDeliveryDate" ,IsNullable = false,ColumnDescription = "预交日期" )]
        public DateTime PreDeliveryDate
        { 
            get{return _PreDeliveryDate;}
            set{
            SetProperty(ref _PreDeliveryDate, value);
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

        private int _DeliveredQuantity= ((0));
        /// <summary>
        /// 已交数
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveredQuantity",ColDesc = "已交数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DeliveredQuantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "已交数" )]
        public int DeliveredQuantity
        { 
            get{return _DeliveredQuantity;}
            set{
            SetProperty(ref _DeliveredQuantity, value);
                        }
        }

        private bool _IncludingTax= false;
        /// <summary>
        /// 含税
        /// </summary>
        [AdvQueryAttribute(ColName = "IncludingTax",ColDesc = "含税")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IncludingTax" ,IsNullable = false,ColumnDescription = "含税" )]
        public bool IncludingTax
        { 
            get{return _IncludingTax;}
            set{
            SetProperty(ref _IncludingTax, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=1000,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        private int _TotalReturnedQty= ((0));
        /// <summary>
        /// 退回数
        /// </summary>
        [AdvQueryAttribute(ColName = "TotalReturnedQty",ColDesc = "退回数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalReturnedQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "退回数" )]
        public int TotalReturnedQty
        { 
            get{return _TotalReturnedQty;}
            set{
            SetProperty(ref _TotalReturnedQty, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(PurOrder_ID))]
        public virtual tb_PurOrder tb_purorder { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}



 
        public override object Clone()
        {
            tb_PurOrderDetail loctype = (tb_PurOrderDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

