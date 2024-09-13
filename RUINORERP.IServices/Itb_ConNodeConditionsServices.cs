
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:30
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.IServices.BASE;
using System.Threading.Tasks;
using RUINORERP.Model;

namespace RUINORERP.IServices
{
    /// <summary>
    /// 流程步骤 为转移条件集合，Field为条件左参数，Operator为操作操作符如果值类型为String则表达式只能为==或者!=，Value为表达式值
    /// </summary>
    public partial interface Itb_ConNodeConditionsServices : IBaseServices<tb_ConNodeConditions>
    {
      
    }
}