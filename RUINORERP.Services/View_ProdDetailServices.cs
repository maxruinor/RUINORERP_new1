
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/14/2024 18:30:40
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
    /// 产品详情视图
    /// </summary>
    public partial class View_ProdDetailServices : BaseServices<View_ProdDetail>, IView_ProdDetailServices
    {
        IMapper _mapper;
        public View_ProdDetailServices(IMapper mapper, IBaseRepository<View_ProdDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}