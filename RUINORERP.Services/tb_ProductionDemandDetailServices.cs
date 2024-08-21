
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/28/2024 11:55:46
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
    /// 生产需求分析表明细
    /// </summary>
    public partial class tb_ProductionDemandDetailServices : BaseServices<tb_ProductionDemandDetail>, Itb_ProductionDemandDetailServices
    {
        IMapper _mapper;
        public tb_ProductionDemandDetailServices(IMapper mapper, IBaseRepository<tb_ProductionDemandDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}