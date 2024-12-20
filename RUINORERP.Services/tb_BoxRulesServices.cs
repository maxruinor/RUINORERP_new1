
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/19/2024 15:52:34
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
    /// 箱规表
    /// </summary>
    public partial class tb_BoxRulesServices : BaseServices<tb_BoxRules>, Itb_BoxRulesServices
    {
        IMapper _mapper;
        public tb_BoxRulesServices(IMapper mapper, IBaseRepository<tb_BoxRules> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}