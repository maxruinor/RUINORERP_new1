
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:56:55
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
    /// 币别换算表-暂时不使用如果ERP系统需要支持多币种，通常需要在所有涉及外币的业务单据和凭证中添加外币和本币两个字段来保存对应的金额
    /// </summary>
    public partial interface Itb_CurrencyExchangeRateServices : IBaseServices<tb_CurrencyExchangeRate>
    {
      
    }
}