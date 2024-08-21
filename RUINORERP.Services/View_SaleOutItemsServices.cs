
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/18/2024 22:55:51
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
    /// 销售出库统计分析
    /// </summary>
    public partial class View_SaleOutItemsServices : BaseServices<View_SaleOutItems>, IView_SaleOutItemsServices
    {
        IMapper _mapper;
        public View_SaleOutItemsServices(IMapper mapper, IBaseRepository<View_SaleOutItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}