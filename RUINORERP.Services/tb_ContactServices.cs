
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:30
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
    /// 联系人表，CRM系统中使用
    /// </summary>
    public partial class tb_ContactServices : BaseServices<tb_Contact>, Itb_ContactServices
    {
        IMapper _mapper;
        public tb_ContactServices(IMapper mapper, IBaseRepository<tb_Contact> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}