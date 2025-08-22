
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/25/2025 18:51:45
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using System.Linq;

namespace RUINORERP.Model
{
    /// <summary>
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_ReceivablePayableDetail : BaseEntity, ICloneable
    {
        #region 扩展属性



        /// <summary>
        /// 来源明细主键
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public long SourceItemRowID
        { get; set;}
       

        #endregion
    }
}

