
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/25/2024 20:07:12
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
    /// 标准物料表BOM明细-要适当冗余
    /// </summary>
    public partial class tb_BOM_SDetailServices : BaseServices<tb_BOM_SDetail>, Itb_BOM_SDetailServices
    {
        IMapper _mapper;
        public tb_BOM_SDetailServices(IMapper mapper, IBaseRepository<tb_BOM_SDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}