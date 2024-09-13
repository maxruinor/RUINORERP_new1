
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:31
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


namespace RUINORERP.Services
{
    /// <summary>
    /// 成本单价表 参考天思货品基本资料中的价格部分
    /// </summary>
    public partial class tb_CostUnitPriceServices : BaseServices<tb_CostUnitPrice>, Itb_CostUnitPriceServices
    {
        IMapper _mapper;
        public tb_CostUnitPriceServices(IMapper mapper, IBaseRepository<tb_CostUnitPrice> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}