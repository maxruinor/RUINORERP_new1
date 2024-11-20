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
    /// 产品其他部分，用于动态合并，体现于明细中表格中,程序中来控制显示哪些字段？
    /// </summary>
    public class InventoryInfo
    {

        public InventoryInfo()
        {

        }
        /// <summary>
        /// 实际库存
        /// </summary>


        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "Quantity", IsNullable = true, ColumnDescription = "实际库存")]
        [Display(Name = "实际库存")]
        public int? Quantity
        { get; set; }
  

        /// <summary>
        /// 预警值
        /// </summary>

        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "Alert_Quantity", IsNullable = true, ColumnDescription = "预警值")]
        [Display(Name = "预警值")]
        public int? Alert_Quantity
        { get; set; }

 


        /// <summary>
        /// 在途库存
        /// </summary>


        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "On_the_way_Qty", IsNullable = true, ColumnDescription = "在途库存")]
        [Display(Name = "在途库存")]
        public int? On_the_way_Qty
        { get; set; }

 


        /// <summary>
        /// 拟销售量
        /// </summary>

        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "Sale_Qty", IsNullable = true, ColumnDescription = "拟销售量")]
        [Display(Name = "拟销售量")]
        public int? Sale_Qty
        { get; set; }

       


        /// <summary>
        /// 在制数量
        /// </summary>


        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "MakingQty", IsNullable = true, ColumnDescription = "在制数量")]
        public int? MakingQty
        { get; set; }
 


        /// <summary>
        /// 未发数量
        /// </summary>


        [SugarColumn(ColumnDataType = "int", SqlParameterDbType = "Int32", ColumnName = "NotOutQty", IsNullable = true, ColumnDescription = "未发数量")]
        public int? NotOutQty
        { get; set; }

    }

}
