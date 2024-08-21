
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:49
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
    /// 系统使用者公司
    /// </summary>
    public partial class tb_CompanyServices : BaseServices<tb_Company>, Itb_CompanyServices
    {
        IMapper _mapper;
        public tb_CompanyServices(IMapper mapper, IBaseRepository<tb_Company> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}