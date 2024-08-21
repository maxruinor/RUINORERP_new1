
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:15
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
    /// 费用报销单明细
    /// </summary>
    public partial class tb_FM_ExpenseClaimDetailServices : BaseServices<tb_FM_ExpenseClaimDetail>, Itb_FM_ExpenseClaimDetailServices
    {
        IMapper _mapper;
        public tb_FM_ExpenseClaimDetailServices(IMapper mapper, IBaseRepository<tb_FM_ExpenseClaimDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}