
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/26/2024 11:47:02
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model
{
    /// <summary>
    /// 采购订单统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_PurOrderItems")]
    public partial class View_PurOrderItems:BaseViewEntity
    {
        public View_PurOrderItems()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_PurOrderItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private long? _PurOrder_ID;
        
        
        /// <summary>
        /// PurOrder_ID
        /// </summary>

        [AdvQueryAttribute(ColName = "PurOrder_ID",ColDesc = "PurOrder_ID")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PurOrder_ID" ,IsNullable = true,ColumnDescription = "PurOrder_ID" )]
        [Display(Name = "PurOrder_ID")]
        public long? PurOrder_ID 
        { 
            get{return _PurOrder_ID;}            set{                SetProperty(ref _PurOrder_ID, value);                }
        }

        private string _PurOrderNo;
        
        
        /// <summary>
        /// 采购单号
        /// </summary>

        [AdvQueryAttribute(ColName = "PurOrderNo",ColDesc = "采购单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PurOrderNo" ,Length=100,IsNullable = true,ColumnDescription = "采购单号" )]
        [Display(Name = "采购单号")]
        public string PurOrderNo 
        { 
            get{return _PurOrderNo;}            set{                SetProperty(ref _PurOrderNo, value);                }
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 厂商
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "厂商")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" ,IsNullable = true,ColumnDescription = "厂商" )]
        [Display(Name = "厂商")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
        }

        private long? _Employee_ID;
        
        
        /// <summary>
        /// 经办人
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" ,IsNullable = true,ColumnDescription = "经办人" )]
        [Display(Name = "经办人")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 使用部门
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "使用部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" ,IsNullable = true,ColumnDescription = "使用部门" )]
        [Display(Name = "使用部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private long? _Paytype_ID;
        
        
        /// <summary>
        /// 付款方式
        /// </summary>

        [AdvQueryAttribute(ColName = "Paytype_ID",ColDesc = "付款方式")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Paytype_ID" ,IsNullable = true,ColumnDescription = "付款方式" )]
        [Display(Name = "付款方式")]
        public long? Paytype_ID 
        { 
            get{return _Paytype_ID;}            set{                SetProperty(ref _Paytype_ID, value);                }
        }

        private long? _SOrder_ID;
        
        
        /// <summary>
        /// 销售订单
        /// </summary>

        [AdvQueryAttribute(ColName = "SOrder_ID",ColDesc = "销售订单")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SOrder_ID" ,IsNullable = true,ColumnDescription = "销售订单" )]
        [Display(Name = "销售订单")]
        public long? SOrder_ID 
        { 
            get{return _SOrder_ID;}            set{                SetProperty(ref _SOrder_ID, value);                }
        }

        private long? _PDID;
        
        
        /// <summary>
        /// 生产需求
        /// </summary>

        [AdvQueryAttribute(ColName = "PDID",ColDesc = "生产需求")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PDID" ,IsNullable = true,ColumnDescription = "生产需求" )]
        [Display(Name = "生产需求")]
        public long? PDID 
        { 
            get{return _PDID;}            set{                SetProperty(ref _PDID, value);                }
        }

        private DateTime? _PurDate;
        
        
        /// <summary>
        /// 采购日期
        /// </summary>

        [AdvQueryAttribute(ColName = "PurDate",ColDesc = "采购日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "PurDate" ,IsNullable = true,ColumnDescription = "采购日期" )]
        [Display(Name = "采购日期")]
        public DateTime? PurDate 
        { 
            get{return _PurDate;}            set{                SetProperty(ref _PurDate, value);                }
        }

        private DateTime? _OrderPreDeliveryDate;
        
        
        /// <summary>
        /// 预交日期
        /// </summary>

        [AdvQueryAttribute(ColName = "OrderPreDeliveryDate",ColDesc = "预交日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "OrderPreDeliveryDate" ,IsNullable = true,ColumnDescription = "预交日期" )]
        [Display(Name = "预交日期")]
        public DateTime? OrderPreDeliveryDate 
        { 
            get{return _OrderPreDeliveryDate;}            set{                SetProperty(ref _OrderPreDeliveryDate, value);                }
        }

        private decimal? _ShippingCost;
        
        
        /// <summary>
        /// 运费
        /// </summary>

        [AdvQueryAttribute(ColName = "ShippingCost",ColDesc = "运费")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ShippingCost" ,IsNullable = true,ColumnDescription = "运费" )]
        [Display(Name = "运费")]
        public decimal? ShippingCost 
        { 
            get{return _ShippingCost;}            set{                SetProperty(ref _ShippingCost, value);                }
        }

        private string _Notes;
        
        
        /// <summary>
        /// 备注
        /// </summary>

        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
        [Display(Name = "备注")]
        public string Notes 
        { 
            get{return _Notes;}            set{                SetProperty(ref _Notes, value);                }
        }

        private DateTime? _Arrival_date;
        
        
        /// <summary>
        /// 到货日期
        /// </summary>

        [AdvQueryAttribute(ColName = "Arrival_date",ColDesc = "到货日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Arrival_date" ,IsNullable = true,ColumnDescription = "到货日期" )]
        [Display(Name = "到货日期")]
        public DateTime? Arrival_date 
        { 
            get{return _Arrival_date;}            set{                SetProperty(ref _Arrival_date, value);                }
        }
 
        
 

        private decimal? _Deposit;
        
        
        /// <summary>
        /// 订金
        /// </summary>

        [AdvQueryAttribute(ColName = "Deposit",ColDesc = "订金")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Deposit" ,IsNullable = true,ColumnDescription = "订金" )]
        [Display(Name = "订金")]
        public decimal? Deposit 
        { 
            get{return _Deposit;}            set{                SetProperty(ref _Deposit, value);                }
        }

        private int? _TaxDeductionType;
        
        
        /// <summary>
        /// 扣税类型
        /// </summary>

        [AdvQueryAttribute(ColName = "TaxDeductionType",ColDesc = "扣税类型")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TaxDeductionType" ,IsNullable = true,ColumnDescription = "扣税类型" )]
        [Display(Name = "扣税类型")]
        public int? TaxDeductionType 
        { 
            get{return _TaxDeductionType;}            set{                SetProperty(ref _TaxDeductionType, value);                }
        }

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 产品详情
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品详情")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" ,IsNullable = true,ColumnDescription = "产品详情" )]
        [Display(Name = "产品详情")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
        }

        private string _SKU;
        
        
        /// <summary>
        /// SKU码
        /// </summary>

        [AdvQueryAttribute(ColName = "SKU",ColDesc = "SKU码")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SKU" ,Length=80,IsNullable = true,ColumnDescription = "SKU码" )]
        [Display(Name = "SKU码")]
        public string SKU 
        { 
            get{return _SKU;}            set{                SetProperty(ref _SKU, value);                }
        }

        private string _Specifications;
        
        
        /// <summary>
        /// 规格
        /// </summary>

        [AdvQueryAttribute(ColName = "Specifications",ColDesc = "规格")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Specifications" ,Length=1000,IsNullable = true,ColumnDescription = "规格" )]
        [Display(Name = "规格")]
        public string Specifications 
        { 
            get{return _Specifications;}            set{                SetProperty(ref _Specifications, value);                }
        }

        private string _CNName;
        
        
        /// <summary>
        /// 品名
        /// </summary>

        [AdvQueryAttribute(ColName = "CNName",ColDesc = "品名")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CNName" ,Length=255,IsNullable = true,ColumnDescription = "品名" )]
        [Display(Name = "品名")]
        public string CNName 
        { 
            get{return _CNName;}            set{                SetProperty(ref _CNName, value);                }
        }

        private string _Model;
        
        
        /// <summary>
        /// 型号
        /// </summary>

        [AdvQueryAttribute(ColName = "Model",ColDesc = "型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Model" ,Length=50,IsNullable = true,ColumnDescription = "型号" )]
        [Display(Name = "型号")]
        public string Model 
        { 
            get{return _Model;}            set{                SetProperty(ref _Model, value);                }
        }

        private long? _Type_ID;
        
        
        /// <summary>
        /// 产品类型
        /// </summary>

        [AdvQueryAttribute(ColName = "Type_ID",ColDesc = "产品类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID" ,IsNullable = true,ColumnDescription = "产品类型" )]
        [Display(Name = "产品类型")]
        public long? Type_ID 
        { 
            get{return _Type_ID;}            set{                SetProperty(ref _Type_ID, value);                }
        }

        private string _property;
        
        
        /// <summary>
        /// 属性
        /// </summary>

        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "属性" )]
        [Display(Name = "属性")]
        public string property 
        { 
            get{return _property;}            set{                SetProperty(ref _property, value);                }
        }

        private long? _Location_ID;
        
        
        /// <summary>
        /// 库位
        /// </summary>

        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" ,IsNullable = true,ColumnDescription = "库位" )]
        [Display(Name = "库位")]
        public long? Location_ID 
        { 
            get{return _Location_ID;}            set{                SetProperty(ref _Location_ID, value);                }
        }

        private int? _Quantity;
        
        
        /// <summary>
        /// 数量
        /// </summary>

        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" ,IsNullable = true,ColumnDescription = "数量" )]
        [Display(Name = "数量")]
        public int? Quantity 
        { 
            get{return _Quantity;}            set{                SetProperty(ref _Quantity, value);                }
        }

        private decimal? _UnitPrice;
        
        
        /// <summary>
        /// 单价
        /// </summary>

        [AdvQueryAttribute(ColName = "UnitPrice",ColDesc = "单价")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitPrice" ,IsNullable = true,ColumnDescription = "单价" )]
        [Display(Name = "单价")]
        public decimal? UnitPrice 
        { 
            get{return _UnitPrice;}            set{                SetProperty(ref _UnitPrice, value);                }
        }



         

        private decimal? _TaxRate;
        
        
        /// <summary>
        /// 税率
        /// </summary>

        [AdvQueryAttribute(ColName = "TaxRate",ColDesc = "税率")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "TaxRate" , DecimalDigits = 5,Length=5,IsNullable = true,ColumnDescription = "税率" )]
        [Display(Name = "税率")]
        public decimal? TaxRate 
        { 
            get{return _TaxRate;}            set{                SetProperty(ref _TaxRate, value);                }
        }

        private decimal? _TaxAmount;
        
        
        /// <summary>
        /// 税额
        /// </summary>

        [AdvQueryAttribute(ColName = "TaxAmount",ColDesc = "税额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TaxAmount" ,IsNullable = true,ColumnDescription = "税额" )]
        [Display(Name = "税额")]
        public decimal? TaxAmount 
        { 
            get{return _TaxAmount;}            set{                SetProperty(ref _TaxAmount, value);                }
        }

        private decimal? _SubtotalAmount;
        
        
        /// <summary>
        /// 成交金额
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalAmount",ColDesc = "成交金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalAmount" ,IsNullable = true,ColumnDescription = "成交金额" )]
        [Display(Name = "成交金额")]
        public decimal? SubtotalAmount 
        { 
            get{return _SubtotalAmount;}            set{                SetProperty(ref _SubtotalAmount, value);                }
        }

        private bool? _IsGift;
        
        
        /// <summary>
        /// 赠品
        /// </summary>

        [AdvQueryAttribute(ColName = "IsGift",ColDesc = "赠品")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsGift" ,IsNullable = true,ColumnDescription = "赠品" )]
        [Display(Name = "赠品")]
        public bool? IsGift 
        { 
            get{return _IsGift;}            set{                SetProperty(ref _IsGift, value);                }
        }

        private DateTime? _ItemPreDeliveryDate;
        
        
        /// <summary>
        /// 预交日期
        /// </summary>

        [AdvQueryAttribute(ColName = "ItemPreDeliveryDate",ColDesc = "预交日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ItemPreDeliveryDate" ,IsNullable = true,ColumnDescription = "预交日期" )]
        [Display(Name = "预交日期")]
        public DateTime? ItemPreDeliveryDate 
        { 
            get{return _ItemPreDeliveryDate;}            set{                SetProperty(ref _ItemPreDeliveryDate, value);                }
        }

        private string _CustomertModel;
        
        
        /// <summary>
        /// 客户型号
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomertModel",ColDesc = "客户型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomertModel" ,Length=50,IsNullable = true,ColumnDescription = "客户型号" )]
        [Display(Name = "客户型号")]
        public string CustomertModel 
        { 
            get{return _CustomertModel;}            set{                SetProperty(ref _CustomertModel, value);                }
        }

        private int? _DeliveredQuantity;
        
        
        /// <summary>
        /// 已交数量
        /// </summary>

        [AdvQueryAttribute(ColName = "DeliveredQuantity",ColDesc = "已交数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DeliveredQuantity" ,IsNullable = true,ColumnDescription = "已交数量" )]
        [Display(Name = "已交数量")]
        public int? DeliveredQuantity 
        { 
            get{return _DeliveredQuantity;}            set{                SetProperty(ref _DeliveredQuantity, value);                }
        }

        private bool? _IncludingTax;
        
        
        /// <summary>
        /// 含税
        /// </summary>

        [AdvQueryAttribute(ColName = "IncludingTax",ColDesc = "含税")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IncludingTax" ,IsNullable = true,ColumnDescription = "含税" )]
        [Display(Name = "含税")]
        public bool? IncludingTax 
        { 
            get{return _IncludingTax;}            set{                SetProperty(ref _IncludingTax, value);                }
        }

        private string _Summary;
        
        
        /// <summary>
        /// 摘要
        /// </summary>

        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=255,IsNullable = true,ColumnDescription = "摘要" )]
        [Display(Name = "摘要")]
        public string Summary 
        { 
            get{return _Summary;}            set{                SetProperty(ref _Summary, value);                }
        }







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
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(View_PurOrderItems);
                    
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
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

