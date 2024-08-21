
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/16/2024 11:43:13
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
    /// 销售退货统计分析
    /// </summary>
    public partial class View_SaleOutReItemsServices : BaseServices<View_SaleOutReItems>, IView_SaleOutReItemsServices
    {
        IMapper _mapper;
        public View_SaleOutReItemsServices(IMapper mapper, IBaseRepository<View_SaleOutReItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}