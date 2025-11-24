
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/24/2025 17:01:21
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
    public partial class tb_ProcessNavigationNodeServices : BaseServices<tb_ProcessNavigationNode>, Itb_ProcessNavigationNodeServices
    {
        IMapper _mapper;
        public tb_ProcessNavigationNodeServices(IMapper mapper, IBaseRepository<tb_ProcessNavigationNode> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}