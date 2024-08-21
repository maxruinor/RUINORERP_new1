
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/16/2023 10:55:43
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
    /// 产品类别表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_ProductCategories")]
    public partial class tb_ProductCategoriesQueryDto
    {
        public tb_ProductCategoriesQueryDto()
        {

        }

    
     
        /// <summary>
        /// 类别
        /// </summary>
        public int Category_ID { get; set; }
     
        /// <summary>
        /// 类别名称
        /// </summary>
        public string category_name { get; set; }
     
        /// <summary>
        /// 类别代码
        /// </summary>
        public string categoryCode { get; set; }
     
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Is_enabled { get; set; }
     
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
     
        /// <summary>
        /// 父类
        /// </summary>
        public int? parent_id { get; set; }
     
        /// <summary>
        /// 类目图
        /// </summary>
        public byte[] Images { get; set; }
     
        /// <summary>
        /// 备注
        /// </summary>
        public string Notes { get; set; }










       
    }
}
