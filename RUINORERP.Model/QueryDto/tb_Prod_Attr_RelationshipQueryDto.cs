
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/13/2023 17:34:45
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
    /// 
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Prod_Attr_Relationship")]
    public partial class tb_Prod_Attr_RelationshipQueryDto
    {
        public tb_Prod_Attr_RelationshipQueryDto()
        {

        }

    
     
        /// <summary>
        /// 关系
        /// </summary>
        public int RAR_ID { get; set; }
     
        /// <summary>
        /// 
        /// </summary>
        public int? Property_ID { get; set; }
     
        /// <summary>
        /// 属性值ID
        /// </summary>
        public int? PropertyValueID { get; set; }
     
        /// <summary>
        /// 
        /// </summary>
        public int? Prod_Base_ID { get; set; }










       
    }
}
