
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/07/2025 21:28:02
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
    /// 制令单明细统计
    /// </summary>
    public partial class View_ManufacturingOrderItemsServices : BaseServices<View_ManufacturingOrderItems>, IView_ManufacturingOrderItemsServices
    {
        IMapper _mapper;
        public View_ManufacturingOrderItemsServices(IMapper mapper, IBaseRepository<View_ManufacturingOrderItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}