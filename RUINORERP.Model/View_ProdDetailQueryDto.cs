
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/14/2024 18:30:34
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
    /// 产品详情视图
    /// </summary>
    [Serializable()]
    [SugarTable("View_ProdDetail")]
    public class View_ProdDetailQueryDto:BaseEntity, ICloneable
    {
        public View_ProdDetailQueryDto()
        {

        }

    
        private string _SKU;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "SKU",Length=80,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string SKU 
        { 
            get{return _SKU;}            set{                SetProperty(ref _SKU, value);                }
        }

        private long _ProdDetailID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ProdDetailID",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public long ProdDetailID 
        { 
            get{return _ProdDetailID;}            set{                SetProperty(ref _ProdDetailID, value);                }
        }

        private string _CNName;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CNName",Length=255,IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public string CNName 
        { 
            get{return _CNName;}            set{                SetProperty(ref _CNName, value);                }
        }

        private string _Specifications;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Specifications",Length=1000,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string Specifications 
        { 
            get{return _Specifications;}            set{                SetProperty(ref _Specifications, value);                }
        }

        private int? _Quantity;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Quantity",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? Quantity 
        { 
            get{return _Quantity;}            set{                SetProperty(ref _Quantity, value);                }
        }

        private string _prop;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "prop",Length=-1,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string prop 
        { 
            get{return _prop;}            set{                SetProperty(ref _prop, value);                }
        }

        private string _ProductNo;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ProductNo",Length=40,IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public string ProductNo 
        { 
            get{return _ProductNo;}            set{                SetProperty(ref _ProductNo, value);                }
        }

        private long? _Unit_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Unit_ID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Unit_ID 
        { 
            get{return _Unit_ID;}            set{                SetProperty(ref _Unit_ID, value);                }
        }

        private string _Model;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Model",Length=50,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string Model 
        { 
            get{return _Model;}            set{                SetProperty(ref _Model, value);                }
        }

        private long? _Category_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Category_ID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Category_ID 
        { 
            get{return _Category_ID;}            set{                SetProperty(ref _Category_ID, value);                }
        }

        private long? _CustomerVendor_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CustomerVendor_ID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}            set{                SetProperty(ref _CustomerVendor_ID, value);                }
        }

        private long? _DepartmentID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "DepartmentID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? DepartmentID 
        { 
            get{return _DepartmentID;}            set{                SetProperty(ref _DepartmentID, value);                }
        }

        private string _ENName;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ENName",Length=255,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string ENName 
        { 
            get{return _ENName;}            set{                SetProperty(ref _ENName, value);                }
        }

        private string _Brand;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Brand",Length=50,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string Brand 
        { 
            get{return _Brand;}            set{                SetProperty(ref _Brand, value);                }
        }

        private byte[] _Images;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Images",Length=2147483647,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public byte[] Images 
        { 
            get{return _Images;}            set{                SetProperty(ref _Images, value);                }
        }

        private long? _Location_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Location_ID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Location_ID 
        { 
            get{return _Location_ID;}            set{                SetProperty(ref _Location_ID, value);                }
        }

        private long? _Rack_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Rack_ID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Rack_ID 
        { 
            get{return _Rack_ID;}            set{                SetProperty(ref _Rack_ID, value);                }
        }

        private int? _On_the_way_Qty;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "On_the_way_Qty",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? On_the_way_Qty 
        { 
            get{return _On_the_way_Qty;}            set{                SetProperty(ref _On_the_way_Qty, value);                }
        }

        private int? _Sale_Qty;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Sale_Qty",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? Sale_Qty 
        { 
            get{return _Sale_Qty;}            set{                SetProperty(ref _Sale_Qty, value);                }
        }

        private int? _Alert_Quantity;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Alert_Quantity",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? Alert_Quantity 
        { 
            get{return _Alert_Quantity;}            set{                SetProperty(ref _Alert_Quantity, value);                }
        }

        private int? _MakingQty;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "MakingQty",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? MakingQty 
        { 
            get{return _MakingQty;}            set{                SetProperty(ref _MakingQty, value);                }
        }

        private int? _NotOutQty;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "NotOutQty",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? NotOutQty 
        { 
            get{return _NotOutQty;}            set{                SetProperty(ref _NotOutQty, value);                }
        }

        private bool? _Is_available;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Is_available",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public bool? Is_available 
        { 
            get{return _Is_available;}            set{                SetProperty(ref _Is_available, value);                }
        }

        private bool? _Is_enabled;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Is_enabled",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public bool? Is_enabled 
        { 
            get{return _Is_enabled;}            set{                SetProperty(ref _Is_enabled, value);                }
        }

        private string _Notes;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Notes",Length=255,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string Notes 
        { 
            get{return _Notes;}            set{                SetProperty(ref _Notes, value);                }
        }

        private long _Type_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Type_ID",IsNullable = false,ColumnDescription = "")]
        [Display(Name = "")]
        public long Type_ID 
        { 
            get{return _Type_ID;}            set{                SetProperty(ref _Type_ID, value);                }
        }

        private bool? _SalePublish;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "SalePublish",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public bool? SalePublish 
        { 
            get{return _SalePublish;}            set{                SetProperty(ref _SalePublish, value);                }
        }

        private string _ShortCode;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ShortCode",Length=50,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string ShortCode 
        { 
            get{return _ShortCode;}            set{                SetProperty(ref _ShortCode, value);                }
        }

        private int? _SourceType;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "SourceType",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? SourceType 
        { 
            get{return _SourceType;}            set{                SetProperty(ref _SourceType, value);                }
        }

        private string _BarCode;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "BarCode",Length=50,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public string BarCode 
        { 
            get{return _BarCode;}            set{                SetProperty(ref _BarCode, value);                }
        }

        private decimal? _Inv_Cost;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Inv_Cost",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? Inv_Cost 
        { 
            get{return _Inv_Cost;}            set{                SetProperty(ref _Inv_Cost, value);                }
        }

        private decimal? _Standard_Price;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Standard_Price",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? Standard_Price 
        { 
            get{return _Standard_Price;}            set{                SetProperty(ref _Standard_Price, value);                }
        }

        private decimal? _Discount_price;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Discount_price",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? Discount_price 
        { 
            get{return _Discount_price;}            set{                SetProperty(ref _Discount_price, value);                }
        }

        private decimal? _Market_price;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Market_price",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? Market_price 
        { 
            get{return _Market_price;}            set{                SetProperty(ref _Market_price, value);                }
        }

        private decimal? _Wholesale_Price;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Wholesale_Price",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? Wholesale_Price 
        { 
            get{return _Wholesale_Price;}            set{                SetProperty(ref _Wholesale_Price, value);                }
        }

        private decimal? _Transfer_price;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Transfer_price",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? Transfer_price 
        { 
            get{return _Transfer_price;}            set{                SetProperty(ref _Transfer_price, value);                }
        }

        private decimal? _Weight;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Weight",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? Weight 
        { 
            get{return _Weight;}            set{                SetProperty(ref _Weight, value);                }
        }

        private long? _BOM_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "BOM_ID",IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
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

