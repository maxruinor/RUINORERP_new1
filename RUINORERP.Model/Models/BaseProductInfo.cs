using RUINORERP.Global.CustomAttribute;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace RUINORERP.Model.Dto
{
    /// <summary>
    /// 产品共用部分，用于动态合并，体现于明细中表格中
    /// 只有基本产品信息
    /// </summary>
    public class BaseProductInfo
    {

        public BaseProductInfo()
        {

        }
 

        /// <summary>
        /// 产品
        /// </summary>
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "ProdDetailID", IsNullable = true, ColumnDescription = "产品", IsPrimaryKey = true)]
        public long? ProdDetailID { get; set; }



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

        // /// <summary>
        ///// 属性
        ///// </summary>
        //[SugarColumn(ColumnName = "prop", Length = 500, IsNullable = true, ColumnDescription = "属性")]
        //[ReadOnly(true)]
        //public string prop { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [SugarColumn(ColumnName = "Unit_ID", IsNullable = true, ColumnDescription = "单位")]
        [ReadOnly(true)]
        [FKRelationAttribute("tb_Unit", "Unit_ID")]
        public long? Unit_ID { get; set; }

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


 





    }

}
