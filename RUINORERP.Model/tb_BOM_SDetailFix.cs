
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 11:23:51
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
    /// 标准物料表BOM明细-要适当冗余
    /// </summary>
    public partial class tb_BOM_SDetail: BaseEntity, ICloneable
    {



     

        #region 扩展属性
         
         [SugarColumn(IsIgnore = true)]
         [Browsable(false)]
         [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
         public virtual View_ProdDetail view_ProdDetail { get; set; }

        #endregion


 
    }
}

