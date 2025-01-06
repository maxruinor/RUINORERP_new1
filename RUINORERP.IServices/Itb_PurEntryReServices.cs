
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/25/2024 15:56:39
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
    /// 采购入库单 供应商接到采购订单后，向企业发货，用户在收到货物时，可以先检验，对合格品进行入库，也可以直接入库，形成采购入库单。为了保证清楚地记录进货情况，对进货的管理就很重要，而在我们的系统中，凭证、收付款是根据进货单自动一环扣一环地切制，故详细输入进货单资料后，存货的数量、成本会随着改变，收付账款也会跟着你的立账方式变化；凭证亦会随着“您是否立即产生凭证”变化。采购入库单可以由采购订单、借入单、在途物资单转入，也可以手动录入新增单据。
    /// </summary>
    public partial interface Itb_PurEntryReServices : IBaseServices<tb_PurEntryRe>
    {
      
    }
}