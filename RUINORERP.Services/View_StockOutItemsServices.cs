
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:10:35
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
    /// 其它出库统计
    /// </summary>
    public partial class View_StockOutItemsServices : BaseServices<View_StockOutItems>, IView_StockOutItemsServices
    {
        IMapper _mapper;
        public View_StockOutItemsServices(IMapper mapper, IBaseRepository<View_StockOutItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}