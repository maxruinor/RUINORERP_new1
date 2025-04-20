
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 18:12:09
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
    /// 收款付款记录表-记录所有资金流动一批订单可分账户分批付 记录真实资金流动，用户需确保其 与银行流水一致
    /// </summary>
    public partial interface Itb_FM_PaymentRecordServices : IBaseServices<tb_FM_PaymentRecord>
    {
      
    }
}