
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/22/2024 16:08:33
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
    /// 打印模板
    /// </summary>
    public partial class tb_PrintTemplateServices : BaseServices<tb_PrintTemplate>, Itb_PrintTemplateServices
    {
        IMapper _mapper;
        public tb_PrintTemplateServices(IMapper mapper, IBaseRepository<tb_PrintTemplate> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}