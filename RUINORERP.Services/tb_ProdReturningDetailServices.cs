
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2024 13:38:39
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
    /// 归还单明细
    /// </summary>
    public partial class tb_ProdReturningDetailServices : BaseServices<tb_ProdReturningDetail>, Itb_ProdReturningDetailServices
    {
        IMapper _mapper;
        public tb_ProdReturningDetailServices(IMapper mapper, IBaseRepository<tb_ProdReturningDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}