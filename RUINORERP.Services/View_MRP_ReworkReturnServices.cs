
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/06/2025 18:55:24
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
    /// 返工退库统计
    /// </summary>
    public partial class View_MRP_ReworkReturnServices : BaseServices<View_MRP_ReworkReturn>, IView_MRP_ReworkReturnServices
    {
        IMapper _mapper;
        public View_MRP_ReworkReturnServices(IMapper mapper, IBaseRepository<View_MRP_ReworkReturn> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}