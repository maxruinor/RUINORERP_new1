
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/10/2024 13:36:35
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
    /// 客户关系配置表
    /// </summary>
    public partial class tb_CRMConfigServices : BaseServices<tb_CRMConfig>, Itb_CRMConfigServices
    {
        IMapper _mapper;
        public tb_CRMConfigServices(IMapper mapper, IBaseRepository<tb_CRMConfig> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}