
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/25/2025 18:51:45
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
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_PreReceivedPayment: BaseEntity, ICloneable
    {
        #region 扩展属性

       
        /// <summary>
        /// 来自核销记录表中的实际核销金额
        /// </summary>
        [SugarColumn(IsIgnore = true,ColumnDescription = "核销金额")]
        public virtual decimal SettledLocalAmount { get; set; }

        #endregion
    }
}

