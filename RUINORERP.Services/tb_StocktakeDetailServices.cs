
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2025 21:05:41
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
    /// 盘点明细表
    /// </summary>
    public partial class tb_StocktakeDetailServices : BaseServices<tb_StocktakeDetail>, Itb_StocktakeDetailServices
    {
        IMapper _mapper;
        public tb_StocktakeDetailServices(IMapper mapper, IBaseRepository<tb_StocktakeDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}