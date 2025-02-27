
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/18/2024 22:55:50
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
    /// 销售订单统计分析
    /// </summary>
    [Serializable()]
    [SugarTable("View_SaleOrderItems")]
    public partial class View_SaleOrderItems:BaseViewEntity
    {
        public View_SaleOrderItems()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_SaleOrderItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private long? _SOrder_ID;
        
        
        /// <summary>
        /// SOrder_ID
        /// </summary>

        [AdvQueryAttribute(ColName = "SOrder_ID",ColDesc = "SOrder_ID")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "SOrder_ID" ,IsNullable = true,ColumnDescription = "SOrder_ID" )]
        [Display(Name = "SOrder_ID")]
        public long? SOrder_ID 
        { 
            get{return _SOrder_ID;}
            set{
                SetProperty(ref _SOrder_ID, value);
                }
        }

        private string _SOrderNo;
        
        
        /// <summary>
        /// 订单编号
        /// </summary>

        [AdvQueryAttribute(ColName = "SOrderNo",ColDesc = "订单编号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SOrderNo" ,Length=50,IsNullable = true,ColumnDescription = "订单编号" )]
        [Display(Name = "订单编号")]
        public string SOrderNo 
        { 
            get{return _SOrderNo;}
            set{
                SetProperty(ref _SOrderNo, value);
                }
        }

        private DateTime? _SaleDate;
        
        
        /// <summary>
        /// 订单日期
        /// </summary>

        [AdvQueryAttribute(ColName = "SaleDate",ColDesc = "订单日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "SaleDate" ,IsNullable = true,ColumnDescription = "订单日期" )]
        [Display(Name = "订单日期")]
        public DateTime? SaleDate 
        { 
            get{return _SaleDate;}
            set{
                SetProperty(ref _SaleDate, value);
                }
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 客户
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "客户")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" ,IsNullable = true,ColumnDescription = "客户" )]
        [Display(Name = "客户")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{
                SetProperty(ref _CustomerVendor_ID, value);
                }
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
            get{return _SKU;}
            set{
                SetProperty(ref _SKU, value);
                }
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
            get{return _Specifications;}
            set{
                SetProperty(ref _Specifications, value);
                }
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
            get{return _CNName;}
            set{
                SetProperty(ref _CNName, value);
                }
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
            get{return _Model;}
            set{
                SetProperty(ref _Model, value);
                }
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
            get{return _Type_ID;}
            set{
                SetProperty(ref _Type_ID, value);
                }
        }

        private int? _PayStatus;
        
        
        /// <summary>
        /// 付款状态
        /// </summary>

        [AdvQueryAttribute(ColName = "PayStatus",ColDesc = "付款状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PayStatus" ,IsNullable = true,ColumnDescription = "付款状态" )]
        [Display(Name = "付款状态")]
        public int? PayStatus 
        { 
            get{return _PayStatus;}
            set{
                SetProperty(ref _PayStatus, value);
                }
        }

        private long? _Paytype_ID;
        
        
        /// <summary>
        /// 付款类型
        /// </summary>

        [AdvQueryAttribute(ColName = "Paytype_ID",ColDesc = "付款类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Paytype_ID" ,IsNullable = true,ColumnDescription = "付款类型" )]
        [Display(Name = "付款类型")]
        public long? Paytype_ID 
        { 
            get{return _Paytype_ID;}
            set{
                SetProperty(ref _Paytype_ID, value);
                }
        }
        private int _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus", ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "DataStatus", DecimalDigits = 0, IsNullable = false, ColumnDescription = "数据状态")]
        public int DataStatus
        {
            get { return _DataStatus; }
            set
            {
                SetProperty(ref _DataStatus, value);
            }
        }
        private int? _ApprovalStatus = ((0));
        /// <summary>
        /// 审批状态
        /// </summary>
        [AdvQueryAttribute(ColName = "ApprovalStatus", ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType = "SByte", ColumnName = "ApprovalStatus", DecimalDigits = 0, IsNullable = true, ColumnDescription = "审批状态")]
        public int? ApprovalStatus
        {
            get { return _ApprovalStatus; }
            set
            {
                SetProperty(ref _ApprovalStatus, value);
            }
        }
        private long? _Employee_ID;
        
        
        /// <summary>
        /// 业务员
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "业务员")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" ,IsNullable = true,ColumnDescription = "业务员" )]
        [Display(Name = "业务员")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}
            set{
                SetProperty(ref _Employee_ID, value);
                }
        }

        private long? _ProjectGroup_ID;
        
        
        /// <summary>
        /// 项目组
        /// </summary>

        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" ,IsNullable = true,ColumnDescription = "项目组" )]
        [Display(Name = "项目组")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}
            set{
                SetProperty(ref _ProjectGroup_ID, value);
                }
        }

        private DateTime? _DeliveryDate;
        
        
        /// <summary>
        /// 发货日期
        /// </summary>

        [AdvQueryAttribute(ColName = "DeliveryDate",ColDesc = "发货日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "DeliveryDate" ,IsNullable = true,ColumnDescription = "发货日期" )]
        [Display(Name = "发货日期")]
        public DateTime? DeliveryDate 
        { 
            get{return _DeliveryDate;}
            set{
                SetProperty(ref _DeliveryDate, value);
                }
        }

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 产品
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" ,IsNullable = true,ColumnDescription = "产品" )]
        [Display(Name = "产品")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}
            set{
                SetProperty(ref _ProdDetailID, value);
                }
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
            get{return _UnitPrice;}
            set{
                SetProperty(ref _UnitPrice, value);
                }
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
            get{return _Quantity;}
            set{
                SetProperty(ref _Quantity, value);
                }
        }

        private decimal? _Discount;
        
        
        /// <summary>
        /// 折扣
        /// </summary>

        [AdvQueryAttribute(ColName = "Discount",ColDesc = "折扣")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Discount" , DecimalDigits = 5,Length=5,IsNullable = true,ColumnDescription = "折扣" )]
        [Display(Name = "折扣")]
        public decimal? Discount 
        { 
            get{return _Discount;}
            set{
                SetProperty(ref _Discount, value);
                }
        }

        private decimal? _TransactionPrice;
        
        
        /// <summary>
        /// 成交价
        /// </summary>

        [AdvQueryAttribute(ColName = "TransactionPrice",ColDesc = "成交价")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "TransactionPrice" ,IsNullable = true,ColumnDescription = "成交价" )]
        [Display(Name = "成交价")]
        public decimal? TransactionPrice 
        { 
            get{return _TransactionPrice;}
            set{
                SetProperty(ref _TransactionPrice, value);
                }
        }

        private decimal? _SubtotalTransAmount;
        
        
        /// <summary>
        /// 成交小计
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalTransAmount",ColDesc = "成交小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalTransAmount" ,IsNullable = true,ColumnDescription = "成交小计" )]
        [Display(Name = "成交小计")]
        public decimal? SubtotalTransAmount 
        { 
            get{return _SubtotalTransAmount;}
            set{
                SetProperty(ref _SubtotalTransAmount, value);
                }
        }

        private int? _TotalDeliveredQty;
        
        
        /// <summary>
        /// 已交数量
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalDeliveredQty",ColDesc = "已交数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalDeliveredQty" ,IsNullable = true,ColumnDescription = "已交数量" )]
        [Display(Name = "已交数量")]
        public int? TotalDeliveredQty 
        { 
            get{return _TotalDeliveredQty;}
            set{
                SetProperty(ref _TotalDeliveredQty, value);
                }
        }

        private decimal? _CommissionAmount;
        
        
        /// <summary>
        /// 佣金
        /// </summary>

        [AdvQueryAttribute(ColName = "CommissionAmount",ColDesc = "佣金")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "CommissionAmount" ,IsNullable = true,ColumnDescription = "佣金" )]
        [Display(Name = "佣金")]
        public decimal? CommissionAmount 
        { 
            get{return _CommissionAmount;}
            set{
                SetProperty(ref _CommissionAmount, value);
                }
        }

        private string _Summary;
        
        
        /// <summary>
        /// 摘要
        /// </summary>

        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=1000,IsNullable = true,ColumnDescription = "摘要" )]
        [Display(Name = "摘要")]
        public string Summary 
        { 
            get{return _Summary;}
            set{
                SetProperty(ref _Summary, value);
                }
        }

        private string _CustomerPartNo;
        
        
        /// <summary>
        /// 客户料号
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerPartNo",ColDesc = "客户料号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerPartNo" ,Length=100,IsNullable = true,ColumnDescription = "客户料号" )]
        [Display(Name = "客户料号")]
        public string CustomerPartNo 
        { 
            get{return _CustomerPartNo;}
            set{
                SetProperty(ref _CustomerPartNo, value);
                }
        }

        private bool? _Gift;
        
        
        /// <summary>
        /// 赠品
        /// </summary>

        [AdvQueryAttribute(ColName = "Gift",ColDesc = "赠品")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Gift" ,IsNullable = true,ColumnDescription = "赠品" )]
        [Display(Name = "赠品")]
        public bool? Gift 
        { 
            get{return _Gift;}
            set{
                SetProperty(ref _Gift, value);
                }
        }

        private int? _TotalReturnedQty;
        
        
        /// <summary>
        /// 已退数量
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalReturnedQty",ColDesc = "已退数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "TotalReturnedQty" ,IsNullable = true,ColumnDescription = "已退数量" )]
        [Display(Name = "已退数量")]
        public int? TotalReturnedQty 
        { 
            get{return _TotalReturnedQty;}
            set{
                SetProperty(ref _TotalReturnedQty, value);
                }
        }

        private string _property;
        
        
        /// <summary>
        /// 属性
        /// </summary>

        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=500,IsNullable = true,ColumnDescription = "属性" )]
        [Display(Name = "属性")]
        public string property 
        { 
            get{return _property;}
            set{
                SetProperty(ref _property, value);
                }
        }

        private string _ProductNo;
        
        
        /// <summary>
        /// 品号
        /// </summary>

        [AdvQueryAttribute(ColName = "ProductNo",ColDesc = "品号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ProductNo" ,Length=40,IsNullable = true,ColumnDescription = "品号" )]
        [Display(Name = "品号")]
        public string ProductNo 
        { 
            get{return _ProductNo;}
            set{
                SetProperty(ref _ProductNo, value);
                }
        }

        private long? _Category_ID;
        
        
        /// <summary>
        /// 类别
        /// </summary>

        [AdvQueryAttribute(ColName = "Category_ID",ColDesc = "类别")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Category_ID" ,IsNullable = true,ColumnDescription = "类别" )]
        [Display(Name = "类别")]
        public long? Category_ID 
        { 
            get{return _Category_ID;}
            set{
                SetProperty(ref _Category_ID, value);
                }
        }

        private long? _Unit_ID;
        
        
        /// <summary>
        /// 单位
        /// </summary>

        [AdvQueryAttribute(ColName = "Unit_ID",ColDesc = "单位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Unit_ID" ,IsNullable = true,ColumnDescription = "单位" )]
        [Display(Name = "单位")]
        public long? Unit_ID 
        { 
            get{return _Unit_ID;}
            set{
                SetProperty(ref _Unit_ID, value);
                }
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
                    Type type = typeof(View_SaleOrderItems);
                    
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

