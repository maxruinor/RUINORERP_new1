
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/24/2025 17:01:20
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
    /// 
    /// </summary>
    public partial class tb_ProcessNavigationServices : BaseServices<tb_ProcessNavigation>, Itb_ProcessNavigationServices
    {
        IMapper _mapper;
        public tb_ProcessNavigationServices(IMapper mapper, IBaseRepository<tb_ProcessNavigation> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}