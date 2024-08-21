
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/03/2023 23:30:58
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
    /// 流程图线
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FlowchartLines")]
    public partial class tb_FlowchartLinesQueryDto
    {
        public tb_FlowchartLinesQueryDto()
        {

        }

    
     
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
     
        /// <summary>
        /// 流程图编号
        /// </summary>
        public string FlowchartNo { get; set; }
     
        /// <summary>
        /// 大小
        /// </summary>
        public string PointToString1 { get; set; }
     
        /// <summary>
        /// 位置
        /// </summary>
        public string PointToString2 { get; set; }










       
    }
}
