
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
    /// 产品套装明细
    /// </summary>
    public partial class tb_ProdBundleDetailServices : BaseServices<tb_ProdBundleDetail>, Itb_ProdBundleDetailServices
    {
        IMapper _mapper;
        public tb_ProdBundleDetailServices(IMapper mapper, IBaseRepository<tb_ProdBundleDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}