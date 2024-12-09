
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 15:51:26
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
    /// 销售分区表-大中华区
    /// </summary>
    public partial class tb_CRM_RegionServices : BaseServices<tb_CRM_Region>, Itb_CRM_RegionServices
    {
        IMapper _mapper;
        public tb_CRM_RegionServices(IMapper mapper, IBaseRepository<tb_CRM_Region> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}