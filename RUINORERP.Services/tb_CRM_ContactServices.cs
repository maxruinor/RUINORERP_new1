
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:42
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
    /// 联系人表-爱好跟进
    /// </summary>
    public partial class tb_CRM_ContactServices : BaseServices<tb_CRM_Contact>, Itb_CRM_ContactServices
    {
        IMapper _mapper;
        public tb_CRM_ContactServices(IMapper mapper, IBaseRepository<tb_CRM_Contact> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}