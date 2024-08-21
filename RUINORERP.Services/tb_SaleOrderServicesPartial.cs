
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/29/2023 15:06:07
// **************************************
using AutoMapper;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Services.BASE;
using System.Threading.Tasks;
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model.QueryDto;


namespace RUINORERP.Services
{
    /// <summary>
    /// 销售订单
    /// </summary>
    public partial class tb_SaleOrderServices : BaseServices<tb_SaleOrder>, Itb_SaleOrderServices
    {
        public async Task<List<tb_SaleOrder>> QueryAsync(tb_SaleOrderQueryDto QueryCriteria, Pagination pagination)
        {
            var list = await base.QueryAsync();
            return list;
        }
    }
}