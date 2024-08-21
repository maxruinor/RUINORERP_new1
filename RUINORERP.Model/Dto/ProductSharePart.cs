using RUINORERP.Global.CustomAttribute;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace RUINORERP.Model.Dto
{
    /// <summary>
    /// 产品共用部分，用于动态合并，体现于明细中表格中
    /// </summary>
    public class ProductSharePart
    {

        public ProductSharePart()
        {

        }

        /// <summary>
        /// 品号
        /// </summary>
        [SugarColumn(ColumnName = "ProductNo", Length = 40, IsNullable = false, ColumnDescription = "品号")]
        public string ProductNo { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [SugarColumn(ColumnName = "CNName", Length = 255, IsNullable = false, ColumnDescription = "品名")]
        [ReadOnly(true)]
        public string CNName { get; set; }

        /// <summary>
        /// SKU码
        /// </summary>
        [SugarColumn(ColumnName = "SKU", Length = 50, IsNullable = true, ColumnDescription = "SKU码")]
        public string SKU { get; set; }

        /// <summary>
        /// 助记码
        /// </summary>
        [SugarColumn(ColumnName = "ShortCode", Length = 50, IsNullable = true, ColumnDescription = "助记码")]
        public string ShortCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        [SugarColumn(ColumnName = "BarCode", Length = 50, IsNullable = true, ColumnDescription = "条码")]
        public string BarCode { get; set; }


        /// <summary>
        /// 属性
        /// </summary>
        [SugarColumn(ColumnName = "prop", Length = 500, IsNullable = true, ColumnDescription = "属性")]
        [ReadOnly(true)]
        public string prop { get; set; }
        
        /// <summary>
        /// 型号
        /// </summary>
        [SugarColumn(ColumnName = "Model", Length = 50, IsNullable = true, ColumnDescription = "型号")]
        public string Model
        {
            get;
            set;
        }
        /// <summary>
        /// 规格
        /// </summary>
        [SugarColumn(ColumnName = "Specifications", Length = 200, IsNullable = true, ColumnDescription = "规格")]
        public string Specifications { get; set; }


        /// <summary>
        /// 产品类型
        /// </summary>
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Type_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "产品类型")]
        [ReadOnly(true)]
        [FKRelationAttribute("tb_ProductType", "Type_ID")]
        public long? Type_ID { get; set; }
         

        /// <summary>
        /// 单位
        /// </summary>
        [SugarColumn(ColumnName = "Unit_ID", IsNullable = true, ColumnDescription = "单位")]
        [ReadOnly(true)]
        [FKRelationAttribute("tb_Unit", "Unit_ID")]
        public long? Unit_ID { get; set; }



        /// <summary>
        /// 默认库位
        /// </summary>
        [SugarColumn(ColumnName = "Location_ID", IsNullable = true, ColumnDescription = "默认库位")]
        [FKRelationAttribute("tb_Location", "Location_ID")]
        public long? Location_ID { get; set; }


        /// <summary>
        /// 默认货架
        /// </summary>
        [SugarColumn(ColumnName = "Rack_ID", IsNullable = true, ColumnDescription = "默认货架")]
        [FKRelationAttribute("tb_StorageRack", "Rack_ID")]
        public long? Rack_ID { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        [SugarColumn(ColumnName = "Brand", Length = 50, IsNullable = true, ColumnDescription = "品牌")]
        [ReadOnly(true)]
        public string Brand { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>
        [SugarColumn(ColumnName = "Image", Length = 2147483647, IsNullable = true, ColumnDescription = "产品图片")]
        [ReadOnly(true)]
        public byte[] Image { get; set; }


        /// <summary>
        /// 成本价格
        /// </summary>
        [SugarColumn(ColumnDataType = "money", ColumnName = "Inv_Cost", IsNullable = true, ColumnDescription = "成本价格")]
        [ReadOnly(true)]
        public decimal? Inv_Cost { get; set; }

        /// <summary>
        /// 销售价格
        /// </summary>
        [SugarColumn(ColumnDataType = "money", ColumnName = "Standard_Price", IsNullable = true, ColumnDescription = "销售价格")]
        [ReadOnly(true)]
        public decimal? Standard_Price { get; set; }

    }

}
