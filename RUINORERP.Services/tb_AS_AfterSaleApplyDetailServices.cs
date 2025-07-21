
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/19/2025 17:12:34
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
    /// 售后申请单明细
    /// </summary>
    public partial class tb_AS_AfterSaleApplyDetailServices : BaseServices<tb_AS_AfterSaleApplyDetail>, Itb_AS_AfterSaleApplyDetailServices
    {
        IMapper _mapper;
        public tb_AS_AfterSaleApplyDetailServices(IMapper mapper, IBaseRepository<tb_AS_AfterSaleApplyDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}