
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/13/2023 17:34:48
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
    /// 产品基本信息表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Product_Base")]
    public partial class tb_Product_BaseQueryDto
    {
        public tb_Product_BaseQueryDto()
        {

        }

    
     
        /// <summary>
        /// 
        /// </summary>
        public int Prod_Base_ID { get; set; }
     
        /// <summary>
        /// 品号
        /// </summary>
        public string ProductNo { get; set; }
     
        /// <summary>
        /// 品名
        /// </summary>
        public string Name { get; set; }
     
        /// <summary>
        /// 助记码
        /// </summary>
        public string ShortCode { get; set; }
     
        /// <summary>
        /// 规格
        /// </summary>
        public string Specifications { get; set; }
     
        /// <summary>
        /// 供应商
        /// </summary>
        public int? CustomerVendor_ID { get; set; }
     
        /// <summary>
        /// 库位
        /// </summary>
        public int? Location_ID { get; set; }
     
        /// <summary>
        /// 产品来源
        /// </summary>
        public string SourceType { get; set; }
     
        /// <summary>
        /// 产品类型
        /// </summary>
        public int? Type_ID { get; set; }
     
        /// <summary>
        /// 部门
        /// </summary>
        public int? DepartmentID { get; set; }
     
        /// <summary>
        /// 单位
        /// </summary>
        public int Unit_ID { get; set; }
     
        /// <summary>
        /// 类别
        /// </summary>
        public int? Category_ID { get; set; }
     
        /// <summary>
        /// 
        /// </summary>
        public int? PropertyType_ID { get; set; }
     
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
     
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? Is_enabled { get; set; }
     
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool? Is_available { get; set; }
     
        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; }
     
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
