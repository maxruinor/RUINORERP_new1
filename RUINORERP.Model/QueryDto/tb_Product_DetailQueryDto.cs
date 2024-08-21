
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/13/2023 17:34:50
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;


namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 产品详细表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Product_Detail")]
    public partial class tb_Product_DetailQueryDto
    {
        public tb_Product_DetailQueryDto()
        {

        }

    
     
        /// <summary>
        /// 产品详情
        /// </summary>
        public int Prod_Detail_ID { get; set; }
     
        /// <summary>
        /// 属性值关系
        /// </summary>
        public int? RAR_ID { get; set; }
     
        /// <summary>
        /// SKU码
        /// </summary>
        public string SKU { get; set; }
     
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
     
        /// <summary>
        /// 产品图片
        /// </summary>
        public string ImagesPath { get; set; }
     
        /// <summary>
        /// 产品图片
        /// </summary>
        public byte[] Images { get; set; }
     
        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight { get; set; }
     
        /// <summary>
        /// 采购价格
        /// </summary>
        public decimal? Purchase_price { get; set; }
     
        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal? Sales_price { get; set; }
     
        /// <summary>
        /// 调拨价格
        /// </summary>
        public decimal? Transfer_price { get; set; }
     
        /// <summary>
        /// 成本价格
        /// </summary>
        public decimal? Cost_price { get; set; }
     
        /// <summary>
        /// 市场价格
        /// </summary>
        public decimal? Market_price { get; set; }
     
        /// <summary>
        /// 折扣价格
        /// </summary>
        public decimal? Discount_price { get; set; }
     
        /// <summary>
        /// 产品图片
        /// </summary>
        public byte[] Image { get; set; }
     
        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; }
     
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? Is_enabled { get; set; }
     
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool? Is_available { get; set; }
     
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Created_at { get; set; }
     
        /// <summary>
        /// 创建人
        /// </summary>
        public int? Created_by { get; set; }
     
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Modified_at { get; set; }
     
        /// <summary>
        /// 修改人
        /// </summary>
        public int? Modified_by { get; set; }










       
    }
}
