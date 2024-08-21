
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/07/2024 19:06:31
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
    /// 产品套装表
    /// </summary>
    public partial class tb_ProdBundleServices : BaseServices<tb_ProdBundle>, Itb_ProdBundleServices
    {
        IMapper _mapper;
        public tb_ProdBundleServices(IMapper mapper, IBaseRepository<tb_ProdBundle> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}