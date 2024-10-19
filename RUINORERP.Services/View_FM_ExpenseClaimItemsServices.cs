
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 01:04:32
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
    /// 费用报销统计分析
    /// </summary>
    public partial class View_FM_ExpenseClaimItemsServices : BaseServices<View_FM_ExpenseClaimItems>, IView_FM_ExpenseClaimItemsServices
    {
        IMapper _mapper;
        public View_FM_ExpenseClaimItemsServices(IMapper mapper, IBaseRepository<View_FM_ExpenseClaimItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}