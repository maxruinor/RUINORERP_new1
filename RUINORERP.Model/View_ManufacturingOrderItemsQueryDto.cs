
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/07/2025 21:28:01
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
    /// 制令单明细统计
    /// </summary>
    [Serializable()]
    [SugarTable("View_ManufacturingOrderItems")]
    public class View_ManufacturingOrderItemsQueryDto:BaseEntity, ICloneable
    {
        public View_ManufacturingOrderItemsQueryDto()
        {

        }

    
        private string _MONO;
        
        
        /// <summary>
        /// 制令单号
        /// </summary>
        [SugarColumn(ColumnName = "MONO",Length=100,IsNullable = true,ColumnDescription = "制令单号")]
        [Display(Name = "制令单号")]
        public string MONO 
        { 
            get{return _MONO;}            set{                SetProperty(ref _MONO, value);                }
        }

        private long? _Employee_ID;
        
        
        /// <summary>
        /// 经办人员
        /// </summary>
        [SugarColumn(ColumnName = "Employee_ID",IsNullable = true,ColumnDescription = "经办人员")]
        [Display(Name = "经办人员")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 生产部门
        /// </summary>
        [SugarColumn(ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "生产部门")]
        [Display(Name = "生产部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 需求客户
        /// </summary>
        [SugarColumn(ColumnName = "CustomerVendor_ID",IsNullable = true,ColumnDescription = "需求客户")]
        [Display(Name = "需求客户")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
        }

        private long? _CustomerVendor_ID_Out;
        
        
        /// <summary>
        /// 外发厂商
        /// </summary>
        [SugarColumn(ColumnName = "CustomerVendor_ID_Out",IsNullable = true,ColumnDescription = "外发厂商")]
        [Display(Name = "外发厂商")]
        public long? CustomerVendor_ID_Out 
        { 
            get{return _CustomerVendor_ID_Out;}            set{                SetProperty(ref _CustomerVendor_ID_Out, value);                }
        }

        private DateTime? _Created_at;
        
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间")]
        [Display(Name = "创建时间")]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}            set{                SetProperty(ref _Created_at, value);                }
        }

        private long? _Created_by;
        
        
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人")]
        [Display(Name = "创建人")]
        public long? Created_by 
        { 
            get{return _Created_by;}            set{                SetProperty(ref _Created_by, value);                }
        }

        private int? _ManufacturingQty;
        
        
        /// <summary>
        /// 生产数量
        /// </summary>
        [SugarColumn(ColumnName = "ManufacturingQty",IsNullable = true,ColumnDescription = "生产数量")]
        [Display(Name = "生产数量")]
        public int? ManufacturingQty 
        { 
            get{return _ManufacturingQty;}            set{                SetProperty(ref _ManufacturingQty, value);                }
        }

        private int? _DataStatus;
        
        
        /// <summary>
        /// 数据状态
        /// </summary>
        [SugarColumn(ColumnName = "DataStatus",IsNullable = true,ColumnDescription = "数据状态")]
        [Display(Name = "数据状态")]
        public int? DataStatus 
        { 
            get{return _DataStatus;}            set{                SetProperty(ref _DataStatus, value);                }
        }

        private string _Notes;
        
        
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "Notes",Length=255,IsNullable = true,ColumnDescription = "备注")]
        [Display(Name = "备注")]
        public string Notes 
        { 
            get{return _Notes;}            set{                SetProperty(ref _Notes, value);                }
        }

        private bool? _IsOutSourced;
        
        
        /// <summary>
        /// 是否托工
        /// </summary>
        [SugarColumn(ColumnName = "IsOutSourced",IsNullable = true,ColumnDescription = "是否托工")]
        [Display(Name = "是否托工")]
        public bool? IsOutSourced 
        { 
            get{return _IsOutSourced;}            set{                SetProperty(ref _IsOutSourced, value);                }
        }

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 产品详情
        /// </summary>
        [SugarColumn(ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "产品详情")]
        [Display(Name = "产品详情")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
        }

        private long? _Location_ID;
        
        
        /// <summary>
        /// 库位
        /// </summary>
        [SugarColumn(ColumnName = "Location_ID",IsNullable = true,ColumnDescription = "库位")]
        [Display(Name = "库位")]
        public long? Location_ID 
        { 
            get{return _Location_ID;}            set{                SetProperty(ref _Location_ID, value);                }
        }

        private decimal? _ShouldSendQty;
        
        
        /// <summary>
        /// 应发数
        /// </summary>
        [SugarColumn(ColumnName = "ShouldSendQty",Length=10,IsNullable = true,ColumnDescription = "应发数")]
        [Display(Name = "应发数")]
        public decimal? ShouldSendQty 
        { 
            get{return _ShouldSendQty;}            set{                SetProperty(ref _ShouldSendQty, value);                }
        }

        private decimal? _ActualSentQty;
        
        
        /// <summary>
        /// 实发数
        /// </summary>
        [SugarColumn(ColumnName = "ActualSentQty",Length=10,IsNullable = true,ColumnDescription = "实发数")]
        [Display(Name = "实发数")]
        public decimal? ActualSentQty 
        { 
            get{return _ActualSentQty;}            set{                SetProperty(ref _ActualSentQty, value);                }
        }

        private decimal? _UnitCost;
        
        
        /// <summary>
        /// 单位成本
        /// </summary>
        [SugarColumn(ColumnName = "UnitCost",IsNullable = true,ColumnDescription = "单位成本")]
        [Display(Name = "单位成本")]
        public decimal? UnitCost 
        { 
            get{return _UnitCost;}            set{                SetProperty(ref _UnitCost, value);                }
        }

        private decimal? _SubtotalUnitCost;
        
        
        /// <summary>
        /// 成本小计
        /// </summary>
        [SugarColumn(ColumnName = "SubtotalUnitCost",Length=10,IsNullable = true,ColumnDescription = "成本小计")]
        [Display(Name = "成本小计")]
        public decimal? SubtotalUnitCost 
        { 
            get{return _SubtotalUnitCost;}            set{                SetProperty(ref _SubtotalUnitCost, value);                }
        }

        private long? _BOM_ID;
        
        
        /// <summary>
        /// 配方
        /// </summary>
        [SugarColumn(ColumnName = "BOM_ID",IsNullable = true,ColumnDescription = "配方")]
        [Display(Name = "配方")]
        public long? BOM_ID 
        { 
            get{return _BOM_ID;}            set{                SetProperty(ref _BOM_ID, value);                }
        }

        private string _BOM_NO;
        
        
        /// <summary>
        /// 配方编号
        /// </summary>
        [SugarColumn(ColumnName = "BOM_NO",Length=50,IsNullable = true,ColumnDescription = "配方编号")]
        [Display(Name = "配方编号")]
        public string BOM_NO 
        { 
            get{return _BOM_NO;}            set{                SetProperty(ref _BOM_NO, value);                }
        }

        private string _Summary;
        
        
        /// <summary>
        /// 摘要
        /// </summary>
        [SugarColumn(ColumnName = "Summary",Length=255,IsNullable = true,ColumnDescription = "摘要")]
        [Display(Name = "摘要")]
        public string Summary 
        { 
            get{return _Summary;}            set{                SetProperty(ref _Summary, value);                }
        }

        private string _property;
        
        
        /// <summary>
        /// 属性
        /// </summary>
        [SugarColumn(ColumnName = "property",Length=255,IsNullable = true,ColumnDescription = "属性")]
        [Display(Name = "属性")]
        public string property 
        { 
            get{return _property;}            set{                SetProperty(ref _property, value);                }
        }

        private long? _ProdBaseID;
        
        
        /// <summary>
        /// 产品主信息
        /// </summary>
        [SugarColumn(ColumnName = "ProdBaseID",IsNullable = true,ColumnDescription = "产品主信息")]
        [Display(Name = "产品主信息")]
        public long? ProdBaseID 
        { 
            get{return _ProdBaseID;}            set{                SetProperty(ref _ProdBaseID, value);                }
        }

        private string _SKU;
        
        
        /// <summary>
        /// SKU码
        /// </summary>
        [SugarColumn(ColumnName = "SKU",Length=80,IsNullable = true,ColumnDescription = "SKU码")]
        [Display(Name = "SKU码")]
        public string SKU 
        { 
            get{return _SKU;}            set{                SetProperty(ref _SKU, value);                }
        }

        private string _CNName;
        
        
        /// <summary>
        /// 品名
        /// </summary>
        [SugarColumn(ColumnName = "CNName",Length=255,IsNullable = true,ColumnDescription = "品名")]
        [Display(Name = "品名")]
        public string CNName 
        { 
            get{return _CNName;}            set{                SetProperty(ref _CNName, value);                }
        }

        private string _Specifications;
        
        
        /// <summary>
        /// 规格
        /// </summary>
        [SugarColumn(ColumnName = "Specifications",Length=1000,IsNullable = true,ColumnDescription = "规格")]
        [Display(Name = "规格")]
        public string Specifications 
        { 
            get{return _Specifications;}            set{                SetProperty(ref _Specifications, value);                }
        }

        private int? _Quantity;
        
        
        /// <summary>
        /// 实际库存
        /// </summary>
        [SugarColumn(ColumnName = "Quantity",IsNullable = true,ColumnDescription = "实际库存")]
        [Display(Name = "实际库存")]
        public int? Quantity 
        { 
            get{return _Quantity;}            set{                SetProperty(ref _Quantity, value);                }
        }

        private string _prop;
        
        
        /// <summary>
        /// 属性
        /// </summary>
        [SugarColumn(ColumnName = "prop",Length=255,IsNullable = true,ColumnDescription = "属性")]
        [Display(Name = "属性")]
        public string prop 
        { 
            get{return _prop;}            set{                SetProperty(ref _prop, value);                }
        }

        private string _ProductNo;
        
        
        /// <summary>
        /// 品号
        /// </summary>
        [SugarColumn(ColumnName = "ProductNo",Length=40,IsNullable = true,ColumnDescription = "品号")]
        [Display(Name = "品号")]
        public string ProductNo 
        { 
            get{return _ProductNo;}            set{                SetProperty(ref _ProductNo, value);                }
        }

        private string _Model;
        
        
        /// <summary>
        /// 型号
        /// </summary>
        [SugarColumn(ColumnName = "Model",Length=50,IsNullable = true,ColumnDescription = "型号")]
        [Display(Name = "型号")]
        public string Model 
        { 
            get{return _Model;}            set{                SetProperty(ref _Model, value);                }
        }

        private long? _Category_ID;
        
        
        /// <summary>
        /// 类别
        /// </summary>
        [SugarColumn(ColumnName = "Category_ID",IsNullable = true,ColumnDescription = "类别")]
        [Display(Name = "类别")]
        public long? Category_ID 
        { 
            get{return _Category_ID;}            set{                SetProperty(ref _Category_ID, value);                }
        }

        private long? _Type_ID;
        
        
        /// <summary>
        /// 产品类型
        /// </summary>
        [SugarColumn(ColumnName = "Type_ID",IsNullable = true,ColumnDescription = "产品类型")]
        [Display(Name = "产品类型")]
        public long? Type_ID 
        { 
            get{return _Type_ID;}            set{                SetProperty(ref _Type_ID, value);                }
        }





  
        

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

