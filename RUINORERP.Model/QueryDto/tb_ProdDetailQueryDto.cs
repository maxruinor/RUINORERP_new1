
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:08
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

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 产品详细表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProdDetail")]
    public partial class tb_ProdDetailQueryDto:BaseEntityDto
    {
        public tb_ProdDetailQueryDto()
        {

        }

    
     

        private long? _ProdBaseID;
        /// <summary>
        /// 货品主信息
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdBaseID",ColDesc = "货品主信息")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ProdBaseID",IsNullable = true,ColumnDescription = "货品主信息" )]
        [FKRelationAttribute("tb_Prod","ProdBaseID")]
        public long? ProdBaseID 
        { 
            get{return _ProdBaseID;}
            set{SetProperty(ref _ProdBaseID, value);}
        }
     

        private long? _BOM_ID;
        /// <summary>
        /// 标准配方
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "标准配方")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "BOM_ID",IsNullable = true,ColumnDescription = "标准配方" )]
        [FKRelationAttribute("tb_BOM_S","BOM_ID")]
        public long? BOM_ID 
        { 
            get{return _BOM_ID;}
            set{SetProperty(ref _BOM_ID, value);}
        }
     

        private string _SKU;
        /// <summary>
        /// SKU码
        /// </summary>
        [AdvQueryAttribute(ColName = "SKU",ColDesc = "SKU码")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "SKU",Length=80,IsNullable = true,ColumnDescription = "SKU码" )]
        public string SKU 
        { 
            get{return _SKU;}
            set{SetProperty(ref _SKU, value);}
        }
     

        private string _BarCode;
        /// <summary>
        /// 条码
        /// </summary>
        [AdvQueryAttribute(ColName = "BarCode",ColDesc = "条码")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "BarCode",Length=50,IsNullable = true,ColumnDescription = "条码" )]
        public string BarCode 
        { 
            get{return _BarCode;}
            set{SetProperty(ref _BarCode, value);}
        }
     

        private string _ImagesPath;
        /// <summary>
        /// 产品图片
        /// </summary>
        [AdvQueryAttribute(ColName = "ImagesPath",ColDesc = "产品图片")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "ImagesPath",Length=2000,IsNullable = true,ColumnDescription = "产品图片" )]
        public string ImagesPath 
        { 
            get{return _ImagesPath;}
            set{SetProperty(ref _ImagesPath, value);}
        }
     

        private byte[] _Images;
        /// <summary>
        /// 产品图片
        /// </summary>
        [AdvQueryAttribute(ColName = "Images",ColDesc = "产品图片")]
        [SugarColumn(ColumnDataType = "image",SqlParameterDbType ="Binary",ColumnName = "Images",Length=2147483647,IsNullable = true,ColumnDescription = "产品图片" )]
        public byte[] Images 
        { 
            get{return _Images;}
            set{SetProperty(ref _Images, value);}
        }
     

        private decimal? _Weight;
        /// <summary>
        /// 重量（千克）
        /// </summary>
        [AdvQueryAttribute(ColName = "Weight",ColDesc = "重量（千克）")]
        [SugarColumn(ColumnDataType = "decimal",SqlParameterDbType ="Decimal",ColumnName = "Weight",IsNullable = true,ColumnDescription = "重量（千克）" )]
        public decimal? Weight 
        { 
            get{return _Weight;}
            set{SetProperty(ref _Weight, value);}
        }
     

        private decimal? _Standard_Price;
        /// <summary>
        /// 标准价
        /// </summary>
        [AdvQueryAttribute(ColName = "Standard_Price",ColDesc = "标准价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Standard_Price",IsNullable = true,ColumnDescription = "标准价" )]
        public decimal? Standard_Price 
        { 
            get{return _Standard_Price;}
            set{SetProperty(ref _Standard_Price, value);}
        }
     

        private decimal? _Transfer_Price;
        /// <summary>
        /// 调拨价格
        /// </summary>
        [AdvQueryAttribute(ColName = "Transfer_Price",ColDesc = "调拨价格")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Transfer_Price",IsNullable = true,ColumnDescription = "调拨价格" )]
        public decimal? Transfer_Price 
        { 
            get{return _Transfer_Price;}
            set{SetProperty(ref _Transfer_Price, value);}
        }
     

        private decimal? _Wholesale_Price;
        /// <summary>
        /// 批发价格
        /// </summary>
        [AdvQueryAttribute(ColName = "Wholesale_Price",ColDesc = "批发价格")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Wholesale_Price",IsNullable = true,ColumnDescription = "批发价格" )]
        public decimal? Wholesale_Price 
        { 
            get{return _Wholesale_Price;}
            set{SetProperty(ref _Wholesale_Price, value);}
        }
     

        private decimal? _Market_Price;
        /// <summary>
        /// 市场零售价
        /// </summary>
        [AdvQueryAttribute(ColName = "Market_Price",ColDesc = "市场零售价")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Market_Price",IsNullable = true,ColumnDescription = "市场零售价" )]
        public decimal? Market_Price 
        { 
            get{return _Market_Price;}
            set{SetProperty(ref _Market_Price, value);}
        }
     

        private decimal? _Discount_Price;
        /// <summary>
        /// 折扣价格
        /// </summary>
        [AdvQueryAttribute(ColName = "Discount_Price",ColDesc = "折扣价格")]
        [SugarColumn(ColumnDataType = "money",SqlParameterDbType ="Decimal",ColumnName = "Discount_Price",IsNullable = true,ColumnDescription = "折扣价格" )]
        public decimal? Discount_Price 
        { 
            get{return _Discount_Price;}
            set{SetProperty(ref _Discount_Price, value);}
        }
     

        private byte[] _Image;
        /// <summary>
        /// 产品图片
        /// </summary>
        [AdvQueryAttribute(ColName = "Image",ColDesc = "产品图片")]
        [SugarColumn(ColumnDataType = "image",SqlParameterDbType ="Binary",ColumnName = "Image",Length=2147483647,IsNullable = true,ColumnDescription = "产品图片" )]
        public byte[] Image 
        { 
            get{return _Image;}
            set{SetProperty(ref _Image, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=255,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }
     

        private bool _SalePublish= true;
        /// <summary>
        /// 参与分销
        /// </summary>
        [AdvQueryAttribute(ColName = "SalePublish",ColDesc = "参与分销")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "SalePublish",IsNullable = false,ColumnDescription = "参与分销" )]
        public bool SalePublish 
        { 
            get{return _SalePublish;}
            set{SetProperty(ref _SalePublish, value);}
        }
     

        private bool _Is_enabled= true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled",ColDesc = "是否启用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_enabled",IsNullable = false,ColumnDescription = "是否启用" )]
        public bool Is_enabled 
        { 
            get{return _Is_enabled;}
            set{SetProperty(ref _Is_enabled, value);}
        }
     

        private bool _Is_available= true;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_available",ColDesc = "是否可用")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "Is_available",IsNullable = false,ColumnDescription = "是否可用" )]
        public bool Is_available 
        { 
            get{return _Is_available;}
            set{SetProperty(ref _Is_available, value);}
        }
     

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
            set{SetProperty(ref _Created_at, value);}
        }
     

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by 
        { 
            get{return _Created_by;}
            set{SetProperty(ref _Created_by, value);}
        }
     

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
            set{SetProperty(ref _Modified_at, value);}
        }
     

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
            set{SetProperty(ref _Modified_by, value);}
        }
     

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "isdeleted",IsNullable = false,ColumnDescription = "逻辑删除" )]
        public bool isdeleted 
        { 
            get{return _isdeleted;}
            set{SetProperty(ref _isdeleted, value);}
        }
     

        private int? _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        [AdvQueryAttribute(ColName = "DataStatus",ColDesc = "数据状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "DataStatus",IsNullable = true,ColumnDescription = "数据状态" )]
        public int? DataStatus 
        { 
            get{return _DataStatus;}
            set{SetProperty(ref _DataStatus, value);}
        }


       
    }
}



