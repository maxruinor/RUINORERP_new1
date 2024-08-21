
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:49:02
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
    /// 销售出库退回单
    /// </summary>
    public partial class tb_SaleOutReServices : BaseServices<tb_SaleOutRe>, Itb_SaleOutReServices
    {
        IMapper _mapper;
        public tb_SaleOutReServices(IMapper mapper, IBaseRepository<tb_SaleOutRe> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}