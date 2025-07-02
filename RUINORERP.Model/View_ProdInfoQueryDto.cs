
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/02/2025 15:03:21
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
    /// 产品信息视图
    /// </summary>
    [Serializable()]
    [SugarTable("View_ProdInfo")]
    public class View_ProdInfoQueryDto:BaseEntity, ICloneable
    {
        public View_ProdInfoQueryDto()
        {

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

        private long? _ProdDetailID;
        
        
        /// <summary>
        /// 产品
        /// </summary>
        [SugarColumn(ColumnName = "ProdDetailID",IsNullable = true,ColumnDescription = "产品")]
        [Display(Name = "产品")]
        public long? ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
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

        private long? _Unit_ID;
        
        
        /// <summary>
        /// 单位
        /// </summary>
        [SugarColumn(ColumnName = "Unit_ID",IsNullable = true,ColumnDescription = "单位")]
        [Display(Name = "单位")]
        public long? Unit_ID 
        { 
            get{return _Unit_ID;}            set{                SetProperty(ref _Unit_ID, value);                }
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

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 厂商
        /// </summary>
        [SugarColumn(ColumnName = "CustomerVendor_ID",IsNullable = true,ColumnDescription = "厂商")]
        [Display(Name = "厂商")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 部门
        /// </summary>
        [SugarColumn(ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "部门")]
        [Display(Name = "部门")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private string _ENName;
        
        
        /// <summary>
        /// 英文名称
        /// </summary>
        [SugarColumn(ColumnName = "ENName",Length=255,IsNullable = true,ColumnDescription = "英文名称")]
        [Display(Name = "英文名称")]
        public string ENName 
        { 
            get{return _ENName;}            set{                SetProperty(ref _ENName, value);                }
        }

        private string _Brand;
        
        
        /// <summary>
        /// 品牌
        /// </summary>
        [SugarColumn(ColumnName = "Brand",Length=50,IsNullable = true,ColumnDescription = "品牌")]
        [Display(Name = "品牌")]
        public string Brand 
        { 
            get{return _Brand;}            set{                SetProperty(ref _Brand, value);                }
        }

        private string _VendorModelCode;
        
        
        /// <summary>
        /// 厂商型号
        /// </summary>
        [SugarColumn(ColumnName = "VendorModelCode",Length=50,IsNullable = true,ColumnDescription = "厂商型号")]
        [Display(Name = "厂商型号")]
        public string VendorModelCode 
        { 
            get{return _VendorModelCode;}            set{                SetProperty(ref _VendorModelCode, value);                }
        }

        private byte[] _Images;
        
        
        /// <summary>
        /// 产品图
        /// </summary>
        [SugarColumn(ColumnName = "Images",IsNullable = true,ColumnDescription = "产品图")]
        [Display(Name = "产品图")]
        public byte[] Images 
        { 
            get{return _Images;}            set{                SetProperty(ref _Images, value);                }
        }

        private long? _Rack_ID;
        
        
        /// <summary>
        /// 默认货架
        /// </summary>
        [SugarColumn(ColumnName = "Rack_ID",IsNullable = true,ColumnDescription = "默认货架")]
        [Display(Name = "默认货架")]
        public long? Rack_ID 
        { 
            get{return _Rack_ID;}            set{                SetProperty(ref _Rack_ID, value);                }
        }

        private bool? _Is_available;
        
        
        /// <summary>
        /// 是否可用
        /// </summary>
        [SugarColumn(ColumnName = "Is_available",IsNullable = true,ColumnDescription = "是否可用")]
        [Display(Name = "是否可用")]
        public bool? Is_available 
        { 
            get{return _Is_available;}            set{                SetProperty(ref _Is_available, value);                }
        }

        private bool? _Is_enabled;
        
        
        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnName = "Is_enabled",IsNullable = true,ColumnDescription = "是否启用")]
        [Display(Name = "是否启用")]
        public bool? Is_enabled 
        { 
            get{return _Is_enabled;}            set{                SetProperty(ref _Is_enabled, value);                }
        }

        private bool? _产品可用;
        
        
        /// <summary>
        /// 是否可用
        /// </summary>
        [SugarColumn(ColumnName = "产品可用",IsNullable = true,ColumnDescription = "是否可用")]
        [Display(Name = "是否可用")]
        public bool? 产品可用 
        { 
            get{return _产品可用;}            set{                SetProperty(ref _产品可用, value);                }
        }

        private bool? _产品启用;
        
        
        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnName = "产品启用",IsNullable = true,ColumnDescription = "是否启用")]
        [Display(Name = "是否启用")]
        public bool? 产品启用 
        { 
            get{return _产品启用;}            set{                SetProperty(ref _产品启用, value);                }
        }

        private bool? _SKU可用;
        
        
        /// <summary>
        /// 是否可用
        /// </summary>
        [SugarColumn(ColumnName = "SKU可用",IsNullable = true,ColumnDescription = "是否可用")]
        [Display(Name = "是否可用")]
        public bool? SKU可用 
        { 
            get{return _SKU可用;}            set{                SetProperty(ref _SKU可用, value);                }
        }

        private bool? _SKU启用;
        
        
        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnName = "SKU启用",IsNullable = true,ColumnDescription = "是否启用")]
        [Display(Name = "是否启用")]
        public bool? SKU启用 
        { 
            get{return _SKU启用;}            set{                SetProperty(ref _SKU启用, value);                }
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

        private bool? _SalePublish;
        
        
        /// <summary>
        /// 参与分销
        /// </summary>
        [SugarColumn(ColumnName = "SalePublish",IsNullable = true,ColumnDescription = "参与分销")]
        [Display(Name = "参与分销")]
        public bool? SalePublish 
        { 
            get{return _SalePublish;}            set{                SetProperty(ref _SalePublish, value);                }
        }

        private string _ShortCode;
        
        
        /// <summary>
        /// 短码
        /// </summary>
        [SugarColumn(ColumnName = "ShortCode",Length=50,IsNullable = true,ColumnDescription = "短码")]
        [Display(Name = "短码")]
        public string ShortCode 
        { 
            get{return _ShortCode;}            set{                SetProperty(ref _ShortCode, value);                }
        }

        private int? _SourceType;
        
        
        /// <summary>
        /// 产品来源
        /// </summary>
        [SugarColumn(ColumnName = "SourceType",IsNullable = true,ColumnDescription = "产品来源")]
        [Display(Name = "产品来源")]
        public int? SourceType 
        { 
            get{return _SourceType;}            set{                SetProperty(ref _SourceType, value);                }
        }

        private string _BarCode;
        
        
        /// <summary>
        /// 条码
        /// </summary>
        [SugarColumn(ColumnName = "BarCode",Length=50,IsNullable = true,ColumnDescription = "条码")]
        [Display(Name = "条码")]
        public string BarCode 
        { 
            get{return _BarCode;}            set{                SetProperty(ref _BarCode, value);                }
        }

        private decimal? _Standard_Price;
        
        
        /// <summary>
        /// 标准价
        /// </summary>
        [SugarColumn(ColumnName = "Standard_Price",IsNullable = true,ColumnDescription = "标准价")]
        [Display(Name = "标准价")]
        public decimal? Standard_Price 
        { 
            get{return _Standard_Price;}            set{                SetProperty(ref _Standard_Price, value);                }
        }

        private decimal? _Discount_price;
        
        
        /// <summary>
        /// 折扣价格
        /// </summary>
        [SugarColumn(ColumnName = "Discount_price",IsNullable = true,ColumnDescription = "折扣价格")]
        [Display(Name = "折扣价格")]
        public decimal? Discount_price 
        { 
            get{return _Discount_price;}            set{                SetProperty(ref _Discount_price, value);                }
        }

        private decimal? _Market_price;
        
        
        /// <summary>
        /// 市场零售价
        /// </summary>
        [SugarColumn(ColumnName = "Market_price",IsNullable = true,ColumnDescription = "市场零售价")]
        [Display(Name = "市场零售价")]
        public decimal? Market_price 
        { 
            get{return _Market_price;}            set{                SetProperty(ref _Market_price, value);                }
        }

        private decimal? _Wholesale_Price;
        
        
        /// <summary>
        /// 批发价格
        /// </summary>
        [SugarColumn(ColumnName = "Wholesale_Price",IsNullable = true,ColumnDescription = "批发价格")]
        [Display(Name = "批发价格")]
        public decimal? Wholesale_Price 
        { 
            get{return _Wholesale_Price;}            set{                SetProperty(ref _Wholesale_Price, value);                }
        }

        private decimal? _Transfer_price;
        
        
        /// <summary>
        /// 调拨价格
        /// </summary>
        [SugarColumn(ColumnName = "Transfer_price",IsNullable = true,ColumnDescription = "调拨价格")]
        [Display(Name = "调拨价格")]
        public decimal? Transfer_price 
        { 
            get{return _Transfer_price;}            set{                SetProperty(ref _Transfer_price, value);                }
        }

        private decimal? _Weight;
        
        
        /// <summary>
        /// 重量（千克）
        /// </summary>
        [SugarColumn(ColumnName = "Weight",Length=10,IsNullable = true,ColumnDescription = "重量（千克）")]
        [Display(Name = "重量（千克）")]
        public decimal? Weight 
        { 
            get{return _Weight;}            set{                SetProperty(ref _Weight, value);                }
        }

        private long? _BOM_ID;
        
        
        /// <summary>
        /// 标准配方
        /// </summary>
        [SugarColumn(ColumnName = "BOM_ID",IsNullable = true,ColumnDescription = "标准配方")]
        [Display(Name = "标准配方")]
        public long? BOM_ID 
        { 
            get{return _BOM_ID;}            set{                SetProperty(ref _BOM_ID, value);                }
        }





  
        

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

