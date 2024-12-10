
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/10/2024 13:12:13
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
    /// 目标客户-公海客户CRM系统中使用，给成交客户作外键引用
    /// </summary>
    public partial class tb_CRM_CustomerServices : BaseServices<tb_CRM_Customer>, Itb_CRM_CustomerServices
    {
        IMapper _mapper;
        public tb_CRM_CustomerServices(IMapper mapper, IBaseRepository<tb_CRM_Customer> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}