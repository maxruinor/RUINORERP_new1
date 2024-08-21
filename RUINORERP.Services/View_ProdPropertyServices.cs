
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/26/2023 23:42:48
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
    /// 产品属性关系
    /// </summary>
    public partial class View_ProdPropertyServices : BaseServices<View_ProdProperty>, IView_ProdPropertyServices
    {
        IMapper _mapper;
        public View_ProdPropertyServices(IMapper mapper, IBaseRepository<View_ProdProperty> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}