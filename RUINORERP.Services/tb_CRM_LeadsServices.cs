
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/11/2024 20:44:24
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
    /// 线索机会-询盘
    /// </summary>
    public partial class tb_CRM_LeadsServices : BaseServices<tb_CRM_Leads>, Itb_CRM_LeadsServices
    {
        IMapper _mapper;
        public tb_CRM_LeadsServices(IMapper mapper, IBaseRepository<tb_CRM_Leads> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}