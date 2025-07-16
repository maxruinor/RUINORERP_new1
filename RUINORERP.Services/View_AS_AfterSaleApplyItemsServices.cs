
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/16/2025 14:14:17
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
    /// 售后申请明细统计
    /// </summary>
    public partial class View_AS_AfterSaleApplyItemsServices : BaseServices<View_AS_AfterSaleApplyItems>, IView_AS_AfterSaleApplyItemsServices
    {
        IMapper _mapper;
        public View_AS_AfterSaleApplyItemsServices(IMapper mapper, IBaseRepository<View_AS_AfterSaleApplyItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}