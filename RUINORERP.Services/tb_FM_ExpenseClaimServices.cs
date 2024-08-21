
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:12
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
    /// 费用报销单
    /// </summary>
    public partial class tb_FM_ExpenseClaimServices : BaseServices<tb_FM_ExpenseClaim>, Itb_FM_ExpenseClaimServices
    {
        IMapper _mapper;
        public tb_FM_ExpenseClaimServices(IMapper mapper, IBaseRepository<tb_FM_ExpenseClaim> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}