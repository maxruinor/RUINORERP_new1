
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/27/2024 14:19:16
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
    /// 库存视图
    /// </summary>
    [Serializable()]
    [SugarTable("View_Inventory")]
    public partial class View_Inventory : BaseViewEntity
    {
        public View_Inventory()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_Inventory" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        private long _Inventory_ID;
        /// <summary>
        /// 库存
        /// </summary>

        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Inventory_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "", IsPrimaryKey = true)]
        public long Inventory_ID
        {
            get { return _Inventory_ID; }
            set
            {
                base.PrimaryKeyID = _Inventory_ID;
                SetProperty(ref _Inventory_ID, value);
            }
        }

        private string _ProductNo;


        /// <summary>
        /// 品号
        /// </summary>

        [AdvQueryAttribute(ColName = "ProductNo", ColDesc = "品号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "ProductNo", Length = 40, IsNullable = true, ColumnDescription = "品号")]
        [Display(Name = "品号")]
        public string ProductNo
        {
            get { return _ProductNo; }            set
            {                SetProperty(ref _ProductNo, value);
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
            get { return _CNName; }            set
            {                SetProperty(ref _CNName, value);
            }
        }

        private string _prop;


        /// <summary>
        /// 属性
        /// </summary>

        [AdvQueryAttribute(ColName = "prop", ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "prop", Length = 255, IsNullable = true, ColumnDescription = "属性")]
        [Display(Name = "属性")]
        public string prop
        {
            get { return _prop; }            set
            {                SetProperty(ref _prop, value);
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
            get { return _Specifications; }            set
            {                SetProperty(ref _Specifications, value);
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
            get { return _Model; }            set
            {                SetProperty(ref _Model, value);
            }
        }

        private int? _Quantity;


        /// <summary>
        /// 实际库存
        /// </summary>

        [AdvQueryAttribute(ColName = "Quantity", ColDesc = "实际库存")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "Quantity", IsNullable = true, ColumnDescription = "实际库存")]
        [Display(Name = "实际库存")]
        public int? Quantity
        {
            get { return _Quantity; }            set
            {                SetProperty(ref _Quantity, value);
            }
        }

        private long? _Type_ID;


        /// <summary>
        /// 产品类型
        /// </summary>

        [AdvQueryAttribute(ColName = "Type_ID", ColDesc = "产品类型")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Type_ID", IsNullable = true, ColumnDescription = "产品类型")]
        [Display(Name = "产品类型")]
        [FKRelationAttribute("tb_ProductType", "Type_ID")]
        public long? Type_ID
        {
            get { return _Type_ID; }            set
            {                SetProperty(ref _Type_ID, value);
            }
        }

        private long? _Unit_ID;


        /// <summary>
        /// 单位
        /// </summary>

        [AdvQueryAttribute(ColName = "Unit_ID", ColDesc = "单位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Unit_ID", IsNullable = true, ColumnDescription = "单位")]
        [Display(Name = "单位")]
        [FKRelationAttribute("tb_Unit", "Unit_ID")]
        public long? Unit_ID
        {
            get { return _Unit_ID; }            set
            {                SetProperty(ref _Unit_ID, value);
            }
        }

        private long? _Category_ID;


        /// <summary>
        /// 类别
        /// </summary>

        [AdvQueryAttribute(ColName = "Category_ID", ColDesc = "类别")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Category_ID", IsNullable = true, ColumnDescription = "类别")]
        [Display(Name = "类别")]
        [FKRelationAttribute("tb_ProdCategories", "Category_ID")]
        public long? Category_ID
        {
            get { return _Category_ID; }            set
            {                SetProperty(ref _Category_ID, value);
            }
        }

        private long? _CustomerVendor_ID;


        /// <summary>
        /// 厂商
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID", ColDesc = "厂商")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "CustomerVendor_ID", IsNullable = true, ColumnDescription = "厂商")]
        [Display(Name = "厂商")]
        [FKRelationAttribute("tb_CustomerVendor", "CustomerVendor_ID")]
        public long? CustomerVendor_ID
        {
            get { return _CustomerVendor_ID; }            set
            {                SetProperty(ref _CustomerVendor_ID, value);
            }
        }

        private long? _DepartmentID;


        /// <summary>
        /// 部门
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID", ColDesc = "部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "DepartmentID", IsNullable = true, ColumnDescription = "部门")]
        [Display(Name = "部门")]
        [FKRelationAttribute("tb_Department", "DepartmentID")]
        public long? DepartmentID
        {
            get { return _DepartmentID; }            set
            {                SetProperty(ref _DepartmentID, value);
            }
        }

        private int? _SourceType;


        /// <summary>
        /// 产品来源
        /// </summary>

        [AdvQueryAttribute(ColName = "SourceType", ColDesc = "产品来源")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "SourceType", IsNullable = true, ColumnDescription = "产品来源")]
        [Display(Name = "产品来源")]
        public int? SourceType
        {
            get { return _SourceType; }            set
            {                SetProperty(ref _SourceType, value);
            }
        }

        private string _Brand;


        /// <summary>
        /// 品牌
        /// </summary>

        [AdvQueryAttribute(ColName = "Brand", ColDesc = "品牌")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType = "String", ColumnName = "Brand", Length = 50, IsNullable = true, ColumnDescription = "品牌")]
        [Display(Name = "品牌")]
        public string Brand
        {
            get { return _Brand; }            set
            {                SetProperty(ref _Brand, value);
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
            get { return _SKU; }            set
            {                SetProperty(ref _SKU, value);
            }
        }

        private long? _Rack_ID;


        /// <summary>
        /// 默认货架
        /// </summary>

        [AdvQueryAttribute(ColName = "Rack_ID", ColDesc = "货架")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Rack_ID", IsNullable = true, ColumnDescription = "货架")]
        [Display(Name = "货架")]
        [FKRelationAttribute("tb_StorageRack", "Rack_ID")]
        public long? Rack_ID
        {
            get { return _Rack_ID; }            set
            {                SetProperty(ref _Rack_ID, value);
            }
        }

        private int? _Alert_Quantity;


        /// <summary>
        /// 预警值
        /// </summary>

        [AdvQueryAttribute(ColName = "Alert_Quantity", ColDesc = "预警值")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "Alert_Quantity", IsNullable = true, ColumnDescription = "预警值")]
        [Display(Name = "预警值")]
        public int? Alert_Quantity
        {
            get { return _Alert_Quantity; }            set
            {                SetProperty(ref _Alert_Quantity, value);
            }
        }

        private decimal? _Inv_Cost;


        /// <summary>
        /// 产品成本
        /// </summary>

        [AdvQueryAttribute(ColName = "Inv_Cost", ColDesc = "产品成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "Inv_Cost", IsNullable = true, ColumnDescription = "产品成本")]
        [Display(Name = "产品成本")]
        public decimal? Inv_Cost
        {
            get { return _Inv_Cost; }            set
            {                SetProperty(ref _Inv_Cost, value);
            }
        }

        private long? _Location_ID;


        /// <summary>
        /// 库位
        /// </summary>

        [AdvQueryAttribute(ColName = "Location_ID", ColDesc = "库位")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Location_ID", IsNullable = true, ColumnDescription = "库位")]
        [Display(Name = "库位")]
        [FKRelationAttribute("tb_Location", "Location_ID")]
        public long? Location_ID
        {
            get { return _Location_ID; }            set
            {                SetProperty(ref _Location_ID, value);
            }
        }

        private long? _ProdDetailID;


        /// <summary>
        /// 产品
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdDetailID", ColDesc = "产品")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "ProdDetailID", IsNullable = true, ColumnDescription = "产品")]
        [Display(Name = "产品")]
        public long? ProdDetailID
        {
            get { return _ProdDetailID; }            set
            {                SetProperty(ref _ProdDetailID, value);
            }
        }

     

        private int? _On_the_way_Qty;


        /// <summary>
        /// 在途库存
        /// </summary>

        [AdvQueryAttribute(ColName = "On_the_way_Qty", ColDesc = "在途库存")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "On_the_way_Qty", IsNullable = true, ColumnDescription = "在途库存")]
        [Display(Name = "在途库存")]
        public int? On_the_way_Qty
        {
            get { return _On_the_way_Qty; }            set
            {                SetProperty(ref _On_the_way_Qty, value);
            }
        }

        private int? _Sale_Qty;


        /// <summary>
        /// 拟销售量
        /// </summary>

        [AdvQueryAttribute(ColName = "Sale_Qty", ColDesc = "拟销售量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "Sale_Qty", IsNullable = true, ColumnDescription = "拟销售量")]
        [Display(Name = "拟销售量")]
        public int? Sale_Qty
        {
            get { return _Sale_Qty; }            set
            {                SetProperty(ref _Sale_Qty, value);
            }
        }

        private int? _MakingQty;


        /// <summary>
        /// 在制数量
        /// </summary>

        [AdvQueryAttribute(ColName = "MakingQty", ColDesc = "在制数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "MakingQty", IsNullable = true, ColumnDescription = "在制数量")]
        [Display(Name = "在制数量")]
        public int? MakingQty
        {
            get { return _MakingQty; }            set
            {                SetProperty(ref _MakingQty, value);
            }
        }

        private int? _NotOutQty;


        /// <summary>
        /// 未发数量
        /// </summary>

        [AdvQueryAttribute(ColName = "NotOutQty", ColDesc = "未发数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "NotOutQty", IsNullable = true, ColumnDescription = "未发数量")]
        [Display(Name = "未发数量")]
        public int? NotOutQty
        {
            get { return _NotOutQty; }            set
            {                SetProperty(ref _NotOutQty, value);
            }
        }

        private DateTime? _LatestOutboundTime;


        /// <summary>
        /// 最新出库时间
        /// </summary>

        [AdvQueryAttribute(ColName = "LatestOutboundTime", ColDesc = "最新出库时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "LatestOutboundTime", IsNullable = true, ColumnDescription = "最新出库时间")]
        [Display(Name = "最新出库时间")]
        public DateTime? LatestOutboundTime
        {
            get { return _LatestOutboundTime; }            set
            {                SetProperty(ref _LatestOutboundTime, value);
            }
        }

        private DateTime? _LatestStorageTime;


        /// <summary>
        /// 最新入库时间
        /// </summary>

        [AdvQueryAttribute(ColName = "LatestStorageTime", ColDesc = "最新入库时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "LatestStorageTime", IsNullable = true, ColumnDescription = "最新入库时间")]
        [Display(Name = "最新入库时间")]
        public DateTime? LatestStorageTime
        {
            get { return _LatestStorageTime; }            set
            {                SetProperty(ref _LatestStorageTime, value);
            }
        }
        private DateTime? _LastInventoryDate;
        /// <summary>
        /// 最后盘点时间
        /// </summary>
        [AdvQueryAttribute(ColName = "LastInventoryDate", ColDesc = "最后盘点时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "LastInventoryDate", IsNullable = true, ColumnDescription = "最后盘点时间")]
        public DateTime? LastInventoryDate
        {
            get { return _LastInventoryDate; }
            set
            {
                SetProperty(ref _LastInventoryDate, value);
            }
        }

        private long? _BOM_ID;
        /// <summary>
        /// 标准配方
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_ID", ColDesc = "标准配方")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "BOM_ID", DecimalDigits = 0, IsNullable = false, ColumnDescription = "标准配方")]
        [FKRelationAttribute("tb_BOM_S", "BOM_ID")]
        public long? BOM_ID
        {
            get { return _BOM_ID; }
            set
            {
                SetProperty(ref _BOM_ID, value);
            }
        }

        #region 扩展属性


        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(BOM_ID))]
        public virtual tb_BOM_S tb_bom_s { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_S.ProdDetailID))]
        public virtual List<tb_BOM_S> tb_BOM_Ss { get; set; }
        #endregion



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
                    Type type = typeof(View_Inventory);

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

