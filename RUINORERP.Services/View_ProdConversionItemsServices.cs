
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:29:23
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
    /// 转换明细统计
    /// </summary>
    public partial class View_ProdConversionItemsServices : BaseServices<View_ProdConversionItems>, IView_ProdConversionItemsServices
    {
        IMapper _mapper;
        public View_ProdConversionItemsServices(IMapper mapper, IBaseRepository<View_ProdConversionItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}