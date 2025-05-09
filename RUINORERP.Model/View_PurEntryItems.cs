
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/07/2024 20:12:58
// **************************************
using System;
using SqlSugar;
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
    /// 采购入库统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_PurEntryItems")]
    public partial class View_PurEntryItems : BaseViewEntity
    {
        public View_PurEntryItems()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_PurEntryItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

        private string _PurOrder_NO;


        /// <summary>
        /// 采购订单单号
        /// </summary>

        [AdvQueryAttribute(ColName = "PurOrder_NO", ColDesc = "采购订单单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "PurOrder_NO", Length = 50, IsNullable = true, ColumnDescription = "采购订单单号")]
        [Display(Name = "采购订单单号")]
        public string PurOrder_NO
        {
            get { return _PurOrder_NO; }
            set
            {
                SetProperty(ref _PurOrder_NO, value);
            }
        }

        private string _PurEntryNo;


        /// <summary>
        /// 入库单号
        /// </summary>

        [AdvQueryAttribute(ColName = "PurEntryNo", ColDesc = "入库单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "PurEntryNo", Length = 50, IsNullable = true, ColumnDescription = "入库单号")]
        [Display(Name = "入库单号")]
        public string PurEntryNo
        {
            get { return _PurEntryNo; }
            set
            {
                SetProperty(ref _PurEntryNo, value);
            }
        }

        private long? _CustomerVendor_ID;


        /// <summary>
        /// 厂商
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID", ColDesc = "厂商")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "CustomerVendor_ID", IsNullable = true, ColumnDescription = "厂商")]
        [Display(Name = "厂商")]
        public long? CustomerVendor_ID
        {
            get { return _CustomerVendor_ID; }
            set
            {
                SetProperty(ref _CustomerVendor_ID, value);
            }
        }

        private long? _Employee_ID;


        /// <summary>
        /// 经办人
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID", ColDesc = "经办人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Employee_ID", IsNullable = true, ColumnDescription = "经办人")]
        [Display(Name = "经办人")]
        public long? Employee_ID
        {
            get { return _Employee_ID; }
            set
            {
                SetProperty(ref _Employee_ID, value);
            }
        }

        private long? _DepartmentID;


        /// <summary>
        /// 使用部门
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID", ColDesc = "使用部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "DepartmentID", IsNullable = true, ColumnDescription = "使用部门")]
        [Display(Name = "使用部门")]
        public long? DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
                SetProperty(ref _DepartmentID, value);
            }
        }

        private long? _Paytype_ID;


        /// <summary>
        /// 交易方式
        /// </summary>

        [AdvQueryAttribute(ColName = "Paytype_ID", ColDesc = "交易方式")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Paytype_ID", IsNullable = true, ColumnDescription = "交易方式")]
        [Display(Name = "交易方式")]
        public long? Paytype_ID
        {
            get { return _Paytype_ID; }
            set
            {
                SetProperty(ref _Paytype_ID, value);
            }
        }

        private DateTime? _EntryDate;


        /// <summary>
        /// 入库日期
        /// </summary>

        [AdvQueryAttribute(ColName = "EntryDate", ColDesc = "入库日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "EntryDate", IsNullable = true, ColumnDescription = "入库日期")]
        [Display(Name = "入库日期")]
        public DateTime? EntryDate
        {
            get { return _EntryDate; }
            set
            {
                SetProperty(ref _EntryDate, value);
            }
        }

        private decimal? _ShippingCost;


        /// <summary>
        /// 运费
        /// </summary>

        [AdvQueryAttribute(ColName = "ShippingCost", ColDesc = "运费")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "ShippingCost", IsNullable = true, ColumnDescription = "运费")]
        [Display(Name = "运费")]
        public decimal? ShippingCost
        {
            get { return _ShippingCost; }
            set
            {
                SetProperty(ref _ShippingCost, value);
            }
        }

        private string _Notes;


        /// <summary>
        /// 备注
        /// </summary>

        [AdvQueryAttribute(ColName = "Notes", ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Notes", Length = 255, IsNullable = true, ColumnDescription = "备注")]
        [Display(Name = "备注")]
        public string Notes
        {
            get { return _Notes; }
            set
            {
                SetProperty(ref _Notes, value);
            }
        }

 

        private int? _TaxDeductionType;


        /// <summary>
        /// 扣税类型
        /// </summary>

        [AdvQueryAttribute(ColName = "TaxDeductionType", ColDesc = "扣税类型")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "TaxDeductionType", IsNullable = true, ColumnDescription = "扣税类型")]
        [Display(Name = "扣税类型")]
        public int? TaxDeductionType
        {
            get { return _TaxDeductionType; }
            set
            {
                SetProperty(ref _TaxDeductionType, value);
            }
        }

        private long? _ProdDetailID;


        /// <summary>
        /// 产品详情
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID", ColDesc = "产品详情")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "ProdDetailID", IsNullable = true, ColumnDescription = "产品详情")]
        [Display(Name = "产品详情")]
        public long? ProdDetailID
        {
            get { return _ProdDetailID; }
            set
            {
                SetProperty(ref _ProdDetailID, value);
            }
        }

        private string _SKU;


        /// <summary>
        /// SKU码
        /// </summary>

        [AdvQueryAttribute(ColName = "SKU", ColDesc = "SKU码")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "SKU", Length = 80, IsNullable = true, ColumnDescription = "SKU码")]
        [Display(Name = "SKU码")]
        public string SKU
        {
            get { return _SKU; }
            set
            {
                SetProperty(ref _SKU, value);
            }
        }

        private string _Specifications;


        /// <summary>
        /// 规格
        /// </summary>

        [AdvQueryAttribute(ColName = "Specifications", ColDesc = "规格")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Specifications", Length = 1000, IsNullable = true, ColumnDescription = "规格")]
        [Display(Name = "规格")]
        public string Specifications
        {
            get { return _Specifications; }
            set
            {
                SetProperty(ref _Specifications, value);
            }
        }

        private string _CNName;


        /// <summary>
        /// 品名
        /// </summary>

        [AdvQueryAttribute(ColName = "CNName", ColDesc = "品名")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "CNName", Length = 255, IsNullable = true, ColumnDescription = "品名")]
        [Display(Name = "品名")]
        public string CNName
        {
            get { return _CNName; }
            set
            {
                SetProperty(ref _CNName, value);
            }
        }

        private string _Model;


        /// <summary>
        /// 型号
        /// </summary>

        [AdvQueryAttribute(ColName = "Model", ColDesc = "型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Model", Length = 50, IsNullable = true, ColumnDescription = "型号")]
        [Display(Name = "型号")]
        public string Model
        {
            get { return _Model; }
            set
            {
                SetProperty(ref _Model, value);
            }
        }

        private long? _Type_ID;


        /// <summary>
        /// 产品类型
        /// </summary>

        [AdvQueryAttribute(ColName = "Type_ID", ColDesc = "产品类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Type_ID", IsNullable = true, ColumnDescription = "产品类型")]
        [Display(Name = "产品类型")]
        public long? Type_ID
        {
            get { return _Type_ID; }
            set
            {
                SetProperty(ref _Type_ID, value);
            }
        }

        private string _property;


        /// <summary>
        /// 属性
        /// </summary>

        [AdvQueryAttribute(ColName = "property", ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "property", Length = 255, IsNullable = true, ColumnDescription = "属性")]
        [Display(Name = "属性")]
        public string property
        {
            get { return _property; }
            set
            {
                SetProperty(ref _property, value);
            }
        }

        private long? _Location_ID;


        /// <summary>
        /// 库位
        /// </summary>

        [AdvQueryAttribute(ColName = "Location_ID", ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Location_ID", IsNullable = true, ColumnDescription = "库位")]
        [Display(Name = "库位")]
        public long? Location_ID
        {
            get { return _Location_ID; }
            set
            {
                SetProperty(ref _Location_ID, value);
            }
        }

        private int? _Quantity;


        /// <summary>
        /// 数量
        /// </summary>

        [AdvQueryAttribute(ColName = "Quantity", ColDesc = "数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "Quantity", IsNullable = true, ColumnDescription = "数量")]
        [Display(Name = "数量")]
        public int? Quantity
        {
            get { return _Quantity; }
            set
            {
                SetProperty(ref _Quantity, value);
            }
        }

        private decimal? _UnitPrice;


        /// <summary>
        /// 单价
        /// </summary>

        [AdvQueryAttribute(ColName = "UnitPrice", ColDesc = "单价")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "UnitPrice", IsNullable = true, ColumnDescription = "单价")]
        [Display(Name = "单价")]
        public decimal? UnitPrice
        {
            get { return _UnitPrice; }
            set
            {
                SetProperty(ref _UnitPrice, value);
            }
        }

        private decimal? _Discount;


        /// <summary>
        /// 折扣
        /// </summary>

        [AdvQueryAttribute(ColName = "Discount", ColDesc = "折扣")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType = "Decimal", ColumnName = "Discount", DecimalDigits = 5, Length = 5, IsNullable = true, ColumnDescription = "折扣")]
        [Display(Name = "折扣")]
        public decimal? Discount
        {
            get { return _Discount; }
            set
            {
                SetProperty(ref _Discount, value);
            }
        }

        private decimal? _TransactionPrice;


        /// <summary>
        /// 成交单价
        /// </summary>

        [AdvQueryAttribute(ColName = "TransactionPrice", ColDesc = "成交单价")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TransactionPrice", IsNullable = true, ColumnDescription = "成交单价")]
        [Display(Name = "成交单价")]
        public decimal? TransactionPrice
        {
            get { return _TransactionPrice; }
            set
            {
                SetProperty(ref _TransactionPrice, value);
            }
        }

        private decimal? _TaxRate;


        /// <summary>
        /// 税率
        /// </summary>

        [AdvQueryAttribute(ColName = "TaxRate", ColDesc = "税率")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType = "Decimal", ColumnName = "TaxRate", DecimalDigits = 5, Length = 5, IsNullable = true, ColumnDescription = "税率")]
        [Display(Name = "税率")]
        public decimal? TaxRate
        {
            get { return _TaxRate; }
            set
            {
                SetProperty(ref _TaxRate, value);
            }
        }

        private decimal? _TaxAmount;


        /// <summary>
        /// 税额
        /// </summary>

        [AdvQueryAttribute(ColName = "TaxAmount", ColDesc = "税额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TaxAmount", IsNullable = true, ColumnDescription = "税额")]
        [Display(Name = "税额")]
        public decimal? TaxAmount
        {
            get { return _TaxAmount; }
            set
            {
                SetProperty(ref _TaxAmount, value);
            }
        }

        private decimal? _SubtotalAmount;


        /// <summary>
        /// 小计
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalAmount", ColDesc = "小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "SubtotalAmount", IsNullable = true, ColumnDescription = "小计")]
        [Display(Name = "小计")]
        public decimal? SubtotalAmount
        {
            get { return _SubtotalAmount; }
            set
            {
                SetProperty(ref _SubtotalAmount, value);
            }
        }

        private bool? _IsGift;


        /// <summary>
        /// 赠品
        /// </summary>

        [AdvQueryAttribute(ColName = "IsGift", ColDesc = "赠品")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "IsGift", IsNullable = true, ColumnDescription = "赠品")]
        [Display(Name = "赠品")]
        public bool? IsGift
        {
            get { return _IsGift; }
            set
            {
                SetProperty(ref _IsGift, value);
            }
        }

        private string _CustomertModel;


        /// <summary>
        /// 客户型号
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomertModel", ColDesc = "客户型号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "CustomertModel", Length = 50, IsNullable = true, ColumnDescription = "客户型号")]
        [Display(Name = "客户型号")]
        public string CustomertModel
        {
            get { return _CustomertModel; }
            set
            {
                SetProperty(ref _CustomertModel, value);
            }
        }

        private int? _ReturnedQty;


        /// <summary>
        /// 退回数量
        /// </summary>

        [AdvQueryAttribute(ColName = "ReturnedQty", ColDesc = "退回数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "ReturnedQty", IsNullable = true, ColumnDescription = "退回数量")]
        [Display(Name = "退回数量")]
        public int? ReturnedQty
        {
            get { return _ReturnedQty; }
            set
            {
                SetProperty(ref _ReturnedQty, value);
            }
        }

        private bool? _IsIncludeTax;


        /// <summary>
        /// 含税
        /// </summary>

        [AdvQueryAttribute(ColName = "IsIncludeTax", ColDesc = "含税")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "IsIncludeTax", IsNullable = true, ColumnDescription = "含税")]
        [Display(Name = "含税")]
        public bool? IsIncludeTax
        {
            get { return _IsIncludeTax; }
            set
            {
                SetProperty(ref _IsIncludeTax, value);
            }
        }

        private string _Summary;


        /// <summary>
        /// 摘要
        /// </summary>

        [AdvQueryAttribute(ColName = "Summary", ColDesc = "摘要")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Summary", Length = 255, IsNullable = true, ColumnDescription = "摘要")]
        [Display(Name = "摘要")]
        public string Summary
        {
            get { return _Summary; }
            set
            {
                SetProperty(ref _Summary, value);
            }
        }

        private int? _PrintStatus;


        /// <summary>
        /// 打印状态
        /// </summary>

        [AdvQueryAttribute(ColName = "PrintStatus", ColDesc = "打印状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "PrintStatus", IsNullable = true, ColumnDescription = "打印状态")]
        [Display(Name = "打印状态")]
        public int? PrintStatus
        {
            get { return _PrintStatus; }
            set
            {
                SetProperty(ref _PrintStatus, value);
            }
        }

        private int? _DataStatus;


        /// <summary>
        /// 数据状态
        /// </summary>

        [AdvQueryAttribute(ColName = "DataStatus", ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "DataStatus", IsNullable = true, ColumnDescription = "数据状态")]
        [Display(Name = "数据状态")]
        public int? DataStatus
        {
            get { return _DataStatus; }
            set
            {
                SetProperty(ref _DataStatus, value);
            }
        }

        private int? _ApprovalStatus;


        /// <summary>
        /// 审批状态
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalStatus", ColDesc = "审批状态")]
        [SugarColumn(ColumnDataType = "tinyint", SqlParameterDbType = "Byte", ColumnName = "ApprovalStatus", IsNullable = true, ColumnDescription = "审批状态")]
        [Display(Name = "审批状态")]
        public int? ApprovalStatus
        {
            get { return _ApprovalStatus; }
            set
            {
                SetProperty(ref _ApprovalStatus, value);
            }
        }

        private bool? _ApprovalResults;


        /// <summary>
        /// 审批结果
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalResults", ColDesc = "审批结果")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "ApprovalResults", IsNullable = true, ColumnDescription = "审批结果")]
        [Display(Name = "审批结果")]
        public bool? ApprovalResults
        {
            get { return _ApprovalResults; }
            set
            {
                SetProperty(ref _ApprovalResults, value);
            }
        }

        private string _ApprovalOpinions;


        /// <summary>
        /// 审批意见
        /// </summary>

        [AdvQueryAttribute(ColName = "ApprovalOpinions", ColDesc = "审批意见")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ApprovalOpinions", Length = 200, IsNullable = true, ColumnDescription = "审批意见")]
        [Display(Name = "审批意见")]
        public string ApprovalOpinions
        {
            get { return _ApprovalOpinions; }
            set
            {
                SetProperty(ref _ApprovalOpinions, value);
            }
        }
 

        private decimal? _TotalAmount;


        /// <summary>
        /// 合计金额
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalAmount", ColDesc = "合计金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalAmount", IsNullable = true, ColumnDescription = "合计金额")]
        [Display(Name = "合计金额")]
        public decimal? TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                SetProperty(ref _TotalAmount, value);
            }
        }

        private decimal? _TotalTaxAmount;


        /// <summary>
        /// 合计税额
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalTaxAmount", ColDesc = "合计税额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalTaxAmount", IsNullable = true, ColumnDescription = "合计税额")]
        [Display(Name = "合计税额")]
        public decimal? TotalTaxAmount
        {
            get { return _TotalTaxAmount; }
            set
            {
                SetProperty(ref _TotalTaxAmount, value);
            }
        }
 

        private decimal? _TotalQty;


        /// <summary>
        /// 合计数量
        /// </summary>

        [AdvQueryAttribute(ColName = "TotalQty", ColDesc = "合计数量")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "TotalQty", IsNullable = true, ColumnDescription = "合计数量")]
        [Display(Name = "合计数量")]
        public decimal? TotalQty
        {
            get { return _TotalQty; }
            set
            {
                SetProperty(ref _TotalQty, value);
            }
        }

        private long? _Category_ID;


        /// <summary>
        /// 类别
        /// </summary>

        [AdvQueryAttribute(ColName = "Category_ID", ColDesc = "类别")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Category_ID", IsNullable = true, ColumnDescription = "类别")]
        [Display(Name = "类别")]
        public long? Category_ID
        {
            get { return _Category_ID; }
            set
            {
                SetProperty(ref _Category_ID, value);
            }
        }

        private long? _Rack_ID;


        /// <summary>
        /// 货架
        /// </summary>

        [AdvQueryAttribute(ColName = "Rack_ID", ColDesc = "货架")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Rack_ID", IsNullable = true, ColumnDescription = "货架")]
        [Display(Name = "货架")]
        public long? Rack_ID
        {
            get { return _Rack_ID; }
            set
            {
                SetProperty(ref _Rack_ID, value);
            }
        }

        private string _UnitName;

        /// <summary>
        /// 单位
        /// </summary>

        [AdvQueryAttribute(ColName = "UnitName", ColDesc = "单位")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "UnitName", Length = 255, IsNullable = true, ColumnDescription = "单位")]
        [Display(Name = "单位")]
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                SetProperty(ref _UnitName, value);
            }
        }





        //如果为false,则不可以。
        private bool PK_FK_ID_Check()
        {
            bool rs = true;
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
                    Type type = typeof(View_PurEntryItems);

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

