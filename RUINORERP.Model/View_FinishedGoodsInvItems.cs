
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 11:25:42
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
    /// 缴库明细统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_FinishedGoodsInvItems")]
    public partial class View_FinishedGoodsInvItems: BaseViewEntity
    {
        public View_FinishedGoodsInvItems()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_FinishedGoodsInvItems" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    

        private string _DeliveryBillNo;
        
        
        /// <summary>
        /// 缴库单号
        /// </summary>

        [AdvQueryAttribute(ColName = "DeliveryBillNo",ColDesc = "缴库单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "DeliveryBillNo" ,Length=50,IsNullable = true,ColumnDescription = "缴库单号" )]
        [Display(Name = "缴库单号")]
        public string DeliveryBillNo 
        { 
            get{return _DeliveryBillNo;}            set{                SetProperty(ref _DeliveryBillNo, value);                }
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
        /// 需求部门
        /// </summary>

        [AdvQueryAttribute(ColName = "DepartmentID",ColDesc = "需求部门")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" ,IsNullable = true,ColumnDescription = "需求部门" )]
        [Display(Name = "需求部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 生产商
        /// </summary>

        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "生产商")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" ,IsNullable = true,ColumnDescription = "生产商" )]
        [Display(Name = "生产商")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
        }

        private DateTime? _DeliveryDate;
        
        
        /// <summary>
        /// 缴库日期
        /// </summary>

        [AdvQueryAttribute(ColName = "DeliveryDate",ColDesc = "缴库日期")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "DeliveryDate" ,IsNullable = true,ColumnDescription = "缴库日期" )]
        [Display(Name = "缴库日期")]
        public DateTime? DeliveryDate 
        { 
            get{return _DeliveryDate;}            set{                SetProperty(ref _DeliveryDate, value);                }
        }

        private DateTime? _Created_at;
        
        
        /// <summary>
        /// 创建时间
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        [Display(Name = "创建时间")]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}            set{                SetProperty(ref _Created_at, value);                }
        }

        private long? _Created_by;
        
        
        /// <summary>
        /// 创建人
        /// </summary>

        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" ,IsNullable = true,ColumnDescription = "创建人" )]
        [Display(Name = "创建人")]
        public long? Created_by 
        { 
            get{return _Created_by;}            set{                SetProperty(ref _Created_by, value);                }
        }

        private string _MONo;
        
        
        /// <summary>
        /// 制令单号
        /// </summary>

        [AdvQueryAttribute(ColName = "MONo",ColDesc = "制令单号")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "MONo" ,Length=50,IsNullable = true,ColumnDescription = "制令单号" )]
        [Display(Name = "制令单号")]
        public string MONo 
        { 
            get{return _MONo;}            set{                SetProperty(ref _MONo, value);                }
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
            get{return _Unit_ID;}            set{                SetProperty(ref _Unit_ID, value);                }
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
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
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

        private long? _Rack_ID;
        
        
        /// <summary>
        /// 货架
        /// </summary>

        [AdvQueryAttribute(ColName = "Rack_ID",ColDesc = "货架")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Rack_ID" ,IsNullable = true,ColumnDescription = "货架" )]
        [Display(Name = "货架")]
        public long? Rack_ID 
        { 
            get{return _Rack_ID;}            set{                SetProperty(ref _Rack_ID, value);                }
        }

        private int? _PayableQty;
        
        
        /// <summary>
        /// 应缴数量
        /// </summary>

        [AdvQueryAttribute(ColName = "PayableQty",ColDesc = "应缴数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "PayableQty" ,IsNullable = true,ColumnDescription = "应缴数量" )]
        [Display(Name = "应缴数量")]
        public int? PayableQty 
        { 
            get{return _PayableQty;}            set{                SetProperty(ref _PayableQty, value);                }
        }

        private int? _Qty;
        
        
        /// <summary>
        /// 实缴数量
        /// </summary>

        [AdvQueryAttribute(ColName = "Qty",ColDesc = "实缴数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Qty" ,IsNullable = true,ColumnDescription = "实缴数量" )]
        [Display(Name = "实缴数量")]
        public int? Qty 
        { 
            get{return _Qty;}            set{                SetProperty(ref _Qty, value);                }
        }

        private decimal? _UnitCost;
        
        
        /// <summary>
        /// 单位成本
        /// </summary>

        [AdvQueryAttribute(ColName = "UnitCost",ColDesc = "单位成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "UnitCost" ,IsNullable = true,ColumnDescription = "单位成本" )]
        [Display(Name = "单位成本")]
        public decimal? UnitCost 
        { 
            get{return _UnitCost;}            set{                SetProperty(ref _UnitCost, value);                }
        }

        private int? _UnpaidQty;
        
        
        /// <summary>
        /// 未缴数量
        /// </summary>

        [AdvQueryAttribute(ColName = "UnpaidQty",ColDesc = "未缴数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "UnpaidQty" ,IsNullable = true,ColumnDescription = "未缴数量" )]
        [Display(Name = "未缴数量")]
        public int? UnpaidQty 
        { 
            get{return _UnpaidQty;}            set{                SetProperty(ref _UnpaidQty, value);                }
        }

        private decimal? _NetMachineHours;
        
        
        /// <summary>
        /// 单位实际机时
        /// </summary>

        [AdvQueryAttribute(ColName = "NetMachineHours",ColDesc = "单位实际机时")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "NetMachineHours" , DecimalDigits = 15,Length=15,IsNullable = true,ColumnDescription = "单位实际机时" )]
        [Display(Name = "单位实际机时")]
        public decimal? NetMachineHours 
        { 
            get{return _NetMachineHours;}            set{                SetProperty(ref _NetMachineHours, value);                }
        }

        private decimal? _NetWorkingHours;
        
        
        /// <summary>
        /// 单位实际工时
        /// </summary>

        [AdvQueryAttribute(ColName = "NetWorkingHours",ColDesc = "单位实际工时")]
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "NetWorkingHours" , DecimalDigits = 15,Length=15,IsNullable = true,ColumnDescription = "单位实际工时" )]
        [Display(Name = "单位实际工时")]
        public decimal? NetWorkingHours 
        { 
            get{return _NetWorkingHours;}            set{                SetProperty(ref _NetWorkingHours, value);                }
        }

        private decimal? _ApportionedCost;
        
        
        /// <summary>
        /// 单位分摊成本
        /// </summary>

        [AdvQueryAttribute(ColName = "ApportionedCost",ColDesc = "单位分摊成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ApportionedCost" ,IsNullable = true,ColumnDescription = "单位分摊成本" )]
        [Display(Name = "单位分摊成本")]
        public decimal? ApportionedCost 
        { 
            get{return _ApportionedCost;}            set{                SetProperty(ref _ApportionedCost, value);                }
        }

        private decimal? _ManuFee;
        
        
        /// <summary>
        /// 单位制造费用
        /// </summary>

        [AdvQueryAttribute(ColName = "ManuFee",ColDesc = "单位制造费用")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ManuFee" ,IsNullable = true,ColumnDescription = "单位制造费用" )]
        [Display(Name = "单位制造费用")]
        public decimal? ManuFee 
        { 
            get{return _ManuFee;}            set{                SetProperty(ref _ManuFee, value);                }
        }

        private decimal? _MaterialCost;
        
        
        /// <summary>
        /// 单位材料成本
        /// </summary>

        [AdvQueryAttribute(ColName = "MaterialCost",ColDesc = "单位材料成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "MaterialCost" ,IsNullable = true,ColumnDescription = "单位材料成本" )]
        [Display(Name = "单位材料成本")]
        public decimal? MaterialCost 
        { 
            get{return _MaterialCost;}            set{                SetProperty(ref _MaterialCost, value);                }
        }

        private decimal? _SubtotalMaterialCost;
        
        
        /// <summary>
        /// 材料小计
        /// </summary>

        [AdvQueryAttribute(ColName = "SubtotalMaterialCost",ColDesc = "材料小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalMaterialCost" ,IsNullable = true,ColumnDescription = "材料小计" )]
        [Display(Name = "材料小计")]
        public decimal? SubtotalMaterialCost 
        { 
            get{return _SubtotalMaterialCost;}            set{                SetProperty(ref _SubtotalMaterialCost, value);                }
        }

        private decimal? _ProductionAllCost;


        /// <summary>
        /// 成本小计
        /// </summary>

        [AdvQueryAttribute(ColName = "ProductionAllCost",ColDesc = "成本小计")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "ProductionAllCost" ,IsNullable = true,ColumnDescription = "成本小计")]
        [Display(Name = "成本小计")]
        public decimal? ProductionAllCost 
        { 
            get{return _ProductionAllCost;}            set{                SetProperty(ref _ProductionAllCost, value);                }
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
            get{return _Summary;}            set{                SetProperty(ref _Summary, value);                }
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

        private long? _ProdBaseID;
        
        
        /// <summary>
        /// 产品主信息
        /// </summary>

        [AdvQueryAttribute(ColName = "ProdBaseID",ColDesc = "产品主信息")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdBaseID" ,IsNullable = true,ColumnDescription = "产品主信息" )]
        [Display(Name = "产品主信息")]
        public long? ProdBaseID 
        { 
            get{return _ProdBaseID;}            set{                SetProperty(ref _ProdBaseID, value);                }
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

        private int? _Quantity;
        
        
        /// <summary>
        /// 实际库存
        /// </summary>

        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "实际库存")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" ,IsNullable = true,ColumnDescription = "实际库存" )]
        [Display(Name = "实际库存")]
        public int? Quantity 
        { 
            get{return _Quantity;}            set{                SetProperty(ref _Quantity, value);                }
        }

        private string _prop;
        
        
        /// <summary>
        /// 属性
        /// </summary>

        [AdvQueryAttribute(ColName = "prop",ColDesc = "属性")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "prop" ,Length=255,IsNullable = true,ColumnDescription = "属性" )]
        [Display(Name = "属性")]
        public string prop 
        { 
            get{return _prop;}            set{                SetProperty(ref _prop, value);                }
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
            get{return _ProductNo;}            set{                SetProperty(ref _ProductNo, value);                }
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

        private long? _Category_ID;
        
        
        /// <summary>
        /// 类别
        /// </summary>

        [AdvQueryAttribute(ColName = "Category_ID",ColDesc = "类别")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Category_ID" ,IsNullable = true,ColumnDescription = "类别" )]
        [Display(Name = "类别")]
        public long? Category_ID 
        { 
            get{return _Category_ID;}            set{                SetProperty(ref _Category_ID, value);                }
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

        private int? _DataStatus;
        
        
        /// <summary>
        /// 数据状态
        /// </summary>

        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DataStatus" ,IsNullable = true,ColumnDescription = "数据状态" )]
        [Display(Name = "数据状态")]
        public int? DataStatus 
        { 
            get{return _DataStatus;}            set{                SetProperty(ref _DataStatus, value);                }
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

        private bool? _IsOutSourced;
        
        
        /// <summary>
        /// 是否托工
        /// </summary>

        [AdvQueryAttribute(ColName = "IsOutSourced",ColDesc = "是否托工")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsOutSourced" ,IsNullable = true,ColumnDescription = "是否托工" )]
        [Display(Name = "是否托工")]
        public bool? IsOutSourced 
        { 
            get{return _IsOutSourced;}            set{                SetProperty(ref _IsOutSourced, value);                }
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
                    Type type = typeof(View_FinishedGoodsInvItems);
                    
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

