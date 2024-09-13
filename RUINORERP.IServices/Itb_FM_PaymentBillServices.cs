
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:43
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
    /// 付款单 中有两种情况，1）如果有应收款，可以抵扣而少付款，如果有预付款也可以抵扣。
    /// </summary>
    public partial interface Itb_FM_PaymentBillServices : IBaseServices<tb_FM_PaymentBill>
    {
      
    }
}