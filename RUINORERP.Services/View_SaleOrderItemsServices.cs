
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/18/2024 22:55:50
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
    /// 销售订单统计分析
    /// </summary>
    public partial class View_SaleOrderItemsServices : BaseServices<View_SaleOrderItems>, IView_SaleOrderItemsServices
    {
        IMapper _mapper;
        public View_SaleOrderItemsServices(IMapper mapper, IBaseRepository<View_SaleOrderItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}