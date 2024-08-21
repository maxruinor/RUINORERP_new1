
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:06
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
    /// 交易方式设定，后面扩展有关账期 账龄分析的字段,暂时保存一个主子关系方便后面扩展
    /// </summary>
    public partial interface Itb_PaymentMethodDetailServices : IBaseServices<tb_PaymentMethodDetail>
    {
      
    }
}