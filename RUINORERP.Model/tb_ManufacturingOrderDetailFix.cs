
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 11:23:53
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 制令单的原料明细表 明细对应的是一个树，结构同BOM，先把BOM搞好再来实现这里的细节
    /// </summary>
    public partial class tb_ManufacturingOrderDetail : BaseEntity, ICloneable
    {



        #region 扩展属性

        /*手动建立了关联,数据库中没有创建强关联关系*/

        /// <summary>
        /// 物料明细所在配方
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(BOM_ID))]
        public virtual tb_BOM_S tb_bom_s { get; set; }


        #endregion


    }
}

