
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/13/2023 17:33:37
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
    public partial class tb_ContactsServices : BaseServices<tb_Contacts>, Itb_ContactsServices
    {
        IMapper _mapper;
        public tb_ContactsServices(IMapper mapper, IBaseRepository<tb_Contacts> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}