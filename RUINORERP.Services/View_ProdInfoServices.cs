
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/02/2025 15:03:21
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
    /// 产品信息视图
    /// </summary>
    public partial class View_ProdInfoServices : BaseServices<View_ProdInfo>, IView_ProdInfoServices
    {
        IMapper _mapper;
        public View_ProdInfoServices(IMapper mapper, IBaseRepository<View_ProdInfo> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}